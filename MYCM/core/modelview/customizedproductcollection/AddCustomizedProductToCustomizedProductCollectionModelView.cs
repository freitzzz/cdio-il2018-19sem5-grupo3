using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing the Model View for adding a Customized Product to a Customized Product Collection
    /// </summary>
    [DataContract]
    public class AddCustomizedProductToCustomizedProductCollectionModelView
    {
        /// <summary>
        /// Customized Product Collection's PID
        /// </summary>
        [DataMember]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// Customized Product's PID
        /// </summary>
        /// <value></value>
        [DataMember]
        public long customizedProductId { get; set; }
    }
}