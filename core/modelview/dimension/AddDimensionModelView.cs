using System.Runtime.Serialization;
using core.modelview.dimension.converters;
using Newtonsoft.Json;

namespace core.modelview.dimension
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of Dimension.
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(AddDimensionModelViewConverter))]
    public abstract class AddDimensionModelView
    {
        /// <summary>
        /// The dimension's unit value.
        /// </summary>
        /// <value>Gets/sets the unit value.</value>
        [DataMember]
        public string unit {get; set;}
    }
}