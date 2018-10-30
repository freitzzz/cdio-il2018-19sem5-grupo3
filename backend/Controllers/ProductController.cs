using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using core.application;
using core.domain;
using core.dto.options;
using support.dto;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using support.utils;
using core.persistence;
using core.dto;
using backend.utils;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace backend.Controllers {

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("myc/api/products")]
    public class ProductController : Controller {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a product
        /// is not found
        /// </summary>
        private const string PRODUCT_NOT_FOUND_REFERENCE = "Product not found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no products
        /// are found
        /// </summary>
        private const string NO_PRODUCTS_FOUND_REFERENCE = "No products found";

        /// <summary>
        /// Constant that represents the message that occurs if the update of a product fails
        /// </summary>
        private const string INVALID_PRODUCT_UPDATE_MESSAGE = "An error occured while updating the product";

        /// <summary>
        /// Constant that represents the message that occurs if the update of a product is successful
        /// </summary>
        private const string VALID_PRODUCT_UPDATE_MESSAGE = "Product was updated with success";
        /// <summary>
        /// Constant that represents the message that occurs if a client attempts to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Request started";
        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START = "POST Request started";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started";
        /// <summary>
        /// Constant that represents the log message for when a PUT Basic Info Request starts
        /// </summary>
        private const string LOG_PUT_BASIC_INFO_START = "PUT Basic Info Request started";
        /// <summary>
        /// Constant that represents the log message for when a PUT Materials Request starts
        /// </summary>
        private const string LOG_PUT_MATERIALS_START = "PUT Materials Request started";
        /// <summary>
        /// Constant that represents the log message for when a PUT Components Request starts
        /// </summary>
        private const string LOG_PUT_COMPONENTS_START = "PUT Components Request started";
        /// <summary>
        /// Constant that represents the log message for when a PUT Dimensions Request starts
        /// </summary>
        private const string LOG_PUT_DIMENSIONS_START = "PUT Dimensions Request started";
        /// <summary>
        /// Constant that represents the log message for when a PUT Product Category Request starts
        /// </summary>
        private const string LOG_PUT_PRODUCT_CATEGORY_START = "PUT Product Category Request started";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST = "GET All Bad Request (No Products Found)";
        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@product} Bad Request";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST = "GET By ID Bad Request ";
        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Product with id {productID} PUT {@updateInfo} Bad Request";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_BAD_REQUEST = "DELETE({id}) Bad Request";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Products {@list} retrieved";
        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>

        private const string LOG_POST_SUCCESS = "Product {@product} created";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Product {@product} retrieved";
        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Product with id {productID} updated with info {@updateInfo}";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS = "Product with ID {id} soft deleted";

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        /// <summary>
        /// ProductControllers logger
        /// </summary>
        private readonly ILogger<ProductController> logger;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository, ILogger<ProductController> logger) {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Finds all products
        /// </summary>
        /// <returns>HTTP Response 400 Bad Request if no products are found;
        /// HTTP Response 200 Ok with the info of all products in JSON format </returns>
        [HttpGet]
        public ActionResult<List<ProductDTO>> findAll() {
            logger.LogInformation(LOG_GET_ALL_START);
            List<ProductDTO> allProductsDTO = new core.application.ProductController().findAllProducts();

            if (allProductsDTO == null) {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(NO_PRODUCTS_FOUND_REFERENCE);
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, allProductsDTO);
            return Ok(allProductsDTO);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="jsonData">JObject with the product information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the product was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the product
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<ProductDTO> addProduct([FromBody]ProductDTO productData) {
            logger.LogInformation(LOG_POST_START);
            try {
                ProductDTO createdProductDTO = new core.application.ProductController().addProduct(productData);
                if (createdProductDTO != null) {
                    logger.LogInformation(LOG_POST_SUCCESS, createdProductDTO);
                    return CreatedAtRoute("GetProduct", new { id = createdProductDTO.id }, createdProductDTO);
                } else {
                    //TODO:????????
                    return BadRequest();
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, productData);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, productData);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            } catch (ArgumentException argumentException) {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, productData);
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        }

        /// <summary>
        /// Finds a product by ID
        /// </summary>
        /// <param name="id"> id of the product</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult<ProductDTO> findById(long id, [FromQuery]string unit) {
            logger.LogInformation(LOG_GET_BY_ID_START);
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = id;
            fetchProductDTO.productDTOOptions = new ProductDTOOptions();
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                ProductDTO productDTOY = new core.application.ProductController().findProductByID(fetchProductDTO);
                if (productDTOY == null) {
                    logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST + PRODUCT_NOT_FOUND_REFERENCE);
                    return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
                }
                logger.LogInformation(LOG_GET_BY_ID_SUCCESS, productDTOY);
                return Ok(productDTOY);
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_GET_BY_ID_BAD_REQUEST + PRODUCT_NOT_FOUND_REFERENCE);
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            } catch (ArgumentException e) {
                logger.LogWarning(e, LOG_GET_BY_ID_BAD_REQUEST);
                return BadRequest(new { error = e.Message }); //this exception should happen when converting to an unknown unit
            }
        }
        
        /// <summary>
        /// Updates a product basic information
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the basic information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult updateProductBasicInformation(long id, [FromBody] UpdateProductDTO updateProductData) {
            logger.LogInformation(LOG_PUT_BASIC_INFO_START);
            updateProductData.id = id;
            if (new core.application.ProductController().updateProductBasicInformation(updateProductData)) {
                logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Updates the materials which a product can be made of
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/materials")]
        public ActionResult updateProductMaterials(long id, [FromBody] UpdateProductDTO updateProductData) {
            logger.LogInformation(LOG_PUT_MATERIALS_START);
            updateProductData.id = id;
            try {
                if (new core.application.ProductController().updateProductMaterials(updateProductData)) {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            logger.LogInformation(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Adds a new material to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which material is being added</param>
        /// <param name="addMaterialToProductDTO">AddMaterialToProductDTO with the information about the material being added</param>
        /// <returns>HTTP Response 201; Created if the material was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/materials")]
        public ActionResult addMaterialToProduct(long id,[FromBody]AddComponentToProductDTO addMaterialToProductDTO){
            addMaterialToProductDTO.productID=id;
            try{
                MaterialDTO materialDTO=new core.application.ProductController().addMaterialToProduct();
                return Created(Request.Path,materialDTO);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a material from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which material will be deleted from</param>
        /// <param name="dimensionID">Long with the material resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the material was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the material
        /// </returns>
        [HttpDelete("{productID}/materials/{materialID}")]
        public ActionResult deleteMaterialFromProduct(long productID,long materialID){
            DeleteMaterialFromProductDTO deleteMaterialFromProductDTO=new DeleteMaterialFromProductDTO();
            deleteMaterialFromProductDTO.productID=productID;
            deleteMaterialFromProductDTO.materialID=dimensionID;
            try{
                new core.application.ProductController().deleteMaterialFromProduct(deleteMaterialFromProductDTO);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Updates the components which a product can be complemented with
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/components")]
        public ActionResult updateProductComponents(long id, [FromBody] UpdateProductDTO updateProductData) {
            logger.LogInformation(LOG_PUT_COMPONENTS_START);
            updateProductData.id = id;
            try {
                if (new core.application.ProductController().updateProductComponents(updateProductData)) {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(invalidOperationException.Message);
            }
            logger.LogInformation(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Adds a new component to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which component is being added</param>
        /// <param name="addComponentToProductDTO">AddComponentToProductDTO with the information about the component being added</param>
        /// <returns>HTTP Response 201; Created if the component was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/components")]
        public ActionResult addComponentToProduct(long id,[FromBody]AddComponentToProductDTO addComponentToProductDTO){
            addComponentToProductDTO.productID=id;
            try{
                ComponentDTO componentDTO=new core.application.ProductController().addProductComponent();
                return Created(Request.Path,componentDTO);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a component from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which component will be deleted from</param>
        /// <param name="componentID">Long with the component resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the component was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the component
        /// </returns>
        [HttpDelete("{productID}/components/{componentID}")]
        public ActionResult deleteComponentFromProduct(long productID,long componentID){
            DeleteComponentFromProductDTO deleteComponentFromProductDTO=new DeleteComponentFromProductDTO();
            deleteComponentFromProductDTO.productID=productID;
            deleteComponentFromProductDTO.componentID=componentID;
            try{
                new core.application.ProductController().deleteComponentFromProduct(deleteComponentFromProductDTO);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/dimensions")]
        public ActionResult updateProductDimensions(long id, [FromBody] UpdateProductDTO updateProductData) {
            logger.LogInformation(LOG_PUT_DIMENSIONS_START);
            updateProductData.id = id;
            try {
                if (new core.application.ProductController().updateProductDimensions(updateProductData)) {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            logger.LogInformation(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Adds a new dimension to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which dimension is being added</param>
        /// <param name="addDimensionToProductDTO">AddDimensionToProductDTO with the information about the dimension being added</param>
        /// <returns>HTTP Response 201; Created if the dimension was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/dimensions")]
        public ActionResult addDimensionToProduct(long id,[FromBody]AddDimensionToProductDTO addDimensionToProductDTO){
            addDimensionToProductDTO.productID=id;
            try{
                DimensionDTO dimensionDTO=new core.application.ProductController().addProductDimension();
                return Created(Request.Path,dimensionDTO);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a dimension from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which dimension will be deleted from</param>
        /// <param name="dimensionID">Long with the dimension resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the dimension was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the dimension
        /// </returns>
        [HttpDelete("{productID}/materials/{dimensionID}")]
        public ActionResult deleteDimensionFromProduct(long productID,long dimensionID){
            DeleteDimensionFromProductDTO deletedDimensionFromProductDTO=new DeleteDimensionFromProductDTO();
            deletedDimensionFromProductDTO.productID=productID;
            deletedDimensionFromProductDTO.dimensionID=dimensionID;
            try{
                new core.application.ProductController().deleteDimensionFromProduct(deleteMaterialFromProduct);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/category")]
        public ActionResult updateProductCategory(long id, [FromBody] UpdateProductDTO updateProductData) {
            logger.LogInformation(LOG_PUT_PRODUCT_CATEGORY_START);
            updateProductData.id = id;
            try {
                if (new core.application.ProductController().updateProductCategory(updateProductData)) {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_PUT_BAD_REQUEST, id, updateProductData);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            logger.LogInformation(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="id">Long with the product being disabled ID</param>
        /// <returns>HTTP Response 204;No Content if the product was disabled with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while disabling the product
        /// </returns>
        /// 
        [HttpDelete("{id}")]
        public ActionResult disableProduct(long id) {
            logger.LogInformation(LOG_DELETE_START);
            ProductDTO productDTO = new ProductDTO();
            productDTO.id = id;
            bool disabledWithSuccess = new core.application.ProductController().disableProduct(productDTO);
            if (disabledWithSuccess) {
                logger.LogInformation(LOG_DELETE_SUCCESS, id);
                return NoContent();
            } else {
                logger.LogWarning(LOG_DELETE_BAD_REQUEST, id);
                return BadRequest();
            }
        }
        /// <summary>
        /// Adds a restriction to product component
        /// </summary>
        /// <param name="productID">id of the product</param>
        /// <param name="productComponentID">id of the product component</param>
        /// <param name="restrictionDTO">restriction to add</param>
        /// <returns>restriction added or list of required inputs</returns>
        [HttpPut("{productID}/components/{productComponentID}/restrictions")]
        public ActionResult addComponentRestriction(long productID, long productComponentID, [FromBody] RestrictionDTO restrictionDTO) {
            try {
                return Ok(new core.application.ProductController().addComponentRestriction(productID, productComponentID, restrictionDTO));
            } catch (NullReferenceException) {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (ArgumentOutOfRangeException rangeEx) {
                return BadRequest(new SimpleJSONMessageService(rangeEx.Message));
            } catch (ArgumentNullException nullEx) {
                return BadRequest(new SimpleJSONMessageService(nullEx.Message));
            } catch (ArgumentException argEx) {
                return BadRequest(new SimpleJSONMessageService(argEx.Message));
            }
        }
    }
}