using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace core.services
{
    /// <summary>
    /// Service that helps with requests related to currencies only
    /// </summary>
    public static class CurrenciesService
    {
        /// <summary>
        /// Message that occurs if the new currency isn't supported
        /// </summary>
        private const string UNSUPPORTED_CURRENCY = "The inserted currency is not being supported at the moment!";

        /// <summary>
        /// Message to help the user know which currencies to use
        /// </summary>
        private const string SUPPORTED_CURRENCIES_MESSAGE = "Please use one of the currencies that are currently being supported";

        /// <summary>
        /// Fetches all available currencies
        /// </summary>
        /// <returns>IEnumerable with all available currencies</returns>
        public static IEnumerable<string> getAvailableCurrencies()
        {
            return loadCurrencies();
        }

        /// <summary>
        /// Checks if a given currency is currently supported
        /// </summary>
        /// <param name="currency">currency to check</param>
        public static void checkCurrencySupport(string currency)
        {
            List<string> availableCurrencies = (List<string>)loadCurrencies();
            if (availableCurrencies.Contains(currency))
            {
                throw new ArgumentException
                (
                    string.Format
                    (
                        "{0} {1}: {2}",
                        UNSUPPORTED_CURRENCY, SUPPORTED_CURRENCIES_MESSAGE, string.Join(", ", availableCurrencies)
                    )
                );
            }
        }

        /// <summary>
        /// Loads all available currencies from a JSON file
        /// </summary>
        /// <returns>IEnumerable with all available currencies</returns>
        private static IEnumerable<string> loadCurrencies()
        {
            IEnumerable<string> currencies = null;

            using (StreamReader file = File.OpenText(@"../core/currencies.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                currencies = (IEnumerable<string>)serializer.Deserialize(file, typeof(IEnumerable<string>));
            }

            return currencies;
        }
    }
}