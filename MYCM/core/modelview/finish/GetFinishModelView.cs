using System.Runtime.Serialization;

namespace core.modelview.finish
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of Finish.
    /// </summary>
    [DataContract]
    public class GetFinishModelView
    {
        /// <summary>
        /// Finish's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Finish's persistence identifier.</value>
        [DataMember(Name = "id")]
        public long finishId { get; set; }

        /// <summary>
        /// Finish's description.
        /// </summary>
        /// <value>Gets/Sets the Finish's description.</value>
        [DataMember]
        public string description { get; set; }

        /// <summary>
        /// Finish's shininess.
        /// </summary>
        /// <value>Gets/Sets the Finish's shininess.</value>
        [DataMember]
        public float shininess { get; set; }
    }
}