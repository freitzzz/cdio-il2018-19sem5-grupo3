using System.Runtime.Serialization;

namespace core.modelview.dimension{
    /// <summary>
    /// Model View representation for the fetch continuous dimension interval information context
    /// </summary>
    [DataContract]
    public sealed class GetContinuousDimensionIntervalModelView:GetDimensionModelView{
        /// <summary>
        /// Minimum value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 2)]
        public double minValue { get; set; }

        /// <summary>
        /// Maximum value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 3)]
        public double maxValue { get; set; }

        /// <summary>
        /// Increment value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 4)]
        public double increment { get; set; }
    }
}