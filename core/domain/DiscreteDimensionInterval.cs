using System;
using System.Collections.Generic;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Class that represents a discrete dimension interval
    /// </summary>
    public class DiscreteDimensionInterval : Restriction, ValueObject
    {
        /// <summary>
        /// Constant that represents the message that occurs if the list is null
        /// </summary>
        private static readonly string NULL_LIST_REFERENCE = "List of values can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the min value is NaN
        /// </summary>
        private static readonly string EMPTY_LIST_REFERENCE = "List of values can't be empty";

        /// <summary>
        /// List of values that make up the interval.
        /// </summary>
        private List<Double> values;

        /// <summary>
        /// Returns a new DiscreteDimensionInterval instance
        /// </summary>
        /// <param name="values">list of values that make up the interval</param>
        /// <returns>DiscreteDimensionInterval instance</returns>
        public static DiscreteDimensionInterval valueOf(List<Double> values)
        {
            return new DiscreteDimensionInterval(values);
        }

        /// <summary>
        /// Builds a DiscreteDimensionInterval instance
        /// </summary>
        /// <param name="values">list of values that make up the interval</param>
        private DiscreteDimensionInterval(List<Double> values)
        {

            if (Collections.isListNull(values))
            {
                throw new ArgumentException(NULL_LIST_REFERENCE);
            }

            if (Collections.isListEmpty(values))
            {
                throw new ArgumentException(EMPTY_LIST_REFERENCE);
            }

            this.values = values;
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

            DiscreteDimensionInterval other = (DiscreteDimensionInterval)obj;

            return this.values.Equals(other.values);
        }

        /// <summary>
        /// Hash code of DiscreteDimensionInterval
        /// </summary>
        /// <returns>hash code of a DiscreteDimensionInterval instance</returns>
        public override int GetHashCode()
        {
            return values.GetHashCode();
        }

        /// <summary>
        /// ToString of DiscreteDimensionInterval
        /// </summary>
        /// <returns>list of values of the DiscreteDimensionInterval instance</returns>
        public override string ToString()
        {
            return string.Format("List of values:\n{0}", values);
        }
    }
}