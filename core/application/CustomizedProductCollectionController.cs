using core.domain;
using core.dto;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a customized product collection by its id
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the fetched customized product collection information</returns>
        public CustomizedProductCollectionDTO findCollectionByID(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new customized products collection
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>CustomizedProductCollectionDTO with the created customized product collection information</returns>
        public CustomizedProductCollectionDTO addCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates basic information of a customized product collection
        /// </summary>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection update information</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateCollectionBasicInformation(UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the customized products of a customized product collection
        /// </summary>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection update information</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateCollectionCustomizedProducts(UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disables a customized product collection
        /// </summary>
        /// <param name="customizedProductCollectionDTO">UpdateCustomizedProductCollectionDTO with the customized product collection information</param>
        /// <returns>boolean true if the disable was successful, false if not</returns>
        public bool disableCustomizedProductCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            throw new NotImplementedException();
        }
    }
}