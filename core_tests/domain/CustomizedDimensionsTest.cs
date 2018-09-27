using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class CustomizedDimensions.
    </summary>
    */
    public class CustomizedDimensionsTest
    {
        /**
        <summary>
            Test to ensure that the method GetHashCode works.
         </summary>
         */
        [Fact]
        public void testGetHashCode()
        { 
            CustomizedDimensions cuntDimensions1 = CustomizedDimensions.valueOf(15.5,10,4.3);
            CustomizedDimensions cuntDimensions2 = CustomizedDimensions.valueOf(15.5,10,4.3);

            Assert.Equal(cuntDimensions1.GetHashCode(), cuntDimensions2.GetHashCode());
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedDimensions with different heigth.
         </summary>
         */
        [Fact]
        public void testDifferentCustDimensionsHeightAreNotEqual()
        {
            CustomizedDimensions custDimensions1 = CustomizedDimensions.valueOf(15.1,3,2);
            CustomizedDimensions custDimensions2 = CustomizedDimensions.valueOf(15.2,3,2);

            Assert.False(custDimensions1.Equals(custDimensions2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedDimensions with different width.
         </summary>
         */
        [Fact]
        public void testDifferentCustDimensionsWidthAreNotEqual()
        {
            CustomizedDimensions custDimensions1 = CustomizedDimensions.valueOf(15.1,3.1,2);
            CustomizedDimensions custDimensions2 = CustomizedDimensions.valueOf(15.1,3,2);

            Assert.False(custDimensions1.Equals(custDimensions2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedDimensions with different depth.
         </summary>
         */
        [Fact]
        public void testDifferentCustDimensionsDepthAreNotEqual()
        {
            CustomizedDimensions custDimensions1 = CustomizedDimensions.valueOf(15.1,3,2);
            CustomizedDimensions custDimensions2 = CustomizedDimensions.valueOf(15.1,3,2.1);

            Assert.False(custDimensions1.Equals(custDimensions2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for two CustomizedDimensions with different depth.
         </summary>
         */
        [Fact]
        public void testEqualCustDimensionsAreEqual()
        {
            CustomizedDimensions custDimensions1 = CustomizedDimensions.valueOf(15.1,3,2.1);
            CustomizedDimensions custDimensions2 = CustomizedDimensions.valueOf(15.1,3,2.1);

            Assert.True(custDimensions1.Equals(custDimensions2));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a null CustomizedDimensions.
         </summary>
         */
        [Fact]
        public void testNullCustomizedDimensionsIsNotEqual()
        {
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.1,3,2.1);

            Assert.False(custDimensions.Equals(null));
        }
        /**
        <summary>
            Test to ensure that the method Equals works, for a CustomizedDimensions and an object of another type.
         </summary>
         */
        [Fact]
        public void testDifferentTypesAreNotEqual()
        {

            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.1,3,2.1);

            Assert.False(finish.Equals(custDimensions));
        }
        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void testToString()
        {
            CustomizedDimensions custDimensions1 = CustomizedDimensions.valueOf(15.1,3,2.1);
            CustomizedDimensions custDimensions2 = CustomizedDimensions.valueOf(15.1,3,2.1);

            Assert.Equal(custDimensions1.ToString(), custDimensions2.ToString());
        }
    }
}