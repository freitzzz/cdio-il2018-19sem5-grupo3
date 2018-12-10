using core.domain;
using core.modelview.customizedproductcollection;
using core.persistence;
using core.services;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.application
{
    /// <summary>
    /// Application controller for the customized product collections
    /// </summary>
    public sealed class CustomizedProductCollectionController
    {

        /// <summary>
        /// Fetches all available customized products collections
        /// </summary>
        /// <returns>List with all available customized products collections</returns>
        public GetAllCustomizedProductCollectionsModelView findAllCollections()
        {
            return CustomizedProductCollectionModelViewService.fromCollection(
                    PersistenceContext.repositories().createCustomizedProductCollectionRepository().findAllCollections()
            );
        }

        /// <summary>
        /// Fetches a customized product collection by its persistence id
        /// </summary>
        /// <param name="modelView">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public GetCustomizedProductCollectionModelView findCollectionByID(GetCustomizedProductCollectionModelView modelView)
        {
            return CustomizedProductCollectionModelViewService.fromEntity(
                PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(modelView.id)
            );
        }

        /// <summary>
        /// Fetches a customized product collection by its entity identifier
        /// </summary>
        /// <param name="modelView">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public GetCustomizedProductCollectionModelView findCollectionByEID(GetCustomizedProductCollectionModelView modelView)
        {
            return CustomizedProductCollectionModelViewService.fromEntity(
                PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(modelView.name)
            );
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