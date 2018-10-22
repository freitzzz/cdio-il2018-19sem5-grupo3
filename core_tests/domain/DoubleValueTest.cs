using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class DoubleValueTest
    {

        [Fact]
        public void ensureValueOfCreatesInstance()
        {
            DoubleValue doubleValue = DoubleValue.valueOf(21);

            Assert.NotNull(doubleValue);
        }


        [Fact]
        public void ensureImplicitOperatorCreatesInstance()
        {
            DoubleValue doubleValue = 21;

            Assert.NotNull(doubleValue);
        }

        [Fact]
        public void ensureDoubleValueAndDoubleAreEqualWhenUsingSameValue()
        {

            double expected = 21;

            double actual = DoubleValue.valueOf(21);

            Assert.True(actual.Equals(expected));
        }

        [Fact]
        public void ensureDoubleValueAndDoubleAreNotEqualWhenUsingDifferentValues()
        {

            double number = 12;

            DoubleValue doubleValue = DoubleValue.valueOf(21);

            Assert.False(doubleValue.Equals(number));
        }

        [Fact]
        public void ensureSameReferenceDoubleValuesAreEqual()
        {

            DoubleValue doubleValue = 21;

            DoubleValue otherDoubleValue = doubleValue;

            Assert.True(doubleValue.Equals(otherDoubleValue));
        }

        [Fact]
        public void ensureDoubleValueAndOtherObjectTypeAreNotEqual()
        {

            DoubleValue doubleValue = 21;

            string other = "This is just a string";

            Assert.False(doubleValue.Equals(other));
        }

        [Fact]
        public void ensureDoubleValueAndNullObjectAreNotEqual()
        {
            DoubleValue doubleValue = 21;

            DoubleValue otherDoubleValue = null;

            Assert.False(doubleValue.Equals(otherDoubleValue));
        }

        [Fact]
        public void ensureDoubleValueAndDoubleHaveSameHashCode()
        {

            double number = 21;

            DoubleValue doubleValue = DoubleValue.valueOf(21);

            Assert.Equal(number.GetHashCode(), doubleValue.GetHashCode());
        }

        [Fact]
        public void ensureDoubleValueAndDoubleHaveDifferentHashCode()
        {
            double number = 12;

            DoubleValue doubleValue = DoubleValue.valueOf(21);

            Assert.NotEqual(number.GetHashCode(), doubleValue.GetHashCode());
        }

    }
}