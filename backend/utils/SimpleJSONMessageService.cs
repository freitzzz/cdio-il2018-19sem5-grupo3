namespace backend.utils{
    /// <summary>
    /// Simples service for representing messages in JSON
    /// </summary>
    public sealed class SimpleJSONMessageService{
        /// <summary>
        /// String with the message
        /// </summary>
        public string message{get;set;}
        /// <summary>
        /// Builds a new JSONMessageService with the message being represented in JSON
        /// </summary>
        /// <param name="message">String with the message</param>
        public SimpleJSONMessageService(string message){this.message=message;}
    }
}