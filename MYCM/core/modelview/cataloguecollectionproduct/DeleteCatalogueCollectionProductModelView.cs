namespace core.modelview.cataloguecollectionproduct
{
    /// <summary>
    /// Class representing the ModelView used for deleting an instance of CatalogueCollection.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteCatalogueCollectionProductModelView
    {
        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long commercialCatalogueId { get; set; }

        /// <summary>
        /// CustomizedProductCollection's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long customizedProductId { get; set; }
    }
}