using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.component;
using core.modelview.customizeddimensions;
using core.modelview.dimension;
using core.modelview.measurement;
using core.modelview.product;
using core.modelview.productmaterial;
using core.modelview.productslotwidths;
using core.persistence;
using core.services.ensurance;
using support.utils;

namespace core.services
{
    /// <summary>
    /// Service used for creating a new instance of Product.
    /// </summary>
    public static class CreateProductService
    {
        /// <summary>
        /// Constant representing the error message presented when no ProductCategory is found.
        /// </summary>
        private const string ERROR_CATEGORY_NOT_FOUND = "No category was found with the matching identifier.";

        /// <summary>
        /// Constant representing the error message presented when the ProductCategory is not a leaf.
        /// </summary>
        private const string ERROR_CATEGORY_NOT_LEAF = "The provided category is not a leaf.";

        /// <summary>
        /// Constant representing the error message presented when no Material was provided.
        /// </summary>
        private const string ERROR_NO_MATERIALS_DEFINED = "No materials were provided, please provide a material.";

        /// <summary>
        /// Constant representing the error message presented when a null AddProductMaterialModelView is in the list of provided AddProductMaterialModelView.
        /// </summary>
        private const string ERROR_NULL_MATERIAL = "An invalid material was provided, please make sure all materials are valid.";

        /// <summary>
        /// Constant representing the error message presented when a null AddComponentModelView is in the list of provided AddComponentModelView.
        /// </summary>
        private const string ERROR_NULL_COMPONENT = "An invalid component was provided, please make sure all components are valid.";

        /// <summary>
        /// Constant representing the error message presented when no Material was found.
        /// </summary>
        private const string ERROR_MATERIAL_NOT_FOUND = "No material was found with the identifier of {0}.";

        /// <summary>
        /// Constant representing the error message presented when no Product was found.
        /// </summary>
        private const string ERROR_PRODUCT_NOT_FOUND = "No product was found with the identifier of {0}.";

        /// <summary>
        /// Constant representing the error message presented when no Measurement was provided.
        /// </summary>
        private const string ERROR_NO_MEASUREMENTS_DEFINED = "No dimensions were provided, please provide dimensions.";


        //TODO: use ProductBuilder here

        /// <summary>
        /// Creates a new instance of Product and saves it to the Repository.
        /// </summary>
        /// <param name="addProductMV">AddProductModelView containing the new Product's information.</param>
        /// <returns>Created instance of Product.</returns>
        /// <exception cref="System.ArgumentException">Throw </exception>
        public static Product create(AddProductModelView addProductMV)
        {
            string reference = addProductMV.reference;
            string designation = addProductMV.designation;
            string modelFilename = addProductMV.modelFilename;
            long productCategoryId = addProductMV.categoryId;

            List<AddMeasurementModelView> measurementModelViews = addProductMV.measurements;

            //NOTE: these checks are made here in order to avoid making requests to repositories unnecessarily
            if (measurementModelViews == null || !measurementModelViews.Any())
            {
                throw new ArgumentException(ERROR_NO_MEASUREMENTS_DEFINED);
            }

            List<AddProductMaterialModelView> materialViews = addProductMV.materials;

            //NOTE: these checks are made here in order to avoid making requests to repositories unnecessarily
            if (materialViews == null || !materialViews.Any())
            {
                throw new ArgumentException(ERROR_NO_MATERIALS_DEFINED);
            }

            ProductCategoryRepository categoryRepository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory category = categoryRepository.find(productCategoryId);

            if (category == null)
            {
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND);
            }

            if (!categoryRepository.isLeaf(category))
            {
                throw new ArgumentException(ERROR_CATEGORY_NOT_LEAF);
            }

            List<Material> materials = new List<Material>();
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();

            foreach (AddProductMaterialModelView materialModelView in materialViews)
            {
                if(materialModelView == null){
                    throw new ArgumentException(ERROR_NULL_MATERIAL);
                }

                long materialId = materialModelView.materialId;

                Material material = materialRepository.find(materialId);
                if (material == null)
                {
                    throw new ArgumentException(string.Format(ERROR_MATERIAL_NOT_FOUND, materialId));
                }
                materials.Add(material);
            }

            IEnumerable<Measurement> measurements = MeasurementModelViewService.fromModelViews(addProductMV.measurements);

            List<AddComponentModelView> componentModelViews = addProductMV.components;
            AddProductSlotWidthsModelView slotWidthsModelView = addProductMV.slotWidths;

            bool hasComponents = componentModelViews != null && componentModelViews.Any();
            bool hasSlots = slotWidthsModelView != null;

            Product product = null;

            if (hasSlots)
            {
                ProductSlotWidths slotWidths = ProductSlotWidthsModelViewService.fromModelView(slotWidthsModelView);
                if (hasComponents)
                {
                    product = new Product(reference, designation, modelFilename, category, materials, measurements, slotWidths);
                    return addComplementaryProducts(product, componentModelViews);
                }
                else
                {
                    return new Product(reference, designation, modelFilename, category, materials, measurements, slotWidths);
                }
            }
            else
            {
                if (hasComponents)
                {
                    product = new Product(reference, designation, modelFilename, category, materials, measurements);
                    return addComplementaryProducts(product, componentModelViews);
                }
                else
                {
                    return new Product(reference, designation, modelFilename, category, materials, measurements);
                }
            }
        }

        /// <summary>
        /// Adds complementary Products to an instance of Product.
        /// </summary>
        /// <param name="product">Product to which the complementary Products will be added.</exception>
        /// <param name="componentModelViews">ModelViews containing component information.</param>
        /// <returns>Product updated with a list of complementary Products.</returns>
        /// <exception cref="System.ArgumentException">Thrown when any complementary Product could not be found.</exception>
        private static Product addComplementaryProducts(Product product, IEnumerable<AddComponentModelView> componentModelViews)
        {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();

            foreach (AddComponentModelView addComponentToProductModelView in componentModelViews)
            {
                if(addComponentToProductModelView == null){
                    throw new ArgumentException(ERROR_NULL_COMPONENT);
                }

                Product complementaryProduct = productRepository.find(addComponentToProductModelView.childProductId);

                if (complementaryProduct == null)
                {
                    throw new ArgumentException(string.Format(ERROR_PRODUCT_NOT_FOUND, addComponentToProductModelView.childProductId));
                }

                if (addComponentToProductModelView.mandatory)
                {
                    product.addMandatoryComplementaryProduct(complementaryProduct);
                }
                else
                {
                    product.addComplementaryProduct(complementaryProduct);
                }
            }

            return product;
        }
    }
}