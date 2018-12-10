using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a ModelView used for retrieving all instances of Customized Product Collection
    /// </summary>
    [DataContract]
    public class GetAllCustomizedProductCollectionsModelView
    {
        /// <summary>
        /// List of model views representing basic info about a customized product collection
        /// </summary>
        [DataMember(Name = "customizedProductCollections")]
        public List<GetBasicCustomizedProductCollectionModelView> customizedProductCollections { get; set; }
    }
}