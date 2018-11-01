using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View DTO representation for the delete dimension from a product context
    /// </summary>
    [DataContract]
    public sealed class DeleteDimensionFromProductModelView{
        /// <summary>
        /// Long with the resource ID of the product which dimension will be deleted
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the resource ID of the dimension which will be deleted
        /// </summary>
        public long dimensionID{get;set;}
    }
}