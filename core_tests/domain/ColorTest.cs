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
        public void EnsureNamesAreEqual()
        {
            var item = new Color();

            string expected = "Azul";

            item.Name = expected;

            string actual = item.Name;

            Assert.True(String.Equals(expected, actual)); //Compare strings and don't ignore case

        }

        [Fact]
        public void EnsureRedCoordinatesAreEqual()
        {
            var item = new Color();

            int expected = 255;

            item.Red = expected;

            int actual = item.Red;

            Assert.True(expected==actual); //Compare strings and don't ignore case

        }

        [Fact]
        public void testToString()
        {
            Color instance = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);
            Color other = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.True(String.Equals(instance.ToString(), other.ToString()));
        }
    }
}