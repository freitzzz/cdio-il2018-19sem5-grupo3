using System.Runtime.Serialization;
using core.modelview.restriction;

namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's Material.
    /// </summary>
    [DataContract]
    public class GetProductMaterialModelView : GetBasicProductMaterialModelView
    {
        /// <summary>
        /// GetAllRestrictionsModelView with all component restrictions
        /// </summary>
        [DataMember(Name = "restrictions", EmitDefaultValue = false)]
        public GetAllRestrictionsModelView restrictions { get; set; }
    }
}