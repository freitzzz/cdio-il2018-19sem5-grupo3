namespace core.modelview.slot
{
    /// <summary>
    /// Class representing the ModelView used for adding a preset Slot layout to a CustomizedProduct.
    /// </summary>
    public class AddSlotLayoutModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long customizedProductId { get; set; }

        /// <summary>
        /// User's authentication token.
        /// </summary>
        /// <value>Gets/Sets the authentication token.</value>
        public string userAuthToken { get; set; }

        /// <summary>
        /// Additional options used for adding a Slot layout to a CustomizedProduct.
        /// </summary>
        /// <returns>Gets/Sets the options.</returns>
        public AddSlotLayoutModelViewOptions options { get; set; } = new AddSlotLayoutModelViewOptions();
    }


    /// <summary>
    /// Class representing the options used for adding a Slot layout.
    /// </summary>
    public class AddSlotLayoutModelViewOptions
    {
        /// <summary>
        /// Unit to which the CustomizedProduct's dimensions will be converted.
        /// </summary>
        /// <value>Gets/Sets the unit.</value>
        public string unit { get; set; }
    }
}