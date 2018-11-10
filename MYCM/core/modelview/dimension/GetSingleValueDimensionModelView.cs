using System.Runtime.Serialization;

namespace core.modelview.dimension{
    /// <summary>
    /// Model View representation for the fetch single dimension interval information context
    /// </summary>
    [DataContract]
    public sealed class GetSingleValueDimensionModelView:GetDimensionModelView{
        /// <summary>
        /// Value that the dimension has
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 2)]
        public double value { get; set; }
    }
}