using support.domain.ddd;
using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    /// <summary>
    /// Class that represents an Email
    /// <br>Email is a Value Object</br>
    /// </summary>
    public class Email : ValueObject {
        /// <summary>
        /// Constant that represents the message that occurs if the user's email is invalid
        /// </summary>
        private static readonly string INVALID_EMAIL = "Email is invalid";
        /// <summary>
        /// String containing the email
        /// </summary>
        private string email;
        /// <summary>
        /// Creates a new isntance of Email using a string containing an email
        /// </summary>
        /// <param name="email">string containing an email</param>
        private Email(string email) {
            checkEmailProperties(email);
        }
        /// <summary>
        /// Creates a new instance of Email using a string containing an email
        /// </summary>
        /// <param name="email">string containing an email</param>
        /// <returns>instance of Email</returns>
        public static Email valueOf(string email) {
            return new Email(email);
        }
        /// <summary>
        /// Checks if the Email's properties are valid
        /// </summary>
        /// <param name="email">string containing an email</param>
        private void checkEmailProperties(string email) {
            if (Strings.isNullOrEmpty(email)) {
                throw new ArgumentException(INVALID_EMAIL);
            }
        }
    }
}
