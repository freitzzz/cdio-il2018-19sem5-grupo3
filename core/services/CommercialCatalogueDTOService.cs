using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Service responsible for transforming instances of CommercialCatalogueDTO into instances of CommercialCatalogue.s
    /// </summary>
    public sealed class CommercialCatalogueDTOService
    {
        /// <summary>
        /// Private constructor used for hiding the implicit public one.
        /// </summary>
        private CommercialCatalogueDTOService() { }

        /// <summary>
        /// Transforms a CommercialCatalogueDTO into a CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogueDTO">CommercialCatalogueDTO with the CommercialCatalogue data.</param>
        /// <returns>Transformed CommercialCatalogue built with CommercialCatalogueDTO info.</returns>
        public static CommercialCatalogue transform(CommercialCatalogueDTO commercialCatalogueDTO)
        {
            string reference = commercialCatalogueDTO.reference;
            string designation = commercialCatalogueDTO.designation;
            
            CommercialCatalogue newComCatalogue = new CommercialCatalogue(reference, designation);

            if (commercialCatalogueDTO.collectionList == null)
            {
                //the specified collections should have id's and not the whole collection information

                List<CatalogueCollection> catalogueCollections = new List<CatalogueCollection>();

                CustomizedProductCollectionRepository collectionRepository = PersistenceContext.repositories().createCustomizedProductCollectionRepository();

                foreach (CatalogueCollectionDTO collectionDTO in commercialCatalogueDTO.collectionList)
                {
                    CustomizedProductCollection collection = collectionRepository.find(collectionDTO.customizedProductCollectionDTO.id);

                    List<CustomizedProduct> customizedProducts = collection.collectionProducts.Select(cp => cp.customizedProduct).ToList();

                    CatalogueCollection catalogueCollection = new CatalogueCollection(newComCatalogue, collection, customizedProducts);

                    catalogueCollections.Add(catalogueCollection);
                }


                newComCatalogue = new CommercialCatalogue(reference, designation, catalogueCollections);
            }

            return newComCatalogue;
        }
    }
}