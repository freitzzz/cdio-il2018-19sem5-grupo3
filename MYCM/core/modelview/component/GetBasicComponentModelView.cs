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
        /// Boolean indicating whether or not a Component is mandatory.
        /// </summary>
        /// <value>Gets/sets the mandatory flag.</value>
        [DataMember(Order = 6)]
        public bool mandatory { get; set; }
    }
}