using System;
using Microsoft.AspNetCore.Mvc;
using core.dto.options;
using core.modelview.component;
using core.modelview.material;
using core.modelview.product;
using core.modelview.restriction;
using support.utils;
using core.persistence;
using core.dto;
using backend.utils;
using Microsoft.Extensions.Logging;
using core.modelview.measurement;
using core.exceptions;
using core.modelview.productmaterial;
using core.modelview.productslotwidths;

namespace backend.Controllers
{

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("mycm/api/products")]
    public class ProductController : Controller {
        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later.";
        /// <summary>
        /// Constant that represents the error message when invalid product information is provided.
        /// </summary>
        private const string INVALID_PRODUCT_DATA = "Invalid product information. Please provide valid information.";
        /// <summary>
        /// Constant that represents the error message when invalid measurement information is provided.
        /// </summary>
        private const string INVALID_MEASUREMENT_DATA = "Invalid dimensions information. Please provide valid information.";
        /// <summary>
        /// Constant that represents the error message when invalid component information is provided.
        /// </summary>
        private const string INVALID_COMPONENT_DATA = "Invalid component information. Please provide valid information.";
        /// <summary>
        /// Constant that represents the error message when invalid material information is provided.
        /// </summary>
        private const string INVALID_MATERIAL_DATA = "Invalid material information. Please provide valid information.";
        /// <summary>
        /// Constant that represents the error message when invalid restriction information is provided.
        /// </summary>
        private const string INVALID_RESTRICTION_DATA = "Invalid restriction information. Please provide valid information.";

        //*LOG MESSAGES */
        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts.
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Products Request started.";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns Not Found.
        /// </summary>
        private const string LOG_GET_ALL_NOT_FOUND = "GET All Not Found (No Products Found).";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful.
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Products {@list} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a GET All Base Products Request starts.
        /// </summary>
        private const string LOG_GET_ALL_BASE_START = "Get All Base Products started.";
        /// <summary>
        /// Constant that represents the log message for when a GET All Base Products Request returns Not Found.
        /// </summary>
        private const string LOG_GET_ALL_BASE_NOT_FOUND = "GET All Base Products Not Found (No Base Products Found).";
        /// <summary>
        /// Constant that represents the log message for when a GET All Base Products Request is successful.
        /// </summary>
        private const string LOG_GET_ALL_BASE_SUCCESS = "Products {@list} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a GET by Reference Request starts.
        /// </summary>
        private const string LOG_GET_BY_REFERENCE_START = "Get By Reference Request Started.";
        /// <summary>
        /// Constant that represents the log message for when a GET By Reference Request returns NotFound.
        /// </summary>
        private const string LOG_GET_BY_REFERENCE_NOT_FOUND = "Get By Reference: {reference} - Not Found.";
        /// <summary>
        /// Constant that repreesents the log message for when a GET By Reference Request returns BadRequest.
        /// </summary>
        private const string LOG_GET_BY_REFERENCE_BAD_REQUEST = "Get By Reference: {reference} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a GET By Reference Request is successful
        /// </summary>
        private const string LOG_GET_BY_REFERENCE_SUCCESS = "Product {@product} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started.";
        /// <summary>
        /// Constant that represents the log message for when a GET By Request returns NotFound.
        /// </summary>
        private const string LOG_GET_BY_ID_NOT_FOUND = "GET By ID: {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST = "GET By ID: {id} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Product {@product} retrieved.";

        /// <summary>
        /// Constant that represents the log message for when a GET Product Measurements Request starts.
        /// </summary>
        private const string LOG_GET_PRODUCT_MEASUREMENTS_STARTED = "GET Product Measurements Request Started.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Measurements Request returns NotFound.
        /// </summary>
        private const string LOG_GET_PRODUCT_MEASUREMENTS_NOT_FOUND = "GET Product's {id}  Measurements - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Measurements Request is successful.
        /// </summary>
        private const string LOG_GET_PRODUCT_MEASUREMENTS_SUCCESS = "Product's {id} Measurements {@measurements} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a GET Product Materials Request starts.
        /// </summary>
        private const string LOG_GET_PRODUCT_MATERIALS_STARTED = "GET Product Materials Request Started.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Materials Request returns NotFound.
        /// </summary>
        private const string LOG_GET_PRODUCT_MATERIALS_NOT_FOUND = "GET Product's {id} Materials - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Materials request is successful.
        /// </summary>
        private const string LOG_GET_PRODUCT_MATERIALS_SUCCESS = "Product's {id} Materials {@materials} retrieved."; 


        /// <summary>
        /// Constant that represents the log message for when a GET Product Components Request starts.
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENTS_STARTED = "GET Product Components Request Started.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Components Request returns NotFound.
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENTS_NOT_FOUND = "GET Product's {id} Components - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Components Request is successful.
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENTS_SUCCESS = "Product's {id} Components {@components} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a GET Product Slot Widths Request starts.
        /// </summary>
        private const string LOG_GET_SLOT_WIDTHS_START = "GET Product Slot Widths Request Started.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Slot Widths Request returns NotFound.
        /// </summary>
        private const string LOG_GET_SLOT_WIDTHS_NOT_FOUND = "GET Product's {id} Slot Widths - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Slot Widths Request returns BadRequest.
        /// </summary>
        private const string LOG_GET_SLOT_WIDTHS_BAD_REQUEST = "GET Product's {id} Slot Widths - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Slot Widths Request is successful.
        /// </summary>
        private const string LOG_GET_SLOT_WIDTHS_SUCCESS = "Product's {id} Slot Widths {@slotWidths} retrieved.";

        /// <summary>
        /// Constant representing the log message for when a GET Product Measurement Restrictions Request starts. 
        /// </summary>
        private const string LOG_GET_MEASUREMENT_RESTRICTIONS_STARTED = "GET Product Measurement Restrictions Started.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Measurement Restrictions Request returns NotFound.
        /// </summary>
        private const string LOG_GET_MEASUREMENT_RESTRICTIONS_NOT_FOUND = "GET Product's {id} Measurement's {id} Restrictions - Not Found.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Measurement Restrictions Request is successful.
        /// </summary>
        private const string LOG_GET_MEASUREMENT_RESTRICTIONS_SUCCESS = "Product's Measurement's Restrictions {@restrictions} retrieved.";

        
        /// <summary>
        /// Constant representing the log message for when a GET Product Component Restrictions Request starts.
        /// </summary>
        private const string LOG_GET_COMPONENT_RESTRICTIONS_STARTED = "GET Product Component Restrictions Started.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Component Restrictions Request returns NotFound.
        /// </summary>
        private const string LOG_GET_COMPONENT_RESTRICTIONS_NOT_FOUND = "GET Product {id} Component {id} Restrictions - Not Found.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Component Restrictions Request is successful.
        /// </summary>
        private const string LOG_GET_COMPONENT_RESTRICTIONS_SUCCESS = "GET Product Component Restrictions {@restrictions} retrieved.";

        
        /// <summary>
        /// Constant representing the log message for when a GET Product Material Restrictions Request starts.
        /// </summary>
        private const string LOG_GET_MATERIAL_RESTRICTIONS_STARTED = "GET Product Material Restrictions Started.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Material Restrictions Request returns NotFound.
        /// </summary>
        private const string LOG_GET_MATERIAL_RESTRICTIONS_NOT_FOUND = "GET Product {id} Material {id} Restrictions - Not Found.";
        /// <summary>
        /// Constant representing the log message for when a GET Product Material Restrictions Request is successful.
        /// </summary>
        private const string LOG_GET_MATERIAL_RESTRICTIONS_SUCCESS = "GET Product Material Restrictions {@restrictions} retrieved.";


        /// <summary>
        /// Constant that represents the log message for when a POST Product Request starts.
        /// </summary>
        private const string LOG_POST_PRODUCT_START = "POST Product Request started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Product Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_PRODUCT_BAD_REQUEST = "POST {@product} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Product Request is successful
        /// </summary>
        private const string LOG_POST_PRODUCT_SUCCESS = "Product {@product} created.";


        /// <summary>
        /// Constant that represents the log message for when a POST Product Measurement Request starts.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_START = "POST Product Measurement Request started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Product Measurement Request returns NotFound.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_NOT_FOUND = "POST Product {id} Measurement Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Product Measurement Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_BAD_REQUEST = "POST Product {id} Measurement {@measurement} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Product Measurement Request is successful.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_SUCCESS = "Product Measurement {@measurement} created.";


        /// <summary>
        /// Constant that represents the log message for when a POST Component Request starts. 
        /// </summary>
        private const string LOG_POST_COMPONENT_START = "POST Component Request started.";
        /// <summary>
        /// Contant that represents the log message for when a POST Component Request returns NotFound.
        /// </summary>
        private const string LOG_POST_COMPONENT_NOT_FOUND = "POST Product {id} Component Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Component Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_COMPONENT_BAD_REQUEST = "POST Product {id} Component {@component} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Component Request is successful.
        /// </summary>
        private const string LOG_POST_COMPONENT_SUCCESS = "Component {@component} added.";


        /// <summary>
        /// Constant that represents the log message for when a POST Material Request starts.
        /// </summary>
        private const string LOG_POST_MATERIAL_START = "POST Material Request started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Request returns NotFound.
        /// </summary>
        private const string LOG_POST_MATERIAL_NOT_FOUND = "POST Product {id} Material Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_MATERIAL_BAD_REQUEST = "POST Product {id} Material {@material} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Request is successful.
        /// </summary>
        private const string LOG_POST_MATERIAL_SUCCESS = "Material {@material} added.";


        /// <summary>
        /// Constant that represents the log message for when a POST Measurement Restriction Request starts.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_RESTRICTION_START = "POST Measurement Restriction Request started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Measurement Restriction Request returns NotFound.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_RESTRICTION_NOT_FOUND = "POST Product {id} Measurement {id} Restriction Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Measurement Restriction Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_RESTRICTION_BAD_REQUEST = "POST Product {id} Measurement  {id} Restriction {@restriction} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Measurement Restriction Request is successful.
        /// </summary>
        private const string LOG_POST_MEASUREMENT_RESTRICTION_SUCCESS = "Measurement Restriction {@restriction} added.";


        /// <summary>
        /// Constant that represents the log message for when a POST Component Restriction Request starts.
        /// </summary>
        private const string LOG_POST_COMPONENT_RESTRICTION_START = "POST Component Restriction Request started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Component Restriction Request returns NotFound.
        /// </summary>
        private const string LOG_POST_COMPONENT_RESTRICTION_NOT_FOUND = "POST Product {id} Component {id} Restriction Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Component Restriction Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_COMPONENT_RESTRICTION_BAD_REQUEST = "POST Product {id} Component {id} Restriction {@restriction} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Component Restriction Request is successful.
        /// </summary>
        private const string LOG_POST_COMPONENT_RESTRICTION_SUCCESS = "Component Restriction {@restriction} added.";


        /// <summary>
        /// Constant that represents the log message for when a POST Material Restriction Request starts.
        /// </summary>
        private const string LOG_POST_MATERIAL_RESTRICTION_START = "POST Material Restriction started.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Restriction Request returns NotFound.
        /// </summary>
        private const string LOG_POST_MATERIAL_RESTRICTION_NOT_FOUND = "POST Product {id} Material {id} Restriction Request - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Restriction Request returns BadRequest.
        /// </summary>
        private const string LOG_POST_MATERIAL_RESTRICTION_BAD_REQUEST = "POST Product {id} Material {id} Restriction {@restriction} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a POST Material Restriction Request is successful.
        /// </summary>
        private const string LOG_POST_MATERIAL_RESTRICTION_SUCCESS = "Material Restriction {@restriction} added.";


        /// <summary>
        /// Constant that represents the log message for when a PUT Product Request starts.
        /// </summary>
        private const string LOG_PUT_START = "PUT Request started.";
        /// <summary>
        /// Constant that represents the log message for when a PUT Product Request returns NotFound.
        /// </summary>
        private const string LOG_PUT_NOT_FOUND = "Product with id {productID} PUT {@updateInfo} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest.
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Product with id {productID} PUT {@updateInfo} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful.
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Product with id {productID} updated with info {@updateInfo}";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Request returns the NotFound HTTP code.
        /// </summary>
        private const string LOG_DELETE_NOT_FOUND = "DELETE({id}) - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS = "Product with ID {id} soft deleted";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Request starts.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_START = "DELETE Product Measurement Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_NOT_FOUND = "DELETE Product {id} Measurement {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Request returns BadRequest.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_BAD_REQUEST = "DELETE Product {id} Measurement {id} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Request is successful.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_SUCCESS = "DELETE Product {id} Measurement {id} - Success.";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component request starts.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_START = "DELETE Product Component Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_NOT_FOUND = "DELETE Product {id} Component {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component Request is successful.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_SUCCESS = "DELETE Product {id} Component {id} - Success.";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material request starts.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_START = "DELETE Product Material Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_NOT_FOUND = "DELETE Product {id} Material {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Request returns BadRequest.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_BAD_REQUEST = "DELETE Product {id} Material {id} - Bad Request.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Request is successful.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_SUCCESS = "DELETE Product {id} Material {id} - Success.";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Restriction request starts.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_RESTRICTION_START = "DELETE Product Measurement Restriction Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Restriction request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_RESTRICTION_NOT_FOUND = "DELETE Product {id} Measurement {id} Restriction {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Measurement Restriction Request is successful.
        /// </summary>
        private const string LOG_DELETE_MEASUREMENT_RESTRICTION_SUCCESS = "DELETE Product {id} Measurement {id} Restriction {id} - Success.";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component Restriction request starts.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_RESTRICTION_START = "DELETE Product Component Restriction Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component Restriction request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_RESTRICTION_NOT_FOUND = "DELETE Product {id} Component {id} Restriction {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Component Restriction Request is successful.
        /// </summary>
        private const string LOG_DELETE_COMPONENT_RESTRICTION_SUCCESS = "DELETE Product {id} Component {id} Restriction {id} - Success.";


        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Restriction request starts.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_RESTRICTION_START = "DELETE Product Material Restriction Request started.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Restriction request returns NotFound.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_RESTRICTION_NOT_FOUND = "DELETE Product {id} Material {id} Restriction {id} - Not Found.";
        /// <summary>
        /// Constant that represents the log message for when a DELETE Product Material Restriction Request is successful.
        /// </summary>
        private const string LOG_DELETE_MATERIAL_RESTRICTION_SUCCESS = "DELETE Product {id} Material {id} Restriction {id} - Success.";

        /// <summary>
        /// Constant that represents the log message for when a GET Product Component Request starts
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENT_STARTED = "GET Component {componentId} of Product {id} Request Started";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Component Request returns Not Found
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENT_NOT_FOUND = "Component {componentId} of Product {id} Not Found";
        /// <summary>
        /// Constant that represents the log message for when a GET Product Component Request is successful
        /// </summary>
        private const string LOG_GET_PRODUCT_COMPONENT_SUCCESS = "Component {componentId} of Product {id} retrieved {@modelView}";


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
        /// Finds all Products in the repository or, if a reference is specified, retrieves a Product with a matching reference.
        /// </summary>
        /// <param name="reference">The product's reference.</param>
        /// <param name="unit">The product's dimensions unit.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult find([FromQuery]string reference, [FromQuery]string unit) {
            if(reference == null){
                return findAll();
            }

            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.reference = reference;
            ProductDTOOptions options = new ProductDTOOptions();
            options.requiredUnit = unit;

            return findByReference(fetchProductDTO);
        }

        /// <summary>
        /// Finds all products in the repository.
        /// </summary>
        /// <returns>HTTP Response 404 Not Found if no products are found;
        /// HTTP Response 200 Ok with the info of all products in JSON format </returns>
        private ActionResult findAll(){
            logger.LogInformation(LOG_GET_ALL_START);
            try{
                GetAllProductsModelView allProductsModelView = new core.application.ProductController().findAllProducts();
                logger.LogInformation(LOG_GET_ALL_SUCCESS, allProductsModelView);
                return Ok(allProductsModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_ALL_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Finds a Product with a matching reference.
        /// </summary>
        /// <param name="fetchProductDTO"></param>
        /// <returns></returns>
        private ActionResult findByReference(FetchProductDTO fetchProductDTO){
            logger.LogInformation(LOG_GET_BY_REFERENCE_START);
            try{
                GetProductModelView getProductModelView = new core.application.ProductController().findProduct(fetchProductDTO);
                logger.LogInformation(LOG_GET_BY_REFERENCE_SUCCESS, getProductModelView);
                return Ok(getProductModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_BY_REFERENCE_NOT_FOUND, fetchProductDTO.reference);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                //this exception may occur if the specified unit does not exist
                logger.LogWarning(e, LOG_GET_BY_REFERENCE_BAD_REQUEST, fetchProductDTO.reference);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Finds all the base Products in the repository.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP Code or the 404 HTTP Code if no Product was found.</returns>
        [HttpGet("base")]
        public ActionResult findBaseProducts(){
            logger.LogInformation(LOG_GET_ALL_BASE_START);
            try{
                GetAllProductsModelView allBaseProductsModelView = new core.application.ProductController().findBaseProducts();
                logger.LogInformation(LOG_GET_ALL_BASE_SUCCESS, allBaseProductsModelView);
                return Ok(allBaseProductsModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_ALL_BASE_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Finds a product by ID
        /// </summary>
        /// <param name="id">The product's persistence identifier.</param>
        /// <param name="unit">The product's dimensions unit.</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult findById(long id, [FromQuery]string unit) {
            logger.LogInformation(LOG_GET_BY_ID_START);
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = id;
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                GetProductModelView getProductModelView = new core.application.ProductController().findProduct(fetchProductDTO);
                logger.LogInformation(LOG_GET_BY_ID_SUCCESS, getProductModelView);
                return Ok(getProductModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_BY_ID_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                //this exception may occur if the specified unit does not exist
                logger.LogWarning(e, LOG_GET_BY_ID_BAD_REQUEST, id);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/dimensions")]
        public ActionResult findProductMeasurements(long productId, [FromQuery] string unit){
            logger.LogInformation(LOG_GET_PRODUCT_MEASUREMENTS_STARTED);
            FetchProductDTO fetchProductDTO = new FetchProductDTO(){id = productId};
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try{
                GetAllMeasurementsModelView allMeasurementsModelView = 
                    new core.application.ProductController().findProductMeasurements(fetchProductDTO);
                logger.LogInformation(LOG_GET_PRODUCT_MEASUREMENTS_SUCCESS, productId, allMeasurementsModelView);
                return Ok(allMeasurementsModelView);
            }catch(ArgumentException e){
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_PRODUCT_MEASUREMENTS_NOT_FOUND, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/components")]
        public ActionResult findProductComponents(long productId, [FromQuery]FindComponentsOptions groupBy){
            logger.LogInformation(LOG_GET_PRODUCT_COMPONENTS_STARTED);
            try{
                    FindComponentsModelView findComponentsModel = new FindComponentsModelView();
                    findComponentsModel.fatherProductId = productId;
                    findComponentsModel.option = groupBy;
                    GetAllComponentsModelView allComponentsByCategory = new core.application.ProductController().findProductComponents(findComponentsModel);
                    return Ok(allComponentsByCategory);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_PRODUCT_COMPONENTS_NOT_FOUND, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/materials")]
        public ActionResult findProductMaterials(long productId){
            logger.LogInformation(LOG_GET_PRODUCT_MATERIALS_STARTED);
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = productId;
            try{
                GetAllMaterialsModelView allMaterialsModelView = new core.application.ProductController().findProductMaterials(fetchProductDTO);
                logger.LogInformation(LOG_GET_PRODUCT_MATERIALS_SUCCESS, productId, allMaterialsModelView);
                return Ok(allMaterialsModelView);
            }
            catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_PRODUCT_MATERIALS_NOT_FOUND, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/components/{componentId}")]
        public ActionResult findProductComponent(long productId, long componentId, [FromQuery]string unit){
            logger.LogInformation(LOG_GET_PRODUCT_COMPONENT_STARTED);
            try{
                FindComponentModelView findComponentModelView = new FindComponentModelView();
                findComponentModelView.fatherProductId = productId;
                findComponentModelView.childProductId = componentId;
                findComponentModelView.unit = unit;
                GetComponentModelView componentModelView = new core.application.ProductController().findProductComponent(findComponentModelView);
                logger.LogInformation(LOG_GET_PRODUCT_COMPONENT_SUCCESS,componentId, productId, componentModelView);
                return Ok(componentModelView);
            }
            catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_PRODUCT_COMPONENT_NOT_FOUND, componentId, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        //TODO: add integration tests
        /// <summary>
        /// Retrieves a Product's Slot Widths.
        /// </summary>
        /// <param name="productId">Product's persistence identifier.</param>
        /// <param name="unit">Unit in which the values will be presented.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP code and body containing the Product's Slot Widths.
        /// or
        /// ActionResult with the 404 HTTP code and body with an error message, if no Product matches the given identifier.
        /// or
        /// ActionResult with th 400 HTTP code and body with an error message, if the Product does not support slots.
        /// </returns>
        [HttpGet("{productId}/slotwidths")]
        public ActionResult findProductSlotWidths(long productId, [FromQuery] string unit){
            //*logger here */
            logger.LogInformation(LOG_GET_SLOT_WIDTHS_START);

            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = productId;
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try{
                GetProductSlotWidthsModelView productSlotWidthsMV = new core.application.ProductController().findProductSlotWidths(fetchProductDTO);
                //*logger here */
                logger.LogInformation(LOG_GET_SLOT_WIDTHS_SUCCESS);
                return Ok(productSlotWidthsMV);
            }catch(ResourceNotFoundException e){
                //*logger here */
                logger.LogWarning(e, LOG_GET_SLOT_WIDTHS_NOT_FOUND, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(InvalidOperationException e){
                //*logger here */
                logger.LogWarning(e, LOG_GET_SLOT_WIDTHS_BAD_REQUEST, productId);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                //*logger here */
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
        public ActionResult findMeasurementRestrictions(long productId, long measurementId){
            logger.LogInformation(LOG_GET_MEASUREMENT_RESTRICTIONS_STARTED);
            FindMeasurementModelView productMeasurementMV = new FindMeasurementModelView();
            productMeasurementMV.productId = productId;
            productMeasurementMV.measurementId = measurementId;
            try{
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findMeasurementRestrictions(productMeasurementMV);
                logger.LogInformation(LOG_GET_MEASUREMENT_RESTRICTIONS_SUCCESS, restrictionModelViews);
                return Ok(restrictionModelViews);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_MEASUREMENT_RESTRICTIONS_NOT_FOUND, productId, measurementId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
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
            logger.LogInformation(LOG_GET_COMPONENT_RESTRICTIONS_STARTED);
            
            FindComponentModelView componentModelView = new FindComponentModelView();
            componentModelView.fatherProductId = parentProductId;
            componentModelView.childProductId = complementaryProductId;

            try{
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findComponentRestrictions(componentModelView);
                logger.LogInformation(LOG_GET_COMPONENT_RESTRICTIONS_SUCCESS, restrictionModelViews);
                return Ok(restrictionModelViews);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_GET_COMPONENT_RESTRICTIONS_NOT_FOUND, parentProductId, complementaryProductId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
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
            logger.LogInformation(LOG_GET_MATERIAL_RESTRICTIONS_STARTED);

            FindProductMaterialModelView productMaterialModelView = new FindProductMaterialModelView();
            productMaterialModelView.productId = productId;
            productMaterialModelView.materialId = materialId;

            try{
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findMaterialRestrictions(productMaterialModelView);
                logger.LogInformation(LOG_GET_MATERIAL_RESTRICTIONS_SUCCESS, restrictionModelViews);
                return Ok(restrictionModelViews);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(LOG_GET_MATERIAL_RESTRICTIONS_NOT_FOUND, productId, materialId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
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
            logger.LogInformation(LOG_POST_PRODUCT_START);

            if(addProductMV == null){
                logger.LogWarning(LOG_POST_PRODUCT_BAD_REQUEST, addProductMV);
                return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_DATA));
            }

            try {
                GetProductModelView createdProductMV = new core.application.ProductController().addProduct(addProductMV);
                logger.LogInformation(LOG_POST_PRODUCT_SUCCESS, createdProductMV);
                return CreatedAtRoute("GetProduct", new { id = createdProductMV.productId }, createdProductMV);
            }catch(ArgumentException e){
                logger.LogWarning(e, LOG_POST_PRODUCT_BAD_REQUEST, addProductMV);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            logger.LogInformation(LOG_POST_MEASUREMENT_START);

            if(measurementModelView == null){
                logger.LogWarning(LOG_POST_MEASUREMENT_BAD_REQUEST, productId, measurementModelView);
                return BadRequest(new SimpleJSONMessageService(INVALID_MEASUREMENT_DATA));    
            }

            measurementModelView.productId = productId;

            try{
                GetProductModelView productModelView = new core.application.ProductController().addMeasurementToProduct(measurementModelView);
                logger.LogInformation(LOG_POST_MEASUREMENT_SUCCESS, measurementModelView);
                return Created(Request.Path, productModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_POST_MEASUREMENT_NOT_FOUND, productId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(e, LOG_POST_MEASUREMENT_BAD_REQUEST, productId, measurementModelView);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
        public ActionResult addComponentToProduct(long id,[FromBody]AddComponentModelView addComponentToProductMV){
            logger.LogInformation(LOG_POST_COMPONENT_START);

            if(addComponentToProductMV == null){
                logger.LogWarning(LOG_POST_COMPONENT_BAD_REQUEST, id, addComponentToProductMV);
                return BadRequest(new SimpleJSONMessageService(INVALID_COMPONENT_DATA));
            }

            addComponentToProductMV.fatherProductId = id;
            try{
                GetProductModelView productModelView=new core.application.ProductController().addComponentToProduct(addComponentToProductMV);
                logger.LogInformation(LOG_POST_COMPONENT_SUCCESS, productModelView);
                return Created(Request.Path,productModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_POST_COMPONENT_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(e, LOG_POST_COMPONENT_BAD_REQUEST, id, addComponentToProductMV);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
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
        public ActionResult addMaterialToProduct(long id,[FromBody]AddProductMaterialModelView addMaterialToProductMV){
            logger.LogInformation(LOG_POST_MATERIAL_START);

            if(addMaterialToProductMV == null){
                logger.LogWarning(LOG_POST_MATERIAL_BAD_REQUEST, id, addMaterialToProductMV);
                return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_DATA));
            }

            addMaterialToProductMV.productId=id;
            try{
                GetProductModelView productModelView=new core.application.ProductController().addMaterialToProduct(addMaterialToProductMV);
                logger.LogInformation(LOG_POST_MATERIAL_SUCCESS, productModelView);
                return Created(Request.Path,productModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_POST_MATERIAL_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(e, LOG_POST_MATERIAL_BAD_REQUEST, id, addMaterialToProductMV);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpPost("{productId}/dimensions/{measurementId}/restrictions")]
        public ActionResult addRestrictionToProductMeasurement(long productId, long measurementId, [FromBody]RestrictionDTO restrictionDTO){
            logger.LogInformation(LOG_POST_MEASUREMENT_RESTRICTION_START);

            if(restrictionDTO == null){
                logger.LogWarning(LOG_POST_MEASUREMENT_BAD_REQUEST, productId, measurementId, restrictionDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
            }

            AddMeasurementRestrictionModelView addRestrictionToProductMeasurementMV = new AddMeasurementRestrictionModelView();
            addRestrictionToProductMeasurementMV.productId = productId;
            addRestrictionToProductMeasurementMV.measurementId = measurementId;
            addRestrictionToProductMeasurementMV.restriction = restrictionDTO;
        
            try{
                GetProductModelView productModelView = new core.application.ProductController().addRestrictionToProductMeasurement(addRestrictionToProductMeasurementMV);
                logger.LogInformation(LOG_POST_MEASUREMENT_RESTRICTION_SUCCESS, productModelView);
                return Created(Request.Path, productModelView);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(LOG_POST_MEASUREMENT_RESTRICTION_NOT_FOUND, productId, measurementId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(LOG_POST_MEASUREMENT_RESTRICTION_BAD_REQUEST, productId, measurementId, addRestrictionToProductMeasurementMV);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            logger.LogInformation(LOG_POST_COMPONENT_RESTRICTION_START);

            if(restrictionDTO == null){
                logger.LogWarning(LOG_POST_COMPONENT_RESTRICTION_BAD_REQUEST, productID, componentID, restrictionDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
            }

            AddComponentRestrictionModelView addRestrictionToProductComponentDTO=new AddComponentRestrictionModelView();
            addRestrictionToProductComponentDTO.fatherProductId=productID;
            addRestrictionToProductComponentDTO.childProductId=componentID;
            addRestrictionToProductComponentDTO.restriction=restrictionDTO;
            try{
                GetProductModelView appliedRestrictionModelView=new core.application.ProductController().addRestrictionToProductComponent(addRestrictionToProductComponentDTO);
                logger.LogInformation(LOG_POST_COMPONENT_RESTRICTION_SUCCESS, appliedRestrictionModelView);
                return Created(Request.Path,appliedRestrictionModelView);
            }catch(ResourceNotFoundException e) {
                logger.LogWarning(LOG_POST_COMPONENT_RESTRICTION_NOT_FOUND, productID, componentID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(LOG_POST_COMPONENT_RESTRICTION_BAD_REQUEST, productID, componentID, addRestrictionToProductComponentDTO);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
        public ActionResult addRestrictionToProductMaterial(long productId, long materialId, RestrictionDTO restrictionDTO){
            logger.LogInformation(LOG_POST_MATERIAL_RESTRICTION_START);

                if(restrictionDTO == null){
                    logger.LogWarning(LOG_POST_MATERIAL_RESTRICTION_BAD_REQUEST, productId, materialId, restrictionDTO);
                    return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
                }

                AddProductMaterialRestrictionModelView addRestrictionToProductMaterialMV = new AddProductMaterialRestrictionModelView();
                addRestrictionToProductMaterialMV.productId = productId;
                addRestrictionToProductMaterialMV.materialId = materialId;
                addRestrictionToProductMaterialMV.restriction = restrictionDTO;

                try{
                    GetProductModelView productModelView = new core.application.ProductController().addRestrictionToProductMaterial(addRestrictionToProductMaterialMV);
                    logger.LogInformation(LOG_POST_MATERIAL_RESTRICTION_SUCCESS);
                    return Created(Request.Path,productModelView);
                }catch(ResourceNotFoundException e){
                    logger.LogWarning(LOG_POST_MATERIAL_RESTRICTION_NOT_FOUND, productId, materialId);
                    return NotFound(new SimpleJSONMessageService(e.Message));
                }catch(ArgumentException e){
                    logger.LogWarning(LOG_POST_MATERIAL_RESTRICTION_BAD_REQUEST, productId, materialId, restrictionDTO);
                    return BadRequest(new SimpleJSONMessageService(e.Message));
                }catch(Exception e){
                    logger.LogWarning(e, UNEXPECTED_ERROR);
                    return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
                }
        }
        

        /// <summary>
        /// Updates the properties of a product
        /// </summary>
        /// <param name="updateProductPropertiesModelView">UpdateProductPropertiesModelView with the information about the product properties update</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult updateProductProperties(long id, [FromBody] UpdateProductPropertiesModelView updateProductPropertiesModelView) {
            logger.LogInformation(LOG_PUT_START);

            if(updateProductPropertiesModelView == null){
                return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_DATA));
            }

            updateProductPropertiesModelView.id = id;
            try{
                GetProductModelView updatedProductMV=new core.application.ProductController().updateProductProperties(updateProductPropertiesModelView);
                logger.LogInformation(LOG_PUT_SUCCESS, id, updateProductPropertiesModelView);
                return Ok(updatedProductMV);
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_PUT_NOT_FOUND, id, updateProductPropertiesModelView);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(ArgumentException e){
                logger.LogWarning(e, LOG_PUT_BAD_REQUEST, id, updateProductPropertiesModelView);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="id">Long with the product being disabled ID</param>
        /// <returns>HTTP Response 204;No Content if the product was disabled with success
        ///      <br>HTTP Response 404;Not Found if no Product could be found
        /// </returns>
        /// 
        [HttpDelete("{id}")]
        public ActionResult disableProduct(long id) {
            logger.LogInformation(LOG_DELETE_START);
            DeleteProductModelView deleteProductMV = new DeleteProductModelView();
            deleteProductMV.productId = id;
            try{
                new core.application.ProductController().disableProduct(deleteProductMV);
                logger.LogInformation(LOG_DELETE_SUCCESS, id);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_DELETE_NOT_FOUND, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

    
        [HttpDelete("{productId}/dimensions/{measurementId}")]
        public ActionResult deleteMeasurementFromProduct(long productId, long measurementId){
            logger.LogInformation(LOG_DELETE_MEASUREMENT_START);

            DeleteMeasurementModelView deleteMeasurementFromProductMV = new DeleteMeasurementModelView();
            deleteMeasurementFromProductMV.productId = productId;
            deleteMeasurementFromProductMV.measurementId = measurementId;

            try{
                new core.application.ProductController().deleteMeasurementFromProduct(deleteMeasurementFromProductMV);
                logger.LogInformation(LOG_DELETE_MEASUREMENT_SUCCESS, productId, measurementId);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_DELETE_MEASUREMENT_NOT_FOUND,productId, measurementId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(InvalidOperationException e){
                //*this exception will occur if the last measurement is attempted to be removed*/
                logger.LogWarning(e, LOG_DELETE_MEASUREMENT_BAD_REQUEST, productId, measurementId);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            logger.LogInformation(LOG_DELETE_COMPONENT_START);

            DeleteComponentModelView deleteComponentFromProductMV=new DeleteComponentModelView();
            deleteComponentFromProductMV.fatherProductId=productID;
            deleteComponentFromProductMV.childProductId=componentID;
            try{
                new core.application.ProductController().deleteComponentFromProduct(deleteComponentFromProductMV);
                logger.LogInformation(LOG_DELETE_COMPONENT_SUCCESS, productID, componentID);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogInformation(e, LOG_DELETE_COMPONENT_NOT_FOUND, productID, componentID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            logger.LogInformation(LOG_DELETE_MATERIAL_START);

            DeleteProductMaterialModelView deleteMaterialFromProductMV=new DeleteProductMaterialModelView();
            deleteMaterialFromProductMV.productId=productID;
            deleteMaterialFromProductMV.materialId=materialID;
            try{
                new core.application.ProductController().deleteMaterialFromProduct(deleteMaterialFromProductMV);
                logger.LogInformation(LOG_DELETE_MATERIAL_SUCCESS, productID, materialID);
                return NoContent();
            }catch(InvalidOperationException e){
                //*this exception will occur if the last material is attempted to be removed*/
                logger.LogWarning(e, LOG_DELETE_MATERIAL_BAD_REQUEST, productID, materialID);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_DELETE_MATERIAL_NOT_FOUND, productID, materialID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{productId}/dimensions/{measurementId}/restrictions/{restrictionId}")]
        public ActionResult deleteRestrictionFromProductMeasurement(long productId, long measurementId, long restrictionId){
            logger.LogInformation(LOG_DELETE_MEASUREMENT_RESTRICTION_START);

            DeleteMeasurementRestrictionModelView deleteRestrictionFromProductMeasurementMV = new DeleteMeasurementRestrictionModelView();
            deleteRestrictionFromProductMeasurementMV.productId = productId;
            deleteRestrictionFromProductMeasurementMV.measurementId = measurementId;
            deleteRestrictionFromProductMeasurementMV.restrictionId = restrictionId;
            try{
                new core.application.ProductController().deleteRestrictionFromProductMeasurement(deleteRestrictionFromProductMeasurementMV);
                logger.LogInformation(LOG_DELETE_MEASUREMENT_RESTRICTION_SUCCESS, productId, measurementId, restrictionId);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogWarning(e, LOG_DELETE_MEASUREMENT_RESTRICTION_NOT_FOUND, productId, measurementId, restrictionId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            logger.LogInformation(LOG_DELETE_COMPONENT_RESTRICTION_START);

            DeleteComponentRestrictionModelView deleteRestrictionFromProductComponentMV=new DeleteComponentRestrictionModelView();
            deleteRestrictionFromProductComponentMV.fatherProductId=productID;
            deleteRestrictionFromProductComponentMV.childProductId=componentID;
            deleteRestrictionFromProductComponentMV.restrictionId=restrictionID;
            try{
                new core.application.ProductController().deleteRestrictionFromProductComponent(deleteRestrictionFromProductComponentMV);
                logger.LogInformation(LOG_DELETE_COMPONENT_RESTRICTION_SUCCESS, productID, componentID, restrictionID);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogInformation(e, LOG_DELETE_COMPONENT_RESTRICTION_NOT_FOUND, productID, componentID, restrictionID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{productID}/materials/{materialID}/restrictions/{restrictionId}")]
        public ActionResult deleteRestrictionFromProductMaterial(long productId, long materialId, long restrictionId){
            logger.LogInformation(LOG_DELETE_MATERIAL_RESTRICTION_START);

            DeleteProductMaterialRestrictionModelView deleteRestrictionFromProductMaterialMV = new DeleteProductMaterialRestrictionModelView();
            deleteRestrictionFromProductMaterialMV.productId = productId;
            deleteRestrictionFromProductMaterialMV.materialId = materialId;
            deleteRestrictionFromProductMaterialMV.restrictionId = restrictionId;
            try{
                new core.application.ProductController().deleteRestrictionFromProductMaterial(deleteRestrictionFromProductMaterialMV);
                logger.LogInformation(LOG_DELETE_MATERIAL_RESTRICTION_SUCCESS, productId, materialId, restrictionId);
                return NoContent();
            }catch(ResourceNotFoundException e){
                logger.LogInformation(e, LOG_DELETE_MATERIAL_RESTRICTION_NOT_FOUND, productId, materialId, restrictionId);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }catch(Exception e){
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}