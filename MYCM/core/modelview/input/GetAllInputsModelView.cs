using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.input
{
    /// <summary>
    /// Class representing the ModelView used for retrieving all the Algorithm's inputs.
    /// </summary>
    [CollectionDataContract]
    public class GetAllInputsModelView : List<GetInputModelView>
    {
        
    }
}