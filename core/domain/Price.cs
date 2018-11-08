using System;
using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Represents the monetary value of something (e.g. a material, or a finish)
    /// </summary>
    public class Price : ValueObject
    {

        /// <summary>
        /// Constant that represents the error message that occurs if the price's value isn't a number
        /// </summary>
        private const string VALUE_NAN = "The price's value has to be a number";

        /// <summary>
        /// Constant that represents the error message that occurs if the price's value is negative
        /// </summary>
        private const string NEGATIVE_VALUE = "The price's value can't be negative";

        /// <summary>
        /// Constant that represents the error message that occurs if the price's value is infinity
        /// </summary>
        private const string VALUE_IS_INFINITY = "The price's value can't be infinity";

        /// <summary>
        /// Monetary value of the price in € per squared meter
        /// </summary>
        /// <value>Gets/Sets the value</value>
        public double value { get; internal set; }

        /// <summary>
        /// Creates a new price with a given value
        /// </summary>
        /// <param name="value">price's monetary value</param>
        /// <returns>Price instance</returns>
        public static Price valueOf(double value)
        {
            return new Price(value);
        }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected Price() { }

        /// <summary>
        /// Builds a Price instance with a given value
        /// </summary>
        /// <param name="value">monetary value of the price</param>
        private Price(double value)
        {
            checkMonetaryValue(value);
            this.value = value;
        }

        /// <summary>
        /// Checks if the monetary value of a price is valid
        /// </summary>
        /// <param name="value">price's monetary value</param>
        private void checkMonetaryValue(double value)
        {
            if (Double.IsNaN(value))
            {
                throw new ArgumentException(VALUE_NAN);
            }

            if (Double.IsNegative(value))
            {
                throw new ArgumentException(NEGATIVE_VALUE);
            }

            if (Double.IsInfinity(value))
            {
                throw new ArgumentException(VALUE_IS_INFINITY);
            }
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            Price other = (Price)obj;

            return Double.Equals(this.value, other.value);
        }

        public override string ToString()
        {
            return String.Format("Price:{0}", value);
        }
    }
}