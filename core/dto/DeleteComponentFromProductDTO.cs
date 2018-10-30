using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// Model View DTO representation for the delete component from a product context
    /// </summary>
    [DataContract]
    public sealed class DeleteComponentFromProductDTO{
        /// <summary>
        /// Long with the resource ID of the product which component will be deleted
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the resource ID of the component which will be deleted
        /// </summary>
        public long componentID{get;set;}
    }
}