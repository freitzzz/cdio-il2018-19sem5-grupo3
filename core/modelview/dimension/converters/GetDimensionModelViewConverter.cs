using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace core.modelview.dimension.converters
{
    /// <summary>
    /// Converts a AddDimensionModelView into a concrete type.
    /// Part of the solution presented <a href = https://stackoverflow.com/a/30579193>here</a>
    /// </summary>
    public class GetDimensionModelViewConverter : JsonConverter
    {
        static JsonSerializerSettings subclassConversion = new JsonSerializerSettings() { ContractResolver = new GetDimensionModelViewContractResolver() };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AddDimensionModelView);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            if (jo.GetValue("value", StringComparison.InvariantCultureIgnoreCase) != null)    //if the object contains the field value, then it's a SingleValueDimensionDTO
            {
                return JsonConvert.DeserializeObject<GetSingleValueDimensionModelView>(jo.ToString(), subclassConversion);
            }
            else if (jo.GetValue("values", StringComparison.InvariantCultureIgnoreCase) != null)  //if the object contains the field values, then it's DiscreteDimensionIntervalDTO
            {
                return JsonConvert.DeserializeObject<GetDiscreteDimensionIntervalModelView>(jo.ToString(), subclassConversion);
            }
            //if the the object contains the fields minvalue, maxvalue and increment, then it's ContinuousDimensionInterval
            else if (jo.GetValue("minValue", StringComparison.InvariantCultureIgnoreCase) != null &&
                jo.GetValue("maxValue", StringComparison.InvariantCultureIgnoreCase) != null &&
                jo.GetValue("increment", StringComparison.InvariantCultureIgnoreCase) != null)
            {
                return JsonConvert.DeserializeObject<GetContinuousDimensionIntervalModelView>(jo.ToString(), subclassConversion);
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //exception is thrown since CanWrite returns false
            throw new NotImplementedException();
        }
    }
}