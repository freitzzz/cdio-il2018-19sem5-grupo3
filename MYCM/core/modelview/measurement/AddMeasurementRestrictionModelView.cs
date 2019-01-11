using System.Runtime.Serialization;
using core.dto;
using core.modelview.restriction;

namespace core.modelview.measurement
{
    [DataContract]
    public class AddMeasurementRestrictionModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        [IgnoreDataMember]
        //* This property should not be included in serialization, it's only used for data transportation */
        public long productId { get; set; }

        /// <summary>
        /// Measurement's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Measurement's persistence identifier.</value>
        [IgnoreDataMember]
        //* This property should not be included in serialization, it's only used for data transportation */
        public long measurementId { get; set; }

        /// <summary>
        /// AddRestrictionModelView containing the Restriction's information.
        /// </summary>
        /// <value>Gets/sets the instance of AddRestrictionModelView.</value>
        [DataMember]
        public AddRestrictionModelView restriction { get; set; }
    }
}