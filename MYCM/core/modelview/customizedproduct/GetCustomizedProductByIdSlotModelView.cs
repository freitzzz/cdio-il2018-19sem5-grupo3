using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;
using core.modelview.customizeddimensions;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Model View that represents a slot for GET By Id request
    /// </summary>
    [DataContract]
    public class GetCustomizedProductByIdSlotModelView
    {
        /// <summary>
        /// Slots PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "id")]
        public long id { get; set; }

        /// <summary>
        /// Slots customized dimensions
        /// </summary>
        /// <value>Gets/Sets the dimensions</value>
        [DataMember(Name = "slotDimensions")]
        public GetCustomizedDimensionsModelView slotDimensions { get; set; }

        /// <summary>
        /// List of customized products the slot contains
        /// </summary>
        /// <value>Gets/Sets the list</value>
        [DataMember(Name = "customizedProducts")]
        public List<BasicCustomizedProductModelView> customizedProducts { get; set; }
    }
}