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
        [DataMember(Order = 0)]
        public long id { get; set; }

        /// <summary>
        /// String with the collection name
        /// </summary>
        [DataMember(Order = 1)]
        public string name { get; set; }

        /// <summary>
        /// List with basic model views of the collection's products
        /// </summary>
        [DataMember(EmitDefaultValue = false, Order = 2)]
        public List<GetBasicCustomizedProductModelView> customizedProducts;
    }
}