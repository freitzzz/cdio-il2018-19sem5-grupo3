using System;

namespace backend.AuthorizationServices.exceptions{
    public sealed class NotAuthorizedException:InvalidOperationException{
        /// <summary>
        /// String with the authorization rejection reason
        /// </summary>
        public string reason{get;}

        /// <summary>
        /// Integer with the authorization rejection request status code
        /// </summary>
        public int statusCode{get;}

        /// <summary>
        /// Builds a new NotAuthorizedException
        /// </summary>
        /// <param name="reason">String with the authorization rejection reason</param>
        /// <param name="statusCode">Integer with the authorization reject request status code</param>
        public NotAuthorizedException(string reason,int statusCode){
            this.reason=reason;
            this.statusCode=statusCode;
        }
    }
}