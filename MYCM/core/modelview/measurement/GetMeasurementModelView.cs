using System.Runtime.Serialization;
using core.modelview.dimension;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing the ModelView being used for fetching instances of Measurement.
    /// </summary>
    [DataContract]
    public class GetMeasurementModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        [IgnoreDataMember]
        //* This property should not be included in serialization, it's only used for data transportation */
        public long productId { get; set; }

        /// <summary>
        /// Measurement's database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember(Name = "id")]
        public long measurementId { get; set; }

        /// <summary>
        /// The Measurement's height Dimension ModelView.
        /// </summary>
        /// <value>Gets/sets the height Dimension ModelView.</value>
        [DataMember]
        public GetDimensionModelView height { get; set; }

        /// <summary>
        /// The Measurement's width Dimension ModelView.
        /// </summary>
        /// <value>Gets/sets the width Dimension ModelView.</value>
        [DataMember]
        public GetDimensionModelView width { get; set; }

        /// <summary>
        /// The Measurement's depth Dimension ModelView.
        /// </summary>
        /// <value>Gets/sets the depth Dimension ModelView.</value>
        [DataMember]
        public GetDimensionModelView depth { get; set; }
    }
}