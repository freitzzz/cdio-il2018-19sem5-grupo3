using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Controllers;
using core.application;
using core.domain;
using support.dto;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using support.utils;
using core.persistence;
using core.dto;
using core.modelview.material;
using backend.utils;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    //<summary>
    //Backend MaterialsController class.
    //</summary>
    [Route("mycm/api/materials")]
    public class MaterialsController : Controller
    {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no Materials are found.
        /// </summary>
        private const string NO_MATERIALS_FOUND_REFERENCE = "No materials found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not found.
        /// </summary>
        private const string MATERIAL_NOT_FOUND_REFERENCE = "Material not found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not removed.
        /// </summary>
        private const string MATERIAL_NOT_REMOVED_REFERENCE = "Could not remove material";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not removed.
        /// </summary>
        private const string MATERIAL_NOT_ADDED_REFERENCE = "Could not add material";
        /// <summary>
        /// Constant that represents the message that ocurres if the update of a material is successful
        /// </summary>
        private const string VALID_MATERIAL_UPDATE_MESSAGE = "Material was updated with success";
        /// <summary>
        /// Constant that represents the message that ocurres if the update of a material fails
        /// </summary>
        private const string INVALID_MATERIAL_UPDATE_MESSAGE = "An error occured while updating the material";

        /// <summary>
        /// Constant that represents the message for creating a Material with an invalid Request Body.
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid\nCheck documentation for more information";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START = "GET By ID Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START = "POST Request started";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request starts
        /// </summary>
        private const string LOG_DELETE_START = "DELETE Request started";

        /// <summary>
        /// Constant that represents the log message for when a PUT Basic Info Request starts
        /// </summary>
        private const string LOG_PUT_BASIC_INFO_START = "PUT Basic Info Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Finish Request starts
        /// </summary>
        private const string LOG_POST_FINISH_START = "POST Finish Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Color Request starts
        /// </summary>
        private const string LOG_POST_COLOR_START = "POST Color Request started";
         /// <summary>
        /// Constant that represents the log message for when a DELETE Finish Request starts
        /// </summary>
        private const string LOG_DELETE_FINISH_START = "DELETE Finish Request started";
         /// <summary>
        /// Constant that represents the log message for when a DELETE Color Request starts
        /// </summary>
        private const string LOG_DELETE_COLOR_START = "DELETE Color Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST = "GET All BadRequest (No Materials Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST = "GETByID({id}) BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@material} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a DELETE Request returns a BadRequest
        /// </summary>
        private const string LOG_DELETE_BAD_REQUEST = "DELETE({id}) BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Material with id {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Materials {@materials} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Material {@material} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS = "Material {@material} created";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_DELETE_SUCCESS = "Material with id {id} soft deleted";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Material with id {id} updated with info {@updateInfo}";

        /// <summary>
        /// Repository used to manipulate Material instances
        /// </summary>
        private readonly MaterialRepository materialRepository;

        /// <summary>
        /// MaterialsControllers logger
        /// </summary>
        private readonly ILogger<MaterialsController> logger;

        /// <summary>
        /// Constructor with injected type of repository
        /// </summary>
        /// <param name="materialRepository">Repository to be used to manipulate Material instances</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public MaterialsController(MaterialRepository materialRepository, ILogger<MaterialsController> logger)
        {
            this.materialRepository = materialRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Finds all Materials.
        /// </summary>
        /// <returns>
        /// HTTP Response 400 Bad Request if no Materials are found;
        /// <br>HTTP Response 200 Ok with the info of all Materials in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult<List<MaterialDTO>> findAll()
        {
            logger.LogInformation(LOG_GET_ALL_START);
            List<MaterialDTO> materials = new core.application.MaterialsController().findAllMaterials();

            if (Collections.isListEmpty(materials))
            {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(new SimpleJSONMessageService(NO_MATERIALS_FOUND_REFERENCE));
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, materials);
            return Ok(materials);
        }

        /// <summary>
        /// Finds a Material by its ID.
        /// </summary>
        /// <returns>
        /// <param name="materialID">ID of the Material</param>
        /// HTTP Response 400 Bad Request if the Material is not found;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.
        /// </returns>
        [HttpGet("{id}", Name = "GetMaterial")]
        public ActionResult<MaterialDTO> findById(long id)
        {
            logger.LogInformation(LOG_GET_BY_ID_START);
            try
            {
                MaterialDTO materialDTO = new core.application.MaterialsController().findMaterialByID(id);
                if (materialDTO == null)
                {
                    logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST, id);
                    return BadRequest(new SimpleJSONMessageService(MATERIAL_NOT_FOUND_REFERENCE));
                }
                logger.LogInformation(LOG_GET_BY_ID_SUCCESS, materialDTO);
                return Ok(materialDTO);
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_GET_BY_ID_BAD_REQUEST, id);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }
        }

        /// <summary>
        /// Creates a new material
        /// </summary>
        /// <param name="jsonData">JObject with the material information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the material was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the material
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<MaterialDTO> addMaterial([FromBody]MaterialDTO materialDTO)
        {
            logger.LogInformation(LOG_POST_START);
            try
            {
                MaterialDTO createdMaterialDTO = new core.application.MaterialsController().addMaterial(materialDTO);
                if (createdMaterialDTO != null)
                {
                    logger.LogInformation(LOG_POST_SUCCESS, createdMaterialDTO);
                    return CreatedAtRoute("GetMaterial", new { id = createdMaterialDTO.id }, createdMaterialDTO);
                }
                else
                {
                    //TODO:????????
                    return BadRequest();
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, materialDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, materialDTO);
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        }

        /// <summary>
        /// Disables a material
        /// </summary>
        /// <param name="id">Long with the material being disabled ID</param>
        /// <returns>HTTP Response 204;No Content if the material was disabled with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while disabling the material
        /// </returns>
        /// 
        [HttpDelete("{id}")]
        public ActionResult disableMaterial(long id)
        {
            logger.LogInformation(LOG_DELETE_START);
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.id = id;
            bool disabledWithSuccess = new core.application.MaterialsController().disableMaterial(materialDTO);
            if (disabledWithSuccess)
            {
                logger.LogInformation(LOG_DELETE_SUCCESS, id);
                return NoContent();
            }
            else
            {
                logger.LogWarning(LOG_DELETE_BAD_REQUEST, id);
                return BadRequest();
            }
        }
        /// <summary>
        /// Updates a material basic information
        /// </summary>
        /// <param name="updateMaterialData">UpdateMaterialDTO with the basic information of the material being updated</param>
        /// <returns>HTTP Response 200;OK if the material was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the material
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult updateMaterialBasicInformation(long id, [FromBody] UpdateMaterialDTO updateMaterialData)
        {
            logger.LogInformation(LOG_PUT_BASIC_INFO_START);
            try
            {
                updateMaterialData.id = id;
                if (new core.application.MaterialsController().updateMaterialBasicInformation(updateMaterialData))
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, id, updateMaterialData);
                    return Ok(new SimpleJSONMessageService(VALID_MATERIAL_UPDATE_MESSAGE));
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, id, updateMaterialData);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, id, updateMaterialData);
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Add finish of a material for update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="addFinishDTO">FinishDTO with the information of finish to add</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpPost("{idMaterial}/finishes")]
        public ActionResult addFinish(long idMaterial, [FromBody] FinishDTO addFinishDTO)
        {
            logger.LogInformation(LOG_POST_FINISH_START);
            try
            {
                AddFinishModelView addFinishModelView = new core.application.MaterialsController().addFinish(idMaterial, addFinishDTO);
                 if (addFinishModelView != null)
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, idMaterial, addFinishDTO);
                    return Created(Request.Path, addFinishModelView);
                    }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, idMaterial, addFinishDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, idMaterial, addFinishDTO);
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }
        /// <summary>
        /// Remove finish of a material for update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idFinish">id of the finish to be remove</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpDelete("{idMaterial}/finishes/{idFinish}")]
        public ActionResult removeFinish(long idMaterial, long idFinish)
        {
            logger.LogInformation(LOG_DELETE_FINISH_START);
            try
            {
                if (new core.application.MaterialsController().removeFinish(idMaterial, idFinish))
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, idMaterial, idFinish);
                    return NoContent();
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, idMaterial, idFinish);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, idMaterial, idFinish);
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Add color of a material for update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="addColorDTO">ColorDTO with the information of finish to add</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpPost("{idMaterial}/colors")]
        public ActionResult addColor(long idMaterial, [FromBody] ColorDTO addColorDTO)
        {
            logger.LogInformation(LOG_POST_COLOR_START);
            try
            {
                AddColorModelView addColorModelView = new core.application.MaterialsController().addColor(idMaterial, addColorDTO);
               if (addColorModelView != null)
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, idMaterial, addColorDTO);
                    return Created(Request.Path, addColorModelView);
                    }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, idMaterial, addColorDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, idMaterial, addColorDTO);
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }
        /// <summary>
        /// Remove color of a material for update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idColor">id of the color to be remove</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpDelete("{idMaterial}/colors/{idColor}")]
        public ActionResult removeColor(long idMaterial, long idColor)
        {
            logger.LogInformation(LOG_DELETE_COLOR_START);
            try
            {
                if (new core.application.MaterialsController().removeColor(idMaterial, idColor))
                {
                    logger.LogInformation(LOG_PUT_SUCCESS, idMaterial, idColor);
                    return NoContent();
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_PUT_BAD_REQUEST, idMaterial, idColor);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            logger.LogWarning(LOG_PUT_BAD_REQUEST, idMaterial, idColor);
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }
    }
}