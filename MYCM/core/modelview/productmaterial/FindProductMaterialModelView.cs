namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for finding a Material from a Product's collection of Material.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class FindProductMaterialModelView
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
    }
}