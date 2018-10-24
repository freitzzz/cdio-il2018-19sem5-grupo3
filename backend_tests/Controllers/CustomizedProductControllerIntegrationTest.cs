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
using System.Linq;

namespace backend_tests.Controllers
{
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class CustomizedProductControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string CUSTOMIZED_PRODUCTS_URI = "myc/api/customizedproducts";
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
        public CustomizedProductControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        /// <summary>
        /// Ensures that a customized product is created succesfuly
        /// </summary>
        [Fact, TestPriority(1)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductIsCreatedSuccesfuly()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            //When creating a customized product, only the product's id is defined
            ProductDTO productDTOWithJustID = new ProductDTO() { id = productDTO.id };

            //CustomizedDimensionsDTO creation
            //Please note that these dimensions reflect those specified in the product
            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();
            customizedDimensionsDTO.height = 30.0;
            customizedDimensionsDTO.width = 50.0;
            customizedDimensionsDTO.depth = 95.0;

            MaterialDTO materialDTO = productDTO.productMaterials.First();
            FinishDTO materialFinishDTO = materialDTO.finishes.First();
            ColorDTO materialColorDTO = materialDTO.colors.First();

            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = materialFinishDTO.description;

            ColorDTO colorDTO = new ColorDTO()
            { red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            MaterialDTO materialDTOWithJustID = new MaterialDTO() { id = materialDTO.id };
            customizedMaterialDTO.material = materialDTOWithJustID;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;


            //CustomizedProductDTO creation with the previously created dimensions and material
            CustomizedProductDTO customizedProductDTO = new CustomizedProductDTO();
            //A customized product requires a valid reference
            customizedProductDTO.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductDTO.designation = productDTO.designation;
            customizedProductDTO.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductDTO.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductDTO.productDTO = productDTOWithJustID;


            //TODO:SLOTS
            var createCustomizedProduct = await httpClient.PostAsJsonAsync(CUSTOMIZED_PRODUCTS_URI, customizedProductDTO);
            Assert.True(createCustomizedProduct.StatusCode == HttpStatusCode.Created);
            return JsonConvert.DeserializeObject<CustomizedProductDTO>(await createCustomizedProduct.Content.ReadAsStringAsync());
        }

    }
}