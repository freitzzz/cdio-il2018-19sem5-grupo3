using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.algorithm
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of Algorithm.
    /// </summary>
    [CollectionDataContract]
    public class GetAllAlgorithmsModelView : List<GetBasicAlgorithmModelView>
    {

    }
}