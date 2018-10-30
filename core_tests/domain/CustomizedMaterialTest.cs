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
            Finish finish = Finish.valueOf("Adeus");

            Action act = () => CustomizedMaterial.valueOf(null, color, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantBeCreatedWithNullColor()
        {
            Finish finish = Finish.valueOf("Ola");
            Material material = new Material("#verycoolreference", "designation",
            new List<Color>(new[] { Color.valueOf("Ola", 1, 1, 1, 1) }),
            new List<Finish>(new[] { finish }));

            Action act = () => CustomizedMaterial.valueOf(material, null, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantBeCreatedWithNullFinish()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Material material = new Material("#eeee213", "designation",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { Finish.valueOf("Ola") }));

            Action act = () => CustomizedMaterial.valueOf(material, color, null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantHaveAColorThatTheMaterialItReferencesDoesNotHave()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("adeus", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish");
            Material material = new Material("#material", "designation",
            new List<Color>(){ color },
            new List<Finish>(){ finish });

            Action act = () => CustomizedMaterial.valueOf(material, otherColor, finish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCantHaveAFinishThatTheMaterialItReferencesDoesNotHave()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish");
            Finish otherFinish = Finish.valueOf("im different");
            Material material = new Material("#material", "designation",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Action act = () => CustomizedMaterial.valueOf(material, color, otherFinish);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAColorAndAMaterialOnly()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("ola");
            Material material = new Material("#HELLO123", "designation",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, color));
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAFinishAndAMaterialOnly()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("ola");
            Material material = new Material("#HELLO123", "designation",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, finish));
        }

        [Fact]
        public void ensureCustomizedMaterialCanBeCreatedWithAColorAMaterialAndAFinish()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("ola");
            Material material = new Material("#HELLO123", "designation",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            Assert.NotNull(CustomizedMaterial.valueOf(material, color, finish));
        }

        [Fact]
        public void ensureGetHashCodeWorks()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", colors, finishes);
            CustomizedMaterial cuntMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial cuntMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.Equal(cuntMaterial1.GetHashCode(), cuntMaterial2.GetHashCode());
        }

        [Fact]
        public void ensureCustomizedMaterialsWithSameMaterialSameColorSameFinishAreEqual()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.True(custMaterial1.Equals(custMaterial2));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithDifferentMaterialsAreNotEqual()
        {
            Color color = Color.valueOf("ola", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("xau");
            Material material = new Material("#imdifferent", "aerg",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#imalsodifferent", "aerge",
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
            Finish finish = Finish.valueOf("xa");
            Material material = new Material("#dzone", "areae",
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
            Finish finish = Finish.valueOf("xa");
            Finish otherFinish = Finish.valueOf("ax");
            Material material = new Material("#dzone", "areae",
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
            Finish finish = Finish.valueOf("xa");
            Material material = new Material("#dzone", "areae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#aoerig", "aer",
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
            Finish finish = Finish.valueOf("xa");
            Material material = new Material("#dzone", "areae",
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
            Finish finish = Finish.valueOf("xa");
            Material material = new Material("#dzone", "areae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, color);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyWithDifferentMaterialsAreNotEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("arg", 1, 1, 1, 1);
            Material material = new Material("#aegrae", "aerga",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));
            Material otherMaterial = new Material("#aegre", "aergae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(otherMaterial, finish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyWithDifferentFinishesAreNotEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Finish otherFinish = Finish.valueOf("bananas");
            Color color = Color.valueOf("aerg", 1, 1, 1, 1);
            Material material = new Material("#aerga", "asdfsa",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish, otherFinish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherFinish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialsWithFinishOnlyAreEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, finish);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithColorOnlyAndCustomizedMaterialWithColorAndFinishAreNotEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Color otherColor = Color.valueOf("otherColor", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae",
            new List<Color>(new[] { color, otherColor }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherColor);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithColorOnlyAndCustomizedMaterialWithColorAndFinishAreEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, color);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithFinishOnlyAndCustomizedMaterialWithColorAndFinishAreNotEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Finish otherFinish = Finish.valueOf("description");
            Material material = new Material("#aergaer", "aergae",
            new List<Color>(new[] { color }),
            new List<Finish>(new[] { finish, otherFinish }));

            CustomizedMaterial instance = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial other = CustomizedMaterial.valueOf(material, otherFinish);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureCustomizedMaterialWithFinishOnlyAndCustomizedMaterialWithColorAndFinishAreEqual()
        {
            Finish finish = Finish.valueOf("ola");
            Color color = Color.valueOf("aerga", 1, 1, 1, 1);
            Material material = new Material("#aergaer", "aergae",
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
            Finish finish = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.False(custMaterial1.Equals(null));
        }

        [Fact]
        public void ensureCustomizedMaterialAndInstanceOfDifferentTypeAreNotEqual()
        {

            Finish finish = Finish.valueOf("Acabamento polido");
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", colors, finishes);
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);

            Assert.False(custMaterial.Equals(finishes));
        }

        [Fact]
        public void testToString()
        {
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(material, color, finish);

            Assert.Equal(custMaterial1.ToString(), custMaterial2.ToString());
        }
    }
}