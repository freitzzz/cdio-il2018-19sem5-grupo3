using System;
using System.Runtime.Serialization;

namespace core.exceptions
{
    /// <summary>
    /// Represents the Exception that is thrown when a resource could not be found.
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of ResourceNotFoundException.
        /// </summary>
        public ResourceNotFoundException()
        {
        }

        /// <summary>
        /// Creates a new instance of ResourceNotFoundException with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns></returns>
        public ResourceNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ResourceNotFoundException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the innerException parameter is not a null reference, 
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        /// <returns></returns>
        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the System.Exception class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.</param>
        /// <returns></returns>
        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}