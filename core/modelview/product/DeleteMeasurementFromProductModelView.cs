namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for deleting a Product's measurement.
    /// </summary>
    public class DeleteMeasurementFromProductModelView
    {
        /// <summary>
        /// Product's database identifier.
        /// </summary>
        /// <value>Gets/sets the product's database identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Measurement's database identifier.
        /// </summary>
        /// <value>Gets/sets the measurement's database identifier.</value>
        public long measurementId { get; set; }
    }
}