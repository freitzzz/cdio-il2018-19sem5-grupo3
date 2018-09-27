using System;
using Xunit;
using core.domain;

namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class Finish.
    </summary>
    */
    public class FinishTest
    {
        /**
        <summary>
            Test to ensure that the method GetHashCode works.
         </summary>
         */
        [Fact]
        public void testGetHashCode()
        {
            Finish finish1 = Finish.valueOf("Acabamento polido");
            Finish finish2= Finish.valueOf("Acabamento polido");

            Assert.Equal(finish1.GetHashCode(), finish2.GetHashCode());
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two Finishs with different references.
         </summary>
         */
        [Fact]
        public void testDifferentFinishsAreNotEqual()
        {
            Finish finish1 = Finish.valueOf("Acabamento polido");
            Finish finish2= Finish.valueOf("Acabamento rogoso");

            Assert.False(finish2.Equals(finish1));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two Finishs with the same reference.
         </summary>
         */
        [Fact]
        public void testEqualFinishsAreEqual()
        {
            Finish finish1 = Finish.valueOf("Acabamento polido");
            Finish finish2 = Finish.valueOf("Acabamento polido");

            Assert.True(finish2.Equals(finish1));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a null Finish.
         </summary>
         */
        [Fact]
        public void testNullFinishIsNotEqual()
        {
            Finish finish = Finish.valueOf("Acabamento polido");

            Assert.False(finish.Equals(null));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a Finish and an object of another type.
         </summary>
         */
        [Fact]
        public void testDifferentTypesAreNotEqual()
        {
            Finish finish = Finish.valueOf("Acabamento polido");
            Material moon = new Material("1160912", "No");

            Assert.False(finish.Equals(moon));
        }
        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void testToString()
        {
            Finish finish1 =  Finish.valueOf("Acabamento polido");
            Finish finish2 =  Finish.valueOf("Acabamento polido");

            Assert.Equal(finish1.ToString(), finish2.ToString());
        }

        /**
        <summary>
            Test to ensure that the instance of Finish isn't built if the description is null.
        </summary>
         */
        [Fact]
        public void ensureNullDescriptionIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf(null));
        }

        /**
        <summary>
            Test to ensure that the instance of Finish isn't built if the description is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyDescriptionIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => Finish.valueOf(""));
        }
    }
}
