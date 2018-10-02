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
            Action act = () => SingleValueDimension.valueOf(Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsValueIsInfinity()
        {
            Action act = () => SingleValueDimension.valueOf(Double.PositiveInfinity);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsNegativeValue()
        {
            Action act = () => SingleValueDimension.valueOf(-4.0);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(100.0);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureInstanceAndNullAreNotEqual()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(333.5);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstancesOfDifferentTypesAreNotEqual()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(123.4);

            Assert.False(instance.Equals("Lil Xan ate too many Hot Cheetos"));
        }

        [Fact]
        public void ensureInstancesWithDifferentValuesAreNotEqual()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(210.5);
            SingleValueDimension other = SingleValueDimension.valueOf(210.4);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstancesWithSameValueAreEqual()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(3.14);
            SingleValueDimension other = SingleValueDimension.valueOf(3.14);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureSameInstanceIsEqual(){
            SingleValueDimension instance = SingleValueDimension.valueOf(3);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void testHashCode()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(2.718);
            SingleValueDimension other = SingleValueDimension.valueOf(2.718);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void testToString()
        {
            SingleValueDimension instance = SingleValueDimension.valueOf(9.8);
            SingleValueDimension other = SingleValueDimension.valueOf(9.8);

            Assert.Equal(instance.ToString(), other.ToString());
        }
    }
}