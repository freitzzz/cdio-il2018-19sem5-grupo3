using Newtonsoft.Json;
using System.Net.Http;

namespace backend_tests.utils{
    /// <summary>
    /// Utility class for creating HTTPContent objects
    /// </summary>
    public sealed class HTTPContentCreator{
        /// <summary>
        /// Creates an HTTPContent with a certain data as application/json
        /// </summary>
        /// <param name="contentToTransform">object with the content to be transformed in JSON</param>
        /// <returns>HTTPContent with the created content as JSON</returns>
        public static HttpContent contentAsJSON(object contentToTransform){
            HttpContent content=new StringContent(JsonConvert.SerializeObject(contentToTransform));
            content.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}