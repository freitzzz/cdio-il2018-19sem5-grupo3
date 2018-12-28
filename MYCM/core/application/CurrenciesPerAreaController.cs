using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using core.modelview.area;
using core.modelview.currency;
using core.modelview.pricetable;
using core.modelview.price;
using core.services;
using System;

namespace core.application
{
    /// <summary>
    /// Application CurrenciesPerArea Controller
    /// </summary>
    public class CurrenciesPerAreaController
    {
        /// <summary>
        /// Message that occurs if the value to convert is invalid
        /// </summary>
        private const string INVALID_VALUE_TO_CONVERT = "The value to convert can't be negative nor infinity and has to be a number!";

        /// <summary>
        /// Message that occurs if the area to convert to and to convert from are the same
        /// </summary>
        private const string SAME_AREA = "The areas to convert to and to convert from are the same!";

        /// <summary>
        /// Message that occurs if the currency to convert to and to convert from are the same
        /// </summary>
        private const string SAME_CURRENCY = "The currencies to convert to and to convert from are the same!";

        /// <summary>
        /// Fetches all available currencies
        /// </summary>
        /// <returns>GetAllCurrenciesModelView containing all available currencies</returns>
        public GetAllCurrenciesModelView getAllCurrencies()
        {
            IEnumerable<string> availableCurrencies = CurrenciesService.getAvailableCurrencies();
            GetAllCurrenciesModelView allCurrenciesModelView = new GetAllCurrenciesModelView();

            foreach (string currency in availableCurrencies)
            {
                CurrencyModelView currencyModelView = new CurrencyModelView();
                currencyModelView.currency = currency;
                allCurrenciesModelView.Add(currencyModelView);
            }

            return allCurrenciesModelView;
        }

        /// <summary>
        /// Fetches all available areas
        /// </summary>
        /// <returns>GetAllAreasModelView with all available areas</returns>
        public GetAllAreasModelView getAllAreas()
        {
            IEnumerable<string> availableAreas = AreasService.getAvailableAreas();
            GetAllAreasModelView allAreasModelView = new GetAllAreasModelView();

            foreach (string area in availableAreas)
            {
                AreaModelView areaModelView = new AreaModelView();
                areaModelView.area = area;
                allAreasModelView.Add(areaModelView);
            }

            return allAreasModelView;
        }

        /// <summary>
        /// Converts a given price in a given currency per area to another currency per area
        /// </summary>
        /// <param name="convertPriceModelView">Model View with all the necessary conversion information</param>
        /// <param name="clientFactory">ClientFactory to create the HTTP Client to get currency conversion rates</param>
        /// <returns>ModelView with the converted price value</returns>
        public async Task<PriceModelView> convertPrice(ConvertPriceModelView convertPriceModelView, IHttpClientFactory clientFactory)
        {
            PriceModelView convertedPrice = new PriceModelView();

            if (Double.IsNaN(convertPriceModelView.value) || Double.IsInfinity(convertPriceModelView.value) || Double.IsNegative(convertPriceModelView.value))
            {
                throw new ArgumentException(INVALID_VALUE_TO_CONVERT);
            }

            if (convertPriceModelView.fromCurrency.Equals(convertPriceModelView.toCurrency))
            {
                throw new ArgumentException(SAME_CURRENCY);
            }

            if (convertPriceModelView.fromArea.Equals(convertPriceModelView.toArea))
            {
                throw new ArgumentException(SAME_AREA);
            }

            convertedPrice.value =
                await new CurrencyPerAreaConversionService(clientFactory)
                    .convertCurrencyPerArea(convertPriceModelView.value, convertPriceModelView.fromCurrency, convertPriceModelView.toCurrency, convertPriceModelView.fromArea, convertPriceModelView.toArea);

            convertedPrice.currency = convertPriceModelView.toCurrency;
            convertedPrice.area = convertPriceModelView.toArea;

            return convertedPrice;
        }
    }
}