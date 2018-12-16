using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing the ModelView used for retrieving multiple instances of CommercialCatalogue.
    /// </summary>
    [DataContract]
    public class GetAllCommercialCataloguesModelView : List<GetBasicCommercialCatalogueModelView>
    {

    }
}