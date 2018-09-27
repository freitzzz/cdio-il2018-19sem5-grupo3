using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    public class Auth : Authentication {
        /// <summary>
        /// Constant that represents the message that occurs if the user's username is invalid
        /// </summary>
        private static readonly string INVALID_USERNAME = "Username is invalid";
        /// <summary>
        /// String with the user's username
        /// </summary>
        private string username;
        /// <summary>
        /// User's email
        /// </summary>
        private Email email;
        /// <summary>
        /// User's password
        /// </summary>
        private Password password;
        /// <summary>
        /// Creates a new User using a username, email and password
        /// </summary>
        /// <param name="username">string with the user's username</param>
        /// <param name="email">string with the user's email</param>
        /// <param name="password">string with the user's password</param>
        public Auth(string username, string email, string password) : base() {
            checkAuthProperties(username);
            this.username = username;
            this.email = Email.valueOf(email);
            this.password = Password.valueOf(password);
        }

        public override bool authenticate() {
            throw new NotImplementedException();
        }

        protected override string generateToken() {
            Random rnd = new Random();
            return "token" + rnd.Next(1, 1000); /// must be changed to actual decent generation
        }

        /// <summary>
        /// Checks if the user's properties are valid
        /// </summary>
        /// <param name="username">string with the user's username</param>
        /// <param name="email">string with the user's email</param>
        /// <param name="password">string with the user's password</param>
        private void checkAuthProperties(string username) {
            if (Strings.isNullOrEmpty(username)) {
                throw new ArgumentException(INVALID_USERNAME);
            }
        }
    }
}
