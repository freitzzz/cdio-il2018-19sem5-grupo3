using core.dto;
using support.utils;
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
        /// Constant that represents the message that ocurres if there are no collections available
        /// </summary>
        private const string NO_COLLECTIONS_AVAILABLE="There are no customized products collections available";

        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE="The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to fetch a resource which doesn't exist or is not available
        /// </summary>
        private const string RESOURCE_NOT_FOUND_MESSAGE="The resource being fetched could not be found";

        /// <summary>
        /// Constant that represents the message that ocurres if an update is invalid
        /// </summary>
        private const string INVALID_UPDATE_MESSAGE="An error ocurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that ocurres if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE="The resource was updated with success";
        /// <summary>
        /// Fetches all available collections of customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult<List<CustomizedProductCollectionDTO>> findAll(){
            List<CustomizedProductCollectionDTO> customizedProductCollectionDTOS=new core.application.CustomizedProductCollectionController().findAllCollections();
            if(!Collections.isEnumerableNullOrEmpty(customizedProductCollectionDTOS))
                return Ok(customizedProductCollectionDTOS);
            return BadRequest(NO_COLLECTIONS_AVAILABLE);
        }

        /// <summary>
        /// Fetches the information of a customized product collection by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the customized product collection information</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> findByID(long id){
            CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
            customizedProductCollectionDTO.id=id;
            CustomizedProductCollectionDTO customizedProductCollection=new core.application.CustomizedProductCollectionController().findCollectionByID(customizedProductCollectionDTO);
            if(customizedProductCollection!=null)
                return Ok(customizedProductCollection);
            return BadRequest(RESOURCE_NOT_FOUND_MESSAGE);
        }

        /// <summary>
        /// Creates a new collection of customized products
        /// </summary>
        /// <param name="customizedProductCollectionDTO"></param>
        /// <returns>ActionResult with the created collection of customized products</returns>
        [HttpPost]
        public ActionResult<CustomizedProductCollectionDTO> addCustomizedProductCollection([FromBody]CustomizedProductCollectionDTO customizedProductCollectionDTO){
            try{
                CustomizedProductCollectionDTO customizedProductCollection=new core.application.CustomizedProductCollectionController().findCollectionByID(customizedProductCollectionDTO);
                return Created(Request.Path,customizedProductCollection);
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                return BadRequest(invalidArgumentsException.Message);
            }
        }

        /// <summary>
        /// Updates basic information of a certain customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information success about update</returns>
        [HttpPut("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> updateCustomizedProductCollection(long id,UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            try{
                updateCustomizedProductCollectionDTO.id=id;
                bool updatedWithSuccess=new core.application.CustomizedProductCollectionController().updateCollectionBasicInformation(updateCustomizedProductCollectionDTO);
                if(updatedWithSuccess)
                    return Ok(VALID_UPDATE_MESSAGE);
                return BadRequest(INVALID_UPDATE_MESSAGE);
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                return BadRequest(invalidArgumentsException.Message);
            }
        }

        /// <summary>
        /// Updates the customized products of a certain customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information success about update</returns>
        [HttpPut("{id}/customizedproducts")]
        public ActionResult<CustomizedProductCollectionDTO> updateCustomizedProductCollectionCustomizedProducts(long id,UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            try{
                updateCustomizedProductCollectionDTO.id=id;
                bool updatedWithSuccess=new core.application.CustomizedProductCollectionController().updateCollectionCustomizedProducts(updateCustomizedProductCollectionDTO);
                if(updatedWithSuccess)
                    return Ok(VALID_UPDATE_MESSAGE);
                return BadRequest(INVALID_UPDATE_MESSAGE);
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                return BadRequest(invalidArgumentsException.Message);
            }
        }

        /// <summary>
        /// Removes a customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the information success about the remove</returns>
        [HttpDelete("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> remove(long id){
            try{
                CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
                customizedProductCollectionDTO.id=id;
                bool disabledWithSuccess=new core.application.CustomizedProductCollectionController().disableCustomizedProductCollection(customizedProductCollectionDTO);
                if(disabledWithSuccess)
                    return NoContent();
                return BadRequest(INVALID_UPDATE_MESSAGE);
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }
    }
}