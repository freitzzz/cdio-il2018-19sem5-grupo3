using System;
using core.dto;
using core.services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Numerics;

namespace core.domain {
    /// <summary>
    /// Class that represents a continuous dimension interval
    /// </summary>
    public class ContinuousDimensionInterval : Dimension {
        /// <summary>
        /// Constant that represents the message that occurs if the min value is NaN
        /// </summary>
        private const string MIN_VALUE_NAN_REFERENCE = "Minimum value has to be a number";

        /// <summary>
        /// Constant that represents the message that occurs if the max value is NaN
        /// </summary>
        private const string MAX_VALUE_NAN_REFERENCE = "Maximum value has to be a number";

        /// <summary>
        /// Constant that represents the message that occurs if the increment value is NaN
        /// </summary>
        private const string INCREMENT_NAN_REFERENCE = "Increment value has to be a number";

        /// <summary>
        /// Constant that represents the message that occurs if the min value is infinity
        /// </summary>
        private const string MIN_VALUE_INFINITY_REFERENCE = "Minimum value can't be infinity";

        /// <summary>
        /// Constant that represents the message that occurs if the max value is infinity
        /// </summary>
        private const string MAX_VALUE_INFINITY_REFERENCE = "Maximum value can't be infinity";

        /// <summary>
        /// Constant that represents the message that occurs if the increment value is infinity
        /// </summary>
        private const string INCREMENT_INFINITY_REFERENCE = "Increment value can't be infinity";

        /// <summary>
        /// Constant that represents the message that occurs if one of the values is negative
        /// </summary>
        private const string NEGATIVE_OR_ZERO_VALUES_REFERENCE = "All values have to be positive and greated than zero";

        /// <summary>
        /// Constant that represents the message that occurs if the min value is greater than the max value
        /// </summary>
        private const string MIN_VALUE_GREATER_THAN_MAX_REFERENCE = "Minimum value can't be greater than maximum value";

        /// <summary>
        /// Constant that represents the message that occurs if the increment value is greater than the
        /// difference between the max and min values
        /// </summary>
        private const string INCREMENT_GREATER_THAN_MAX_MIN_DIFFERENCE_REFERENCE = "Increment can't be greater than the difference between the max and min values";

        /// <summary>
        /// Constant that represents the message that occurs if the max value is not a multiple of the increment.
        /// </summary>
        private const string INTERVAL_NOT_MULTIPLE_OF_INCREMENT = "The maximum value is not a multiple of increment.";

        /// <summary>
        /// Minimum value of the interval
        /// </summary>
        public double minValue { get; protected set; }
        /// <summary>
        /// Maximum value of the interval
        /// </summary>
        public double maxValue { get; protected set; }
        /// <summary>
        /// Increment value of the interval
        /// </summary>
        public double increment { get; protected set; }

        private ContinuousDimensionInterval(ILazyLoader lazyLoader) : base(lazyLoader) { }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected ContinuousDimensionInterval() { }

        /// <summary>
        /// Builds a ContinuousDimensionInterval instance with a minimum value, a maximum value 
        /// and an increment value
        /// </summary>
        /// <param name="minValue">minimum value of the interval</param>
        /// <param name="maxValue">maximum value of the interval</param>
        /// <param name="increment">increment value of the interval</param>
        public ContinuousDimensionInterval(double minValue, double maxValue, double increment) {
            if (Double.IsNaN(minValue)) {
                throw new ArgumentException(MIN_VALUE_NAN_REFERENCE);
            }

            if (Double.IsNaN(maxValue)) {
                throw new ArgumentException(MAX_VALUE_NAN_REFERENCE);
            }

            if (Double.IsNaN(increment)) {
                throw new ArgumentException(INCREMENT_NAN_REFERENCE);
            }

            if (Double.IsInfinity(minValue)) {
                throw new ArgumentException(MIN_VALUE_INFINITY_REFERENCE);
            }

            if (Double.IsInfinity(maxValue)) {
                throw new ArgumentException(MAX_VALUE_INFINITY_REFERENCE);
            }

            if (Double.IsInfinity(increment)) {
                throw new ArgumentException(INCREMENT_INFINITY_REFERENCE);
            }

            if (minValue <= 0 || maxValue <= 0 || increment <= 0) {
                throw new ArgumentException(NEGATIVE_OR_ZERO_VALUES_REFERENCE);
            }

            if (minValue > maxValue) {
                throw new ArgumentException(MIN_VALUE_GREATER_THAN_MAX_REFERENCE);
            }

            if (increment > (maxValue - minValue)) {
                throw new ArgumentException(INCREMENT_GREATER_THAN_MAX_MIN_DIFFERENCE_REFERENCE);
            }

            checkIfIntervalIsMultipleOfIncrement(maxValue - minValue, increment);

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.increment = increment;
        }

        /// <summary>
        /// Method used in constructor used for validating if the maximum value is reachable with a given increment.
        /// </summary>
        /// <param name="maxValue">double representing the interval's maximum value.</param>
        /// <param name="increment">double representing the interval's increment value.</param>
        private void checkIfIntervalIsMultipleOfIncrement(double interval, double increment) {
            decimal intervalAsDecimal = (decimal)interval;
            decimal incrementAsDecimal = (decimal)increment;

            decimal remainder = intervalAsDecimal % incrementAsDecimal;

            if (decimal.Compare(decimal.Zero, remainder) != 0) {
                throw new ArgumentException(INTERVAL_NOT_MULTIPLE_OF_INCREMENT);
            }
        }

        public override bool hasValue(double value) {
            if (value < minValue || value > maxValue) {
                return false;
            }

            decimal valueAsDecimal = (decimal)value;
            decimal incrementAsDecimal = (decimal)increment;

            decimal remainder = valueAsDecimal % incrementAsDecimal;

            return decimal.Compare(remainder, 0) == 0;
        }

        //*These methods may seem redundant in instances of this particular class, but these belong to the abstract Dimension 

        public override double getMaxValue() {
            return maxValue;
        }

        public override double getMinValue() {
            return minValue;
        }

        public override double[] getValuesAsArray() {
            List<double> values = new List<double>();
            for (double i = minValue; i <= maxValue; i += increment) {
                values.Add(i);
            }
            return values.ToArray();
        }

        /// <summary>
        /// Equals method of ContinuousDimensionInterval 
        /// Two objects are the same if the min, max and increment values are equal
        /// </summary>
        /// <param name="obj">object that is being compared to</param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj) {
            if (obj == null || !obj.GetType().ToString().Equals("core.domain.ContinuousDimensionInterval")) {
                return false;
            }

            if (this == obj) {
                return true;
            }

            ContinuousDimensionInterval other = (ContinuousDimensionInterval)obj;

            if (!Double.Equals(this.minValue, other.minValue) || !Double.Equals(this.maxValue, other.maxValue)) {
                return false;
            }

            return Double.Equals(this.increment, other.increment);
        }

        /// <summary>
        /// HashCode of ContinuousDimensionInterval
        /// </summary>
        /// <returns>hash code of a ContinuousDimensionInterval instance</returns>
        public override int GetHashCode() {
            return minValue.GetHashCode() + maxValue.GetHashCode() + increment.GetHashCode();
        }

        /// <summary>
        /// ToString of ContinuousDimensionInterval
        /// </summary>
        /// <returns>minimum, maximum and increment values of the interval</returns>
        public override string ToString() {
            return string.Format("Minimum Value: {0}\nMaximum Value: {1}\nIncrement Value: {2}",
            minValue, maxValue, increment);
        }

        /// <summary>
        /// Builds a DimensionDTO out of a ContinuousDimensionInterval instance
        /// </summary>
        /// <returns>DimensionDTO instance</returns>
        public override DimensionDTO toDTO() {
            ContinuousDimensionIntervalDTO dto = new ContinuousDimensionIntervalDTO();

            dto.id = Id;
            dto.unit = MeasurementUnitService.getMinimumUnit();
            dto.minValue = minValue;
            dto.maxValue = maxValue;
            dto.increment = increment;

            return dto;
        }

        /// <summary>
        /// Builds a DimensionDTO out of a ContinuousDimensionInterval instance
        /// </summary>
        /// <param name="unit">Desired unit</param>
        /// <returns>DimensionDTO instance</returns>
        public override DimensionDTO toDTO(string unit) {

            if (unit == null) {
                return this.toDTO();
            }

            ContinuousDimensionIntervalDTO dto = new ContinuousDimensionIntervalDTO();

            dto.id = Id;
            dto.minValue = MeasurementUnitService.convertToUnit(minValue, unit);
            dto.maxValue = MeasurementUnitService.convertToUnit(maxValue, unit);
            dto.increment = MeasurementUnitService.convertToUnit(increment, unit);
            dto.unit = unit;

            return dto;
        }
    }
}