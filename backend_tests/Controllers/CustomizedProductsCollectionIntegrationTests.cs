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
        /// Ensures that's not possible to create a customized product collection with an invalid name (null/empty)
        /// </summary>
        [Fact, TestPriority(0)]
        public async void ensureCantCreateACustomizedProductCollectionIfItHasAnInvalidName(){
            //We are attempting to create a customized product collection with an invalid name
            CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
            //Our first attempt will have the name a "null" reference
            customizedProductCollectionDTO.name=null;
            var createCustomizedProductCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI
                                                                                ,customizedProductCollectionDTO);
            //The result should be a bad request since it's not possible to create a customized product with a "null" name
            Assert.Equal(HttpStatusCode.BadRequest,createCustomizedProductCollection.StatusCode);
            
            //Our second attempt will have the name as an empty string
            customizedProductCollectionDTO.name="";
            createCustomizedProductCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI
                                                                                ,customizedProductCollectionDTO);
            //The result should be a bad request since it's not possible to create a customized product with an "empty" name
            Assert.Equal(HttpStatusCode.BadRequest,createCustomizedProductCollection.StatusCode);
            
            Console.WriteLine("\001b[0;32m"+"ensureCantCreateACustomizedProductCollectionIfItHasAnInvalidName"+"\u001b[0m");
        }

        /// <summary>
        /// Ensures that's not possible to create a customized product collection which already exists (name (identity) already exists)
        /// </summary>
        [Fact, TestPriority(1)]
        public async void ensureCantCreateACustomizedProductCollectionIfItAlreadyExists(){
            //We are attempting to create a customized product collection which already exists
            //First we need to grant that a product with a certain name already exists
            Task<CustomizedProductCollectionDTO> validCustomizedProductCollectionDTOTask=ensureCustomizedProductCollectionIsCreatedSuccesfuly();
            validCustomizedProductCollectionDTOTask.Wait();
            
            CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
            //Then we will apply the name of the existing product to the customized product collection
            customizedProductCollectionDTO.name=validCustomizedProductCollectionDTOTask.Result.name;
            var createCustomizedProductCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI
                                                                                    ,customizedProductCollectionDTO);
            //The result should be a bad request since it's not possible to create a customized product which already exists
            Assert.Equal(HttpStatusCode.BadRequest,createCustomizedProductCollection.StatusCode);
            
            Console.WriteLine("\001b[0;32m"+"ensureCantCreateACustomizedProductCollectionIfItAlreadyExists"+"\u001b[0m");
        }

        /// <summary>
        /// Ensures that's not possible to create a customized product collection which aggregated customized products are invalid (empty/dont exist)
        /// </summary>
        [Fact, TestPriority(2)]
        public async void ensureCantCreateACustomizedProductCollectionIfItAggregatesInvalidCustomizedProducts(){
            //We are attempting to create a customized product collection which aggregated customized products are invalid
            CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
            //We first need to generate an atomic valid name for the customized products collection
            customizedProductCollectionDTO.name=Guid.NewGuid().ToString("n");

            //In our first attempt we will try to create a customized product collection with "empty" customized products
            customizedProductCollectionDTO.customizedProducts=new List<CustomizedProductDTO>();

            var createCustomizedProductCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI
                                                                                    ,customizedProductCollectionDTO);
            //The result should be a bad request since it's not possible to create a customized product collection with "empty" customized products
            Assert.Equal(HttpStatusCode.BadRequest,createCustomizedProductCollection.StatusCode);

            //In our second and last attempt we will try to create a customized product collection with customized products which don't exist (resources don't exist)
            customizedProductCollectionDTO.customizedProducts=new List<CustomizedProductDTO>(new []{new CustomizedProductDTO()});

            createCustomizedProductCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI
                                                                                    ,customizedProductCollectionDTO);
            //The result should be a bad request since it's not possible to create a customized product collection with customized products which don't exist (resources don't exist)
            Assert.Equal(HttpStatusCode.BadRequest,createCustomizedProductCollection.StatusCode);
            
            Console.WriteLine("\001b[0;32m"+"ensureCantCreateACustomizedProductCollectionIfItAggregatesInvalidCustomizedProducts"+"\u001b[0m");
        }

        /// <summary>
        /// Ensures that a customized product collection is created succesfuly
        /// </summary>
        [Fact, TestPriority(3)]
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


        [Fact, TestPriority(4)]
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