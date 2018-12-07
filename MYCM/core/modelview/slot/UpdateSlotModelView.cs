using System.Runtime.Serialization;
using core.modelview.customizeddimensions;

namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for updating an instance of Slot.
    /// </summary>
    [DataContract]
    public class UpdateSlotModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct's persistence identifier.</value>
        [IgnoreDataMember]
        public long customizedProductId { get; set; }

        /// <summary>
        /// Slot's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Slot's persistence identifier.</value>
        [IgnoreDataMember]
        public long slotId { get; set; }

        /// <summary>
        /// AddCustomizedDimensionsModelView detailing the Slot's new dimensions.
        /// </summary>
        /// <value>Gets/Sets the AddCustomizedDimensionsModelView instance.</value>
        [DataMember]
        public AddCustomizedDimensionsModelView dimensions { get; set; }
    }
}