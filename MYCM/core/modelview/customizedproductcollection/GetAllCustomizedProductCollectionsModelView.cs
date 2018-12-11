using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a ModelView used for retrieving all instances of Customized Product Collection
    /// </summary>
    [CollectionDataContract]
    public class GetAllCustomizedProductCollectionsModelView : List<GetBasicCustomizedProductCollectionModelView> { }
}