namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for deleting a Restriction from a Product's Material.
    /// </summary>
    public class DeleteRestrictionFromProductMaterialModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Product's persistence identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Material's persistence identifier.</value>
        public long materialId {get; set;}

        /// <summary>
        /// Restriction's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Restriction's persistence identifier.</value>
        public long restrictionId {get; set;}
    }
}