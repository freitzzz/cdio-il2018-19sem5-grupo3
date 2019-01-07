using System;
using System.Collections.Generic;
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
        /// Constant rerpresenting the error message presented when a null IEnumerable of CustomizedDimensions is provided.
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_DIMENSIONS_COLLECTION = "Invalid dimensions collection.";

        /// <summary>
        /// Converts an instance of AddCustomizedDimensionsModelView into an instance of CustomizedDimensions.
        /// </summary>
        /// <param name="modelView">Instance of AddCustomizedDimensionsModelView.</param>
        /// <returns>The created instance of CustomizedDimensions</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided AddCustomizedDimensionsModelView is null.</exception>
        public static CustomizedDimensions fromModelView(AddCustomizedDimensionsModelView modelView)
        {
            if (modelView == null)
            {
                throw new ArgumentException(ERROR_NULL_MODEL_VIEW);
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
        /// <exception cref="System.ArgumentException">Thrown when the provided CustomizedDimensions is null.</exception>
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
        /// <exception cref="System.ArgumentException">Thrown when the provided CustomizedDimensions is null.</exception>
        public static GetCustomizedDimensionsModelView fromEntity(CustomizedDimensions customizedDimensions, string unit)
        {
            if (customizedDimensions == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_DIMENSIONS);
            }
            //if no unit is provided resort to the default implementation
            if (Strings.isNullOrEmpty(unit))
            {
                return fromEntity(customizedDimensions);
            }

            GetCustomizedDimensionsModelView modelView = new GetCustomizedDimensionsModelView();
            modelView.unit = unit;
            modelView.height = MeasurementUnitService.convertToUnit(customizedDimensions.height, unit);
            modelView.width = MeasurementUnitService.convertToUnit(customizedDimensions.width, unit);
            modelView.depth = MeasurementUnitService.convertToUnit(customizedDimensions.depth, unit);

            return modelView;
        }

        /// <summary>
        /// Converts an IEnumerable of CustomizedDimensions into an instance of GetAllCustomizedDimensionsModelView.
        /// </summary>
        /// <param name="customizedDimensions">IEnumerable of CustomizedDimensions.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided IEnumerable of CustomizedDimensions is null.</exception>
        /// <returns>Instance of GetAllCustomizedDimensionsModelView.</returns>
        public static GetAllCustomizedDimensionsModelView fromCollection(IEnumerable<CustomizedDimensions> customizedDimensions)
        {
            return fromCollection(customizedDimensions, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an IEnumerable of CustomizedDimensions into an instance of GetAllCustomizedDimensionsModelView.
        /// </summary>
        /// <param name="customizedDimensions">IEnumerable of CustomizedDimensions.</param>
        /// <param name="unit">String representing the unit to which the values will be converted.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided IEnumerable of CustomizedDimensions is null.</exception>
        /// <returns>Instance of GetAllCustomizedDimensionsModelView.</returns>
        public static GetAllCustomizedDimensionsModelView fromCollection(IEnumerable<CustomizedDimensions> customizedDimensions, string unit)
        {
            if (customizedDimensions == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_DIMENSIONS_COLLECTION);
            }

            GetAllCustomizedDimensionsModelView allCustomDimensionsMV = new GetAllCustomizedDimensionsModelView();
            foreach (CustomizedDimensions customizedDimension in customizedDimensions)
            {
                allCustomDimensionsMV.Add(fromEntity(customizedDimension, unit));
            }

            return allCustomDimensionsMV;
        }
    }
}