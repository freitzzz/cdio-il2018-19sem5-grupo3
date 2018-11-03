using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.component{
    /// <summary>
    /// Model View representation for the fetch all component basic information context
    /// </summary>
    /// <typeparam name="GetBasicComponentModelView">Generic-Type param of the component basic information model view</typeparam>
    [CollectionDataContract]
    public sealed class GetAllComponentsModelView:List<GetBasicComponentModelView>{}
}