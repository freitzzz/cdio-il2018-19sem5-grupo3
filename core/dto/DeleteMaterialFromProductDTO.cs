using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// Model View DTO representation for the delete material from a product context
    /// </summary>
    [DataContract]
    public sealed class DeleteMaterialFromProductDTO{
        /// <summary>
        /// Long with the resource ID of the product which material will be deleted
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the resource ID of the material which will be deleted
        /// </summary>
        public long materialID{get;set;}
    }
}