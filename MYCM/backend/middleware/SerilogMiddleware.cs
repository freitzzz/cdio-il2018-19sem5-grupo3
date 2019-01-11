using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace backend.middleware
{
    /// <summary>
    /// Class representing the Middleware used for logging
    /// </summary>
    public class SerilogMiddleware
    {
        /// <summary>
        /// Template log message.
        /// </summary>
        private const string messageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms; Request Body: {RequestBody}; Response Body: {ResponseBody}";

        /// <summary>
        /// Global, static instance of Serilog.
        /// </summary>
        /// <returns>Serilog's instance.</returns>
        private static readonly ILogger logger = Serilog.Log.ForContext<SerilogMiddleware>();

        /// <summary>
        /// Next layer of Middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Creates an instance of SerilogMiddleware, registering the next layer in the pipeline.
        /// </summary>
        /// <param name="next">RequestDelegate representing the next layer.</param>
        public SerilogMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            this.next = next;
        }


        /// <summary>
        /// Asynchronously invokes the middleware.
        /// </summary>
        /// <param name="httpContext">HttpContext to which the middleware is attached.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            //read request before any other middleware since it might not be readable later
            string requestBodyContent = await readRequestBody(httpContext.Request);

            var stopWatch = Stopwatch.StartNew();
            try
            {
                //Copy a pointer to the original response body stream
                var originalBodyStream = httpContext.Response?.Body;

                using (var responseBody = new MemoryStream())
                {
                    httpContext.Response.Body = responseBody;

                    await this.next(httpContext);
                    stopWatch.Stop();

                    string responseBodyContent = await readResponseBody(httpContext.Response);

                    double elapsedMs = stopWatch.Elapsed.TotalMilliseconds;

                    var statusCode = httpContext.Response?.StatusCode;

                    //client errors are logged as warnings
                    var isWarning = statusCode > 399 && statusCode < 500;
                    //server errors are logged as errors
                    var isError = statusCode > 499;

                    ILogger log = logger;
                    LogEventLevel loggingLevel = LogEventLevel.Information;

                    if (isWarning)
                    {
                        log = logForWarningContext(httpContext);
                        loggingLevel = LogEventLevel.Warning;
                    }
                    else if (isError)
                    {
                        log = logForErrorContext(httpContext);
                        loggingLevel = LogEventLevel.Error;
                    }

                    log.Write(loggingLevel, messageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, elapsedMs, requestBodyContent, responseBodyContent);

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody?.CopyToAsync(originalBodyStream);
                }
            }
            //Exception is never caught, because logException() always returns false, so that the next middleware can handle it
            catch (Exception exception) when (logException(httpContext, stopWatch, exception)) { }
        }


        /// <summary>
        /// Reads the HttpRequest's body.
        /// </summary>
        /// <param name="request">HttpRequest with the body being read.</param>
        /// <returns>Task with the string representing the HttpRequest's body.</returns>
        private async Task<string> readRequestBody(HttpRequest request)
        {
            string requestBodyContent = "";

            int bufferSize = Convert.ToInt32(request.ContentLength);

            //Check is necessary in order to avoid errors when rewinding
            if (bufferSize > 0)
            {
                //Allows for the stream to be used multiple times
                request.EnableRewind();

                //Allows for large requests to be temporarily stored in the disk
                request.EnableBuffering();

                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, bufferSize, true))
                {
                    requestBodyContent = await reader.ReadToEndAsync();
                }

                //rewind the stream back to the start
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            return requestBodyContent;
        }


        /// <summary>
        /// Reads the HttpResponse's body.
        /// </summary>
        /// <param name="response">HttpResponse with the body being read.</param>
        /// <returns>Task with the string representing the HttpResponse's body.</returns>
        private async Task<string> readResponseBody(HttpResponse response)
        {
            //read response from the beginning
            response?.Body.Seek(0, SeekOrigin.Begin);

            string responseBodyContent = responseBodyContent = await new StreamReader(response?.Body).ReadToEndAsync();

            //reset the stream so that the response can be read again
            response?.Body.Seek(0, SeekOrigin.Begin);

            return responseBodyContent;
        }


        /// <summary>
        /// Logs any exception that occurs while executing the Middleware.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="stopWatch"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private bool logException(HttpContext httpContext, Stopwatch stopWatch, Exception exception)
        {
            stopWatch.Stop();

            logForErrorContext(httpContext)
                .Error(exception, messageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, stopWatch.Elapsed.TotalMilliseconds);

            return false;
        }


        private static ILogger logForWarningContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = logger
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }


        private static ILogger logForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = logger
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }

    }


    /// <summary>
    /// Static class representing SerilogMiddlewareExtensions.
    /// </summary>
    public static class SerilogMiddlewareExtensions
    {

        /// <summary>
        /// Registers the SerilogMiddleware in the Application.
        /// </summary>
        /// <param name="builder">Instance of IAppucationBuilder, building the Application.</param>
        /// <returns>IApplicationBuilder with the SerilogMiddleware attached to it.</returns>
        public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }

    }
}