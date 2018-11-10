using System.Runtime.Serialization;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of Customized Product
    /// </summary>
    [DataContract]

    public class GetCustomizedProductModelView
    {
        /// <summary>
        /// Long that represents the CustomizedProduct's persistence ID.
        /// </summary>
        [DataMember]
        public long id { get; internal set; }

        /// <summary>
        /// String with the CustomizedProduct's reference
        /// </summary>
        [DataMember]
        public string reference { get; protected set; }

        /// <summary>
        /// String with the CustomizedProduct's designation
        /// </summary>
        [DataMember]
        public string designation { get; protected set; }
        
    }
}