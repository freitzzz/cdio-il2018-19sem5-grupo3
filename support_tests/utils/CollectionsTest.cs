using System;
using Xunit;
using support.utils;
using System.Collections.Generic;

namespace support_tests.utils
{
    /// <summary>
    /// Unit testing class for class Collections.
    /// </summary>
    public class CollectionsTest
    {
        [Fact]
        public void ensureIsListEmptyDetectsEmptyList()
        {
            List<Object> test = new List<Object>();
            Assert.True(Collections.isListEmpty(test));
        }

        [Fact]
        public void ensureIsListEmptyDetectsNullList()
        {
            List<Object> test = null;
            Assert.True(Collections.isListEmpty(test));
        }

        [Fact]
        public void ensureIsListNullDetectsNullList()
        {
            List<Object> test = null;
            Assert.True(Collections.isListNull(test));
        }

        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsEmptyEnumerable()
        {
            IEnumerable<Object> test = new List<Object>();
            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }

        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsNullEnumerable()
        {
            IEnumerable<Object> test = null;
            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }

        [Fact]
        public void ensureGetEnumerableSizeWorksForList()
        {
            List<string> test = new List<string>();
            test.Add("The cake is a lie");
            Assert.Equal(1, Collections.getEnumerableSize(test));
        }

        [Fact]
        public void ensureGetEnumerableSizeWorksForAnEnumerableOtherThanAList()
        {
            HashSet<string> test = new HashSet<string>();
            test.Add("The cake is a lie");
            Assert.Equal(1, Collections.getEnumerableSize(test));
        }

        [Fact]
        public void ensureEnumerableAsListWorksForList()
        {
            List<string> test = new List<string>();
            test.Add("The cake is a lie");
            Assert.Equal(test, Collections.enumerableAsList(test));
        }

        [Fact]
        public void ensureEnumerableAsListWorksForAnEnumerableOtherThanAList()
        {
            HashSet<string> test = new HashSet<string>();
            List<string> expected = new List<string>();

            string element = "The cake is a lie";
            test.Add(element);
            expected.Add(element);

            Assert.Equal(expected, Collections.enumerableAsList(test));
        }
    }
}