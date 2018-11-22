using core.dto;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for adding a Restriction to Product's Material.
    /// </summary>
    public class AddRestrictionToProductMaterialModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Product's persistence identifier.</value>
        public long productId {get; set;}

        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Material's persistence identifier.</value>
        public long materialId {get; set;}

        /// <summary>
        /// RestrictionDTO with the Restriction information being applied to the Material.
        /// </summary>
        /// <value>Gets/Sets the RestrictionDTO.</value>
        public RestrictionDTO restriction {get; set;}
    }
}