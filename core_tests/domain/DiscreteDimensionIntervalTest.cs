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
            Action act = () => new DiscreteDimensionInterval(new List<double>());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNullValue()
        {
            Action act = () => new DiscreteDimensionInterval(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            var list = new List<double>();
            list.Add(1.0);

            Assert.NotNull(new DiscreteDimensionInterval(list));
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            var list = new List<double>();
            list.Add(4.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            var list = new List<double>();
            list.Add(2.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.False(instance.Equals("Lil Pump"));
        }

        [Fact]
        public void ensureInstancesWithDifferentListsAreNotEqual()
        {
            var list = new List<double>();
            var otherList = new List<double>();
            list.Add(6.9);
            otherList.Add(4.20);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(otherList);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameListsAreEqual()
        {
            var list = new List<double>();
            list.Add(33.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            var list = new List<double>();
            list.Add(3.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testGetHashCode()
        {
            var list = new List<double>();
            list.Add(30.0);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            int instanceHash = instance.GetHashCode();
            int otherHash = other.GetHashCode();

            Assert.Equal(instanceHash, otherHash);
        }

        [Fact]
        public void testToString()
        {
            var list = new List<double>();
            list.Add(30.3);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            var list = new List<double>();
            list.Add(1.234);
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(list);
            DiscreteDimensionInterval other = new DiscreteDimensionInterval(list);

            Assert.Equal(instance.toDTO().ToString(), other.toDTO().ToString());
        }
    }
}