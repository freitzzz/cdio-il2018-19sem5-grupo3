using backend.AuthorizationServices.exceptions;
using backend.AuthorizationServices.modelviews;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace backend.AuthorizationServices{
    /// <summary>
    /// Services class for granting users authorization
    /// </summary>
    public sealed class AuthorizationService{

        /// <summary>
        /// HttpClient with the http client configured for MYCA Web API
        /// </summary>
        public static HttpClient MYCA_CLIENT;

        // <summary>
        /// Grants that a user is authorized
        /// </summary>
        /// <param name="authorizationRequestDetails">AuthorizationRequestMV with the authorization request details</param>
        public static void grantUserIsAuthorized(AuthorizationRequestMV authorizationRequestMV){
            Task<NotAuthorizedException> grantTask=_grantUserIsAuthorized(authorizationRequestMV);
            grantTask.Wait();
            if(grantTask==null)return;
            if(grantTask.Result!=null)
                throw grantTask.Result;
        }

        /// <summary>
        /// Grants thata user is authorized
        /// </summary>
        /// <param name="authorizationRequestDetails">AuthorizationRequestMV with the authorization request details</param>
        public async static Task<NotAuthorizedException> _grantUserIsAuthorized(AuthorizationRequestMV authorizationRequestDetails){
            HttpRequestMessage requestDetails=new HttpRequestMessage(HttpMethod.Get,string.Format("/myca/api/autho?manager={0}",authorizationRequestDetails.userIsContentManager));
            requestDetails.Headers.Add("User-Agent",authorizationRequestDetails.userUserAgent);
            requestDetails.Headers.Add("Cookie",authorizationRequestDetails.userSessionCookie);
            requestDetails.Headers.Add("Secrete",authorizationRequestDetails.userSecreteKey);
            var response=await MYCA_CLIENT.SendAsync(requestDetails);
            AuthorizationResponse responseBody=null;
            try{
                responseBody=await response.Content.ReadAsAsync<AuthorizationResponse>();
            }catch(Exception){
                return new NotAuthorizedException("An unexpected error occurd :(",500);
            }
            if(response.StatusCode!=HttpStatusCode.NoContent){
                return new NotAuthorizedException(responseBody.message,response.StatusCode.GetHashCode());
            }
            return null;
        }

        /// <summary>
        /// Inner class to help authorization responses deserilization
        /// </summary>
        private class AuthorizationResponse{
            /// <summary>
            /// String with the authorization response message
            /// </summary>
            public string message{get;set;}
        }
    }
}