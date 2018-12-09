using System.Runtime.Serialization;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing a Model View for updating the basic info of a customized product collection
    /// </summary>
    [DataContract]
    public class UpdateCustomizedProductCollectionModelView
    {

        /// <summary>
        /// Customized Product Collection's PID
        /// </summary>
        /// <value></value>
        [IgnoreDataMember]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// Customized Product Collection's updated name
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }
    }
}