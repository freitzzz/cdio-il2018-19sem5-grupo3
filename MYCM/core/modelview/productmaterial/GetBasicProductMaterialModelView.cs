using System.Runtime.Serialization;
using core.modelview.material;

namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for retrieving basic information from a Product's Material.
    /// </summary>
    [DataContract]
    public class GetBasicProductMaterialModelView : GetBasicMaterialModelView
    {
        [IgnoreDataMember]
        public long productId { get; set; }
    }
}