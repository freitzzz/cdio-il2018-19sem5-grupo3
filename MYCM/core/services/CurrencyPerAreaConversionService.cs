using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace core.services
{
    /// <summary>
    /// Service to convert currencies and areas to default values set in a configuration file
    /// </summary>
    public class CurrencyPerAreaConversionService
    {
        /// <summary>
        /// Message that occurs if the currency conversion fails
        /// </summary>
        private const string FAILED_TO_CONVERT = "Failed to convert currency";

        /// <summary>
        /// Represents the key of the json file that matches the current currency being persisted in the database
        /// </summary>
        private const string CURRENCY_KEY = "currency";

        /// <summary>
        /// Represents the key of the json file that matches the current area unit being persisted in the database
        /// </summary>
        private const string AREA_KEY = "area";

        /// <summary>
        /// Represents a unit
        /// </summary>
        private const double UNIT = 1.0;

        /// <summary>
        /// Represents the power of two
        /// </summary>
        private const double SQUARE = 2.0;

        /// <summary>
        /// Number of decimal places that a price always has
        /// </summary>
        private const int DECIMAL_PLACES = 10;

        /// <summary>
        /// Represents the error message that occurs if an error happens while parsing the JSON file that holds the default area unit
        /// </summary>
        private const string PARSE_FILE_AREA_ERROR = "Error happened when trying to parse the configuration file for the default area";

        private const string PARSE_FILE_CURRENCY_ERROR = "Error happened when trying to parse the configuration file for the default currency";

        /// <summary>
        /// Injected HTTPClientFactory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Currency being persisted in the database
        /// </summary>
        /// <value>Gets/Sets the base currency</value>
        private static string defaultCurrency { get; set; }

        /// <summary>
        /// Current area unit being persisted in the database
        /// </summary>
        /// <value>Gets/Sets the area unit</value>
        private static string defaultArea { get; set; }

        /// <summary>
        /// Builds an instance of the service with the injected HTTPClientFactory
        /// </summary>
        /// <param name="clientFactory">injected HTTPClientFactory</param>
        public CurrencyPerAreaConversionService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            loadDefaultCurrencyPerAreaUnitValues();
        }

        /// <summary>
        /// Returns the current base currency
        /// </summary>
        /// <returns>String with the current base currency</returns>
        public static string getBaseCurrency()
        {
            loadDefaultCurrencyPerAreaUnitValues();
            return defaultCurrency;
        }

        /// <summary>
        /// Returns the current base area
        /// </summary>
        /// <returns>String with the current base area</returns>
        public static string getBaseArea()
        {
            loadDefaultCurrencyPerAreaUnitValues();
            return defaultArea;
        }

        /**
            *   To convert prices (value that's in currency per unit of area)
            *   we need to use the following formula:
            *  =======================================
            *          !Cp = Op * CR * 1/F²
            *  =======================================
            *   Where Cp is the converted price,
            *         Op is the original price,
            *         CR is the conversion rate between both currencies and
            *         F is the conversion factor between both measurement units
            *
            *   Example:
            *           Op = 100 €/m²
            *           Suppose we want to convert this to USD/in²
            *           Let's admit that CR = 1.14
            *           We know that 1 in = 0.0254 m
            *           So F = 0.0254
            *           Therefore we have
            *           Cp = 100 (€) * 1.14 ($/€) * (1/m² * (0.0254 m / in)²)
            *           Cp = 100 * 1.14 * 0.0254²
            *           Cp = 0.0735 $/in²
        */

        /// <summary>
        /// Converts a value in a given currency to the default currency's value by fetching the exchange rate with the use of an external API
        /// </summary>
        /// <param name="convertFromCurrency">currency that the value is originally in</param>
        /// <param name="valueToConvert">value to be converted</param>
        /// <returns>price converted to default currency per area</returns>
        public async Task<double> convertCurrencyPerAreaToDefaultCurrencyPerArea(string convertFromCurrency, string convertFromArea, double valueToConvert)
        {
            HttpClient client = clientFactory.CreateClient("CurrencyConversion");

            String getRequest = String.Format(client.BaseAddress.OriginalString + "/currency?from={0}&to={1}", convertFromCurrency, defaultCurrency);

            HttpResponseMessage response = await client.GetAsync(getRequest);

            if (response.IsSuccessStatusCode)
            {
                CurrencyConversionInfoDTO currencyConversionInfoDTO = JsonConvert.DeserializeObject<CurrencyConversionInfoDTO>(await response.Content.ReadAsStringAsync());

                double minimumMeasurementUnitValue =
                    MeasurementUnitService.convertFromUnit
                        (UNIT,
                            new String(convertFromArea.Where(c => Char.IsLetter(c)).ToArray()));

                double areaConversionFactor =
                    Math.Pow(
                        MeasurementUnitService.convertToUnit(
                                                                minimumMeasurementUnitValue,
                                                                new String(defaultArea.Where(c => Char.IsLetter(c)).ToArray())
                                                            ), SQUARE);

                double convertedValue = currencyConversionInfoDTO.rate * valueToConvert * 1 / areaConversionFactor;
                return Math.Round(convertedValue, DECIMAL_PLACES);
            }
            else
            {
                throw new HttpRequestException(FAILED_TO_CONVERT);
            }
        }

        /// <summary>
        /// Converts a value that's in default currency per default area to a requested currency per area
        /// </summary>
        /// <param name="valueToConvert">value in default currency</param>
        /// <param name="convertToCurrency">currency to convert the value to</param>
        /// <param name="convertToArea">area to convert the value to</param>
        /// <returns>price converted to specific currency per area</returns>
        public async Task<double> convertDefaultCurrencyPerAreaToCurrencyPerArea(double valueToConvert, string convertToCurrency, string convertToArea)
        {
            HttpClient client = clientFactory.CreateClient("CurrencyConversion");

            String getRequest = String.Format(client.BaseAddress.OriginalString + "/currency?from={0}&to={1}", defaultCurrency, convertToCurrency);

            HttpResponseMessage response = await client.GetAsync(getRequest);

            if (response.IsSuccessStatusCode)
            {
                CurrencyConversionInfoDTO currencyConversionInfoDTO = JsonConvert.DeserializeObject<CurrencyConversionInfoDTO>(await response.Content.ReadAsStringAsync());

                double minimumMeasurementUnitValue =
                    MeasurementUnitService.convertFromUnit
                        (UNIT,
                            new String(defaultArea.Where(c => Char.IsLetter(c)).ToArray()));

                double areaConversionFactor =
                    Math.Pow(
                        MeasurementUnitService.convertToUnit(
                                                                minimumMeasurementUnitValue,
                                                                new String(convertToArea.Where(c => Char.IsLetter(c)).ToArray())
                                                            ), SQUARE);

                double convertedValue = currencyConversionInfoDTO.rate * valueToConvert * 1 / areaConversionFactor;
                return Math.Round(convertedValue, DECIMAL_PLACES);
            }
            else
            {
                throw new HttpRequestException(FAILED_TO_CONVERT);
            }
        }

        /// <summary>
        /// Converts a given value in a given currency per area to another currency per area
        /// </summary>
        /// <param name="valueToConvert">Value to convert</param>
        /// <param name="fromCurrency">Currency to convert from</param>
        /// <param name="toCurrency">Currency to convert to</param>
        /// <param name="fromArea">Area to convert from</param>
        /// <param name="toArea">Area to convert to</param>
        /// <returns>Converted value to requested currency per area</returns>
        public async Task<double> convertCurrencyPerArea(double valueToConvert, string fromCurrency, string toCurrency, string fromArea, string toArea)
        {
            HttpClient client = clientFactory.CreateClient("CurrencyConversion");

            String getRequest = String.Format(client.BaseAddress.OriginalString + "/currency?from={0}&to={1}", fromCurrency, toCurrency);

            HttpResponseMessage response = await client.GetAsync(getRequest);

            if (response.IsSuccessStatusCode)
            {
                CurrencyConversionInfoDTO currencyConversionInfoDTO = JsonConvert.DeserializeObject<CurrencyConversionInfoDTO>(await response.Content.ReadAsStringAsync());

                double minimumMeasurementUnitValue =
                    MeasurementUnitService.convertFromUnit
                        (UNIT,
                            new String(fromArea.Where(c => Char.IsLetter(c)).ToArray()));

                double areaConversionFactor =
                    Math.Pow(
                        MeasurementUnitService.convertToUnit(
                                                                minimumMeasurementUnitValue,
                                                                new String(toArea.Where(c => Char.IsLetter(c)).ToArray())
                                                            ), SQUARE);

                double convertedValue = currencyConversionInfoDTO.rate * valueToConvert * 1 / areaConversionFactor;
                return Math.Round(convertedValue, DECIMAL_PLACES);
            }
            else
            {
                throw new HttpRequestException(FAILED_TO_CONVERT);
            }
        }

        /// <summary>
        /// Loads default currency and area unit being used from a JSON file
        /// </summary>
        private static void loadDefaultCurrencyPerAreaUnitValues()
        {
            if (defaultCurrency == null)
            {
                Dictionary<string, string> pricePerAreaDictionary = null;

                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(@"../core/current_price_per_area_units.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    pricePerAreaDictionary = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
                }

                String currency;

                if (!pricePerAreaDictionary.TryGetValue(CURRENCY_KEY, out currency))
                {
                    throw new InvalidOperationException(PARSE_FILE_CURRENCY_ERROR);
                }
                defaultCurrency = currency;

                String area;
                if (!pricePerAreaDictionary.TryGetValue(AREA_KEY, out area))
                {
                    throw new InvalidOperationException(PARSE_FILE_AREA_ERROR);
                }
                defaultArea = area;
            }
        }

        /// <summary>
        /// DTO that represents the information that is fetched from the external API
        /// </summary>
        [DataContract]
        private class CurrencyConversionInfoDTO
        {
            /// <summary>
            /// Currency that we are converting to
            /// </summary>
            /// <value>Gets/Sets the currency</value>
            [DataMember(Name = "to")]
            public string toCurrency { get; set; }

            /// <summary>
            /// Exchange rate between two currencies
            /// </summary>
            /// <value>Gets/Sets the rate</value>
            [DataMember(Name = "rate")]
            public double rate { get; set; }

            /// <summary>
            /// Currency that we are converting from
            /// </summary>
            /// <value>Gets/Sets the currency</value>
            [DataMember(Name = "from")]
            public string fromCurrency { get; set; }
        }
    }
}