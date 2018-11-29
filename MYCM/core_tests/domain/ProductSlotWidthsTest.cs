using System;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class ProductSlotWidthsTest
    {
        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsNaN()
        {
            Action nanMinSlotWidth = () => ProductSlotWidths.valueOf(double.NaN, 21, 15);
            Assert.Throws<ArgumentException>(nanMinSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsPositiveInfinity()
        {
            Action positiveInfinityMinSlotWidth = () => ProductSlotWidths.valueOf(double.PositiveInfinity, 21, 15);
            Assert.Throws<ArgumentException>(positiveInfinityMinSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsNegativeInfinity()
        {
            Action negativeInfinityMinSlotWidth = () => ProductSlotWidths.valueOf(double.NegativeInfinity, 21, 15);
            Assert.Throws<ArgumentException>(negativeInfinityMinSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsNegative()
        {
            Action negativeMinSlotWidth = () => ProductSlotWidths.valueOf(-1, 21, 15);
            Assert.Throws<ArgumentException>(negativeMinSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsZero()
        {
            Action zeroMinSlotWidth = () => ProductSlotWidths.valueOf(0, 21, 15);
            Assert.Throws<ArgumentException>(zeroMinSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMaxWidthIsNaN()
        {
            Action nanMaxSlotWidth = () => ProductSlotWidths.valueOf(9, double.NaN, 15);
            Assert.Throws<ArgumentException>(nanMaxSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMaxWidthIsPositiveInfinity()
        {
            Action positiveInfinityMaxSlotWidth = () => ProductSlotWidths.valueOf(9, double.PositiveInfinity, 15);
            Assert.Throws<ArgumentException>(positiveInfinityMaxSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMaxWidthIsNegativeInfinity()
        {
            Action negativeInfinityMaxSlotWidth = () => ProductSlotWidths.valueOf(9, double.NegativeInfinity, 15);
            Assert.Throws<ArgumentException>(negativeInfinityMaxSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMaxWidthIsNegative()
        {
            Action negativeMaxSlotWidth = () => ProductSlotWidths.valueOf(9, -1, 15);
            Assert.Throws<ArgumentException>(negativeMaxSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMaxWidthIsZero()
        {
            Action zeroMaxSlotWidth = () => ProductSlotWidths.valueOf(9, 0, 15);
            Assert.Throws<ArgumentException>(zeroMaxSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfRecommendedWidthIsNaN()
        {
            Action nanRecSlotWidth = () => ProductSlotWidths.valueOf(9, 21, double.NaN);
            Assert.Throws<ArgumentException>(nanRecSlotWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfMinWidthIsGreaterThanMaxWidth()
        {
            Action smallerMaxWidth = () => ProductSlotWidths.valueOf(9, 7, 5);
            Assert.Throws<ArgumentException>(smallerMaxWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfRecommendedWidthIsSmallerThanMinWidth()
        {
            Action smallerThanMinWidthRecWidth = () => ProductSlotWidths.valueOf(7, 9, 3);
            Assert.Throws<ArgumentException>(smallerThanMinWidthRecWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsCantBeCreatedIfRecommendedWidthIsGreaterThanMaxWidth()
        {
            Action greaterThanMaxWidthRecWidth = () => ProductSlotWidths.valueOf(7, 9, 11);
            Assert.Throws<ArgumentException>(greaterThanMaxWidthRecWidth);
        }

        [Fact]
        public void ensureProductSlotWidthsIsCreated()
        {

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            Assert.NotNull(slotWidths);
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            Assert.Equal(slotWidths, slotWidths);
        }

        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            Assert.False(slotWidths.Equals(null));
        }

        [Fact]
        public void ensureDifferentTypeObjectIsNotEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(3, 14, 8);
            Assert.False(slotWidths.Equals(customizedDimensions));
        }

        [Fact]
        public void ensureDifferentMinWidthInstanceIsNotEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(4, 14, 8);
            Assert.NotEqual(slotWidths, otherInstance);
        }

        [Fact]
        public void ensureDifferentMaxWidthInstanceIsNotEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(3, 15, 8);
            Assert.NotEqual(slotWidths, otherInstance);
        }

        [Fact]
        public void ensureDifferentRecommendedWidthInstanceIsNotEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(3, 14, 9);
            Assert.NotEqual(slotWidths, otherInstance);
        }

        [Fact]
        public void ensureSameWidthsInstanceIsEqual()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(3, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(3, 14, 8);
            Assert.Equal(slotWidths, otherInstance);
        }

        [Fact]
        public void ensureDifferentMinWidthProducesDifferentHashCode()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(4, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(3, 14, 8);
            Assert.NotEqual(slotWidths.GetHashCode(), otherInstance.GetHashCode());
        }

        [Fact]
        public void ensureDifferentMaxWidthProducesDifferentHashCode()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(4, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(4, 16, 8);
            Assert.NotEqual(slotWidths.GetHashCode(), otherInstance.GetHashCode());
        }

        [Fact]
        public void ensureDifferentRecommendedWidthProducesDifferenceHashCode()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(4, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(4, 16, 10);
            Assert.NotEqual(slotWidths.GetHashCode(), otherInstance.GetHashCode());
        }

        [Fact]
        public void ensureSameWidthsProducesSameHashCode()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(4, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(4, 14, 8);
            Assert.Equal(slotWidths.GetHashCode(), otherInstance.GetHashCode());
        }

        [Fact]
        public void ensureSameWidthsProducesSameToString()
        {
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(4, 14, 8);
            ProductSlotWidths otherInstance = ProductSlotWidths.valueOf(4, 14, 8);
            Assert.Equal(slotWidths.ToString(), otherInstance.ToString());
        }
    }
}