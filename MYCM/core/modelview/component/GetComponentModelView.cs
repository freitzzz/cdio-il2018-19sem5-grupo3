using core.modelview.restriction;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's component.
    /// </summary>
    [DataContract]
    public class GetComponentModelView : GetBasicComponentModelView
    {
        /// <summary>
        /// GetAllRestrictionsModelView with all component restrictions
        /// </summary>
        [DataMember(Name = "restrictions", EmitDefaultValue=false)]
        public GetAllRestrictionsModelView restrictions { get; set; }
    }
}