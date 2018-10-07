using System;
using Xunit;
using System.Net.Http;
using backend_tests.Setup;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using backend_tests.utils;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for Materials Collection API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    public class MaterialsControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        /// <summary>
        /// Materials URI for HTTP Requests
        /// </summary>
        private const string urlBase = "/myc/api/materials";

        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Builds a MaterialsControllerIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public MaterialsControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            client = fixture.httpClient;
        }

        [Fact]
        public async Task ensureGetAllMaterialsSendsBadRequestWhenListIsEmpty()
        {

            var response = await client.GetAsync(urlBase);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Object>(responseString);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.NotNull(result);
        }

        //!Test is failing. NullReferenceException at line 61 of core/application/MaterialsController
        //TODO Assert that response message is the correct one?
        [Fact]
        public async Task ensurePostMaterialFailsWithEmptyRequestBody(){
            var response = await client.PostAsJsonAsync(urlBase,"{}");

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

    }
}