using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for adding a Component to a Product.
    /// </summary>
    [DataContract]
    public class AddComponentModelView
    {
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
        [DataMember(Name = "id")]
        public long childProductId { get; set; }

        /// <summary>
        /// Boolean indicating whether the component is mandatory or not.
        /// </summary>
        /// <value>Gets/sets the mandatory flag.</value>
        [DataMember]
        public bool mandatory { get; set; }
    }
}