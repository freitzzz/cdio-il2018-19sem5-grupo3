using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of CatalogueCollection.
    /// </summary>
    [DataContract]
    public class GetCatalogueCollectionModelView : GetBasicCatalogueCollectionModelView
    {
        /// <summary>
        /// GetAllCustomizedProductsModelView representing all of the CatalogueCollection's CustomizedProducts.
        /// </summary>
        /// <value>Gets/Sets the instance of GetAllCustomizedProductsModelView.</value>
        [DataMember(Order = 2)]
        public GetAllCustomizedProductsModelView customizedProducts { get; set; }
    }
}