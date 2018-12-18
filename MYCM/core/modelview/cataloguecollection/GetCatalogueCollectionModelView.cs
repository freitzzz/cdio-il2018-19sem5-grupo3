using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of CatalogueCollection.
    /// </summary>
    [DataContract]
    public class GetCatalogueCollectionModelView
    {
        /// <summary>
        /// CustomizedProductCollection's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [DataMember(Name = "id")]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// CustomizedProductCollection's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// GetAllCustomizedProductsModelView representing all of the CatalogueCollection's CustomizedProducts.
        /// </summary>
        /// <value>Gets/Sets the instance of GetAllCustomizedProductsModelView.</value>
        [DataMember(EmitDefaultValue = false)]
        public GetAllCustomizedProductsModelView customizedProducts { get; set; }
    }
}