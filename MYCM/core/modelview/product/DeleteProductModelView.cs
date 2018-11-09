namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for deleting an instance of Product.
    /// </summary>
    public class DeleteProductModelView
    {
        /// <summary>
        /// Database identifier of the Product being deleted.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        public long productId { get; set; }
    }
}