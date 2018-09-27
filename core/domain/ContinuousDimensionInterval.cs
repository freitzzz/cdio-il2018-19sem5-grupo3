using System;
using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Class that represents a continuous dimension interval
    /// </summary>
    public class ContinuousDimensionInterval : Restriction, ValueObject
    {
        /// <summary>
        /// Minimum value of the interval
        /// </summary>
        private double minValue;
        /// <summary>
        /// Maximum value of the interval
        /// </summary>
        private double maxValue;
        /// <summary>
        /// Increment value of the interval
        /// </summary>
        private double increment;

        public static ContinuousDimensionInterval valueOf(double minValue, double maxValue, double increment)
        {
            return new ContinuousDimensionInterval(minValue, maxValue, increment);
        }


        /// <summary>
        /// Builds a ContinuousDimensionInterval instance with a minimum value, a maximum value 
        /// and an increment value
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="increment"></param>
        private ContinuousDimensionInterval(double minValue, double maxValue, double increment)
        {
            if (Double.IsNaN(minValue))
            {
                throw new ArgumentException("Minimum value has to be a number");
            }

            if (Double.IsNaN(maxValue))
            {
                throw new ArgumentException("Maximum value has to be a number");
            }

            if (Double.IsNaN(increment))
            {
                throw new ArgumentException("Increment value has to be a number");
            }

            if (Double.IsInfinity(minValue))
            {
                throw new ArgumentException("Minimum value can't be infinity");
            }

            if (Double.IsInfinity(maxValue))
            {
                throw new ArgumentException("Maximum value can't be infinity");
            }

            if (Double.IsInfinity(increment))
            {
                throw new ArgumentException("Increment can't be infinity");
            }

            if (minValue < 0 || maxValue < 0 || increment < 0)
            {
                throw new ArgumentException("All values have to be positive");
            }

            if (minValue > maxValue)
            {
                throw new ArgumentException("Minimum value can't be greater than maximum value");
            }

            if (increment > (maxValue - minValue))
            {
                throw new ArgumentException("Increment can't be greater than the difference between the " +
                "max and min value");
            }

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.increment = increment;
        }

        /// <summary>
        /// Equals method of ContinuousDimensionInterval.null 
        /// Two objects are the same if the min, max and increment values are equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ContinuousDimensionInterval other = (ContinuousDimensionInterval)obj;

            if (!Double.Equals(this.minValue, other.minValue) || !Double.Equals(this.maxValue, other.maxValue))
            {
                return false;
            }

            return Double.Equals(this.increment, other.increment);
        }

        /// <summary>
        /// HashCode of ContinuousDimensionInterval
        /// </summary>
        /// <returns>hash code of a ContinuousDimensionInterval instance</returns>
        public override int GetHashCode()
        {
            return minValue.GetHashCode() + maxValue.GetHashCode() + increment.GetHashCode();
        }

        /// <summary>
        /// ToString of ContinuousDimensionInterval
        /// </summary>
        /// <returns>minimum, maximum and increment values of the interval</returns>
        public override string ToString()
        {
            return "Minimum Value: " + minValue.ToString() +
                    "\nMaximum Value: " + maxValue.ToString() +
                    "\nIncrement: " + increment.ToString();
        }
    }
}