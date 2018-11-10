using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.dimension;
using core.modelview.measurement;
using core.modelview.product;
using core.modelview.slotdimensions;
using core.persistence;
using core.services.ensurance;

namespace core.services
{
    /// <summary>
    /// Service used for creating a new i
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
        /// 
        /// </summary>
        /// <param name="addProductMV"></param>
        /// <returns></returns>
        public static Product create(AddProductModelView addProductMV)
        {
            string reference = addProductMV.reference;
            string designation = addProductMV.designation;
            long productCategoryId = addProductMV.categoryId;

            List<AddMeasurementModelView> measurementModelViews = addProductMV.measurements;

            if (measurementModelViews == null || !measurementModelViews.Any())
            {
                throw new ArgumentException(ERROR_NO_MEASUREMENTS_DEFINED);
            }

            List<AddMaterialToProductModelView> materialViews = addProductMV.materials;

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

            List<long> materialIds = addProductMV.materials.Select(m => m.materialID).ToList();
            List<Material> materials = new List<Material>();
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();

            foreach (long materialId in materialIds)
            {
                Material material = materialRepository.find(materialId);
                if (material == null)
                {
                    throw new ArgumentException(string.Format(ERROR_MATERIAL_NOT_FOUND, materialId));
                }
                materials.Add(material);
            }

            IEnumerable<Measurement> measurements = MeasurementModelViewService.fromModelViews(addProductMV.measurements);

            List<AddComponentToProductModelView> componentModelViews = addProductMV.components;
            AddSlotDimensionsModelView slotDimensionsModelView = addProductMV.slotSizes;

            bool hasComponents = componentModelViews != null && componentModelViews.Any();
            bool hasSlots = slotDimensionsModelView != null;

            if (hasSlots)
            {
                CustomizedDimensions minSize = CustomizedDimensionsModelViewService.fromModelView(addProductMV.slotSizes.minSize);
                CustomizedDimensions maxSize = CustomizedDimensionsModelViewService.fromModelView(addProductMV.slotSizes.maxSize);
                CustomizedDimensions recommendedSize = CustomizedDimensionsModelViewService.fromModelView(addProductMV.slotSizes.recommendedSize);
                if (hasComponents)
                {
                    return new Product(reference, designation, maxSize, minSize, recommendedSize, category, materials, getComplementaryProducts(componentModelViews), measurements);
                }
                else
                {
                    return new Product(reference, designation, maxSize, minSize, recommendedSize, category, materials, measurements);
                }
            }
            else
            {
                if (hasComponents)
                {
                    return new Product(reference, designation, category, materials, getComplementaryProducts(componentModelViews), measurements);
                }
                else
                {
                    return new Product(reference, designation, category, materials, measurements);
                }
            }
        }

        /// <summary>
        /// Creates a List containg instances of Product, which represent a Product's complements.
        /// </summary>
        /// <param name="componentModelViews">ModelViews containing component information.</param>
        /// <returns>List of Product.</returns>
        private static List<Product> getComplementaryProducts(List<AddComponentToProductModelView> componentModelViews)
        {
            List<Product> complementaryProducts = new List<Product>();
            List<long> componentIds = componentModelViews.Select(c => c.complementedProductID).ToList();

            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();

            foreach (long componentId in componentIds)
            {
                Product complementaryProduct = productRepository.find(componentId);

                if (complementaryProduct == null)
                {
                    throw new ArgumentException(string.Format(ERROR_PRODUCT_NOT_FOUND, componentId));
                }
                complementaryProducts.Add(complementaryProduct);
            }

            return complementaryProducts;
        }
    }
}