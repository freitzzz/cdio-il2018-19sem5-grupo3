using System;
using System.Collections.Generic;
using backend.utils;
using core.application;
using core.domain;
using core.dto;
using core.exceptions;
using core.modelview.algorithm;
using core.modelview.input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using support.dto;
using support.utils;

namespace backend.Controllers
{
    /// <summary>
    /// Backend AlgorithmController class
    /// </summary>
    [Route("mycm/api/algorithms")]
    public class AlgorithmController : Controller
    {
        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START = "GET All Request started";
        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_ID_START = "GET by ID Request started";
        /// <summary>
        /// Constant that represents the log message for when a GET Inputs Request starts
        /// </summary>
        private const string LOG_GET_INPUTS_START = "GET Inputs Request started";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a NotFound
        /// </summary>
        private const string LOG_GET_ALL_NOT_FOUND = "GET All NotFound (No Algorithms Found)";
        /// <summary>
        /// Constant that represents the log message for when a GET by ID Request returns a NotFound
        /// </summary>
        private const string LOG_GET_ID_NOT_FOUND = "GET by ID NotFound (Not a valid algorithm)";
        /// <summary>
        /// Constant that represents the log message for when a GET by ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_INPUTS_BAD_REQUEST = "GET Inputs BadRequest (Not a valid algorithm)";
        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS = "Algorithms {@algorithms} retrieved";
        /// <summary>
        /// Constant that represents the log message for when a GET by ID Request is successful
        /// </summary>
        private const string LOG_GET_ID_SUCCESS = "Algorithm {@algorithm} retrieved";
        /// <summary>
        /// Constant that represents the log message for when a GET Inputs Request is successful
        /// </summary>
        private const string LOG_GET_INPUTS_SUCCESS = "Algorithm Inputs {@inputs} retrieved";
        /// <summary>
        /// Constant that represents the log message for when a GET Inputs Request is successful but the algorithm does not require any input
        /// </summary>
        private const string LOG_GET_INPUTS_SUCCESS_NO_INPUTS = "Algorithm does not need any input";
        /// <summary>
        /// AlgorithmController's logger
        /// </summary>
        private readonly ILogger<AlgorithmController> logger;
        /// <summary>
        /// Constructor with injected type of repository
        /// </summary>
        /// <param name="materialRepository">Repository to be used to manipulate Material instances</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public AlgorithmController(ILogger<AlgorithmController> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Finds all Algorithms.
        /// </summary>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the basic info of all Algorithms in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult findAll()
        {
            logger.LogInformation(LOG_GET_ALL_START);

            try
            {
                GetAllAlgorithmsModelView allAlgorithms = new core.application.AlgorithmController().getAllAlgorithms();

                logger.LogInformation(LOG_GET_ALL_SUCCESS, allAlgorithms);
                return Ok(allAlgorithms);
            }
            catch (ResourceNotFoundException ex)
            {
                logger.LogWarning(LOG_GET_ALL_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
        }

        /// <summary>
        /// Finds an Algorithm by its id
        /// </summary>
        /// <param name="id">id of the algorithm to retrieve</param>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the info of the algorithm
        /// </returns>
        [HttpGet("{id}", Name = "GetAlgorithm")]
        public ActionResult findAlgorithm(int id)
        {
            logger.LogInformation(LOG_GET_ID_START);
            try
            {
                GetAlgorithmModelView algorithmModelView = new core.application.AlgorithmController().getAlgorithm((RestrictionAlgorithm)id);

                logger.LogInformation(LOG_GET_ID_SUCCESS, algorithmModelView);
                return Ok(algorithmModelView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //this exception should only occur if the factory does not recognize an enumerate element with the given id value
                logger.LogWarning(LOG_GET_ID_NOT_FOUND);
                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
        }

        /// <summary>
        /// Retrieves a certain algorithm's inputs
        /// </summary>
        /// <param name="id">id of the algorithm to retrieve respective inputs</param>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the inputs the algorithm needs
        /// </returns>
        [HttpGet("{id}/inputs", Name = "GetAlgorithmRequiredInputs")]
        public ActionResult getAlgorithmRequiredInputs(int id)
        {
            logger.LogInformation(LOG_GET_INPUTS_START);
            try
            {
                GetAllInputsModelView requiredInputsModelView = new core.application.AlgorithmController().getAlgorithmRequiredInputs((RestrictionAlgorithm)id);

                logger.LogInformation(LOG_GET_INPUTS_SUCCESS, requiredInputsModelView);
                return Ok(requiredInputsModelView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //this exception should only occur if the factory does not recognize an enumerate element with the given id value
                logger.LogWarning(LOG_GET_INPUTS_BAD_REQUEST);
                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
            catch (ResourceNotFoundException ex)
            {
                //this exception should only occur if the algorithm has no required inputs

                return NotFound(new SimpleJSONMessageService(ex.Message));
            }
        }
    }
}