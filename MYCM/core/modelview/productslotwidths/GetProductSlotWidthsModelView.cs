using System.Runtime.Serialization;
using core.modelview.customizeddimensions;

namespace core.modelview.productslotwidths
{
    /// <summary>
    /// Class representing the ModelView used for retrieving the Product's slots' widths.
    /// </summary>
    [DataContract]
    public class GetProductSlotWidthsModelView
    {
        /// <summary>
        /// Product's slots' minimum width value.
        /// </summary>
        /// <value>Gets/sets the minimum width value.</value>
        [DataMember]
        public double minWidth { get; set; }

        /// <summary>
        /// Product's slots' maximum width value.
        /// </summary>
        /// <value>Gets/sets the maximum width value.</value>
        [DataMember]
        public double maxWidth { get; set; }

        /// <summary>
        /// Product's slots' recommended width value.
        /// </summary>
        /// <value>Gets/sets the recommended width value.</value>
        [DataMember]
        public double recommendedWidth { get; set; }

        /// <summary>
        /// Product's slots' width values' unit.
        /// </summary>
        /// <value>Gets/sets the width values' unit.</value>
        [DataMember]
        public string unit {get; set;}
    }
}