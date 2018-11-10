using System.Runtime.Serialization;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a Price Table Entry
    /// </summary>
    [DataContract]
    public class PriceTableEntryDTO
    {
        /// <summary>
        /// Table Entry's price
        /// </summary>
        [DataMember(Name = ("price"))]
        public PriceDTO price {get; set;}

        /// <summary>
        /// Table Entry's starting date
        /// </summary>
        [DataMember(Name = ("startingDate"))]
        public string startingDate {get; set;}

        /// <summary>
        /// Table Entry's ending date
        /// </summary>
        [DataMember(Name = ("endingDate"))]
        public string endingDate {get; set;}
    }
}