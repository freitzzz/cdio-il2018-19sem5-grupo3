using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of Component as a List.
    /// </summary>
    [CollectionDataContract]
    public class GetAllComponentsListModelView : List<GetBasicComponentModelView>, GetAllComponentsModelView
    {
        
    }
}