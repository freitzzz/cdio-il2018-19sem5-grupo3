using System;
using backend.utils;
using core.modelview.area;
using core.modelview.currency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    /// <summary>
    /// Currencies Per Area MVC 
    /// </summary>

    [Route("mycm/api/currenciesperarea")]
    public class CurrenciesPerAreaController : Controller
    {
        private const string GET_ALL_CURRENCIES_START = "GET ALL Request started";

        private const string GET_ALL_CURRENCIES_SUCCESS = "GET ALL Request retrieved {@modelView}";

        private const string UNEXPECTED_ERROR = "An unexpected error occured, please try again later";

        private readonly ILogger<CurrenciesPerAreaController> logger;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="logger"></param>
        public CurrenciesPerAreaController(ILogger<CurrenciesPerAreaController> logger)
        {
            this.logger = logger;
        }


        /// <summary>
        /// Fetches all available currencies
        /// </summary>
        /// <returns>ActionResult with HTTP 200 Ok and all available currencies
        ///     Or ActionResult with HTTP 500 if an error happens</returns>
        [HttpGet("currencies")]
        public ActionResult getAllCurrencies()
        {
            logger.LogInformation(GET_ALL_CURRENCIES_START);
            try
            {
                GetAllCurrenciesModelView modelView = new core.application.CurrenciesPerAreaController().getAllCurrencies();
                logger.LogInformation(GET_ALL_CURRENCIES_SUCCESS, modelView);
                return Ok(modelView);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, UNEXPECTED_ERROR);
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Fetches all available areas
        /// </summary>
        /// <returns>ActionResult with HTTP 200 Ok and all available areas
        ///     Or ActionResult with HTTP 500 if an error happens</returns>
        [HttpGet("areas")]
        public ActionResult getAllAreas()
        {
            try
            {
                GetAllAreasModelView modelView = new core.application.CurrenciesPerAreaController().getAllAreas();
                return Ok(modelView);
            }
            catch (Exception e)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        //TODO Convert Request
    }
}