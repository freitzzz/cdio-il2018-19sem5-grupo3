using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing the ModelView used for fetching data from a Collection of Measurement.
    /// </summary>
    [CollectionDataContract]
    public class GetAllMeasurementsModelView : List<GetMeasurementModelView>
    {
        
    }
}