using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;

namespace core.domain
{
    public class ProductSlotWidths : ValueObject
    {
        /// <summary>
        /// Constant representing the error message presented when the provided minimum slot width is not a number.
        /// </summary>
        private const string ERROR_MIN_WIDTH_NAN = "The provided minimum slot width is not a number.";
        /// <summary>
        /// Constant representing the error message presented when the provided minimum slot width is infinity.
        /// </summary>
        private const string ERROR_MIN_WIDTH_INFINITY = "The provided minimum slot width can not be infinity.";
        /// <summary>
        /// Constant representing the error message presented when the provided minimum slot width is equal to or less than zero.
        /// </summary>
        private const string ERROR_MIN_WIDTH_NEGATIVE_OR_ZERO = "The provided minimum slot width can not have a value equal to or less than zero.";

        /// <summary>
        /// Constant representing the error message presented when the provided maximum slot width is not a number.
        /// </summary>
        private const string ERROR_MAX_WIDTH_NAN = "The provided maximum slot width is not a number.";
        /// <summary>
        /// Constant representing the error message presented when the provided maximum slot width is infinity.
        /// </summary>
        private const string ERROR_MAX_WIDTH_INFINITY = "The provided maximum slot width can not be infinity.";
        /// <summary>
        /// Constant representing the error message presented when the provided maximum slot width is equal to or less than zero.
        /// </summary>
        private const string ERROR_MAX_WIDTH_NEGATIVE_OR_ZERO = "The provided maximum slot width can not have a value equal to or less than zero.";

        /// <summary>
        /// Constant representing the error message presented when the provided recommended slot width is not a number.
        /// </summary>
        private const string ERROR_REC_WIDTH_NAN = "The provided recommended slot width is not a number.";
        /// <summary>
        /// Constant representing the error message presented when the provided recommended slot width is infinity.
        /// </summary>
        private const string ERROR_REC_WIDTH_INFINITY = "The provided recommended slot width can not be infinity.";
        /// <summary>
        /// Constant representing the error message presented when the provided recommended slot width is equal to or less than zero.
        /// </summary>
        private const string ERROR_REC_WIDTH_NEGATIVE_OR_ZERO = "The provided recommended slot width can not have a value equal to or less than zero.";

        /// <summary>
        /// Constant representing the error message presented when the provided minimum value is greater than the maximum value.
        /// </summary>
        private const string ERROR_MIN_WIDTH_GREATER_THAN_MAX = "The provided minimum slot width is greater than the provided maximum width.";
        /// <summary>
        /// Constant representing the error message presented when the provided recommended value is smaller than the minimum value.
        /// </summary>
        private const string ERROR_REC_WIDTH_SMALLER_THAN_MIN = "The provided recommended slot width is smaller than the minimum width.";
        /// <summary>
        /// Constant representing the error message presented when the provided recommended value is greater than the maximum value.
        /// </summary>
        private const string ERROR_REC_WIDTH_GREATER_THAN_MAX = "The provided recommended slot width is greater than the maximum width.";

        /// <summary>
        /// Persistence identifier.
        /// </summary>
        /// <value>Gets/Protected Sets the persistence identifier.</value>
        public long Id { get; protected set; }

        /// <summary>
        /// Product's minimum slot width.
        /// </summary>
        /// <value>Gets/sets the minimum width value.</value>
        public double minWidth { get; protected set; }

        /// <summary>
        /// Product's maximum slot width.
        /// </summary>
        /// <value>Gets/sets the maximum width value.</value>
        public double maxWidth { get; protected set; }

        /// <summary>
        /// Product's recommended slot width.
        /// </summary>
        /// <value>Gets/sets the recommended width value.</value>
        public double recommendedWidth { get; protected set; }

        /// <summary>
        /// Creates a new instance of ProductSlotWidths.
        /// </summary>
        /// <param name="minWidth">The Slot's minimum width.</param>
        /// <param name="maxWidth">The Slots's maximum width.</param>
        /// <param name="recommendedWidth">The Slot's recommended width.</param>
        /// <returns>New instance of ProductSlotWidths with the given values.</returns>
        public static ProductSlotWidths valueOf(double minWidth, double maxWidth, double recommendedWidth)
        {
            return new ProductSlotWidths(minWidth, maxWidth, recommendedWidth);
        }

        /// <summary>
        /// Creates a new instance of ProductSlotWidths.
        /// </summary>
        /// <param name="minWidth">The Slot's minimum width.</param>
        /// <param name="maxWidth">The Slots's maximum width.</param>
        /// <param name="recommendedWidth">The Slot's recommended width.</param>
        /// <returns>New instance of ProductSlotWidths with the given values.</returns>
        private ProductSlotWidths(double minWidth, double maxWidth, double recommendedWidth)
        {
            checkWidths(minWidth, maxWidth, recommendedWidth);
            this.minWidth = minWidth;
            this.maxWidth = maxWidth;
            this.recommendedWidth = recommendedWidth;
        }

        /// <summary>
        /// Checks if the given widths are valid.
        /// </summary>
        /// <param name="minWidth">Minimum value being checked.</param>
        /// <param name="maxWidth">Maximum value being checked.</param>
        /// <param name="recommendedWidth">Recommended value being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when any of the values are not valid.</exception>
        private void checkWidths(double minWidth, double maxWidth, double recommendedWidth)
        {
            if (Double.IsNaN(minWidth))
            {
                throw new ArgumentException(ERROR_MIN_WIDTH_NAN);
            }
            if (Double.IsInfinity(minWidth))
            {
                throw new ArgumentException(ERROR_MIN_WIDTH_INFINITY);
            }
            if (minWidth <= 0)
            {
                throw new ArgumentException(ERROR_MIN_WIDTH_NEGATIVE_OR_ZERO);
            }
            if (Double.IsNaN(maxWidth))
            {
                throw new ArgumentException(ERROR_MAX_WIDTH_NAN);
            }
            if (Double.IsInfinity(maxWidth))
            {
                throw new ArgumentException(ERROR_MAX_WIDTH_INFINITY);
            }
            if (maxWidth <= 0)
            {
                throw new ArgumentException(ERROR_MAX_WIDTH_NEGATIVE_OR_ZERO);
            }
            if (Double.IsNaN(recommendedWidth))
            {
                throw new ArgumentException(ERROR_REC_WIDTH_NAN);
            }
            if (Double.IsInfinity(recommendedWidth))
            {
                throw new ArgumentException(ERROR_REC_WIDTH_INFINITY);
            }
            if (recommendedWidth <= 0)
            {
                throw new ArgumentException(ERROR_REC_WIDTH_NEGATIVE_OR_ZERO);
            }

            if (minWidth > maxWidth)
            {
                throw new ArgumentException(ERROR_MIN_WIDTH_GREATER_THAN_MAX);
            }

            if (minWidth > recommendedWidth)
            {
                throw new ArgumentException(ERROR_REC_WIDTH_SMALLER_THAN_MIN);
            }

            if (maxWidth < recommendedWidth)
            {
                throw new ArgumentException(ERROR_REC_WIDTH_GREATER_THAN_MAX);
            }
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

            ProductSlotWidths other = (ProductSlotWidths)obj;
            
            return this.minWidth.Equals(other.minWidth) 
                && this.maxWidth.Equals(other.maxWidth) 
                && this.recommendedWidth.Equals(other.recommendedWidth);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;

                hash = hash * 29 + minWidth.GetHashCode();
                hash = hash * 29 + maxWidth.GetHashCode();
                hash = hash * 29 + recommendedWidth.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("Minimum Width: {0}; Maximum Width: {1}; Recommended Width: {2}", minWidth, maxWidth, recommendedWidth);
        }
    }
}
