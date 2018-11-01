using core.dto;
using core.persistence;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace backend.Controllers{
    /// <summary>
    /// MVC Controller for CustomizedProductCollection operations
    /// </summary>
    [Route("/mycm/api/collections")]
    public class CustomizedProductCollectionController:Controller{
        /// <summary>
        /// Constant that represents the message that occurs if there are no collections available
        /// </summary>
        private const string NO_COLLECTIONS_AVAILABLE="There are no customized products collections available";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE="The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to fetch a resource which doesn't exist or is not available
        /// </summary>
        private const string RESOURCE_NOT_FOUND_MESSAGE="The resource being fetched could not be found";

        /// <summary>
        /// Constant that represents the message that occurs if an update is invalid
        /// </summary>
        private const string INVALID_UPDATE_MESSAGE="An error ocurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that occurs if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE="The resource was updated with success";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START="GET All Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START="GET By ID Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START="POST Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Customized Product Request starts
        /// </summary>
        private const string LOG_POST_CUSTOMIZED_PRODUCT_START="POST Customized Product Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Customized Product Request starts
        /// </summary>
        private const string LOG_DELETE_CUSTOMIZED_PRODUCT_START="DELETE Customized Product Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START="DELETE Request started";

        /// <summary>
        /// Constant that represents the log message for when a PUT Customized Products Request starts
        /// </summary>
        private const string LOG_PUT_CUSTOMIZED_PRODUCTS_START="PUT Customized Products Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST="GET All BadRequest (No Customized Product Collections Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST="GETByID({id}) BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST="POST {@collection} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST="Customized Product Collection with id {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_BAD_REQUEST="DELETE({id}) BadRequest";

        /// <summary>
        /// Constant representing the log message for when a DELETE Customized Product Request returns NotFound
        /// </summary>
        private const string LOG_DELETE_CUSTOMIZED_PRODUCT_NOT_FOUND = "DELETE({customizedProductID}) NotFound";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS="Customized Product Collections {@collectionList} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS="Customized Product Collection {@collection} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
       private const string LOG_POST_SUCCESS="Customized Product Collection {@collection} created";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
       private const string LOG_PUT_SUCCESS="Customized Product Collection with id {id} updated with info {@updateInfo}";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS="Customized Product Collection with id {id} soft deleted";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Customized Product Request is successful
        /// </summary>
        private const string LOG_DELETE_CUSTOMIZED_PRODUCT_SUCCESS = "Customized Product with id {id} deleted from the Customized Product Collection";
        
        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductCollectionRepository customizedProductCollectionRepository;

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// Controllers logger
        /// </summary>
        private readonly ILogger<CustomizedProductCollectionController> logger;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductCollectionRepository">Injected repository of customized products collections</param>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public CustomizedProductCollectionController(CustomizedProductCollectionRepository customizedProductCollectionRepository,
        CustomizedProductRepository customizedProductRepository, ILogger<CustomizedProductCollectionController> logger){
            this.customizedProductCollectionRepository=customizedProductCollectionRepository;
            this.customizedProductRepository=customizedProductRepository;
            this.logger=logger;
        }

        /// <summary>
        /// Fetches all available collections of customized products
        /// <br>Additionaly it can fetch a customized product collection by query params
        /// </summary>
        /// <returns>ActionResult with all available customized products or a customized product collection by query params</returns>
        [HttpGet]
        public ActionResult<List<CustomizedProductCollectionDTO>> findAll([FromQuery]string name){
            if(name==null){
                logger.LogInformation(LOG_GET_ALL_START);
                List<CustomizedProductCollectionDTO> customizedProductCollectionDTOS=new core.application.CustomizedProductCollectionController().findAllCollections();
                if(!Collections.isEnumerableNullOrEmpty(customizedProductCollectionDTOS))
                {
                    logger.LogInformation(LOG_GET_ALL_SUCCESS,customizedProductCollectionDTOS);
                    return Ok(customizedProductCollectionDTOS);
                }
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(NO_COLLECTIONS_AVAILABLE);
            }else{
                try{
                    logger.LogInformation(LOG_GET_BY_ID_START);
                    CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
                    customizedProductCollectionDTO.name=name;
                    CustomizedProductCollectionDTO customizedProductCollection=new core.application.CustomizedProductCollectionController().findCollectionByEID(customizedProductCollectionDTO);
                    if(customizedProductCollection!=null){
                        logger.LogInformation(LOG_GET_BY_ID_SUCCESS,customizedProductCollection);
                        return Ok(customizedProductCollection);
                    }else{
                        logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST,name);
                        return NotFound(RESOURCE_NOT_FOUND_MESSAGE);
                    }
                }catch(NullReferenceException){
                    logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST,name);
                    return NotFound(RESOURCE_NOT_FOUND_MESSAGE);
                }
            }
        }

        /// <summary>
        /// Fetches the information of a customized product collection by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the customized product collection information</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> findByID(long id){
            logger.LogInformation(LOG_GET_BY_ID_START);
            try{
                CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
                customizedProductCollectionDTO.id=id;
                CustomizedProductCollectionDTO customizedProductCollection=new core.application.CustomizedProductCollectionController().findCollectionByID(customizedProductCollectionDTO);
                if(customizedProductCollection!=null)
                {
                    logger.LogInformation(LOG_GET_BY_ID_SUCCESS,customizedProductCollection);
                    return Ok(customizedProductCollection);
                }
                logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST,id);
                return NotFound(RESOURCE_NOT_FOUND_MESSAGE);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_GET_BY_ID_BAD_REQUEST,id);
                return NotFound(RESOURCE_NOT_FOUND_MESSAGE);
            }
        }

        /// <summary>
        /// Creates a new collection of customized products
        /// </summary>
        /// <param name="customizedProductCollectionDTO"></param>
        /// <returns>ActionResult with the created collection of customized products</returns>
        [HttpPost]
        public ActionResult<CustomizedProductCollectionDTO> addCustomizedProductCollection([FromBody]CustomizedProductCollectionDTO customizedProductCollectionDTO){
            logger.LogInformation(LOG_POST_START);
            try{
                CustomizedProductCollectionDTO customizedProductCollection=new core.application.CustomizedProductCollectionController().addCollection(customizedProductCollectionDTO);
                logger.LogInformation(LOG_POST_SUCCESS,customizedProductCollection);
                return Created(Request.Path,customizedProductCollection);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,customizedProductCollectionDTO);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,customizedProductCollectionDTO);
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                logger.LogWarning(invalidArgumentsException,LOG_POST_BAD_REQUEST,customizedProductCollectionDTO);
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
        public ActionResult<CustomizedProductCollectionDTO> updateCustomizedProductCollection(long id,[FromBody]UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
          //  logger.LogInformation(LOG_PUT_BASIC_INFO_START);
            try{
                updateCustomizedProductCollectionDTO.id=id;
                bool updatedWithSuccess=new core.application.CustomizedProductCollectionController().updateCollectionBasicInformation(updateCustomizedProductCollectionDTO);
                if(updatedWithSuccess)
                {
                    logger.LogInformation(LOG_PUT_SUCCESS,id,updateCustomizedProductCollectionDTO);
                    return Ok(VALID_UPDATE_MESSAGE);
                }
                logger.LogWarning(LOG_PUT_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(INVALID_UPDATE_MESSAGE);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                logger.LogWarning(invalidArgumentsException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(invalidArgumentsException.Message);
            }
        }

        /// <summary>
        /// Adds a given customized product to the customized product collection.
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionDTO">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information regarding the update</returns>
        [HttpPost("{id}/customizedproducts")]
        public ActionResult<CustomizedProductCollectionDTO> addCustomizedProductsToCustomizedProductCollection(long id, [FromBody] UpdateCustomizedProductCollectionDTO updateCustomizedProductCollectionDTO){
            logger.LogInformation(LOG_POST_CUSTOMIZED_PRODUCT_START);
            try {
                updateCustomizedProductCollectionDTO.id = id;
                if(new core.application.CustomizedProductCollectionController().addCustomizedProductsToCustomizedProductCollection(updateCustomizedProductCollectionDTO)){
                    logger.LogInformation(LOG_POST_SUCCESS);
                    return Ok(VALID_UPDATE_MESSAGE);
                }
                return BadRequest(INVALID_UPDATE_MESSAGE);
            } catch (NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            } catch (InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(invalidOperationException.Message);
            } catch (ArgumentException argumentException){
                logger.LogWarning(argumentException,LOG_POST_BAD_REQUEST,id,updateCustomizedProductCollectionDTO);
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Removes a given customized product to the customized product collection.
        /// </summary>
        /// <param name="collectionID">Long with the customized products collection resource id</param>
        /// <param name="customizedProductID">Long with the customized product resource id</param>
        /// <returns>ActionResult with the information regarding the update</returns>
        [HttpDelete("{collectionID}/customizedproducts/{customizedProductID}")]
        public ActionResult<CustomizedProductCollectionDTO> removeCustomizedProductsToCustomizedProductCollection(long collectionID, long customizedProductID){
            logger.LogInformation(LOG_DELETE_CUSTOMIZED_PRODUCT_START);
            try {
                if(new core.application.CustomizedProductCollectionController().removeCustomizedProductsToCustomizedProductCollection(collectionID, customizedProductID)){
                    logger.LogInformation(LOG_DELETE_CUSTOMIZED_PRODUCT_SUCCESS, customizedProductID);
                    return NoContent();
                }
                logger.LogWarning(LOG_DELETE_BAD_REQUEST, customizedProductID);
                return BadRequest(INVALID_UPDATE_MESSAGE);
            } catch (ArgumentException argumentException){
                logger.LogWarning(argumentException, LOG_DELETE_CUSTOMIZED_PRODUCT_NOT_FOUND, customizedProductID);
                return NotFound(RESOURCE_NOT_FOUND_MESSAGE);
            }
        }

        /// <summary>
        /// Removes a customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the information success about the remove</returns>
        [HttpDelete("{id}")]
        public ActionResult<CustomizedProductCollectionDTO> remove(long id){
            logger.LogInformation(LOG_DELETE_START);
            try{
                CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
                customizedProductCollectionDTO.id=id;
                bool disabledWithSuccess=new core.application.CustomizedProductCollectionController().disableCustomizedProductCollection(customizedProductCollectionDTO);
                if(disabledWithSuccess){
                    logger.LogWarning(LOG_DELETE_SUCCESS,id);
                    return NoContent();
                }
                logger.LogWarning(LOG_DELETE_BAD_REQUEST,id);
                return BadRequest(INVALID_UPDATE_MESSAGE);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_DELETE_BAD_REQUEST,id);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_DELETE_BAD_REQUEST,id);
                return BadRequest(invalidOperationException.Message);
            }
        }
    }
}