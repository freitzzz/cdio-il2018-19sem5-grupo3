using core.domain;
using core.modelview.component;
using core.modelview.customizeddimensions;
using core.modelview.material;
using core.modelview.measurement;
using core.modelview.productcategory;
using System.Collections.Generic;
using System.Linq;
using System;
using core.services;
using core.modelview.productslotwidths;
using core.modelview.productmaterial;

namespace core.modelview.product
{
    /// <summary>
    /// Service for creating model views based on certain product contexts
    /// </summary>
    public static class ProductModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when the provided Product is null.
        /// </summary>
        private const string ERROR_NULL_PRODUCT = "The provided product is invalid.";

        /// <summary>
        /// Constant representing the error message presented when the provided Collection of Product is null.
        /// </summary>
        private const string ERROR_NULL_PRODUCT_COLLECTION = "The provided collection is invalid.";

        /// <summary>
        /// Creates a model view with a product basic information
        /// </summary>
        /// <param name="product">Product with the product being created the model view</param>
        /// <returns>GetBasicProductModelView with the product basic information model view</returns>
        public static GetBasicProductModelView fromEntityAsBasic(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT);
            }

            GetBasicProductModelView basicProductModelView = new GetBasicProductModelView();
            basicProductModelView.productId = product.Id;
            basicProductModelView.reference = product.reference;
            basicProductModelView.designation = product.designation;
            basicProductModelView.modelFilename = product.modelFilename;
            basicProductModelView.supportsSlots = product.supportsSlots;
            basicProductModelView.hasComponents = product.components.Any();
            return basicProductModelView;
        }

        /// <summary>
        /// Creates a model view with a product information.
        /// </summary>
        /// <param name="product">Product with the product being created the model view.</param>
        /// <returns>GetProductModelView with the product information model view</returns>
        public static GetProductModelView fromEntity(Product product)
        {
            return fromEntity(product, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Creates a model view with a product information.
        /// </summary>
        /// <param name="product">Product with the product being created the model view.</param>
        /// <param name="unit">Unit to which all the dimension data will be converted to.</param>
        /// <returns>GetProductModelView with the product information model view</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Product is null.</exception>
        public static GetProductModelView fromEntity(Product product, string unit)
        {
            if (product == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT);
            }

            GetProductModelView productModelView = new GetProductModelView();
            productModelView.productId = product.Id;
            productModelView.reference = product.reference;
            productModelView.designation = product.designation;
            productModelView.modelFilename = product.modelFilename;
            productModelView.category = ProductCategoryModelViewService.fromEntityAsBasic(product.productCategory);
            if (product.components.Any())
            {
                productModelView.components = ComponentModelViewService.fromCollection(product.components);
            }
            //no need to check if the product has materials and measurements, since they're mandatory
            productModelView.materials = ProductMaterialModelViewService.fromCollection(product.productMaterials);
            productModelView.measurements = MeasurementModelViewService.fromCollection(product.productMeasurements.Select(pm => pm.measurement), unit);
            if (product.supportsSlots)
            {
                productModelView.slotWidths = ProductSlotWidthsModelViewService.fromEntity(product.slotWidths, unit);
            }
            return productModelView;
        }

        /// <summary>
        /// Creates a model view with the information about a collection of products
        /// </summary>
        /// <param name="products">IEnumerable with the collection of products</param>
        /// <returns>GetAllProductsModelView with the collection of products model view</returns>
        public static GetAllProductsModelView fromCollection(IEnumerable<Product> products)
        {
            if (products == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_COLLECTION);
            }

            GetAllProductsModelView allProductsModelView = new GetAllProductsModelView();
            foreach (Product product in products) allProductsModelView.Add(fromEntityAsBasic(product));
            return allProductsModelView;
        }
    }
}