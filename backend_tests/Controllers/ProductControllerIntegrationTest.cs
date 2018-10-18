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
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
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
        /// Ensures that the product collection fetch is empty
        /// </summary>
        [Fact,TestPriority(0)]
        public async void ensureProductCollectionFetchIsEmpty(){
            //If we haven't created any products yet, the product collection 
            //fetch should be empty
            var fetchProductCollection=await httpClient.GetAsync(PRODUCTS_URI);
            //We performed a GET of the products collection
            //Since there are no products available
            //The response status code should be a Bad Request
            //Since there aren't any products available
            Assert.True(fetchProductCollection.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that the product collection fetch isnt empty when previously added a product
        /// </summary>
        [Fact,TestPriority(1)]
        public async void ensureProductCollectionFetchIsNotEmpty(){
            ensureProductIsCreatedSuccesfuly().Wait();
            //Since we have created a product, the product collection fetch
            //fetch shouldn't be empty
            var fetchProductCollection=await httpClient.GetAsync(PRODUCTS_URI);
            //We performed a GET of the products collection
            //Since there are no products available
            //The response status code should be a Bad Request
            //Since there aren't any products available
            Assert.True(fetchProductCollection.StatusCode==HttpStatusCode.OK);
        }

        /// <summary>
        /// Ensures that the fetch of a product resource is invalid
        /// </summary>
        [Fact,TestPriority(2)]
        public async void ensureProductResourceFetchIsInvalid(){
            //If we haven't created any products yet, the product collection 
            //fetch should be empty
            long invalidResourceID=0;
            var fetchProduct=await httpClient.GetAsync(PRODUCTS_URI+"/"+invalidResourceID);
            //We performed a GET of a certain product by its resource
            //Since the resource doesn't exist, the fetch should be invalid
            //And the response status code should be a Bad Request
            Assert.True(fetchProduct.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that the fetch of a product resource is valid
        /// </summary>
        [Fact,TestPriority(3)]
        public async void ensureProductResourceFetchIsValid(){
            Task<ProductDTO> createdProductDTORequest=ensureProductIsCreatedSuccesfuly();
            createdProductDTORequest.Wait();
            ProductDTO createdProductDTO=createdProductDTORequest.Result;
            //We just performed a POST request to create a new product
            //We deserialized the response, and it has a resource ID
            //By performing a GET request on the resource, the product fetched
            //Should be the same one as the created one
            var fetchProduct=await httpClient.GetAsync(PRODUCTS_URI+"/"+createdProductDTO.id);
            ProductDTO fetchedProductDTO=JsonConvert.DeserializeObject<ProductDTO>(await fetchProduct.Content.ReadAsStringAsync());
            Assert.True(fetchProduct.StatusCode==HttpStatusCode.OK);
            Assert.Equal(createdProductDTO.id,fetchedProductDTO.id);
            Assert.Equal(createdProductDTO.reference,fetchedProductDTO.reference);
        }

        /// <summary>
        /// Ensures that a product reference cant be updated if the reference is invalid
        /// </summary>
        [Fact,TestPriority(4)]
        public async void ensureProductReferenceCantBeUpdatedIfInvalid(){
             //We're are going to created two different products, X & Y
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            //If we try to update it to a reference that is invalid, then it should fail
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            var updateReference=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateReference.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product reference cant be updated if the reference is duplicated
        /// </summary>
        [Fact,TestPriority(5)]
        public async void ensureProductReferenceCantBeUpdatedIfDuplicated(){
            //We're are going to created two different products, X & Y
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            Task<ProductDTO> createdProductDTOY=ensureProductIsCreatedSuccesfuly();
            createdProductDTOY.Wait();
            //Product X has different reference to Y
            //Since the reference of a product is the identity of a product
            //If we try to update it to a reference that already exists, then it should fail
            ProductDTO productDTOX=createdProductDTOX.Result;
            ProductDTO productDTOY=createdProductDTOY.Result;
            UpdateProductDTO updatedProductY=new UpdateProductDTO();
            updatedProductY.reference=productDTOX.reference;
            //DONT UNCOMMENT UNTIL INTEGRATION DATABASE PROBLEMS IS SOLVED
            //PROBLEM HERE IS THAT SINCE WE ARE USING AN IN MEMORY DATABASE PROVIDER
            //IT LITERALLY IS IN MEMORY SO IF WE CHANGE AN OBJECT WIHTOUT UPDATING IT
            //WITH THE RESPECTIVE REPOSITORY, ITS STILL AN UPDATE SINCE OBJECTS HAVE THE SAME MEMORY REFERENCE
            //:(
            /* var updateReference=await httpClient.PutAsync(PRODUCTS_URI+"/"+productDTOY.id
                                        ,HTTPContentCreator.contentAsJSON(updatedProductY));
            Assert.True(updateReference.StatusCode==HttpStatusCode.BadRequest); */
        }
        
        /// <summary>
        /// Ensures that a product designation cant be updated if the designation is invalid
        /// </summary>
        [Fact,TestPriority(6)]
        public async void ensureProductDesignationCantBeUpdatedIfInvalid(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            //If we try to update it to a designation that is invalid, then it should fail
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            var updateDesignation=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateDesignation.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product materials cant be updated if the materials are invalid
        /// </summary>
        [Fact,TestPriority(7)]
        public async void ensureProductMaterialsCantBeUpdatedIfInvalid(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
                                                                            .Result.id
                                                                        +"/materials"
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product materials cant be added if the materials are duplicated
        /// </summary>
        [Fact,TestPriority(8)]
        public async void ensureProductMaterialsCantBeAddedIfDuplicated(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            updatedProductX.materialsToAdd=new List<MaterialDTO>(createdProductDTOX.Result.productMaterials);
            var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
                                                                            .Result.id
                                                                        +"/materials"
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product materials cant be added if the materials don't exist/are not found
        /// </summary>
        [Fact,TestPriority(9)]
        public async void ensureProductMaterialsCantBeAddedIfNotFound(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            updatedProductX.materialsToAdd=new List<MaterialDTO>(new []{new MaterialDTO()});
            var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
                                                                            .Result.id
                                                                        +"/materials"
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Ensures that a product materials cant be removed if the materials don't exist/are not found
        /// </summary>
        [Fact,TestPriority(10)]
        public async void ensureProductMaterialsCantBeRemovedIfNotFound(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            updatedProductX.materialsToRemove=new List<MaterialDTO>(new []{new MaterialDTO()});
            var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
                                                                            .Result.id
                                                                        +"/materials"
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
        }

        /* /// <summary>
        /// Ensures that the dimensions of a product cant be removed if the dimensions don't exist/are not found
        /// </summary>
        [Fact,TestPriority(11)]
        public async void ensureProductDimensionsCantBeRemovedIfNotFound(){
            //We need to create a product for the test
            Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
            createdProductDTOX.Wait();
            UpdateProductDTO updatedProductX=new UpdateProductDTO();
            updatedProductX.dimensionsToRemove=new List<MaterialDTO>(new []{new DimensionDTO()});
            var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
                                                                            .Result.id
                                                                        +"/dimensions"
                                        ,HTTPContentCreator.contentAsJSON(updatedProductX));
            Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
        } */

        /// <summary>
        /// Ensures that a product can't be created if the request body is empty
        /// </summary>
        [Fact, TestPriority(13)]
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
        [Fact, TestPriority(14)]
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
        [Fact, TestPriority(15)]
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
        [Fact, TestPriority(16)]
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
        [Fact, TestPriority(17)]
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
        [Fact, TestPriority(18)]
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
        
        /// <summary>
        /// Ensures that a product is created succesfuly
        /// </summary>
        /// <returns>ProductDTO with the created product</returns>
        [Fact, TestPriority(19)]
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
            DimensionsListDTO dimensionsListDTO=new DimensionsListDTO();
            dimensionsListDTO.depthDimensionDTOs=new List<DimensionDTO>(new[]{discreteDimensionIntervalDTO});
            dimensionsListDTO.heightDimensionDTOs=new List<DimensionDTO>(new[]{continuousDimensionIntervalDTO});
            dimensionsListDTO.widthDimensionDTOs=new List<DimensionDTO>(new[]{singleValueDimensionDTO});
            productDTO.dimensions=dimensionsListDTO;
            var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productDTO);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
        }
    }
}