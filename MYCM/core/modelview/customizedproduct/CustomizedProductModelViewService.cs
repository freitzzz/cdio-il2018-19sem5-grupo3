using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;

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
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CustomizedProduct is null.</exception>
        public static GetBasicCustomizedProductModelView fromEntityAsBasic(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }

            GetBasicCustomizedProductModelView basicModelView = new GetBasicCustomizedProductModelView();

            basicModelView.customizedProductId = customizedProduct.Id;
            basicModelView.productId = customizedProduct.product.Id;
            basicModelView.designation = customizedProduct.designation;
            basicModelView.reference = customizedProduct.reference;
            basicModelView.serialNumber = customizedProduct.serialNumber;

            return basicModelView;
        }

        public static GetCustomizedProductModelView fromEntity(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }

            GetCustomizedProductModelView customizedProductModelView = new GetCustomizedProductModelView();
            customizedProductModelView.customizedProductId = customizedProduct.Id;
            customizedProductModelView.reference = customizedProduct.reference;
            customizedProductModelView.serialNumber = customizedProduct.serialNumber;
            customizedProductModelView.designation = customizedProduct.designation;
            customizedProductModelView.customizedDimensions = CustomizedDimensionsModelViewService.fromEntity(customizedProduct.customizedDimensions);

            if (customizedProduct.customizedMaterial == null)
            {
                customizedProductModelView.customizedMaterial = CustomizedMaterialModelViewService.fromEntity(customizedProduct.customizedMaterial);
            }

            //customizedProductModelView.slots = 

            return customizedProductModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of CustomizedProduct into an instance of GetAllCustomizedProductsModelView.
        /// </summary>
        /// <param name="customizedProducts">IEnumerable containing the CustomizedProducts being converted.</param>
        /// <returns>Instance of GetAllCustomizedProductsModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of CustomizedProduct is null.</exception>
        public static GetAllCustomizedProductsModelView fromCollection(IEnumerable<CustomizedProduct> customizedProducts)
        {
            if (customizedProducts == null)
            {
                throw new ArgumentNullException(nameof(customizedProducts));
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