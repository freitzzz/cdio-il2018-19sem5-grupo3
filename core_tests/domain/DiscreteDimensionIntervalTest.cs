using System;
using System.Collections.Generic;
using Xunit;
using core.domain;

namespace core_tests.domain
{
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
        public void ensureSameInstanceIsEqual(){
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
    }
}