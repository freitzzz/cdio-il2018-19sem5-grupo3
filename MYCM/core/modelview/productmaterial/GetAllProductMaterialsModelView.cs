using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.material;

namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's collection of Material.
    /// </summary>
    [CollectionDataContract]
    public class GetAllProductMaterialsModelView : List<GetBasicProductMaterialModelView> { }
}