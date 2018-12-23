using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace core.services
{
    /// <summary>
    /// Service to help with requests related to areas only
    /// </summary>
    public static class AreasService
    {
        /// <summary>
        /// Fetches all available areas
        /// </summary>
        /// <returns>IEnumerable with all available areas</returns>
        public static IEnumerable<string> getAvailableAreas()
        {
            return loadAreas();
        }

        /// <summary>
        /// Loads all available areas from a JSON file
        /// </summary>
        /// <returns>IEnumerable with all available areas</returns>
        private static IEnumerable<string> loadAreas()
        {
            IEnumerable<string> areas = null;

            using (StreamReader file = File.OpenText(@"../core/areas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                areas = (IEnumerable<string>)serializer.Deserialize(file, typeof(IEnumerable<string>));
            }

            return areas;
        }
    }
}