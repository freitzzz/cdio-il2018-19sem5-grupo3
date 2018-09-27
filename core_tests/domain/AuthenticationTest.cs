using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace core_tests.domain {
    /// <summary>
    /// Test class of Authentication class
    /// </summary>
    public class AuthenticationTest {
        /// <summary>
        /// tets of method Equals, of class Authentication
        /// </summary>
        [Fact]
        public void testEquals() {
            Console.WriteLine("Equals");
            Authentication auth1 = new Auth("username", "email", "password");
            Authentication auth2 = new Auth("username", "email", "password");
            Assert.True(auth1.Equals(auth1));
            Assert.False(auth1.Equals(auth2));
            Assert.False(auth1.Equals(null));
            Assert.False(auth1.Equals(new User(auth1)));
        }
        /// <summary>
        /// test of method GetHashCode, of class Authentication
        /// </summary>
        [Fact]
        public void testGetHashCode() {
            Console.WriteLine("GetHashCode");
            Authentication auth1 = new Auth("username", "email", "password");
            Authentication auth2 = new Auth("username", "email", "password");
            Assert.False(auth1.GetHashCode().Equals(auth2.GetHashCode()));
        }
    }
}
