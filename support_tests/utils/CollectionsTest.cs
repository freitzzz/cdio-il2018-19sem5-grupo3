using System;
using Xunit;
using support.utils;
using System.Collections.Generic;

namespace support_tests.utils
{
    /**
    <summary>
      Unit testing class for class Lists.
    </summary>
    */
    public class CollectionsTest
    {

        /**
        <summary>
            Test to ensure that the method isListEmpty detects an empty list.
        </summary>
         */
        [Fact]
        public void ensureIsListEmptyDetectsEmptyList()
        {
            Console.WriteLine("ensureIsListEmptyDetectsEmptyList");

            List<Object> test = new List<Object>();

            Assert.True(Collections.isListEmpty(test));
        }

        /**
        <summary>
            Test to ensure that the method isListEmpty detects a null list.
        </summary>
         */
        [Fact]
        public void ensureIsListEmptyDetectsNullList()
        {
            Console.WriteLine("ensureIsListEmptyDetectsNullObject");

            List<Object> test = null;

            Assert.False(Collections.isListEmpty(test));
        }

        /**
        <summary>
            Test to ensure that the method isListNull detects a null list.
        </summary>
         */
        [Fact]
        public void ensureIsListNullDetectsNullList()
        {
            Console.WriteLine("ensureIsListNullDetectsNullList");

            List<Object> test = null;

            Assert.True(Collections.isListNull(test));
        }

        /**
        <summary>
            Test to ensure that the method isEnumerableNullOrEmpty detects an empty enumerable.
        </summary>
         */
        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsEmptyEnumerable()
        {
            Console.WriteLine("ensureIsEnumerableNullOrEmptyDetectsEmptyEnumerable");

            IEnumerable<Object> test = new List<Object>();

            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }

        /**
        <summary>
            Test to ensure that the method isEnumerableNullOrEmpty detects a null enumerable.
        </summary>
         */
        [Fact]
        public void ensureIsEnumerableNullOrEmptyDetectsNullEnumerable()
        {
            Console.WriteLine("ensureIsEnumerableNullOrEmptyDetectsNullEnumerable");

            IEnumerable<Object> test = null;

            Assert.True(Collections.isEnumerableNullOrEmpty(test));
        }
    }
}