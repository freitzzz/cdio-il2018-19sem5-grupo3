using System.Runtime.Serialization;

namespace core.modelview.dimension
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of SingleValueDimension.
    /// </summary>
    [DataContract]
    public class AddSingleValueDimensionModelView : AddDimensionModelView
    {
        /// <summary>
        /// The dimension's value.
        /// </summary>
        /// <value>Gets/sets the dimension's value.</value>
        [DataMember]
        public double value { get; set; }
    }
}