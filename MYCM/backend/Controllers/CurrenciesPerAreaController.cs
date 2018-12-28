using System;
using System.Net.Http;
using System.Threading.Tasks;
using backend.utils;
using core.modelview.area;
using core.modelview.currency;
using core.modelview.price;
using core.modelview.pricetable;
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
        private const string UNEXPECTED_ERROR = "An unexpected error occured, please try again later";

        /// <summary>
        /// Injected client factory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="logger"></param>
        public CurrenciesPerAreaController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }


        /// <summary>
        /// Fetches all available currencies
        /// </summary>
        /// <returns>ActionResult with HTTP 200 Ok and all available currencies
        ///     Or ActionResult with HTTP 500 if an error happens</returns>
        [HttpGet("currencies")]
        public ActionResult getAllCurrencies()
        {
            try
            {
                GetAllCurrenciesModelView modelView = new core.application.CurrenciesPerAreaController().getAllCurrencies();
                return Ok(modelView);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Converts a given price
        /// </summary>
        /// <param name="fromCurrency">Query parameter to know which currency to convert from</param>
        /// <param name="toCurrency">Query parameter to know which currency to convert to</param>
        /// <param name="fromArea">Query parameter to know which area to convert from</param>
        /// <param name="toArea">Query parameter to know which area to convert to</param>
        /// <param name="value">Query parameter to know the value to convert</param>
        /// <returns>Action Result with HTTP Code 200 with the converted prrice
        ///         Or Action Result with HTTP Code 400 if any currency or area aren't supported
        ///         Or Action Result with HTTP Code 500 if an unexpected error happens</returns>
        [HttpGet("convert")]
        public async Task<ActionResult> convertPrice([FromQuery] string fromCurrency, [FromQuery] string toCurrency, [FromQuery] string fromArea, [FromQuery] string toArea, [FromQuery] double value)
        {
            try
            {
                ConvertPriceModelView convertPriceModelView = new ConvertPriceModelView();
                convertPriceModelView.fromCurrency = fromCurrency;
                convertPriceModelView.toCurrency = toCurrency;
                convertPriceModelView.fromArea = fromArea;
                convertPriceModelView.toArea = toArea;
                convertPriceModelView.value = value;
                PriceModelView convertedPrice = await new core.application.CurrenciesPerAreaController().convertPrice(convertPriceModelView, clientFactory);
                return Ok(convertedPrice);
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