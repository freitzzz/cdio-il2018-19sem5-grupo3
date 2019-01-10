namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the ModelView used for finding instances of CustomizedProduct created by a certain User.
    /// </summary>
    public class FindUserCreatedCustomizedProductsModelView
    {
        /// <summary>
        /// User's API token.
        /// </summary>
        /// <value>Gets/Sets the API token.</value>
        public string userApiToken { get; set; }
    }
}