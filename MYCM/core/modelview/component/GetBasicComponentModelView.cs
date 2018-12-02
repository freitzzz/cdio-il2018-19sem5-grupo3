using System.Runtime.Serialization;
using core.modelview.product;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving basic information from a Product's Component.
    /// </summary>
    [DataContract]
    public class GetBasicComponentModelView : GetBasicProductModelView
    {
        /// <summary>
        /// Father Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the father Product's persistence identifier.</value>
        [IgnoreDataMember]
        //*This property should not be included in serialization, it's only used for data transportation */
        public long fatherProductId { get; set; }

        /// <summary>
        /// Boolean indicating whether or not a Component is mandatory.
        /// </summary>
        /// <value>Gets/sets the mandatory flag.</value>
        [DataMember(Name = "mandatory", Order = 4)]
        public bool mandatory { get; set; }
    }
}