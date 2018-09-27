using System;
using core.domain;
using Xunit;


namespace dotnet_example_unittests
{
    public class TodoItemTest
    {
        [Fact]
        public void EnsureNamesAreEqual()
        {
            var item = new Color();

            string expected = "Azul";

            item.Name = expected;

            string actual = item.Name;

            Assert.Equal(expected, actual, false); //Compare strings and don't ignore case

        }

        [Fact]
        public void EnsureRedCoordinatesAreEqual()
        {
            var item = new Color();

            int expected = 255;

            item.Red = expected;

            int actual = item.Red;

            Assert.Equal(expected, actual); //Compare strings and don't ignore case

        }

        
    }
}