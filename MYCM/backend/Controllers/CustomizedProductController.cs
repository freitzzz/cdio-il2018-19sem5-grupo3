using core.dto;
using core.persistence;
using core.modelview.customizedproduct;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using core.exceptions;
using backend.utils;
using core.modelview.slot;
using core.modelview.customizeddimensions;
using System.Threading.Tasks;
using core.modelview.customizedproduct.customizedproductprice;
using System.Net.Http;

namespace backend.Controllers
{
    /// <summary>
    /// MVC Controller for CustomizedProduct operations
    /// </summary>
    [Route("/mycm/api/customizedproducts")]
    public class CustomizedProductController : Controller
    {
        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later.";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid! Check documentation for more information";


        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// Injected instance of CustomizedProductSerialNumberRepository.
        /// </summary>
        private readonly CustomizedProductSerialNumberRepository customizedProductSerialNumberRepository;

        /// <summary>
        /// Injected client factory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        /// <param name="customizedProductSerialNumberRepository">Injected instance of CustomizedProductSerialNumberRepository.</param>
        /// <param name="clientFactory">Injected http client factory</param>
        public CustomizedProductController(CustomizedProductRepository customizedProductRepository, CustomizedProductSerialNumberRepository customizedProductSerialNumberRepository, IHttpClientFactory clientFactory)
        {
            this.customizedProductRepository = customizedProductRepository;
            this.customizedProductSerialNumberRepository = customizedProductSerialNumberRepository;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult findAll()
        {

            try
            {
                GetAllCustomizedProductsModelView getAllModelView = new core.application.CustomizedProductController().findAllCustomizedProducts();
                return Ok(getAllModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("base")]
        public ActionResult findBaseCustomizedProducts()
        {

            try
            {
                GetAllCustomizedProductsModelView getAllModelView = new core.application.CustomizedProductController().findAllBaseCustomizedProducts();
                return Ok(getAllModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches the information of a customized product by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products resource id</param>
        /// <param name="unit">String representing the unit in which the CustomizedProduct's dimensions will be retrieved in.</param>
        /// <returns>ActionResult with the customized product information</returns>
        [HttpGet("{id}", Name = "GetCustomizedProduct")]
        public ActionResult findByID(long id, [FromQuery]string unit)
        {
            try
            {
                FindCustomizedProductModelView findCustomizedProductModelView = new FindCustomizedProductModelView();
                findCustomizedProductModelView.customizedProductId = id;
                findCustomizedProductModelView.options.unit = unit;

                GetCustomizedProductModelView fetchedCustomizedProduct = new core.application.CustomizedProductController().findCustomizedProduct(findCustomizedProductModelView);
                return Ok(fetchedCustomizedProduct);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return NotFound(new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("{customizedProductId}/slots/{slotId}", Name = "GetSlot")]
        public ActionResult findSlotById(long customizedProductId, long slotId)
        {
            try
            {
                FindSlotModelView findSlotModelView = new FindSlotModelView();
                findSlotModelView.customizedProductId = customizedProductId;
                findSlotModelView.slotId = slotId;

                GetSlotModelView slotModelView = new core.application.CustomizedProductController().findSlot(findSlotModelView);
                return Ok(slotModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Calculates and fetches the price of a requested customized product
        /// </summary>
        /// <param name="id">Customized Product's PID</param>
        /// <param name="currency">Requested Currency to present the price in</param>
        /// <param name="area">Requested Area to present the price in</param>
        /// <returns>Action Result with HTTP Code 200 and detailed information about the customized product's price
        ///         Or Action Result with HTTP Code 404 if the requested customized product isn't found
        ///         Or Action Result with HTTP Code 400 if an error happens
        ///         Or Action Result with HTTP Code 500 if an unexpected error happens</returns>
        [HttpGet("{id}/price")]
        public async Task<ActionResult> fetchPrice(long id, [FromQuery] string currency, [FromQuery] string area)
        {
            try
            {
                FetchCustomizedProductPriceModelView fetchPrice = new FetchCustomizedProductPriceModelView();
                fetchPrice.id = id;
                fetchPrice.currency = currency;
                fetchPrice.area = area;

                CustomizedProductFinalPriceModelView customizedProductPrice = await new core.application.CustomizedProductController().calculateCustomizedProductPrice(fetchPrice, clientFactory);
                return Ok(customizedProductPrice);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Creates a new customized product
        /// </summary>
        /// <param name="customizedProductModelView">CustomizedProductDTO with the customized product being added</param>
        /// <returns>ActionResult with the created customized product</returns>
        [HttpPost]
        [HttpPost("{customizedProductId}/slots/{slotId}/customizedproducts")]
        public ActionResult addCustomizedProduct(long? customizedProductId, long? slotId, [FromHeader]string userAuthToken,
            [FromBody]AddCustomizedProductModelView customizedProductModelView)
        {

            if (customizedProductModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                customizedProductModelView.parentCustomizedProductId = customizedProductId;
                customizedProductModelView.insertedInSlotId = slotId;
                customizedProductModelView.userAuthToken = userAuthToken;

                GetCustomizedProductModelView createdCustomizedProductModelView = new core.application
                    .CustomizedProductController().addCustomizedProduct(customizedProductModelView);

                return CreatedAtRoute("GetCustomizedProduct", new { id = createdCustomizedProductModelView.customizedProductId }, createdCustomizedProductModelView);
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

        [HttpPost("{id}/slots")]
        public ActionResult addSlotToCustomizedProduct(long id, [FromBody] AddCustomizedDimensionsModelView slotDimensions)
        {

            if (slotDimensions == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                AddSlotModelView addSlotModelView = new AddSlotModelView();
                addSlotModelView.customizedProductId = id;
                addSlotModelView.slotDimensions = slotDimensions;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().addSlotToCustomizedProduct(addSlotModelView);

                return Created(Request.Path, customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        [HttpPut("{id}")]
        public ActionResult updateCustomizedProduct(long id, [FromBody] UpdateCustomizedProductModelView updateCustomizedProductModelView)
        {
            if (updateCustomizedProductModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateCustomizedProductModelView.customizedProductId = id;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().updateCustomizedProduct(updateCustomizedProductModelView);

                return Ok(customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpPut("{customizedProductId}/slots/{slotId}")]
        public ActionResult updateSlot(long customizedProductId, long slotId, [FromBody] UpdateSlotModelView updateSlotModelView)
        {

            if (updateSlotModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateSlotModelView.customizedProductId = customizedProductId;
                updateSlotModelView.slotId = slotId;

                GetCustomizedProductModelView customizedProductModelView = new core.application.CustomizedProductController().updateSlot(updateSlotModelView);

                return Ok(customizedProductModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{customizedProductId}")]
        public ActionResult deleteCustomizedProduct(long customizedProductId)
        {
            try
            {
                DeleteCustomizedProductModelView deleteCustomizedProductModelView = new DeleteCustomizedProductModelView();
                deleteCustomizedProductModelView.customizedProductId = customizedProductId;

                new core.application.CustomizedProductController().deleteCustomizedProduct(deleteCustomizedProductModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpDelete("{customizedProductId}/slots/{slotId}")]
        public ActionResult deleteSlot(long customizedProductId, long slotId)
        {
            try
            {
                DeleteSlotModelView deleteSlotModelView = new DeleteSlotModelView();
                deleteSlotModelView.customizedProductId = customizedProductId;
                deleteSlotModelView.slotId = slotId;

                new core.application.CustomizedProductController().deleteSlot(deleteSlotModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
        [HttpGet("{customizedProductId}/recommendedslots")]
        public ActionResult getRecommendedSlots(long customizedProductId)
        {
            try
            {
                GetAllCustomizedDimensionsModelView allCustomDimensionsMV = new core.application.CustomizedProductController().getRecommendedSlots(customizedProductId);
                return Ok(allCustomDimensionsMV);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
        [HttpGet("{customizedProductId}/minslots")]
        public ActionResult getMinSlots(long customizedProductId)
        {
            try
            {
                GetAllCustomizedDimensionsModelView allCustomDimensionsMV = new core.application.CustomizedProductController().getMinSlots(customizedProductId);
                return Ok(allCustomDimensionsMV);
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}