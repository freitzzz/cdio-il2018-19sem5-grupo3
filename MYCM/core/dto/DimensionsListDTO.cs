using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto
{
    /// <summary>
    /// ProductObject class to help the deserialization of a product's updates from JSON format
    /// </summary>
    [DataContract]
    public class DimensionsListDTO
    {
        [DataMember(Name = "heightDimensions")]
        public List<DimensionDTO> heightDimensionDTOs { get; set; }
        [DataMember(Name = "widthDimensions")]
        public List<DimensionDTO> widthDimensionDTOs { get; set; }
        [DataMember(Name = "depthDimensions")]
        public List<DimensionDTO> depthDimensionDTOs { get; set; }
    }
}