using System;
using System.Collections.Generic;
using backend.utils;
using core.application;
using core.domain;
using core.dto;
using core.modelview.algorithm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using support.dto;
using support.utils;

namespace backend.Controllers {
    /// <summary>
    /// Backend AlgorithmController class
    /// </summary>
    public class AlgorithmController : Controller {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no Algorithms are found.
        /// </summary>
        private const string NO_ALGORITHMS_FOUND = "No algorithms found";
        /// <summary>
        /// Constant that represents the 200 Ok message for when no inputs are required by the Algorithm
        /// </summary>
        private const string NO_INPUTS_NEEDED_MESSAGE = "Algorithm does not require any inputs";
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
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST = "GET All BadRequest (No Algorithms Found)";
        /// <summary>
        /// Constant that represents the log message for when a GET by ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ID_BAD_REQUEST = "GET by ID BadRequest (Not a valid algorithm)";
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
        public AlgorithmController(ILogger<AlgorithmController> logger) {
            this.logger = logger;
        }
        /// <summary>
        /// Finds all Algorithms.
        /// </summary>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the basic info of all Algorithms in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult findAll() {
            logger.LogInformation(LOG_GET_ALL_START);
            List<GetBasicAlgorithmModelView> algorithms = new core.application.AlgorithmController().getAllAlgorithms();
            if (Collections.isListEmpty(algorithms)) {
                logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
                return BadRequest(new SimpleJSONMessageService(NO_ALGORITHMS_FOUND));
            }
            logger.LogInformation(LOG_GET_ALL_SUCCESS, algorithms);
            return Ok(algorithms);
        }
        /// <summary>
        /// Finds an Algorithm by its id
        /// </summary>
        /// <param name="id">id of the algorithm to retrieve</param>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the info of the algorithm
        /// </returns>
        [HttpGet("{id}", Name = "GetAlgorithm")]
        public ActionResult findAlgorithm(int id) {
            logger.LogInformation(LOG_GET_ID_START);
            try {
                AlgorithmDTO algDTO = new core.application.AlgorithmController().getAlgorithm((RestrictionAlgorithm)id);
                logger.LogInformation(LOG_GET_ID_SUCCESS, algDTO);
                return Ok(algDTO);
            } catch (ArgumentOutOfRangeException ex) {
                logger.LogWarning(LOG_GET_ID_BAD_REQUEST);
                return BadRequest(new SimpleJSONMessageService(ex.Message));
            }
        }
        /// <summary>
        /// Retrieves a certain algorithm's inputs
        /// </summary>
        /// <param name="id">id of the algorithm to retrieve respective inputs</param>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the inputs the algorithm needs
        /// </returns>
        [HttpGet("{id}", Name = "GetAlgorithmInputs")]
        public ActionResult getAlgorithmInputs(int id) {
            logger.LogInformation(LOG_GET_INPUTS_START);
            try {
                List<InputDTO> inputs = new core.application.AlgorithmController().getAlgorithmInputs((RestrictionAlgorithm)id);
                if (Collections.isListEmpty(inputs)) {
                    logger.LogInformation(LOG_GET_INPUTS_SUCCESS_NO_INPUTS);
                    return Ok(new SimpleJSONMessageService(NO_INPUTS_NEEDED_MESSAGE));
                }
                logger.LogInformation(LOG_GET_INPUTS_SUCCESS, inputs);
                return Ok(inputs);
            } catch (ArgumentOutOfRangeException ex) {
                logger.LogWarning(LOG_GET_INPUTS_BAD_REQUEST);
                return BadRequest(new SimpleJSONMessageService(ex.Message));
            }
        }
    }
}