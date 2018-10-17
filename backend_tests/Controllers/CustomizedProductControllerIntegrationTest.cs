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
    public sealed class CustomizedProductControllerIntegrationTest:IClassFixture<TestFixture<TestStartupSQLite>>{
        
        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string CUSTOMIZED_PRODUCTS_URI="myc/api/customizedproducts";
        /// <summary>
        /// Injected Mock Server
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;
        /// <summary>
        /// Current HTTP Client
        /// </summary>
        private HttpClient httpClient;
        /// <summary>
        /// Builds a new CustomizedProductControllerIntegrationTest with the mocked server injected by parameters
        /// </summary>
        /// <param name="fixture">Injected Mocked Server</param>
        public CustomizedProductControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture){
            this.fixture=fixture;
            this.httpClient=fixture.httpClient;
        }
        
        /// <summary>
        /// Ensures that a customized product is created succesfuly
        /// </summary>
        [Fact,TestPriority(14209)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductIsCreatedSuccesfuly(){
            CustomizedProductDTO customizedProductDTO=new CustomizedProductDTO();
            //A customized product requires a valid reference
            customizedProductDTO.reference="#CP4445"+Guid.NewGuid().ToString("n");
            //A customized product requires a valid designation
            customizedProductDTO.designation="Pride Closet";
            Task<ProductDTO> productDTO=new ProductControllerIntegrationTest(fixture).ensureProductIsCreatedSuccesfuly();
            productDTO.Wait();
            //A customized product references a product which is being customized
            customizedProductDTO.productDTO=productDTO.Result;
            CustomizedMaterialDTO customizedMaterialDTO=new CustomizedMaterialDTO();
            ColorDTO colorDTO=new ColorDTO();
            FinishDTO finishDTO=new FinishDTO();
            colorDTO.name="White";
            colorDTO.red=0xFF;
            colorDTO.green=0xFF;
            colorDTO.blue=0xFF;
            finishDTO.description="MDF";
            //A customized product requires a customized material
            customizedMaterialDTO.color=colorDTO;
            customizedMaterialDTO.finish=finishDTO;
            //TODO:SLOTS
            //var createCustomizedProduct=await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_URI,customizedProductDTO);
            //Uncomment when slots creation is fixed
            //Assert.True(createCustomizedProduct.StatusCode==HttpStatusCode.Created);
            //return JsonConvert.DeserializeObject<CustomizedProductDTO>(await createCustomizedProduct.Content.ReadAsStringAsync());
            return null;
        }

    }
}