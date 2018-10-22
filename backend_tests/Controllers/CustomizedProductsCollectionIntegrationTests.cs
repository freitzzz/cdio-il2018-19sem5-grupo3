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
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace backend_tests.Controllers
{

    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class CustomizedProductsCollectionControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string CUSTOMIZED_PRODUCTS_COLLECTION_URI = "myc/api/collections";
        /// <summary>
        /// Injected Mock Server
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;
        /// <summary>
        /// Current HTTP Client
        /// </summary>
        private HttpClient httpClient;
        /// <summary>
        /// Builds a new CustomizedProductsCollectionControllerIntegrationTest with the mocked server injected by parameters
        /// </summary>
        /// <param name="fixture">Injected Mocked Server</param>
        public CustomizedProductsCollectionControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        /// <summary>
        /// Ensures that a customized product collection is created succesfuly
        /// </summary>
        [Fact, TestPriority(1)]
        public async Task<CustomizedProductCollectionDTO> ensureCustomizedProductCollectionIsCreatedSuccesfuly()
        {
            CustomizedProductCollectionDTO customizedProductCollectionDTO = new CustomizedProductCollectionDTO();
            //A collection of customized products requires a valid name
            customizedProductCollectionDTO.name = "Braga" + Guid.NewGuid().ToString("n");
            //A collection of customized products can be created with only a name
            //But if needed customized products for it, uncomment the lines below
            //Task<CustomizedProductDTO> customizedProductDTO=new CustomizedProductControllerIntegrationTest(fixture).ensureCustomizedProductIsCreatedSuccesfuly();
            //customizedProductDTO.Wait();
            //customizedProductCollectionDTO.customizedProducts=new List<CustomizedProductDTO>(new []{customizedProductDTO.Result});
            var createCustomizedProductsCollection = await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI, customizedProductCollectionDTO);
            //Uncomment when slots creation is fixed
            Assert.True(createCustomizedProductsCollection.StatusCode == HttpStatusCode.Created);
            return JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await createCustomizedProductsCollection.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(2)]
        public async Task<CustomizedProductCollectionDTO> ensureCustomizedProductCollectionWithCustomizedProductsIsCreatedSuccessfully()
        {
            //Create a new CustomizedProduct that will be added to the Collection
            CustomizedProductDTO customizedProductDTO = await new CustomizedProductControllerIntegrationTest(fixture).ensureCustomizedProductIsCreatedSuccesfuly();

            //when adding new customized products to the collection, only the id is specified.
            CustomizedProductDTO customizedProductDTOWithJustID = new CustomizedProductDTO {id = customizedProductDTO.id};


            CustomizedProductCollectionDTO customizedProductCollectionDTO = new CustomizedProductCollectionDTO();
            customizedProductCollectionDTO.name = "Porto" + Guid.NewGuid().ToString("n");
            customizedProductCollectionDTO.customizedProducts = new List<CustomizedProductDTO>() {customizedProductDTOWithJustID};

            var createCustomizedProductsCollection = await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI, customizedProductCollectionDTO);
            Assert.True(createCustomizedProductsCollection.StatusCode == HttpStatusCode.Created);
            return JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await createCustomizedProductsCollection.Content.ReadAsStringAsync());
        }

    }
}