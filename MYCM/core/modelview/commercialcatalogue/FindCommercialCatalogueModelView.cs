namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of CommercialCatalogue.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class FindCommercialCatalogueModelView
    {
        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long commercialCatalogueId { get; set; }
    }
}