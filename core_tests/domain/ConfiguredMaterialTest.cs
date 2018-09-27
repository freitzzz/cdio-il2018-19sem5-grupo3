using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class ConfiguredMaterial.
    </summary>
    */
    public class ConfiguredMaterialTest
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
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color,finish);
            ConfiguredMaterial congMaterial2 = ConfiguredMaterial.valueOf(color,finish);

            Assert.Equal(congMaterial1.GetHashCode(), congMaterial2.GetHashCode());
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two ConfiguredMaterial with different colors.
         </summary>
         */
        [Fact]
        public void testDifferentConfMaterialColorAreNotEqual()
        {
            Color color1 = Color.valueOf("Azul",1,1,1,1);
            Color color2 = Color.valueOf("Rosa",2,2,2,2);
            Finish finish = Finish.valueOf("Acabamento polido");
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color1,finish);
            ConfiguredMaterial congMaterial2 = ConfiguredMaterial.valueOf(color2,finish);

            Assert.False(congMaterial1.Equals(congMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two ConfiguredMaterial with different finish.
         </summary>
         */
        [Fact]
        public void testDifferentConfigMaterialFinishAreNotEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish1 = Finish.valueOf("Acabamento polido");
            Finish finish2 = Finish.valueOf("Acabamento rogoso");
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color,finish1);
            ConfiguredMaterial congMaterial2 = ConfiguredMaterial.valueOf(color,finish2);

            Assert.False(congMaterial1.Equals(congMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two ConfiguredMaterial with the same color and finish.
         </summary>
         */
        [Fact]
        public void testEqualConfMaterialAreEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color,finish);
            ConfiguredMaterial congMaterial2 = ConfiguredMaterial.valueOf(color,finish);

            Assert.True(congMaterial1.Equals(congMaterial2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a null ConfiguredMaterial.
         </summary>
         */
        [Fact]
        public void testNullConfiguredMaterialIsNotEqual()
        {
            Color color = Color.valueOf("Azul",1,1,1,1);
            Finish finish = Finish.valueOf("Acabamento polido");
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color,finish);

            Assert.False(congMaterial1.Equals(null));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a ConfiguredMaterial and an object of another type.
         </summary>
         */
        [Fact]
        public void testDifferentTypesAreNotEqual()
        {

            
            Finish finish = Finish.valueOf("Acabamento polido");
            Color color = Color.valueOf("Azul",1,1,1,1);
            ConfiguredMaterial congMaterial = ConfiguredMaterial.valueOf(color,finish);

            Assert.False(finish.Equals(congMaterial));
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
            ConfiguredMaterial congMaterial1 = ConfiguredMaterial.valueOf(color,finish);
            ConfiguredMaterial congMaterial2 = ConfiguredMaterial.valueOf(color,finish);

            Assert.Equal(congMaterial1.ToString(), congMaterial2.ToString());
        }
    }
}