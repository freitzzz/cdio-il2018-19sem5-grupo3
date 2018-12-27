using System;
using backend.utils;
using core.application;
using core.dto;
using core.persistence;
using core.modelview.pricetable;
using core.modelview.pricetableentries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime.Text;
using System.Threading.Tasks;
using System.Net.Http;
using core.exceptions;
using core.modelview.material;
using core.modelview.price;

namespace backend.Controllers
{
    /// <summary>
    /// Controller class to handle requests related to price tables
    /// </summary>
    [Route("mycm/api/prices")]
    public class PriceTablesController : Controller
    {
        /// <summary>
        /// Constant that represents the message that is presented/logged if an unexpected error happens
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occured, please try again later";

        /// <summary>
        /// Constant that represents the message that is logged if a price entry for a material isn't created
        /// </summary>
        private const string PRICE_ENTRY_NOT_CREATED = "The price entry wasn't created";

        /// <summary>
        /// Constant that represents the message that is logged if a price entry isn't updated
        /// </summary>
        private const string ENTRY_NOT_UPDATED = "The price entry wasn't updated";

        /// <summary>
        /// Material Price Table Repository
        /// </summary>
        private readonly MaterialPriceTableRepository materialPriceTableRepository;

        /// <summary>
        /// Finish Price Table Repository
        /// </summary>
        private readonly FinishPriceTableRepository finishPriceTableRepository;

        /// <summary>
        /// Injected client factory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Constructor with injected type of repositories
        /// </summary>
        /// <param name="materialPriceTableRepository">material price table repository</param>
        /// <param name="finishPriceTableRepository">finish price table repository</param>
        public PriceTablesController(MaterialPriceTableRepository materialPriceTableRepository, FinishPriceTableRepository finishPriceTableRepository, IHttpClientFactory clientFactory)
        {
            this.materialPriceTableRepository = materialPriceTableRepository;
            this.finishPriceTableRepository = finishPriceTableRepository;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Fetches price history of all materials
        /// </summary>
        /// <param name="currency">Query parameter to know the currency in which the price is presented</param>
        /// <param name="area">Query parameter to know the area in which the price is presented</param>
        /// <returns>HTTP Response 200; OK with the price history of all materials
        ///          HTTP Response 404; Not Found if no materials have price histories
        /// </returns>
        [HttpGet("materials")]
        public async Task<ActionResult> fetchAllMaterialsPriceHistory([FromQuery] string currency, [FromQuery] string area)
        {
            try
            {
                GetAllMaterialPriceHistoryModelView modelView = await new core.application.PriceTablesController().fetchPriceHistoryOfAllMaterials(currency, area, clientFactory);
                return Ok(modelView);
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
        /// Fetches price history of all finishes of a material
        /// </summary>
        /// <param name="materialID">Material's PID</param>
        /// <param name="currency">Query parameter to know the currency in which the price is presented</param>
        /// <param name="area">Query parameter to know the area in which the price is presented</param>
        /// <returns>Action Result with HTTP Code 200 and the price history of all material finishes
        ///         Or Action Result with HTTP Code 404 if no material finishes have a price history</returns>
        [HttpGet("materials/{materialID}/finishes")]
        public async Task<ActionResult> fetchAllMaterialFinishesPriceHistory(long materialID, [FromQuery] string currency, [FromQuery] string area)
        {
            try
            {
                FetchMaterialFinishPriceHistoryDTO priceHistoryDTO = new FetchMaterialFinishPriceHistoryDTO();
                priceHistoryDTO.materialID = materialID;
                priceHistoryDTO.currency = currency;
                priceHistoryDTO.area = area;
                GetAllMaterialFinishPriceHistoryModelView allMaterialFinishesPriceHistoryModelView = await new core.application.PriceTablesController().fetchPriceHistoryOfAllMaterialFinishes(priceHistoryDTO, clientFactory);
                return Ok(allMaterialFinishesPriceHistoryModelView);
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
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <param name="currency">Query parameter to know the currency in which the price is presented</param>
        /// <param name="area">Query parameter to know the area in which the price is presented</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 404; Not Found if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}")]
        public async Task<ActionResult> fetchMaterialPriceHistory(long materialID, [FromQuery] string currency, [FromQuery] string area)
        {
            FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO = new FetchMaterialPriceHistoryDTO();
            fetchMaterialPriceHistoryDTO.materialID = materialID;
            fetchMaterialPriceHistoryDTO.currency = currency;
            fetchMaterialPriceHistoryDTO.area = area;
            try
            {
                GetAllMaterialPriceHistoryModelView materialPriceHistoryModelView = await new core.application.PriceTablesController().fetchMaterialPriceHistory(fetchMaterialPriceHistoryDTO, clientFactory);
                return Ok(materialPriceHistoryModelView);
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
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <param name="finishID">Long with the resource ID of the material finish being fetched the price history</param>
        /// <param name="currency">Query parameter to know the currency in which the price is presented</param>
        /// <param name="area">Query parameter to know the area in which the price is presented</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 404; Not Found if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}/finishes/{finishID}")]
        public async Task<ActionResult> fetchMaterialFinishPriceHistory(long materialID, long finishID, [FromQuery] string currency, [FromQuery] string area)
        {
            FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO = new FetchMaterialFinishPriceHistoryDTO();
            fetchMaterialFinishPriceHistoryDTO.materialID = materialID;
            fetchMaterialFinishPriceHistoryDTO.finishID = finishID;
            fetchMaterialFinishPriceHistoryDTO.currency = currency;
            fetchMaterialFinishPriceHistoryDTO.area = area;
            try
            {
                GetAllMaterialFinishPriceHistoryModelView materialFinishPriceHistoryModelView = await new core.application.PriceTablesController().fetchMaterialFinishPriceHistory(fetchMaterialFinishPriceHistoryDTO, clientFactory);
                return Ok(materialFinishPriceHistoryModelView);
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
        /// Fetches the current price of a material
        /// </summary>
        /// <param name="materialId">Material's PID</param>
        /// <param name="currency">Query parameter to know to which currency to convert the price to</param>
        /// <param name="area">Query parameter to know to which area to convert the price to</param>
        /// <returns>Action Result with HTTP Code 200 with the material's current price
        ///         Or Action Result with HTTP Code 404 if the requested material isn't found
        ///         Or Action Result with HTTP Code 500 if an unexpected error happens </returns>
        [HttpGet("materials/{materialId}/currentprice")]
        public async Task<ActionResult> fetchCurrentMaterialPrice(long materialId, [FromQuery] string currency, [FromQuery] string area)
        {
            GetCurrentMaterialPriceModelView fetchCurrentMaterialPrice = new GetCurrentMaterialPriceModelView();
            fetchCurrentMaterialPrice.material = new GetBasicMaterialModelView();
            fetchCurrentMaterialPrice.currentPrice = new PriceModelView();
            fetchCurrentMaterialPrice.material.id = materialId;
            fetchCurrentMaterialPrice.currentPrice.currency = currency;
            fetchCurrentMaterialPrice.currentPrice.area = area;
            try
            {
                GetCurrentMaterialPriceModelView currentMaterialPrice = await new core.application.PriceTablesController().fetchCurrentMaterialPrice(fetchCurrentMaterialPrice, clientFactory);
                return Ok(currentMaterialPrice);
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
        /// Fetches the current price of a material finish
        /// </summary>
        /// <param name="materialId">Material's PID</param>
        /// <param name="finishId">Finish's PID</param>
        /// <param name="currency">Query parameter to know in which currency to present the price in</param>
        /// <param name="area">Query parameter to know in which area to present the price in</param>
        /// <returns>Action Result with HTTP Code 200 and the material's finish current price
        ///         Or Action Result with HTTP Code 404 if one of the requested resources isn't found
        ///         Or Action Result with HTTP Code 500 if an unexpected error happens</returns>
        [HttpGet("materials/{materialId}/finishes/{finishId}/currentprice")]
        public async Task<ActionResult> fetchCurrentMaterialFinishPrice(long materialId, long finishId, [FromQuery] string currency, [FromQuery] string area)
        {
            GetCurrentMaterialFinishPriceModelView fetchCurrentMaterialFinishPrice = new GetCurrentMaterialFinishPriceModelView();
            fetchCurrentMaterialFinishPrice.finish = new GetMaterialFinishModelView();
            fetchCurrentMaterialFinishPrice.currentPrice = new PriceModelView();
            fetchCurrentMaterialFinishPrice.finish.materialId = materialId;
            fetchCurrentMaterialFinishPrice.finish.id = finishId;
            fetchCurrentMaterialFinishPrice.currentPrice.currency = currency;
            fetchCurrentMaterialFinishPrice.currentPrice.area = area;
            try
            {
                GetCurrentMaterialFinishPriceModelView currentMaterialFinishPrice = await new core.application.PriceTablesController().fetchCurrentMaterialFinishPrice(fetchCurrentMaterialFinishPrice, clientFactory);
                return Ok(currentMaterialFinishPrice);
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
        /// Adds a new price table entry for a given material
        /// </summary>
        /// <param name="id">PID of the material</param>
        /// <param name="modelView">AddMaterialPriceTableEntryModelView with the price's information</param>
        /// <returns>ActionResult with HTTP Code 201 if the creation is successful
        ///         Or ActionResult with HTTP Code 400 if an error happens</returns>
        [HttpPost("materials/{id}")]
        public async Task<ActionResult> addMaterialPriceTableEntry(long id, [FromBody] AddPriceTableEntryModelView modelView)
        {
            try
            {
                modelView.entityId = id;
                GetMaterialPriceModelView createdPrice = await new core.application.PriceTablesController().addMaterialPriceTableEntry(modelView, clientFactory);
                if (createdPrice == null)
                {
                    return BadRequest(new SimpleJSONMessageService(PRICE_ENTRY_NOT_CREATED));
                }
                return Created(Request.Path, createdPrice);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
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
        /// Adds a new price table entry for a given material
        /// </summary>
        /// <param name="id">PID of the material</param>
        /// <param name="modelView">AddMaterialPriceTableEntryModelView with the price's information</param>
        /// <returns>ActionResult with HTTP Code 201 if the creation is successful
        ///         Or ActionResult with HTTP Code 400 if an error happens</returns>
        [HttpPost("materials/{materialid}/finishes/{finishid}")]
        public async Task<ActionResult> addFinishPriceTableEntry(long materialid, long finishid, [FromBody] AddFinishPriceTableEntryModelView modelView)
        {
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                GetMaterialFinishPriceModelView createdPrice = await new core.application.PriceTablesController().addFinishPriceTableEntry(modelView, clientFactory);
                if (createdPrice == null)
                {
                    return BadRequest(new SimpleJSONMessageService(PRICE_ENTRY_NOT_CREATED));
                }
                return Created(Request.Path, createdPrice);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
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
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="materialid">material's PID</param>
        /// <param name="entryid">price table entry's PID</param>
        /// <param name="modelView">model view with the update information</param>
        /// <returns>ActionResult with HTTP Code 200 if the update is successful
        ///         Or ActionResult with HTTP Code 400 if the update isn't successful</returns>
        [HttpPut("materials/{materialid}/entries/{entryid}")]
        public async Task<ActionResult> updateMaterialPriceTableEntry(long materialid, long entryid, [FromBody] UpdatePriceTableEntryModelView modelView)
        {
            try
            {
                modelView.entityId = materialid;
                modelView.tableEntryId = entryid;
                GetMaterialPriceModelView updatedEntry = await new core.application.PriceTablesController().updateMaterialPriceTableEntry(modelView, clientFactory);
                if (updatedEntry == null)
                {
                    return BadRequest(new SimpleJSONMessageService(ENTRY_NOT_UPDATED));
                }
                return Ok(updatedEntry);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
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
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="materialid">PID of the material that the finish belongs to</param>
        /// <param name="finishid">finish's PID</param>
        /// <param name="entryid">finish's price table entry's PID</param>
        /// <param name="modelView">model view with the update information</param>
        /// <returns>ActionResult with HTTP Code 200 if the update is successful
        ///         Or ActionResult with HTTP Code 400 if the update isn't successful</returns>
        [HttpPut("materials/{materialid}/finishes/{finishid}/entries/{entryid}")]
        public async Task<ActionResult> updateFinishPriceTableEntry(long materialid, long finishid, long entryid, [FromBody] UpdateFinishPriceTableEntryModelView modelView)
        {
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                modelView.tableEntryId = entryid;
                GetMaterialFinishPriceModelView updatedEntry = await new core.application.PriceTablesController().updateFinishPriceTableEntry(modelView, clientFactory);
                if (updatedEntry == null)
                {
                    return BadRequest(new { error = ENTRY_NOT_UPDATED });
                }
                return Ok(updatedEntry);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
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
    }
}