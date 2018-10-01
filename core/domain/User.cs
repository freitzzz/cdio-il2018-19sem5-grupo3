using support.domain;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    /// <summary>
    /// Class that represents a User
    /// <br>User is an Entity</br>
    /// <br>User is an Aggregate Root</br>
    /// </summary>
    /// <typeparam name="Authentication">Generic-Type of the User entity identifier</typeparam>
    public class User : AggregateRoot<Authentication>, DTOAble<GenericDTO> {
        /// <summary>
        /// User's authentication
        /// </summary>
        [Key]
        private readonly Authentication auth;
        /// <summary>
        /// Creates a new User using a username, email and password
        /// </summary>
        /// <param name="auth">instance of Authentication</param>
        public User(Authentication auth) {
            this.auth = auth;
        }
        /// <summary>
        /// Compares User with an Object
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>true if they are equal, false if not</returns>
        override
        public bool Equals(object obj) {
            if (obj == null || !this.GetType().Equals(obj.GetType())) {
                return false;
            } else {
                User user = (User)obj;
                return this.auth.Equals(user.auth);
            }
        }
        /// <summary>
        /// Returns the generated hash code for the user
        /// </summary>
        /// <returns>generated hash code for the user</returns>
        override
        public int GetHashCode() {
            return auth.GetHashCode();
        }
        /// <summary>
        /// Returns the id of the User
        /// </summary>
        /// <returns>user's id</returns>
        public Authentication id() {
            return auth;
        }
        /// <summary>
        /// Checks if the User's identity is the same as a certain User
        /// </summary>
        /// <param name="comparingEntity">Authentication identity of another User</param>
        /// <returns>true if the identities are the same, false if not</returns>
        public bool sameAs(Authentication comparingEntity) {
            return id().Equals(comparingEntity);
        }
        /// <summary>
        /// Returns the User as a DTO
        /// </summary>
        /// <returns>User represented as a DTO</returns>
        public GenericDTO toDTO() {
            GenericDTO dto = new GenericDTO("User");
            dto.put("1", auth);
            return dto;
        }
    }
}
