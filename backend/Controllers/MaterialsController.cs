using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using support.dto;
using System.Collections.Generic;


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

        public ActionResult<DTO> findById(string materialID)
        {
            DTO materialDTO = new core.application.MaterialsController().findMaterialByID(materialID);

            if (materialDTO == null)
            {
                return BadRequest(MATERIAL_NOT_FOUND_REFERENCE);
            }

            return Ok(materialDTO);
        }
    }
}