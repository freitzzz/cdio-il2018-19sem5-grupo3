using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of Customized Product Collection
    /// </summary>
    [DataContract]
    public class GetCustomizedProductCollectionModelView
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

        /// <summary>
        /// List with basic model views of the collection's products
        /// </summary>
        [DataMember]
        public List<GetBasicCustomizedProductModelView> customizedProducts;
    }
}