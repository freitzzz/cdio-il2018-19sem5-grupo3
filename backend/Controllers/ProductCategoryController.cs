using core.persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using core.dto;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using core.modelview.productcategory;

namespace backend.Controllers
{
    /// <summary>
    /// REST controller being called when accessing the myc/categories resource.
    /// </summary>
    [Route("mycm/api/categories")]
    public class ProductCategoryController : Controller
    {
        /// <summary>
        /// Constant representing an error that occured when attempting to add a ProductCategory with an empty request body.
        /// </summary>
        private const string ERROR_EMPTY_BODY = "Unable to add a category with an empty body";

        /// <summary>
        /// Constant representing an error that occured when attempting to update a category.
        /// </summary>
        private const string ERROR_UPDATE_CATEGORY = "An error occured while attempting to update the category";

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
        /// Constant representing the log message for when a PUT Request starts
        /// </summary>
        private const string LOG_PUT_START = "PUT Request started";

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
        /// Constant representing the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Product Category {@Category} updated";

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
        /// Constant representing the log message for when a GET All Subcategories Request is successful
        /// </summary>
        private const string LOG_GET_SUBCATEGORIES_SUCCESS = "Sub Product Categories {@list} retrieved";

        /// <summary>
        /// Constant representing the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@category} BadRequest";

        /// <summary>
        /// Constant representing the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "PUT {@category} BadRequest";

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
        /// Constant representing the log message for when a GET All Sub Categories returns NotFound
        /// </summary>
        private const string LOG_GET_SUBCATEGORIES_NOT_FOUND = "GET All Subcategories NOT FOUND";

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
        public ActionResult addProductCategory([FromBody] AddProductCategoryModelView categoryAsJson)
        {
            logger.LogInformation(LOG_POST_START);
            if (categoryAsJson == null)
            {
                logger.LogWarning(LOG_POST_EMPTY_BODY);
                return BadRequest(new { error = ERROR_EMPTY_BODY });
            }

            try
            {
                GetProductCategoryModelView createdCategory = new core.application
                    .ProductCategoryController().addProductCategory(categoryAsJson);

                logger.LogInformation(LOG_POST_SUCCESS, createdCategory);
                return CreatedAtRoute("GetCategory", new { id = createdCategory.id }, createdCategory);

            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_POST_BAD_REQUEST, categoryAsJson);
                return BadRequest(new { error = e.Message });
            }
        }

        /// <summary>
        /// Adds a new sub ProductCategory from HTTP request's body data. 
        /// </summary>
        /// <param name="parentId">JSON body containing the new category's information.</param>
        /// <param name="modelView">ActionResult with the 201 HTTP code and the newly added category in JSON format 
        /// or the an ActionResult with the 400 HTTP code if the ProductCategory could not be added.</param>
        /// <returns></returns>
        [HttpPost("{parentId}/subcategories")]
        public ActionResult addSubProductCategory(long parentId, [FromBody]AddProductCategoryModelView modelView)
        {
            logger.LogInformation(LOG_POST_START);
            if (modelView == null)
            {
                logger.LogWarning(LOG_POST_EMPTY_BODY);
                return BadRequest(new { error = ERROR_EMPTY_BODY });
            }

            try
            {
                GetProductCategoryModelView createdCategory = new core.application
                    .ProductCategoryController().addSubProductCategory(parentId, modelView);

                logger.LogInformation(LOG_POST_SUCCESS, createdCategory);
                return CreatedAtRoute("GetCategory", new { id = createdCategory.id }, createdCategory);
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_POST_BAD_REQUEST, modelView);
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
                GetProductCategoryModelView removedCategory = new core.application.ProductCategoryController().removeProductCategory(id);
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
                GetProductCategoryModelView result = new core.application.
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
                List<GetBasicProductCategoryModelView> result = new core.application.
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
                GetProductCategoryModelView result = new core.application.
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

        /// <summary>
        /// Retrieves all instances of ProductCategory that are subcategories of the ProductCategory 
        /// with the given database identifier.
        /// </summary>
        /// <param name="parentId">Database identifier of the parent ProductCategory.</param>
        /// <returns>ActionResult with the 200 HTTP code if the parent ProductCategory has sub 
        /// categories or the 404 HTTP code if no sub Product Categories were found.</returns>
        [HttpGet("{parentId}/subcategories")]
        public ActionResult findSubCategories(long parentId)
        {
            logger.LogInformation(LOG_GET_BY_ID_START);
            try
            {
                List<GetBasicProductCategoryModelView> result = new core.application.
                    ProductCategoryController().findAllSubCategories(parentId);
                logger.LogInformation(LOG_GET_SUBCATEGORIES_SUCCESS, result);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                logger.LogInformation(LOG_GET_SUBCATEGORIES_NOT_FOUND);
                return NotFound(new { error = e.Message });
            }
        }


        /// <summary>
        /// Updates an instance of ProductCategory.
        /// </summary>
        /// <param name="id">Database identifier of the ProductCategory.</param>
        /// <param name="modelView">DTO containing being updated.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult updateCategory(long id, [FromBody]UpdateProductCategoryModelView modelView)
        {
            logger.LogInformation(LOG_PUT_START);

            try
            {
                GetProductCategoryModelView updatedCategoryModelView = new core.application.ProductCategoryController().updateProductCategory(id, modelView);
                logger.LogInformation(LOG_PUT_SUCCESS, updatedCategoryModelView);

                return Ok(updatedCategoryModelView);
            }
            catch (ArgumentException e)
            {
                logger.LogInformation(LOG_PUT_BAD_REQUEST);
                return BadRequest(new { error = e.Message });
            }
        }

    }
}