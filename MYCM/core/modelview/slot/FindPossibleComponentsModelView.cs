using System;
using System.Collections.Generic;
using System.Text;

namespace core.modelview.slot {
    /// <summary>
    /// Class representing the ModelView used for fetching all possible components for a selected slot of a customized product
    /// </summary>
    public class FindPossibleComponentsModelView {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct's persistence identifier.</value>
        public long customizedProductID { get; set; }

        /// <summary>
        /// Slot's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Slot's persistence identifier.</value>
        public long slotID { get; set; }
    }
}
