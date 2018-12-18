using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.services;
using System.Linq;
using core.exceptions;
using core.modelview.commercialcatalogue;
using core.modelview.cataloguecollection;
using core.modelview.customizedproduct;
using core.modelview.cataloguecollectionproduct;

namespace core.application
{
    public class CommercialCatalogueController
    {
        /// <summary>
        /// Constant that represents the message presented when no instance of CommercialCatalogue is found.
        /// </summary>
        private const string CATALOGUES_NOT_FOUND = "Unable to find any catalogues. Please, try adding a catalogue first.";

        /// <summary>
        /// Constant that represents the message presented when no instance of CatalogueCollection is found in the CommercialCatalogue.
        /// </summary>
        private const string CATALOGUE_COLLECTIONS_NOT_FOUND = "Unable to find any collections. Please, try adding a collection first.";

        /// <summary>
        /// Constant that represents the message presented when no instance of CommercialCatalogue with a matching identifier is found.
        /// </summary>
        private const string CATALOGUE_NOT_FOUND_BY_ID = "Unable to find any catalogue with an identifier of: {0}";

        /// <summary>
        /// Constant that represents the message presented when no instance of CatalogueCollection is found in the CommercialCatalogue's IEnumerable of CatalogueCollection.
        /// </summary>
        private const string CATALOGUE_COLLECTION_NOT_FOUND_BY_ID = "Unable to find a collection with an identifier of: {0} in a catalogue with an identifier of: {1}";

        /// <summary>
        /// Constant that represents the message presented when no CatalogueCollectionProduct was found in a CatalogueCollectionProduct. 
        /// </summary>
        private const string CATALOGUE_COLLECTION_PRODUCTS_NOT_FOUND = "Unable to find any customized products in the collection.";

        /// <summary>
        /// Constant that represents the message presented when no CustomizedProduct could be found.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_FOUND_BY_ID = "Unable to find a customized product with an identifier of: {0} in a collection with an identifier of: {1}";

        /// <summary>
        /// Constant that represents the message presented when an instance of CommercialCatalogue could not be saved.
        /// </summary>
        private const string UNABLE_TO_SAVE_CATALOGUE = "Unable to save the commercial catalogue. Please, make sure the reference is unique.";

        /// <summary>
        /// Constant that represents the message presented when no update is made to an instance of CommercialCatalogue.
        /// </summary>
        private const string NO_UPDATES_PERFORMED = "Unable to update the commercial catalogue since no data was provided.";

        /// <summary>
        /// Retrieves all instances of CommercialCatalogue stored in the repository.
        /// </summary>
        /// <returns>Instance of GetAllCommercialCataloguesModelView representing all instances of CommercialCatalogue.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instances of CommercialCatalogue are found.</exception>
        public GetAllCommercialCataloguesModelView findAll()
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            IEnumerable<CommercialCatalogue> commercialCatalogues = catalogueRepository.findAll();

            if (!commercialCatalogues.Any())
            {
                throw new ResourceNotFoundException(CATALOGUES_NOT_FOUND);
            }

            return CommercialCatalogueModelViewService.fromCollection(commercialCatalogues);
        }

        /// <summary>
        /// Retrieves an instance of CommercialCatalogue stored in the repository with a matching identifier.
        /// </summary>
        /// <param name="findCommercialCatalogueModelView">FindCommercialCatalogueModelView with the identifier.</param>
        /// <returns>Instance of GetCommercialCatalogueModelView representing the instance of CommercialCatalogue.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instance of CommercialCatalogue is found with a matching identifier.</exception>
        public GetCommercialCatalogueModelView findCommercialCatalogue(FindCommercialCatalogueModelView findCommercialCatalogueModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(findCommercialCatalogueModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, findCommercialCatalogueModelView.commercialCatalogueId));
            }

            return CommercialCatalogueModelViewService.fromEntity(commercialCatalogue);
        }

        /// <summary>
        /// Retrieves the CommercialCatalogue's CatalogueCollections.
        /// </summary>
        /// <param name="findCommercialCatalogueModelView">FindCommercialCatalogueModelView with the identifier.</param>
        /// <returns>Instance of GetAllCatalogueCollectionsModelView representing the IEnumerable of CatalogueCollection.</returns>
        /// <exception cref="ResourceNotFoundException">
        /// Thrown when no instance of CommercialCatalogue is found with a matching identifier of when the CommercialCatalogue has no CatalogueCollections.
        /// </exception>
        public GetAllCatalogueCollectionsModelView findCatalogueCollections(FindCommercialCatalogueModelView findCommercialCatalogueModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(findCommercialCatalogueModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, findCommercialCatalogueModelView.commercialCatalogueId));
            }

            if (!commercialCatalogue.catalogueCollectionList.Any())
            {
                throw new ResourceNotFoundException(CATALOGUE_COLLECTIONS_NOT_FOUND);
            }
            return CatalogueCollectionModelViewService.fromCollection(commercialCatalogue.catalogueCollectionList);
        }


        /// <summary>
        /// Retrieves a specific CatalogueCollection from the CommercialCatalogue.
        /// </summary>
        /// <param name="findCatalogueCollectionModelView">FindCatalogueCollectionModelView with the CommercialCatalogue's and CatalogueCollection's identifiers.</param>
        /// <returns>An instance of GetCatalogueCollectionModelView representing the CatalogueCollection.</returns>
        /// <exception cref="ResourceNotFoundException">
        /// Thrown when no instance of CommercialCatalogue or CatalogueCollection are found with the given identifiers.
        /// </exception>
        public GetCatalogueCollectionModelView findCatalogueCollection(FindCatalogueCollectionModelView findCatalogueCollectionModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(findCatalogueCollectionModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, findCatalogueCollectionModelView.commercialCatalogueId));
            }

            CatalogueCollection catalogueCollection = commercialCatalogue.catalogueCollectionList
                .Where(cc => cc.customizedProductCollectionId == findCatalogueCollectionModelView.customizedProductCollectionId).SingleOrDefault();

            if (catalogueCollection == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_COLLECTION_NOT_FOUND_BY_ID, findCatalogueCollectionModelView.commercialCatalogueId,
                        findCatalogueCollectionModelView.customizedProductCollectionId)
                );
            }

            return CatalogueCollectionModelViewService.fromEntity(catalogueCollection);
        }

        /// <summary>
        /// Retrieves all the CustomizedProducts added to the CatalogueCollection.
        /// </summary>
        /// <param name="findCatalogueCollectionModelView">FindCatalogueCollectionModelView with the CommercialCatalogue's and CatalogueCollection's identifiers.</param>
        /// <returns>Instance of GetAllCustomizedProductsModelView representing all of the CatalogueCollectionProduct's.</returns>
        /// <exception cref="ResourceNotFoundException">
        /// Thrown when no CommercialCatalogue or CatalogueCollection could be found with the provided identifiers or when the IEnumerable of CatalogueCollectionProduct is empty.
        /// </exception>
        public GetAllCustomizedProductsModelView findCatalogueCollectionProducts(FindCatalogueCollectionModelView findCatalogueCollectionModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(findCatalogueCollectionModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, findCatalogueCollectionModelView.commercialCatalogueId));
            }

            CatalogueCollection catalogueCollection = commercialCatalogue.catalogueCollectionList
                .Where(cc => cc.customizedProductCollectionId == findCatalogueCollectionModelView.customizedProductCollectionId).SingleOrDefault();

            if (catalogueCollection == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_COLLECTION_NOT_FOUND_BY_ID, findCatalogueCollectionModelView.customizedProductCollectionId,
                    findCatalogueCollectionModelView.commercialCatalogueId)
                );
            }

            IEnumerable<CustomizedProduct> customizedProducts = catalogueCollection.catalogueCollectionProducts.Select(ccp => ccp.customizedProduct).ToList();

            if (!customizedProducts.Any())
            {
                throw new ResourceNotFoundException(CATALOGUE_COLLECTION_PRODUCTS_NOT_FOUND);
            }

            return CustomizedProductModelViewService.fromCollection(customizedProducts);
        }


        /// <summary>
        /// Adds a CommercialCatalogue.
        /// </summary>
        /// <param name="addCommercialCatalogueModelView">AddCommercialCatalogueModelView with the data used for creating the CommercialCatalogue.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Thrown when no CustomizedProductCollection or CustomizedProduct could be found with the provided identifiers or when the CommercialCatalogue can't be saved.
        /// </exception>
        public GetCommercialCatalogueModelView addCommercialCatalogue(AddCommercialCatalogueModelView addCommercialCatalogueModelView)
        {
            CommercialCatalogue commercialCatalogue = CreateCommercialCatalogueService.create(addCommercialCatalogueModelView);

            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            commercialCatalogue = catalogueRepository.save(commercialCatalogue);

            //an error occurred while saving the catalogue
            if (commercialCatalogue == null)
            {
                throw new ArgumentException(UNABLE_TO_SAVE_CATALOGUE);
            }

            return CommercialCatalogueModelViewService.fromEntity(commercialCatalogue);
        }

        /// <summary>
        /// Adds a CatalogueCollection to a CommercialCatalogue.
        /// </summary>
        /// <param name="addCatalogueCollectionModelView">AddCatalogueCollectionModelView with the data used for creating a CatalogueCollection.</param>
        /// <returns>GetCommercialCatalogueModelView representing the updated CommercialCatalogue.</returns>
        /// <exception cref="ResourceNotFoundException">
        /// Thrown when no CommercialCatalogue could be found with the provided identifier.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when no CustomizedProductCollection or CustomizedProduct could be found with the provided identifiers.
        /// </exception>
        public GetCommercialCatalogueModelView addCatalogueCollection(AddCatalogueCollectionModelView addCatalogueCollectionModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(addCatalogueCollectionModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_NOT_FOUND_BY_ID, addCatalogueCollectionModelView.commercialCatalogueId
                ));
            }

            CatalogueCollection catalogueCollection = CreateCatalogueCollectionService.create(addCatalogueCollectionModelView);

            commercialCatalogue.addCollection(catalogueCollection);
            commercialCatalogue = catalogueRepository.update(commercialCatalogue);

            return CommercialCatalogueModelViewService.fromEntity(commercialCatalogue);
        }

        /// <summary>
        /// Adds a CustomizedProduct to the CatalogueCollection.
        /// </summary>
        /// <param name="addCatalogueCollectionProductModelView">AddCatalogueCollectionProductModelView with the data used </param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">
        /// Thrown when no instance of CommercialCatalogue or CatalogueCollection could be found with the provided identifiers.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the CustomizedProduct being added does not exist in a given CustomizedProductCollection.
        /// </exception>
        public GetCommercialCatalogueModelView addCatalogueCollectionProduct(AddCatalogueCollectionProductModelView addCatalogueCollectionProductModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(addCatalogueCollectionProductModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, addCatalogueCollectionProductModelView.commercialCatalogueId));
            }

            CatalogueCollection catalogueCollection = commercialCatalogue.catalogueCollectionList
                .Where(cc => cc.customizedProductCollectionId == addCatalogueCollectionProductModelView.customizedProductCollectionId).SingleOrDefault();

            if (catalogueCollection == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_COLLECTION_NOT_FOUND_BY_ID, addCatalogueCollectionProductModelView.commercialCatalogueId,
                        addCatalogueCollectionProductModelView.customizedProductCollectionId)
                );
            }

            //retrieve the instance of CustomizedProduct from the the CustomizedProductCollection linked to the CatalogueCollection
            CustomizedProduct customizedProduct = catalogueCollection.customizedProductCollection.collectionProducts
                .Where(cp => cp.customizedProductId == addCatalogueCollectionProductModelView.customizedProductId)
                    .Select(cp => cp.customizedProduct).SingleOrDefault();

            if (customizedProduct == null)
            {
                throw new ArgumentException(string.Format(
                    CUSTOMIZED_PRODUCT_NOT_FOUND_BY_ID, addCatalogueCollectionProductModelView.customizedProductId, addCatalogueCollectionProductModelView.customizedProductCollectionId
                ));
            }

            catalogueCollection.addCustomizedProduct(customizedProduct);
            commercialCatalogue = catalogueRepository.update(commercialCatalogue);

            return CommercialCatalogueModelViewService.fromEntity(commercialCatalogue);
        }


        /// <summary>
        /// Updates an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="updateCommercialCatalogueModelView">UpdateCommercialCatalogueModelView with the CommerciaCatalogue's identifier and data being updated.</param>
        /// <returns>An instance of GetCommercialCatalogueModelView representing the updated CommercialCatalogue.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no CommercialCatalogue is found with a given identifier.</exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown no update is made to the CommercialCatalogue or when the CommercialCatalogue could not be saved with the updated data.
        /// </exception>
        public GetCommercialCatalogueModelView updateCommercialCatalogue(UpdateCommercialCatalogueModelView updateCommercialCatalogueModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(updateCommercialCatalogueModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, updateCommercialCatalogueModelView.commercialCatalogueId));
            }

            bool performedUpdates = false;

            if (updateCommercialCatalogueModelView.reference != null)
            {
                commercialCatalogue.changeReference(updateCommercialCatalogueModelView.reference);
                performedUpdates = true;
            }

            if (updateCommercialCatalogueModelView.designation != null)
            {
                commercialCatalogue.changeDesignation(updateCommercialCatalogueModelView.designation);
                performedUpdates = true;
            }

            //if no update was performed
            if (!performedUpdates)
            {
                throw new ArgumentException(NO_UPDATES_PERFORMED);
            }

            commercialCatalogue = catalogueRepository.update(commercialCatalogue);

            if (commercialCatalogue == null)
            {
                throw new ArgumentException(UNABLE_TO_SAVE_CATALOGUE);
            }

            return CommercialCatalogueModelViewService.fromEntity(commercialCatalogue);
        }


        /// <summary>
        /// Deletes an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="deleteCommercialCatalogueModelView">DeleteCommercialCatalogueModelView with CommercialCatalogue's identifier.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CommercialCatalogue could be found with the provided identifier.</exception>
        public void deleteCommercialCatalogue(DeleteCommercialCatalogueModelView deleteCommercialCatalogueModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(deleteCommercialCatalogueModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, deleteCommercialCatalogueModelView.commercialCatalogueId));
            }

            catalogueRepository.remove(commercialCatalogue);
        }


        /// <summary>
        /// Removes an instance of CatalogueCollection from a CommercialCatalogue.
        /// </summary>
        /// <param name="deleteCatalogueCollectionModelView">DeleteCatalogueCollectionModelView with the CusttotmizedProductCollection and CommercialCatalogue's identifiers.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CommercialCatalogue or CatalogueCollection could be found.</exception>
        public void deleteCatalogueCollection(DeleteCatalogueCollectionModelView deleteCatalogueCollectionModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(deleteCatalogueCollectionModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, deleteCatalogueCollectionModelView.commercialCatalogueId));
            }

            CatalogueCollection catalogueCollection = commercialCatalogue.catalogueCollectionList
                .Where(cc => cc.customizedProductCollection.Id == deleteCatalogueCollectionModelView.customizedProductCollectionId).SingleOrDefault();

            if (catalogueCollection == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_COLLECTION_NOT_FOUND_BY_ID, deleteCatalogueCollectionModelView.customizedProductCollectionId, deleteCatalogueCollectionModelView.commercialCatalogueId
                ));
            }

            commercialCatalogue.removeCollection(catalogueCollection);
            catalogueRepository.update(commercialCatalogue);
        }


        /// <summary>
        /// Removes an instance of CatalogueCollectionProduct from a CommercialCatalogue's CatalogueCollection.
        /// </summary>
        /// <param name="deleteCatalogueCollectionProductModelView">DeleteCatalogueCollectionProductModelView with the CustomizedProduct's, CustomizedProductCollection's and CommercialCatalogue's identifiers.</param>
        ///<exception cref="ResourceNotFoundException">Thrown when no CommercialCatalogue, CatalogueCollection or CatalogueCollectionProduct could be found.</exception>
        public void deleteCatalogueCollectionProduct(DeleteCatalogueCollectionProductModelView deleteCatalogueCollectionProductModelView)
        {
            CommercialCatalogueRepository catalogueRepository = PersistenceContext.repositories().createCommercialCatalogueRepository();

            CommercialCatalogue commercialCatalogue = catalogueRepository.find(deleteCatalogueCollectionProductModelView.commercialCatalogueId);

            if (commercialCatalogue == null)
            {
                throw new ResourceNotFoundException(string.Format(CATALOGUE_NOT_FOUND_BY_ID, deleteCatalogueCollectionProductModelView.commercialCatalogueId));
            }

            CatalogueCollection catalogueCollection = commercialCatalogue.catalogueCollectionList
                .Where(cc => cc.customizedProductCollection.Id == deleteCatalogueCollectionProductModelView.customizedProductCollectionId).SingleOrDefault();

            if (catalogueCollection == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CATALOGUE_COLLECTION_NOT_FOUND_BY_ID, deleteCatalogueCollectionProductModelView.customizedProductCollectionId, deleteCatalogueCollectionProductModelView.commercialCatalogueId
                ));
            }

            CustomizedProduct customizedProduct = catalogueCollection.catalogueCollectionProducts
                .Where(ccp => ccp.customizedProductId == deleteCatalogueCollectionProductModelView.customizedProductId)
                    .Select(ccp => ccp.customizedProduct).SingleOrDefault();

            if (customizedProduct == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    CUSTOMIZED_PRODUCT_NOT_FOUND_BY_ID, deleteCatalogueCollectionProductModelView.customizedProductCollectionId, deleteCatalogueCollectionProductModelView.customizedProductId
                ));
            }

            catalogueCollection.removeCustomizedProduct(customizedProduct);
            catalogueRepository.update(commercialCatalogue);
        }
    }
}