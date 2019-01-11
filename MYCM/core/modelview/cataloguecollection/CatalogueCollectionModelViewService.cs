using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.customizedproduct;
using core.modelview.customizedproductcollection;

namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Static class representing a service used for converting instances of CatalogueCollection into ModelViews.
    /// </summary>
    public static class CatalogueCollectionModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when the provided instance of CatalogueCollection is null.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE_COLLECTION = "The provided catalogue collection is invalid.";

        /// <summary>
        /// Constant representing the error message presented when the provided IEnumerable of CatalogueCollection is null.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE_COLLECTION_ENUMERABLE = "The provided enumerable of catalogue collection is invalid.";

        /// <summary>
        /// Converts an instance of CatalogueCollection into an instance of GetBasicCatalogueCollectionModelView.
        /// </summary>
        /// <param name="catalogueCollection">Instance of CatalogueCollection being converted.</param>
        /// <returns>An instance of GetBasicCatalogueCollectionModelView representing the CatalogueCollection.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CatalogueCollection is null.</exception>
        public static GetBasicCatalogueCollectionModelView fromEntityAsBasic(CatalogueCollection catalogueCollection)
        {
            if (catalogueCollection == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE_COLLECTION);
            }

            GetBasicCatalogueCollectionModelView basicCatalogueCollectionModelView = new GetBasicCatalogueCollectionModelView();
            basicCatalogueCollectionModelView.id = catalogueCollection.customizedProductCollectionId;
            basicCatalogueCollectionModelView.name = catalogueCollection.customizedProductCollection.name;
            basicCatalogueCollectionModelView.hasCustomizedProducts = catalogueCollection.catalogueCollectionProducts.Any();

            return basicCatalogueCollectionModelView;
        }

        /// <summary>
        /// Converts an instance of CatalogueCollection into an instance of GetCatalogueCollectionModelView.
        /// </summary>
        /// <param name="catalogueCollection">Instance of CatalogueCollection being converted.</param>
        /// <returns>An instance of GetCatalogueCollectionModelView representing the CatalogueCollection.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CatalogueCollection is null.</exception>
        public static GetCatalogueCollectionModelView fromEntity(CatalogueCollection catalogueCollection)
        {
            if (catalogueCollection == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE_COLLECTION);
            }

            GetCatalogueCollectionModelView catalogueCollectionModelView = new GetCatalogueCollectionModelView();
            catalogueCollectionModelView.customizedProductCollectionId = catalogueCollection.customizedProductCollectionId;
            catalogueCollectionModelView.name = catalogueCollection.customizedProductCollection.name;

            if (catalogueCollection.catalogueCollectionProducts.Any())
            {
                IEnumerable<CustomizedProduct> customizedProducts = catalogueCollection.catalogueCollectionProducts.Select(ccc => ccc.customizedProduct).ToList();
                catalogueCollectionModelView.customizedProducts = CustomizedProductModelViewService.fromCollection(customizedProducts);
            }

            return catalogueCollectionModelView;
        }


        /// <summary>
        /// Converts an IEnumerable of CatalogueCollection into an instance of GetAllCatalogueCollectionsModelView.
        /// </summary>
        /// <param name="catalogueCollections"></param>
        /// <returns>An instance of GetAllCatalogueCollectionsModelView representing the IEnumerable of CatalogueCollection.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of CatalogueCollection is null.</exception>
        public static GetAllCatalogueCollectionsModelView fromCollection(IEnumerable<CatalogueCollection> catalogueCollections)
        {
            if (catalogueCollections == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE_COLLECTION_ENUMERABLE);
            }

            GetAllCatalogueCollectionsModelView catalogueCollectionsModelView = new GetAllCatalogueCollectionsModelView();

            foreach (CatalogueCollection catalogueCollection in catalogueCollections)
            {
                catalogueCollectionsModelView.Add(fromEntityAsBasic(catalogueCollection));
            }

            return catalogueCollectionsModelView;
        }
    }
}