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

namespace backend_tests.Controllers{

    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class CustomizedProductsCollectionControllerIntegrationTest:IClassFixture<TestFixture<TestStartupSQLite>>{
        
        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string CUSTOMIZED_PRODUCTS_COLLECTION_URI="myc/api/collections";
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
        public CustomizedProductsCollectionControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture){
            this.fixture=fixture;
            this.httpClient=fixture.httpClient;
        }
        
        /// <summary>
        /// Ensures that a customized product collection is created succesfuly
        /// </summary>
        [Fact,TestPriority(14209)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductIsCreatedSuccesfuly(){
            CustomizedProductCollectionDTO customizedProductCollectionDTO=new CustomizedProductCollectionDTO();
            //A collection of customized products requires a valid name
            customizedProductCollectionDTO.name="Braga"+Guid.NewGuid().ToString("n");
            //A collection of customized products can be created with only a name
            //But if needed customized products for it, uncomment the lines below
            //Task<CustomizedProductDTO> customizedProductDTO=new CustomizedProductControllerIntegrationTest(fixture).ensureCustomizedProductIsCreatedSuccesfuly();
            //customizedProductDTO.Wait();
            //customizedProductCollectionDTO.customizedProducts=new List<CustomizedProductDTO>(new []{customizedProductDTO.Result});
            var createCustomizedProductsCollection=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI,customizedProductCollectionDTO);
            //Uncomment when slots creation is fixed
            Assert.True(createCustomizedProductsCollection.StatusCode==HttpStatusCode.Created);
            return JsonConvert.DeserializeObject<CustomizedProductDTO>(await createCustomizedProductsCollection.Content.ReadAsStringAsync());
        }

    }
}