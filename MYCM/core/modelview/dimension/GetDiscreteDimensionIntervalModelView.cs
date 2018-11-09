using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.dimension{
    /// <summary>
    /// Model View representation for the fetch discrete dimension interval information context
    /// </summary>
    [DataContract]
    public sealed class GetDiscreteDimensionIntervalModelView:GetDimensionModelView{
        
        /// <summary>
        /// List of values that the dimension can have
        /// </summary>
        /// <value>Get/Set of the list of values</value>
        [DataMember (Order = 2)]
        public List<double> values { get; set; }
    }
}