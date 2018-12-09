using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a Model View for deleting a customized product from a customized product collection
    /// </summary>
    [DataContract]
    public class DeleteCustomizedProductFromCustomizedProductCollectionModelView
    {
        /// <summary>
        /// Customized Product Collection's PID
        /// </summary>
        [DataMember]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// Customized Product's PID
        /// </summary>
        [DataMember]
        public long customizedProductId { get; set; }
    }
}