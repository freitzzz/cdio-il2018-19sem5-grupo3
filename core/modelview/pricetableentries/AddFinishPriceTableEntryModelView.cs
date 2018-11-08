using System.Runtime.Serialization;

namespace core.modelview.pricetableentries
{
    [DataContract]
    public class AddFinishPriceTableEntryModelView : AddPriceTableEntryModelView
    {
        [DataMember(Name = "finishId")]
        public long finishId {get; set;}
    }
}