using System;
using System.Collections.Generic;
using Xunit;
using core.domain;
using core.dto;

namespace core_tests.domain {
    /// <summary>
    /// Unit testing class for DiscreteDimensionInterval
    /// </summary>
    public class DiscreteDimensionIntervalTest {

        [Fact]
        public void ensureConstructorDetectsEmptyList() {
            Action act = () => new DiscreteDimensionInterval(new List<double>());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNullValue() {
            Action act = () => new DiscreteDimensionInterval(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNaNValue() {
            var list = new List<double>();
            list.Add(1.0);
            list.Add(Double.NaN);

            Action act = () => new DiscreteDimensionInterval(list);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsInfinityValue() {
            var list = new List<double>();
            list.Add(2.0);
            list.Add(Double.PositiveInfinity);

            Action act = () => new DiscreteDimensionInterval(list);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeValue() {
            var list = new List<double>();
            list.Add(2.0);
            list.Add(-1.0);

            Action act = () => new DiscreteDimensionInterval(list);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsZeroValue() {
            var list = new List<double>();
            list.Add(2.0);
            list.Add(0);

            Action act = () => new DiscreteDimensionInterval(list);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated() {
            var list = new List<double>();
            list.Add(1.0);

            Assert.NotNull(new DiscreteDimensionInterval(list));
        }

        [Fact]
        public void ensureDiscreteDimensionIntervalDoesNotHaveValue() {
            List<double> values = new List<double>();
            values.Add(12);
            values.Add(13);
            values.Add(13.5);
            values.Add(14);

            DiscreteDimensionInterval discreteDimension = new DiscreteDimensionInterval(values);

            Assert.False(discreteDimension.hasValue(15));
        }

        [Fact]
        public void ensureDiscretDimensioIntervalHasValue() {
            List<double> values = new List<double>();
            values.Add(12);
            values.Add(13);
            values.Add(13.5);
            values.Add(14);

            DiscreteDimensionInterval discreteDimension = new DiscreteDimensionInterval(values);

            Assert.True(discreteDimension.hasValue(13.5));
        }

        [Fact]
        public void ensureDiscreteDimensionIntervalGetMaxValueIsTheSameAsTheLargestValueOnTheCollection() {
            double maxValue = 14;
            List<double> values = new List<double>();
            values.Add(12);
            values.Add(13);
            values.Add(13.5);
            values.Add(maxValue);

            DiscreteDimensionInterval discreteDimension = new DiscreteDimensionInterval(values);

            double obtainedMaxValue = discreteDimension.getMaxValue();
            Assert.Equal(maxValue, obtainedMaxValue, 0);
        }

        [Fact]
        public void ensureDiscreteDimensionIntervalGetMinValueIsTheSameAsTheLowestValueOnTheCollection() {
            double minValue = 12;
            List<double> values = new List<double>();
            values.Add(minValue);
            values.Add(13);
            values.Add(13.5);
            values.Add(14);

            DiscreteDimensionInterval discreteDimension = new DiscreteDimensionInterval(values);

            double obtainedMinValue = discreteDimension.getMinValue();
            Assert.Equal(minValue, obtainedMinValue, 0);
        }

        [Fact]
        public void ensureGetValuesAsArrayWorks() {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);

            double[] expectedValues = { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            Assert.Equal(expectedValues, instance.getValuesAsArray());
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual() {
            var list = new List<double>();
            list.Add(4.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual() {
            var list = new List<double>();
            list.Add(2.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.False(instance.Equals("Lil Pump"));
        }

        [Fact]
        public void ensureInstancesWithDifferentListsAreNotEqual() {
            var list = new List<double>();
            var otherList = new List<double>();
            list.Add(6.9);
            otherList.Add(4.20);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(otherList);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameListsAreEqual() {
            var list = new List<double>();
            list.Add(33.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual() {
            var list = new List<double>();
            list.Add(3.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode() {
            var list = new List<double>();
            list.Add(30.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString() {
            var list = new List<double>();
            list.Add(30.3);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO() {
            var list = new List<double>();
            list.Add(1.234);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.Equal(instance.toDTO().ToString(), other.toDTO().ToString());
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDefaultsToMilimetres() {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO(null);

            Assert.Equal("mm", dto.unit);
        }

        [Fact]
        public void ensureToDTOWithNullStringDoesNotConvertValues() {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO(null);

            var expectedValues = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            Assert.Equal(expectedValues, dto.values);
        }

        [Fact]
        public void ensureToDTOConvertsValuesToGivenUnit() {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO("cm");

            var expectedValues = new List<double>() { 1.25, 1.3, 1.35, 1.4, 1.45, 1.5, 1.6, 1.7 };

            Assert.Equal(expectedValues, dto.values);
        }
    }
}