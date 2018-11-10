using System;
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

        private const string ERROR_CUSTOMIZED_PRODUCT_COLLECTION_NOT_FOUND = "No Customized Product Collection was found with a matching identifier.";

        private const string ERROR_CUSTOMIZED_PRODUCT_NOT_FOUND = "No Customized Product was found with a matching identifier.";

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

            CommercialCatalogue newComCatalogue = null;

            //if no collections are specified, build a catalogue with just the given reference and designation.
            if (commercialCatalogueDTO.catalogueCollectionDTOs != null)
            {
                CustomizedProductCollectionRepository collectionRepository = PersistenceContext.repositories().createCustomizedProductCollectionRepository();

                CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

                //the specified collections should have id's and not the whole collection information

                List<CatalogueCollection> catalogueCollections = new List<CatalogueCollection>();

                foreach (CatalogueCollectionDTO collectionDTO in commercialCatalogueDTO.catalogueCollectionDTOs)
                {
                    CatalogueCollection catalogueCollection = null;
                    CustomizedProductCollection collection = collectionRepository.find(collectionDTO.customizedProductCollectionDTO.id);

                    //the collection was not found
                    if (collection == null)
                    {
                        throw new ArgumentException(ERROR_CUSTOMIZED_PRODUCT_COLLECTION_NOT_FOUND);
                    }

                    //if no products are specified, then add all contained in the collection
                    if (collectionDTO.customizedProductDTOs == null)
                    {
                        catalogueCollection = new CatalogueCollection(collection);
                    }
                    else
                    {
                        List<CustomizedProduct> customizedProducts = new List<CustomizedProduct>();

                        foreach (CustomizedProductDTO customizedProductDTO in collectionDTO.customizedProductDTOs)
                        {
                            CustomizedProduct customizedProduct = customizedProductRepository.find(customizedProductDTO.id);

                            //the product was not found
                            if (customizedProduct == null)
                            {
                                throw new ArgumentException(ERROR_CUSTOMIZED_PRODUCT_NOT_FOUND);
                            }

                            customizedProducts.Add(customizedProduct);
                        }

                        catalogueCollection = new CatalogueCollection(collection, customizedProducts);

                    }

                    catalogueCollections.Add(catalogueCollection);
                }

                newComCatalogue = new CommercialCatalogue(reference, designation, catalogueCollections);
            }
            else
            {
                newComCatalogue = new CommercialCatalogue(reference, designation);
            }

            return newComCatalogue;
        }
    }
}