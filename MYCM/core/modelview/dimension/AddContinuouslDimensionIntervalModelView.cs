using System.Runtime.Serialization;

namespace core.modelview.dimension
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of ContinuousDimensionInterval.
    /// </summary>
    [DataContract]
    public class AddContinuousDimensionIntervalModelView : AddDimensionModelView
    {
        /// <summary>
        /// The interval's minimum value.
        /// </summary>
        /// <value>Gets/sets the minimum value.</value>
        [DataMember]
        public double minValue { get; set; }

        /// <summary>
        /// The interval's maximum value.
        /// </summary>
        /// <value>Gets/sets the maximum value.</value>
        [DataMember]
        public double maxValue { get; set; }

        /// <summary>
        /// The interval's increment value.
        /// </summary>
        /// <value>Gets/sets the increment value.</value>
        [DataMember]
        public double increment { get; set; }

    }
}