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
        /// Retrieves all the available units.
        /// </summary>
        /// <returns>ActionResult with the 200 HTTP Code and all the available units in JSON format.</returns>
        [HttpGet]
        public ActionResult getAllUnits()
        {
            try
            {
                GetUnitsModelView unitsMV = new core.application.UnitConversionController().getAllAvailableUnits();
                return Ok(unitsMV);
            }
            catch (Exception)
            {
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
            ConvertUnitModelView convertMV = new ConvertUnitModelView();
            convertMV.toUnit = to;
            convertMV.fromUnit = from;
            convertMV.value = value;

            try
            {
                ConvertUnitModelView convertedUnitMV = new core.application.UnitConversionController().convertValue(convertMV);
                return Ok(convertedUnitMV);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}