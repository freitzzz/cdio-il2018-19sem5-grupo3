namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for deleting an instance of Slot from a CustomizedProduct's collection of Slot.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class DeleteSlotModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct's persistence identifier.</value>
        public long customizedProductId { get; set; }

        /// <summary>
        /// Slot's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Slot's persistence identifier.</value>
        public long slotId { get; set; }

        /// <summary>
        /// User's authentication token.
        /// </summary>
        /// <value>Gets/Sets the authentication token.</value>
        public string userAuthToken { get; set; }
    }
}