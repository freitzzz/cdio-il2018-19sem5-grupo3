using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of Slot.
    /// </summary>
    [CollectionDataContract]
    public class GetAllSlotsModelView : List<GetSlotModelView>
    {

    }
}