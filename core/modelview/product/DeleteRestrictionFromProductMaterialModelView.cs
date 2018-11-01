using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the delete restriction from a product material context
    /// </summary>
    public sealed class DeleteRestrictionFromProductMaterialModelView{
        /// <summary>
        /// Long with the product resource ID which restriction will be deleted from its material
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the material resource ID which restriction will be deleted from
        /// </summary>
        public long materialID{get;set;}

        /// <summary>
        /// Long with the restriction resource ID which will be deleted
        /// </summary>
        public long restrictionID{get;set;}
    }
}