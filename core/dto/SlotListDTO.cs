using System.Collections.Generic;
using System.Runtime.Serialization;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Class to help the deserialization of a CustomizedProduct's updates from JSON format
    /// </summary>
    [DataContract]
    public class SlotListDTO : DTO
    {
        /// <summary>
        /// List of SlotDTOs with the CustomizedProduct's Slots
        /// </summary>
        [DataMember(Name = "slots")]
        public List<SlotDTO> slotDTOs { get; set; }
    }
}