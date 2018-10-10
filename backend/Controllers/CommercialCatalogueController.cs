
using System.Collections.Generic;
using core.dto;
using core.persistence;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    //<summary>
    //Backend CommercialCatalogueController class.
    //</summary>
    [Route("myc/api/CommercialCatalogue")]
    public class CommercialCatalogueController : Controller
    {
        private readonly CommercialCatalogueRepository commercialCatalogueRepository;

        public CommercialCatalogueController(CommercialCatalogueRepository commercialCatalogueRepository){
            this.commercialCatalogueRepository = commercialCatalogueRepository;
        }
         /// <summary>
        /// Finds all CommercialCatalogues.
        /// </summary>
        /// <returns>
        /// HTTP Response 400 Bad Request if no Materials are found;
        /// <br>HTTP Response 200 Ok with the info of all Materials in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult<List<CommercialCatalogueDTO>> findAll()
        {
            // List<MaterialDTO> materials = new core.application.MaterialsController().findAllMaterials();

            // if (Collections.isListEmpty(materials))
            // {
            //     string jsonFormattedMessage = JSONStringFormatter.formatMessageToJson(MessageTypes.ERROR_MSG, NO_MATERIALS_FOUND_REFERENCE);
            //     return BadRequest(jsonFormattedMessage);
            // }

            return Ok(null);
        }
    }
}