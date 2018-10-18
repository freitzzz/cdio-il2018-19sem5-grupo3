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
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
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

            MaterialDTO materialDTOFromPost = JsonConvert.DeserializeObject<MaterialDTO>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.True(materialDTO.id != -1);
            Assert.Equal(materialDTO.reference, materialDTOFromPost.reference);

            return materialDTOFromPost;
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
            materialDTO.Wait();

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

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", -1, "finishes"), materialUpdates);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(13)]
        public async Task ensureUpdateFinishesPutFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), "invalid body");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(14)]
        public async Task ensureUpdateFinishesPutSucceedsWhenOnlyAddingFinishes()
        {
            FinishDTO finishDTOToAdd = new FinishDTO();
            finishDTOToAdd.description = "new finish";
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.finishesToAdd = new List<FinishDTO>(new[] { finishDTOToAdd });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(15)]
        public async Task ensureUpdateFinishesPutSucceedsWhenOnlyRemovingFinishes()
        {
            FinishDTO finishDTOToRemove = new FinishDTO();
            finishDTOToRemove.description = "ola";
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.finishesToRemove = new List<FinishDTO>(new[] { finishDTOToRemove });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(16)]
        public async Task ensureUpdateFinishesPutSucceedsWhenAddingAndRemovingFinishes()
        {
            FinishDTO finishDTOToAdd = new FinishDTO();
            FinishDTO finishDTOToRemove = new FinishDTO();
            finishDTOToAdd.description = "new finish";
            finishDTOToRemove.description = "ola";
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.finishesToAdd = new List<FinishDTO>(new[] { finishDTOToAdd });
            materialUpdates.finishesToRemove = new List<FinishDTO>(new[] { finishDTOToRemove });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(16)]
        public async Task ensureUpdateColorsPutFailsForNonExistingMaterial()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lean";
            colorDTO.red = 1;
            colorDTO.green = 2;
            colorDTO.blue = 3;
            colorDTO.alpha = 0;
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.colorsToAdd = new List<ColorDTO>(new[] { colorDTO });

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", -1, "colors"), materialUpdates);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(17)]
        public async Task ensureUpdateColorsPutFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), "invalid body");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(18)]
        public async Task ensureUpdateColorsPutSucceedsWhenOnlyAddingColors()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lean";
            colorDTO.red = 1;
            colorDTO.green = 2;
            colorDTO.blue = 3;
            colorDTO.alpha = 0;
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.colorsToAdd = new List<ColorDTO>(new[] { colorDTO });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(19)]
        public async Task ensureUpdateColorsPutSucceedsWhenOnlyRemovingColors()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lilxan";
            colorDTO.red = 100;
            colorDTO.green = 200;
            colorDTO.blue = 10;
            colorDTO.alpha = 0;
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.colorsToRemove = new List<ColorDTO>(new[] { colorDTO });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(20)]
        public async Task ensureUpdateColorsPutSucceedsWhenAddingAndRemovingColors()
        {
            ColorDTO colorDTOToAdd = new ColorDTO();
            colorDTOToAdd.name = "lean";
            colorDTOToAdd.red = 2;
            colorDTOToAdd.green = 3;
            colorDTOToAdd.blue = 4;
            colorDTOToAdd.alpha = 0;
            ColorDTO colorDTOToRemove = new ColorDTO();
            colorDTOToRemove.name = "lilxan";
            colorDTOToRemove.red = 100;
            colorDTOToRemove.green = 200;
            colorDTOToRemove.blue = 10;
            colorDTOToRemove.alpha = 0;
            var materialUpdates = new UpdateMaterialDTO();
            materialUpdates.colorsToAdd = new List<ColorDTO>(new[] { colorDTOToAdd });
            materialUpdates.colorsToRemove = new List<ColorDTO>(new[] { colorDTOToRemove });
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PutAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), materialUpdates);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }
    }
}