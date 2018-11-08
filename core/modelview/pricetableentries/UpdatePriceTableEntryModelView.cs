using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.pricetableentries
{
    [DataContract]
    public class UpdatePriceTableEntryModelView
    {
        [DataMember(Name = "tableEntryId")]
        public long tableEntryId { get; set; }

        [DataMember(Name = "entityId")]
        public long entityId { get; set; }

        [DataMember(Name = "tableEntry")]
        public PriceTableEntryDTO priceTableEntry { get; set; }
    }
}