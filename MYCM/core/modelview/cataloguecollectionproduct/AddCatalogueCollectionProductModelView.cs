using System.Runtime.Serialization;

namespace core.modelview.cataloguecollectionproduct
{
    /// <summary>
    /// Class representing the ModelView used for adding a CustomizedProduct to a CatalogueCollection.
    /// </summary>
    [DataContract]
    public class AddCatalogueCollectionProductModelView
    {
        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long commercialCatalogueId { get; set; }

        /// <summary>
        /// CustomizedProductCollection's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [DataMember]
        public long customizedProductId { get; set; }
    }
}