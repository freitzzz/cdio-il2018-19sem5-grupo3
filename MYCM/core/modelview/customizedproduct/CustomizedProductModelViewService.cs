using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;
using core.modelview.product;
using core.modelview.slot;
using core.services;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the service for converting instances of CustomizedProduct into ModelViews.
    /// </summary>
    public static class CustomizedProductModelViewService
    {
        /// <summary>
        /// Constant that represents the message presented when the provided instance of CustomizedProduct is null.
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_PRODUCT = "The provided customized product is invalid.";

        /// <summary>
        /// Constant that represents the message presented when the provided IEnumerable of CustomizedProduct is null.
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_PRODUCT_COLLECTION = "The provided customized product collection is invalid.";

        /// <summary>
        /// Converts an instance of CustomizedProduct into an instance of GetBasicCustomizedProductModelView.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct being converted.</param>
        /// <returns>Instance of GetBasicCustomizedProductModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of CustomizedProduct is null.</exception>
        public static GetBasicCustomizedProductModelView fromEntityAsBasic(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }

            GetBasicCustomizedProductModelView basicModelView = new GetBasicCustomizedProductModelView();

            basicModelView.customizedProductId = customizedProduct.Id;
            basicModelView.productId = customizedProduct.product.Id;
            basicModelView.designation = customizedProduct.designation;
            basicModelView.reference = customizedProduct.reference;

            return basicModelView;
        }


        /// <summary>
        /// Converts an instance of CustomizedProduct into an instance of GetCustomizedProductModelView.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct being converted.</param>
        /// <returns>Instance of GetCustomizedProductModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of CustomizedProduct is null.</exception>
        public static GetCustomizedProductModelView fromEntity(CustomizedProduct customizedProduct)
        {
            return fromEntity(customizedProduct, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an instance of CustomizedProduct into an instance of GetCustomizedProductModelView.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct being converted.</param>
        /// <param name="unit">String representing the unit to which the CustomizedProduct's dimensions will be converted to.</param>
        /// <returns>Instance of GetCustomizedProductModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of CustomizedProduct is null.</exception>
        public static GetCustomizedProductModelView fromEntity(CustomizedProduct customizedProduct, string unit)
        {
            if (customizedProduct == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }

            GetCustomizedProductModelView customizedProductModelView = new GetCustomizedProductModelView();
            customizedProductModelView.customizedProductId = customizedProduct.Id;
            customizedProductModelView.reference = customizedProduct.reference;
            customizedProductModelView.designation = customizedProduct.designation;
            customizedProductModelView.status = customizedProduct.status;
            customizedProductModelView.customizedDimensions = CustomizedDimensionsModelViewService.fromEntity(customizedProduct.customizedDimensions, unit);

            if (customizedProduct.customizedMaterial != null)
            {
                customizedProductModelView.customizedMaterial = CustomizedMaterialModelViewService.fromEntity(customizedProduct.customizedMaterial);
            }

            customizedProductModelView.product = ProductModelViewService.fromEntityAsBasic(customizedProduct.product);
            customizedProductModelView.slots = SlotModelViewService.fromCollection(customizedProduct.slots, unit);


            return customizedProductModelView;
        }


        /// <summary>
        /// Converts an IEnumerable of CustomizedProduct into an instance of GetAllCustomizedProductsModelView.
        /// </summary>
        /// <param name="customizedProducts">IEnumerable containing the CustomizedProducts being converted.</param>
        /// <returns>Instance of GetAllCustomizedProductsModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided IEnumerable of CustomizedProduct is null.</exception>
        public static GetAllCustomizedProductsModelView fromCollection(IEnumerable<CustomizedProduct> customizedProducts)
        {
            if (customizedProducts == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_PRODUCT_COLLECTION);
            }

            GetAllCustomizedProductsModelView allCustomizedProductsModelView = new GetAllCustomizedProductsModelView();

            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                allCustomizedProductsModelView.Add(fromEntityAsBasic(customizedProduct));
            }

            return allCustomizedProductsModelView;
        }
    }
}