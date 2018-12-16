using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.cataloguecollection;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Static class representing a Service used for creating an instance of CatalogueCollection.
    /// </summary>
    public static class CreateCatalogueCollectionService
    {
        /// <summary>
        /// Constant representing the message presented when a CustomizedProductCollection could not be found with a given identifier.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_COLLECTION_NOT_FOUND = "Unable to find a customized product collection with an identifier of: {0}";

        /// <summary>
        /// Constant representing the message presented when a CustomizedProduct could not be found in a certain CustomizedProductCollection.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_FOUND_IN_COLLECTION = "Unable to find a customized product with an identifier of: {0} in the collection with an identifier of: {1}";

        /// <summary>
        /// Creates an instance of CatalogueCollection with the data in the given AddCatalogueCollectionModelView.
        /// </summary>
        /// <param name="addCatalogueCollectionModelView">AddCatalogueCollectionModelView with the CatalogueCollection's data.</param>
        /// <returns>An instance of CatalogueCollection.</returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when no CustomizedProductCollection or CustomizedProduct could be found with the provided identifiers.
        /// </exception>
        public static CatalogueCollection create(AddCatalogueCollectionModelView addCatalogueCollectionModelView)
        {
            CustomizedProductCollectionRepository collectionRepository = PersistenceContext.repositories().createCustomizedProductCollectionRepository();

            CustomizedProductCollection customizedProductCollection = collectionRepository.find(addCatalogueCollectionModelView.customizedProductCollectionId);

            if (customizedProductCollection == null)
            {
                throw new ArgumentException(string.Format(CUSTOMIZED_PRODUCT_COLLECTION_NOT_FOUND, addCatalogueCollectionModelView.customizedProductCollectionId));
            }

            CatalogueCollection catalogueCollection = null;

            //check if any customized product was defined
            if (addCatalogueCollectionModelView.customizedProductIds.Any())
            {
                IEnumerable<long> customizedProductIds = addCatalogueCollectionModelView.customizedProductIds.Select(cp => cp.customizedProductId).ToList();

                List<CustomizedProduct> customizedProducts = new List<CustomizedProduct>();

                foreach (long customizedProductId in customizedProductIds)
                {
                    CustomizedProduct customizedProduct = customizedProductCollection.collectionProducts
                        .Where(cp => cp.customizedProductId == customizedProductId)
                            .Select(cp => cp.customizedProduct).SingleOrDefault();

                    if (customizedProduct == null)
                    {
                        throw new ArgumentException(string.Format(
                            CUSTOMIZED_PRODUCT_NOT_FOUND_IN_COLLECTION, customizedProductId, addCatalogueCollectionModelView.customizedProductCollectionId)
                        );
                    }

                    customizedProducts.Add(customizedProduct);
                }

                catalogueCollection = new CatalogueCollection(customizedProductCollection, customizedProducts);
            }
            else
            {
                catalogueCollection = new CatalogueCollection(customizedProductCollection);
            }

            return catalogueCollection;
        }
    }
}