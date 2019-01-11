using System.Runtime.Serialization;
namespace backend.AuthorizationServices.modelviews{
    /// <summary>
    /// Model View for representing users authorization requests details
    /// </summary>
    [DataContract]
    public sealed class AuthorizationRequestMV:AuthorizationMV{
        /// <summary>
        /// String with the user session cookie
        /// </summary>
        [DataMember(Name="Cookie")]
        public string userSessionCookie{get;set;}

        /// <summary>
        /// String with the user User-Agent
        /// </summary>
        [DataMember(Name="User-Agent")]
        public string userUserAgent{get;set;}

        /// <summary>
        /// String with the user secrete key
        /// </summary>
        [DataMember(Name="Secrete")]
        public string userSecreteKey{get;set;}
    }
}