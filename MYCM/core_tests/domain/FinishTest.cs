using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
using core.dto;

namespace core_tests.domain
{

    /// <summary>
    /// Tests of the class Finish
    /// </summary>
    public class FinishTest
    {

        [Fact]
        public void testGetHashCode()
        {
            Assert.Equal(Finish.valueOf("Acabamento polido", 12).GetHashCode(),
            Finish.valueOf("Acabamento polido", 34).GetHashCode());
        }

        [Fact]
        public void testDifferentFinishsAreNotEqual()
        {
            Assert.False(Finish.valueOf("Acabamento polido", 12).Equals(
            Finish.valueOf("Acabamento matte", 12)));
        }

        [Fact]
        public void testEqualFinishsAreEqual()
        {
            Assert.True(Finish.valueOf("Acabamento polido", 12).Equals(
            Finish.valueOf("Acabamento polido", 34)));
        }

        [Fact]
        public void testNullFinishIsNotEqual()
        {
            Assert.False(Finish.valueOf("Acabamento polido", 12).Equals(null));
        }

        [Fact]
        public void testDifferentTypesAreNotEqual()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Cor de burro quando foge", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            finishes.Add(finish);

            Assert.False(finish.Equals(new Material("1160912", "No", "ola.jpg", colors, finishes)));
        }

        [Fact]
        public void testToString()
        {
            Assert.Equal(Finish.valueOf("Acabamento polido", 12).ToString(),
            Finish.valueOf("Acabamento polido", 12).ToString());
        }

        [Fact]
        public void ensureNullDescriptionIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf(null, 12));
        }

        [Fact]
        public void ensureEmptyDescriptionIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf("", 12));
        }

        [Fact]
        public void ensureNegativeShininessIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf("Acabamento polido", -1));
        }

        [Fact]
        public void ensureShininessGreaterThanOneHundredIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf("Acabamento polido", 101));
        }

        [Fact]
        public void testToDTO()
        {
            Console.WriteLine("toDTO");
            string description = "this is the best finish ever";
            float shininess = 12;

            Finish finish = Finish.valueOf(description, shininess);
            FinishDTO dto = new FinishDTO();
            dto.description = description;
            dto.shininess = shininess;

            Assert.Equal(dto.description, finish.toDTO().description);
            Assert.Equal(dto.shininess, finish.toDTO().shininess);
        }
    }
}
