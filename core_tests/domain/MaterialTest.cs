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
        public void ensureIdMehodWorks()
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
        public void ensureMaterialsWithEqualIdentitiesAreTheSame()
        {
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            Material material = new Material(reference, designation);

            Assert.True(material.sameAs(reference));
        }

        /**
        <summary>
            Test to ensure that the method sameAs works, for two different identities.
         </summary>
         */
        [Fact]
        public void ensureMaterialsWithDifferentIdentitiesAreNotTheSame()
        {
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            string anotherReference = "1160907";

            Material material = new Material(reference, designation);

            Assert.False(material.sameAs(anotherReference));
        }

        /**
        <summary>
            Test to ensure that the method GetHashCode works.
         </summary>
         */
        [Fact]
        public void ensureGetHashCodeWorks()
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
        public void ensureMaterialsWithDifferentReferencesAreNotEqual()
        {
            Material salt = new Material("1160912", "Guru");
            Material pepper = new Material("1160907", "Velhinho");

            Assert.False(salt.Equals(pepper));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for two Materials with the same reference.
         </summary>
         */
        [Fact]
        public void ensureMaterialsWithSameReferencesAreEqual()
        {
            Material ping = new Material("1160912", "Ping");
            Material pong = new Material("1160912", "Pong");

            Assert.True(ping.Equals(pong));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a null Material.
         </summary>
         */
        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            Material loner = new Material("1160912", "John Snow");

            Assert.False(loner.Equals(null));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a Material and an object of another type.
         </summary>
         */
        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            Material moon = new Material("1160912", "No");
            Product stars = new Product("1160907", "Yes");

            Assert.False(moon.Equals(stars));
        }

        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void ensureToStringWorks()
        {
            Material balsamic = new Material("1160912", "Cowboy Boots");
            Material vinegar = new Material("1160912", "Cowboy Boots");

            Assert.Equal(balsamic.ToString(), vinegar.ToString());
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the reference is null.
        </summary>
         */
        [Fact]
        public void ensureNullReferenceIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new Material(null, "This doesn't work"));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the reference is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyReferenceIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new Material("", "Let me see..."));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the designation is null.
        </summary>
       */
        [Fact]
        public void ensureNullDesignationIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new Material("Have you tried turning it off and then on again?", null));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the designation is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyDesignationIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new Material("Still not working", ""));
        }
    }
}
