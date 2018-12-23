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
        /// Fetches all available currencies
        /// </summary>
        /// <returns>IEnumerable with all available currencies</returns>
        public static IEnumerable<string> getAvailableCurrencies()
        {
            return loadCurrencies();
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