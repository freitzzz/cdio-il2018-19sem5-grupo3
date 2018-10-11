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
        /// Fetches a customized product collection by its id
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public CustomizedProductCollectionDTO findCollectionByID(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            return PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(customizedProductCollectionDTO.id).toDTO();
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
        /// Updates the customized products of a customized product collection
        /// </summary>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection update information</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateCollectionCustomizedProducts(UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            CustomizedProductCollectionRepository customizedProductCollectionRepository=PersistenceContext.repositories().createCustomizedProductCollectionRepository();
            CustomizedProductCollection customizedProductCollection=customizedProductCollectionRepository.find(updateCustomizedProductCollectionDTO.id);
            bool updatedWithSuccess=true;
            bool performedAtLeastOneUpdate=false;
            
            if(!Collections.isEnumerableNullOrEmpty(updateCustomizedProductCollectionDTO.customizedProductsToAdd)){
                IEnumerable<CustomizedProduct> customizedProductsToAdd=PersistenceContext.repositories().createCustomizedProductRepository().findCustomizedProductsByTheirPIDS(updateCustomizedProductCollectionDTO.customizedProductsToAdd);
                //TODO: CHECK LISTS LENGTH
                foreach(CustomizedProduct customizedProduct in customizedProductsToAdd)
                    updatedWithSuccess&=customizedProductCollection.addCustomizedProduct(customizedProduct);
                performedAtLeastOneUpdate=true;
            }

            if(!updatedWithSuccess)return false;

            if(!Collections.isEnumerableNullOrEmpty(updateCustomizedProductCollectionDTO.customizedProductsToRemove)){
                IEnumerable<CustomizedProduct> customizedProductsToRemove=PersistenceContext.repositories().createCustomizedProductRepository().findCustomizedProductsByTheirPIDS(updateCustomizedProductCollectionDTO.customizedProductsToRemove);
                //TODO: CHECK LISTS LENGTH
                foreach(CustomizedProduct customizedProduct in customizedProductsToRemove)
                    updatedWithSuccess&=customizedProductCollection.removeCustomizedProduct(customizedProduct);
                performedAtLeastOneUpdate=true;
            }

            if(!performedAtLeastOneUpdate||!updatedWithSuccess)return false;
            updatedWithSuccess&=customizedProductCollectionRepository.update(customizedProductCollection)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Disables a customized product collection
        /// </summary>
        /// <param name="customizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the disable was successful, false if not</returns>
        public bool disableCustomizedProductCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            CustomizedProductCollection customizedProductCollection=PersistenceContext.repositories().createCustomizedProductCollectionRepository().find(customizedProductCollectionDTO.id);
            return customizedProductCollection!=null && customizedProductCollection.disable();
        }
    }
}