using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    [DataContract]
    public class DeleteCustomizedProductCollectionModelView
    {
        [DataMember]
        public long customizedProductCollectionId { get; set; }
    }
}