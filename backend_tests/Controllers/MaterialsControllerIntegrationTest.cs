using System;
using Xunit;
using System.Net.Http;
using backend_tests.Setup;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using backend_tests.utils;
using System.Linq;
using core.dto;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for Materials Collection API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    [Collection("Integration Collection")]
    [TestCaseOrderer("backend_tests.Setup.TestPriorityOrderer", "backend_tests")]
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

        //!Test is failing due to priorities not having any effect on the execution order
        [Fact, TestPriority(0)]
        public async Task ensureGetAllMaterialsSendsBadRequestWhenListIsEmpty()
        {
            var response = await client.GetAsync(urlBase);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseString);
        }

        [Fact, TestPriority(1)]
        public async Task ensurePostMaterialFailsWithEmptyRequestBody()
        {
            var response = await client.PostAsJsonAsync(urlBase, "{}");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact, TestPriority(2)]
        public async Task<MaterialDTO> ensurePostMaterialWorks()
        {
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.designation = "mdf";
            materialDTO.reference = "bananas" + Guid.NewGuid().ToString("n");
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lilxan";
            colorDTO.red = 100;
            colorDTO.green = 200;
            colorDTO.blue = 10;
            colorDTO.alpha = 0;
            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = "ola";
            materialDTO.colors = new List<ColorDTO>(new[] { colorDTO });
            materialDTO.finishes = new List<FinishDTO>(new[] { finishDTO });
            var response = await client.PostAsJsonAsync(urlBase, materialDTO);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            return JsonConvert.DeserializeObject<MaterialDTO>(await response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(3)]
        public async Task ensureGetAllMaterialsSendsOkMessageWithMaterialsCollection()
        {

            Task<MaterialDTO> materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.GetAsync(urlBase);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(materialDTO.Result.id, JsonConvert.DeserializeObject<List<MaterialDTO>>(await response.Content.ReadAsStringAsync()).ElementAt(1).id);
        }

        [Fact, TestPriority(4)]
        public async Task ensureGetMaterialWithIDSendsOkMessageWithExistingMaterial()
        {

            Task<MaterialDTO> materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.GetAsync(String.Format(urlBase + "/{0}", materialDTO.Result.id));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(materialDTO.Result.id, JsonConvert.DeserializeObject<MaterialDTO>(await response.Content.ReadAsStringAsync()).id);
        }

        [Fact, TestPriority(5)]
        public async Task ensureGetMaterialSendsBadRequestWithNonExistingMaterial()
        {
            var response = await client.GetAsync(String.Format(urlBase + "/{0}", -1));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(6)]
        public async Task ensurePostFailsWithInvalidBody()
        {
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.designation = "mdf" + DateTime.Now;

            var response = await client.PostAsJsonAsync(urlBase, materialDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(7)]
        public async Task ensureDeleteFailsWithNonExistingMaterial()
        {
            var response = await client.DeleteAsync(String.Format(urlBase + "/{0}", -1));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(8)]
        public async Task ensureDeleteSucceedsWithExistingMaterial()
        {
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.DeleteAsync(String.Format(urlBase + "/{0}", materialDTO.Id));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, TestPriority(9)]
        public async Task ensureBasicInfoPutFailsForNonExistingMaterial()
        {
            UpdateMaterialDTO materialDTO = new UpdateMaterialDTO();
            materialDTO.designation = "updated designation";
            materialDTO.reference = "updated reference";
            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}", -1), materialDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(10)]
        public async Task ensureBasicInfoPutFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();
            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}", materialDTO.Id), "invalid body");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(11)]
        public async Task ensureBasicInfoPutSucceedsForValidBody()
        {
            var updatedDTO = new UpdateMaterialDTO();
            updatedDTO.designation = "new designation";
            updatedDTO.reference = "new reference";
            var materialDTO = ensurePostMaterialWorks();
            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}", materialDTO.Id), updatedDTO);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(12)]
        public async Task ensureUpdateFinishesPutFailsForNonExistingMaterial()
        {
            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = "ola";
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.finishesToAdd = new List<FinishDTO>(new[] { finishDTO });
            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}", -1), materialUpdates);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }


        public async Task ensureUpdateFinishesPutFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();

        }


    }
}