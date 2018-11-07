using System;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for Price
    /// </summary>
    public class PriceTest
    {
        [Fact]
        public void ensurePriceCantBeCreatedIfValueIsNaN()
        {
            Action act = () => Price.valueOf(Double.NaN);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensurePriceCantBeCreatedIfValueIsNegative()
        {
            Action act = () => Price.valueOf(-1);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensurePriceCantBeCreatedIfValueIsInfinity()
        {
            Action act = () => Price.valueOf(Double.PositiveInfinity);
            Action otherAct = () => Price.valueOf(Double.NegativeInfinity);

            Assert.Throws<ArgumentException>(act);
            Assert.Throws<ArgumentException>(otherAct);
        }

        [Fact]
        public void ensurePriceCanBeCreatedIfValidIsValue()
        {
            Price instance = Price.valueOf(20);

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensurePricesWithDifferentValuesHaveDifferentHashCodes()
        {
            Price instance = Price.valueOf(20);
            Price other = Price.valueOf(10);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensurePricesWithSameValueHaveEqualHashCodes()
        {
            Price instance = Price.valueOf(30);
            Price other = Price.valueOf(30);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensurePriceIsntEqualToNull()
        {
            Price instance = Price.valueOf(30);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensurePriceIsntEqualToObjectOfDifferentType()
        {
            Price instance = Price.valueOf(20);

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensurePricesWithDifferentValuesArentEqual()
        {
            Price instance = Price.valueOf(20);
            Price other = Price.valueOf(10);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensurePriceIsEqualToItself()
        {
            Price instance = Price.valueOf(20);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensurePricesWithSameValueAreEqual()
        {
            Price instance = Price.valueOf(20);
            Price other = Price.valueOf(20);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            Price instance = Price.valueOf(20);
            Price other = Price.valueOf(20);

            Assert.Equal(instance.ToString(), other.ToString());
        }
    }
}