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
        /// Constant that represents the message that is logged if a price entry is updated successfully
        /// </summary>
        private const string ENTRY_UPDATE_SUCCESSFUL = "The price entry was updated successfully";

        /// <summary>
        /// Constant that represents the message that is logged if a get all materials price history request starts
        /// </summary>
        private const string LOG_GET_ALL_MATERIALS_PRICE_HISTORY_REQUEST_START = "GET ALL Materials Price History Request started";

        /// <summary>
        /// Constant that represents the message that is logged if a get material price history request starts
        /// </summary>
        private const string LOG_GET_MATERIAL_PRICE_HISTORY_REQUEST_START = "GET{id} Material Price History Request started";

        /// <summary>
        /// Constant that represents the message that is logged if a get finish price history request starts
        /// </summary>
        private const string LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_REQUEST_START = "GET Price History of Finish {finishId} of Material{materialId} Request started";

        /// <summary>
        /// Constant that represents the message that is logged if a get all material finishes price history request starts
        /// </summary>
        private const string LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_REQUEST_START = "GET ALL Finishes Price History of Material {id} Request started";

        /// <summary>
        /// Constant that represents the message that is logged everytime this controller receives a POST Request
        /// </summary>
        private const string LOG_POST_START = "POST Request started";

        /// <summary>
        /// Constant that represents the message that is logged everytime this controller receives a PUT Request
        /// </summary>
        private const string LOG_PUT_START = "PUT Request started";

        /// <summary>
        /// Constant that represents the message that is logged everytime a material price entry is created successfully
        /// </summary>
        private const string LOG_POST_MATERIAL_ENTRY_SUCCESS = "Price Entry {@entry} for Material {id} created successfully";

        /// <summary>
        /// Constant that represents the message that is logged everytime a material's finish price entry is created successfully
        /// </summary>
        private const string LOG_POST_FINISH_ENTRY_SUCCESS = "Price Entry {@entry} for Finish {id} of Material {materialId} created successfully";

        /// <summary>
        /// Constant that represents the message that is logged everytime an update for a material's price table entry returns a BadRequest
        /// </summary>
        private const string LOG_PUT_MATERIAL_ENTRY_SUCCESS = "Price Table Entry {entryid} of Material {materialId} updated succesfully with {@update}";

        /// <summary>
        /// Constant that represents the message that is logged everytime an update for a finish's price table entry returns a BadRequest
        /// </summary>
        private const string LOG_PUT_FINISH_ENTRY_SUCCESS = "Price Table Entry {entryid} of Finish {finishId} of Material {materialId} updated succesfully with {@update}";

        /// <summary>
        /// Constant that represents the message that is logged if a get all materials price history request succeeds
        /// </summary>
        private const string LOG_GET_ALL_MATERIALS_PRICE_HISTORY_SUCCESS = "GET ALL Materials Price History retrieved {@modelView}";

        /// <summary>
        /// Constant that represents the message that is logged if a get materials price history request is successful
        /// </summary>
        private const string LOG_GET_MATERIAL_PRICE_HISTORY_SUCCESS = "GET{id} Materials Price History retrieved {@modelView}";

        /// <summary>
        /// Constant that represents the message that is logged if a get material finish price history request is successful
        /// </summary>
        private const string LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_SUCCESS = "GET{finishId} Finish Price History of Material {materialId} retrieved {@modelView}";

        /// <summary>
        /// Constant that represents the message that is logged if a get all material finishes price history request is successful
        /// </summary>
        private const string LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_SUCCESS = "GET ALL Finishes Price History of Material{id} retrieved {@modelView}";

        /// <summary>
        /// Constant that represents the message that is logged everytime a material price entry POST returns a BadRequest
        /// </summary>
        private const string LOG_POST_MATERIAL_ENTRY_BAD_REQUEST = "POST Price Entry {@entry} for Material {id} BadRequest";

        /// <summary>
        /// Constant that represents the message that is logged everytime a material price entry POST returns a BadRequest
        /// </summary>
        private const string LOG_POST_FINISH_ENTRY_BAD_REQUEST = "POST Price Entry {@entry} for Finish {id} of Material {materialId} BadRequest";

        /// <summary>
        /// Constant that represents the message that is logged everytime an update for a material's price table entry returns a BadRequest
        /// </summary>
        private const string LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST = "PUT {@update} for Price Table Entry {entryid} of Material {materialId} BadRequest";

        /// <summary>
        /// Constant that represents the message that is logged everytime an update for a finish's price table entry returns a BadRequest
        /// </summary>
        private const string LOG_PUT_FINISH_ENTRY_BAD_REQUEST = "PUT {@update} for Price Table Entry {entryid} of Finish {finishId} of Material {materialId} BadRequest";

        /// <summary>
        /// Constant that represents the message that is logged if a get all materials price history request fails
        /// </summary>
        private const string LOG_GET_ALL_MATERIALS_PRICE_HISTORY_NOT_FOUND = "GET ALL Materials Price History NotFound";

        /// <summary>
        /// Constant that represents the message that is logged if a get materials price history request returns not found
        /// </summary>
        private const string LOG_GET_MATERIAL_PRICE_HISTORY_NOT_FOUND = "GET{id} Material Price History NotFound";

        /// <summary>
        /// Constant that represents the message that is logged if a get material finish price history request returns not found
        /// </summary>
        private const string LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_NOT_FOUND = "GET{finishId} Finish Price History of Material {materialId} NotFound";

        /// <summary>
        /// Constant that represents the message that is logged if a get all material finishes price history request returns not found
        /// </summary>
        private const string LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_NOT_FOUND = "GET ALL Finishes Price History of Material {id} NotFound";

        /// <summary>
        /// Constant that represents the message that is logged if a post material price table entry returns not found
        /// </summary>
        private const string LOG_POST_MATERIAL_ENTRY_MATERIAL_NOT_FOUND = "POST{@modelView} Material Price Table Entry Material{id} NotFound";

        /// <summary>
        /// Material Price Table Repository
        /// </summary>
        private readonly MaterialPriceTableRepository materialPriceTableRepository;

        /// <summary>
        /// Finish Price Table Repository
        /// </summary>
        private readonly FinishPriceTableRepository finishPriceTableRepository;

        /// <summary>
        /// Controller's logger
        /// </summary>
        private readonly ILogger<PriceTablesController> logger;

        /// <summary>
        /// Injected client factory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Constructor with injected type of repositories
        /// </summary>
        /// <param name="materialPriceTableRepository">material price table repository</param>
        /// <param name="finishPriceTableRepository">finish price table repository</param>
        /// <param name="logger">controller's logger</param>
        public PriceTablesController(MaterialPriceTableRepository materialPriceTableRepository, FinishPriceTableRepository finishPriceTableRepository, ILogger<PriceTablesController> logger, IHttpClientFactory clientFactory)
        {
            this.materialPriceTableRepository = materialPriceTableRepository;
            this.finishPriceTableRepository = finishPriceTableRepository;
            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Fetches price history of all materials
        /// </summary>
        /// <returns>HTTP Response 200; OK with the price history of all materials
        ///          HTTP Response 404; Not Found if there no material has a price history
        /// </returns>
        [HttpGet("materials")]
        public ActionResult fetchAllMaterialsPriceHistory()
        {
            logger.LogInformation(LOG_GET_ALL_MATERIALS_PRICE_HISTORY_REQUEST_START);
            try
            {
                GetAllMaterialPriceHistoryModelView modelView = new core.application.PriceTablesController().fetchPriceHistoryOfAllMaterials();
                logger.LogInformation(LOG_GET_ALL_MATERIALS_PRICE_HISTORY_SUCCESS, modelView);
                return Ok(modelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(e, LOG_GET_ALL_MATERIALS_PRICE_HISTORY_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        [HttpGet("materials/{materialID}/finishes")]
        public ActionResult fetchAllMaterialFinishesPriceHistory(long materialID)
        {
            logger.LogInformation(LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_REQUEST_START, materialID);
            try
            {
                FetchMaterialFinishPriceHistoryDTO priceHistoryDTO = new FetchMaterialFinishPriceHistoryDTO();
                priceHistoryDTO.materialID = materialID;
                GetAllMaterialFinishPriceHistoryModelView allMaterialFinishesPriceHistoryModelView = new core.application.PriceTablesController().fetchPriceHistoryOfAllMaterialFinishes(priceHistoryDTO);
                logger.LogInformation(LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_SUCCESS, materialID, allMaterialFinishesPriceHistoryModelView);
                return Ok(allMaterialFinishesPriceHistoryModelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(e, LOG_GET_ALL_MATERIAL_FINISHES_PRICE_HISTORY_NOT_FOUND, materialID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 404; Not Found if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}")]
        public ActionResult fetchMaterialPriceHistory(long materialID)
        {
            logger.LogInformation(LOG_GET_MATERIAL_PRICE_HISTORY_REQUEST_START, materialID);
            FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO = new FetchMaterialPriceHistoryDTO();
            fetchMaterialPriceHistoryDTO.materialID = materialID;
            try
            {
                GetAllMaterialPriceHistoryModelView materialPriceHistoryModelView = new core.application.PriceTablesController().fetchMaterialPriceHistory(fetchMaterialPriceHistoryDTO);
                logger.LogInformation(LOG_GET_MATERIAL_PRICE_HISTORY_SUCCESS, materialID, materialPriceHistoryModelView);
                return Ok(materialPriceHistoryModelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(e, LOG_GET_MATERIAL_PRICE_HISTORY_NOT_FOUND, materialID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <param name="finishID">Long with the resource ID of the material finish being fetched the price history</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 404; Not Found if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}/finishes/{finishID}")]
        public ActionResult fetchMaterialFinishPriceHistory(long materialID, long finishID)
        {
            logger.LogInformation(LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_REQUEST_START, finishID, materialID);
            FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO = new FetchMaterialFinishPriceHistoryDTO();
            fetchMaterialFinishPriceHistoryDTO.materialID = materialID;
            fetchMaterialFinishPriceHistoryDTO.finishID = finishID;
            try
            {
                GetAllMaterialFinishPriceHistoryModelView materialFinishPriceHistoryModelView = new core.application.PriceTablesController().fetchMaterialFinishPriceHistory(fetchMaterialFinishPriceHistoryDTO);
                logger.LogInformation(LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_SUCCESS, finishID, materialID, materialFinishPriceHistoryModelView);
                return Ok(materialFinishPriceHistoryModelView);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(e, LOG_GET_MATERIAL_FINISH_PRICE_HISTORY_NOT_FOUND, finishID, materialID);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
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
            logger.LogInformation(LOG_POST_START);
            try
            {
                modelView.entityId = id;
                GetMaterialPriceModelView createdPrice = await new core.application.PriceTablesController().addMaterialPriceTableEntry(modelView, clientFactory);
                if (createdPrice == null)
                {
                    logger.LogWarning(LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                    return BadRequest(new SimpleJSONMessageService(PRICE_ENTRY_NOT_CREATED));
                }
                logger.LogInformation(LOG_POST_MATERIAL_ENTRY_SUCCESS, createdPrice, createdPrice.id);
                return Created(Request.Path, createdPrice);
            }
            catch (ResourceNotFoundException e)
            {
                logger.LogWarning(e, LOG_POST_MATERIAL_ENTRY_MATERIAL_NOT_FOUND, modelView, id);
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                logger.LogWarning(e, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                logger.LogWarning(e, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                logger.LogWarning(e, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
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
            logger.LogInformation(LOG_POST_START);
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                GetMaterialFinishPriceModelView createdPrice = await new core.application.PriceTablesController().addFinishPriceTableEntry(modelView, clientFactory);
                if (createdPrice == null)
                {
                    logger.LogWarning(LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                    return BadRequest(new SimpleJSONMessageService(PRICE_ENTRY_NOT_CREATED));
                }
                logger.LogInformation(LOG_POST_MATERIAL_ENTRY_SUCCESS, createdPrice, createdPrice.finishId, createdPrice.id);
                return Created(Request.Path, createdPrice);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                logger.LogWarning(e, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                logger.LogWarning(e, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                logger.LogWarning(e, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
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
            logger.LogInformation(LOG_PUT_START);
            try
            {
                modelView.entityId = materialid;
                modelView.tableEntryId = entryid;
                GetMaterialPriceModelView updatedEntry = await new core.application.PriceTablesController().updateMaterialPriceTableEntry(modelView, clientFactory);
                if (updatedEntry == null)
                {
                    logger.LogWarning(LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                    return BadRequest(new SimpleJSONMessageService(ENTRY_NOT_UPDATED));
                }
                logger.LogInformation(LOG_PUT_MATERIAL_ENTRY_SUCCESS, entryid, materialid, modelView);
                return Ok(updatedEntry);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                logger.LogWarning(e, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                logger.LogWarning(e, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                logger.LogWarning(e, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
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
            logger.LogInformation(LOG_PUT_START);
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                modelView.tableEntryId = entryid;
                GetMaterialFinishPriceModelView updatedEntry = await new core.application.PriceTablesController().updateFinishPriceTableEntry(modelView, clientFactory);
                if (updatedEntry == null)
                {
                    logger.LogWarning(LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                    return BadRequest(new { error = ENTRY_NOT_UPDATED });
                }
                logger.LogInformation(LOG_PUT_FINISH_ENTRY_SUCCESS, entryid, finishid, materialid, modelView);
                return Ok(updatedEntry);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (NullReferenceException e)
            {
                logger.LogWarning(e, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (InvalidOperationException e)
            {
                logger.LogWarning(e, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (UnparsableValueException e)
            {
                logger.LogWarning(e, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        //TODO GET Current Material Price & GET Current Material Finish Price
    }
}