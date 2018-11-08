using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.pricetableentries
{
    /// <summary>
    /// ModelView to represent the necessary information to add a new generic price table entry
    /// </summary>
    [DataContract]
    public class AddPriceTableEntryModelView
    {
        [DataMember(Name = "entityId")]
        public long entityId { get; set; }

        [DataMember(Name = "tableEntry")]
        public PriceTableEntryDTO priceTableEntry { get; set; }
    }
}