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
using core.modelview.measurement;

namespace backend.Controllers {

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("mycm/api/products")]
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
        public ActionResult findAll() {
            logger.LogInformation(LOG_GET_ALL_START);
            GetAllProductsModelView allProductsModelView = new core.application.ProductController().findAllProducts();

            if (allProductsModelView == null||Collections.isEnumerableNullOrEmpty(allProductsModelView)) {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return NotFound(NO_PRODUCTS_FOUND_REFERENCE);
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, allProductsModelView);
            return Ok(allProductsModelView);
        }

        /// <summary>
        /// Finds a product by ID
        /// </summary>
        /// <param name="id"> id of the product</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult findById(long id, [FromQuery]string unit) {
            logger.LogInformation(LOG_GET_BY_ID_START);
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = id;
            fetchProductDTO.productDTOOptions = new ProductDTOOptions();
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                GetProductModelView productDTOY = new core.application.ProductController().findProductByID(fetchProductDTO);
                if (productDTOY == null) {
                    logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST + PRODUCT_NOT_FOUND_REFERENCE);
                    return NotFound(PRODUCT_NOT_FOUND_REFERENCE);
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
        /// Fetches a list of Restriction for a given Product's Measurement.
        /// </summary>
        /// <param name="productId">Product's database identifier.</param>
        /// <param name="measurementId">Measurement's database identifier.</param>
        /// <returns>ActionResult with the 200 HTTP Response Code and the list of Restriction 
        /// or the 404 HTTP Response Code if no Restriction was found.</returns>
        [HttpGet("{productId}/dimensions/{measurementId}/restrictions")]
        public ActionResult findDimensionRestrictions(long productId, long measurementId){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a list of Restriction for a given Product's Component.
        /// </summary>
        /// <param name="parentProductId">Parent Product's database identifier.</param>
        /// <param name="complementaryProductId">Child Product's database identifier.</param>
        /// <returns>ActionResult with the 200 HTTP Response Code and the list of Restriction 
        /// or the 404 HTTP Response Code if no Restriction was found.</returns>
        [HttpGet("{parentProductId}/components/{complementaryProductId}/restrictions")]
        public ActionResult findComponentRestrictions(long parentProductId, long complementaryProductId){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a list of Restriction for a given Product's Material.
        /// </summary>
        /// <param name="productId">Product's database identifier..</param>
        /// <param name="materialId">Material's database identifier.</param>
        /// <returns>ActionResult with the 200 HTTP Response Code and the list of Restriction 
        /// or the 404 HTTP Response Code if no Restriction was found.</returns>
        [HttpGet("{productId}/materials/{materialId}/restrictions")]
        public ActionResult findMaterialRestrictions(long productId, long materialId){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="addProductMV">ModelView of the Product being added.</param>
        /// <returns>HTTP Response 200 Ok if the product was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the product
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult addProduct([FromBody] AddProductModelView addProductMV) {
            logger.LogInformation(LOG_POST_START);
            try {
                GetProductModelView createdProductMV = new core.application.ProductController().addProduct(addProductMV);;
                if (createdProductMV != null) {
                    logger.LogInformation(LOG_POST_SUCCESS, createdProductMV);
                    return CreatedAtRoute("GetProduct", new { id = createdProductMV.id }, createdProductMV);
                } else {
                    //TODO:????????
                    return BadRequest();
                }
            } catch (NullReferenceException nullReferenceException) {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, addProductMV);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            } catch (InvalidOperationException invalidOperationException) {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, addProductMV);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            } catch (ArgumentException argumentException) {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, addProductMV);
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        }

        /// <summary>
        /// Adds a new component to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which component is being added</param>
        /// <param name="addComponentToProductMV">AddComponentToProductModelView with the information about the component being added</param>
        /// <returns>HTTP Response 201; Created if the component was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/components")]
        public ActionResult addComponentToProduct(long id,[FromBody]AddComponentToProductModelView addComponentToProductMV){
            addComponentToProductMV.productID=id;
            try{
                GetComponentModelView componentModelView=new core.application.ProductController().addComponentToProduct(addComponentToProductMV);
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
        /// Adds an instance of Measurement to the Product's list of Measurement.
        /// </summary>
        /// <param name="productId">Product's database identifier.</param>
        /// <param name="measurementModelView">ModelView used for adding a Measurement to a Product.</param>
        /// <returns>ActionResult with the 200 HTTP Response Code and the list of Restriction 
        /// or the 400 HTTP Response Code if the Measurement was not able to be added.</returns>
        [HttpPost("{productId}/dimensions")]
        public ActionResult addMeasurementToProduct(long productId, [FromBody] AddMeasurementModelView measurementModelView){

            AddMeasurementToProductModelView addMeasurementToProductModelView = new AddMeasurementToProductModelView();
            addMeasurementToProductModelView.productID = productId;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new material to a product
        /// </summary>
        /// <param name="id">Long with the product resource ID which material is being added</param>
        /// <param name="addMaterialToProductMV">AddMaterialToProductModelView with the information about the material being added</param>
        /// <returns>HTTP Response 201; Created if the material was added with success to the product
        ///      <br>HTTP Response 400; Bad Request if the an error occured during the add operation 
        /// </returns>
        [HttpPost("{id}/materials")]
        public ActionResult addMaterialToProduct(long id,[FromBody]AddMaterialToProductModelView addMaterialToProductMV){
            addMaterialToProductMV.productID=id;
            try{
                GetMaterialModelView materialModelView=new core.application.ProductController().addMaterialToProduct(addMaterialToProductMV);
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
        /// Adds an instance of Restriction to the Product's Material's list of Restrictions. 
        /// </summary>
        /// <param name="productId">Product's database identifier.</param>
        /// <param name="materialId">Material's database identifier.</param>
        /// <returns>ActionResult with the 200 HTTP Response Code and the list of Restriction 
        /// or the 400 HTTP Response Code if the Restriction was not able to be added.</returns>
        [HttpPost("{productId}/materials/{materialId}/restrictions")]
        public ActionResult addRestrictionToProductMaterial(long productId, long materialId){

            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Updates a product basic information
        /// </summary>
        /// <param name="updateProduct">UpdateProductModelView with the basic information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult updateProduct(long id, [FromBody] UpdateProductModelView updateProductData) {
            logger.LogInformation(LOG_PUT_BASIC_INFO_START);
            updateProductData.productId = id;
            if (new core.application.ProductController().updateProduct(updateProductData)) {
                logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductData);
                return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, id, updateProductData);
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
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
            DeleteComponentFromProductModelView deleteComponentFromProductMV=new DeleteComponentFromProductModelView();
            deleteComponentFromProductMV.productID=productID;
            deleteComponentFromProductMV.componentID=componentID;
            try{
                new core.application.ProductController().deleteComponentFromProduct(deleteComponentFromProductMV);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
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
            DeleteRestrictionFromProductComponentModelView deleteRestrictionFromProductComponentMV=new DeleteRestrictionFromProductComponentModelView();
            deleteRestrictionFromProductComponentMV.productID=productID;
            deleteRestrictionFromProductComponentMV.componentID=componentID;
            deleteRestrictionFromProductComponentMV.restrictionID=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductComponent(deleteRestrictionFromProductComponentMV);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        [HttpDelete("{productId}/dimensions/{dimensionId}")]
        public ActionResult deleteMeasurementFromProduct(long productId, long dimensionId){
            throw new NotImplementedException();
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
            DeleteMaterialFromProducModelView deleteMaterialFromProductMV=new DeleteMaterialFromProducModelView();
            deleteMaterialFromProductMV.productID=productID;
            deleteMaterialFromProductMV.materialID=materialID;
            try{
                new core.application.ProductController().deleteMaterialFromProduct(deleteMaterialFromProductMV);
                return NoContent();
            }catch(NullReferenceException){
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
        }

        [HttpDelete("{productID}/materials/{materialID}/restrictions/{restrictionId}")]
        public ActionResult deleteRestrictionFromProductMaterial(long productId, long materialId, long restrictionId){
            throw new NotImplementedException();
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
    }
}