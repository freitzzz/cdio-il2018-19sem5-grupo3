using core.persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using core.dto;
using System;
using backend.utils;
using static backend.utils.JSONStringFormatter;

namespace backend.Controllers
{
    /// <summary>
    /// REST controller being called when accessing the myc/categories resource.
    /// </summary>
    [Route("myc/api/categories")]
    public class ProductCategoryController : Controller
    {

        /// <summary>
        /// Constant representing an error that occurred when attempting to add a ProductCategory.
        /// </summary>
        private const string ERROR_ADD_CATEGORY = "The category could not be added";

        /// <summary>
        /// Constant representing an error that occured when attempting to remove a ProductCategory
        /// </summary>
        private const string ERROR_REMOVE_CATEGORY = "The category could not be removed.";

        /// <summary>
        /// Constant representing an error that occured when attempting to find instance(s) of ProductCategory.
        /// </summary>
        private const string ERROR_NO_CATEGORIES = "No categories were found.";

        /// <summary>
        /// Repository being used to store instances of ProductCategory.
        /// </summary>
        private readonly ProductCategoryRepository categoryRepository;

        /// <summary>
        /// Constructor with injected type of repository.
        /// </summary>
        /// <param name="categoryRepository">Repository to be used when storing instances of ProductCategory.</param>
        public ProductCategoryController(ProductCategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
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

            if (categoryAsJson == null)
            {
                return BadRequest(new { error = ERROR_ADD_CATEGORY });
            }

            try
            {
                ProductCategoryDTO createdCategory = new core.application.ProductCategoryController().
                addProductCategory(categoryAsJson);

                //category was not added (probably due to a duplicate business identifier)
                if (createdCategory == null)
                {
                    return BadRequest(new { error = ERROR_ADD_CATEGORY });
                }

                return CreatedAtRoute("GetCategory", new { id = createdCategory.id }, createdCategory);

            }
            catch (ArgumentException e)
            {
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
            ProductCategoryDTO removedCategory = new core.application.
                ProductCategoryController().removeProductCategory(id);

            if (removedCategory == null)
            {
                return NotFound(new { error = ERROR_REMOVE_CATEGORY });
            }

            return NoContent();
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
            ProductCategoryDTO result = new core.application.
                ProductCategoryController().findByDatabaseId(id);

            if (result == null)
            {
                return NotFound(new { error = ERROR_NO_CATEGORIES });
            }

            return Ok(result);
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
            List<ProductCategoryDTO> result = new core.application.
                ProductCategoryController().findAllCategories();

            if (result.Count == 0)
            {
                return NotFound(new { error = ERROR_NO_CATEGORIES });
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves information of an instance of ProductCategory from its business identifier.
        /// </summary>
        /// <param name="name">ProductCategory's business identifier.</param>
        /// <returns>ActionResult with the 200 HTTP code if an instance of ProductCategory with a matching
        /// business identifier was found or 404 HTTP code if no ProductCategory was found.</returns>
        private ActionResult findByName(string name)
        {
            ProductCategoryDTO result = new core.application.
                ProductCategoryController().findByName(name);

            if (result == null)
            {
                return NotFound(new { error = ERROR_NO_CATEGORIES });
            }

            return Ok(result);
        }

    }
}