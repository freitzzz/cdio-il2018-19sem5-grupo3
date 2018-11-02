using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.dimension{
    /// <summary>
    /// Model View representation for the fetch all dimensions context
    /// </summary>
    /// <typeparam name="GetBasicProductModelView">Generic-Type param of the dimension information model view</typeparam>
    [CollectionDataContract]
    public sealed class GetAllDimensionsModelView:List<GetDimensionModelView>{}
}