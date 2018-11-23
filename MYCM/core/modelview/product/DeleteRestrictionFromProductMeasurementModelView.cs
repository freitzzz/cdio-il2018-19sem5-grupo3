namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for deleting a Restriction from a Product's Measurement.
    /// </summary>
    public class DeleteRestrictionFromProductMeasurementModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long productId {get; set;}

        /// <summary>
        /// Measurement's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long measurementId {get; set;}

        /// <summary>
        /// Restriction's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long restrictionId {get; set;}
    }
}