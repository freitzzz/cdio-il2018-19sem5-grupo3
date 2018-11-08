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
    }
}