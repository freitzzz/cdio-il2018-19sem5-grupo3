namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing the ModelView used for removing a Product's Measurement's Restriction.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteMeasurementRestrictionModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Measurement's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Measurement's persistence identifier.</value>
        public long measurementId { get; set; }

        /// <summary>
        /// Restriction's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Restriction's persistence identifier.</value>
        public long restrictionId { get; set; }
    }
}