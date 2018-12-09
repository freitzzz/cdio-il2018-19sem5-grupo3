using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a ModelView used for retrieving data from instances of Customized Product Collection
    /// </summary>
    [DataContract]
    public class GetBasicCustomizedProductCollectionModelView
    {
        /// <summary>
        /// Persistence identifier of the current CustomizedProductCollection
        /// </summary>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// String with the collection name
        /// </summary>
        [DataMember]
        public string name { get; set; }
    }
}