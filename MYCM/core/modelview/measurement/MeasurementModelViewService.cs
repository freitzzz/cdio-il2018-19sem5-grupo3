using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.dimension;
using core.modelview.product;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing a service used for converting Measurement's ModelViews into instances of Measurement and vice-versa.
    /// </summary>
    public static class MeasurementModelViewService
    {
        /// <summary>
        /// Constant representing the error message being presented when the provided ModelView is null.
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT_VIEW = "No measurement data provided.";

        /// <summary>
        /// Constant representing the error message being presented when the provided Measurement is null.
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT = "The provided Measurement is null.";

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
            List<Measurement> measurements = new List<Measurement>();

            if (measurementModelViews == null)
            {
                return measurements;
            }

            foreach (AddMeasurementModelView measurementMV in measurementModelViews)
            {
                measurements.Add(fromModelView(measurementMV));
            }

            return measurements;
        }

        /// <summary>
        /// Converts an instance of Measurement into an instance of GetMeasurementModelView.
        /// </summary>
        /// <param name="measurement">Measurement being converted.</param>
        /// <returns>An instance of GetMeasurementModelView with the Measurement data.</returns>
        /// <exception cref="System.ArgumentException">If the provided Measurement is null.</exception>
        public static GetMeasurementModelView fromEntity(Measurement measurement)
        {
            if (measurement == null)
            {
                throw new ArgumentException();
            }

            GetDimensionModelView heightMV = DimensionModelViewService.fromEntity(measurement.height);
            GetDimensionModelView widthMV = DimensionModelViewService.fromEntity(measurement.width);
            GetDimensionModelView depthMV = DimensionModelViewService.fromEntity(measurement.depth);

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
        /// <returns>An IEnumerable of GetMeasurementModelView </returns>
        /// <exception cref="System.ArgumentException">If any instance of Measurement in the IEnumerable is null.</exception>
        public static IEnumerable<GetMeasurementModelView> fromCollection(IEnumerable<Measurement> measurements)
        {
            List<GetMeasurementModelView> measurementModelViews = new List<GetMeasurementModelView>();

            if (measurements == null)
            {
                return measurementModelViews;
            }

            foreach (Measurement measurement in measurements)
            {
                measurementModelViews.Add(fromEntity(measurement));
            }

            return measurementModelViews;
        }
    }
}