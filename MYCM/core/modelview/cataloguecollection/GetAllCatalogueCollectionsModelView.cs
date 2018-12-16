using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an IEnumerable of CatalogueCollection.
    /// </summary>
    [CollectionDataContract]
    public class GetAllCatalogueCollectionsModelView : List<GetBasicCatalogueCollectionModelView>
    {

    }
}