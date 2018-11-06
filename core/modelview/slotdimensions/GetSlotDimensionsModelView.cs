using System.Runtime.Serialization;
using core.modelview.customizeddimensions;

namespace core.modelview.slotdimensions
{
    /// <summary>
    /// Class representing the ModelView used for fetching Slot's CustomizedDimensions.
    /// </summary>
    [DataContract]
    public class GetSlotDimensionsModelView
    {
        /// <summary>
        /// ModelView representing the Slot's minimum size CustomizedDimensions.
        /// </summary>
        /// <value>Gets/sets the minimum size ModelView.</value>
        [DataMember]
        public GetCustomizedDimensionsModelView minSize { get; set; }

        /// <summary>
        /// ModelView representing the Slot's maximum size CustomizedDimensions.
        /// </summary>
        /// <value>Gets/sets the maximum size ModelView.</value>
        [DataMember]
        public GetCustomizedDimensionsModelView maxSize { get; set; }

        /// <summary>
        /// ModelView representing the Slot's recommended size CustomizedDimensions.
        /// </summary>
        /// <value>Gets/sets the recommended size ModelView.</value>
        [DataMember]
        public GetCustomizedDimensionsModelView recommendedSize { get; set; }
    }
}