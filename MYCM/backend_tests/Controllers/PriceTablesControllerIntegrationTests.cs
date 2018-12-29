using backend_tests.Setup;
using backend_tests.utils;
using core.dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using core.modelview.pricetable;
using core.modelview.pricetableentries;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for PriceTables Collection API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public class PriceTablesControllerIntegrationTests : IClassFixture<TestFixture<TestStartupSQLite>>
    {
        /// <summary>
        /// Materials URI for HTTP Requests
        /// </summary>
        private const string BASE_URI = "/mycm/api/prices";

        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Test Fixture
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;


        /// <summary>
        /// Builds a MaterialsControllerIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public PriceTablesControllerIntegrationTests(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        [Fact, TestPriority(-12)]
        public async void ensureGetMaterialPriceHistoryByIdReturnsNotFoundIfCollectionIsEmpty()
        {
            MaterialDTO createdMaterial = await createMaterial("-12");

            var response = await httpClient.GetAsync(BASE_URI + "/materials/" + createdMaterial.id);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message

            await httpClient.DeleteAsync("mycm/api/materials/" + createdMaterial.id);
        }

        [Fact, TestPriority(-11)]
        public async void ensureGetMaterialPriceHistoryByIdReturnsNotFoundIfMaterialsCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/materials/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }


        [Fact, TestPriority(-10)]
        public async void ensureGetAllMaterialsPriceHistoryReturnsNotFoundIfMaterialsCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/materials");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-9)]
        public async void ensureGetAllMaterialFinishesPriceHistoryReturnsNotFoundIfCollectionIsEmpty()
        {
            MaterialDTO createdMaterial = await createMaterial("-9");

            var response = await httpClient.GetAsync(BASE_URI + "/materials/" + createdMaterial.id + "/finishes");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message

            await httpClient.DeleteAsync("mycm/api/materials/" + createdMaterial.id);
        }

/*         [Fact, TestPriority(-8)]
        public async void ensureGetAllMaterialsPriceHistoryReturnsOkIfTheresContent()
        {
            string testNumber = "-8";

            MaterialDTO createdMaterial = await createMaterial(testNumber + " first material");
            MaterialDTO otherCreatedMaterial = await createMaterial(testNumber + " second material");
            AddPriceTableEntryModelView addPriceTableEntry =
                createPriceTableEntry(-8);


            var createMaterialPriceTableEntry =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/materials/" + createdMaterial.id,
                    addPriceTableEntry
                );

            var otherCreateMaterialPriceTableEntry =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/materials/" + otherCreatedMaterial.id,
                    addPriceTableEntry
                );

            Assert.Equal(HttpStatusCode.Created, otherCreateMaterialPriceTableEntry.StatusCode);
            //TODO Check why the creation of this entry fails
            //Assert.Equal(HttpStatusCode.Created, createMaterialPriceTableEntry.StatusCode);

            var getAll = await httpClient.GetAsync(BASE_URI + "/materials");

            Assert.Equal(HttpStatusCode.OK, getAll.StatusCode);

            GetAllMaterialPriceHistoryModelView responseContent =
                await getAll.Content.ReadAsAsync<GetAllMaterialPriceHistoryModelView>();

            Assert.Equal(addPriceTableEntry.priceTableEntry.price.value,
                        responseContent[0].value);
            Assert.Equal(addPriceTableEntry.priceTableEntry.price.currency,
                        responseContent[0].currency);
            Assert.Equal(addPriceTableEntry.priceTableEntry.price.area,
                        responseContent[0].area);
            Assert.Equal(addPriceTableEntry.priceTableEntry.startingDate,
                        responseContent[0].startingDate);
            Assert.Equal(addPriceTableEntry.priceTableEntry.endingDate,
                        responseContent[0].endingDate);
            Assert.Equal(otherCreatedMaterial.id, responseContent[0].materialId);
            Assert.Equal(addPriceTableEntry.priceTableEntry.price.value,
                        responseContent[1].value);
            Assert.Equal(addPriceTableEntry.priceTableEntry.price.currency,
                        responseContent[1].currency);
            Assert.Equal(addPriceTableEntry.priceTableEntry.price.area,
                        responseContent[1].area);
            Assert.Equal(addPriceTableEntry.priceTableEntry.startingDate,
                        responseContent[1].startingDate);
            Assert.Equal(addPriceTableEntry.priceTableEntry.endingDate,
                        responseContent[1].endingDate);
            Assert.Equal(otherCreatedMaterial.id, responseContent[1].materialId);
        } */

/*         [Fact, TestPriority(-7)]
        public async void ensureGetMaterialPriceHistoryByIdReturnsNotFoundForNonExistingId()
        {
            string testNumber = "-7";

            MaterialDTO otherCreatedMaterial = await createMaterial(testNumber);
            AddPriceTableEntryModelView addPriceTableEntry =
                createPriceTableEntry(-7);

            var otherCreateMaterialPriceTableEntry =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/materials/" + otherCreatedMaterial.id,
                    addPriceTableEntry
                );

            Assert.Equal(HttpStatusCode.Created, otherCreateMaterialPriceTableEntry.StatusCode);

            var getById =
                await httpClient.GetAsync(BASE_URI + "/materials/" + otherCreatedMaterial.id + 1);

            Assert.Equal(HttpStatusCode.NotFound, getById.StatusCode);
        } */

       /*  [Fact, TestPriority(-6)]
        public async void ensureGetMaterialPriceHistoryByIdReturnsOk()
        {
            string testNumber = "-6";

            MaterialDTO otherCreatedMaterial = await createMaterial(testNumber);
            AddPriceTableEntryModelView addPriceTableEntry =
                createPriceTableEntry(-6);

            var otherCreateMaterialPriceTableEntry =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/materials/" + otherCreatedMaterial.id,
                    addPriceTableEntry
                );

            Assert.Equal(HttpStatusCode.Created, otherCreateMaterialPriceTableEntry.StatusCode);

            var getById =
                await httpClient.GetAsync(BASE_URI + "/materials/" + otherCreatedMaterial.id);

            Assert.Equal(HttpStatusCode.OK, getById.StatusCode);

            GetAllMaterialPriceHistoryModelView responseContent =
                await getById.Content.ReadAsAsync<GetAllMaterialPriceHistoryModelView>();

            assertPriceTableEntry(addPriceTableEntry, responseContent[0]);
        }

        private void assertPriceTableEntry(AddPriceTableEntryModelView expectedModelView, GetMaterialPriceModelView actualModelView)
        {
            Assert.Equal(expectedModelView.priceTableEntry.startingDate,
                        actualModelView.startingDate);
            Assert.Equal(expectedModelView.priceTableEntry.endingDate,
                        actualModelView.endingDate);
            Assert.Equal(expectedModelView.priceTableEntry.price.currency,
                        actualModelView.currency);
            Assert.Equal(expectedModelView.priceTableEntry.price.area,
                        actualModelView.area);
            Assert.Equal(expectedModelView.priceTableEntry.price.value,
                        actualModelView.value);
        } */

        private AddPriceTableEntryModelView createPriceTableEntry(double testNumber)
        {
            AddPriceTableEntryModelView priceTableEntry =
                new AddPriceTableEntryModelView();

            priceTableEntry.priceTableEntry = new PriceTableEntryDTO();
            priceTableEntry.priceTableEntry.price = new PriceDTO();
            priceTableEntry.priceTableEntry.price.currency = "EUR";
            priceTableEntry.priceTableEntry.price.value = 1000 + testNumber;
            priceTableEntry.priceTableEntry.price.area = "m2";
            double year = 2016 - testNumber;
            priceTableEntry.priceTableEntry.startingDate = year + "-01-22T12:04:00";
            priceTableEntry.priceTableEntry.endingDate = year + "-05-22T12:04:00";

            return priceTableEntry;
        }

        private async Task<MaterialDTO> createMaterial(string testNumber)
        {
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.designation = "Material Designation Test" + testNumber;
            materialDTO.reference = "Material Reference Test" + testNumber;
            materialDTO.image = "Material Image Test" + testNumber + ".jpeg";
            ColorDTO colorDTO = new ColorDTO();
            ColorDTO otherColorDTO = new ColorDTO();
            FinishDTO finishDTO = new FinishDTO();
            FinishDTO otherFinishDTO = new FinishDTO();
            colorDTO.red = 100;
            colorDTO.blue = 100;
            colorDTO.green = 100;
            colorDTO.alpha = 10;
            colorDTO.name = "Color Test" + testNumber;
            otherColorDTO.red = 150;
            otherColorDTO.blue = 150;
            otherColorDTO.green = 150;
            otherColorDTO.alpha = 100;
            otherColorDTO.name = "Other Color Test" + testNumber;
            finishDTO.description = "Finish Test" + testNumber;
            otherFinishDTO.description = "Other Finish Test" + testNumber;
            materialDTO.colors = new List<ColorDTO>() { colorDTO, otherColorDTO };
            materialDTO.finishes = new List<FinishDTO>() { finishDTO, otherFinishDTO };

            var response = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            MaterialDTO createdMaterial =
                await response.Content.ReadAsAsync<MaterialDTO>();

            return createdMaterial;
        }
    }
}