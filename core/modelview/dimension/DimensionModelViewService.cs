using core.domain;
using core.services;
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
        /// Constant representing the error message presented when a null Dimension is provided.
        /// </summary>
        private const string ERROR_NULL_DIMENSION = "No dimension provided.";

        //TODO: Add unit conversion

        /// <summary>
        /// Creates a model view with a dimension information
        /// </summary>
        /// <param name="dimension">Dimension with the dimension being created the model view</param>
        /// <returns>GetDimensionModelView with the dimension information model view</returns>
        public static GetDimensionModelView fromEntity(Dimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentException(ERROR_NULL_DIMENSION);
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
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Creates a model view with the information about a collection of dimension
        /// </summary>
        /// <param name="dimensions">IEnumerable with the collection of dimensions</param>
        /// <returns>GetAllDimensionsModelView with the collection of dimensions model view</returns>
        public static GetAllDimensionsModelView fromCollection(IEnumerable<Dimension> dimensions)
        {
            GetAllDimensionsModelView allDimensionsModelView = new GetAllDimensionsModelView();
            foreach (Dimension dimension in dimensions) allDimensionsModelView.Add(fromEntity(dimension));
            return allDimensionsModelView;
        }


        /// <summary>
        /// Creates an instance of Dimension from an AddDImensionModelView instance.
        /// </summary>
        /// <param name="addDimensionModelView">AddDimensionModelView instance.</param>
        /// <returns>Created Dimension.</returns>
        public static Dimension fromModelView(AddDimensionModelView addDimensionModelView)
        {
            if (addDimensionModelView == null)
            {
                throw new ArgumentException(ERROR_NULL_DIMENSION_VIEW);
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
        public static IEnumerable<Dimension> fromModelViews(IEnumerable<AddDimensionModelView> addDimensionModelViews)
        {
            List<Dimension> dimensions = new List<Dimension>();

            if (addDimensionModelViews == null)
            {
                return dimensions;
            }

            foreach (AddDimensionModelView dimensionModelView in addDimensionModelViews)
            {
                Dimension dimension = fromModelView(dimensionModelView);

                dimensions.Add(dimension);
            }

            return dimensions;
        }
    }
}