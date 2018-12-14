using System;
using core.domain;
using core.services;
using support.utils;

namespace core.modelview.customizeddimensions
{
    /// <summary>
    /// Class representing a service used for converting CustomizedDimensions into ModelViews and vice-versa.
    /// </summary>
    public static class CustomizedDimensionsModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when a null AddCustomizedDimensionsModelView is provided.
        /// </summary>
        private const string ERROR_NULL_MODEL_VIEW = "No dimensions view provided.";

        /// <summary>
        /// Constant representing the error message presented when a null CustomizedDimensions is provided.
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_DIMENSIONS = "Invalid dimensions.";

        /// <summary>
        /// Converts an instance of AddCustomizedDimensionsModelView into an instance of CustomizedDimensions.
        /// </summary>
        /// <param name="modelView">Instance of AddCustomizedDimensionsModelView.</param>
        /// <returns>The created instance of CustomizedDimensions</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided AddCustomizedDimensionsModelView is null.</exception>
        public static CustomizedDimensions fromModelView(AddCustomizedDimensionsModelView modelView)
        {
            if (modelView == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MODEL_VIEW);
            }

            double height = MeasurementUnitService.convertFromUnit(modelView.height, modelView.unit);
            double width = MeasurementUnitService.convertFromUnit(modelView.width, modelView.unit);
            double depth = MeasurementUnitService.convertFromUnit(modelView.depth, modelView.unit);

            return CustomizedDimensions.valueOf(height, width, depth);
        }

        /// <summary>
        /// Converts an instance of CustomizedDimensions into an instance of GetCustomizedModelView.
        /// </summary>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions.</param>
        /// <returns>The created instance of GetCustomizedDimensionsModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided CustomizedDimensions is null.</exception>
        public static GetCustomizedDimensionsModelView fromEntity(CustomizedDimensions customizedDimensions)
        {
            return fromEntity(customizedDimensions, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an instance of CustomizedDimensions into an instance of GetCustomizedModelView.
        /// </summary>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions.</param>
        /// <param name="unit">Unit to which the values will be converted.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided CustomizedDimensions is null.</exception>
        public static GetCustomizedDimensionsModelView fromEntity(CustomizedDimensions customizedDimensions, string unit)
        {
            if (customizedDimensions == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CUSTOMIZED_DIMENSIONS);
            }
            //if no unit is provided resort to the default implementation
            if (Strings.isNullOrEmpty(unit))
            {
                return fromEntity(customizedDimensions);
            }
            GetCustomizedDimensionsModelView modelView = new GetCustomizedDimensionsModelView();
            modelView.customizedDimensionsId = customizedDimensions.Id;
            modelView.unit = unit;
            modelView.height = MeasurementUnitService.convertToUnit(customizedDimensions.height, unit);
            modelView.width = MeasurementUnitService.convertToUnit(customizedDimensions.width, unit);
            modelView.depth = MeasurementUnitService.convertToUnit(customizedDimensions.depth, unit);

            return modelView;
        }
    }
}