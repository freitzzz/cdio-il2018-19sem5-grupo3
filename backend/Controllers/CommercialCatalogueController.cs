
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
    //Backend CommercialCatalogueController class.
    //</summary>
    [Route("myc/api/commercialcatalogue")]
    public class CommercialCatalogueController : Controller
    {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a commercialCatalogue
        /// is not found
        /// </summary>
        private const string COM_CATALOGUE_NOT_FOUND_REFERENCE = "Commercial Catalogue not found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no commercialCatalogues
        /// are found
        /// </summary>
        private const string NO_COM_CATALOGUES_FOUND_REFERENCE = "No Commercial Catalogues found";

        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to create a commercialCatalogue with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid\nCheck documentation for more information";

        private readonly CommercialCatalogueRepository commercialCatalogueRepository;

        public CommercialCatalogueController(CommercialCatalogueRepository commercialCatalogueRepository)
        {
            this.commercialCatalogueRepository = commercialCatalogueRepository;
        }
        /// <summary>
        /// Finds all CommercialCatalogues.
        /// </summary>
        /// <returns>
        /// HTTP Response 400 Bad Request if no Commercial Catalogues are found;
        /// <br>HTTP Response 200 Ok with the info of all Commercial Catalogues in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult<List<CommercialCatalogueDTO>> findAll()
        {
            List<CommercialCatalogueDTO> comCatalogues = new core.application.CommercialCatalogueController().findAll();

            if (comCatalogues == null)
            {
                return BadRequest(NO_COM_CATALOGUES_FOUND_REFERENCE);
            }

            return Ok(null);
        }
        /// <summary>
        /// Finds a commercialCatalogue by ID
        /// </summary>
        /// <param name="id"> id of the commercialCatalogue</param>
        /// <returns>HTTP Response 400 Bad Request if a commercialCatalogue with the id isn't found;
        /// HTTP Response 200 Ok with the commercialCatalogue's info in JSON format </returns>
        [HttpGet("{id}", Name = "GetCommercialCatalogue")]
        public ActionResult<CommercialCatalogueDTO> findById(long id)
        {
            CommercialCatalogueDTO comCatalogueDTOX = new CommercialCatalogueDTO();
            comCatalogueDTOX.id = id;
            CommercialCatalogueDTO comCatalogueDTOY = new core.application.CommercialCatalogueController().findComCatalogueByID(comCatalogueDTOX);
            if (comCatalogueDTOY == null)
            {
                return BadRequest(COM_CATALOGUE_NOT_FOUND_REFERENCE);
            }
            return Ok(comCatalogueDTOY);
        }
        /// <summary>
        /// Creates a new CommercialCatalogue
        /// </summary>
        /// <param name="jsonData">JObject with the commercialCatalogue information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the commercialCatalogue was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the commercialCatalogue
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<CommercialCatalogueDTO> addCommercialCatalogue([FromBody]CommercialCatalogueDTO comCatalogueDTO)
        {
            try
            {
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().addCommercialCatalogue(comCatalogueDTO);
                if (createdComCatalogueDTO != null)
                {
                    return Created(Request.Path, createdComCatalogueDTO);
                }
                else
                {
                    return BadRequest();
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
            catch (ArgumentException argumentException)
            {
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        }

        /// <summary>
        /// Adds a new collection 
        /// </summary>
        /// <param name="jsonData">JObject with the commercialCatalogue information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the commercialCatalogue was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the commercialCatalogue
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<CommercialCatalogueDTO> addCollection([FromBody]CommercialCatalogueDTO comCatalogueDTO, CustomizedProductCollectionDTO customizedProductCollectionDTO)
        {
            try
            {
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().addCollection(comCatalogueDTO, customizedProductCollectionDTO);
                if (createdComCatalogueDTO != null)
                {
                    return Created(Request.Path, createdComCatalogueDTO);
                }
                else
                {
                    return BadRequest();
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
            catch (ArgumentException argumentException)
            {
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        } 

        /// <summary>
        /// Removes a collection.
        /// </summary>
        /// <param name="jsonData">JObject with the commercialCatalogue information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the commercialCatalogue was created with success
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the commercialCatalogue
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<CommercialCatalogueDTO> removeCollection([FromBody]CommercialCatalogueDTO comCatalogueDTO, CustomizedProductCollectionDTO customizedProductCollectionDTO)
        {
            try
            {
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().removeCollection(comCatalogueDTO, customizedProductCollectionDTO);
                if (createdComCatalogueDTO != null)
                {
                    return Created(Request.Path, createdComCatalogueDTO);
                }
                else
                {
                    return BadRequest();
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
            catch (ArgumentException argumentException)
            {
                return BadRequest(new SimpleJSONMessageService(argumentException.Message));
            }
        } 
    }
}