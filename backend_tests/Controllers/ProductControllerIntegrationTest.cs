using backend_tests.Setup;
using backend_tests.utils;
using core.dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace backend_tests.Controllers{
    
    /// <summary>
    /// Integration Tests for Products Collection API
    /// </summary>
    [Collection("Integration Collection")]
    [TestCaseOrderer("backend_tests.Setup.PriorityOrderer", "backend_tests.Setup")]
    public sealed class ProductControllerIntegrationTest:IClassFixture<TestFixture<TestStartupSQLite>>{
        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string PRODUCTS_URI="myc/api/products";
        /// <summary>
        /// Current HTTP Client being used to perform API requests
        /// </summary>
        private HttpClient httpClient;
        /// <summary>
        /// Injected Mock Server
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;
        /// <summary>
        /// Builds a new ProductControllerIntegrationTest with the mocked server injected by parameters
        /// </summary>
        /// <param name="fixture">Injected Mocked Server</param>
        public ProductControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture){
            this.fixture=fixture;
            this.httpClient=fixture.httpClient;
        }

        /// <summary>
        /// Ensures that the products collection is empty if there are no products available
        /// </summary>
        [Fact]
        public async void ensureProductsCollecionFetchIsEmpty(){
            //Since we haven't add any products yet, the products collection should be empty
            var productsCollectionFetch=await httpClient.GetAsync(PRODUCTS_URI);
            //HTTP Response should be a Bad Request
            Assert.True(productsCollectionFetch.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if the request body is empty
        /// </summary>
        [Fact]
        public async void ensureProductCantBeCreatedWithEmptyRequestBody(){
            //We are attempting to create an object with an empty request body
            var createProductEmptyRequestBody=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentUtils.contentAsJSON("{}"));
            //Since we performed a request to create a product with empty request body
            //Then the response should be a Bad Request
            Assert.True(createProductEmptyRequestBody.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product is created succesfuly
        /// </summary>
        /// <returns>ProductDTO with the created product</returns>
        public async Task<ProductDTO> ensureProductIsCreatedSuccesfuly(){
            //We are going to create a valid product
            //A valid product creation requires a valid reference, a valid desgination
            //A valid category, valid dimensions and valid materials
            //Components are not required
            //To ensure atomicity, our reference will be generated with a timestamp (We have no bussiness rules so far as how they should be so its valid at this point)
            string reference="#666"+DateTime.Now;
            //Designation can be whatever we decide
            string designation="Time N Place";
            //Categories must previously exist as they can be shared in various products
            Task<ProductCategoryDTO> categoryDTO=new ProductCategoryControllerIntegrationTest(fixture).ensureProductCategoryIsCreatedSucessfuly();
            //Materials must previously exist as they can be shared in various products
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference=reference;
            productDTO.designation=designation;
            return httpClient.PostAsync(PRODUCTS_URI,HTTPContentUtils.contentAsJSON(productDTO)).;
        }
    }
}