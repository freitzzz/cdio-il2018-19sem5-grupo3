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
        public CustomizedDimensionsDTO minimumSlotDimensions;
        /// <summary>
        /// Slots Maximum Dimensions
        /// </summary>
        [DataMember(Name = "maxSize")]
        public CustomizedDimensionsDTO maximumSlotDimensions;
        /// <summary>
        /// Slots Recommended Dimensions
        /// </summary>
        [DataMember(Name = "recommendedSize")]
        public CustomizedDimensionsDTO recommendedSlotDimensions;

    }
}