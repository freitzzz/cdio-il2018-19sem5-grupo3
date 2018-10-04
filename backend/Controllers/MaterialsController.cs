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
            List<MaterialDTO> materials = new core.application.MaterialsController(materialRepository).findAllMaterials();

            if (materials == null) {
                return BadRequest(NO_MATERIALS_FOUND_REFERENCE);
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
        [HttpGet("{id}")]
        public ActionResult<MaterialDTO> findById(long materialID) {
            MaterialDTO materialDTO = new core.application.MaterialsController(materialRepository).findMaterialByID(materialID);

            if (materialDTO == null) {
                return BadRequest(MATERIAL_NOT_FOUND_REFERENCE);
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
            MaterialDTO removedDTO = new core.application.MaterialsController(materialRepository).removeMaterial(materialID);

            if (removedDTO == null) {
                return BadRequest(MATERIAL_NOT_REMOVED_REFERENCE);
            }

            return Ok(removedDTO);
        }

        /// <summary>
        /// Adds a material given its DTO.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not added;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.</returns>
        [HttpPost]
        public ActionResult<MaterialDTO> add([FromBody] JObject jsonData) {
            {
                MaterialObject materialObject = JsonConvert.DeserializeObject<MaterialObject>(jsonData.ToString());

                MaterialDTO materialDTO = materialObjectToMaterialDTO(materialObject);
                MaterialDTO addedDTO = new core.application.MaterialsController(materialRepository).addMaterial(materialDTO);

                if (addedDTO == null) {
                    return BadRequest(MATERIAL_NOT_ADDED_REFERENCE);
                }
                return Ok(addedDTO);
            }
        }

        /// <summary>
        /// Parses a MaterialObjec into a Material DTO.
        /// </summary>
        /// <param name="materialObject">MaterialObject with the Material's info</param>
        /// <returns>DTO with the parsed MaterialObject</returns>
        private MaterialDTO materialObjectToMaterialDTO(MaterialObject materialObject) {
            MaterialDTO materialDTO = new MaterialDTO();

            materialDTO.reference = materialObject.reference; //Holds the reference of the Material
            materialDTO.designation = materialObject.designation; //Holds the designation of the Material

            List<ColorDTO> colorsDTOList = new List<ColorDTO>();

            if (!Collections.isListEmpty(materialObject.colors)) {
                foreach (Color color in materialObject.colors) {
                    ColorDTO colorDTO = new ColorDTO();
                    colorDTO.name = color.Name;
                    colorDTO.red = color.Red;
                    colorDTO.green = color.Green;
                    colorDTO.blue = color.Blue;
                    colorDTO.alpha = color.Alpha;

                    colorsDTOList.Add(colorDTO);
                }
            }

            materialDTO.colors = colorsDTOList;

            List<FinishDTO> finishesDTOList = new List<FinishDTO>();

            if (!Collections.isListEmpty(materialObject.finishes)) {
                foreach (Finish finish in materialObject.finishes) {
                    FinishDTO finishDTO = new FinishDTO();
                    finishDTO.description = finish.description;
                    finishesDTOList.Add(finishDTO);
                }
            }

            materialDTO.finishes = finishesDTOList;

            return materialDTO;
        }

        /// <summary>
        /// Updates a material given its DTO.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not updated;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.</returns>
        [HttpPut]
        public ActionResult<MaterialDTO> update([FromBody] JObject jsonData, long materialID) {
            MaterialObject materialObject = JsonConvert.DeserializeObject<MaterialObject>(jsonData.ToString());
            MaterialDTO materialDTO = materialObjectToMaterialDTO(materialObject);

            materialObject.persistenceID = materialID;
            List<ColorDTO> colors = materialDTO.colors;
            List<FinishDTO> finishes = materialDTO.finishes;

            bool wasUpdated = new core.application.MaterialsController(materialRepository).updateMaterial(materialDTO);

            if (!wasUpdated) {
                return BadRequest();
            }

            return Ok();
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
                MaterialDTO matDTO = new core.application.MaterialsController(materialRepository).updateFinishes(id, finishes);
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
        /// Auxiliar MaterialObject class used in the deserialization of a Material's updates/addition from JSON format.
        /// </summary>
        public class MaterialObject {
            /// <summary>
            /// Material's reference.
            /// </summary>
            public string reference { get; set; }

            /// <summary>
            /// Materials's designation.
            /// </summary>
            public string designation { get; set; }

            /// <summary>
            /// Material's persistence ID.
            /// </summary>
            public long persistenceID { get; set; }
            /// <summary>
            /// List of colors.
            /// </summary>
            public List<Color> colors { get; set; }

            /// <summary>
            /// List of finishes.
            /// </summary>
            public List<Finish> finishes { get; set; }
        }
    }
}