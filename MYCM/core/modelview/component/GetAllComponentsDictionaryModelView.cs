using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.productcategory;

namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of Component grouped by a key.
    /// </summary>
    [CollectionDataContract]
    public class GetAllComponentsDictionaryModelView : Dictionary<string, GetAllComponentsListModelView>, GetAllComponentsModelView
    {

    }
}