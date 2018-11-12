using System.Runtime.Serialization;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Model View that represents the basic info of a Customized Product
    /// </summary>
    [DataContract]
    public class BasicCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProducts Identifier
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Identifier of the Product that the Customized Product is built off of
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember]
        public long productId { get; set; }

        /// <summary>
        /// CustomizedProducts designation
        /// </summary>
        /// <value>Gets/Sets the designation</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// CustomizedProducts reference
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember]
        public string reference { get; set; }
    }
}