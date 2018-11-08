using System.Runtime.Serialization;

namespace core.modelview.pricetableentries
{
    /// <summary>
    /// ModelView that represents the information for creating a finish price table entry
    /// </summary>
    [DataContract]
    public class AddFinishPriceTableEntryModelView : BasicPriceTableEntryModelView
    {
        /// <summary>
        /// Finish's PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "finishId")]
        public long finishId {get; set;}
    }
}