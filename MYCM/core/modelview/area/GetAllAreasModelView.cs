using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.area
{
    /// <summary>
    /// ModelView to represent all available areas
    /// </summary>
    /// <typeparam name="AreaModelView">ModelView that represents an area</typeparam>
    [CollectionDataContract]
    public class GetAllAreasModelView : List<AreaModelView>
    {

    }
}