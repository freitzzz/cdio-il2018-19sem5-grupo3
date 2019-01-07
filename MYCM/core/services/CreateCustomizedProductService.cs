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
        /// Constant representing the error message presented when the Slot is not found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_SLOT = "Unable to find a slot with an identifier of: {0}";

        /// <summary>
        /// Constant representing the error message presented when the CustomizedProduct could not be saved.
        /// </summary>
        private const string ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT = "Unable to save the customized product. Please, make sure the reference is unique";

        /// <summary>
        /// Constant representing the error message presented when a CustomizedProduct is attempted to be created without dimensions.
        /// </summary>
        private const string ERROR_NO_CUSTOMIZED_DIMENSIONS = "Unable to create a customized product without dimensions.";

        /// <summary>
        /// Constant representing the message presented when a CustomizedProduct is attempted to be created for a 
        /// </summary>
        private const string ERROR_CUSTOMIZING_COMPONENT_WITHOUT_PARENT = "Unable to customize this product without adding it to another product.";

        //TODO: Change reference based on user's role

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

            CustomizedProduct customizedProduct = null;

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

                customizedProduct = buildSubCustomizedProduct(addCustomizedProductModelView, product, parentCustomizedProduct, slot);

                customizedProductRepository.update(parentCustomizedProduct);
            }
            else
            {
                //this occurs if, for example, a drawer is attempted to be customized without adding it to a closet
                if (!productRepository.isBaseProduct(product.Id))
                {
                    throw new ArgumentException(ERROR_CUSTOMIZING_COMPONENT_WITHOUT_PARENT);
                }

                customizedProduct = buildCustomizedProduct(addCustomizedProductModelView, product);
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
        /// <exception cref="System.ArgumentException">
        /// Thrown when the Material referenced by the CustomizedMaterial is not found or when no CustomizedDimensions are provided.
        /// </exception>
        /// <returns>A new instance of CustomizedProduct.</returns>
        private static CustomizedProduct buildCustomizedProduct(AddCustomizedProductModelView addCustomizedProductModelView, Product product)
        {
            CustomizedProductBuilder customizedProductBuilder = null;

            if (addCustomizedProductModelView.customizedDimensions == null)
            {
                throw new ArgumentException(ERROR_NO_CUSTOMIZED_DIMENSIONS);
            }

            CustomizedDimensions customizedProductDimensions = CustomizedDimensionsModelViewService.fromModelView(addCustomizedProductModelView.customizedDimensions);

            if (addCustomizedProductModelView.userAuthToken == null)
            {
                customizedProductBuilder = CustomizedProductBuilder.createCustomizedProduct(addCustomizedProductModelView.reference, product, customizedProductDimensions);
            }
            else
            {
                customizedProductBuilder = CustomizedProductBuilder
                    .createCustomizedProduct(addCustomizedProductModelView.userAuthToken, addCustomizedProductModelView.reference, product, customizedProductDimensions);
            }

            //build customized product with optional properties if they're defined
            if (addCustomizedProductModelView.customizedMaterial != null)
            {
                CustomizedMaterial customizedMaterial = CreateCustomizedMaterialService.create(addCustomizedProductModelView.customizedMaterial);

                customizedProductBuilder.withMaterial(customizedMaterial);
            }

            if (addCustomizedProductModelView.designation != null)
            {
                customizedProductBuilder.withDesignation(addCustomizedProductModelView.designation);
            }

            return customizedProductBuilder.build();
        }


        /// <summary>
        /// Builds an instance of CustomizedProduct based on the given Product with the data in the given instance of AddCustomizedProductModelView. 
        /// </summary>
        /// <param name="addCustomizedProductModelView">AddCustomizedProductModelView containing the CustomizedProduct's information.</param>
        /// <param name="product">Instance of Product.</param>
        /// <param name="parentCustomizedProduct">Parent CustomizedProduct.</param>
        /// <param name="insertedInSlot">Slot in which the CustomizedProduct will be inserted.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the Material referenced by the CustomizedMaterial is not found or when no CustomizedDimensions are provided.
        /// </exception>
        /// <returns>A new instance of CustomizedProduct.</returns>
        private static CustomizedProduct buildSubCustomizedProduct(AddCustomizedProductModelView addCustomizedProductModelView, Product product, 
            CustomizedProduct parentCustomizedProduct, Slot insertedInSlot)
        {
            CustomizedProductBuilder customizedProductBuilder = null;

            if (addCustomizedProductModelView.customizedDimensions == null)
            {
                throw new ArgumentException(ERROR_NO_CUSTOMIZED_DIMENSIONS);
            }

            CustomizedDimensions customizedProductDimensions = CustomizedDimensionsModelViewService.fromModelView(addCustomizedProductModelView.customizedDimensions);

            if (addCustomizedProductModelView.userAuthToken == null)
            {
                customizedProductBuilder = CustomizedProductBuilder.createCustomizedProduct(product, customizedProductDimensions, parentCustomizedProduct, insertedInSlot);
            }
            else
            {
                customizedProductBuilder = CustomizedProductBuilder
                    .createCustomizedProduct(addCustomizedProductModelView.userAuthToken, product, customizedProductDimensions, parentCustomizedProduct, insertedInSlot);
            }

            if (addCustomizedProductModelView.customizedMaterial != null)
            {
                CustomizedMaterial customizedMaterial = CreateCustomizedMaterialService.create(addCustomizedProductModelView.customizedMaterial);

                customizedProductBuilder.withMaterial(customizedMaterial);
            }

            //ignore designation since only the base customized products can set the designation

            return customizedProductBuilder.build();
        }
    }
}