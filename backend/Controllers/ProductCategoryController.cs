using core.persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using core.dto;
using System;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    /// <summary>
    /// REST controller being called when accessing the myc/categories resource.
    /// </summary>
    [Route("myc/api/categories")]
    public class ProductCategoryController : Controller
    {
        /// <summary>
        /// Constant representing an error that occured when attempting to add a ProductCategory with an empty request body.
        /// </summary>
        private const string ERROR_EMPTY_BODY = "Unable to add a category with an empty body";

        /// <summary>
        /// Constant representing the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START = "POST Request started";

        /// <summary>
        /// Constant representing the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";

        /// <summary>
        /// Constant representing the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started";

        /// <summary>
        /// Constant representing the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All request started";

        /// <summary>
        /// Constant representing the log message for when a GET By Name Request starts
        /// </summary>
        private const string LOG_GET_BY_NAME_START = "GET By Name request started";

        /// <summary>
        /// Constant representing the log message for when a POST Request with an empty body occurs
        /// </summary>
        private const string LOG_POST_EMPTY_BODY = "POST Request with empty body";

        /// <summary>
        /// Constant representing the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS = "Product Category {@Category} created";

        /// <summary>
        /// Constant representing the log message for when a DELETE Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS = "Product Category {@Category} soft deleted";

        /// <summary>
        /// Constant representing the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Product Category {@Category} retrieved";

        /// <summary>
        /// Constant representing the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Product Categories {@list} retrieved";

        /// <summary>
        /// Constant representing the log message for when a GET By Name Request is successful
        /// </summary>
        private const string LOG_GET_BY_NAME_SUCCESS = "Product Category {@Category} retrieved";

        /// <summary>
        /// Constant representing the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@category} BadRequest";

        /// <summary>
        /// Constant representing the log message for when a DELETE Request returns NotFound
        /// </summary>
        private const string LOG_DELETE_NOT_FOUND = "DELETE({categoryID}) NOT FOUND";

        /// <summary>
        /// Constant representing the log message for when a GET By ID Request returns NotFound
        /// </summary>
        private const string LOG_GET_BY_ID_NOT_FOUND = "GETByID({categoryID}) NOT FOUND";

        /// <summary>
        /// Constant representing the log message for when a GET All Request returns NotFound
        /// </summary>
        private const string LOG_GET_ALL_NOT_FOUND = "GET All NOT FOUND";

        /// <summary>
        /// Constant representing the log message for when a GET All Request returns NotFound
        /// </summary>
        private const string LOG_GET_BY_NAME_NOT_FOUND = "GETByName({name}) NOT FOUND";

        /// <summary>
        /// Repository being used to store instances of ProductCategory.
        /// </summary>
        private readonly ProductCategoryRepository categoryRepository;

        /// <summary>
        /// Controllers Logger
        /// </summary>
        readonly ILogger<ProductCategoryController> logger;

        /// <summary>
        /// Constructor with injected type of repository.
        /// </summary>
        /// <param name="categoryRepository">Repository to be used when storing instances of ProductCategory.</param>
        /// <param name="logger"> Controllers logger to log any information about HTTP Requests and Responses</param>
        public ProductCategoryController(ProductCategoryRepository categoryRepository, ILogger<ProductCategoryController> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }


        /// <summary>
        /// Adds a new ProductCategory from HTTP request's body data.
        /// </summary>
        /// <param name="categoryAsJson">JSON body containing the new category's information</param>
        /// <returns>ActionResult with the 201 HTTP code and the newly added category in JSON format 
        /// or the an ActionResult with the 400 HTTP code if the ProductCategory could not be added.</returns>
        [HttpPost]
        public ActionResult addProductCategory([FromBody] ProductCategoryDTO categoryAsJson)
        {
            logger.LogInformation(LOG_POST_START);
            if (categoryAsJson == null)
            {
                logger.LogWarning(LOG_POST_EMPTY_BODY);
                return BadRequest(new { error = ERROR_EMPTY_BODY });
            }

            try
            {
                ProductCategoryDTO createdCategory = new core.application.ProductCategoryController().
                addProductCategory(categoryAsJson);
                logger.LogInformation(LOG_POST_SUCCESS, createdCategory);
                return CreatedAtRoute("GetCategory", new { id = createdCategory.id }, createdCategory);

            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_POST_BAD_REQUEST,categoryAsJson);
                return BadRequest(new { error = e.Message });
            }
        }

        /// <summary>
        /// Deletes an existing ProductCategory.
        /// </summary>
        /// <param name="id">ProductCategory's database identifier.</param>
        /// <returns>ActionResult with the 204 HTTP code if the ProductCategory was removed successfully 
        /// or 404 HTTP code if no ProductCategory matching the given database identifier was found.</returns>
        [HttpDelete("{id}")]
        public ActionResult removeProductCategory(long id)
        {
            logger.LogInformation(LOG_DELETE_START);
            try
            {
                ProductCategoryDTO removedCategory = new core.application.ProductCategoryController().removeProductCategory(id);
                logger.LogInformation(LOG_DELETE_SUCCESS, removedCategory);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_DELETE_NOT_FOUND, id);
                return NotFound(new { error = e.Message });
            }
        }

        /// <summary>
        /// Retrieves information of an instance of ProductCategory from its database identifier.
        /// </summary>
        /// <param name="id">ProductCategory's database identifier.</param>
        /// <returns>ActionResult with the 200 HTTP code if an instance of ProductCategory with a matching
        /// database identifier was found or 404 HTTP code if no ProductCategory was found.</returns>
        [HttpGet("{id}", Name = "GetCategory")]
        public ActionResult findById(long id)
        {
            logger.LogInformation(LOG_GET_BY_ID_START);
            try
            {
                ProductCategoryDTO result = new core.application.
                    ProductCategoryController().findByDatabaseId(id);
                logger.LogInformation(LOG_GET_BY_ID_SUCCESS, result);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_GET_BY_ID_NOT_FOUND, id);
                return NotFound(new { error = e.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of all instances of ProductCategory or a single ProductCategory if a business identifier is given.
        /// </summary>
        /// <param name="name">ProductCategory's business idenfifier</param>
        /// <returns>ActionResult with the 200 HTTP code if an instance of ProductCategory if instance(s) were/was found or 
        /// 404 HTTP code if no ProductCategory was found.</returns>
        [HttpGet]
        public ActionResult find([FromQuery] string name)
        {
            if (name != null)
            {
                return findByName(name);
            }
            return findAll();
        }

        /// <summary>
        /// Private method that retrieves all instances of ProductCategory currently stored within the repository.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP code with any instance of ProductCategory was found</returns>
        private ActionResult findAll()
        {
            logger.LogInformation(LOG_GET_ALL_START);
            try
            {
                List<ProductCategoryDTO> result = new core.application.
                    ProductCategoryController().findAllCategories();
                logger.LogInformation(LOG_GET_ALL_SUCCESS, result);
                return Ok(result);

            }
            catch (ArgumentException e)
            {
                logger.LogInformation(LOG_GET_ALL_NOT_FOUND);
                return NotFound(new { error = e.Message });
            }
        }

        /// <summary>
        /// Retrieves information of an instance of ProductCategory from its business identifier.
        /// </summary>
        /// <param name="name">ProductCategory's business identifier.</param>
        /// <returns>ActionResult with the 200 HTTP code if an instance of ProductCategory with a matching
        /// business identifier was found or 404 HTTP code if no ProductCategory was found.</returns>
        private ActionResult findByName(string name)
        {
            logger.LogInformation(LOG_GET_BY_NAME_START);
            try
            {
                ProductCategoryDTO result = new core.application.
                    ProductCategoryController().findByName(name);
                logger.LogInformation(LOG_GET_BY_NAME_SUCCESS, result);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_GET_BY_NAME_NOT_FOUND, name);
                return NotFound(new { error = e.Message });
            }
        }

    }
}