using System;
using Xunit;
using support.utils;
using System.Collections.Generic;

namespace support_tests.utils
{
    /// <summary>
    /// Unit testing class for class Lists.
    /// </summary>
    public class CollectionsTest
    {
        /// <summary>
        /// Test to ensure that the method isListEmpty detects an empty list.
        /// </summary>
        [Fact]
        public void ensureIsListEmptyDetectsEmptyList()
        {
            Console.WriteLine("ensureIsListEmptyDetectsEmptyList");

            List<Object> test = new List<Object>();

            Assert.True(Collections.isListEmpty(test));
        }

        /// <summary>
        /// Test to ensure that the method isListEmpty detects a null list.
        /// </summary>
        [Fact]
        public void ensureIsListEmptyDetectsNullList()
        {
            Console.WriteLine("ensureIsListEmptyDetectsNullList");

            List<Object> test = null;

            Assert.True(Collections.isListEmpty(test));
        }

        /// <summary>
        /// Test to ensure that the method isListNull detects a null list.
        /// </summary>
        [Fact]
        public void ensureIsListNullDetectsNullList()
        {
            Console.WriteLine("ensureIsListNullDetectsNullList");

            List<Object> test = null;

            Assert.True(Collections.isListNull(test));
        }

        /// <summary>
        ///Test to ensure that the method isEnumerableNullOrEmpty detects an empty enumerable.
        /// </summary>
        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsEmptyEnumerable()
        {
            Console.WriteLine("ensureIsEnumerableNullOrEmptyDetectsEmptyEnumerable");

            IEnumerable<Object> test = new List<Object>();

            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }

        /// <summary>
        /// Test to ensure that the method isEnumerableNullOrEmpty detects a null enumerable.
        /// </summary>
        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsNullEnumerable()
        {
            Console.WriteLine("ensureIsEnumerableNullOrEmptyDetectsNullEnumerable");

            IEnumerable<Object> test = null;

            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }

        /// <summary>
        /// Test to ensure that the expected size is returned for a list.
        /// </summary>
        [Fact]
        public void ensureGetEnumerableSizeWorksForList(){
            Console.WriteLine("ensureGetEnumerableSizeWorksForList");

            List<string> test = new List<string>();
            test.Add("The cake is a lie");

            Assert.Equal(1, Collections.getEnumerableSize(test));
        }

        /// <summary>
        /// Test to ensure that the expected size is returned for an enumerable other than a list.
        /// </summary>
        [Fact]
        public void ensureGetEnumerableSizeWorksForAnEnumerableOtherThanAList(){
            Console.WriteLine("ensureGetEnumerableSizeWorksForAnEnumerableOtherThanAList");

            HashSet<string> test = new HashSet<string>();
            test.Add("The cake is a lie");

            Assert.Equal(1, Collections.getEnumerableSize(test));
        }

        [Fact]
        public void ensureEnumerableAsListWorksForList(){
            Console.WriteLine("ensureEnumerableAsListWorksForList");

            List<string> test = new List<string>();
            test.Add("The cake is a lie");

            Assert.Equal(test, Collections.enumerableAsList(test));
        }

        [Fact]
        public void ensureEnumerableAsListWorksForAnEnumerableOtherThanAList(){
            Console.WriteLine("ensureEnumerableAsListWorksForAnEnumerableOtherThanAList");

            HashSet<string> test = new HashSet<string>();
            List<string> expected = new List<string>();

            string element = "The cake is a lie";
            test.Add(element);
            expected.Add(element);

            Assert.Equal(expected, Collections.enumerableAsList(test));
        }
    }
}