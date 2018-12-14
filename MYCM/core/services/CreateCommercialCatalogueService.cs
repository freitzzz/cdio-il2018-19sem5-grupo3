using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.cataloguecollection;
using core.modelview.commercialcatalogue;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Class representing the service used for creating instances of CommercialCatalogue.
    /// </summary>
    public static class CreateCommercialCatalogueService
    {
        /// <summary>
        /// Creates a new instance of CommercialCatalogue with the data in the given instance of AddCommercialCatalogueModelView.
        /// </summary>
        /// <param name="addCommercialCatalogueModelView">AddCommercialCatalogueModelView with the CommercialCatalogue's data.</param>
        /// <returns>An instance of CommercialCatalogue.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when no CustomizedProductCollection or CustomizedProduct could be found with the provided identifiers.
        /// </exception>
        public static CommercialCatalogue create(AddCommercialCatalogueModelView addCommercialCatalogueModelView)
        {
            string reference = addCommercialCatalogueModelView.reference;
            string designation = addCommercialCatalogueModelView.designation;

            //check if catalogue collections were specified
            if (!addCommercialCatalogueModelView.catalogueCollections.Any())
            {
                return new CommercialCatalogue(reference, designation);
            }
            else
            {
                //create repositories so that customized products and collections can be fetched
                CustomizedProductCollectionRepository customizedProductCollectionRepository = PersistenceContext
                    .repositories().createCustomizedProductCollectionRepository();

                CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories()
                    .createCustomizedProductRepository();

                List<CatalogueCollection> catalogueCollections = new List<CatalogueCollection>();

                foreach (AddCatalogueCollectionModelView addCatalogueCollectionModelView in addCommercialCatalogueModelView.catalogueCollections)
                {
                    CatalogueCollection catalogueCollection = CreateCatalogueCollectionService.create(addCatalogueCollectionModelView);

                    catalogueCollections.Add(catalogueCollection);
                }

                return new CommercialCatalogue(reference, designation, catalogueCollections);
            }
        }
    }
}