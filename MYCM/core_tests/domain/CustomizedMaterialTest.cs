using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
namespace core_tests.domain
{
    ///<summary>
    ///Tests of the class CustomizedMaterial.
    ///</summary>
    public class CustomizedMaterialTest
    {

        [Fact]
        public void ensureCustomizedMaterialCantBeCreatedWithNullMaterial()
        {
            Color color = Color.valueOf("Ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Adeus", 12);

            Action act = () => CustomizedMaterial.valueOf(null, color, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantBeCreatedWithNullColor()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Material material = new Material("#verycoolreference", "designation", "ola.jpg",
            new List<Color>(new[] { Color.valueOf("Ola", 1, 1, 1, 1) }),
            new List<Finish>(new[] { finish }));

            Action act = () => CustomizedMaterial.valueOf(material, null, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantBeCreatedWithNullFinish()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Material material = new Material("#eeee213", "designation", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { Finish.valueOf("Ola", 12) }));

            Action act = () => CustomizedMaterial.valueOf(material, color, null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantHaveAColorThatTheMaterialItReferencesDoesNotHave()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("adeus", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish", 12);
            Material material = new Material("#material", "designation", "ola.jpg",
            new List<Color>() { color },
            new List<Finish>() { finish });

            Action act = () => CustomizedMaterial.valueOf(material, otherColor, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantHaveAFinishThatTheMaterialItReferencesDoesNotHave()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish", 12);
            Finish otherFinish = Finish.valueOf("im different", 23);
            Material material = new Material("#material", "designation", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Action act = () => CustomizedMaterial.valueOf(material, color, otherFinish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAColorAndAMaterialOnly()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Ola", 12);
            Material material = new Material("#HELLO123", "designation", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, color));
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAFinishAndAMaterialOnly()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Ola", 12);
            Material material = new Material("#HELLO123", "designation", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, finish));
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAColorAMaterialAndAFinish()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Ola", 12);
            Material material = new Material("#HELLO123", "designation", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, color, finish));
        }

        [Fact]
        public void ensureGetHashCodeWorks()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial cuntMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial cuntMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.Equal(cuntMaterial1.GetHashCode(), cuntMaterial2.GetHashCode());
        }

        [Fact]
        public void ensureCustomizedMaterialsWithSameMaterialSameColorSameFinishAreEqual()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.True(custMaterial1.Equals(custMaterial2));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithDifferentMaterialsAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xau", 12);
            Material material = new Material("#imdifferent", "aerg", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#imalsodifferent", "aerge", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(otherMaterial, color, finish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithDifferentColorsAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("adios", 2, 2, 2, 1);
            Finish finish = Finish.valueOf("xa", 12);
            Material material = new Material("#dzone", "areae", "ola.jpg",
            new List<Color>(new[] { color, otherColor }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherColor, finish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithDifferentFinishesAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xa", 12);
            Finish otherFinish = Finish.valueOf("ax", 12);
            Material material = new Material("#dzone", "areae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish, otherFinish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, color, otherFinish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithColorOnlyWithDifferentMaterialsAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xa", 12);
            Material material = new Material("#dzone", "areae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#aoerig", "aer", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color);
            CustomizedMaterial other = CustomizedMaterial.valueOf(otherMaterial, color);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithColorOnlyWithDifferentColorsAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("adeus", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xa", 12);
            Material material = new Material("#dzone", "areae", "ola.jpg",
            new List<Color>(new[] { color, otherColor }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherColor);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithColorOnlyAreEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xa", 12);
            Material material = new Material("#dzone", "areae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, color);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyWithDifferentMaterialsAreNotEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("arg", 1, 1, 1, 1);
            Material material = new Material("#aegrae", "aerga", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#aegre", "aergae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(otherMaterial, finish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyWithDifferentFinishesAreNotEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Finish otherFinish = Finish.valueOf("bananas", 12);
            Color color = Color.valueOf("aerg", 1, 1, 1, 1);
            Material material = new Material("#aerga", "asdfsa", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish, otherFinish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherFinish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyAreEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, finish);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithColorOnlyAndCustomizedMaterialWithColorAndFinishAreNotEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("otherColor", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae", "ola.jpg",
            new List<Color>(new[] { color, otherColor }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherColor);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithColorOnlyAndCustomizedMaterialWithColorAndFinishAreEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, color);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithFinishOnlyAndCustomizedMaterialWithColorAndFinishAreNotEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Finish otherFinish = Finish.valueOf("description", 12);
            Material material = new Material("#aergaer", "aergae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish, otherFinish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherFinish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithFinishOnlyAndCustomizedMaterialWithColorAndFinishAreEqual()
        {
            Finish finish = Finish.valueOf("Ola", 12);
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae", "ola.jpg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, finish);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialAndNullAreNotEqual()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.False(custMaterial1.Equals(null));
        }

        [Fact]
        public void ensureCustomizedMaterialAndInstanceOfDifferentTypeAreNotEqual()
        {

            Finish finish = Finish.valueOf("Acabamento polido", 12);
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Assert.False(custMaterial.Equals(finishes));
        }

        [Fact]
        public void testToString()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.Equal(custMaterial1.ToString(), custMaterial2.ToString());
        }

        [Fact]
        public void ensureChangeColorChangesColor()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("Amarelo", 2, 2, 3, 3);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            colors.Add(otherColor);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Assert.True(customizedMaterial.changeColor(otherColor));
            Assert.NotEqual(customizedMaterial.color, color);
        }

        [Fact]
        public void ensureChangeColorDoesNotChangeColorIfNewColorIsNull()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Action act = () => customizedMaterial.changeColor(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.color, color);
        }

        [Fact]
        public void ensureChangeColorDoesNotChangeColorIfNewColorIsNotInTheMaterial()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("Amarelo", 2, 2, 3, 3);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Action act = () => customizedMaterial.changeColor(otherColor);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.color, color);
        }

        [Fact]
        public void ensureChangeFinishChangesFinish()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            Finish otherFinish = Finish.valueOf("Wax", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            finishes.Add(otherFinish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Assert.True(customizedMaterial.changeFinish(otherFinish));
            Assert.NotEqual(customizedMaterial.finish, finish);
        }

        [Fact]
        public void ensureChangeFinishDoesNotChangeFinishIfNewFinishIsNull()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Action act = () => customizedMaterial.changeFinish(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.finish, finish);
        }

        [Fact]
        public void ensureChangeFinishDoesNotChangeFinishIfNewFinishIsNotInTheMaterial()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido", 12);
            Finish otherFinish = Finish.valueOf("Wax", 12);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Action act = () => customizedMaterial.changeFinish(otherFinish);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.finish, finish);
        }
    }
}