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
using Microsoft.AspNetCore.Mvc.Testing;

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
        private const string urlBase = "/mycm/api/materials";

        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient client;

        private TestFixture<TestStartupSQLite> fixture;


        /// <summary>
        /// Builds a MaterialsControllerIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public MaterialsControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.client = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
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
            ColorDTO otherColorDTO = new ColorDTO();
            otherColorDTO.name = "another color";
            otherColorDTO.red = 10;
            otherColorDTO.green = 10;
            otherColorDTO.blue = 10;
            otherColorDTO.alpha = 10;
            FinishDTO finishDTO = new FinishDTO();
            FinishDTO otherFinishDTO = new FinishDTO();
            finishDTO.description = "ola";
            otherFinishDTO.description = "another finish";
            materialDTO.colors = new List<ColorDTO>(new[] { colorDTO, otherColorDTO });
            materialDTO.finishes = new List<FinishDTO>(new[] { finishDTO, otherFinishDTO });
            var response = await client.PostAsJsonAsync(urlBase, materialDTO);

            MaterialDTO materialDTOFromPost = JsonConvert.DeserializeObject<MaterialDTO>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.True(materialDTO.id != -1);
            Assert.Equal(materialDTO.reference, materialDTOFromPost.reference);
            Assert.NotNull(materialDTOFromPost.colors);
            Assert.NotNull(materialDTOFromPost.finishes);
            Assert.Equal(materialDTO.designation, materialDTOFromPost.designation);

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
        public async Task ensureUpdateFinishesPostFailsForNonExistingMaterial()
        {
            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = "ola";
           
            var response = await client.PostAsJsonAsync(String.Format(urlBase + "/{0}/{1}", -1, "finishes"), finishDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(13)]
        public async Task ensureUpdateFinishesPostFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PostAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), "InvalidBody");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(14)]
        public async Task ensureUpdateFinishesPostSucceedsWhenAddingFinish()
        {
            FinishDTO finishDTOToAdd = new FinishDTO();
            finishDTOToAdd.description = "new finish";
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PostAsJsonAsync(String.
            Format(urlBase + "/{0}/{1}", materialDTO.Id, "finishes"), finishDTOToAdd);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(15)]
        public async Task ensureUpdateFinishesDeleteSucceedsWhenRemovingFinish()
        {
            FinishDTO finishDTOToRemove = new FinishDTO();
            finishDTOToRemove.description = "ola";
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.DeleteAsync(String.
            Format(urlBase + "/{0}/{1}/{2}", materialDTO.Id, "finishes", finishDTOToRemove.id));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(16)]
        public async Task ensureUpdateColorPostFailsForNonExistingMaterial()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lean";
            colorDTO.red = 1;
            colorDTO.green = 2;
            colorDTO.blue = 3;
            colorDTO.alpha = 0;
           
            var response = await client.PostAsJsonAsync(String.Format(urlBase + "/{0}/{1}", -1, "colors"), colorDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(17)]
        public async Task ensureUpdateColorPostFailsForInvalidBody()
        {
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PostAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), "invalid body");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(18)]
        public async Task ensureUpdateColorPostSucceedsWhenAddingColor()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lean";
            colorDTO.red = 1;
            colorDTO.green = 2;
            colorDTO.blue = 3;
            colorDTO.alpha = 0;
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.PostAsJsonAsync(String.Format(urlBase + "/{0}/{1}", materialDTO.Id, "colors"), colorDTO);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(19)]
        public async Task ensureUpdateColorsDeleteSucceedsWhenOnlyRemovingColor()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lilxan";
            colorDTO.red = 100;
            colorDTO.green = 200;
            colorDTO.blue = 10;
            colorDTO.alpha = 0;
            var materialDTO = ensurePostMaterialWorks();
            materialDTO.Wait();

            var response = await client.DeleteAsync(String.Format(urlBase + "/{0}/{1}/{2}", materialDTO.Id, "colors", colorDTO.id));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }
    
         [Fact, TestPriority(20)]
        public async Task ensureUpdateFinishPostFailsForNonExistingMaterial()
        {
            FinishDTO finishDTOToRemove = new FinishDTO();
            finishDTOToRemove.description = "ola";
           
            var response = await client.DeleteAsync(String.Format(urlBase + "/{0}/{1}/{2}", -1, "colors", finishDTOToRemove.id));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }
         [Fact, TestPriority(21)]
        public async Task ensureUpdateColorDeleteFailsForNonExistingMaterial()
        {
            ColorDTO colorDTO = new ColorDTO();
            colorDTO.name = "lean";
            colorDTO.red = 1;
            colorDTO.green = 2;
            colorDTO.blue = 3;
            colorDTO.alpha = 0;
           
            var response = await client.DeleteAsync(String.Format(urlBase + "/{0}/{1}/{2}", -1, "colors", colorDTO.id));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(response.Content.ReadAsStringAsync());
        }
     }
 }