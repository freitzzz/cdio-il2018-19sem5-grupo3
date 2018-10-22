using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.utils
{
    /// <summary>
    /// Static class used for formatting messages to JSON.
    /// </summary>
    [Obsolete("This class does not truly convert strings into json when responses are being sent, either use anonymous type or create POCO's to send messages")]
    public static class JSONStringFormatter
    {

        /// <summary>
        /// Enum containing all available message types.
        /// </summary>
        public enum MessageTypes
        {
            //*Add other types of messages here 
            ERROR_MSG
        }

        /// <summary>
        /// Dictionary that maps MessageTypes to their respective string output.
        /// </summary>
        /// <value></value>
        private static Dictionary<MessageTypes, string> messageKeys = new Dictionary<MessageTypes, string>{
            {MessageTypes.ERROR_MSG, "error"}
        };


        /// <summary>
        /// Formats the given message to JSON format.
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="message">The message itself</param>
        /// <returns>The message in JSON format.</returns>
        public static string formatMessageToJson(MessageTypes type, string message)
        {    
            string json = string.Format("{{\"{0}\": \"{1}\"}}", messageKeys[type], message);

            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);

            return jsonFormatted;
        }

    }
}