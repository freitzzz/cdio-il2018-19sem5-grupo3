using core.modelview.product;
using core.modelview.restriction;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's component.
    /// </summary>
    [DataContract]
    public class GetComponentModelView : GetProductModelView
    {
        /// <summary>
        /// Boolean indicating whether or not a Component is mandatory.
        /// </summary>
        /// <value>Gets/sets the mandatory flag.</value>
        [DataMember(Order = 9)]
        public bool mandatory { get; set; }

        /// <summary>
        /// GetAllRestrictionsModelView with all component restrictions
        /// </summary>
        [DataMember(Name = "restrictions", Order = 10, EmitDefaultValue = false)] //since restrictions are optional, don't serialize if this is null
        public GetAllRestrictionsModelView restrictions { get; set; }
    }
}