using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using backend.AuthorizationServices.modelviews;
using backend.utils;
using core.exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using support.utils;

namespace backend.middleware
{
    /// <summary>
    /// Class representing the Middleware used for authorizing users.
    /// </summary>
    public class AuthorizationMiddleware
    {
        /// <summary>
        /// Next layer of middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// HttpClientFactory used for creating instances of HttpClient to communicate with MYCA.
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Constant representing the name of the HttpClients used for communicating with MYCA.
        /// </summary>
        private const string MYCA_CLIENT_NAME = "MYCA";

        /// <summary>
        /// Constant representing the message presented when a request to the MYCA API returns the 500 Status Code.
        /// </summary>
        private const string AUTHORIZATION_SERVICE_UNEXPECTED_ERROR = "The authorization service is not responding. Please, try again later.";

        /// <summary>
        /// Constant representing the name of the MYCA Session Cookie.
        /// </summary>
        private const string SESSION_COOKIE_NAME = "MYCASESSION";

        /// <summary>
        /// Constant representing the User-Agent header.
        /// </summary>
        private const string USER_AGENT_HEADER = "User-Agent";

        /// <summary>
        /// Constant representing the Cookie header.
        /// </summary>
        private const string COOKIE_HEADER = "Cookie";

        /// <summary>
        /// Cosnstant representing the session's secrete header.
        /// </summary>
        private const string SECRET_HEADER = "Secrete";

        /// <summary>
        /// Constant representing the user's token.
        /// </summary>
        private const string USER_TOKEN_HEADER = "UserToken";

        /// <summary>
        /// Constant representing the message presented when a user is attempted to be authorized without the user agent header.
        /// </summary>
        private const string USER_AGENT_HEADER_NOT_FOUND = "User-Agent header not found.";

        /// <summary>
        /// Constant representing the message presented when a user is attempted to be authorized without the secret header.
        /// </summary>
        private const string SECRET_HEADER_NOT_FOUND = "Secret header not found.";

        /// <summary>
        /// Constant representing the message presented when a request is performed without an active session cookie.
        /// </summary>
        private const string SESSION_COOKIE_NOT_FOUND = "Session cookie not found.";

        /// <summary>
        /// Creates a new instance of AuthorizationMiddleware.
        /// </summary>
        /// <param name="next">Next RequestDelegate in the pipeline.</param>
        /// <param name="clientFactory">Instance of IHttpClientFactory used for creating instances of HttpClient.</param>
        public AuthorizationMiddleware(RequestDelegate next, IHttpClientFactory clientFactory)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (clientFactory == null) throw new ArgumentNullException(nameof(clientFactory));
            this.next = next;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Asynchronously invokes the AuthorizationMiddleware.
        /// </summary>
        /// <param name="httpContext">HttpContext to which the middleware is attached.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            HttpRequest request = httpContext.Request;

            if (request.Method != HttpMethod.Get.Method &&
                (request.Path.StartsWithSegments("/mycm/api/algorithms") || request.Path.StartsWithSegments("/mycm/api/categories")
                    || request.Path.StartsWithSegments("/mycm/api/products") || request.Path.StartsWithSegments("/mycm/api/materials")
                    || request.Path.StartsWithSegments("/mycm/api/collections") || request.Path.StartsWithSegments("/mycm/api/commercialcatalogues")))
            {
                await this.authorizeContentManager(request);
            }
            else if (request.Path.StartsWithSegments("/mycm/api/prices"))
            {
                await this.authorizeAdministrator(request);
            }
            else if (request.Path.StartsWithSegments("/mycm/api/customizedproducts"))
            {
                await this.authorizeClient(request);
            }

            await this.next(httpContext);
        }

        /// <summary>
        /// Checks if a user is authorized as a content manager.
        /// </summary>
        /// <param name="request">HttpRequest being performed.</param>
        private async Task authorizeContentManager(HttpRequest request)
        {
            StringValues userAgentHeaderValue;
            bool hasUserAgentHeader = request.Headers.TryGetValue(USER_AGENT_HEADER, out userAgentHeaderValue);

            StringValues secretHeaderValue;
            bool hasSecretHeader = request.Headers.TryGetValue(SECRET_HEADER, out secretHeaderValue);

            string sessionCookieValue;
            bool hasSessionCookie = request.Cookies.TryGetValue(SESSION_COOKIE_NAME, out sessionCookieValue);

            if (!hasUserAgentHeader) throw new ArgumentException(USER_AGENT_HEADER_NOT_FOUND);
            if (!hasSecretHeader) throw new ArgumentException(SECRET_HEADER_NOT_FOUND);
            if (!hasSessionCookie) throw new ArgumentException(SESSION_COOKIE_NOT_FOUND);

            HttpClient client = this.clientFactory.CreateClient(MYCA_CLIENT_NAME);

            Uri uri = new Uri(client.BaseAddress + "/autho?contentmanager=true");

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            requestMessage.Headers.TryAddWithoutValidation(SECRET_HEADER, secretHeaderValue.ToString());
            requestMessage.Headers.TryAddWithoutValidation(USER_AGENT_HEADER, userAgentHeaderValue.ToString());
            requestMessage.Headers.TryAddWithoutValidation(COOKIE_HEADER, "MYCASESSION=" + sessionCookieValue);

            var response = await client.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    SimpleJSONMessageService message = await response.Content.ReadAsAsync<SimpleJSONMessageService>();
                    throw new NotAuthorizedException(message.message);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception(AUTHORIZATION_SERVICE_UNEXPECTED_ERROR);
                }
            }
        }


        /// <summary>
        /// Checks if user is authorized as an administrator.
        /// </summary>
        /// <param name="request">HttpRequest being performed.</param>
        private async Task authorizeAdministrator(HttpRequest request)
        {
            StringValues userAgentHeaderValue;
            bool hasUserAgentHeader = request.Headers.TryGetValue(USER_AGENT_HEADER, out userAgentHeaderValue);

            StringValues secretHeaderValue;
            bool hasSecretHeader = request.Headers.TryGetValue(SECRET_HEADER, out secretHeaderValue);

            string sessionCookieValue;
            bool hasSessionCookie = request.Cookies.TryGetValue(SESSION_COOKIE_NAME, out sessionCookieValue);

            if (!hasUserAgentHeader) throw new ArgumentException(USER_AGENT_HEADER_NOT_FOUND);
            if (!hasSecretHeader) throw new ArgumentException(SECRET_HEADER_NOT_FOUND);
            if (!hasSessionCookie) throw new ArgumentException(SESSION_COOKIE_NOT_FOUND);

            HttpClient client = this.clientFactory.CreateClient(MYCA_CLIENT_NAME);

            Uri uri = new Uri(client.BaseAddress + "/autho?administrator=true");

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            requestMessage.Headers.TryAddWithoutValidation(SECRET_HEADER, secretHeaderValue.ToString());
            requestMessage.Headers.TryAddWithoutValidation(USER_AGENT_HEADER, userAgentHeaderValue.ToString());
            requestMessage.Headers.TryAddWithoutValidation(COOKIE_HEADER, "MYCASESSION=" + sessionCookieValue);

            var response = await client.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    SimpleJSONMessageService message = await response.Content.ReadAsAsync<SimpleJSONMessageService>();
                    throw new NotAuthorizedException(message.message);

                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception(AUTHORIZATION_SERVICE_UNEXPECTED_ERROR);
                }
            }
        }

        /// <summary>
        /// Checks if user is authorized as a client.
        /// </summary>
        /// <param name="request">HttpRequest being performed.</param>
        private async Task authorizeClient(HttpRequest request)
        {
            string sessionCookieValue;
            bool hasSessionCookie = request.Cookies.TryGetValue(SESSION_COOKIE_NAME, out sessionCookieValue);

            //if the session cookie has a value, check if the user is authorized as a client
            if (hasSessionCookie && sessionCookieValue.Length > 0)
            {
                StringValues userAgentHeaderValue;
                bool hasUserAgentHeader = request.Headers.TryGetValue(USER_AGENT_HEADER, out userAgentHeaderValue);

                StringValues secretHeaderValue;
                bool hasSecretHeader = request.Headers.TryGetValue(SECRET_HEADER, out secretHeaderValue);

                HttpClient authorizationClient = this.clientFactory.CreateClient(MYCA_CLIENT_NAME);

                Uri authorizationUri = new Uri(authorizationClient.BaseAddress + "/autho");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, authorizationUri);

                requestMessage.Headers.TryAddWithoutValidation(SECRET_HEADER, secretHeaderValue.ToString());
                requestMessage.Headers.TryAddWithoutValidation(USER_AGENT_HEADER, userAgentHeaderValue.ToString());
                requestMessage.Headers.TryAddWithoutValidation(COOKIE_HEADER, "MYCASESSION=" + sessionCookieValue);

                var response = await authorizationClient.SendAsync(requestMessage);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        SimpleJSONMessageService message = await response.Content.ReadAsAsync<SimpleJSONMessageService>();
                        throw new NotAuthorizedException(message.message);

                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(AUTHORIZATION_SERVICE_UNEXPECTED_ERROR);
                    }
                }

                //if the user is authorized, capture the api token
                await this.captureUserApiToken(request);
            }
        }

        /// <summary>
        /// Capture a user's API token and adds it as a header to the request made to the CustomizedProductController.
        /// </summary>
        /// <param name="request">HttpRequest being performed.</param>
        private async Task captureUserApiToken(HttpRequest request)
        {
            string sessionCookieValue;
            bool hasSessionCookie = request.Cookies.TryGetValue(SESSION_COOKIE_NAME, out sessionCookieValue);

            StringValues userAgentHeaderValue;
            bool hasUserAgentHeader = request.Headers.TryGetValue(USER_AGENT_HEADER, out userAgentHeaderValue);

            StringValues secretHeaderValue;
            bool hasSecretHeader = request.Headers.TryGetValue(SECRET_HEADER, out secretHeaderValue);

            HttpClient httpClient = this.clientFactory.CreateClient(MYCA_CLIENT_NAME);

            Uri userUri = new Uri(httpClient.BaseAddress + "/users");

            HttpRequestMessage userRequestMessage = new HttpRequestMessage(HttpMethod.Get, userUri);

            userRequestMessage.Headers.TryAddWithoutValidation(SECRET_HEADER, secretHeaderValue.ToString());
            userRequestMessage.Headers.TryAddWithoutValidation(USER_AGENT_HEADER, userAgentHeaderValue.ToString());
            userRequestMessage.Headers.TryAddWithoutValidation(COOKIE_HEADER, "MYCASESSION=" + sessionCookieValue);

            var userResponse = await httpClient.SendAsync(userRequestMessage);

            if (userResponse.StatusCode == HttpStatusCode.OK)
            {
                UserDetailsModelView userDetailsModelView = await userResponse.Content.ReadAsAsync<UserDetailsModelView>();
                request.Headers.TryAdd(USER_TOKEN_HEADER, userDetailsModelView.apiToken);
            }
            else
            {
                SimpleJSONMessageService message = await userResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

                if (userResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new ArgumentException(message.message);
                }
                else if (userResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new NotAuthorizedException(message.message);
                }
                else if (userResponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception(AUTHORIZATION_SERVICE_UNEXPECTED_ERROR);
                }
            }
        }
    }


    /// <summary>
    /// Static class representing AuthorizationMiddlewareExtensions.
    /// </summary>
    public static class AuthorizationMiddlewareExtensions
    {
        /// <summary>
        /// Registers the AuthorizationMiddleware in the Application.
        /// </summary>
        /// <param name="builder">Instance of IAppucationBuilder, building the Application.</param>
        /// <returns>IApplicationBuilder with the AuthorizationMiddleware attached to it.</returns>
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}