using System;
using core.domain;
using support.dto;
using Xunit;
namespace core_tests.domain {
    /// <summary>
    /// Test class of User class
    /// </summary>
    public class UserTest {
        /// <summary>
        /// test of Equals method, of class User
        /// </summary>
        [Fact]
        public void testEquals() {
            Console.WriteLine("Equals");
            Authentication auth1 = new Auth("username", "email", "password");
            User u1 = new User(auth1);
            User u2 = new User(auth1);
            Assert.True(u1.Equals(u1)); ///same instance
            Assert.False(u1.Equals(null)); ///null parameter
            Assert.False(u1.Equals(new User(new Auth("username2", "email2", "password2")))); ///different id
            Assert.False(u1.Equals(auth1)); ///different types
            Assert.True(u1.Equals(u2)); ///different instances, same id
        }
        /// <summary>
        /// test of GetHashCode method, of class User
        /// </summary>
        [Fact]
        public void testGetHashCode() {
            Console.WriteLine("GetHashCode");
            Authentication auth1 = new Auth("username", "email", "password");
            User u1 = new User(auth1);
            Assert.True(u1.GetHashCode().Equals(auth1.GetHashCode())); /// User hash code should be same as its identity hash code
            Assert.False(u1.GetHashCode().Equals(new Auth("username2", "email2", "password2")));
        }
        /// <summary>
        /// test of sameAs method, of class User
        /// </summary>
        [Fact]
        public void testSameAs() {
            Console.WriteLine("sameAs");
            Authentication auth1 = new Auth("username", "email", "password");
            User u1 = new User(auth1);
            User u2 = new User(auth1);
            Assert.True(u1.sameAs(auth1));
            Assert.False(u1.sameAs(new Auth("username2", "email2", "password2")));
        }
        /// <summary>
        /// test of id method, of class User
        /// </summary>
        [Fact]
        public void testID() {
            Console.WriteLine("id");
            Authentication auth1 = new Auth("username", "email", "password");
            User u1 = new User(auth1);
            Assert.True(auth1.Equals(u1.id()));
            Assert.False(new Auth("username2", "email2", "password2").Equals(u1.id()));
        }
        /// <summary>
        /// test of toDTO method, of class User
        /// </summary>
        [Fact]
        public void testToDTO() {
            Console.WriteLine("toDTO");
            Authentication auth1 = new Auth("username", "email", "password");
            User u1 = new User(auth1);
            DTO dto = new GenericDTO("User");
            dto.put("1", auth1);
            Assert.True(dto.get("1").Equals(u1.toDTO().get("1")));
        }
    }
}
