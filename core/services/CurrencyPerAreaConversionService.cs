using System;
using System.Collections.Generic;
using System.IO;
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
        public static string getBaseCurrency(){
            loadDefaultCurrencyPerAreaUnitValues();
            return defaultCurrency;
        }

        /// <summary>
        /// Returns the current base area
        /// </summary>
        /// <returns>String with the current base area</returns>
        public static string getBaseArea(){
            loadDefaultCurrencyPerAreaUnitValues();
            return defaultArea;
        }

        /// <summary>
        /// Converts a value in a given currency to the default currency' value by fetching the exchange rate with the use of an external API
        /// </summary>
        /// <param name="convertFrom">currency that the value is originally in</param>
        /// <param name="valueToConvert">value to be converted</param>
        /// <returns>converted value in  euros</returns>
        public async Task<double> convertCurrencyToDefaultCurrency(string convertFrom, double valueToConvert)
        {

            HttpClient client = clientFactory.CreateClient("CurrencyConversion");

            String getRequest = String.Format(client.BaseAddress.OriginalString + "/currency?from={0}&to={1}", convertFrom, defaultCurrency);

            HttpResponseMessage response = await client.GetAsync(getRequest);

            if (response.IsSuccessStatusCode)
            {
                CurrencyConversionInfoDTO currencyConversionInfoDTO = JsonConvert.DeserializeObject<CurrencyConversionInfoDTO>(await response.Content.ReadAsStringAsync());

                return Math.Round(Double.Parse(currencyConversionInfoDTO.rate) * valueToConvert);
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
            if(defaultCurrency==null){
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
            /// <value></value>
            [DataMember(Name = "to")]
            public string toCurrency { get; set; }

            /// <summary>
            /// Exchange rate between two currencies
            /// </summary>
            /// <value></value>
            [DataMember(Name = "rate")]
            public string rate { get; set; }

            /// <summary>
            /// Currency that we are converting from
            /// </summary>
            /// <value></value>
            [DataMember(Name = "from")]
            public string fromCurrency { get; set; }
        }
    }
}