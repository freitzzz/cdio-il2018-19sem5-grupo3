using System;
using System.Collections.Generic;
using support.domain.ddd;
using support.utils;
using core.dto;
using System.Linq;

namespace core.domain
{
    /// <summary>
    /// Class that represents a discrete dimension interval
    /// </summary>
    public class DiscreteDimensionInterval : Dimension, ValueObject
    {

        /// <summary>
        /// Constant that represents the message that occurs if the list is null
        /// </summary>
        private const string NULL_LIST_REFERENCE = "List of values can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the min value is NaN
        /// </summary>
        private const string EMPTY_LIST_REFERENCE = "List of values can't be empty";

        /// <summary>
        /// List of values that make up the interval.
        /// </summary>
        //*Since EF Core 2.1 does not support collections of primitive types, a wrapper ValueObject class must be used */
        public virtual List<DoubleValue> values { get; set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected DiscreteDimensionInterval() { }

        /// <summary>
        /// Returns a new DiscreteDimensionInterval instance
        /// </summary>
        /// <param name="values">list of values that make up the interval</param>
        /// <returns>DiscreteDimensionInterval instance</returns>
        public static DiscreteDimensionInterval valueOf(List<double> values)
        {
            return new DiscreteDimensionInterval(values);
        }

        /// <summary>
        /// Builds a DiscreteDimensionInterval instance
        /// </summary>
        /// <param name="values">list of values that make up the interval</param>
        private DiscreteDimensionInterval(List<double> values)
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
                doubleValues.Add(value);
            }

            this.values = doubleValues;
        }

        /// <summary>
        /// Equals method of DiscreteDimensionInterval
        /// Two objects are the same if they have the same list of values
        /// </summary>
        /// <param name="obj">object that is being compared to</param>
        /// <returns>true if the objects are the same, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
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

                foreach(DoubleValue value in values){
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

            foreach (DoubleValue doubleValue in values)
            {
                dto.values.Add(doubleValue);
            }

            return dto;
        }
    }
}