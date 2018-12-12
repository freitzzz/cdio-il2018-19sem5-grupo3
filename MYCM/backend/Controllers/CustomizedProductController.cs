using core.dto;
using core.persistence;
using core.modelview.customizedproduct;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using core.exceptions;
using backend.utils;
using core.modelview.slot;
using core.modelview.customizeddimensions;

namespace backend.Controllers
{
    /// <summary>
    /// MVC Controller for CustomizedProduct operations
    /// </summary>
    [Route("/mycm/api/customizedproducts")]
    public class CustomizedProductController : Controller
    {
        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later.";

        /// <summary>
        /// Constant that represents the message that occurs if there are no customized products available
        /// </summary>
        private const string NO_CUSTOMIZED_PRODUCTS_AVAILABLE = "There are no customized products available";

        /// <summary>
        /// Constant that represents the message that occurs if a slot wasn't deleted successfully from a customized product
        /// </summary>
        private const string SLOT_NOT_DELETED = "Slot wasn't deleted from customized product";

        /// <summary>
        /// Constant that represents the message that occurs if a customized product wasn't deleted from a slot
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_DELETED_FROM_SLOT = "Customized product wasn't deleted from slot";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to fetch a resource which doesn't exist or is not available
        /// </summary>
        private const string RESOURCE_NOT_FOUND_MESSAGE = "The resource being fetched could not be found";

        /// <summary>
        /// Constant that represents the message that occurs if an update is invalid
        /// </summary>
        private const string INVALID_UPDATE_MESSAGE = "An error occurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that occurs if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE = "The resource was updated with success";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START = "POST Request started";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request starts
        /// </summary>
        private const string LOG_PUT_START = "PUT Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Customized Products {@customizedProducts} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Customized Product {@customizedProduct} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS = "Customized Product {@customizedProduct} created";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Customized Product with id {customizedProductId} updated with info {@updateInfo}";

        /// <summary>
        /// Constant that represents the log message for when a DELETE slot from customized product request is successful
        /// </summary>
        private const string LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_SUCCESS = "Slot {slotId} removed from CustomizedProduct {customizedProductId} successfully";

        /// <summary>
        /// Constant that represents the log message for when a DELETE child customized product request is successful
        /// </summary>
        private const string LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_SUCCESS = "CustomizedProduct child {childId} removed from Slot {slotId} with CustomizedProduct parent {customizedProductId}";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_NOT_FOUND = "GET All NotFound (No Customized Products Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_NOT_FOUND = "GETByID({id}) NotFound";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@customizedProduct} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Customized Product {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE slot from customized product Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_SLOT_FROM_CUSTOMIZED_PRODUCT_BAD_REQUEST = "DELETE Slot {slotId} from Customized Product {customizedProductId} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE child customized product request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_CHILD_CUSTOMIZED_PRODUCT_BAD_REQUEST = "DELETE Customized Product chil {childId} from Slot {slotId} with Customized Product parent {parentId} BadRequest";

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// Injected instance of CustomizedProductSerialNumberRepository.
        /// </summary>
        private readonly CustomizedProductSerialNumberRepository customizedProductSerialNumberRepository;

        /// <summary>
        /// Controllers logger
        /// </summary>
        private readonly ILogger<CustomizedProductController> logger;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        /// <param name="customizedProductSerialNumberRepository">Injected instance of CustomizedProductSerialNumberRepository.</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public CustomizedProductController(CustomizedProductRepository customizedProductRepository, CustomizedProductSerialNumberRepository customizedProductSerialNumberRepository, ILogger<CustomizedProductController> logger)
        {
            this.customizedProductRepository = customizedProductRepository;
            this.customizedProductSerialNumberRepository = customizedProductSerialNumberRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult findAll()
        {
            logger.LogInformation(LOG_GET_ALL_START);

            try
            {
                GetAllCustomizedProductsModelView getAllModelView = new core.application.CustomizedProductController().findAllCustomizedProducts();
                logger.LogInformation(LOG_GET_ALL_SUCCESS);
                return Ok(getAllModelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(LOG_GET_ALL_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("base")]
        public ActionResult findBaseCustomizedProducts()
        {
            logger.LogInformation(LOG_GET_ALL_START);

            try
            {
                GetAllCustomizedProductsModelView getAllModelView = new core.application.CustomizedProductController().findAllBaseCustomizedProducts();
                logger.LogInformation(LOG_GET_ALL_SUCCESS);
                return Ok(getAllModelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(LOG_GET_ALL_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches the information of a customized product by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products resource id</param>
        /// <returns>ActionResult with the customized product information</returns>
        [HttpGet("{id}", Name = "GetCustomizedProduct")]
        public ActionResult findByID(long id)
        {
            logger.LogInformation(LOG_GET_BY_ID_START);
            try
            {
                FindCustomizedProductModelView findCustomizedProductModelView = new FindCustomizedProductModelView();
                findCustomizedProductModelView.customizedProductId = id;
                GetCustomizedProductModelView fetchedCustomizedProduct = new core.application.CustomizedProductController().findCustomizedProduct(findCustomizedProductModelView);
                logger.LogInformation(LOG_GET_BY_ID_SUCCESS);
                return Ok(fetchedCustomizedProduct);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(LOG_GET_BY_ID_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return NotFound(new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{customizedProductId}/slots/{slotId}", Name = "GetSlot")]
        public ActionResult findSlotById(long customizedProductId, long slotId)
        {
            //logger.LogInformation(LOG_GET_SLOT_START);

            try
            {
                FindSlotModelView findSlotModelView = new FindSlotModelView();
                findSlotModelView.customizedProductId = customizedProductId;
                findSlotModelView.slotId = slotId;

                GetSlotModelView slotModelView = new core.application.CustomizedProductController().findSlot(findSlotModelView);
                //logger.LogInformation(LOG_GET_SLOT_SUCESS, slotModelView);
                return Ok(slotModelView);
            }
            catch (ResourceNotFoundException e)
            {
                //logger.LogWarning(e, LOG_GET_SLOT_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Creates a new customized product
        /// </summary>
        /// <param name="customizedProductModelView">CustomizedProductDTO with the customized product being added</param>
        /// <returns>ActionResult with the created customized product</returns>
        [HttpPost]
        [HttpPost("{customizedProductId}/slots/{slotId}/customizedproducts")]
        public ActionResult addCustomizedProduct(long? customizedProductId, long? slotId, [FromHeader]string userAuthToken, 
            [FromBody]AddCustomizedProductModelView customizedProductModelView)
        {
            logger.LogInformation(LOG_POST_START);

            if (customizedProductModelView == null)
            {
                logger.LogWarning(LOG_POST_BAD_REQUEST, customizedProductModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                customizedProductModelView.parentCustomizedProductId = customizedProductId;
                customizedProductModelView.insertedInSlotId = slotId;
                customizedProductModelView.userAuthToken = userAuthToken;

                GetCustomizedProductModelView createdCustomizedProductModelView = new core.application
                    .CustomizedProductController().addCustomizedProduct(customizedProductModelView);

                logger.LogInformation(LOG_POST_SUCCESS, createdCustomizedProductModelView);
                return CreatedAtRoute("GetCustomizedProduct", new { id = createdCustomizedProductModelView.customizedProductId }, createdCustomizedProductModelView);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, customizedProductModelView);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, customizedProductModelView);
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpPost("{id}/slots")]
        public ActionResult addSlotToCustomizedProduct(long id, [FromBody] AddCustomizedDimensionsModelView slotDimensions)
        {
            //logger.LogInformation(LOG_ADD_SLOT_START);

            if (slotDimensions == null)
            {
                //logger.LogWarning(LOG_POST_SLOT_BAD_REQUEST, slotDimensions);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                AddSlotModelView addSlotModelView = new AddSlotModelView();
                addSlotModelView.customizedProductId = id;
                addSlotModelView.slotDimensions = slotDimensions;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().addSlotToCustomizedProduct(addSlotModelView);

                return Created(Request.Path, customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        [HttpPut("{id}")]
        public ActionResult updateCustomizedProduct(long id, [FromBody] UpdateCustomizedProductModelView updateCustomizedProductModelView)
        {
            if (updateCustomizedProductModelView == null)
            {
                //logger.LogWarning(LOG_POST_SLOT_BAD_REQUEST, slotDimensions);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateCustomizedProductModelView.customizedProductId = id;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().updateCustomizedProduct(updateCustomizedProductModelView);

                return Ok(customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpPut("{customizedProductId}/slots/{slotId}")]
        public ActionResult updateSlot(long customizedProductId, long slotId, [FromBody] UpdateSlotModelView updateSlotModelView)
        {

            if (updateSlotModelView == null)
            {
                //logger.LogWarning(LOG_POST_SLOT_BAD_REQUEST, slotDimensions);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateSlotModelView.customizedProductId = customizedProductId;
                updateSlotModelView.slotId = slotId;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().updateSlot(updateSlotModelView);

                return Ok(customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{customizedProductId}")]
        public ActionResult deleteCustomizedProduct(long customizedProductId)
        {
            try
            {
                DeleteCustomizedProductModelView deleteCustomizedProductModelView = new DeleteCustomizedProductModelView();
                deleteCustomizedProductModelView.customizedProductId = customizedProductId;

                new core.application.CustomizedProductController().deleteCustomizedProduct(deleteCustomizedProductModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{customizedProductId}/slots/{slotId}")]
        public ActionResult deleteSlot(long customizedProductId, long slotId)
        {
            try
            {
                DeleteSlotModelView deleteSlotModelView = new DeleteSlotModelView();
                deleteSlotModelView.customizedProductId = customizedProductId;
                deleteSlotModelView.slotId = slotId;

                new core.application.CustomizedProductController().deleteSlot(deleteSlotModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}