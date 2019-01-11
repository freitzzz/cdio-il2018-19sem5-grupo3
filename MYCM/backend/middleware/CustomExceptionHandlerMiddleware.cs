using System;
using System.Net;
using System.Threading.Tasks;
using backend.utils;
using core.exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace backend.middleware
{
    /// <summary>
    /// Class representing the Middleware used for creating responses when an unhandled Exception is raised.
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private const string UNEXPECTED_ERROR_MSG = "An unexpected error occurred, please try again later.";

        /// <summary>
        /// Next layer of Middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Creates an instance of CustomExceptionHandlerMiddleware, registering the next layer in the pipeline.
        /// </summary>
        /// <param name="next">RequestDelegate representing the next layer.</param>
        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            this.next = next;
        }


        /// <summary>
        /// Asynchronously invokes the middleware.
        /// </summary>
        /// <param name="context">HttpContext to which the middleware is attached.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await this.next(context);
            }
            catch (NotAuthorizedException ex)
            {
                await buildResponse(ex.Message, HttpStatusCode.Unauthorized, context);
                return;
            }
            catch (ResourceNotFoundException ex)
            {
                await buildResponse(ex.Message, HttpStatusCode.NotFound, context);
                return;
            }
            catch (ArgumentException ex)
            {
                await buildResponse(ex.Message, HttpStatusCode.BadRequest, context);
                return;
            }
            catch (InvalidOperationException ex)
            {
                await buildResponse(ex.Message, HttpStatusCode.BadRequest, context);
                return;
            }
            catch (Exception)
            {
                await buildResponse(UNEXPECTED_ERROR_MSG, HttpStatusCode.InternalServerError, context);
                return;
            }
        }

        /// <summary>
        /// Builds the response.
        /// </summary>
        /// <param name="message">Message that will be attached to the response.</param>
        /// <param name="statusCode">Response's status code.</param>
        /// <param name="context">HttpContext to which the response will be attached.</param>
        public async Task buildResponse(string message, HttpStatusCode statusCode, HttpContext context)
        {
            var result = JsonConvert.SerializeObject(new SimpleJSONMessageService(message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(result);
        }
    }

    /// <summary>
    /// Static class reprsenting the CustomExceptionHandlerMiddlewareExtensions.
    /// </summary>
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Registers the CustomExceptionHandlerMiddleware in the Application.
        /// </summary>
        /// <param name="builder">Instance of IApplicationBuilder, building the Application.</param>
        /// <returns>IApplicationBuilder with the CustomExceptionHandlerMiddleware attached to it.</returns>
        public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}