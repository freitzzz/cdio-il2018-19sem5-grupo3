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
using System.Diagnostics;

namespace backend_tests.Controllers{
    
    /// <summary>
    /// Integration Tests for Products Collection API
    /// </summary>
    [Collection("Integration Collection")]
    [TestCaseOrderer("backend_tests.Setup.TestPriorityOrderer", "backend_tests")]
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
        [Fact, TestPriority(0)]
        public async void ensureProductsCollecionFetchIsEmpty(){
            //Since we haven't add any products yet, the products collection should be empty
            var productsCollectionFetch=await httpClient.GetAsync(PRODUCTS_URI);
            //HTTP Response should be a Bad Request
            Assert.True(productsCollectionFetch.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if the request body is empty
        /// </summary>
        [Fact, TestPriority(1)]
        public async void ensureProductCantBeCreatedWithEmptyRequestBody(){
            //We are attempting to create an object with an empty request body
            var createProductEmptyRequestBody=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON("{}"));
            //Since we performed a request to create a product with empty request body
            //Then the response should be a Bad Request
            Assert.True(createProductEmptyRequestBody.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if it has no reference
        /// </summary>
        [Fact, TestPriority(2)]
        public async void ensureProductCantBeCreatedWithNoReference(){
            //We are attempting to created a product with no referene
            ProductDTO productDTO=new ProductDTO();
            productDTO.designation="Valid Designation";
            var createProductNoReference=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
            //Since there was an attempt to create a product with no reference
            //Then the response should be a Bad Request
            Assert.True(createProductNoReference.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if it has no designation
        /// </summary>
        [Fact, TestPriority(3)]
        public async void ensureProductCantBeCreatedWithNoDesignation(){
            //We are attempting to created a product with no designation
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference="Valid Reference";
            var createProductNoDesignation=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
            //Since there was an attempt to create a product with no designation
            //Then the response should be a Bad Request
            Assert.True(createProductNoDesignation.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if it has no category
        /// </summary>
        [Fact, TestPriority(4)]
        public async void ensureProductCantBeCreatedWithNoCategory(){
            //We are attempting to created a product with no category
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference="Valid Reference";
            productDTO.designation="Valid Designation";
            var createProductNoCategory=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
            //Since there was an attempt to create a product with no category
            //Then the response should be a Bad Request
            Assert.True(createProductNoCategory.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if it has no materials
        /// </summary>
        [Fact, TestPriority(5)]
        public async void ensureProductCantBeCreatedWithNoMaterials(){
            //We are attempting to created a product with no materials
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference="Valid Reference";
            productDTO.designation="Valid Designation";
            var productCategoryDTO=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
            productCategoryDTO.Wait();
            productDTO.productCategory=productCategoryDTO.Result;
            var createProductNoMaterials=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
            //Since there was an attempt to create a product with no materials
            //Then the response should be a Bad Request
            Assert.True(createProductNoMaterials.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product can't be created if it has no dimensions
        /// </summary>
        [Fact, TestPriority(6)]
        public async void ensureProductCantBeCreatedWithNoDimensions(){
            //We are attempting to created a product with no dimensions
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference="Valid Reference";
            productDTO.designation="Valid Designation";
            var productCategoryDTO=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
            productCategoryDTO.Wait();
            productDTO.productCategory=productCategoryDTO.Result;
            //Materials must previously exist as they can be shared in various products
            Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
            materialDTO.Wait();
            productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});
            var createProductNoDimensions=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
            //Since there was an attempt to create a product with no dimensions
            //Then the response should be a Bad Request
            Assert.True(createProductNoDimensions.StatusCode==HttpStatusCode.BadRequest);
        }

        //public async void ensureProductCollectionFetch

        /// <summary>
        /// Ensures that a product is created succesfuly
        /// </summary>
        /// <returns>ProductDTO with the created product</returns>
        [Fact, TestPriority(8)]
        public async Task<ProductDTO> ensureProductIsCreatedSuccesfuly(){
            //We are going to create a valid product
            //A valid product creation requires a valid reference, a valid desgination
            //A valid category, valid dimensions and valid materials
            //Components are not required
            //To ensure atomicity, our reference will be generated with a timestamp (We have no bussiness rules so far as how they should be so its valid at this point)
            string reference="#666"+Guid.NewGuid().ToString("n");
            //Designation can be whatever we decide
            string designation="Time N Place";
            //Categories must previously exist as they can be shared in various products
            Task<ProductCategoryDTO> categoryDTO=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
            categoryDTO.Wait();
            //Materials must previously exist as they can be shared in various products
            Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
            materialDTO.Wait();
            DiscreteDimensionIntervalDTO discreteDimensionIntervalDTO=new DiscreteDimensionIntervalDTO();
            discreteDimensionIntervalDTO.values=new List<double>(new[]{1.0,2.0,30.0});
            ContinuousDimensionIntervalDTO continuousDimensionIntervalDTO=new ContinuousDimensionIntervalDTO();
            continuousDimensionIntervalDTO.increment=1;
            continuousDimensionIntervalDTO.minValue=10;
            continuousDimensionIntervalDTO.maxValue=100;
            SingleValueDimensionDTO singleValueDimensionDTO=new SingleValueDimensionDTO();
            singleValueDimensionDTO.value=50;
            ProductDTO productDTO=new ProductDTO();
            productDTO.reference=reference;
            productDTO.designation=designation;
            productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});
            productDTO.productCategory=categoryDTO.Result;
            productDTO.dimensions.depthDimensionDTOs=new List<DimensionDTO>(new[]{discreteDimensionIntervalDTO});
            productDTO.dimensions.heightDimensionDTOs=new List<DimensionDTO>(new[]{continuousDimensionIntervalDTO});
            productDTO.dimensions.widthDimensionDTOs=new List<DimensionDTO>(new[]{singleValueDimensionDTO});
            var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productDTO);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
        }
    }
}