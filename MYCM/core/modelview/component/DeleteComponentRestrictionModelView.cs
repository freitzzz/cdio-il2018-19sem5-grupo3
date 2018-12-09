namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for deleting a Component's Restriction.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteComponentRestrictionModelView
    {
        /// <summary>
        /// Father Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the father's persistence identifier.</value>
        public long fatherProductId { get; set; }

        /// <summary>
        /// Child Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the child's persistence identifier.</value>
        public long childProductId { get; set; }

        /// <summary>
        /// Restriction's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the restriction's persistence identifier.</value>
        public long restrictionId { get; set; }
    }
}