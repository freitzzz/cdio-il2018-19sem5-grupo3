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

namespace backend.Controllers {

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

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository) {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// Finds all Products in the repository or, if a reference is specified, retrieves a Product with a matching reference.
        /// </summary>
        /// <param name="reference">The product's reference.</param>
        /// <param name="unit">The product's dimensions unit.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult find([FromQuery]string reference, [FromQuery]string unit) {
            if (reference == null) {
                return findAll();
            }

            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.reference = reference;
            fetchProductDTO.productDTOOptions.requiredUnit = unit;

            return findByReference(fetchProductDTO);
        }

        /// <summary>
        /// Finds all products in the repository.
        /// </summary>
        /// <returns>HTTP Response 404 Not Found if no products are found;
        /// HTTP Response 200 Ok with the info of all products in JSON format </returns>
        private ActionResult findAll() {
            try {
                GetAllProductsModelView allProductsModelView = new core.application.ProductController().findAllProducts();
                return Ok(allProductsModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Finds a Product with a matching reference.
        /// </summary>
        /// <param name="fetchProductDTO"></param>
        /// <returns></returns>
        private ActionResult findByReference(FetchProductDTO fetchProductDTO) {
            try {
                GetProductModelView getProductModelView = new core.application.ProductController().findProduct(fetchProductDTO);
                return Ok(getProductModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                //this exception may occur if the specified unit does not exist
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Finds all the base Products in the repository.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP Code or the 404 HTTP Code if no Product was found.</returns>
        [HttpGet("base")]
        public ActionResult findBaseProducts() {
            try {
                GetAllProductsModelView allBaseProductsModelView = new core.application.ProductController().findBaseProducts();
                return Ok(allBaseProductsModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = id;
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                GetProductModelView getProductModelView = new core.application.ProductController().findProduct(fetchProductDTO);
                return Ok(getProductModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                //this exception may occur if the specified unit does not exist
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/dimensions")]
        public ActionResult findProductMeasurements(long productId, [FromQuery] string unit) {
            FetchProductDTO fetchProductDTO = new FetchProductDTO() { id = productId };
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                GetAllMeasurementsModelView allMeasurementsModelView =
                    new core.application.ProductController().findProductMeasurements(fetchProductDTO);
                return Ok(allMeasurementsModelView);
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/components")]
        public ActionResult findProductComponents(long productId, [FromQuery]FindComponentsOptions groupBy) {
            try {
                FindComponentsModelView findComponentsModel = new FindComponentsModelView();
                findComponentsModel.fatherProductId = productId;
                findComponentsModel.option = groupBy;
                GetAllComponentsModelView allComponentsByCategory = new core.application.ProductController().findProductComponents(findComponentsModel);
                return Ok(allComponentsByCategory);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/materials")]
        public ActionResult findProductMaterials(long productId, [FromQuery] bool pricedMaterialsOnly) {
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = productId;
            fetchProductDTO.pricedMaterialsOnly = pricedMaterialsOnly;
            try {
                GetAllMaterialsModelView allMaterialsModelView = new core.application.ProductController().findProductMaterials(fetchProductDTO);
                return Ok(allMaterialsModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{productId}/components/{componentId}")]
        public ActionResult findProductComponent(long productId, long componentId, [FromQuery]string unit) {
            try {
                FindComponentModelView findComponentModelView = new FindComponentModelView();
                findComponentModelView.fatherProductId = productId;
                findComponentModelView.childProductId = componentId;
                findComponentModelView.unit = unit;
                GetComponentModelView componentModelView = new core.application.ProductController().findProductComponent(findComponentModelView);
                return Ok(componentModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult findProductSlotWidths(long productId, [FromQuery] string unit) {
            FetchProductDTO fetchProductDTO = new FetchProductDTO();
            fetchProductDTO.id = productId;
            fetchProductDTO.productDTOOptions.requiredUnit = unit;
            try {
                GetProductSlotWidthsModelView productSlotWidthsMV = new core.application.ProductController().findProductSlotWidths(fetchProductDTO);
                return Ok(productSlotWidthsMV);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (InvalidOperationException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult findMeasurementRestrictions(long productId, long measurementId) {
            FindMeasurementModelView productMeasurementMV = new FindMeasurementModelView();
            productMeasurementMV.productId = productId;
            productMeasurementMV.measurementId = measurementId;
            try {
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findMeasurementRestrictions(productMeasurementMV);
                return Ok(restrictionModelViews);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult findComponentRestrictions(long parentProductId, long complementaryProductId) {
            FindComponentModelView componentModelView = new FindComponentModelView();
            componentModelView.fatherProductId = parentProductId;
            componentModelView.childProductId = complementaryProductId;

            try {
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findComponentRestrictions(componentModelView);
                return Ok(restrictionModelViews);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult findMaterialRestrictions(long productId, long materialId) {
            FindProductMaterialModelView productMaterialModelView = new FindProductMaterialModelView();
            productMaterialModelView.productId = productId;
            productMaterialModelView.materialId = materialId;

            try {
                GetAllRestrictionsModelView restrictionModelViews = new core.application.ProductController().findMaterialRestrictions(productMaterialModelView);
                return Ok(restrictionModelViews);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
            if (addProductMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_DATA));
            }

            try {
                GetProductModelView createdProductMV = new core.application.ProductController().addProduct(addProductMV);
                return CreatedAtRoute("GetProduct", new { id = createdProductMV.productId }, createdProductMV);
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult addMeasurementToProduct(long productId, [FromBody] AddMeasurementModelView measurementModelView) {
            if (measurementModelView == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_MEASUREMENT_DATA));
            }

            measurementModelView.productId = productId;

            try {
                GetProductModelView productModelView = new core.application.ProductController().addMeasurementToProduct(measurementModelView);
                return Created(Request.Path, productModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult addComponentToProduct(long id, [FromBody]AddComponentModelView addComponentToProductMV) {

            if (addComponentToProductMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_COMPONENT_DATA));
            }

            addComponentToProductMV.fatherProductId = id;
            try {
                GetProductModelView productModelView = new core.application.ProductController().addComponentToProduct(addComponentToProductMV);
                return Created(Request.Path, productModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult addMaterialToProduct(long id, [FromBody]AddProductMaterialModelView addMaterialToProductMV) {

            if (addMaterialToProductMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_DATA));
            }

            addMaterialToProductMV.productId = id;
            try {
                GetProductModelView productModelView = new core.application.ProductController().addMaterialToProduct(addMaterialToProductMV);
                return Created(Request.Path, productModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpPost("{productId}/dimensions/{measurementId}/restrictions")]
        public ActionResult addRestrictionToProductMeasurement(long productId, long measurementId, [FromBody]AddRestrictionModelView restrictionMV) {

            if (restrictionMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
            }

            AddMeasurementRestrictionModelView addRestrictionToProductMeasurementMV = new AddMeasurementRestrictionModelView();
            addRestrictionToProductMeasurementMV.productId = productId;
            addRestrictionToProductMeasurementMV.measurementId = measurementId;
            addRestrictionToProductMeasurementMV.restriction = restrictionMV;

            try {
                GetProductModelView productModelView = new core.application.ProductController().addRestrictionToProductMeasurement(addRestrictionToProductMeasurementMV);
                return Created(Request.Path, productModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult addRestrictionToProductComponent(long productID, long componentID, [FromBody]AddRestrictionModelView restrictionMV) {

            if (restrictionMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
            }

            AddComponentRestrictionModelView addRestrictionToProductComponentDTO = new AddComponentRestrictionModelView();
            addRestrictionToProductComponentDTO.fatherProductId = productID;
            addRestrictionToProductComponentDTO.childProductId = componentID;
            addRestrictionToProductComponentDTO.restriction = restrictionMV;
            try {
                GetProductModelView appliedRestrictionModelView = new core.application.ProductController().addRestrictionToProductComponent(addRestrictionToProductComponentDTO);
                return Created(Request.Path, appliedRestrictionModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult addRestrictionToProductMaterial(long productId, long materialId, [FromBody] AddRestrictionModelView restrictionMV) {

            if (restrictionMV == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_RESTRICTION_DATA));
            }

            AddProductMaterialRestrictionModelView addRestrictionToProductMaterialMV = new AddProductMaterialRestrictionModelView();
            addRestrictionToProductMaterialMV.productId = productId;
            addRestrictionToProductMaterialMV.materialId = materialId;
            addRestrictionToProductMaterialMV.restriction = restrictionMV;

            try {
                GetProductModelView productModelView = new core.application.ProductController().addRestrictionToProductMaterial(addRestrictionToProductMaterialMV);
                return Created(Request.Path, productModelView);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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

            if (updateProductPropertiesModelView == null) {
                return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_DATA));
            }

            updateProductPropertiesModelView.id = id;
            try {
                GetProductModelView updatedProductMV = new core.application.ProductController().updateProductProperties(updateProductPropertiesModelView);
                return Ok(updatedProductMV);
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (ArgumentException e) {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
            DeleteProductModelView deleteProductMV = new DeleteProductModelView();
            deleteProductMV.productId = id;
            try {
                new core.application.ProductController().disableProduct(deleteProductMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        [HttpDelete("{productId}/dimensions/{measurementId}")]
        public ActionResult deleteMeasurementFromProduct(long productId, long measurementId) {

            DeleteMeasurementModelView deleteMeasurementFromProductMV = new DeleteMeasurementModelView();
            deleteMeasurementFromProductMV.productId = productId;
            deleteMeasurementFromProductMV.measurementId = measurementId;

            try {
                new core.application.ProductController().deleteMeasurementFromProduct(deleteMeasurementFromProductMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (InvalidOperationException e) {
                //*this exception will occur if the last measurement is attempted to be removed*/
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult deleteComponentFromProduct(long productID, long componentID) {

            DeleteComponentModelView deleteComponentFromProductMV = new DeleteComponentModelView();
            deleteComponentFromProductMV.fatherProductId = productID;
            deleteComponentFromProductMV.childProductId = componentID;
            try {
                new core.application.ProductController().deleteComponentFromProduct(deleteComponentFromProductMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult deleteMaterialFromProduct(long productID, long materialID) {

            DeleteProductMaterialModelView deleteMaterialFromProductMV = new DeleteProductMaterialModelView();
            deleteMaterialFromProductMV.productId = productID;
            deleteMaterialFromProductMV.materialId = materialID;
            try {
                new core.application.ProductController().deleteMaterialFromProduct(deleteMaterialFromProductMV);
                return NoContent();
            } catch (InvalidOperationException e) {
                //*this exception will occur if the last material is attempted to be removed*/
                return BadRequest(new SimpleJSONMessageService(e.Message));
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{productId}/dimensions/{measurementId}/restrictions/{restrictionId}")]
        public ActionResult deleteRestrictionFromProductMeasurement(long productId, long measurementId, long restrictionId) {

            DeleteMeasurementRestrictionModelView deleteRestrictionFromProductMeasurementMV = new DeleteMeasurementRestrictionModelView();
            deleteRestrictionFromProductMeasurementMV.productId = productId;
            deleteRestrictionFromProductMeasurementMV.measurementId = measurementId;
            deleteRestrictionFromProductMeasurementMV.restrictionId = restrictionId;
            try {
                new core.application.ProductController().deleteRestrictionFromProductMeasurement(deleteRestrictionFromProductMeasurementMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
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
        public ActionResult deleteRestrictionFromProductComponent(long productID, long componentID, long restrictionID) {

            DeleteComponentRestrictionModelView deleteRestrictionFromProductComponentMV = new DeleteComponentRestrictionModelView();
            deleteRestrictionFromProductComponentMV.fatherProductId = productID;
            deleteRestrictionFromProductComponentMV.childProductId = componentID;
            deleteRestrictionFromProductComponentMV.restrictionId = restrictionID;
            try {
                new core.application.ProductController().deleteRestrictionFromProductComponent(deleteRestrictionFromProductComponentMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{productID}/materials/{materialID}/restrictions/{restrictionId}")]
        public ActionResult deleteRestrictionFromProductMaterial(long productId, long materialId, long restrictionId) {

            DeleteProductMaterialRestrictionModelView deleteRestrictionFromProductMaterialMV = new DeleteProductMaterialRestrictionModelView();
            deleteRestrictionFromProductMaterialMV.productId = productId;
            deleteRestrictionFromProductMaterialMV.materialId = materialId;
            deleteRestrictionFromProductMaterialMV.restrictionId = restrictionId;
            try {
                new core.application.ProductController().deleteRestrictionFromProductMaterial(deleteRestrictionFromProductMaterialMV);
                return NoContent();
            } catch (ResourceNotFoundException e) {
                return NotFound(new SimpleJSONMessageService(e.Message));
            } catch (Exception) {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}