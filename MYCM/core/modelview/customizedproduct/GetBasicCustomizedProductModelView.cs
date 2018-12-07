using System.Runtime.Serialization;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Model View that represents the basic info of a Customized Product
    /// </summary>
    [DataContract]
    public class GetBasicCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProducts Identifier
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "id")]
        public long customizedProductId { get; set; }

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
        [DataMember(EmitDefaultValue = false)] //since this value is optional, do not display null values
        public string designation { get; set; }

        /// <summary>
        /// CustomizedProducts reference
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember(EmitDefaultValue = false)]  //if serial number is set, reference is null
        public string reference { get; set; }

        /// <summary>
        /// CustomizedProduct's serial number.
        /// </summary>
        /// <value></value>
        [DataMember(EmitDefaultValue = false)]  //if reference is set, serial number is null
        public string serialNumber { get; set; }
    }
}