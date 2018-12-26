using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using support.utils;
using core.persistence;
using core.dto;
using core.modelview.material;
using backend.utils;
using Microsoft.Extensions.Logging;
using core.exceptions;

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
        private const string NO_MATERIALS_FOUND = "No materials found.";
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not found.
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Material not found.";
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not removed.
        /// </summary>
        private const string UNABLE_TO_REMOVE_MATERIAL = "Could not remove the material.";
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not removed.
        /// </summary>
        private const string UNABLE_TO_ADD_MATERIAL = "Could not add the material.";
        /// <summary>
        /// Constant that represents the message that ocurres if the update of a material is successful
        /// </summary>
        private const string UPDATE_MATERIAL_SUCCESS = "The material was updated with success.";
        /// <summary>
        /// Constant that represents the message that ocurres if the update of a material fails
        /// </summary>
        private const string UNABLE_TO_UPDATE_MATERIAL = "An error occured while updating the material.";
        /// <summary>
        /// Constant that represents the message for creating a Material with an invalid Request Body.
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid\nCheck documentation for more information";

        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later.";

        /// <summary>
        /// Repository used to manipulate Material instances
        /// </summary>
        private readonly MaterialRepository materialRepository;

        /// <summary>
        /// Constructor with injected type of repository
        /// </summary>
        /// <param name="materialRepository">Repository to be used to manipulate Material instances</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public MaterialsController(MaterialRepository materialRepository)
        {
            this.materialRepository = materialRepository;
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
            List<MaterialDTO> materials = new core.application.MaterialsController().findAllMaterials();

            if (Collections.isListEmpty(materials))
            {
                return BadRequest(new SimpleJSONMessageService(NO_MATERIALS_FOUND));
            }

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
            try
            {
                MaterialDTO materialDTO = new core.application.MaterialsController().findMaterialByID(id);
                if (materialDTO == null)
                {
                    return BadRequest(new SimpleJSONMessageService(MATERIAL_NOT_FOUND));
                }

                return Ok(materialDTO);
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
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
            try
            {
                MaterialDTO createdMaterialDTO = new core.application.MaterialsController().addMaterial(materialDTO);
                if (createdMaterialDTO != null)
                {
                    return CreatedAtRoute("GetMaterial", new { id = createdMaterialDTO.id }, createdMaterialDTO);
                }
                else
                {
                    return BadRequest(new SimpleJSONMessageService(UNABLE_TO_ADD_MATERIAL));
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (ArgumentException argumentException)
            {
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
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.id = id;

            try
            {
                new core.application.MaterialsController().disableMaterial(materialDTO);
                return NoContent();
            }
            catch (ResourceNotFoundException)
            {
                return NotFound(new SimpleJSONMessageService(UNABLE_TO_REMOVE_MATERIAL));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
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
            try
            {
                updateMaterialData.id = id;
                if (new core.application.MaterialsController().updateMaterialBasicInformation(updateMaterialData))
                {
                    return Ok(new SimpleJSONMessageService(UPDATE_MATERIAL_SUCCESS));
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(UNABLE_TO_UPDATE_MATERIAL));
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
            try
            {
                AddFinishModelView addFinishModelView = new core.application.MaterialsController().addFinish(idMaterial, addFinishDTO);
                if (addFinishModelView != null)
                {
                    return Created(Request.Path, addFinishModelView);
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(UNABLE_TO_UPDATE_MATERIAL));
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
            try
            {
                if (new core.application.MaterialsController().removeFinish(idMaterial, idFinish))
                {
                    return NoContent();
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(UNABLE_TO_UPDATE_MATERIAL));
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
            try
            {
                AddColorModelView addColorModelView = new core.application.MaterialsController().addColor(idMaterial, addColorDTO);
                if (addColorModelView != null)
                {
                    return Created(Request.Path, addColorModelView);
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(UNABLE_TO_UPDATE_MATERIAL));
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
            try
            {
                if (new core.application.MaterialsController().removeColor(idMaterial, idColor))
                {
                    return NoContent();
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(UNABLE_TO_UPDATE_MATERIAL));
        }
    }
}