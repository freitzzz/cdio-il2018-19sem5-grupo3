using System.Runtime.Serialization;
using core.modelview.customizeddimensions;

namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for adding an instance of Slot.
    /// </summary>
    [DataContract]
    public class AddSlotModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the CustomizedProduct's persistence identifier.</value>
        [IgnoreDataMember]
        public long customizedProductId { get; set; }

        /// <summary>
        /// AddCustomizedDimensionsModelView detailing the Slot's dimensions.
        /// </summary>
        /// <value>Gets/Sets the AddCustomizedDimensionsModelView instance.</value>
        [DataMember(Name = "dimensions")]
        public AddCustomizedDimensionsModelView slotDimensions { get; set; }
    }
}