using System;
using core.domain;
using core.services;
using support.utils;

namespace core.modelview.productslotwidths
{
    /// <summary>
    /// Class representing a service used for converting ProductSlotWidths into ModelViews and vice-versa.
    /// </summary>
    public static class ProductSlotWidthsModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when a null AddProductSlotWidthsModelView is provided.
        /// </summary>
        private const string ERROR_NULL_MODEL_VIEW = "No product slot widths view provided.";
        /// <summary>
        /// Constant representing the error message presented when a null ProductSlotWidths is provided.
        /// </summary>
        private const string ERROR_NULL_PRODUCT_SLOT_WIDTHS = "No product slot widths provided.";

        /// <summary>
        /// Converts an instance of AddProductSlotWidthsModelView into an instance of ProductSlotWidths.
        /// </summary>
        /// <param name="modelView">Instance of AddProductSlotWidthsModelView.</param>
        /// <returns>The created instance of ProductSlotWidths.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of AddProductSlotWidthsModelView is null.</exception>
        public static ProductSlotWidths fromModelView(AddProductSlotWidthsModelView modelView)
        {
            if (modelView == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MODEL_VIEW);
            }

            double minWidth = MeasurementUnitService.convertFromUnit(modelView.minWidth, modelView.unit);

            double maxWidth = MeasurementUnitService.convertFromUnit(modelView.maxWidth, modelView.unit);

            double recommendedWidth = MeasurementUnitService.convertFromUnit(modelView.recommendedWidth, modelView.unit);

            return ProductSlotWidths.valueOf(minWidth, maxWidth, recommendedWidth);
        }


        /// <summary>
        /// Converts an instance of ProductSlotWidths into an instance of GetProductSlotWidthsModelView.
        /// </summary>
        /// <param name="productSlotWidths">Instance of ProductSlotWidths.</param>
        /// <returns>The created instance of GetProductSlotWidthsModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of ProductSlotWidths is null.</exception>
        public static GetProductSlotWidthsModelView fromEntity(ProductSlotWidths productSlotWidths)
        {
            if (productSlotWidths == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_SLOT_WIDTHS);
            }

            GetProductSlotWidthsModelView productSlotWidthsModelView = new GetProductSlotWidthsModelView();
            productSlotWidthsModelView.unit = MeasurementUnitService.getMinimumUnit();
            productSlotWidthsModelView.minWidth = productSlotWidths.minWidth;
            productSlotWidthsModelView.maxWidth = productSlotWidths.maxWidth;
            productSlotWidthsModelView.recommendedWidth = productSlotWidths.recommendedWidth;

            return productSlotWidthsModelView;
        }


        /// <summary>
        /// Converts an instance of ProductSlotWidths into an instance of GetProductSlotWidthsModelView.
        /// </summary>
        /// <param name="productSlotWidths">Instance of ProductSlotWidths.</param>
        /// <param name="unit">Unit to which the values will be converted.</exception>
        /// <returns>The created instance of GetProductSlotWidthsModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of ProductSlotWidths is null.</exception>
        public static GetProductSlotWidthsModelView fromEntity(ProductSlotWidths productSlotWidths, string unit)
        {
            if (productSlotWidths == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_SLOT_WIDTHS);
            }

            //if no unit is specified, resort to the default implementation
            if (Strings.isNullOrEmpty(unit))
            {
                return fromEntity(productSlotWidths);
            }
            GetProductSlotWidthsModelView productSlotWidthsModelView = new GetProductSlotWidthsModelView();
            productSlotWidthsModelView.unit = unit;
            productSlotWidthsModelView.minWidth = MeasurementUnitService.convertToUnit(productSlotWidths.minWidth, unit);
            productSlotWidthsModelView.maxWidth = MeasurementUnitService.convertToUnit(productSlotWidths.maxWidth, unit);
            productSlotWidthsModelView.recommendedWidth = MeasurementUnitService.convertToUnit(productSlotWidths.recommendedWidth, unit);

            return productSlotWidthsModelView;
        }
    }
}