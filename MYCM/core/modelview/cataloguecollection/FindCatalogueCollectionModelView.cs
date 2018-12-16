namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of CatalogueCollection.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */

    public class FindCatalogueCollectionModelView
    {
        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long commercialCatalogueId { get; set; }

        /// <summary>
        /// CatalogueCollection's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long customizedProductCollectionId { get; set; }
    }
}