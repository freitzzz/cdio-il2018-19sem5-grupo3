using core.dto;
using core.persistence;
using core.modelview.customizedproduct;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace backend.Controllers{
    /// <summary>
    /// MVC Controller for CustomizedProduct operations
    /// </summary>
    [Route("/mycm/api/customizedproducts")]
    public class CustomizedProductController:Controller{
        /// <summary>
        /// Constant that represents the message that occurs if there are no customized products available
        /// </summary>
        private const string NO_CUSTOMIZED_PRODUCTS_AVAILABLE="There are no customized products available";

        /// <summary>
        /// Constant that represents the message that occurs if a slot wasn't deleted successfully from a customized product
        /// </summary>
        private const string SLOT_NOT_DELETED="Slot wasn't deleted from customized product";

        /// <summary>
        /// Constant that represents the message that occurs if a customized product wasn't deleted from a slot
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_DELETED_FROM_SLOT="Customized product wasn't deleted from slot";

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
        private const string INVALID_UPDATE_MESSAGE="An error occurred during the update of the resource";

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
        /// Constant that represents the log message for when a PUT Request starts
        /// </summary>
        private const string LOG_PUT_START="PUT Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START="DELETE Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS="Customized Products {@customizedProducts} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS="Customized Product {@customizedProduct} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS="Customized Product {@customizedProduct} created";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS="Customized Product with id {customizedProductId} updated with info {@updateInfo}";

        /// <summary>
        /// Constant that represents the log message for when a DELETE slot from customized product request is successful
        /// </summary>
        private const string LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_SUCCESS="Slot {slotId} removed from CustomizedProduct {customizedProductId} successfully";

        /// <summary>
        /// Constant that represents the log message for when a DELETE child customized product request is successful
        /// </summary>
        private const string LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_SUCCESS="CustomizedProduct child {childId} removed from Slot {slotId} with CustomizedProduct parent {customizedProductId}";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_NOT_FOUND="GET All NotFound (No Customized Products Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_NOT_FOUND="GETByID({id}) NotFound";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST="POST {@customizedProduct} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST="Customized Product {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE slot from customized product Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_BAD_REQUEST="DELETE Slot {slotId} from Customized Product {customizedProductId} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE child customized product request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_BAD_REQUEST="DELETE Customized Product chil {childId} from Slot {slotId} with Customized Product parent {parentId} BadRequest";

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// Controllers logger
        /// </summary>
        private readonly ILogger<CustomizedProductController> logger;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public CustomizedProductController(CustomizedProductRepository customizedProductRepository, ILogger<CustomizedProductController> logger){
            this.customizedProductRepository=customizedProductRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult findAll(){
            logger.LogInformation(LOG_GET_ALL_START);
            GetAllCustomizedProductsModelView getAllModelView=new core.application.CustomizedProductController().findAllCustomizedProducts();
            if(!Collections.isEnumerableNullOrEmpty(getAllModelView.basicModelViewList))
            {
                logger.LogInformation(LOG_GET_ALL_SUCCESS,getAllModelView);
                return Ok(getAllModelView.basicModelViewList);
            }
            logger.LogWarning(LOG_GET_ALL_NOT_FOUND);
            return NotFound(new {error = NO_CUSTOMIZED_PRODUCTS_AVAILABLE });
        }

        /// <summary>
        /// Fetches the information of a customized product by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products resource id</param>
        /// <returns>ActionResult with the customized product information</returns>
        [HttpGet("{id}")]
        public ActionResult findByID(long id){
            logger.LogInformation(LOG_GET_BY_ID_START);
            try{
                GetCustomizedProductModelView customizedProductModelView=new GetCustomizedProductModelView();
                customizedProductModelView.id=id;
                GetCustomizedProductModelView fetchedCustomizedProduct=new core.application.CustomizedProductController().findCustomizedProductByID(customizedProductModelView);
                if(fetchedCustomizedProduct!=null)
                {
                    logger.LogInformation(LOG_GET_BY_ID_SUCCESS);
                    return Ok(fetchedCustomizedProduct);
                }
                logger.LogWarning(LOG_GET_BY_ID_NOT_FOUND,id);
                return NotFound(new {error = RESOURCE_NOT_FOUND_MESSAGE});
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_GET_BY_ID_NOT_FOUND,id);
                return NotFound(new {error = RESOURCE_NOT_FOUND_MESSAGE});
            }
        }

        /// <summary>
        /// Creates a new customized product
        /// </summary>
        /// <param name="customizedProductModelView">CustomizedProductDTO with the customized product being added</param>
        /// <returns>ActionResult with the created customized product</returns>
        [HttpPost]
        public ActionResult addCustomizedProduct([FromBody]PostCustomizedProductModelView customizedProductModelView){
            logger.LogInformation(LOG_POST_START);
            try{
                GetCustomizedProductModelView createdCustomizedProductModelView=new core.application.CustomizedProductController().addCustomizedProduct(customizedProductModelView);
                logger.LogInformation(LOG_POST_SUCCESS,createdCustomizedProductModelView);
                return Created(Request.Path,createdCustomizedProductModelView);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,customizedProductModelView);
                return BadRequest(new {error = INVALID_REQUEST_BODY_MESSAGE});
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,customizedProductModelView);
                return BadRequest(new {error = invalidOperationException.Message});
            }catch(ArgumentException argumentException){
                logger.LogWarning(argumentException,LOG_POST_BAD_REQUEST,customizedProductModelView);
                return BadRequest(new {error = argumentException.Message});
            }
        }

        /// <summary>
        /// Adds a new customized product to a slot of another customized product
        /// </summary>
        /// <param name="id">PID of the customized product that owns the slot</param>
        /// <param name="slotid">PID of a slot that belongs to a customized product</param>
        /// <param name="addCustomizedProductToSlotModelView">ModelView representing the customized product to be added to the slot</param>
        /// <returns>Action Result with HTTP Code 201 and the newly created Customized Product
        ///         Or Action Result with HTTP Code 401 and the error that happened during the creation of the customized product</returns>
        [HttpPost("{id}/slots/{slotid}")]
        public ActionResult addCustomizedProductToSlot(long id, long slotid, [FromBody]PostCustomizedProductToSlotModelView addCustomizedProductToSlotModelView){
            logger.LogInformation(LOG_POST_START);
            try{
                addCustomizedProductToSlotModelView.baseId = id;
                addCustomizedProductToSlotModelView.slotId = slotid;
                PostCustomizedProductModelView createdCustomizedProductModelView = new core.application.CustomizedProductController().addCustomizedProductToSlot(addCustomizedProductToSlotModelView);
                logger.LogInformation(LOG_POST_SUCCESS,createdCustomizedProductModelView);
                return Created(Request.Path,createdCustomizedProductModelView);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,addCustomizedProductToSlotModelView);
                return BadRequest(new {error = INVALID_REQUEST_BODY_MESSAGE});
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,addCustomizedProductToSlotModelView);
                return BadRequest(new {error = invalidOperationException.Message});
            }catch(ArgumentException invalidArgumentsException){
                logger.LogWarning(invalidArgumentsException,LOG_POST_BAD_REQUEST,addCustomizedProductToSlotModelView);
                return BadRequest(new {error = invalidArgumentsException.Message});
            }
        }

        /// <summary>
        /// Updates a customized product
        /// </summary>
        /// <param name="id">PID of the customized product being updated</param>
        /// <param name="updateCustomizedProductModelView">ModelView containing the update information</param>
        /// <returns>Action Result with HTTP Code 200 indicating update success
        ///         Or Action Result with HTTP Code 404 indicating an error occurred</returns>
        [HttpPut("{id}")]
        public ActionResult updateCustomizedProduct(long id, [FromBody]UpdateCustomizedProductModelView updateCustomizedProductModelView){
            logger.LogInformation(LOG_PUT_START);
            try{
                updateCustomizedProductModelView.Id = id;
                if(new core.application.CustomizedProductController().updateCustomizedProduct(updateCustomizedProductModelView)){
                    logger.LogInformation(LOG_PUT_SUCCESS,id,updateCustomizedProductModelView);
                    return Ok(new {message = VALID_UPDATE_MESSAGE});
                }
            } catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST,id,updateCustomizedProductModelView);
                return BadRequest(new {error = INVALID_UPDATE_MESSAGE});
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException, LOG_PUT_BAD_REQUEST,id,updateCustomizedProductModelView);
                return BadRequest(new {error = invalidOperationException.Message});
            }catch(ArgumentException argumentException){
                logger.LogWarning(argumentException,LOG_PUT_BAD_REQUEST,id,updateCustomizedProductModelView);
                return BadRequest(new {error = argumentException.Message});
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST,id,updateCustomizedProductModelView);
            return BadRequest(new {error = INVALID_UPDATE_MESSAGE});
        }

        /// <summary>
        /// Deletes a slot from a customized product
        /// </summary>
        /// <param name="id">PID of the customized product</param>
        /// <param name="slotid">PID of the slot being removed</param>
        /// <returns>Action Result with HTTP Code 204 if the slot is removed successfully
        ///         Or Action Result with HTTP Code 401 if the slot isn't removed successfully</returns>
        [HttpDelete("{id}/slots/{slotid}")]
        public ActionResult deleteSlotFromCustomizedProduct(long id, long slotid){
            logger.LogInformation(LOG_DELETE_START);
            DeleteSlotFromCustomizedProductModelView deleteSlotFromCustomizedProductModelView = new DeleteSlotFromCustomizedProductModelView();
            deleteSlotFromCustomizedProductModelView.customizedProductId = id;
            deleteSlotFromCustomizedProductModelView.slotId = slotid;
            try{
                if(new core.application.CustomizedProductController().deleteSlotFromCustomizedProduct(deleteSlotFromCustomizedProductModelView)){
                    logger.LogInformation(LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_SUCCESS,slotid,id);
                    return NoContent();
                }else{
                    logger.LogWarning(LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_BAD_REQUEST,slotid,id);
                    return BadRequest(new{error = SLOT_NOT_DELETED});
                }
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_BAD_REQUEST,slotid,id);
                return BadRequest(new{error = SLOT_NOT_DELETED});
            }
        }

        /// <summary>
        /// Deletes a Customized Product that's inside of a slot of another Customized Product
        /// </summary>
        /// <param name="parentid">PID of the parent customized product</param>
        /// <param name="slotid">PID of the parent's slot</param>
        /// <param name="childid">PID of the child customized product</param>
        /// <returns>Action Result with HTTP Code 204 if the child customized product is removed successfully
        ///         Or Action Result with HTTP Code 401 if the child customized product isn't removed successfully</returns>
        [HttpDelete("{parentid}/slots/{slotid}/customizedproducts/{childid}")]
        public ActionResult deleteChildCustomizedProduct(long parentid, long slotid, long childid){
            logger.LogInformation(LOG_DELETE_START);
            DeleteChildCustomizedProductModelView deleteChildCustomizedProductModelView = new DeleteChildCustomizedProductModelView();
            deleteChildCustomizedProductModelView.parentId = parentid;
            deleteChildCustomizedProductModelView.slotId = slotid;
            deleteChildCustomizedProductModelView.childId = childid;
            if(new core.application.CustomizedProductController().deleteChildCustomizedProduct(deleteChildCustomizedProductModelView)){
                logger.LogInformation(LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_SUCCESS,childid,slotid,parentid);
                return NoContent();
            }else{
                logger.LogWarning(LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_BAD_REQUEST,childid,slotid,parentid);
                return BadRequest(new{error = CUSTOMIZED_PRODUCT_NOT_DELETED_FROM_SLOT});
            }
        }
    }
}