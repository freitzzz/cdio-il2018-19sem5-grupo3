using core.domain;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Service class used for converting instances of Commercial Catalogues to ModelViews.
    /// </summary>
    public class CommercialCatalogueModelViewService
    {
          /// <summary>
        /// Builds an instance of GetCommercialCatalogueModelView from an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogue">Instance of CommercialCatalogue from which the ModelView will be built.</param>
        /// <returns>An instance of GetCommercialCatalogueModelView.</returns>
        public static GetCommercialCatalogueModelView fromEntity(CommercialCatalogue commercialCatalogue)
        {
            GetCommercialCatalogueModelView commercialCatalogueView = new GetCommercialCatalogueModelView();

            commercialCatalogueView.id = commercialCatalogue.Id;
            commercialCatalogueView.reference = commercialCatalogue.reference;
            commercialCatalogueView.designation = commercialCatalogue.designation;
            //commercialCatalogueView.commercialCatalogueCatalogueCollectionList = commercialCatalogue.catalogueCollectionList;
            return commercialCatalogueView;
        }

    }
}