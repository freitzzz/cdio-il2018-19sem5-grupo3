using core.modelview.customizedproductcollection;
using core.persistence;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using backend.utils;
using core.exceptions;

namespace backend.Controllers
{
    /// <summary>
    /// MVC Controller for CustomizedProductCollection operations
    /// </summary>
    [Route("/mycm/api/collections")]
    public class CustomizedProductCollectionController : Controller
    {
        /// <summary>
        /// Constant that represents the message that occurs if a client attempts to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid! Check documentation for more information";


        /// <summary>
        /// Constant that represents the message that occurs if an unexpected error happens
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later";


        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductCollectionRepository customizedProductCollectionRepository;

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductCollectionRepository">Injected repository of customized products collections</param>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        public CustomizedProductCollectionController(CustomizedProductCollectionRepository customizedProductCollectionRepository,
        CustomizedProductRepository customizedProductRepository)
        {
            this.customizedProductCollectionRepository = customizedProductCollectionRepository;
            this.customizedProductRepository = customizedProductRepository;
        }

        /// <summary>
        /// Fetches all available collections of customized products
        /// <br>Additionally it can fetch a customized product collection by query params
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
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches all available customized product collections
        /// </summary>
        /// <returns>ActionResult with all available customized product collections or an error message</returns>
        private ActionResult findAll()
        {
            try
            {
                GetAllCustomizedProductCollectionsModelView modelView = new core.application.CustomizedProductCollectionController().findAllCollections();

                return Ok(modelView);
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
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
                GetCustomizedProductCollectionModelView modelView = new GetCustomizedProductCollectionModelView();
                modelView.name = name;
                GetCustomizedProductCollectionModelView customizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().findCollectionByEID(modelView);

                return Ok(customizedProductCollectionModelView);

            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            try
            {
                GetCustomizedProductCollectionModelView modelView = new GetCustomizedProductCollectionModelView();
                modelView.id = id;
                GetCustomizedProductCollectionModelView customizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().findCollectionByID(modelView);


                return Ok(customizedProductCollectionModelView);
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (Exception)
            {
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
            if (addCustomizedProductCollectionModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                GetCustomizedProductCollectionModelView createdCustomizedProductCollection = new core.application.CustomizedProductCollectionController().addCollection(addCustomizedProductCollectionModelView);
                return Created(Request.Path, createdCustomizedProductCollection);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException invalidArgumentsException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidArgumentsException.Message));
            }
            catch (Exception)
            {
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
            if (addCustomizedProductToCustomizedProductCollectionModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                addCustomizedProductToCustomizedProductCollectionModelView.customizedProductCollectionId = id;
                GetCustomizedProductCollectionModelView updatedCustomizedProductCollectionModelView = new core.application.CustomizedProductCollectionController().addCustomizedProductToCustomizedProductCollection(addCustomizedProductToCustomizedProductCollectionModelView);

                return Created(Request.Path, updatedCustomizedProductCollectionModelView);
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
            catch (Exception)
            {
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
            if (updateCustomizedProductCollectionModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateCustomizedProductCollectionModelView.customizedProductCollectionId = id;
                GetBasicCustomizedProductCollectionModelView updatedCollection = new core.application.CustomizedProductCollectionController().updateCollectionBasicInformation(updateCustomizedProductCollectionModelView);

                return Ok(updatedCollection);
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException invalidArgumentsException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidArgumentsException.Message));
            }
            catch (Exception)
            {
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
            try
            {
                DeleteCustomizedProductFromCustomizedProductCollectionModelView modelView = new DeleteCustomizedProductFromCustomizedProductCollectionModelView();
                modelView.customizedProductCollectionId = collectionID;
                modelView.customizedProductId = customizedProductID;
                new core.application.CustomizedProductCollectionController().removeCustomizedProductFromCustomizedProductCollection(modelView);

                return NoContent();
            }
            catch (ArgumentException argumentException)
            {
                return NotFound(new SimpleJSONMessageService(argumentException.Message));
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (Exception)
            {
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
            try
            {
                DeleteCustomizedProductCollectionModelView modelView = new DeleteCustomizedProductCollectionModelView();
                modelView.customizedProductCollectionId = id;
                new core.application.CustomizedProductCollectionController().disableCustomizedProductCollection(modelView);

                return NoContent();
            }
            catch (ResourceNotFoundException resourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(resourceNotFoundException.Message));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}