using System.Runtime.Serialization;

namespace backend.AuthorizationServices.modelviews{
    /// <summary>
    /// Model View for representing users authorization details
    /// </summary>
    [DataContract]
    public class AuthorizationMV{
        /// <summary>
        /// String with the user API token
        /// </summary>
        [DataMember(Name="token")]
        public string userAPIToken{get;set;}

        /// <summary>
        /// Boolean true if the user is a content manager
        /// </summary>
        [DataMember(Name="contentmanager")]
        public bool userIsContentManager;

    }
}