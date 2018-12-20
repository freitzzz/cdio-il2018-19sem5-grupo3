using System.Runtime.Serialization;

namespace core.modelview.input
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an Algorithm's input.
    /// </summary>
    [DataContract]
    public class GetInputModelView
    {
        /// <summary>
        /// Input's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Input's value range.
        /// </summary>
        /// <value>Gets/Sets the value range.</value>
        [DataMember]
        public string range { get; set; }
    }
}