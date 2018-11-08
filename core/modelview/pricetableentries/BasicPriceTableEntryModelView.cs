using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.pricetableentries
{
    /// <summary>
    /// ModelView that represents a basic price table entry
    /// </summary>
    [DataContract]
    public class BasicPriceTableEntryModelView
    {
        /// <summary>
        /// Table Entry's PID
        /// </summary>
        /// <value>Gets/Sets the idenfier</value>
        [DataMember(Name = "tableEntryId")]
        public long tableEntryId { get; set; }

        /// <summary>
        /// Entity's PID
        /// </summary>
        /// <value>Gets/Sets the idenfier</value>
        [DataMember(Name = "entityId")]
        public long entityId { get; set; }

        /// <summary>
        /// Table Entry's DTO
        /// </summary>
        /// <value>Gets/Sets DTO</value>
        [DataMember(Name = "tableEntry")]
        public PriceTableEntryDTO priceTableEntry { get; set; }
    }
}