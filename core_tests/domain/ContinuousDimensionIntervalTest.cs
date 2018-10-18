using System;
using Xunit;
using core.domain;
using core.dto;

namespace core_tests.domain
{
    /// <summary>
    /// Unit Testing class for ContinuousDimensionInterval
    /// </summary>
    public class ContinuousDimensionIntervalTest
    {

        [Fact]
        public void ensureConstructorDetectsNegativeMinValue()
        {

            Action act = () => ContinuousDimensionInterval.valueOf(-5.0, 4.0, 1.0);

            Assert.Throws<ArgumentException>(act);

        }

        [Fact]
        public void ensureConstructorDetectsNegativeMaxValue()
        {

            Action act = () => ContinuousDimensionInterval.valueOf(5.0, -10, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeIncrementValue()
        {

            Action act = () => ContinuousDimensionInterval.valueOf(5.0, 10.0, -3.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMinAndMaxValue()
        {

            Action act = () => ContinuousDimensionInterval.valueOf(-5.0, -6.0, 0.1);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMinAndIncrementValue()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(-5.0, 6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMaxAndIncrementValue()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(5.0, -6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsAllNegativeValues()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(-5.0, -6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueGreaterThanMaxValue()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(7.0, 5.0, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementGreaterThanMaxMinDifference()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(5.0, 10.0, 5.1);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueIsInfinity()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(Double.PositiveInfinity, 6.0, 5.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMaxValueIsInfinity()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(4.0, Double.PositiveInfinity, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementIsInfinity()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(4.0, 5.0, Double.PositiveInfinity);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueIsNaN()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(Double.NaN, 5.0, 3.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMaxValueIsNaN()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(100.0, Double.NaN, 20.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementIsNaN()
        {
            Action act = () => ContinuousDimensionInterval.valueOf(100.0, 200.0, Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(100.0, 200.0, 1.0);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(100.0, 150.0, 0.5);

            Assert.False(instance.Equals(null));
        }


        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(5.0, 20.0, 1.0);
            string other = "bananas";

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMinValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 0.1);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(2.0, 10.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMaxValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 50.0, 5.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 40.0, 5.0);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMinAndMaxValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 5.0, 0.1);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(2.0, 4.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentIncrementsAreNotEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 5.0, 1.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 5.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesAreEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            ContinuousDimensionInterval instance = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = ContinuousDimensionInterval.valueOf(1.0, 10.0, 1.0);

            Assert.Equal(instance.toDTO().ToString(), other.toDTO().ToString());
        }
    }
}