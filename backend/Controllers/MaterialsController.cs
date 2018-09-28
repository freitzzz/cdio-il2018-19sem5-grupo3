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


namespace backend.Controllers
{
    //<summary>
    //Backend MaterialsController class.
    //</summary>
    [Route("myc/materials")]
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
        /// Finds all Materials.
        /// </summary>
        /// <returns>
        /// HTTP Response 400 Bad Request if no Materials are found;
        /// <br>HTTP Response 200 Ok with the info of all Materials in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult<List<DTO>> findAll()
        {
            List<DTO> materials = new core.application.MaterialsController().findAllMaterials();

            if (materials == null)
            {
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
        public ActionResult<DTO> findByID(string materialID)
        {
            DTO materialDTO = new core.application.MaterialsController().findMaterialByID(materialID);

            if (materialDTO == null)
            {
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
        public ActionResult<DTO> remove(string materialID)
        {
            DTO removedDTO = new core.application.MaterialsController().removeMaterial(materialID);

            if (removedDTO == null)
            {
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
        public ActionResult<DTO> add([FromBody] JObject jsonData)
        {
            {
                MaterialObject materialObject = JsonConvert.DeserializeObject<MaterialObject>(jsonData.ToString());

                DTO materialDTO = materialObjectToMaterialDTO(materialObject);
                DTO addedDTO = new core.application.MaterialsController().addMaterial(materialDTO);

                if (addedDTO == null)
                {
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
        private DTO materialObjectToMaterialDTO(MaterialObject materialObject)
        {
            DTO materialDTO = new GenericDTO(Material.Properties.CONTEXT);

            materialDTO.put(Material.Properties.REFERENCE_PROPERTY, materialObject.reference); //Holds the reference of the Material
            materialDTO.put(Material.Properties.DESIGNATION_PROPERTY, materialObject.designation); //Holds the designation of the Material

            List<DTO> colorsDTOList = new List<DTO>();

            if (!Collections.isListEmpty(materialObject.colors))
            {
                foreach (Color color in materialObject.colors)
                {
                    DTO colorDTO = new GenericDTO("color");
                    colorDTO.put("name", color.Name);
                    colorDTO.put("red", color.Red);
                    colorDTO.put("green", color.Green);
                    colorDTO.put("blue", color.Blue);
                    colorDTO.put("alpha", color.Alpha);

                    colorsDTOList.Add(colorDTO);
                }
            }

            materialDTO.put(Material.Properties.COLORS_PROPERTY, colorsDTOList);

            List<DTO> finishesDTOList = new List<DTO>();

            if (!Collections.isListEmpty(materialObject.finishes))
            {
                foreach (Finish finish in materialObject.finishes)
                {
                    DTO finishDTO = new GenericDTO("finish");
                    finishDTO.put("description", finish.description);
                    finishesDTOList.Add(finishDTO);
                }
            }

            materialDTO.put(Material.Properties.FINISHES_PROPERTY, finishesDTOList);

            return materialDTO;
        }

        /// <summary>
        /// Updates a material given its DTO.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>HTTP Response 400 Bad Request if the Material is not updated;
        /// <br>HTTP Response 200 Ok with the info of the Material in JSON format.</returns>
        //[HttpPut]
        //public ActionResult<DTO> update(DTO materialDTO){
        //  DTO updatedDTO = new core.application.MaterialsController().updateMaterial(materialDTO);            
        //}

        /// <summary>
        /// Auxiliar MaterialObject class used in the deserialization of a Material's updates/addition from JSON format.
        /// </summary>
        public class MaterialObject
        {
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