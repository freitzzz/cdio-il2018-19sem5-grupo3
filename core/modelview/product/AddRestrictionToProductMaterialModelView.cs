using core.dto;
using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the add restriction to a product material context
    /// </summary>
    [DataContract]
    public sealed class AddRestrictionToProductMaterialModelView{
        /// <summary>
        /// Long with the product resource ID which restriction will be applied to its material
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the material resource ID which restriction will be applied to
        /// </summary>
        public long materialID{get;set;}
        
        /// <summary>
        /// RestrictionDTO with the restriction information which will be applied to the product material
        /// </summary>
        public RestrictionDTO restriction{get;set;}
    }
}