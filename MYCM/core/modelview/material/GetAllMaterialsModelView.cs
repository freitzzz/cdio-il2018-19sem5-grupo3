using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.material{
    /// <summary>
    /// Model View representation for the fetch all material basic information context
    /// </summary>
    /// <typeparam name="GetBasicMaterialModelView">Generic-Type param of the material basic information model view</typeparam>
    [CollectionDataContract]
    public sealed class GetAllMaterialsModelView:List<GetBasicMaterialModelView>{}
}