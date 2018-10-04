using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
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

namespace backend.Controllers {
    //<summary>
    //Backend MaterialsController class.
    //</summary>
    [Route("myc/materials")]
    public class MaterialsController : Controller {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no Materials are found.
        /// </summary>
        private const string NO_MATERIALS_FOUND_REFERENCE = "No materials found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a Material is not found.
        /// </summary>
        private const string MATERIAL_NOT_FOUND_REFERENCE = "Material not found";


        private readonly MaterialRepository materialRepository;

        public MaterialsController(MaterialRepository materialRepository) {
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
        /// Finds all Materials.
        /// </summary>
        /// <returns>
        /// HTTP Response 400 Bad Request if no Materials are found;
        /// <br>HTTP Response 200 Ok with the info of all Materials in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult<List<MaterialDTO>> findAll() {
            List<MaterialDTO> materials = new core.application.MaterialsController().findAllMaterials();

            if (materials == null) {
                string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, MATERIAL_NOT_FOUND_REFERENCE);
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
        public ActionResult<MaterialDTO> findById(long id) {
            MaterialDTO materialDTO = new core.application.MaterialsController(materialRepository).findMaterialByID(id);

            if (materialDTO == null) {
                string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, MATERIAL_NOT_FOUND_REFERENCE);
                return BadRequest(jsonFormattedMessage);
            }

            return Ok(materialDTO);
        }

        /// <summary>
        /// Removes a material given its DTO.
        /// </summary>
        /// <param name="materialID">ID of the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not removed;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.</returns>
        [HttpDelete("{id}")]
        public ActionResult<MaterialDTO> remove(long materialID) {
            MaterialDTO removedDTO = new core.application.MaterialsController().removeMaterial(materialID);

            if (removedDTO == null) {
                string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, MATERIAL_NOT_REMOVED_REFERENCE);
                return BadRequest(jsonFormattedMessage);
            }

            return Ok(removedDTO);
        }

        /// <summary>
        /// Adds a material given its DTO.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not added;
        /// <br>HTTP Response 201 Created with the info of the Material in JSON format.</returns>
        [HttpPost]
        public ActionResult<MaterialDTO> add([FromBody] MaterialDTO jsonData) {
            try {
                MaterialDTO addedDTO = new core.application.MaterialsController().addMaterial(jsonData);
                if (addedDTO == null) {
                    string formattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, MATERIAL_NOT_ADDED_REFERENCE);
                    return BadRequest(formattedMessage);
                }
                return CreatedAtRoute("GetMaterial", new {id = addedDTO.id}, addedDTO);
            } catch (ArgumentException e) {
                string formattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, e.Message);
                return BadRequest(formattedMessage);
            }
        }


        /// <summary>
        /// Updates a material given its DTO.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not updated;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.</returns>
        [HttpPut]
        public ActionResult<MaterialDTO> update([FromBody] MaterialDTO jsonData) {
            try {
                MaterialDTO matDTO = new core.application.MaterialsController().updateMaterial(jsonData);

                if (matDTO == null) {
                    return BadRequest();
                }

                return Ok(matDTO);
            } catch (ArgumentException e) {
                string formattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, e.Message);
                return BadRequest(formattedMessage);
            }
        }
        /// <summary>
        /// Updates the finishes of a material
        /// </summary>
        /// <param name="id">id of the material to be updated</param>
        /// <param name="finishes">new list of finishes</param>
        /// <returns>ActionResult with the 200 Http code and the updated material or ActionResult with the 400 Http code</returns>
        [HttpPut("{id}/finishes")]
        public ActionResult updateFinishes(long id, [FromBody] List<FinishDTO> finishes) {
            try {
                MaterialDTO matDTO = new core.application.MaterialsController().updateFinishes(id, finishes);
                if (matDTO == null) {
                    return BadRequest();
                }
                return Ok(matDTO);
            } catch (ArgumentException e) {
                string formattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, e.Message);
                return BadRequest(formattedMessage);
            }
        }
    }
}