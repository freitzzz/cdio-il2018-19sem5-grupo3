using System;
using core.domain;
using core.dto;
using Xunit;


namespace dotnet_example_unittests {
    public class TodoItemTest {

        [Fact]
        public void ensureConstructorDetectsNegativeValue() {

            Action act = () => Color.valueOf("test", 5, 5, 250, 2);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureConstructorDetectsOverValue() {

            Action act = () => Color.valueOf("test", 5, 254, 1, 2);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureNamesAreEqual() {
            var item = new Color();

            string expected = "Azul";

            item.Name = expected;

            string actual = item.Name;

            Assert.Equal(expected, actual); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureRedCoordinatesAreEqual() {
            var item = new Color();

            byte expected = 255;

            item.Red = expected;

            byte actual = item.Red;

            Assert.True(expected == actual); //Compare strings and don't ignore case

        }

        [Fact]
        public void testToString() {
            Color instance = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);
            Color other = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void ensureStringsAreEqual() {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance2 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.True(instance0.sameAs(instance2.Name)); //Compare strings and don't ignore case

        }


        [Fact]
        public void ensureStringsAreNotEqual() {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.sameAs(instance1.Name)); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashisEqual() {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Assert.True(instance0.GetHashCode() == instance1.GetHashCode());//Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashisNotEqual() {
            Color instance0 = Color.valueOf("0mg Cholestherol", 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.GetHashCode() == instance1.GetHashCode()); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureHashIsEqual() {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.GetHashCode() == instance1.GetHashCode()); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsEqual() {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Color instance1 = Color.valueOf("10mg Cholestherol", 255, 1, 2, 66);

            Assert.False(instance0.Equals(instance1)); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsNotEqualDiffObj() {
            Color instance0 = Color.valueOf(null, 255, 1, 2, 66);

            Assert.False(instance0.Equals(new Object())); //Compare strings and don't ignore case

        }

        [Fact]
        public void ensureIsNotEqualDiffCoordinatesRed() {
            Color instance0 = Color.valueOf("10mg Cholestherol", 25, 1, 2, 66);
            Color instance1 = Color.valueOf("10mg Cholestherol", 111, 1, 2, 66);

            Assert.False(instance0.Equals(instance1)); //Compare strings and don't ignore case

        }

        [Fact]
        public void testToDTO() {
            Console.WriteLine("toDTO");
            byte red = 1;
            byte blue = 1;
            byte green = 1;
            byte alpha = 1;
            string name = "Cor de Burro quando foge";
            Color color = Color.valueOf(name, red, green, blue, alpha);
            ColorDTO dto = new ColorDTO();
            dto.name = name;
            dto.red = red;
            dto.green = green;
            dto.blue = blue;
            dto.alpha = alpha;
            ColorDTO dto2 = color.toDTO();
            Assert.Equal(dto.name, dto2.name);
            Assert.Equal(dto.red, dto2.red);
            Assert.Equal(dto.green, dto2.green);
            Assert.Equal(dto.blue, dto2.blue);
            Assert.Equal(dto.alpha, dto2.alpha);
        }
    }
}