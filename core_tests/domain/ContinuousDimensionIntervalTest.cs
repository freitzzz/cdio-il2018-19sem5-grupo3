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

            Action act = () => new ContinuousDimensionInterval(-5.0, 4.0, 1.0);

            Assert.Throws<ArgumentException>(act);

        }

        [Fact]
        public void ensureConstructorDetectsNegativeMaxValue()
        {

            Action act = () => new ContinuousDimensionInterval(5.0, -10, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeIncrementValue()
        {

            Action act = () => new ContinuousDimensionInterval(5.0, 10.0, -3.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMinAndMaxValue()
        {

            Action act = () => new ContinuousDimensionInterval(-5.0, -6.0, 0.1);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMinAndIncrementValue()
        {
            Action act = () => new ContinuousDimensionInterval(-5.0, 6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeMaxAndIncrementValue()
        {
            Action act = () => new ContinuousDimensionInterval(5.0, -6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsAllNegativeValues()
        {
            Action act = () => new ContinuousDimensionInterval(-5.0, -6.0, -1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueGreaterThanMaxValue()
        {
            Action act = () => new ContinuousDimensionInterval(7.0, 5.0, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementGreaterThanMaxMinDifference()
        {
            Action act = () => new ContinuousDimensionInterval(5.0, 10.0, 5.1);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueIsInfinity()
        {
            Action act = () => new ContinuousDimensionInterval(Double.PositiveInfinity, 6.0, 5.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMaxValueIsInfinity()
        {
            Action act = () => new ContinuousDimensionInterval(4.0, Double.PositiveInfinity, 1.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementIsInfinity()
        {
            Action act = () => new ContinuousDimensionInterval(4.0, 5.0, Double.PositiveInfinity);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMinValueIsNaN()
        {
            Action act = () => new ContinuousDimensionInterval(Double.NaN, 5.0, 3.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsMaxValueIsNaN()
        {
            Action act = () => new ContinuousDimensionInterval(100.0, Double.NaN, 20.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsIncrementIsNaN()
        {
            Action act = () => new ContinuousDimensionInterval(100.0, 200.0, Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(100.0, 200.0, 1.0);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(100.0, 150.0, 0.5);

            Assert.False(instance.Equals(null));
        }


        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(5.0, 20.0, 1.0);
            string other = "bananas";

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMinValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 0.1);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(2.0, 10.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMaxValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 50.0, 5.0);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(1.0, 40.0, 5.0);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentMinAndMaxValuesAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 5.0, 0.1);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(2.0, 4.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithDifferentIncrementsAreNotEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 5.0, 1.0);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(1.0, 5.0, 0.1);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesAreEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(1.0, 10.0, 1.0);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(1.0, 10.0, 1.0);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionInterval other = new ContinuousDimensionInterval(1.0, 10.0, 1.0);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionIntervalDTO dto = (ContinuousDimensionIntervalDTO)instance.toDTO();

            Assert.Equal("mm", dto.unit);
            Assert.Equal(1.0, dto.minValue);
            Assert.Equal(10.0, dto.maxValue);
            Assert.Equal(1.0, dto.increment);
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDefaultsToMilimetres()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionIntervalDTO dto = (ContinuousDimensionIntervalDTO)instance.toDTO(null);

            Assert.Equal("mm", dto.unit);
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDoesNotConvertValues()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionIntervalDTO dto = (ContinuousDimensionIntervalDTO)instance.toDTO(null);

            Assert.Equal(1.0, dto.minValue);
            Assert.Equal(10.0, dto.maxValue);
            Assert.Equal(1.0, dto.increment);
        }

        [Fact]
        public void ensureToDTOConvertsValuesToGivenUnit()
        {
            ContinuousDimensionInterval instance = new ContinuousDimensionInterval(1.0, 10.0, 1.0);
            ContinuousDimensionIntervalDTO dto = (ContinuousDimensionIntervalDTO)instance.toDTO("cm");

            Assert.Equal(0.1, dto.minValue);
            Assert.Equal(1.0, dto.maxValue);
            Assert.Equal(0.1, dto.increment);
        }
    }
}