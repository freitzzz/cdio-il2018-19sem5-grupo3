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
using core.modelview.component;
using core.modelview.dimension;
using core.modelview.material;
using core.modelview.product;
using core.modelview.restriction;
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
            GetAllProductsModelView allProductsModelView = new core.application.ProductController().findAllProducts();

            if (allProductsModelView == null||Collections.isEnumerableNullOrEmpty(allProductsModelView)) {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(NO_PRODUCTS_FOUND_REFERENCE);
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, allProductsModelView);
            return Ok(allProductsModelView);
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
                GetProductModelView productDTOY = new core.application.ProductController().findProductByID(fetchProductDTO);
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
        public ActionResult addMaterialToProduct(long id,[FromBody]AddMaterialToProductModelView addMaterialToProductDTO){
            addMaterialToProductDTO.productID=id;
            try{
                GetMaterialModelView materialModelView=new core.application.ProductController().addMaterialToProduct(addMaterialToProductDTO);
                return Created(Request.Path,materialModelView);
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
        /// <param name="materialID">Long with the material resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the material was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the material
        /// </returns>
        [HttpDelete("{productID}/materials/{materialID}")]
        public ActionResult deleteMaterialFromProduct(long productID,long materialID){
            DeleteMaterialFromProducModelView deleteMaterialFromProductDTO=new DeleteMaterialFromProducModelView();
            deleteMaterialFromProductDTO.productID=productID;
            deleteMaterialFromProductDTO.materialID=materialID;
            try{
                new core.application.ProductController().deleteMaterialFromProduct(deleteMaterialFromProductDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Adds a restriction to a product component material
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will apply to its material</param>
        /// <param name="materialID">Long with the material resource ID which restriction will be applied to</param>
        /// <param name="restrictionDTO">RestrictionDTO with the restriction information</param>
        /// <returns>HTTP Response 201; Created if the restriction was added to the product material with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while adding the restriction to the product material
        /// </returns>
        [HttpPost("{productID}/components/{componentID}/materials/{materialID}/restrictions")]
        public ActionResult addRestrictionToProductMaterial(long productID,long componentID,long materialID,[FromBody]RestrictionDTO restrictionDTO){
            AddRestrictionToProductComponentMaterialModelView addRestrictionToProductComponentMaterialDTO=new AddRestrictionToProductComponentMaterialModelView();
            addRestrictionToProductComponentMaterialDTO.productID=productID;
            addRestrictionToProductComponentMaterialDTO.componentID=componentID;
            addRestrictionToProductComponentMaterialDTO.materialID=materialID;
            addRestrictionToProductComponentMaterialDTO.restriction=restrictionDTO;
            try{
                GetRestrictionModelView appliedRestrictionModelView=new core.application.ProductController().addRestrictionToProductComponentMaterial(addRestrictionToProductComponentMaterialDTO);
                return Created(Request.Path,appliedRestrictionModelView);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a restriction from a product component material
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will be deleted from its material</param>
        /// <param name="materialID">Long with the material resource ID which restriction will be deleted from</param>
        /// <param name="restrictionID">Long with the restriction resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the restriction was deleted from the product material with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the restriction from the product material
        /// </returns>
        [HttpDelete("{productID}/materials/components/{componentID}/{materialID}/restrictions/{restrictionID}")]
        public ActionResult deleteRestrictionFromProductMaterial(long productID,long componentID,long materialID,long restrictionID){
            DeleteRestrictionFromProductComponentMaterialModelView deleteRestrictionFromProductMaterialDTO=new DeleteRestrictionFromProductComponentMaterialModelView();
            deleteRestrictionFromProductMaterialDTO.productID=productID;
            deleteRestrictionFromProductMaterialDTO.componentID=componentID;
            deleteRestrictionFromProductMaterialDTO.materialID=materialID;
            deleteRestrictionFromProductMaterialDTO.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductComponentMaterial(deleteRestrictionFromProductMaterialDTO);
                return NoContent();
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
        public ActionResult addComponentToProduct(long id,[FromBody]AddComponentToProductModelView addComponentToProductDTO){
            addComponentToProductDTO.productID=id;
            try{
                GetComponentModelView componentModelView=new core.application.ProductController().addComponentToProduct(addComponentToProductDTO);
                return Created(Request.Path,componentModelView);
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
            DeleteComponentFromProductModelView deleteComponentFromProductDTO=new DeleteComponentFromProductModelView();
            deleteComponentFromProductDTO.productID=productID;
            deleteComponentFromProductDTO.componentID=componentID;
            try{
                new core.application.ProductController().deleteComponentFromProduct(deleteComponentFromProductDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Adds a restriction to a product component
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will apply to its component</param>
        /// <param name="componentID">Long with the component resource ID which restriction will be applied to</param>
        /// <param name="restrictionDTO">RestrictionDTO with the restriction information</param>
        /// <returns>HTTP Response 201; Created if the restriction was added to the product component with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while adding the restriction to the product component
        /// </returns>
        [HttpPost("{productID}/components/{componentID}/restrictions")]
        public ActionResult addRestrictionToProductComponent(long productID,long componentID,[FromBody]RestrictionDTO restrictionDTO){
            AddRestrictionToProductComponentModelView addRestrictionToProductComponentDTO=new AddRestrictionToProductComponentModelView();
            addRestrictionToProductComponentDTO.productID=productID;
            addRestrictionToProductComponentDTO.componentID=componentID;
            addRestrictionToProductComponentDTO.restriction=restrictionDTO;
            try{
                GetRestrictionModelView appliedRestrictionModelView=new core.application.ProductController().addRestrictionToProductComponent(addRestrictionToProductComponentDTO);
                return Created(Request.Path,appliedRestrictionModelView);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a restriction from a product component
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will be deleted from its component</param>
        /// <param name="dimensionID">Long with the component resource ID which restriction will be deleted from</param>
        /// <param name="restrictionID">Long with the restriction resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the restriction was deleted from the product component with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the restriction from the product component
        /// </returns>
        [HttpDelete("{productID}/components/{componentID}/restrictions/{restrictionID}")]
        public ActionResult deleteRestrictionFromProductComponent(long productID,long componentID,long restrictionID){
            DeleteRestrictionFromProductComponentModelView deleteRestrictionFromProductComponentDTO=new DeleteRestrictionFromProductComponentModelView();
            deleteRestrictionFromProductComponentDTO.productID=productID;
            deleteRestrictionFromProductComponentDTO.componentID=componentID;
            deleteRestrictionFromProductComponentDTO.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductComponent(deleteRestrictionFromProductComponentDTO);
                return NoContent();
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
        /// Adds a new width dimension to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which dimension is being added</param>
        /// <param name="addDimensionToProductDTO">AddDimensionToProductDTO with the information about the dimension being added</param>
        /// <returns>HTTP Response 201; Created if the dimension was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/dimensions/width")]
        public ActionResult addWidthDimensionToProduct(long id,[FromBody]AddDimensionToProductModelView addDimensionToProductDTO){
            addDimensionToProductDTO.productID=id;
            try{
                GetAllDimensionsModelView updateWidthDimension=new core.application.ProductController().addWidthDimensionToProduct(addDimensionToProductDTO);
                return Created(Request.Path,updateWidthDimension);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Adds a new height dimension to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which dimension is being added</param>
        /// <param name="addDimensionToProductDTO">AddDimensionToProductDTO with the information about the dimension being added</param>
        /// <returns>HTTP Response 201; Created if the dimension was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/dimensions/width")]
        public ActionResult addHeightDimensionToProduct(long id,[FromBody]AddDimensionToProductModelView addDimensionToProductDTO){
            addDimensionToProductDTO.productID=id;
            try{
                GetAllDimensionsModelView updateHeightDimension=new core.application.ProductController().addHeightDimensionToProduct(addDimensionToProductDTO);
                return Created(Request.Path,updateHeightDimension);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Adds a new depth dimension to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which dimension is being added</param>
        /// <param name="addDimensionToProductDTO">AddDimensionToProductDTO with the information about the dimension being added</param>
        /// <returns>HTTP Response 201; Created if the dimension was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/dimensions/width")]
        public ActionResult addDepthDimensionToProduct(long id,[FromBody]AddDimensionToProductModelView addDimensionToProductDTO){
            addDimensionToProductDTO.productID=id;
            try{
                GetAllDimensionsModelView updateDepthDimension=new core.application.ProductController().addDepthDimensionToProduct(addDimensionToProductDTO);
                return Created(Request.Path,updateDepthDimension);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a width dimension from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which dimension will be deleted from</param>
        /// <param name="dimensionID">Long with the width dimension resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the width dimension was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/width/{dimensionID}")]
        public ActionResult deleteWidthDimensionFromProduct(long productID,long dimensionID){
            DeleteDimensionFromProductModelView deletedDimensionFromProductDTO=new DeleteDimensionFromProductModelView();
            deletedDimensionFromProductDTO.productID=productID;
            deletedDimensionFromProductDTO.widthDimensionID=dimensionID;
            try{
                new core.application.ProductController().deleteWidthDimensionFromProduct(deletedDimensionFromProductDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Deletes a height dimension from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which dimension will be deleted from</param>
        /// <param name="dimensionID">Long with the height dimension resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the height dimension was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/height/{dimensionID}")]
        public ActionResult deleteHeightDimensionFromProduct(long productID,long dimensionID){
            DeleteDimensionFromProductModelView deletedDimensionFromProductDTO=new DeleteDimensionFromProductModelView();
            deletedDimensionFromProductDTO.productID=productID;
            deletedDimensionFromProductDTO.heightDimensionID=dimensionID;
            try{
                new core.application.ProductController().deleteHeightDimensionFromProduct(deletedDimensionFromProductDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Deletes a depth dimension from a product
        /// </summary>
        /// <param name="productID">Long with the product resource ID which dimension will be deleted from</param>
        /// <param name="dimensionID">Long with the depth dimension resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the depth dimension was deleted with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/depth/{dimensionID}")]
        public ActionResult deleteDepthDimensionFromProduct(long productID,long dimensionID){
            DeleteDimensionFromProductModelView deletedDimensionFromProductDTO=new DeleteDimensionFromProductModelView();
            deletedDimensionFromProductDTO.productID=productID;
            deletedDimensionFromProductDTO.depthDimensionID=dimensionID;
            try{
                new core.application.ProductController().deleteDepthDimensionFromProduct(deletedDimensionFromProductDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Adds a restriction to a product width dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will apply to its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be applied to</param>
        /// <param name="restrictionDTO">RestrictionDTO with the restriction information</param>
        /// <returns>HTTP Response 201; Created if the restriction was added to the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while adding the restriction to the product dimension
        /// </returns>
        [HttpPost("{productID}/dimensions/width/{dimensionID}/restrictions")]
        public ActionResult addRestrictionToProductWidthDimension(long productID,long dimensionID,[FromBody]RestrictionDTO restrictionDTO){
            AddRestrictionToProductDimensionModelView addRestrictionToProductDimensionDTO=new AddRestrictionToProductDimensionModelView();
            addRestrictionToProductDimensionDTO.productID=productID;
            addRestrictionToProductDimensionDTO.dimensionID=dimensionID;
            addRestrictionToProductDimensionDTO.restriction=restrictionDTO;
            try{
                GetAllRestrictionsModelView updatedAppliedRestrictions=new core.application.ProductController().addRestrictionToProductWidthDimension(addRestrictionToProductDimensionDTO);
                return Created(Request.Path,updatedAppliedRestrictions);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Adds a restriction to a product height dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will apply to its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be applied to</param>
        /// <param name="restrictionDTO">RestrictionDTO with the restriction information</param>
        /// <returns>HTTP Response 201; Created if the restriction was added to the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while adding the restriction to the product dimension
        /// </returns>
        [HttpPost("{productID}/dimensions/height/{dimensionID}/restrictions")]
        public ActionResult addRestrictionToProductHeightDimension(long productID,long dimensionID,[FromBody]RestrictionDTO restrictionDTO){
            AddRestrictionToProductDimensionModelView addRestrictionToProductDimensionDTO=new AddRestrictionToProductDimensionModelView();
            addRestrictionToProductDimensionDTO.productID=productID;
            addRestrictionToProductDimensionDTO.dimensionID=dimensionID;
            addRestrictionToProductDimensionDTO.restriction=restrictionDTO;
            try{
                GetAllRestrictionsModelView updatedAppliedRestrictions=new core.application.ProductController().addRestrictionToProductHeightDimension(addRestrictionToProductDimensionDTO);
                return Created(Request.Path,updatedAppliedRestrictions);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Adds a restriction to a product depth dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will apply to its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be applied to</param>
        /// <param name="restrictionDTO">RestrictionDTO with the restriction information</param>
        /// <returns>HTTP Response 201; Created if the restriction was added to the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while adding the restriction to the product dimension
        /// </returns>
        [HttpPost("{productID}/dimensions/depth/{dimensionID}/restrictions")]
        public ActionResult addRestrictionToProductDepthDimension(long productID,long dimensionID,[FromBody]RestrictionDTO restrictionDTO){
            AddRestrictionToProductDimensionModelView addRestrictionToProductDimensionDTO=new AddRestrictionToProductDimensionModelView();
            addRestrictionToProductDimensionDTO.productID=productID;
            addRestrictionToProductDimensionDTO.dimensionID=dimensionID;
            addRestrictionToProductDimensionDTO.restriction=restrictionDTO;
            try{
                GetAllRestrictionsModelView updatedAppliedRestrictions=new core.application.ProductController().addRestrictionToProductDepthDimension(addRestrictionToProductDimensionDTO);
                return Created(Request.Path,updatedAppliedRestrictions);
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException argumentException){
                return BadRequest(argumentException.Message);
            }
        }

        /// <summary>
        /// Deletes a restriction from a product width dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will be deleted from its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be deleted from</param>
        /// <param name="restrictionID">Long with the restriction resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the restriction was deleted from the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the restriction from the product dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/width/{dimensionID}/restrictions/{restrictionID}")]
        public ActionResult deleteRestrictionFromProductWidthDimension(long productID,long dimensionID,long restrictionID){
            DeleteRestrictionFromProductDimensionModelView deleteRestrictionFromProductDimensionDTO=new DeleteRestrictionFromProductDimensionModelView();
            deleteRestrictionFromProductDimensionDTO.productID=productID;
            deleteRestrictionFromProductDimensionDTO.dimensionID=dimensionID;
            deleteRestrictionFromProductDimensionDTO.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductWidthDimension(deleteRestrictionFromProductDimensionDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }
        
        /// <summary>
        /// Deletes a restriction from a product height dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will be deleted from its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be deleted from</param>
        /// <param name="restrictionID">Long with the restriction resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the restriction was deleted from the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the restriction from the product dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/height/{dimensionID}/restrictions/{restrictionID}")]
        public ActionResult deleteRestrictionFromProductHeightDimension(long productID,long dimensionID,long restrictionID){
            DeleteRestrictionFromProductDimensionModelView deleteRestrictionFromProductDimensionDTO=new DeleteRestrictionFromProductDimensionModelView();
            deleteRestrictionFromProductDimensionDTO.productID=productID;
            deleteRestrictionFromProductDimensionDTO.dimensionID=dimensionID;
            deleteRestrictionFromProductDimensionDTO.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductHeightDimension(deleteRestrictionFromProductDimensionDTO);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// Deletes a restriction from a product depth dimension
        /// </summary>
        /// <param name="productID">Long with the product resource ID which restriction will be deleted from its dimension</param>
        /// <param name="dimensionID">Long with the dimension resource ID which restriction will be deleted from</param>
        /// <param name="restrictionID">Long with the restriction resource ID which will be deleted</param>
        /// <returns>HTTP Response 204; No Content if the restriction was deleted from the product dimension with success
        ///      <br>HTTP Response 400; Bad Request if an error occured while deleting the restriction from the product dimension
        /// </returns>
        [HttpDelete("{productID}/dimensions/depth/{dimensionID}/restrictions/{restrictionID}")]
        public ActionResult deleteRestrictionFromProductDepthDimension(long productID,long dimensionID,long restrictionID){
            DeleteRestrictionFromProductDimensionModelView deleteRestrictionFromProductDimensionDTO=new DeleteRestrictionFromProductDimensionModelView();
            deleteRestrictionFromProductDimensionDTO.productID=productID;
            deleteRestrictionFromProductDimensionDTO.dimensionID=dimensionID;
            deleteRestrictionFromProductDimensionDTO.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductDepthDimension(deleteRestrictionFromProductDimensionDTO);
                return NoContent();
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