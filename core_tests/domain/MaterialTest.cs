using System;
using Xunit;
using core.domain;

namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class Material.
    </summary>
    */
    public class MaterialTest
    {
        /**
        <summary>
            Test to ensure that the method id works.
         </summary>
         */
        [Fact]
        public void testId()
        {
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            Material material = new Material(reference, designation);

            Assert.Equal(material.id(), reference, true);
        }

        /**
        <summary>
            Test to ensure that the method sameAs works, for two equal identities.
         </summary>
         */
        [Fact]
        public void testSameForSameIdentity()
        {
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            Material material = new Material(reference, designation);

            Assert.Equal(material.sameAs("1160912"), true);
        }

        /**
        <summary>
            Test to ensure that the method sameAs works, for two different identities.
         </summary>
         */
        [Fact]
        public void tesNotSameForDifferentIdentity()
        {
            Material material = new Material("1160912", "FR E SH A VOCA DO");

            Assert.Equal(material.sameAs("1160907"), true);
        }

        /**
        <summary>
            Test to ensure that the method GetHashCode works.
         </summary>
         */
        [Fact]
        public void testGetHashCode()
        {
            Material balsamic = new Material("1160912", "Cowboy Boots");
            Material vinegar = new Material("1160912", "Cowboy Boots");

            Assert.Equal(balsamic.GetHashCode(), vinegar.GetHashCode());
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for two Materials with different references.
         </summary>
         */
        [Fact]
        public void testDifferentMaterialsAreNotEqual()
        {
            Material salt = new Material("1160912", "Guru");
            Material pepper = new Material("1160907", "Velhinho");

            Assert.NotEqual(salt.Equals(pepper), true);
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for two Materials with the same reference.
         </summary>
         */
        [Fact]
        public void testEqualMaterialsAreEqual()
        {
            Material ping = new Material("1160912", "Ping");
            Material pong = new Material("1160912", "Pong");

            Assert.NotEqual(ping.Equals(pong), true);
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a null Material.
         </summary>
         */
        [Fact]
        public void testNullMaterialIsNotEqual()
        {
            Material loner = new Material("1160912", "John Snow");

            Assert.NotEqual(loner.Equals(null), true);
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a Material and an object of another type.
         </summary>
         */
        [Fact]
        public void testDifferentTypesAreNotEqual()
        {
            Material moon = new Material("1160912", "No");

            Assert.NotEqual(moon.Equals("stars"), true);
        }

        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void testToString()
        {
            Material balsamic = new Material("1160912", "Cowboy Boots");
            Material vinegar = new Material("1160912", "Cowboy Boots");

            Assert.Equal(balsamic.GetHashCode(), vinegar.GetHashCode());
        }
    }
}
