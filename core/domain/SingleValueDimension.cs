using System;
using support.domain.ddd;

namespace core.domain
{

    /// <summary>
    /// Class that represents a dimension and its value (e.g. width - 20 cm)
    /// </summary>
    public class SingleValueDimension : Dimension, ValueObject
    {

        public long Id {get; set;}

        /// <summary>
        /// Constant that represents the message that occurs if the value is NaN
        /// </summary>
        private const string VALUE_IS_NAN_REFERENCE = "Dimension value has to be a number";

        /// <summary>
        /// Constant that represents the message that occurs if the value is infinity
        /// </summary>
        private const string VALUE_IS_INFINITY_REFERENCE = "Dimension value can't be infinity";

        /// <summary>
        /// Constant that represents the message that occurs if the value is negative
        /// </summary>
        private const string NEGATIVE_VALUE_REFERENCE = "Dimension value can't be negative";

        /// <summary>
        /// Value that the dimension has
        /// </summary>
        public double value {get; set;}

        /// <summary>
        /// Returns a new instance of Dimension
        /// </summary>
        /// <param name="value">value that the dimension has</param>
        /// <returns>Dimension instance</returns>
        public static SingleValueDimension valueOf(double value)
        {
            return new SingleValueDimension(value);
        }

        /// <summary>
        /// Builds a new instance of Dimension
        /// </summary>
        /// <param name="value">value that the dimension has</param>
        private SingleValueDimension(double value)
        {
            if (Double.IsNaN(value))
            {
                throw new ArgumentException(VALUE_IS_NAN_REFERENCE);
            }

            if (Double.IsInfinity(value))
            {
                throw new ArgumentException(VALUE_IS_INFINITY_REFERENCE);
            }

            if (value < 0)
            {
                throw new ArgumentException(NEGATIVE_VALUE_REFERENCE);
            }

            this.value = value;
        }

        /// <summary>
        /// Equals method of Dimension
        /// Two instances are equal if they share the same value
        /// </summary>
        /// <param name="obj">object that is being compared</param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
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

            SingleValueDimension other = (SingleValueDimension)obj;

            return Double.Equals(this.value, other.value);
        }

        /// <summary>
        /// Hash code of Dimension
        /// </summary>
        /// <returns>hash code of a Dimension instance</returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// ToString method of Dimension
        /// </summary>
        /// <returns>value of the dimension</returns>
        public override string ToString()
        {
            return string.Format("Value: {0}", value);
        }
    }
}