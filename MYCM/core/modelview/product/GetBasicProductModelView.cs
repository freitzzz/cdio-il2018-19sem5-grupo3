using System.Runtime.Serialization;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for retrieving Products' basic information.
    /// </summary>
    [DataContract]
    public class GetBasicProductModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        [DataMember(Name = "id")]
        public long productId { get; set; }

        /// <summary>
        /// Product's reference.
        /// </summary>
        /// <value>Gets/sets the Product's reference.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Product's designation.
        /// </summary>
        /// <value>Gets/sets the Product's designation.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// Product's model's filename.
        /// </summary>
        /// <value>Gets/sets the model's filename.</value>
        [DataMember(Name = "model")]
        public string modelFilename { get; set; }

        /// <summary>
        /// Boolean indicating whether or not the Product supports slots.
        /// </summary>
        /// <value>Gets/sets the flag.</value>
        [DataMember]
        public bool supportsSlots { get; set; }

        /// <summary>
        /// Boolean indicating whether or not the Product has any components.
        /// </summary>
        /// <value>Gets/sets the flag.</value>
        [DataMember]
        public bool hasComponents { get; set; }
    }
}