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
using core.domain;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for Commercial Catalogue API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public class CommercialCatalogueIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {
        /// <summary>
        /// Materials URI for HTTP Requests
        /// </summary>
        private const string urlBase = "/myc/api/commercialcatalogues";

        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Injected Mock Server
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;

        /// <summary>
        /// Builds a CommercialCatalogueIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public CommercialCatalogueIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            client = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        [Fact, TestPriority(1)]
        public async Task ensurePostCommercialCatalogueFailsWithEmptyRequestBody()
        {
            var response = await client.PostAsJsonAsync(urlBase, "{}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task<CommercialCatalogueDTO> ensurePostCommercialCatalogueWithNoCollectionsReturnsCreated()
        {
            CommercialCatalogueDTO commercialCatalogue = new CommercialCatalogueDTO();
            commercialCatalogue.reference = "6" + Guid.NewGuid().ToString("n");
            commercialCatalogue.designation = " Catalogue" + Guid.NewGuid().ToString("n");

            var response = await client.PostAsJsonAsync(urlBase, commercialCatalogue);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            return JsonConvert.DeserializeObject<CommercialCatalogueDTO>(await response.Content.ReadAsStringAsync());
        }

        [Fact, TestPriority(3)]
        public async Task<CommercialCatalogueDTO> ensurePostCommercialCatalogueWithCollectionsReturnsCreated()
        {

            CustomizedProductCollectionDTO collectionDTO = await new CustomizedProductsCollectionControllerIntegrationTest(fixture).ensureCanCreateACustomizedProductCollectionIfItHasAValidNameAndValidCustomizedProducts();

            //only the collection's ID needs to be specified in the DTO
            CustomizedProductCollectionDTO collectionDTOWithJustID = new CustomizedProductCollectionDTO() { id = collectionDTO.id };

            //only the customized product's ID needst to be specified in the DTO
            CustomizedProductDTO customizedProductDTOWithJustID = new CustomizedProductDTO() { id = collectionDTO.customizedProducts.FirstOrDefault().id };


            //Build a CatalogueCollectionDTO with just the identifiers
            CatalogueCollectionDTO catalogueCollectionDTO = new CatalogueCollectionDTO();
            catalogueCollectionDTO.customizedProductCollectionDTO = collectionDTOWithJustID;
            catalogueCollectionDTO.customizedProductDTOs = new List<CustomizedProductDTO>() { customizedProductDTOWithJustID };


            //Build CommercialCatalogueDTO with the previously built CatalogueCollectionDTO
            CommercialCatalogueDTO commercialCatalogue = new CommercialCatalogueDTO();
            commercialCatalogue.reference = "7" + Guid.NewGuid().ToString("n");
            commercialCatalogue.designation = " Catalogue" + Guid.NewGuid().ToString("n");
            commercialCatalogue.catalogueCollectionDTOs = new List<CatalogueCollectionDTO>() { catalogueCollectionDTO };

            var response = await client.PostAsJsonAsync(urlBase, commercialCatalogue);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            CommercialCatalogueDTO createdCatalogueDTO = JsonConvert.DeserializeObject<CommercialCatalogueDTO>(await response.Content.ReadAsStringAsync());

            return createdCatalogueDTO;
        }


        [Fact, TestPriority(4)]
        public async Task<CommercialCatalogueDTO> ensurePutCatalogueCollectionWorks()
        {
            /* 
                        Task<CommercialCatalogueDTO> commercialCatalogueDTOTask = ensurePostCommercialCatalogueWorks();
                        commercialCatalogueDTOTask.Wait();
                        CommercialCatalogueDTO commercialCatalogueDTO = commercialCatalogueDTOTask.Result;

                        long id = commercialCatalogueDTO.id;
                        CatalogueCollectionDTO catalogueCollectionDTO = new CatalogueCollectionDTO();
                         */

            //TODO:WAIT FOR IMPLEMENTATION OF OTHER INTEGRATION TESTS

            ///catalogueCollectionDTO.customizedProductCollectionDTO = CustomizedProductCollectionIn

            //List<CustomizedProductDTO> customizedProductDTOs=new List<CustomizedProductDTO>();

            //var response = await client.PutAsJsonAsync(urlBase+"/"+id+"/collections", catalogueCollectionDTO);

            //Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //return JsonConvert.DeserializeObject<CommercialCatalogueDTO>(await response.Content.ReadAsStringAsync());
            return null;
        }

    }
}