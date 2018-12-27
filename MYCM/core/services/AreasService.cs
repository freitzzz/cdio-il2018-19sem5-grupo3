using System;
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
        /// Message that occurs if the new area isn't supported
        /// </summary>
        private const string UNSUPPORTED_AREA = "The inserted area is not being supported at the moment!";

        /// <summary>
        /// Message to help the user know which areas to use
        /// </summary>
        private const string SUPPORTED_AREAS_MESSAGE = "Please use one of the areas that are currently being supported";

        /// <summary>
        /// Fetches all available areas
        /// </summary>
        /// <returns>IEnumerable with all available areas</returns>
        public static IEnumerable<string> getAvailableAreas()
        {
            return loadAreas();
        }

        /// <summary>
        /// Checks if a given area is currently supported
        /// </summary>
        /// <param name="area">currency to check</param>
        public static void checkAreaSupport(string area)
        {
            List<string> availableAreas = (List<string>)loadAreas();
            if (availableAreas.Contains(area))
            {
                throw new ArgumentException
                (
                    string.Format
                    (
                        "{0} {1}: {2}",
                        UNSUPPORTED_AREA, SUPPORTED_AREAS_MESSAGE, string.Join(", ", availableAreas)
                    )
                );
            }
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