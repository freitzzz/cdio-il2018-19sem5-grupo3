using System;
using Xunit;
using System.Net.Http;
using backend_tests.Setup;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using backend_tests.utils;
using core.dto;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for Materials Collection API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    [Collection("Integration Collection")]
    [TestCaseOrderer("backend_tests.Setup.PriorityOrderer","backend_tests.Setup")]
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

        [Fact, TestPriority(0)]
        public async Task ensureGetAllMaterialsSendsBadRequestWhenListIsEmpty()
        {
            var response = await client.GetAsync(urlBase);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Object>(responseString);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.NotNull(result);
        }

        [Fact, TestPriority(1)]
        public async Task ensurePostMaterialFailsWithEmptyRequestBody()
        {
            var response = await client.PostAsJsonAsync(urlBase, "{}");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact, TestPriority(2)]
        public async Task<MaterialDTO> ensurePostMaterialWorks(){
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.designation = "mdf"+DateTime.Now;
            materialDTO.reference = "bananas"+DateTime.Now;
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lilxan";
            colorDTO.red = 100;
            colorDTO.green = 200;
            colorDTO.blue = 10;
            colorDTO.alpha = 0;
            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description="ola";
            materialDTO.colors = new List<ColorDTO>(new []{colorDTO});
            materialDTO.finishes = new List<FinishDTO>(new []{finishDTO});
            var response = await client.PostAsJsonAsync(urlBase,materialDTO);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return JsonConvert.DeserializeObject<MaterialDTO>(await response.Content.ReadAsStringAsync());
        }
    }
}