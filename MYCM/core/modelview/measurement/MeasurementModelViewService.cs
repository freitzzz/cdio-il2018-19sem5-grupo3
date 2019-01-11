using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.dimension;
using core.modelview.product;
using core.services;
using support.utils;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing a service used for converting Measurement's ModelViews into instances of Measurement and vice-versa.
    /// </summary>
    public static class MeasurementModelViewService
    {
        /// <summary>
        /// Constant representing the error message being presented when the provided AddMeasuremntModelView is null.
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT_VIEW = "No measurement view provided.";

        /// <summary>
        /// Constant representing the error message being presented when a null Collection of AddMeasurementModelView is provided.
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT_VIEW_COLLECTION = "Invalid measurement view collection provided";

        /// <summary>
        /// Constant representing the error message being presented when the provided Measurement is null.
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT = "The provided Measurement is null.";

        /// <summary>
        /// Constant representing the error message being presente when the provided Collection of Measurement is null. 
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT_COLLECTION = "Invalid Measurement collection";

        /// <summary>
        /// Converts an instance of Measurement into an instance of GetMeasurementModelView.
        /// </summary>
        /// <param name="measurement">Measurement being converted.</param>
        /// <returns>An instance of GetMeasurementModelView with the Measurement data.</returns>
        /// <exception cref="System.ArgumentNullException">If the provided Measurement is null.</exception>
        public static GetMeasurementModelView fromEntity(Measurement measurement)
        {
            return fromEntity(measurement, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an instance of Measurement into an instance of GetMeasurementModelView 
        /// with the values converted to the specified unit.
        /// </summary>
        /// <param name="measurement">Measurement being converted.</param>
        /// <param name="unit">Unit to which the values will be converted.</param>
        /// <returns>An instance of GetMeasurementModelView with the Measurement data.</returns>
        /// <exception cref="System.ArgumentNullException">If the provided Measurement is null.</exception>
        public static GetMeasurementModelView fromEntity(Measurement measurement, string unit)
        {
            if (measurement == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MEASUREMENT);
            }

            //if no unit is provided, resort to the default implementation
            if (Strings.isNullOrEmpty(unit))
            {
                return fromEntity(measurement);
            }

            GetDimensionModelView heightMV = DimensionModelViewService.fromEntity(measurement.height, unit);
            GetDimensionModelView widthMV = DimensionModelViewService.fromEntity(measurement.width, unit);
            GetDimensionModelView depthMV = DimensionModelViewService.fromEntity(measurement.depth, unit);

            GetMeasurementModelView measurementModelView = new GetMeasurementModelView();
            measurementModelView.measurementId = measurement.Id;
            measurementModelView.height = heightMV;
            measurementModelView.width = widthMV;
            measurementModelView.depth = depthMV;

            return measurementModelView;
        }

        /// <summary>
        /// Creates an IEnumerable of GetMeasurementModelView from a given IEnumerable of Measurement.
        /// </summary>
        /// <param name="measurements">IEnumerable of Measurement being converted.</param>
        /// <returns>An instance of GetAllMeasurementsModelView. </returns>
        /// <exception cref="System.ArgumentNullException">If the IEnumerable is null or 
        /// any instance of Measurement in the IEnumerable is null.</exception>
        public static GetAllMeasurementsModelView fromCollection(IEnumerable<Measurement> measurements)
        {
            if (measurements == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MEASUREMENT_COLLECTION);
            }

            GetAllMeasurementsModelView measurementModelViews = new GetAllMeasurementsModelView();

            foreach (Measurement measurement in measurements)
            {
                measurementModelViews.Add(fromEntity(measurement));
            }

            return measurementModelViews;
        }

        /// <summary>
        /// Creates an IEnumerable of GetMeasurementModelView from a given IEnumerable of Measurement
        ///  with the values converted to a given unit.
        /// </summary>
        /// <param name="measurements">IEnumerable of Measurement being converted.</param>
        /// <param name="unit">Unit to which the values will be converted.</param>
        /// <returns>An instance of GetAllMeasurementsModelView.</returns>
        /// <exception cref="System.ArgumentNullException">If the IEnumerable is null or 
        /// any instance of Measurement in the IEnumerable is null.</exception>
        public static GetAllMeasurementsModelView fromCollection(IEnumerable<Measurement> measurements, string unit)
        {
            if (measurements == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MEASUREMENT_COLLECTION);
            }

            GetAllMeasurementsModelView measurementModelViews = new GetAllMeasurementsModelView();

            if (Strings.isNullOrEmpty(unit))
            {
                return fromCollection(measurements);
            }

            foreach (Measurement measurement in measurements)
            {
                measurementModelViews.Add(fromEntity(measurement, unit));
            }

            return measurementModelViews;
        }

        /// <summary>
        /// Creates an instance of Measurement from an AddMeasurementModelView instance.
        /// </summary>
        /// <param name="measurementModelView">AddMeasurementModelView instance.</param>
        /// <returns>Created Measurement.</returns>
        public static Measurement fromModelView(AddMeasurementModelView measurementModelView)
        {
            if (measurementModelView == null)
            {
                throw new ArgumentException(ERROR_NULL_MEASUREMENT_VIEW);
            }

            Dimension heightDimension = DimensionModelViewService.fromModelView(measurementModelView.heightDimension);
            Dimension widthDimension = DimensionModelViewService.fromModelView(measurementModelView.widthDimension);
            Dimension depthDimension = DimensionModelViewService.fromModelView(measurementModelView.depthDimension);

            return new Measurement(heightDimension, widthDimension, depthDimension);
        }

        /// <summary>
        /// Creates an IEnumerable of Measurement from an IEnumerable of AddMeasurementModelView.
        /// </summary>
        /// <param name="measurementModelViews">AddMeasurementModelView IEnumerable.</param>
        /// <returns>An IEnumerable of Measurement.</returns>
        public static IEnumerable<Measurement> fromModelViews(IEnumerable<AddMeasurementModelView> measurementModelViews)
        {
            if (measurementModelViews == null)
            {
                throw new ArgumentNullException(ERROR_NULL_MEASUREMENT_VIEW_COLLECTION);
            }

            List<Measurement> measurements = new List<Measurement>();

            foreach (AddMeasurementModelView measurementMV in measurementModelViews)
            {
                measurements.Add(fromModelView(measurementMV));
            }

            return measurements;
        }
    }
}