using System;
using backend.utils;
using core.modelview.unitconversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using support.utils;

namespace backend.Controllers
{
    [Route("mycm/api/units")]
    public class UnitConversionController : Controller
    {
        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred. Please try again.";

        /// <summary>
        /// Constant that represents the log message for when a GET All Units Request starts.
        /// </summary>
        private const string LOG_GET_ALL_UNITS_START = "GET All Units started.";
        /// <summary>
        /// Constant that represents the log message for when a GET All Unit Request is successful.
        /// </summary>
        private const string LOG_GET_ALL_UNITS_SUCCESS = "Units {@units} retrieved.";

        /// <summary>
        /// Constant that represents the log message for when a GET Convert Value Request starts.
        /// </summary>
        private const string LOG_CONVERT_VALUE_START = "GET Convert Value started.";
        /// <summary>
        /// Constant that represents the log message for when a GET Convert Value Request returns BadRequest.
        /// </summary>
        private const string LOG_CONVERT_VALUE_BAD_REQUEST = "GET Convert Value {@conversionData}- Bad Request";
        /// <summary>
        /// Constant that represents the log message for when a GET Convert Value Request is succesful
        /// </summary>
        private const string LOG_CONVERT_CONVERT_SUCCESS = "Conversion Data {@conversionData} retrieved.";

        /// <summary>
        /// UnitConversionController's logger.
        /// </summary>
        private readonly ILogger<UnitConversionController> logger;

        /// <summary>
        /// Creates a new instance of UnitConversionController.
        /// </summary>
        /// <param name="logger">ILogger being injected.</param>
        public UnitConversionController(ILogger<UnitConversionController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all the available units.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP Code and all the available units in JSON format.</returns>
        [HttpGet]
        public ActionResult getAllUnits()
        {
            logger.LogInformation(LOG_GET_ALL_UNITS_START);
            try
            {
                GetUnitsModelView unitsMV = new core.application.UnitConversionController().getAllAvailableUnits();
                logger.LogInformation(LOG_GET_ALL_UNITS_SUCCESS, unitsMV);
                return Ok(unitsMV);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Converts the value from a unit to another unit.
        /// </summary>
        /// <param name="to">Unit to which the value will be converted.</param>
        /// <param name="from">Unit from which the value will be converted.</param>
        /// <param name="value">Value being converted.</param>
        /// <returns>ActionResult with the 200HTTP Code and the conversion data in JSON format.</returns>
        [HttpGet("convert")]
        public ActionResult convertValue([FromQuery]string to, [FromQuery] string from, [FromQuery]double value)
        {
            logger.LogInformation(LOG_CONVERT_VALUE_START);

            ConvertUnitModelView convertMV = new ConvertUnitModelView();
            convertMV.toUnit = to;
            convertMV.fromUnit = from;
            convertMV.value = value;

            try
            {
                ConvertUnitModelView convertedUnitMV = new core.application.UnitConversionController().convertValue(convertMV);
                logger.LogInformation(LOG_CONVERT_CONVERT_SUCCESS, convertedUnitMV);
                return Ok(convertedUnitMV);
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(LOG_CONVERT_VALUE_BAD_REQUEST, convertMV);
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}