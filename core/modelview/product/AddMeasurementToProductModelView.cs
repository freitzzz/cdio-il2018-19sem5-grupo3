using core.dto;
using core.modelview.dimension;
using core.modelview.measurement;
using System.Runtime.Serialization;

namespace core.modelview.product{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add dimension to a product context
    /// </summary>
    public sealed class AddMeasurementToProductModelView : AddMeasurementModelView{
        /// <summary>
        /// Long with the resource ID of the product which will be complemented
        /// </summary>
        public long productID{get;set;}
    }
}