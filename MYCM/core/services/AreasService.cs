using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace core.services
{
    public static class AreasService
    {
        public static IEnumerable<string> getAvailableAreas()
        {
            return loadAreas();
        }

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