using System.Runtime.Serialization;

namespace core.modelview.pricetableentries
{
    /// <summary>
    /// ModelView that represents the necessary information to update a finish's price table entry
    /// </summary>
    [DataContract]
    public class UpdateFinishPriceTableEntryModelView : BasicPriceTableEntryModelView
    {
        /// <summary>
        /// Finish's PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "finishId")]
        public long finishId { get; set; }
    }
}