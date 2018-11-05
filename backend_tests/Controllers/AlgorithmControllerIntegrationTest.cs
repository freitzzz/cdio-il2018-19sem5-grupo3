using backend_tests.Setup;
using core.domain;
using core.dto;
using core.modelview.algorithm;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend_tests.Controllers {
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public class AlgorithmControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>> {
        /// <summary>
        /// Materials URI for HTTP Requests
        /// </summary>
        private const string urlBase = "/mycm/api/algorithms";
        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient client;
        private TestFixture<TestStartupSQLite> fixture;
        /// <summary>
        /// Builds a MaterialsControllerIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public AlgorithmControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture) {
            this.fixture = fixture;
            this.client = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }
        [Fact]
        public async Task ensureGetAllAlgorithmsSucceeds() {
            var response = await client.GetAsync(urlBase);
            var responseString = await response.Content.ReadAsStringAsync();
            List<GetBasicAlgorithmModelView> list = (List<GetBasicAlgorithmModelView>)JsonConvert.DeserializeObject<IEnumerable<GetBasicAlgorithmModelView>>(responseString);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Enum.GetValues(typeof(RestrictionAlgorithm)).Length, list.Count);
        }
        [Fact]
        public async Task ensureGetAlgorithmSucceeds() {
            var response = await client.GetAsync(urlBase + "/" + (int)RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM);
            var responseString = await response.Content.ReadAsStringAsync();
            AlgorithmDTO dto = JsonConvert.DeserializeObject<AlgorithmDTO>(responseString);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AlgorithmAttributes.getName(RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM), dto.name);
        }
        [Fact]
        public async Task ensureGetAlgorithmReturnsBadRequestIfAlgorithmDoesNotExist() {
            var response = await client.GetAsync(urlBase + "/" + Enum.GetValues(typeof(RestrictionAlgorithm)).Length + 1);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ensureGetAlgorithmInputsSucceeds() {
            var response = await client.GetAsync(urlBase + "/" + (int)RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM + "/inputs");
            var responseString = await response.Content.ReadAsStringAsync();
            List<InputDTO> list = (List<InputDTO>)JsonConvert.DeserializeObject<IEnumerable<InputDTO>>(responseString);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM).getRequiredInputs().Count, list.Count);
        }
        [Fact]
        public async Task ensureGetAlgorithmInputsReturnsBadRequestIfAlgorithmDoesNotRequireInputs() {
            var response = await client.GetAsync(urlBase + "/" + (int)RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM + "/inputs");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ensureGetAlgorithmInputsReturnsBadRequestIfAlgorithmDoesNotExist() {
            var response = await client.GetAsync(urlBase + "/" + Enum.GetValues(typeof(RestrictionAlgorithm)).Length + 1 + "/inputs");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
