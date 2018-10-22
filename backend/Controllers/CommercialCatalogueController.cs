
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
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    ///<summary>
    ///Backend CommercialCatalogueController class.
    ///</summary>
    [Route("myc/api/commercialcatalogues")]
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
        /// Constant that represents the message that ocurres if the update of a material is successful
        /// </summary>
        private const string VALID_COMMERCIAL_CATALOGUE_UPDATE_MESSAGE = "Commercial Catalogue was updated with success";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a commercialCatalogue with an invalid request body
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
        /// Constant that represents the log message for when a PUT Request starts
        /// </summary>
        private const string LOG_PUT_START = "PUT Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST = "GET All BadRequest (No Commercial Catalogues Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST = "GETByID({id}) BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST = "POST {@catalogue} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request returns a BadRequest
        /// </summary>
        private const string LOG_PUT_BAD_REQUEST = "Commercial Catalogue with id {id} PUT {@updateInfo} BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Commercial Catalogues {@list} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS = "Commercial Catalogue {@commercialCatalogue} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS = "Commercial Catalogue {@commercialCatalogue} created";

        /// <summary>
        /// Constant that represents the log message for when a PUT Request is successful
        /// </summary>
        private const string LOG_PUT_SUCCESS = "Commercial Catalogue with id {id} updated with info {@updateInfo}";

        /// <summary>
        /// Repository used to manipulate CommercialCatalogue instances
        /// </summary>
        private readonly CommercialCatalogueRepository commercialCatalogueRepository;

        /// <summary>
        /// Controllers logger
        /// </summary>
        private readonly ILogger<CommercialCatalogueController> logger;

        /// <summary>
        /// Constructor with injected type of repository and logger
        /// </summary>
        /// <param name="commercialCatalogueRepository">repository used to manipulate CommercialCatalogue instances</param>
        /// <param name="logger">logger used to log messages regarding HTTP Requests and Responses</param>
        public CommercialCatalogueController(CommercialCatalogueRepository commercialCatalogueRepository, ILogger<CommercialCatalogueController> logger)
        {
            this.commercialCatalogueRepository = commercialCatalogueRepository;
            this.logger = logger;
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
            logger.LogInformation(LOG_GET_ALL_START);
            List<CommercialCatalogueDTO> comCatalogues = new core.application.CommercialCatalogueController().findAll();

            if (comCatalogues == null)
            {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(NO_COM_CATALOGUES_FOUND_REFERENCE);
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, comCatalogues);
            return Ok(comCatalogues);
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
            logger.LogInformation(LOG_GET_BY_ID_START);
            CommercialCatalogueDTO comCatalogueDTOX = new CommercialCatalogueDTO();
            comCatalogueDTOX.id = id;
            CommercialCatalogueDTO comCatalogueDTOY = new core.application.CommercialCatalogueController().findComCatalogueByID(comCatalogueDTOX);
            if (comCatalogueDTOY == null)
            {
                logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST, id);
                return BadRequest(COM_CATALOGUE_NOT_FOUND_REFERENCE);
            }
            logger.LogInformation(LOG_GET_BY_ID_SUCCESS, comCatalogueDTOY);
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
            logger.LogInformation(LOG_POST_START);
            try
            {
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().addCommercialCatalogue(comCatalogueDTO);
                if (createdComCatalogueDTO != null)
                {
                    logger.LogInformation(LOG_POST_SUCCESS, createdComCatalogueDTO);
                    return Created(Request.Path, createdComCatalogueDTO);
                }
                else
                {
                    logger.LogWarning(LOG_POST_BAD_REQUEST, comCatalogueDTO);
                    return BadRequest();
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException, LOG_POST_BAD_REQUEST, comCatalogueDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException, LOG_POST_BAD_REQUEST, comCatalogueDTO);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException, LOG_POST_BAD_REQUEST, comCatalogueDTO);
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
        [HttpPut("{id}/collections")]
        public ActionResult<CommercialCatalogueDTO> addCollection(long id, [FromBody]UpdateCommercialCatalogueDTO updateCatalogueCollectionDTO)
        {
            logger.LogInformation(LOG_PUT_START);
            try
            {
                updateCatalogueCollectionDTO.id = id;
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().updateCollection(updateCatalogueCollectionDTO);
                if (createdComCatalogueDTO != null)
                {
                    logger.LogInformation(LOG_PUT_SUCCESS,id,updateCatalogueCollectionDTO);
                    return Ok(new SimpleJSONMessageService(VALID_COMMERCIAL_CATALOGUE_UPDATE_MESSAGE));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (NullReferenceException nullReferenceException)
            {
                logger.LogWarning(nullReferenceException,LOG_PUT_BAD_REQUEST,id,updateCatalogueCollectionDTO);
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }
            catch (InvalidOperationException invalidOperationException)
            {
                logger.LogWarning(invalidOperationException,LOG_PUT_BAD_REQUEST,id,updateCatalogueCollectionDTO);
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            catch (ArgumentException argumentException)
            {
                logger.LogWarning(argumentException,LOG_PUT_BAD_REQUEST,id,updateCatalogueCollectionDTO);
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
        //!This method needs to be refactored because there are TWO PUT Requests with the same path
        [HttpPut("{id}/collections/")]
        public ActionResult<CommercialCatalogueDTO> removeCollection(long id, long idC)
        {
            try
            {
                CommercialCatalogueDTO createdComCatalogueDTO = new core.application.CommercialCatalogueController().removeCollection(id, idC);
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