using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class CustomizedMaterial.
    </summary>
    */
    public class CustomizedMaterialTest
    {
        /**
        <summary>
            Test to ensure that the method GetHashCode works.
         </summary>
         */
        [Fact]
        public void testGetHashCode()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial cuntMaterial1 = CustomizedMaterial.valueOf(color,finish);
            CustomizedMaterial cuntMaterial2 = CustomizedMaterial.valueOf(color,finish);

            Assert.Equal(cuntMaterial1.GetHashCode(), cuntMaterial2.GetHashCode());
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedMaterial with different colors.
         </summary>
         */
        /**[Fact]
        public void testDifferentCustMaterialColorAreNotEqual()
        {
            Color color1 = Color.valueOf("Azul",1,1,1,1);
            Color color2 = Color.valueOf("Rosa",2,2,2,2);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1,finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(color2,finish);

            Assert.False(custMaterial1.Equals(custMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedMaterial with different finish.
         </summary>
         */
        /**[Fact]
        public void testDifferentCustMaterialFinishAreNotEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish1 = Finish.valueOf("Acabamento polido");
            Finish finish2 = Finish.valueOf("Acabamento rogoso");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color,finish1);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(color,finish2);

            Assert.False(custMaterial1.Equals(custMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedMaterial with the same color and finish.
         </summary>
         */
        [Fact]
        public void testEqualCustMaterialAreEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color,finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(color,finish);

            Assert.True(custMaterial1.Equals(custMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a null CustomizedMaterial.
         </summary>
         */
        [Fact]
        public void testNullCustomizedMaterialIsNotEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color,finish);

            Assert.False(custMaterial1.Equals(null));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a CustomizedMaterial and an object of another type.
         </summary>
         */
        [Fact]
        public void testDifferentTypesAreNotEqual()
        {

            Finish finish = Finish.valueOf("Acabamento polido");
            Color color = Color.valueOf("Azul",1,1,1,1);
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color,finish);

            Assert.False(finish.Equals(custMaterial));
        }
        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void testToString()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color,finish);
            CustomizedMaterial custMaterial2 = CustomizedMaterial.valueOf(color,finish);

            Assert.Equal(custMaterial1.ToString(), custMaterial2.ToString());
        }
    }
}