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
        private const string CUSTOMIZED_PRODUCTS_COLLECTION_URI = "mycm/api/collections";
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
/*         
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
            Task<CustomizedProductCollectionDTO> validCustomizedProductCollectionDTOTask=ensureCanCreateACustomizedProductCollectionIfItHasAValidName();
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
        /// Ensures that's possible to create a customized product collection with a valid name
        /// </summary>
        [Fact, TestPriority(3)]
        public async Task<CustomizedProductCollectionDTO> ensureCanCreateACustomizedProductCollectionIfItHasAValidName()
        {
            //First we will generate an atomic name for the customized products collection
            string name="Braga" + Guid.NewGuid().ToString("n");
            //Now we will grant that there are no customized product collection with that name
            grantNoCustomizedProductCollectionExistWithName(name);
            //Now let's add that name to the customized product collection
            CustomizedProductCollectionDTO customizedProductCollectionDTO = new CustomizedProductCollectionDTO();
            customizedProductCollectionDTO.name = name;

            //We will try to create a customized product collection with the generated name
            var createCustomizedProductsCollection = await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI, customizedProductCollectionDTO);
            //Since there were no customized product collection with the generated name, then the result should tell us that it was created (sucessfuly)
            Assert.Equal(HttpStatusCode.Created,createCustomizedProductsCollection.StatusCode);
            //To ensure that the creation was sucessful we will fetch the customized product collection by its name
            grantExistsCustomizedProductCollectionExistWithName(name);
            //We can also grant that its possible to fetch the customized product collection by its ID
            return JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await createCustomizedProductsCollection.Content.ReadAsStringAsync());
        } */

        //!
        //TODO URGENT COMMENT OUT THESE TESTS AND FIX THEM AFTER FIXES ARE MADE TO CUSTOMIZEDPRODUCTCONTROLLERINTEGRATION TESTS
        //!

        /// <summary>
        /// Ensures that's possible to create a customized product collection with a valid name and valid customized products
        /// </summary>
/*         [Fact, TestPriority(4)]
        public async Task<CustomizedProductCollectionDTO> ensureCanCreateACustomizedProductCollectionIfItHasAValidNameAndValidCustomizedProducts()
        {
            //First we will generate an atomic name for the customized products collection
            string name="Braga" + Guid.NewGuid().ToString("n");
            //Now we will grant that there are no customized product collection with that name
            grantNoCustomizedProductCollectionExistWithName(name);
            //Now let's add that name to the customized product collection
            CustomizedProductCollectionDTO customizedProductCollectionDTO = new CustomizedProductCollectionDTO();
            customizedProductCollectionDTO.name = name;
            CustomizedProductDTO customizedProductDTO=new CustomizedProductDTO();
            //We need a valid customized product so let's create one
            Task<CustomizedProductDTO> customizedProductDTOTask=new CustomizedProductControllerIntegrationTest(fixture).ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();
            customizedProductDTOTask.Wait();
            //Now let's add the customized product to the customized product collection
            customizedProductCollectionDTO.customizedProducts=new List<CustomizedProductDTO>(new []{customizedProductDTOTask.Result});
            //We will try to create a customized product collection with the generated name and customized products
            var createCustomizedProductsCollection = await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI, customizedProductCollectionDTO);
            //Since there were no customized product collection with the generated name and customized products then the result should tell us that it was created (sucessfuly)
            Assert.Equal(HttpStatusCode.Created,createCustomizedProductsCollection.StatusCode);
            //To ensure that the creation was sucessful we will fetch the customized product collection by its name
            grantExistsCustomizedProductCollectionExistWithName(name);
            //We can also grant that its possible to fetch the customized product collection by its ID
            return JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await createCustomizedProductsCollection.Content.ReadAsStringAsync());
        } */

        //[Fact, TestPriority(5)]
/*         public async Task<CustomizedProductCollectionDTO> ensureCanRemoveACustomizedProductFromTheCustomizedProductCollectionIfItIsValid()
        {
            //Creates a customized product collection with a valid name and a customized product
            CustomizedProductCollectionDTO customizedProductCollectionDTO = await ensureCanCreateACustomizedProductCollectionIfItHasAValidNameAndValidCustomizedProducts();
            long id = 0;

            //Fetches the customized product's ID of the customized product collection
            foreach(CustomizedProductDTO customizedProductDTO in customizedProductCollectionDTO.customizedProducts) id = customizedProductDTO.id;

            //Deletes the customized product with the given ID
            var removeCustomizedProduct = await httpClient.DeleteAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI + 
            "/" + customizedProductCollectionDTO.id + "/customizedproducts/" + id);

            //Since the customized product could be removed, the result is no content, meaning it was deleted from the collection
            Assert.Equal(HttpStatusCode.NoContent, removeCustomizedProduct.StatusCode);

            //Fetches the updated customized product collection
            var getCustomizedProductCollection = await httpClient.GetAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI + "/" + customizedProductCollectionDTO.id);
            
            var changedCustomizedProductCollectionDTO = JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await getCustomizedProductCollection.Content.ReadAsStringAsync());
            //Since its only product was removed, now the customized product collection should have an empty list of customized products
            Assert.Empty(changedCustomizedProductCollectionDTO.customizedProducts);       

            return changedCustomizedProductCollectionDTO;
        } */

 /*        [Fact, TestPriority(6)]
        public async Task<CustomizedProductCollectionDTO> ensureCantRemoveANonExistentCustomizedProductFromTheCustomizedProductCollection()
        {
            //Creates a customized product collection with a valid name and a customized product
            CustomizedProductCollectionDTO customizedProductCollectionDTO = await ensureCanCreateACustomizedProductCollectionIfItHasAValidNameAndValidCustomizedProducts();

            //Tries to delete the customized product with an invalid ID
            var removeCustomizedProduct = await httpClient.DeleteAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI + 
            "/" + customizedProductCollectionDTO.id + "/customizedproducts/" + 0);

            //Since the customized product doesn't exist, the result is bad request, meaning it couldn't be removed
            Assert.Equal(HttpStatusCode.BadRequest, removeCustomizedProduct.StatusCode);

            //Fetches the updated customized product collection
            var getCustomizedProductCollection = await httpClient.GetAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI + "/" + customizedProductCollectionDTO.id);
            var updatedCustomizedProductCollectionDTO =  JsonConvert.DeserializeObject<CustomizedProductCollectionDTO>(await getCustomizedProductCollection.Content.ReadAsStringAsync());

            //Ensures the customized product wasn't deleted
            Assert.NotEmpty(updatedCustomizedProductCollectionDTO.customizedProducts);

            return updatedCustomizedProductCollectionDTO;
        } */

/*         /// <summary>
        /// Ensures that there is no customized product collection with a certain name
        /// </summary>
        /// <param name="name">string with the customized product collection name</param>
        private void grantNoCustomizedProductCollectionExistWithName(string name){
            var customizedProductCollection=httpClient.GetAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI+"/?name="+name);
            Assert.Equal(HttpStatusCode.NotFound,customizedProductCollection.Result.StatusCode);
        }

        /// <summary>
        /// Ensures that there is a customized product collection with a certain name
        /// </summary>
        /// <param name="name">string with the customized product collection name</param>
        private void grantExistsCustomizedProductCollectionExistWithName(string name){
            var customizedProductCollection=httpClient.GetAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI+"/?name="+name);
            Assert.Equal(HttpStatusCode.OK,customizedProductCollection.Result.StatusCode);
        }

        /// <summary>
        /// Ensures that there is a customized product collection with a certain resource ID
        /// </summary>
        /// <param name="id">long with the customized product collection resource ID</param>
        private void grantExistsCustomizedProductCollectionExistWithResourceID(long id){
            var customizedProductCollection=httpClient.GetAsync(CUSTOMIZED_PRODUCTS_COLLECTION_URI+"/"+id);
            Assert.Equal(HttpStatusCode.OK,customizedProductCollection.Result.StatusCode);
        } */
    }
}