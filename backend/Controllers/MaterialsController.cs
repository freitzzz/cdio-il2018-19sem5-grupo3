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
using backend.utils;
using static backend.utils.JSONStringFormatter;

namespace backend.Controllers
{
    //<summary>
    //Backend MaterialsController class.
    //</summary>
    [Route("myc/api/materials")]
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


        private readonly MaterialRepository materialRepository;

        public MaterialsController(MaterialRepository materialRepository)
        {
            this.materialRepository = materialRepository;
        }

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
                string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, NO_MATERIALS_FOUND_REFERENCE);
                return BadRequest(jsonFormattedMessage);
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
                    string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, MATERIAL_NOT_FOUND_REFERENCE);
                    return BadRequest(jsonFormattedMessage);
                }
                return Ok(materialDTO);
            }catch(NullReferenceException){
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
            try
            {
                MaterialDTO createdMaterialDTO = new core.application.MaterialsController().addMaterial(materialDTO);
                if (createdMaterialDTO != null)
                {
                    return Created(Request.Path, createdMaterialDTO);
                }
                else
                {
                    //TODO:????????
                    return BadRequest();
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
            bool disabledWithSuccess = new core.application.MaterialsController().disableMaterial(materialDTO);
            if (disabledWithSuccess)
            {
                return NoContent();
            }
            else
            {
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
            try
            {
                 updateMaterialData.id = id;
                if (new core.application.MaterialsController().updateMaterialBasicInformation(updateMaterialData))
                    return Ok(new SimpleJSONMessageService(VALID_MATERIAL_UPDATE_MESSAGE));
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }
        /// <summary>
        /// Updates finishes of a material
        /// </summary>
        /// <param name="id">id of the material to be updated</param>
        /// <param name="upMat">dto with the list of finishes to add and remove</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpPut("{id}/finishes")]
        public ActionResult updateFinishes(long id, [FromBody] UpdateMaterialDTO upMat)
        {
            upMat.id = id;
            try
            {
                if (new core.application.MaterialsController().updateFinishes(upMat))
                {
                    return Ok(new SimpleJSONMessageService(VALID_MATERIAL_UPDATE_MESSAGE));
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }/// <summary>
         /// Updates colors of a material
         /// </summary>
         /// <param name="id">id of the material to be updated</param>
         /// <param name="upMat">dto with the list of colors to add and remove</param>
         /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpPut("{id}/finishes")]
        public ActionResult updateColors(long id, [FromBody] UpdateMaterialDTO upMat)
        {
            upMat.id = id;
            try
            {
                if (new core.application.MaterialsController().updateColors(upMat))
                {
                    return Ok(new SimpleJSONMessageService(VALID_MATERIAL_UPDATE_MESSAGE));
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            return BadRequest(new SimpleJSONMessageService(INVALID_MATERIAL_UPDATE_MESSAGE));
        }



    }
}