using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.dimension
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of DiscreteDimensionInterval.
    /// </summary>
    [DataContract]
    public class AddDiscreteDimensionIntervalModelView : AddDimensionModelView
    {
        /// <summary>
        /// The interval's list of values.
        /// </summary>
        /// <value>Gets/sets the list of values.</value>
        [DataMember]
        public List<double> values { get; set; }
    }
}