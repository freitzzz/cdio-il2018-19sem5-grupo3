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
using core.modelview.customizedproduct;

namespace backend_tests.Controllers
{
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class CustomizedProductControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        private const string baseUri = "mycm/api/customizedproducts";

        private TestFixture<TestStartupSQLite> fixture;

        private HttpClient httpClient;

        public CustomizedProductControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        [Fact, TestPriority(0)]
        public async Task ensureGetAllReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(baseUri);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(1)]
        public async Task ensureGetByIdReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(String.Format(baseUri + "/{0}", 1));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task ensureGetByIdReturnsNotFoundIfIdIsNotValid()
        {
            var response = await httpClient.GetAsync(String.Format(baseUri + "/{0}", -1));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task ensurePostWithInvalidProductReferenceReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();
            customizedDimensionsDTO.depth = 10;
            customizedDimensionsDTO.height = 20;
            customizedDimensionsDTO.width = 30;

            MaterialDTO materialDTO = productDTO.productMaterials.First();
            FinishDTO materialFinishDTO = materialDTO.finishes.First();
            ColorDTO materialColorDTO = materialDTO.colors.First();

            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = materialFinishDTO.description;

            ColorDTO colorDTO = new ColorDTO()
            { name = materialColorDTO.name, red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = -1;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task ensurePostWithInvalidRequestBodyReturnsBadRequest()
        {
            PostCustomizedProductModelView customizedProductModelView = null;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(5)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

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
            { name = materialColorDTO.name, red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;


            //CustomizedProductDTO creation with the previously created dimensions and material
            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();
            //A customized product requires a valid reference
            customizedProductModelView.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductModelView.designation = productDTO.designation;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.productId = productDTO.id;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, createCustomizedProduct.StatusCode);

            PostCustomizedProductModelView customizedProductModelViewFromPost = JsonConvert.DeserializeObject<PostCustomizedProductModelView>(await createCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, customizedProductModelViewFromPost.reference);
            Assert.Equal(customizedProductModelView.designation, customizedProductModelViewFromPost.designation);
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), customizedProductModelViewFromPost.customizedDimensionsDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), customizedProductModelViewFromPost.customizedMaterialDTO.toEntity());
            Assert.Empty(customizedProductModelViewFromPost.slots);

            var fetchCreatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductModelViewFromPost.id));

            Assert.Equal(HttpStatusCode.OK, fetchCreatedCustomizedProduct.StatusCode);

            CustomizedProductDTO fetchedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchCreatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, fetchedCustomizedProductDTO.reference);
            Assert.Equal(customizedProductModelView.designation, fetchedCustomizedProductDTO.designation);
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), fetchedCustomizedProductDTO.customizedDimensionsDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), fetchedCustomizedProductDTO.customizedMaterialDTO.toEntity());
            Assert.Empty(fetchedCustomizedProductDTO.slotListDTO);

            return fetchedCustomizedProductDTO;
        }

    }
}