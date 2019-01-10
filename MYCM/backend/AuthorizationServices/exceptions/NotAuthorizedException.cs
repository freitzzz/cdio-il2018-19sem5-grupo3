using System;
using System.Runtime.Serialization;

namespace backend.AuthorizationServices.exceptions
{

    /// <summary>
    /// Class representing the exception throw when a user is not authorized to perform a certain action.
    /// </summary>
    public class NotAuthorizedException : Exception
    {
        /// <summary>
        /// Creates a new instance of NotAuthorizedException.
        /// </summary>
        public NotAuthorizedException()
        {
        }

        /// <summary>
        /// Creates a new instance of NotAuthorizedException with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotAuthorizedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotAuthorizedException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotAuthorizedException class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.</param>
        protected NotAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}