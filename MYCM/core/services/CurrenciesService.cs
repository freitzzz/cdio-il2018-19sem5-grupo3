using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace core.services
{
    public static class CurrenciesService
    {
        public static IEnumerable<string> getAvailableCurrencies()
        {
            return loadCurrencies();
        }

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