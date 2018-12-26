using core.domain;
using core.exceptions;
using core.modelview.customizedproductcollection;
using core.persistence;
using core.services;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.application
{
    /// <summary>
    /// Application controller for the customized product collections
    /// </summary>
    public sealed class CustomizedProductCollectionController
    {
        /// <summary>
        /// Constant that represents the message that occurs if there are no collections available
        /// </summary>
        private const string NO_COLLECTIONS_AVAILABLE = "There are no customized products collections available";

        /// <summary>
        /// Constant that represents the message presented when no collection could be found with a matching identifier.
        /// </summary>
        private const string UNABLE_TO_FIND_COLLECTION_BY_ID = "Unable to find a collection with an identifier of: {0}";

        /// <summary>
        /// Constant that represents the message presented when no collection could be found with a matching name.
        /// </summary>
        private const string UNABLE_TO_FIND_COLLECTION_BY_NAME = "Unable to find a collection with the name '{0}'";

        /// <summary>
        /// Fetches all available customized products collections
        /// </summary>
        /// <returns>List with all available customized products collections</returns>
        public GetAllCustomizedProductCollectionsModelView findAllCollections()
        {
            IEnumerable<CustomizedProductCollection> collections = PersistenceContext.repositories().createCustomizedProductCollectionRepository().findAll();

            if (!collections.Any())
            {
                throw new ResourceNotFoundException(NO_COLLECTIONS_AVAILABLE);
            }

            return CustomizedProductCollectionModelViewService.fromCollection(collections);
        }

        /// <summary>
        /// Fetches a customized product collection by its persistence id
        /// </summary>
        /// <param name="modelView">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public GetCustomizedProductCollectionModelView findCollectionByID(GetCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollection collection = PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(modelView.id);

            if (collection == null)
            {
                throw new ResourceNotFoundException(string.Format(UNABLE_TO_FIND_COLLECTION_BY_ID, modelView.id));
            }

            return CustomizedProductCollectionModelViewService.fromEntity(collection);
        }

        /// <summary>
        /// Fetches a customized product collection by its entity identifier
        /// </summary>
        /// <param name="modelView">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public GetCustomizedProductCollectionModelView findCollectionByEID(GetCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollection collection = PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(modelView.name);

            if (collection == null)
            {
                throw new ResourceNotFoundException(string.Format(UNABLE_TO_FIND_COLLECTION_BY_NAME, modelView.name));
            }

            return CustomizedProductCollectionModelViewService.fromEntity(collection);
        }

        /// <summary>
        /// Adds a new customized products collection
        /// </summary>
        /// <param name="modelView">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the created customized product collection information</returns>
        public GetCustomizedProductCollectionModelView addCollection(AddCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollection customizedProductCollection = CreateCustomizedProductCollectionService.create(modelView);

            return CustomizedProductCollectionModelViewService.fromEntity(customizedProductCollection);
        }

        /// <summary>
        /// Updates basic information of a customized product collection
        /// </summary>
        /// <param name="modelView">UpdateCustomizedProductCollectionDTO with the customized product collection update information</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public GetBasicCustomizedProductCollectionModelView updateCollectionBasicInformation(UpdateCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollection updatedCustomizedProductCollection = UpdateCustomizedProductCollectionService.update(modelView);

            return CustomizedProductCollectionModelViewService.fromEntityAsBasic(updatedCustomizedProductCollection);
        }

        /// <summary>
        /// Adds a customized product to the customized product collection
        /// </summary>
        /// <param name="modelView">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the customized product was successfully added, false if not</returns>
        public GetCustomizedProductCollectionModelView addCustomizedProductToCustomizedProductCollection(AddCustomizedProductToCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollection updatedCustomizedProductCollection = UpdateCustomizedProductCollectionService.update(modelView);

            return CustomizedProductCollectionModelViewService.fromEntity(updatedCustomizedProductCollection);
        }

        /// <summary>
        /// Removes a customized product from a customized product collection
        /// </summary>
        /// <param name="collectionID">ID of the customized product collection to update</param>
        /// <param name="customizedProductID">ID of the customized product to remove</param>
        /// <returns>boolean true if the customized product was successfully removed, false if not</returns>
        public void removeCustomizedProductFromCustomizedProductCollection(DeleteCustomizedProductFromCustomizedProductCollectionModelView modelView)
        {
            DeleteCustomizedProductCollectionService.delete(modelView);
        }

        /// <summary>
        /// Disables a customized product collection
        /// </summary>
        /// <param name="modelView">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the disable was successful, false if not</returns>
        public void disableCustomizedProductCollection(DeleteCustomizedProductCollectionModelView modelView)
        {
            DeleteCustomizedProductCollectionService.deactivate(modelView);
        }
    }
}