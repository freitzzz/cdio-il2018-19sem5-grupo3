using System;
using System.Collections.Generic;
using Xunit;
using core.domain;
using core.dto;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for DiscreteDimensionInterval
    /// </summary>
    public class DiscreteDimensionIntervalTest
    {

        [Fact]
        public void ensureConstructorDetectsEmptyList()
        {
            Action act = () => DiscreteDimensionInterval.valueOf(new List<double>());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNullValue()
        {
            Action act = () => DiscreteDimensionInterval.valueOf(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            var list = new List<double>();
            list.Add(1.0);

            Assert.NotNull(DiscreteDimensionInterval.valueOf(list));
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            var list = new List<double>();
            list.Add(4.0);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            var list = new List<double>();
            list.Add(2.0);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);

            Assert.False(instance.Equals("Lil Pump"));
        }

        [Fact]
        public void ensureInstancesWithDifferentListsAreNotEqual()
        {
            var list = new List<double>();
            var otherList = new List<double>();
            list.Add(6.9);
            otherList.Add(4.20);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);
            DiscreteDimensionInterval other = DiscreteDimensionInterval.valueOf(otherList);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameListsAreEqual()
        {
            var list = new List<double>();
            list.Add(33.0);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);
            DiscreteDimensionInterval other = DiscreteDimensionInterval.valueOf(list);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            var list = new List<double>();
            list.Add(3.0);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode()
        {
            var list = new List<double>();
            list.Add(30.0);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);
            DiscreteDimensionInterval other = DiscreteDimensionInterval.valueOf(list);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString()
        {
            var list = new List<double>();
            list.Add(30.3);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);
            DiscreteDimensionInterval other = DiscreteDimensionInterval.valueOf(list);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            var list = new List<double>();
            list.Add(1.234);
            DiscreteDimensionInterval instance = DiscreteDimensionInterval.valueOf(list);
            DiscreteDimensionInterval other = DiscreteDimensionInterval.valueOf(list);

            Assert.Equal(instance.toDTO().ToString(), other.toDTO().ToString());
        }

        [Fact]
        public void ensureToDTOWithNullUnitStringDefaultsToMilimetres()
        {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO(null);

            Assert.Equal("mm", dto.unit);
        }

        [Fact]
        public void ensureToDTOWithNullStringDoesNotConvertValues()
        {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO(null);

            var expectedValues = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            Assert.Equal(expectedValues, dto.values);
        }

        [Fact]
        public void ensureToDTOConvertsValuesToGivenUnit()
        {
            var values = new List<double>() { 12.5, 13, 13.5, 14, 14.5, 15, 16, 17 };

            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(values);
            DiscreteDimensionIntervalDTO dto = (DiscreteDimensionIntervalDTO)instance.toDTO("cm");

            var expectedValues = new List<double>() { 1.25, 1.3, 1.35, 1.4, 1.45, 1.5, 1.6, 1.7 };

            Assert.Equal(expectedValues, dto.values);
        }
    }
}