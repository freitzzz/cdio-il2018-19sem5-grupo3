using System;
using core.application;
using core.persistence;
using core.modelview.pricetableentries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime.Text;

namespace backend.Controllers
{
    /// <summary>
    /// Controller class to handle requests related to price tables
    /// </summary>
    [Route("mycm/api/prices")]
    public class PriceTablesController : Controller
    {
        /// <summary>
        /// Constant that represents the message that occurs if a price entry for a material isn't created
        /// </summary>
        private const string PRICE_ENTRY_NOT_CREATED = "The price entry wasn't created";

        /// <summary>
        /// Constant that represents the message that occurs if a price entry isn't updated
        /// </summary>
        private const string ENTRY_NOT_UPDATED = "The price entry wasn't updated";

        /// <summary>
        /// Constant that represents the message that occurs if a price entry is updated successfully
        /// </summary>
        private const string ENTRY_UPDATE_SUCCESSFUL = "The price entry was updated successfully";

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
        /// Constructor with injected type of repositories
        /// </summary>
        /// <param name="materialPriceTableRepository">material price table repository</param>
        /// <param name="finishPriceTableRepository">finish price table repository</param>
        /// <param name="logger">controller's logger</param>
        public PriceTablesController(MaterialPriceTableRepository materialPriceTableRepository, FinishPriceTableRepository finishPriceTableRepository, ILogger<PriceTablesController> logger)
        {
            this.materialPriceTableRepository = materialPriceTableRepository;
            this.finishPriceTableRepository = finishPriceTableRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 400; Bad Request if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}")]
        public ActionResult fetchMaterialPriceHistory(long materialID){throw new NotImplementedException();}

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="materialID">Long with the resource ID of the material being fetched the price history</param>
        /// <param name="finishID">Long with the resource ID of the material finish being fetched the price history</param>
        /// <returns>HTTP Response 200; OK with the material price history
        ///      <br>HTTP Response 400; Bad Request if there is no price history for the given material
        /// </returns>
        [HttpGet("materials/{materialID}/finishes/{finishID}")]
        public ActionResult fetchMaterialPriceHistory(long materialID,long finishID){throw new NotImplementedException();}

        /// <summary>
        /// Adds a new price table entry for a given material
        /// </summary>
        /// <param name="id">PID of the material</param>
        /// <param name="modelView">AddMaterialPriceTableEntryModelView with the price's information</param>
        /// <returns>ActionResult with HTTP Code 201 if the creation is successful
        ///         Or ActionResult with HTTP Code 401 if an error happens</returns>
        [HttpPost("materials/{id}")]
        public ActionResult addMaterialPriceTableEntry(long id, [FromBody] AddPriceTableEntryModelView modelView)
        {
            logger.LogInformation(LOG_POST_START);
            try
            {
                modelView.entityId = id;
                AddPriceTableEntryModelView createdPrice = new core.application.PriceTablesController().addMaterialPriceTableEntry(modelView);
                if (createdPrice == null)
                {
                    logger.LogWarning(LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                    return BadRequest(new { error = PRICE_ENTRY_NOT_CREATED });
                }
                logger.LogInformation(LOG_POST_MATERIAL_ENTRY_SUCCESS, createdPrice, createdPrice.entityId);
                return Created(Request.Path, createdPrice);
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new { error = nullReferenceException.Message });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new { error = invalidOperationException.Message });
            }
            catch (UnparsableValueException unparsableValueException)
            {
                logger.LogWarning(unparsableValueException, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new { error = unparsableValueException.Message });
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_MATERIAL_ENTRY_BAD_REQUEST, modelView, id);
                return BadRequest(new { error = argumentException.Message });
            }
        }

        /// <summary>
        /// Adds a new price table entry for a given material
        /// </summary>
        /// <param name="id">PID of the material</param>
        /// <param name="modelView">AddMaterialPriceTableEntryModelView with the price's information</param>
        /// <returns>ActionResult with HTTP Code 201 if the creation is successful
        ///         Or ActionResult with HTTP Code 401 if an error happens</returns>
        [HttpPost("materials/{materialid}/finishes/{finishid}")]
        public ActionResult addFinishPriceTableEntry(long materialid, long finishid, [FromBody] AddFinishPriceTableEntryModelView modelView)
        {
            logger.LogInformation(LOG_POST_START);
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                AddFinishPriceTableEntryModelView createdPrice = new core.application.PriceTablesController().addFinishPriceTableEntry(modelView);
                if (createdPrice == null)
                {
                    logger.LogWarning(LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                    return BadRequest(new { error = PRICE_ENTRY_NOT_CREATED });
                }
                logger.LogInformation(LOG_POST_MATERIAL_ENTRY_SUCCESS, createdPrice, createdPrice.finishId, createdPrice.entityId);
                return Created(Request.Path, createdPrice);
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new { error = nullReferenceException.Message });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new { error = invalidOperationException.Message });
            }
            catch (UnparsableValueException unparsableValueException)
            {
                logger.LogWarning(unparsableValueException, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new { error = unparsableValueException.Message });
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_FINISH_ENTRY_BAD_REQUEST, modelView, finishid, materialid);
                return BadRequest(new { error = argumentException.Message });
            }
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="materialid">material's PID</param>
        /// <param name="entryid">price table entry's PID</param>
        /// <param name="modelView">model view with the update information</param>
        /// <returns>ActionResult with HTTP Code 200 if the update is successful
        ///         Or ActionResult with HTTP Code 401 if the update isn't successful</returns>
        [HttpPut("materials/{materialid}/entries/{entryid}")]
        public ActionResult updateMaterialPriceTableEntry(long materialid, long entryid, [FromBody] UpdatePriceTableEntryModelView modelView)
        {
            logger.LogInformation(LOG_PUT_START);
            try
            {
                modelView.entityId = materialid;
                modelView.tableEntryId = entryid;
                if (!new core.application.PriceTablesController().updateMaterialPriceTableEntry(modelView))
                {
                    logger.LogWarning(LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                    return BadRequest(new { error = ENTRY_NOT_UPDATED });
                }
                logger.LogInformation(LOG_PUT_MATERIAL_ENTRY_SUCCESS, entryid, materialid, modelView);
                return Ok(new { message = ENTRY_UPDATE_SUCCESSFUL });
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new { error = nullReferenceException.Message });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new { error = invalidOperationException.Message });
            }
            catch (UnparsableValueException unparsableValueException)
            {
                logger.LogWarning(unparsableValueException, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new { error = unparsableValueException.Message });
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_PUT_MATERIAL_ENTRY_BAD_REQUEST, modelView, entryid, materialid);
                return BadRequest(new { error = argumentException.Message });
            }
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="materialid">PID of the material that the finish belongs to</param>
        /// <param name="finishid">finish's PID</param>
        /// <param name="entryid">finish's price table entry's PID</param>
        /// <param name="modelView">model view with the update information</param>
        /// <returns></returns>
        [HttpPut("materials/{materialid}/finishes/{finishid}/entries/{entryid}")]
        public ActionResult updateFinishPriceTableEntry(long materialid, long finishid, long entryid, [FromBody] UpdateFinishPriceTableEntryModelView modelView)
        {
            logger.LogInformation(LOG_PUT_START);
            try
            {
                modelView.entityId = materialid;
                modelView.finishId = finishid;
                modelView.tableEntryId = entryid;
                if (!new core.application.PriceTablesController().updateFinishPriceTableEntry(modelView))
                {
                    logger.LogWarning(LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                    return BadRequest(new { error = ENTRY_NOT_UPDATED });
                }
                logger.LogInformation(LOG_PUT_FINISH_ENTRY_SUCCESS, entryid, finishid, materialid, modelView);
                return Ok(new { message = ENTRY_UPDATE_SUCCESSFUL });
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new { error = nullReferenceException.Message });
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new { error = invalidOperationException.Message });
            }
            catch (UnparsableValueException unparsableValueException)
            {
                logger.LogWarning(unparsableValueException, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new { error = unparsableValueException.Message });
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_PUT_FINISH_ENTRY_BAD_REQUEST, modelView, entryid, finishid, materialid);
                return BadRequest(new { error = argumentException.Message });
            }
        }
    }
}