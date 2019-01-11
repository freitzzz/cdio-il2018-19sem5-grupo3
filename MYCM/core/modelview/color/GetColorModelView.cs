using System.Runtime.Serialization;

namespace core.modelview.color
{
    /// <summary>
    /// Class representing the ModelView used for retrieving Color data.
    /// </summary>
    [DataContract]
    public class GetColorModelView
    {
        /// <summary>
        /// Color's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [DataMember(Name = "id")]
        public long colorId { get; set; }

        /// <summary>
        /// Color's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Color's red value.
        /// </summary>
        /// <value>Gets/Sets the red value.</value>
        [DataMember]
        public byte red { get; set; }

        /// <summary>
        /// Color's green value.
        /// </summary>
        /// <value>Gets/Sets the green value.</value>
        [DataMember]
        public byte green { get; set; }

        /// <summary>
        /// Color's blue value.
        /// </summary>
        /// <value>Gets/Sets the blue value.</value>
        [DataMember]
        public byte blue { get; set; }

        /// <summary>
        /// Color's alpha value.
        /// </summary>
        /// <value>Gets/Sets the alpha value.</value>
        [DataMember]
        public byte alpha { get; set; }
    }
}