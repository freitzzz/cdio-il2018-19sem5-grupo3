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
        /// Finds all Algorithms.
        /// </summary>
        /// <returns>
        /// <br>HTTP Response 200 Ok with the basic info of all Algorithms in JSON format.
        /// </returns>
        [HttpGet]
        public ActionResult findAll()
        {
            try
            {
                GetAllAlgorithmsModelView allAlgorithms = new core.application.AlgorithmController().getAllAlgorithms();

                return Ok(allAlgorithms);
            }
            catch (ResourceNotFoundException ex)
            {
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
            try
            {
                GetAlgorithmModelView algorithmModelView = new core.application.AlgorithmController().getAlgorithm((RestrictionAlgorithm)id);

                return Ok(algorithmModelView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //this exception should only occur if the factory does not recognize an enumerate element with the given id value
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
            try
            {
                GetAllInputsModelView requiredInputsModelView = new core.application.AlgorithmController().getAlgorithmRequiredInputs((RestrictionAlgorithm)id);

                return Ok(requiredInputsModelView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //this exception should only occur if the factory does not recognize an enumerate element with the given id value
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