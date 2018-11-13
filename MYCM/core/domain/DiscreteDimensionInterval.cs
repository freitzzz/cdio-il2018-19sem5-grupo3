using System;
using System.Collections.Generic;
using support.domain.ddd;
using support.utils;
using core.dto;
using System.Linq;
using core.services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.domain
{
    /// <summary>
    /// Class that represents a discrete dimension interval
    /// </summary>
    public class DiscreteDimensionInterval : Dimension
    {

        /// <summary>
        /// Constant that represents the message that occurs if the list is null
        /// </summary>
        private const string NULL_LIST_REFERENCE = "List of values can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the list is empty
        /// </summary>
        private const string EMPTY_LIST_REFERENCE = "List of values can't be empty";

        /// <summary>
        /// Constant that represents the message that occurs if a value is NaN
        /// </summary>
        private const string VALUE_NAN = "All values must be a number";

        /// <summary>
        /// Constant that represents the message that occurs if a value is infinity
        /// </summary>
        private const string VALUE_INFINITY = "No value can be infinity";

        /// <summary>
        /// Constant that represents the message that occurs if a zero or negative value exists in the list
        /// </summary>
        private const string NEGATIVE_OR_ZERO_VALUE = "All values have to be greater than zero";

        /// <summary>
        /// List of values that make up the interval.
        /// </summary>
        //*Since EF Core 2.1 does not support collections of primitive types, a wrapper ValueObject class must be used */
        private List<DoubleValue> _values;  //!private field used for lazy loading, do not use this for storing or fetching data
        public List<DoubleValue> values { get => LazyLoader.Load(this, ref _values); set => _values = value; }

        private DiscreteDimensionInterval(ILazyLoader lazyLoader) : base(lazyLoader) { }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected DiscreteDimensionInterval() { }

        /// <summary>
        /// Builds a DiscreteDimensionInterval instance
        /// </summary>
        /// <param name="values">list of values that make up the interval</param>
        public DiscreteDimensionInterval(List<double> values)
        {

            if (Collections.isListNull(values))
            {
                throw new ArgumentException(NULL_LIST_REFERENCE);
            }

            if (Collections.isListEmpty(values))
            {
                throw new ArgumentException(EMPTY_LIST_REFERENCE);
            }

            List<DoubleValue> doubleValues = new List<DoubleValue>();
            foreach (double value in values)
            {
                checkValue(value);
                doubleValues.Add(value);
            }

            this.values = doubleValues;
        }

        /// <summary>
        /// Checks if a value for the interval is valid
        /// </summary>
        /// <param name="value">value being checked</param>
        private void checkValue(double value)
        {
            if (Double.IsNaN(value))
            {
                throw new ArgumentException(VALUE_NAN);
            }

            if (Double.IsInfinity(value))
            {
                throw new ArgumentException(VALUE_INFINITY);
            }

            if (Double.IsNegative(value) || value == 0)
            {
                throw new ArgumentException(NEGATIVE_OR_ZERO_VALUE);
            }
        }

        public override bool hasValue(double value)
        {
            bool matchingValueFound = false;

            foreach (double dimensionValue in values)
            {

                decimal dimensionValueAsDecimal = (decimal)dimensionValue;
                decimal valueAsDecimal = (decimal)value;

                if (decimal.Compare(dimensionValueAsDecimal, valueAsDecimal) == 0)
                {
                    matchingValueFound = true;
                    break;
                }
            }

            return matchingValueFound;
        }

        /// <summary>
        /// Equals method of DiscreteDimensionInterval
        /// Two objects are the same if they have the same list of values
        /// </summary>
        /// <param name="obj">object that is being compared to</param>
        /// <returns>true if the objects are the same, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().ToString().Equals("core.domain.DiscreteDimensionInterval"))
            {
                return false;
            }

            if (this == obj)
            {
                return true;
            }

            DiscreteDimensionInterval other = (DiscreteDimensionInterval)obj;
            //!Equals() on lists tests reference equality
            //if order does not matter then use All()
            return this.values.SequenceEqual(other.values);
        }

        /// <summary>
        /// Hash code of DiscreteDimensionInterval
        /// </summary>
        /// <returns>hash code of a DiscreteDimensionInterval instance</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;

                foreach (DoubleValue value in values)
                {
                    hash = hash * 31 + value.GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// ToString of DiscreteDimensionInterval
        /// </summary>
        /// <returns>list of values of the DiscreteDimensionInterval instance</returns>
        public override string ToString()
        {
            return string.Format("List of values:\n{0}", values);
        }

        /// <summary>
        /// Builds a DimensionDTO out of a DiscreteDimensionInterval instance
        /// </summary>
        /// <returns>DimensionDTO instance</returns>
        public override DimensionDTO toDTO()
        {
            DiscreteDimensionIntervalDTO dto = new DiscreteDimensionIntervalDTO();

            dto.id = Id;
            dto.values = new List<double>();
            dto.unit = MeasurementUnitService.getMinimumUnit();

            foreach (DoubleValue doubleValue in values)
            {
                dto.values.Add(doubleValue);
            }

            return dto;
        }

        public override DimensionDTO toDTO(string unit)
        {
            if (unit == null)
            {
                return this.toDTO();
            }
            DiscreteDimensionIntervalDTO dto = new DiscreteDimensionIntervalDTO();

            dto.id = Id;
            dto.values = new List<double>();

            foreach (DoubleValue doubleValue in values)
            {
                dto.values.Add(MeasurementUnitService.convertToUnit(doubleValue.value, unit));
            }
            dto.unit = unit;

            return dto;
        }
    }
}