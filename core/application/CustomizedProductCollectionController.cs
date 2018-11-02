using core.domain;
using core.dto;
using core.persistence;
using core.services;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.application{
    /// <summary>
    /// Application controller for the customized product collections
    /// </summary>
    public sealed class CustomizedProductCollectionController{
        
        /// <summary>
        /// Fetches all available customized products collections
        /// </summary>
        /// <returns>List with all available customized products collections</returns>
        public List<CustomizedProductCollectionDTO> findAllCollections(){
            return Collections.enumerableAsList(
                DTOUtils.parseToDTOS(
                    PersistenceContext.repositories().createCustomizedProductCollectionRepository().findAllCollections()
                )
            );
        }

        /// <summary>
        /// Fetches a customized product collection by its persistence id
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public CustomizedProductCollectionDTO findCollectionByID(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            return PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(customizedProductCollectionDTO.id).toDTO();
        }

        /// <summary>
        /// Fetches a customized product collection by its entity identifier
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public CustomizedProductCollectionDTO findCollectionByEID(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            return PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(customizedProductCollectionDTO.name).toDTO();
        }

        /// <summary>
        /// Adds a new customized products collection
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the created customized product collection information</returns>
        public CustomizedProductCollectionDTO addCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            CustomizedProductCollection customizedProductCollection=new CustomizedProductCollectionDTOService().transform(customizedProductCollectionDTO);
            return PersistenceContext.repositories().createCustomizedProductCollectionRepository().save(customizedProductCollection).toDTO();
        }

        /// <summary>
        /// Updates basic information of a customized product collection
        /// </summary>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection update information</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateCollectionBasicInformation(UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            CustomizedProductCollectionRepository customizedProductCollectionRepository=PersistenceContext.repositories().createCustomizedProductCollectionRepository();
            CustomizedProductCollection customizedProductCollection=customizedProductCollectionRepository.find(updateCustomizedProductCollectionDTO.id);
            bool updatedWithSuccess=true;
            bool performedAtLeastOneUpdate=false;
            if(updateCustomizedProductCollectionDTO.name!=null){
                updatedWithSuccess&=customizedProductCollection.changeName(updateCustomizedProductCollectionDTO.name);
                performedAtLeastOneUpdate=true;
            }
            if(!performedAtLeastOneUpdate||!updatedWithSuccess)return false;
            updatedWithSuccess&=customizedProductCollectionRepository.update(customizedProductCollection)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Adds a customized product to the customized product collection
        /// </summary>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the customized product was successfully added, false if not</returns>
        public bool addCustomizedProductsToCustomizedProductCollection(UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            CustomizedProductCollectionRepository customizedProductCollectionRepository = PersistenceContext.repositories().createCustomizedProductCollectionRepository();
            CustomizedProductCollection customizedProductCollection = customizedProductCollectionRepository.find(updateCustomizedProductCollectionDTO.id);
            CustomizedProductDTO customizedProductDTO = updateCustomizedProductCollectionDTO.customizedProductToAdd;
            if(customizedProductDTO != null){
                CustomizedProduct customizedProduct = PersistenceContext.repositories().createCustomizedProductRepository().find(customizedProductDTO.id);
                return customizedProductCollection.addCustomizedProduct(customizedProduct) && customizedProductCollectionRepository.update(customizedProductCollection) != null;
            }
            return false;
        }

        /// <summary>
        /// Removes a customized product from the customized product collection
        /// </summary>
        /// <param name="collectionID">ID of the customized product collection to update</param>
        /// <param name="customizedProductID">ID of the customized product to remove</param>
        /// <returns>boolean true if the customized product was successfully removed, false if not</returns>
        public bool removeCustomizedProductsToCustomizedProductCollection(long collectionID, long customizedProductID){
            CustomizedProduct customizedProduct = PersistenceContext.repositories().createCustomizedProductRepository().find(customizedProductID);
            if(customizedProduct != null){
                CustomizedProductCollectionRepository customizedProductCollectionRepository = PersistenceContext.repositories().createCustomizedProductCollectionRepository();
                CustomizedProductCollection customizedProductCollection = customizedProductCollectionRepository.find(collectionID);
                return customizedProductCollection.removeCustomizedProduct(customizedProduct) && customizedProductCollectionRepository.update(customizedProductCollection) != null;
            }
            return false;
        }

        /// <summary>
        /// Disables a customized product collection
        /// </summary>
        /// <param name="customizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the disable was successful, false if not</returns>
        public bool disableCustomizedProductCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            CustomizedProductCollectionRepository customizedProductCollectionRepository=PersistenceContext.repositories().createCustomizedProductCollectionRepository();
            CustomizedProductCollection customizedProductCollection=customizedProductCollectionRepository.find(customizedProductCollectionDTO.id);
            return customizedProductCollection!=null && customizedProductCollection.deactivate() && customizedProductCollectionRepository.update(customizedProductCollection)!=null;
        }
    }
}