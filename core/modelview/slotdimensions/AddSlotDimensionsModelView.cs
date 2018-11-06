using System.Runtime.Serialization;
using core.modelview.customizeddimensions;

namespace core.modelview.slotdimensions
{
    /// <summary>
    /// Class representing the ModelView used for adding slot dimensions.
    /// </summary>
    [DataContract]
    public class AddSlotDimensionsModelView
    {
        /// <summary>
        /// AddCustomizedDimensionsModelView containg the Slot's minimum size information.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember]
        public AddCustomizedDimensionsModelView minSize { get; set; }

        /// <summary>
        /// AddCustomizedDimensionsModelView containg the Slot's maximum size information.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember]
        public AddCustomizedDimensionsModelView maxSize { get; set; }

        /// <summary>
        /// AddCustomizedDimensionsModelView containg the Slot's recommended size information.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember]
        public AddCustomizedDimensionsModelView recommendedSize { get; set; }
    }
}