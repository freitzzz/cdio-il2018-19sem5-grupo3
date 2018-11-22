namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's Measurement.
    /// </summary>
    public class GetProductMeasurementModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Product's persistence identifier.</value>
        public long productId {get; set;}

        /// <summary>
        /// Measurement's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Measurement's persistence identifier.</value>
        public long measurementId {get; set;}
    }
}