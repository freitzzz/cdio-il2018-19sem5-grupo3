using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the fetch all products context
    /// </summary>
    /// <typeparam name="GetBasicProductModelView">Generic-Type param of the product basic information model view</typeparam>
    [CollectionDataContract]
    public sealed class GetAllProductsModelView:List<GetBasicProductModelView>{}
}