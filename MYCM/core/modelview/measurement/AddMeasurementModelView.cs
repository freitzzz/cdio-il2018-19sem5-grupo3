using System.Runtime.Serialization;
using core.modelview.dimension;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of Measurement.
    /// </summary>
    [DataContract]
    public class AddMeasurementModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        [IgnoreDataMember]
        //* This property should not be included in serialization, it's only used for data transportation */
        public long productId { get; set; }

        /// <summary>
        /// AddDimensionModelView with the width dimension which will be added to the product
        /// </summary>
        [DataMember(Name = "width")]
        public AddDimensionModelView widthDimension { get; set; }

        /// <summary>
        /// AddDimensionModelView with the height dimension which will be added to the product
        /// </summary>
        [DataMember(Name = "height")]
        public AddDimensionModelView heightDimension { get; set; }

        /// <summary>
        /// AddDimensionModelView with the depth dimension which will be added to the product
        /// </summary>
        [DataMember(Name = "depth")]
        public AddDimensionModelView depthDimension { get; set; }
    }
}