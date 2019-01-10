using System;
using Xunit;
using core.domain;
using core.dto;

namespace core_tests.domain {
    /// <summary>
    /// Unit testing class for SingleValueDimension
    /// </summary>
    public class SingleValueDimensionTest {

        [Fact]
        public void ensureConstructorDetectsValueIsNaN() {
            Action act = () => new SingleValueDimension(Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsValueIsInfinity() {
            Action act = () => new SingleValueDimension(Double.PositiveInfinity);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeValue() {
            Action act = () => new SingleValueDimension(-4.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsZeroValue() {
            Action act = () => new SingleValueDimension(0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated() {
            SingleValueDimension instance = new SingleValueDimension(100.0);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureSingleValueDimensionDoesNotHaveDifferentValue() {
            SingleValueDimension instance = new SingleValueDimension(28);

            Assert.False(instance.hasValue(29));
        }

        [Fact]
        public void ensureSingleValueDimensionHasValue() {
            SingleValueDimension instance = new SingleValueDimension(28);

            Assert.True(instance.hasValue(28));
        }

        [Fact]
        public void ensureSingleValueDimensionGetMaxValueIsTheSameAsDefinedValue() {
            double value = 28;
            SingleValueDimension instance = new SingleValueDimension(value);
            double maxValue = instance.getMaxValue();
            Assert.Equal(value, maxValue, 0);
        }

        [Fact]
        public void ensureSingleValueDimensionGetMinValueIsTheSameAsDefinedValue() {
            double value = 28;
            SingleValueDimension instance = new SingleValueDimension(value);
            double minValue = instance.getMinValue();

            Assert.Equal(value, minValue, 0);
        }

        [Fact]
        public void ensureGetValuesAsArrayWorks() {
            double value = 28;
            SingleValueDimension instance = new SingleValueDimension(value);
            double[] expectedValues = { 28 };
            Assert.Equal(expectedValues, instance.getValuesAsArray());
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual() {
            SingleValueDimension instance = new SingleValueDimension(333.5);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual() {
            SingleValueDimension instance = new SingleValueDimension(123.4);

            Assert.False(instance.Equals("Lil Xan ate too many Hot Cheetos"));
        }

        [Fact]
        public void ensureInstancesWithDifferentValuesAreNotEqual() {
            SingleValueDimension instance = new SingleValueDimension(210.5);
            SingleValueDimension other = new SingleValueDimension(210.4);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameValueAreEqual() {
            SingleValueDimension instance = new SingleValueDimension(3.14);
            SingleValueDimension other = new SingleValueDimension(3.14);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual() {
            SingleValueDimension instance = new SingleValueDimension(3);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode() {
            SingleValueDimension instance = new SingleValueDimension(2.718);
            SingleValueDimension other = new SingleValueDimension(2.718);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString() {
            SingleValueDimension instance = new SingleValueDimension(9.8);
            SingleValueDimension other = new SingleValueDimension(9.8);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO() {
            SingleValueDimension instance = new SingleValueDimension(10.0);
            SingleValueDimensionDTO dto = (SingleValueDimensionDTO)instance.toDTO();

            Assert.Equal("mm", dto.unit);
            Assert.Equal(10.0, dto.value);
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDefaultsToMilimetres() {
            SingleValueDimension instance = new SingleValueDimension(10.0);
            SingleValueDimensionDTO dto = (SingleValueDimensionDTO)instance.toDTO(null);

            Assert.Equal("mm", dto.unit);
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDoesNotConvertValues() {
            SingleValueDimension instance = new SingleValueDimension(10.0);
            SingleValueDimensionDTO dto = (SingleValueDimensionDTO)instance.toDTO(null);

            Assert.Equal(10.0, dto.value);
        }

        [Fact]
        public void ensureToDTOConvertsValuesToGivenUnit() {
            SingleValueDimension instance = new SingleValueDimension(10.0);
            SingleValueDimensionDTO dto = (SingleValueDimensionDTO)instance.toDTO("cm");

            Assert.Equal("cm", dto.unit);
            Assert.Equal(1.0, dto.value);
        }
    }
}