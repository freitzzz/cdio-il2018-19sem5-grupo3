using System;
using System.Runtime.Serialization;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// DTO representation of a Slots Dimension Set (Minimum Dimensions, Maximum Dimensions, Recommended Dimensions)
    /// </summary>
    [DataContract]
    public class SlotDimensionSetDTO : DTO
    {
        /// <summary>
        /// Slots Minimum Dimensions
        /// </summary>
        [DataMember(Name = "minSize")]
        public CustomizedDimensionsDTO minimumSlotDimensions { get; set; }
        /// <summary>
        /// Slots Maximum Dimensions
        /// </summary>
        [DataMember(Name = "maxSize")]
        public CustomizedDimensionsDTO maximumSlotDimensions { get; set; }
        /// <summary>
        /// Slots Recommended Dimensions
        /// </summary>
        [DataMember(Name = "recommendedSize")]
        public CustomizedDimensionsDTO recommendedSlotDimensions { get; set; }

    }
}