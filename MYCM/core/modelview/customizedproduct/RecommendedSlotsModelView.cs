using core.modelview.customizeddimensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.customizedproduct {
    /// <summary>
    /// Class representing the ModelView used for fetching a CustomizedProduct's recommended slots.
    /// </summary>
    [DataContract]
    public class RecommendedSlotsModelView {
        /// <summary>
        /// List of Slot's Customized Dimensions.
        /// </summary>
        /// <value>Gets/sets the list.</value>
        [DataMember]
        public List<GetCustomizedDimensionsModelView> recommendedSlots { get; set; }
    }
}
