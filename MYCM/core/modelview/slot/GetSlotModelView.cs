using System.Runtime.Serialization;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct;

namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of Slot.
    /// </summary>
    [DataContract]
    public class GetSlotModelView
    {
        /// <summary>
        /// Slot's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Slot's persistence identifier.</value>
        [DataMember(Name = "id")]
        public long slotId { get; set; }

        /// <summary>
        /// GetCustomizedDimensionsModelView detailing the Slot's dimensions.
        /// </summary>
        /// <value>Gets/Sets the Slot's GetCustomizedDimensionsModelView.</value>
        [DataMember(Name = "dimensions")]
        public GetCustomizedDimensionsModelView slotDimensions { get; set; }

        /// <summary>
        /// GetAllCustomizedDimensionsModelView detailing the CustomizedProducts inserted in the Slot.
        /// </summary>
        /// <value>Gets/Sets the instance of GetAllCustomizedProductsModelView.</value>
        [DataMember]
        public GetAllCustomizedProductsModelView customizedProducts { get; set; }
    }
}