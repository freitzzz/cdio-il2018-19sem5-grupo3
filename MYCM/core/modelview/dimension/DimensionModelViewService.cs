using core.domain;
using core.services;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.modelview.dimension
{
    /// <summary>
    /// Service for creating model views based on certain dimension contexts
    /// </summary>
    public static class DimensionModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when a null AddDimensionModelView is provided.
        /// </summary>
        private const string ERROR_NULL_DIMENSION_VIEW = "No dimension view provided.";

        /// <summary>
        /// Constant representing the error message presented when a null Collection of AddDimensionModelView is provided.
        /// </summary>
        private const string ERROR_NULL_DIMENSION_VIEW_COLLECTION = "Invalid dimension view collection provided.";
        
        /// <summary>
        /// Constant representing the error message presented when a null Dimension is provided.
        /// </summary>
        private const string ERROR_NULL_DIMENSION = "No dimension provided.";

        /// <summary>
        /// Constant representing the error message presented when a null Collection of Dimension is provided.
        /// </summary>
        private const string ERROR_NULL_DIMENSION_COLLECTION = "Invalid dimension collection provided.";

        /// <summary>
        /// Constant representing the error message presented when an unknown type of Dimension is provided for conversion.
        /// </summary>
        private const string ERROR_NO_IMPLEMENTATION_UNKNOWN_DIMENSION = "No conversion implementation for unknown type of dimension.";

        /// <summary>
        /// Constant representing the error message presented when an unknown type of AddDimensionModelView is provided for conversion.
        /// </summary>
        private const string ERROR_NO_IMPLEMENTATION_UNKNOWN_VIEW = "No conversion implementation for unknown type of dimension view.";

        /// <summary>
        /// Creates a model view with a dimension information
        /// </summary>
        /// <param name="dimension">Dimension with the dimension being created the model view</param>
        /// <returns>GetDimensionModelView with the dimension information model view</returns>
        /// <exception cref="System.ArgumentNullException">Throw when the provided Dimension is null.</exception>
        public static GetDimensionModelView fromEntity(Dimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(ERROR_NULL_DIMENSION);
            }

            Type dimensionType = dimension.GetType();

            if (dimensionType == typeof(SingleValueDimension))
            {
                SingleValueDimension singleValueDimension = (SingleValueDimension)dimension;
                GetSingleValueDimensionModelView singleMV = new GetSingleValueDimensionModelView();
                singleMV.id = singleValueDimension.Id;
                singleMV.unit = MeasurementUnitService.getMinimumUnit();
                singleMV.value = singleValueDimension.value;

                return singleMV;
            }
            else if (dimensionType == typeof(DiscreteDimensionInterval))
            {
                DiscreteDimensionInterval discreteDimension = (DiscreteDimensionInterval)dimension;
                GetDiscreteDimensionIntervalModelView discreteMV = new GetDiscreteDimensionIntervalModelView();
                discreteMV.id = discreteDimension.Id;
                discreteMV.unit = MeasurementUnitService.getMinimumUnit();

                List<double> values = new List<double>();
                foreach (double value in discreteDimension.values)
                {
                    values.Add(value);
                }
                discreteMV.values = values;

                return discreteMV;
            }
            else if (dimensionType == typeof(ContinuousDimensionInterval))
            {
                ContinuousDimensionInterval continuousDimension = (ContinuousDimensionInterval)dimension;
                GetContinuousDimensionIntervalModelView continuousMV = new GetContinuousDimensionIntervalModelView();
                continuousMV.id = continuousDimension.Id;
                continuousMV.unit = MeasurementUnitService.getMinimumUnit();
                continuousMV.minValue = continuousDimension.minValue;
                continuousMV.maxValue = continuousDimension.maxValue;
                continuousMV.increment = continuousDimension.increment;

                return continuousMV;
            }
            else
            {
                throw new NotImplementedException(ERROR_NO_IMPLEMENTATION_UNKNOWN_DIMENSION);
            }

        }

        /// <summary>
        /// Creates a model view with a dimension information
        /// </summary>
        /// <param name="dimension">Dimension with the dimension being created the model view</param>
        /// <param name="unit">Unit to which the values will be converted</param>
        /// <returns>GetDimensionModelView with the dimension information model view</returns>
        /// <exception cref="System.ArgumentNullException">Throw when the provided Dimension is null.</exception>
        public static GetDimensionModelView fromEntity(Dimension dimension, string unit)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(ERROR_NULL_DIMENSION);
            }
            //if no unit is provided, use the default implementation
            if (Strings.isNullOrEmpty(unit))
            {
                return fromEntity(dimension);
            }

            Type dimensionType = dimension.GetType();

            if (dimensionType == typeof(SingleValueDimension))
            {
                SingleValueDimension singleValueDimension = (SingleValueDimension)dimension;
                GetSingleValueDimensionModelView singleMV = new GetSingleValueDimensionModelView();
                singleMV.id = singleValueDimension.Id;
                singleMV.unit = unit;
                singleMV.value = MeasurementUnitService.convertToUnit(singleValueDimension.value, unit);

                return singleMV;
            }
            else if (dimensionType == typeof(DiscreteDimensionInterval))
            {
                DiscreteDimensionInterval discreteDimension = (DiscreteDimensionInterval)dimension;
                GetDiscreteDimensionIntervalModelView discreteMV = new GetDiscreteDimensionIntervalModelView();
                discreteMV.id = discreteDimension.Id;
                discreteMV.unit = unit;

                List<double> values = new List<double>();
                foreach (double value in discreteDimension.values)
                {
                    values.Add(MeasurementUnitService.convertToUnit(value, unit));
                }
                discreteMV.values = values;

                return discreteMV;
            }
            else if (dimensionType == typeof(ContinuousDimensionInterval))
            {
                ContinuousDimensionInterval continuousDimension = (ContinuousDimensionInterval)dimension;
                GetContinuousDimensionIntervalModelView continuousMV = new GetContinuousDimensionIntervalModelView();
                continuousMV.id = continuousDimension.Id;
                continuousMV.unit = unit;
                continuousMV.minValue = MeasurementUnitService.convertToUnit(continuousDimension.minValue, unit);
                continuousMV.maxValue = MeasurementUnitService.convertToUnit(continuousDimension.maxValue, unit);
                continuousMV.increment = MeasurementUnitService.convertToUnit(continuousDimension.increment, unit);

                return continuousMV;
            }
            else
            {
                throw new NotImplementedException(ERROR_NO_IMPLEMENTATION_UNKNOWN_DIMENSION);
            }
        }

        /// <summary>
        /// Creates a model view with the information about a collection of dimension
        /// </summary>
        /// <param name="dimensions">IEnumerable with the collection of dimensions</param>
        /// <returns>GetAllDimensionsModelView with the collection of dimensions model view</returns>
        /// <exception cref="System.ArgumentNullException">Throw when the provided IEnumerable of Dimension is null.</exception>
        public static GetAllDimensionsModelView fromCollection(IEnumerable<Dimension> dimensions)
        {
            if(dimensions == null){
                throw new ArgumentException(ERROR_NULL_DIMENSION_COLLECTION);
            }

            GetAllDimensionsModelView allDimensionsModelView = new GetAllDimensionsModelView();
            foreach (Dimension dimension in dimensions) allDimensionsModelView.Add(fromEntity(dimension));
            return allDimensionsModelView;
        }

        /// <summary>
        /// Creates a model view with the information about a collection of dimension
        /// </summary>
        /// <param name="dimensions">IEnumerable with the collection of dimensions</param>
        /// <param name="unit">Unit to which the values will be converted</param>
        /// <returns>GetAllDimensionsModelView with the collection of dimensions model view</returns>
        /// <exception cref="System.ArgumentNullException">Throw when the provided IEnumerable of Dimension is null.</exception>
        public static GetAllDimensionsModelView fromCollection(IEnumerable<Dimension> dimensions, string unit)
        {
            if(dimensions == null){
                throw new ArgumentNullException(ERROR_NULL_DIMENSION_COLLECTION);
            }
            //if the provided unit string is null or empty, resort to the default implementation
            if(Strings.isNullOrEmpty(unit)){
                return fromCollection(dimensions);
            }

            GetAllDimensionsModelView allDimensionsModelView = new GetAllDimensionsModelView();
            foreach (Dimension dimension in dimensions) allDimensionsModelView.Add(fromEntity(dimension, unit));
            return allDimensionsModelView;
        }

        /// <summary>
        /// Creates an instance of Dimension from an AddDImensionModelView instance.
        /// </summary>
        /// <param name="addDimensionModelView">AddDimensionModelView instance.</param>
        /// <returns>Created Dimension.</returns>
        /// <exception cref="System.ArgumentNullException">Thtown when the provided AddDimensionModelView is null.</exception>
        public static Dimension fromModelView(AddDimensionModelView addDimensionModelView)
        {
            if (addDimensionModelView == null)
            {
                throw new ArgumentNullException(ERROR_NULL_DIMENSION_VIEW);
            }

            string unit = addDimensionModelView.unit;

            Type modelViewType = addDimensionModelView.GetType();

            if (modelViewType == typeof(AddSingleValueDimensionModelView))
            {
                AddSingleValueDimensionModelView singleValueMV = (AddSingleValueDimensionModelView)addDimensionModelView;

                double dimensionValue = MeasurementUnitService.convertFromUnit(singleValueMV.value, unit);

                return new SingleValueDimension(dimensionValue);
            }
            else if (modelViewType == typeof(AddDiscreteDimensionIntervalModelView))
            {
                AddDiscreteDimensionIntervalModelView discreteMV = (AddDiscreteDimensionIntervalModelView)addDimensionModelView;

                List<double> dimensionValues = new List<double>();

                foreach (double value in discreteMV.values)
                {
                    double dimensionValue = MeasurementUnitService.convertFromUnit(value, unit);
                    dimensionValues.Add(dimensionValue);
                }

                return new DiscreteDimensionInterval(dimensionValues);
            }
            else if (modelViewType == typeof(AddContinuousDimensionIntervalModelView))
            {
                AddContinuousDimensionIntervalModelView continuousMV = (AddContinuousDimensionIntervalModelView)addDimensionModelView;

                double minValue = MeasurementUnitService.convertFromUnit(continuousMV.minValue, unit);
                double maxValue = MeasurementUnitService.convertFromUnit(continuousMV.maxValue, unit);
                double increment = MeasurementUnitService.convertFromUnit(continuousMV.increment, unit);

                return new ContinuousDimensionInterval(minValue, maxValue, increment);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates an IEnumerable of Dimension from an IEnumerabel of AddDimensionModelView.
        /// </summary>
        /// <param name="addDimensionModelViews">AddDimensionModelView IEnumerable-</param>
        /// <returns>IEnumerable of Dimension.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable is null.</exception>
        public static IEnumerable<Dimension> fromModelViews(IEnumerable<AddDimensionModelView> addDimensionModelViews)
        {
            if (addDimensionModelViews == null)
            {
                throw new ArgumentNullException(ERROR_NULL_DIMENSION_VIEW_COLLECTION);
            }

            List<Dimension> dimensions = new List<Dimension>();

            foreach (AddDimensionModelView dimensionModelView in addDimensionModelViews)
            {
                Dimension dimension = fromModelView(dimensionModelView);

                dimensions.Add(dimension);
            }

            return dimensions;
        }
    }
}