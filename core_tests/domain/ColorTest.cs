using System;
using core.domain;
using Xunit;


namespace dotnet_example_unittests
{
    public class TodoItemTest
    {

        [Fact]
        public void ensureConstructorDetectsNegativeValue()
        {

            Action act = () => Color.valueOf("test", 5, -10, -555, 2);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsOverValue()
        {

            Action act = () => Color.valueOf("test", 5, 2561, 1, 2);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureNamesAreEqual()
        {
            var item = new Color();

            string expected = "Azul";

            item.Name = expected;

            string actual = item.Name;

            Assert.Equal(expected, actual); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureRedCoordinatesAreEqual()
        {
            var item = new Color();

            int expected = 255;

            item.Red = expected;

            int actual = item.Red;

            Assert.True(expected == actual); //Compare strings and don't ignore case

        }

        [Fact]
        public void testToString()
        {
            Color instance = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);
            Color other = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void ensureStringsAreEqual()
        {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance2 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.True(instance0.sameAs(instance2.Name)); //Compare strings and don't ignore case

        }


        [Fact]
        public void ensureStringsAreNotEqual()
        {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.sameAs(instance1.Name)); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashisEqual()
        {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.True(instance0.GetHashCode() == instance1.GetHashCode());//Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashisNotEqual()
        {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.GetHashCode() == instance1.GetHashCode()); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashIsEqual()
        {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.GetHashCode() == instance1.GetHashCode()); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsEqual()
        {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.Equals(instance1)); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsNotEqualDiffObj()
        {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Assert.False(instance0.Equals(new Object())); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsNotEqualDiffCoordinatesRed()
        {
            Color instance0 = Color.valueOf("10mg Cholestherol", 25, 1, 2, 66);
            Color instance1 = Color.valueOf("10mg Cholestherol", 111, 1, 2, 66);

            Assert.False(instance0.Equals(instance1)); //Compare strings and don't ignore case

        }
    }
}