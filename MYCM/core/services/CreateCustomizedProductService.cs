using System;
using System.Linq;
using core.domain;
using core.dto;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct;
using core.modelview.material;
using core.persistence;
using static core.domain.CustomizedProduct;

namespace core.services
{
    /// <summary>
    /// Class representing the service responsible for creating instances of CustomizedProduct.
    /// </summary>
    public static class CreateCustomizedProductService
    {
        /// <summary>
        /// Constant representing the error message presented when the Product is not found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_PRODUCT = "Unable to find a product with an identifier of: {0}";
        /// <summary>
        /// Constant representing the error message presented when the Material is not found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_MATERIAL = "Unable to find a material with an identifier of: {0}";
        /// <summary>
        /// Constant representing the error message presented when the Slot is not found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_SLOT = "Unable to find a slot with an identifier of: {0}";

        /// <summary>
        /// Constant representing the error message presented when the CustomizedProduct could not be saved.
        /// </summary>
        private const string ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT = "Unable to save the customized product. Please, make sure the reference/serial number is unique";

        /// <summary>
        /// Creates an instance of CustomizedProduct.
        /// </summary>
        /// <param name="addCustomizedProductModelView">AddCustomizedProductModelView containing </param>
        /// <returns>Created and persisted instance of CustomizedProduct.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the Product is not found, when the parent CustomizedProduct or Slot are not found and when the CustomizedProduct could not be saved.
        /// </exception>
        public static CustomizedProduct create(AddCustomizedProductModelView addCustomizedProductModelView)
        {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();

            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            Product product = productRepository.find(addCustomizedProductModelView.productId);

            if (product == null)
            {
                throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT, addCustomizedProductModelView.productId));
            }

            CustomizedProduct customizedProduct = buildCustomizedProduct(addCustomizedProductModelView, product);

            if (addCustomizedProductModelView.insertedInSlotId.HasValue && addCustomizedProductModelView.parentCustomizedProductId.HasValue)
            {
                CustomizedProduct parentCustomizedProduct = customizedProductRepository.find(addCustomizedProductModelView.parentCustomizedProductId.Value);

                if (parentCustomizedProduct == null)
                {
                    throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT, addCustomizedProductModelView.parentCustomizedProductId.Value));
                }

                Slot slot = parentCustomizedProduct.slots.Where(s => s.Id == addCustomizedProductModelView.insertedInSlotId.Value).SingleOrDefault();

                if (slot == null)
                {
                    throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_SLOT, addCustomizedProductModelView.insertedInSlotId.Value));
                }

                customizedProduct = customizedProductRepository.save(customizedProduct);

                if (customizedProduct == null)
                {
                    throw new ArgumentException(ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT);
                }

                parentCustomizedProduct.addCustomizedProduct(customizedProduct, slot);
                customizedProductRepository.update(parentCustomizedProduct);
            }
            else
            {
                customizedProduct = customizedProductRepository.save(customizedProduct);

                if (customizedProduct == null)
                {
                    throw new ArgumentException(ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT);
                }
            }

            return customizedProduct;
        }

        /// <summary>
        /// Builds an instance of CustomizedProduct based on the given Product with the data in the given instance of AddCustomizedProductModelView.
        /// </summary>
        /// <param name="addCustomizedProductModelView">AddCustomizedProductModelView containing the CustomizedProduct's information.</param>
        /// <param name="product">Instance of Product.</param>
        /// <returns>Built instance of CustomizedProduct.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the Material referenced by the CustomizedMaterial is not found.</exception>
        private static CustomizedProduct buildCustomizedProduct(AddCustomizedProductModelView addCustomizedProductModelView, Product product)
        {
            CustomizedProductBuilder customizedProductBuilder = null;

            CustomizedDimensions customizedProductDimensions = CustomizedDimensionsModelViewService.fromModelView(addCustomizedProductModelView.customizedDimensions);

            if (addCustomizedProductModelView.reference == null && addCustomizedProductModelView.userAuthToken == null)
            {
                CustomizedProductSerialNumberRepository serialNumberRepository = PersistenceContext.repositories().createCustomizedProductSerialNumberRepository();

                string serialNumber = serialNumberRepository.findSerialNumber().serialNumber;

                customizedProductBuilder = CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, product, customizedProductDimensions);

                serialNumberRepository.increment();
            }
            else if (addCustomizedProductModelView.reference == null && addCustomizedProductModelView.userAuthToken != null)
            {
                CustomizedProductSerialNumberRepository serialNumberRepository = PersistenceContext.repositories().createCustomizedProductSerialNumberRepository();

                string serialNumber = serialNumberRepository.findSerialNumber().serialNumber;

                customizedProductBuilder = CustomizedProductBuilder
                    .createRegisteredUserCustomizedProduct(serialNumber, addCustomizedProductModelView.userAuthToken,
                         product, customizedProductDimensions);

                serialNumberRepository.increment();
            }
            else if (addCustomizedProductModelView.reference != null && addCustomizedProductModelView.userAuthToken != null)
            {
                customizedProductBuilder = CustomizedProductBuilder
                    .createManagerCustomizedProduct(addCustomizedProductModelView.reference, addCustomizedProductModelView.userAuthToken,
                        product, customizedProductDimensions);
            }

            //build customized product with optional properties if they're defined
            if (addCustomizedProductModelView.customizedMaterial != null)
            {
                MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();

                Material material = materialRepository.find(addCustomizedProductModelView.customizedMaterial.materialId);

                if (material == null)
                {
                    throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL, addCustomizedProductModelView.customizedMaterial.materialId));
                }
                //TODO: replace usage of dto
                FinishDTO finishDTO = addCustomizedProductModelView.customizedMaterial.finish;
                ColorDTO colorDTO = addCustomizedProductModelView.customizedMaterial.color;

                CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, colorDTO.toEntity(), finishDTO.toEntity());

                customizedProductBuilder.withMaterial(customizedMaterial);
            }

            if (addCustomizedProductModelView.designation != null)
            {
                customizedProductBuilder.withDesignation(addCustomizedProductModelView.designation);
            }

            return customizedProductBuilder.build();
        }
    }
}