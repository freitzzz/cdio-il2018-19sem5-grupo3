namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's Material.
    /// </summary>
    public class GetProductMaterialModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long materialId { get; set; }
    }
}