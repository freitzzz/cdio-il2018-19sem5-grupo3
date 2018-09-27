using System;
using Xunit;
using core.domain;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for Dimension
    /// </summary>
    public class DimensionTest
    {

        [Fact]
        public void ensureConstructorDetectsValueIsNaN()
        {
            Action act = () => Dimension.valueOf(Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsValueIsInfinity()
        {
            Action act = () => Dimension.valueOf(Double.PositiveInfinity);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeValue()
        {
            Action act = () => Dimension.valueOf(-4.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            Dimension instance = Dimension.valueOf(100.0);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            Dimension instance = Dimension.valueOf(333.5);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            Dimension instance = Dimension.valueOf(123.4);

            Assert.False(instance.Equals("Lil Xan ate too many Hot Cheetos"));
        }

        [Fact]
        public void ensureInstancesWithDifferentValuesAreNotEqual()
        {
            Dimension instance = Dimension.valueOf(210.5);
            Dimension other = Dimension.valueOf(210.4);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameValueAreEqual()
        {
            Dimension instance = Dimension.valueOf(3.14);
            Dimension other = Dimension.valueOf(3.14);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void testHashCode()
        {
            Dimension instance = Dimension.valueOf(2.718);
            Dimension other = Dimension.valueOf(2.718);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString()
        {
            Dimension instance = Dimension.valueOf(9.8);
            Dimension other = Dimension.valueOf(9.8);

            Assert.Equal(instance.ToString(), other.ToString());
        }
    }
}