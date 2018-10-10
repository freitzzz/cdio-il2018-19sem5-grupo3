using core.dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace backend.Controllers{
    /// <summary>
    /// MVC Controller for CustomizedProductCollection operations
    /// </summary>
    [Route("/myc/api/collections")]
    public class CustomizedProductCollectionController:Controller{

        /// <summary>
        /// Fetches all available collections of customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult<List<CustomizedProductCollectionDTO>> findAll(){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the information of a customized product collection by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the customized product collection information</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> findByID(long id){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new collection of customized products
        /// </summary>
        /// <param name="customizedProductCollectionDTO"></param>
        /// <returns>ActionResult with the created collection of customized products</returns>
        [HttpPost]
        public ActionResult<CustomizedProductCollectionDTO> addCustomizedProductCollection(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates basic information of a certain customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information success about update</returns>
        [HttpPut("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> updateCustomizedProductCollection(long id,UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the customized products of a certain customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information success about update</returns>
        [HttpPut("{id}/customizedproducts")]
        public ActionResult<CustomizedProductCollectionDTO> updateCustomizedProductCollectionCustomizedProducts(long id,UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the information success about the remove</returns>
        [HttpDelete("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> remove(long id){
            throw new NotImplementedException();
        }
    }
}