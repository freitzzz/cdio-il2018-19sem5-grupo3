using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.customizeddimensions {
    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of CustomizedDimensions.
    /// </summary>
    [CollectionDataContract]
    public class GetAllCustomizedDimensionsModelView : List<GetCustomizedDimensionsModelView> {
    }
}
