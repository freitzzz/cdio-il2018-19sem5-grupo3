using support.domain.ddd;
using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    /// <summary>
    /// Class that represents a Password
    /// <br>Password is a Value Object</br>
    /// </summary>
    public class Password : ValueObject {
        /// <summary>
        /// Constant that represents the message that occurs if the user's password is invalid
        /// </summary>
        private static readonly string INVALID_PASSWORD = "Password is invalid";
        /// <summary>
        /// String containing an encrypted password
        /// </summary>
        private string password;
        /// <summary>
        /// Creates a new instance of Password using a string containing a password
        /// </summary>
        /// <param name="password">string containing a password</param>
        private Password(string password) {
            checkPasswordProperties(password);
            /// check length and encrypt
            this.password = password;
        }
        /// <summary>
        /// Creates a new instance of Password using a string containing a password
        /// </summary>
        /// <param name="password">string containing a password</param>
        /// <returns>new instance of Password</returns>
        public static Password valueOf(string password) {
            return new Password(password);
        }
        /// <summary>
        /// Checks if the Password's properties are valid
        /// </summary>
        /// <param name="password">string containing a password</param>
        private void checkPasswordProperties(string password) {
            if (Strings.isNullOrEmpty(password)) {
                throw new ArgumentException(INVALID_PASSWORD);
            }
        }
    }
}
