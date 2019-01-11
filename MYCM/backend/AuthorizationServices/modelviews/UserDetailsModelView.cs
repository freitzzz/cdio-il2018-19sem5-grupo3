namespace backend.AuthorizationServices.modelviews
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a User's details.
    /// </summary>
    public class UserDetailsModelView
    {
        /// <summary>
        /// User's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        public string name { get; set; }

        /// <summary>
        /// User's MYCA (Authorization and Authentication) API Token.
        /// </summary>
        /// <value>Gets/Sets the API Token.</value>
        public string apiToken { get; set; }
    }
}