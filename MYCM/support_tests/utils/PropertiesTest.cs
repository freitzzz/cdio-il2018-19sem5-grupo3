using System;
using Xunit;
using support.utils;
using System.Collections.Generic;
using support.system;

namespace support_tests.utils
{
    public class PropertiesTest
    {
         [Fact]
         public void ensurePut()
        {
                Properties p1 = new Properties();
                Object obj = new Object();
                Object obj1 = new Object();

                p1.put(obj,obj1);

                Assert.True(String.Equals(p1.get(obj),obj1));
        }
    }
}