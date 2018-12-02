namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for deleting a Restriction from a Product's Material.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteProductMaterialRestrictionModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Material's persistence identifier.</value>
        public long materialId { get; set; }

        /// <summary>
        /// Restriction's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Restriction's persistence identifier.</value>
        public long restrictionId { get; set; }
    }
}