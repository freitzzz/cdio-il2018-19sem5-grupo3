namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the ModelView used for finding instances of CustomizedProduct created by a certain User.
    /// </summary>
    public class FindUserCreatedCustomizedProductsModelView
    {
        /// <summary>
        /// User's authentication token.
        /// </summary>
        /// <value>Gets/Sets the authentication token.</value>
        public string userAuthToken { get; set; }
    }
}