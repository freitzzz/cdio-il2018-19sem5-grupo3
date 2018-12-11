using core.modelview.customizedproductcollection;
using core.persistence;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using backend.utils;

namespace backend.Controllers
{
    /// <summary>
    /// MVC Controller for CustomizedProductCollection operations
    /// </summary>
    [Route("/mycm/api/collections")]
    public class CustomizedProductCollectionController : Controller
    {
        /// <summary>
        /// Constant that represents the message that occurs if there are no collections available
        /// </summary>
        private const string NO_COLLECTIONS_AVAILABLE = "There are no customized products collections available";

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
        private const string INVALID_UPDATE_MESSAGE = "An error ocurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that occurs if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE = "The resource was updated with success";

        /// <summary>
        /// Constant that represents the message that occurs if an unexpected error happens
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By Name Request starts
        /// </summary>
        private const string LOG_GET_BY_NAME_START = "GET By Name Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START = "POST Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Customized Product Request starts
        /// </summary>
        private const string LOG_POST_CUSTOMIZED_PRODUCT_START = "POST Customized Product Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Customized Product Request starts
        /// </summary>
        private const string LOG_DELETE_CUSTOMIZED_PRODUCT_START = "DELETE Customized Product Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";

        /// <summary>
        /// Constant that represents the log message for when a PUT Customized Products Request starts
        /// </summary>
        private const string LOG_PUT_CUSTOMIZED_PRODUCTS_START = "PUT Customized Products Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST = "GET All BadRequest (No Customized Product Collections Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns Not Found
        /// </summary>
        private const string LOG_GET_BY_ID_NOT_FOUND = "GETByID({id}) Not Found";

        /// <summary>
        /// Constant that represents the log message for when a GET By Name Request returns Not Found
        /// </summary>
        private const string LOG_GET_BY_NAME_NOT_FOUND = "GETByName({name}) Not Found";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@collection} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Customized Product Collection with id {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_BAD_REQUEST = "DELETE({id}) BadRequest";

        /// <summary>
        /// Constant representing the log message for when a DELETE Customized Product Request returns NotFound
        /// </summary>
        private const string LOG_DELETE_CUSTOMIZED_PRODUCT_NOT_FOUND = "DELETE({customizedProductID}) NotFound";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Customized Product Collections {@collectionList} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Customized Product Collection {@collection} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By Name Request is successful
        /// </summary>
        private const string LOG_GET_BY_NAME_SUCCESS = "Customized Product Collection {@collection} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS = "Customized Product Collection {@collection} created";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Customized Product Collection with id {id} updated with info {@updateInfo}";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS = "Customized Product Collection with id {id} soft deleted";

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
        CustomizedProductRepository customizedProductRepository, ILogger<CustomizedProductCollectionController> logger)
        {
            this.customizedProductCollectionRepository = customizedProductCollectionRepository;
            this.customizedProductRepository = customizedProductRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches all available collections of customized products
        /// <br>Additionaly it can fetch a customized product collection by query params
        /// </summary>
        /// <returns>ActionResult with all available customized products or a customized product collection by query params</returns>
        [HttpGet]
        public ActionResult find([FromQuery]string name)
        {
            try
            {
                if (name == null)
                {
                    return findAll();
                }
                else
                {
                    return findByName(name);
                }
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches all available customized product collections
        /// </summary>
        /// <returns>ActionResult with all available customized product collections or an error message</returns>
        private ActionResult findAll()
        {
            logger.LogInformation(LOG_GET_ALL_START);
            GetAllCustomizedProductCollectionsModelView modelView = new core.application.CustomizedProductCollectionController().findAllCollections();
            if (!Collections.isEnumerableNullOrEmpty(modelView))
            {
                logger.LogInformation(LOG_GET_ALL_SUCCESS, modelView);
                return Ok(modelView);
            }
            logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
            return NotFound(new SimpleJSONMessageService(NO_COLLECTIONS_AVAILABLE));
        }

        /// <summary>
        /// Fetches a customized product collection by its name
        /// </summary>
        /// <param name="name">name of the customized product collection</param>
        /// <returns>ActionResult with the requested customized product collection or an error message</returns>
        private ActionResult findByName(string name)
        {
            try
            {
                logger.LogInformation(LOG_GET_BY_NAME_START);
                GetCustomizedProductCollectionModelView modelView = new GetCustomizedProductCollectionModelView();
                modelView.name = name;
                GetCustomizedProductCollectionModelView customizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().findCollectionByEID(modelView);
                if (customizedProductCollectionModelView != null)
                {
                    logger.LogInformation(LOG_GET_BY_NAME_SUCCESS, customizedProductCollectionModelView);
                    return Ok(customizedProductCollectionModelView);
                }
                else
                {
                    logger.LogWarning(LOG_GET_BY_NAME_NOT_FOUND, name);
                    return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_GET_BY_NAME_NOT_FOUND, name);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_GET_BY_NAME_NOT_FOUND, name);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
        }

        /// <summary>
        /// Fetches the information of a customized product collection by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with the customized product collection information</returns>
        [HttpGet("{id}")]
        public ActionResult findByID(long id)
        {
            logger.LogInformation(LOG_GET_BY_ID_START);
            try
            {
                GetCustomizedProductCollectionModelView modelView = new GetCustomizedProductCollectionModelView();
                modelView.id = id;
                GetCustomizedProductCollectionModelView customizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().findCollectionByID(modelView);
                if (customizedProductCollectionModelView != null)
                {
                    logger.LogInformation(LOG_GET_BY_ID_SUCCESS, customizedProductCollectionModelView);
                    return Ok(customizedProductCollectionModelView);
                }
                logger.LogWarning(LOG_GET_BY_ID_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_GET_BY_ID_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Creates a new collection of customized products
        /// </summary>
        /// <param name="addCustomizedProductCollectionModelView"> model view with the new customized product collection information</param>
        /// <returns>ActionResult with the created collection of customized products</returns>
        [HttpPost]
        public ActionResult addCustomizedProductCollection([FromBody]AddCustomizedProductCollectionModelView addCustomizedProductCollectionModelView)
        {
            logger.LogInformation(LOG_POST_START);
            try
            {
                if (addCustomizedProductCollectionModelView.name == null)
                {
                    logger.LogWarning(LOG_POST_BAD_REQUEST, addCustomizedProductCollectionModelView);
                    return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
                }

                GetCustomizedProductCollectionModelView createdCustomizedProductCollection = new core.application.CustomizedProductCollectionController().addCollection(addCustomizedProductCollectionModelView);
                logger.LogInformation(LOG_POST_SUCCESS, createdCustomizedProductCollection);
                return Created(Request.Path, createdCustomizedProductCollection);
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, addCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, addCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException invalidArgumentsException)
            {
                logger.LogWarning(invalidArgumentsException, LOG_POST_BAD_REQUEST, addCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(invalidArgumentsException.Message));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Adds a given customized product to the customized product collection.
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="addCustomizedProductToCustomizedProductCollectionModelView">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information regarding the update</returns>
        [HttpPost("{id}/customizedproducts")]
        public ActionResult addCustomizedProductsToCustomizedProductCollection(long id, [FromBody]AddCustomizedProductToCustomizedProductCollectionModelView addCustomizedProductToCustomizedProductCollectionModelView)
        {
            logger.LogInformation(LOG_POST_CUSTOMIZED_PRODUCT_START);
            try
            {
                addCustomizedProductToCustomizedProductCollectionModelView.customizedProductCollectionId = id;
                GetCustomizedProductCollectionModelView updatedCustomizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().addCustomizedProductToCustomizedProductCollection(addCustomizedProductToCustomizedProductCollectionModelView);
                if (updatedCustomizedProductCollectionModelView != null)
                {
                    logger.LogInformation(LOG_POST_SUCCESS);
                    return Ok(updatedCustomizedProductCollectionModelView);
                }
                return BadRequest(new SimpleJSONMessageService(INVALID_UPDATE_MESSAGE));
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, id, addCustomizedProductToCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, id, addCustomizedProductToCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, id, addCustomizedProductToCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Updates basic information of a certain customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <param name="updateCustomizedProductCollectionModelView">UpdateCustomizedProductCollection with the information about the update</param>
        /// <returns>ActionResult with the information success about update</returns>
        [HttpPut("{id}")]
        public ActionResult updateCustomizedProductCollection(long id, [FromBody]UpdateCustomizedProductCollectionModelView updateCustomizedProductCollectionModelView)
        {
            //  logger.LogInformation(LOG_PUT_BASIC_INFO_START);
            try
            {
                updateCustomizedProductCollectionModelView.customizedProductCollectionId = id;
                GetBasicCustomizedProductCollectionModelView updatedCollection = new core.application.CustomizedProductCollectionController().updateCollectionBasicInformation(updateCustomizedProductCollectionModelView);
                if (updatedCollection != null)
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateCustomizedProductCollectionModelView);
                    return Ok(updatedCollection);
                }
                logger.LogWarning(LOG_PUT_BAD_REQUEST, id, updateCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_UPDATE_MESSAGE));
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, id, updateCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, id, updateCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException invalidArgumentsException)
            {
                logger.LogWarning(invalidArgumentsException, LOG_POST_BAD_REQUEST, id, updateCustomizedProductCollectionModelView);
                return BadRequest(new SimpleJSONMessageService(invalidArgumentsException.Message));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Removes a customized product from a customized product collection.
        /// </summary>
        /// <param name="collectionID">Long with the customized products collection resource id</param>
        /// <param name="customizedProductID">Long with the customized product resource id</param>
        /// <returns>ActionResult with the information regarding the update</returns>
        [HttpDelete("{collectionID}/customizedproducts/{customizedProductID}")]
        public ActionResult removeCustomizedProductFromCustomizedProductCollection(long collectionID, long customizedProductID)
        {
            logger.LogInformation(LOG_DELETE_CUSTOMIZED_PRODUCT_START);
            try
            {
                DeleteCustomizedProductFromCustomizedProductCollectionModelView modelView = new DeleteCustomizedProductFromCustomizedProductCollectionModelView();
                modelView.customizedProductCollectionId = collectionID;
                modelView.customizedProductId = customizedProductID;
                new core.application.CustomizedProductCollectionController().removeCustomizedProductFromCustomizedProductCollection(modelView);

                logger.LogInformation(LOG_DELETE_CUSTOMIZED_PRODUCT_SUCCESS, customizedProductID);
                return NoContent();
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_DELETE_CUSTOMIZED_PRODUCT_NOT_FOUND, customizedProductID);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_DELETE_CUSTOMIZED_PRODUCT_NOT_FOUND, customizedProductID);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Disables a customized products collection
        /// </summary>
        /// <param name="id">Long with the customized products collection resource id</param>
        /// <returns>ActionResult with information about the request's success/failure</returns>
        [HttpDelete("{id}")]
        public ActionResult disableCustomizedProductCollection(long id)
        {
            logger.LogInformation(LOG_DELETE_START);
            try
            {
                DeleteCustomizedProductCollectionModelView modelView = new DeleteCustomizedProductCollectionModelView();
                modelView.customizedProductCollectionId = id;
                new core.application.CustomizedProductCollectionController().disableCustomizedProductCollection(modelView);

                logger.LogWarning(LOG_DELETE_SUCCESS, id);
                return NoContent();
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_DELETE_BAD_REQUEST, id);
                return NotFound(new SimpleJSONMessageService(RESOURCE_NOT_FOUND_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_DELETE_BAD_REQUEST, id);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}