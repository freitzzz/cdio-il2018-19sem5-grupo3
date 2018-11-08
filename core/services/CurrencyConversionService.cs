using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace core.services
{
    /// <summary>
    /// Service to convert currencies to corresponding euro value
    /// </summary>
    public class CurrencyConversionService
    {
        /// <summary>
        /// Injected HTTPClientFactory
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Message that occurs if the currency conversion fails
        /// </summary>
        private string FAILED_TO_CONVERT = "Failed to convert currency";

        /// <summary>
        /// Builds an instance of the service with the injected HTTPClientFactory
        /// </summary>
        /// <param name="clientFactory">injected HTTPClientFactory</param>
        public CurrencyConversionService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Converts a value in a given currency to it's corresponding EURO value by fetching the exchange rate with the use of an external API
        /// </summary>
        /// <param name="convertFrom">currency that the value is originally in</param>
        /// <param name="valueToConvert">value to be converted</param>
        /// <returns>converted value in  euros</returns>
        public async Task<double> convertCurrencyToEuro(string convertFrom, double valueToConvert)
        {

            HttpClient client = clientFactory.CreateClient("CurrencyConversion");

            String getRequest = String.Format(client.BaseAddress.OriginalString + "/currency?from={0}&to=EUR", convertFrom);

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