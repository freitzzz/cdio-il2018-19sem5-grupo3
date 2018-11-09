using System;
using Xunit;
using support.utils;

namespace support_tests.utils
{
    public class StringsTest
    {
        [Fact]
        public void ensureIsEmpty(){
            String test = "";
            Assert.True(Strings.isNullOrEmpty(test));
        }

        [Fact]
        public void ensureIsNull(){
            string  test = null;
            Assert.True(Strings.isNullOrEmpty(test));
        }

        [Fact]
        public void ensureIsEitherNullOrEmpty(){
            string  test = "n";
            Assert.False(Strings.isNullOrEmpty(test));
        }

    }
}