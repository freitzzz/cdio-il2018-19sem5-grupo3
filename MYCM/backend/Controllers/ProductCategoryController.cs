using core.persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using core.dto;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using core.modelview.productcategory;
using core.exceptions;

namespace backend.Controllers
{
    /// <summary>
    /// REST controller being called when accessing the myc/categories resource.
    /// </summary>
    [Route("mycm/api/categories")]
    public class ProductCategoryController : Controller
    {
        /// <summary>
        /// Constant representing an error that occurred when attempting to add a ProductCategory with an empty request body.
        /// </summary>
        private const string ERROR_EMPTY_BODY = "Unable to add a category with an empty body";

        /// <summary>
        /// Constant representing an error that occurred when attempting to update a category.
        /// </summary>
        private const string ERROR_UPDATE_CATEGORY = "An error occured while attempting to update the category";

        /// <summary>
        /// Repository being used to store instances of ProductCategory.
        /// </summary>
        private readonly ProductCategoryRepository categoryRepository;

        /// <summary>
        /// Constructor with injected type of repository.
        /// </summary>
        /// <param name="categoryRepository">Repository to be used when storing instances of ProductCategory.</param>
        public ProductCategoryController(ProductCategoryRepository categoryRepository, ILogger<ProductCategoryController> logger)
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
        public ActionResult addProductCategory([FromBody] AddProductCategoryModelView categoryAsJson)
        {
            if (categoryAsJson == null)
            {
                return BadRequest(new { error = ERROR_EMPTY_BODY });
            }

            try
            {
                GetProductCategoryModelView createdCategory = new core.application
                    .ProductCategoryController().addProductCategory(categoryAsJson);

                return CreatedAtRoute("GetCategory", new { id = createdCategory.id }, createdCategory);

            }
            catch (ArgumentException e)
            {
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
            if (modelView == null)
            {
                return BadRequest(new { error = ERROR_EMPTY_BODY });
            }

            try
            {
                GetProductCategoryModelView createdCategory = new core.application
                    .ProductCategoryController().addSubProductCategory(parentId, modelView);

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
            try
            {
                GetProductCategoryModelView removedCategory = new core.application.ProductCategoryController().removeProductCategory(id);
                return NoContent();
            }
            catch (ArgumentException e)
            {
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
            try
            {
                GetProductCategoryModelView result = new core.application.
                    ProductCategoryController().findByDatabaseId(id);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
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
            try
            {
                List<GetBasicProductCategoryModelView> result = new core.application.
                    ProductCategoryController().findAllCategories();
                return Ok(result);

            }
            catch (ArgumentException e)
            {
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
            try
            {
                GetProductCategoryModelView result = new core.application.
                    ProductCategoryController().findByName(name);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return NotFound(new { error = e.Message });
            }
        }

        /// <summary>
        /// Retrieves all the leaf instances of ProductCategory.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP code if an instance of ProductCategory if instance(s) were/was found or 
        /// 404 HTTP code if no ProductCategory was found.</returns>
        [HttpGet("leaves")]
        public ActionResult findLeaves(){
            try{
                GetAllProductCategoriesModelView result = new core.application.ProductCategoryController().findLeaves();
                return Ok(result);
            }catch(ResourceNotFoundException e){
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
            try
            {
                List<GetBasicProductCategoryModelView> result = new core.application.
                    ProductCategoryController().findAllSubCategories(parentId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
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

            try
            {
                GetProductCategoryModelView updatedCategoryModelView = new core.application.ProductCategoryController().updateProductCategory(id, modelView);

                return Ok(updatedCategoryModelView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

    }
}