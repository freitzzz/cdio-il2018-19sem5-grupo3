using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    [DataContract]
    public class DeleteCustomizedProductCollectionModelView
    {
        [DataMember(Name = "customizedProductCollectionId")]
        public long customizedProductCollectionId { get; set; }
    }
}