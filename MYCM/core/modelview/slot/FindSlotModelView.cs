namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for finding an instance of Slot.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class FindSlotModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the CustomizedProduct's persistence identifier.</value>
        public long customizedProductId { get; set; }

        /// <summary>
        /// Slot's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Slot's persistence identifier.</value>
        public long slotId { get; set; }
    }
}