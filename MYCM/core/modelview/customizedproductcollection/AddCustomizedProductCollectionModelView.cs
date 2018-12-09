using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a model view for adding a new customized product collection
    /// </summary>
    [DataContract]
    public class AddCustomizedProductCollectionModelView
    {
        /// <summary>
        /// Customized Product Collection's name
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }

        /// <summary>
        /// Customized Product Collection's list of customized products
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "customizedProducts")]
        public List<GetBasicCustomizedProductModelView> customizedProducts;
    }
}