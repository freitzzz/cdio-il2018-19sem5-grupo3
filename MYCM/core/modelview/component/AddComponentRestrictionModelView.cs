using System.Runtime.Serialization;
using core.dto;
using core.modelview.restriction;

namespace core.modelview.component {
    /// <summary>
    /// Class representing the ModelView used for adding a Restriction to a Component.
    /// </summary>
    [DataContract]
    public class AddComponentRestrictionModelView {
        /// <summary>
        /// Father Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the father's persistence identifier.</value>
        //*This property should not be included in serialization, it's only used for data transportation */
        [IgnoreDataMember]
        public long fatherProductId { get; set; }

        /// <summary>
        /// Child Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the child's persistence identifier.</value>
        //*This property should not be included in serialization, it's only used for data transportation */
        [IgnoreDataMember]
        public long childProductId { get; set; }

        /// <summary>
        /// AddRestrictionModelView containing the Restriction's information.
        /// </summary>
        /// <value>Gets/sets the instance of AddRestrictionModelView.</value>
        [DataMember]
        public AddRestrictionModelView restriction { get; set; }
    }
}