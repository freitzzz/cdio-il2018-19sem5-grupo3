using core.dto;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for adding a Restriction to a Product's Measurement.
    /// </summary>
    public class AddRestrictionToProductMeasurementModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Product's persistence identifier.</value>
        public long productId { get; set; }

        /// <summary>
        /// Measurement's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Measurement's persistence identifier.</value>
        public long measurementId { get; set; }

        /// <summary>
        /// RestrictionDTO with the Restriction information being applied to the Measurement.
        /// </summary>
        /// <value>Gets/Sets the RestrictionDTO.</value>
        public RestrictionDTO restriction { get; set; }
    }
}