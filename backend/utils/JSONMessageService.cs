namespace backend.utils{
    /// <summary>
    /// Simples service for representing messages in JSON
    /// </summary>
    public sealed class JSONMessageService{
        /// <summary>
        /// String with the message
        /// </summary>
        public string message{get;set;}
        /// <summary>
        /// Builds a new JSONMessageService with the message being represented in JSON
        /// </summary>
        /// <param name="message">String with the message</param>
        public JSONMessageService(string message){this.message=message;}
    }
}