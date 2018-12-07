using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class CustomizedProductSerialNumberTest
    {
        [Fact]
        public void ensureConstructorCreatesInstance()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            Assert.NotNull(serialNumber);
        }

        [Fact]
        public void ensureNewInstanceHasAnInitialSerialNumberOfZero()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            string expected = "0";

            Assert.Equal(expected, serialNumber.serialNumber);
        }

        [Fact]
        public void ensureIncrementSerialNumberChangesSerialNumber()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();

            string expected = "1";

            Assert.Equal(expected, serialNumber.serialNumber);
        }

        [Fact]
        public void ensureIdReturnsTheSerialNumber()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();

            string expected = "1";

            Assert.Equal(expected, serialNumber.id());
            Assert.Equal(serialNumber.serialNumber, serialNumber.id());
        }

        [Fact]
        public void ensureSameAs()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();

            string expected = "1";

            Assert.True(serialNumber.sameAs(expected));
        }

        [Fact]
        public void ensureSerialNumberIsEqualToItself()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            Assert.True(serialNumber.Equals(serialNumber));
        }

        [Fact]
        public void ensureNullIsNotEqual()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            Assert.False(serialNumber.Equals(null));
        }

        [Fact]
        public void ensureDifferentObjectTypeIsNotEqual()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            Assert.False(serialNumber.Equals("string here"));
        }

        [Fact]
        public void ensureSerialNumbersAreEqual()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            CustomizedProductSerialNumber otherSerialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();
            otherSerialNumber.incrementSerialNumber();

            Assert.Equal(serialNumber, otherSerialNumber);
        }

        [Fact]
        public void ensureSerialNumbersAreNotEqual()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();

            CustomizedProductSerialNumber otherSerialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();

            Assert.NotEqual(serialNumber, otherSerialNumber);
        }

        [Fact]
        public void ensureSerialNumbersHaveSameHashCode()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();
            CustomizedProductSerialNumber otherSerialNumber = new CustomizedProductSerialNumber();

            Assert.Equal(serialNumber.GetHashCode(), otherSerialNumber.GetHashCode());
        }

        [Fact]
        public void ensureSerialNumbersHaveDifferentHashCodes()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();
            CustomizedProductSerialNumber otherSerialNumber = new CustomizedProductSerialNumber();

            serialNumber.incrementSerialNumber();

            Assert.NotEqual(serialNumber.GetHashCode(), otherSerialNumber.GetHashCode());
        }

        [Fact]
        public void ensureToStringWorks()
        {
            CustomizedProductSerialNumber serialNumber = new CustomizedProductSerialNumber();
            CustomizedProductSerialNumber otherSerialNumber = new CustomizedProductSerialNumber();

            Assert.Equal(serialNumber.ToString(), otherSerialNumber.ToString());
        }
    }
}