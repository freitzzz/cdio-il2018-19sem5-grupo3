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
                    BaseAddress =  new Uri("http://localhost:5001")
                });
        }

        [Fact, TestPriority(1)]
        public async Task ensurePostCommercialCatalogueFailsWithEmptyRequestBody()
        {
            var response = await client.PostAsJsonAsync(urlBase, "{}");
            Console.WriteLine(response.StatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact, TestPriority(2)]
        public async Task<CommercialCatalogueDTO> ensurePostCommercialCatalogueWorks()
        {


            Task<ProductDTO> productDTOTask = new ProductControllerIntegrationTest(fixture).ensureProductIsCreatedSuccesfuly();
            productDTOTask.Wait();
            ProductDTO productDTO = productDTOTask.Result;


            CustomizedMaterialDTO custMaterialDTO = new CustomizedMaterialDTO();
            /* custMaterialDTO.color = colorDTO;
            custMaterialDTO .finish = finishDTO;*/

            CustomizedDimensionsDTO custDimensionsDTO = new CustomizedDimensionsDTO();
            custDimensionsDTO.height = 23.4;
            custDimensionsDTO.width = 4.5;
            custDimensionsDTO.depth = 6.0;


            CustomizedProductDTO custProduct = new CustomizedProductDTO();
            custProduct.reference = "3";
            custProduct.customizedDimensionsDTO = custDimensionsDTO;
            custProduct.customizedMaterialDTO = custMaterialDTO;
            custProduct.designation = "Customized Product";
            custProduct.productDTO = productDTO;


            List<CustomizedProductDTO> custProducts = new List<CustomizedProductDTO>();
            custProducts.Add(custProduct);

            CustomizedProductCollectionDTO productsCollection = new CustomizedProductCollectionDTO();
            productsCollection.name = "CustomizedProductsCollection";
            productsCollection.customizedProducts = new List<CustomizedProductDTO>(custProducts);

            CatalogueCollectionDTO catalogueCollection = new CatalogueCollectionDTO();
            catalogueCollection.customizedProductsDTO = new List<CustomizedProductDTO>(custProducts);
            catalogueCollection.customizedProductCollectionDTO = productsCollection;

            List<CatalogueCollectionDTO> listCatalogueCollection = new List<CatalogueCollectionDTO>();
            listCatalogueCollection.Add(catalogueCollection);


            CommercialCatalogueDTO commercialCatalogue = new CommercialCatalogueDTO();
            commercialCatalogue.reference = "6";
            commercialCatalogue.designation = " Catalogue";
            commercialCatalogue.collectionList = listCatalogueCollection;

            var response = await client.PostAsJsonAsync(urlBase, commercialCatalogue);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            return JsonConvert.DeserializeObject<CommercialCatalogueDTO>(await response.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(3)]
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