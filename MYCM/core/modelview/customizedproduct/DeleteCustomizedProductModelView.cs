namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the ModelView used for deleting an instance of CustomizedProduct.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct's persistence identifier.</value>
        public long customizedProductId { get; set; }
    }
}