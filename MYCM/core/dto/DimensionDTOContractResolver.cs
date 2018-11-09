using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace core.dto
{
    /// <summary>
    /// Resolves a DimensionDTO's contract for each of its concrete classes.
    /// Part of the solution presented <a href = https://stackoverflow.com/a/30579193>here</a>
    /// </summary>
    public class DimensionDTOContractResolver : DefaultContractResolver
    {

        protected override JsonConverter ResolveContractConverter(System.Type objectType)
        {
            if (typeof(DimensionDTO).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            {
                return null;
            }
            return base.ResolveContractConverter(objectType);
        }

    }
}