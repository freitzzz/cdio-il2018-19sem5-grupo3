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
using core.modelview.product;

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

        //TODO test generic flow of operations (e.g. get -> post -> get -> several puts -> delete -> get)
        /* [Fact, TestPriority(0)]
        public async Task testCRUDOperations()
        {
         } */

        [Fact, TestPriority(1)]
        public async Task ensureGetAllReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(baseUri);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task ensureGetByIdReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(String.Format(baseUri + "/{0}", 1));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task ensureGetByIdReturnsNotFoundIfIdIsNotValid()
        {
            var response = await httpClient.GetAsync(String.Format(baseUri + "/{0}", -1));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task ensurePostWithNullRequestBodyReturnsBadRequest()
        {
            AddCustomizedProductModelView customizedProductModelView = null;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(5)]
        public async Task ensurePostWithEmptyRequestBodyReturnsBadRequest()
        {
            AddCustomizedProductModelView customizedProductModelView = new AddCustomizedProductModelView();

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        //!
        //TODO URGENT FIX THESE TESTS DUE TO REFACTOR FROM PRODUCTDTO TO PRODUCT MODEL VIEW
        //!

 /*        [Fact, TestPriority(6)]
        public async Task ensurePostWithNullCustomizedProductReferenceReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            GetProductModelView productMV = await productControllerTest.ensureProductIsCreatedSuccesfuly();
            //TODO: DONT FORGET TO CHANGE THIS
            ProductDTO productDTO = new ProductDTO();
            
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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = null;
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        } */

  /*       [Fact, TestPriority(7)]
        public async Task ensurePostWithEmptyCustomizedProductReferenceReturnsBadRequest()
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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(8)]
        public async Task ensurePostWithNullCustomizedProductDesignationReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            //ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            //TODO: DONT FORGET TO CHANGE THIS

            ProductDTO productDTO = new ProductDTO();

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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = null;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(9)]
        public async Task ensurePostWithEmptyCustomizedProductDesignationReturnsBadRequest()
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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(10)]
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

            return fetchedCustomizedProductDTO;
            return null;
        }

        [Fact, TestPriority(13)]
        public async Task ensurePostWithEmptyMaterialReferenceReturnsBadRequest()
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
            customizedMaterialDTO.material = new MaterialDTO();
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(14)]
        public async Task ensurePostWithInvalidFinishReturnsBadRequest()
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
            finishDTO.description = "-1";

            ColorDTO colorDTO = new ColorDTO()
            { name = materialColorDTO.name, red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = -productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(15)]
        public async Task ensurePostWithEmptyFinishReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();
            customizedDimensionsDTO.depth = 10;
            customizedDimensionsDTO.height = 20;
            customizedDimensionsDTO.width = 30;

            MaterialDTO materialDTO = productDTO.productMaterials.First();
            ColorDTO materialColorDTO = materialDTO.colors.First();

            ColorDTO colorDTO = new ColorDTO()
            { name = materialColorDTO.name, red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = new FinishDTO();
            customizedMaterialDTO.color = colorDTO;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = -productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(16)]
        public async Task ensurePostWithInvalidColorReturnsBadRequest()
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
            { name = "-1", red = materialColorDTO.red, green = materialColorDTO.green, blue = materialColorDTO.blue, alpha = materialColorDTO.alpha };

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = colorDTO;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(17)]
        public async Task ensurePostWithEmptyColorReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();
            customizedDimensionsDTO.depth = 10;
            customizedDimensionsDTO.height = 20;
            customizedDimensionsDTO.width = 30;

            MaterialDTO materialDTO = productDTO.productMaterials.First();
            FinishDTO materialFinishDTO = materialDTO.finishes.First();

            FinishDTO finishDTO = new FinishDTO();
            finishDTO.description = materialFinishDTO.description;

            //CustomizedMaterialDTO creation;
            CustomizedMaterialDTO customizedMaterialDTO = new CustomizedMaterialDTO();
            customizedMaterialDTO.material = materialDTO;
            customizedMaterialDTO.finish = finishDTO;
            customizedMaterialDTO.color = new ColorDTO();

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = -productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(18)]
        public async Task ensurePostWithEmptyCustomizedMaterialReturnsBadRequest()
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

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(19)]
        public async Task ensurePostWithInvalidCustomizedDimensionsReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();
            customizedDimensionsDTO.height = -1;
            customizedDimensionsDTO.width = -2;
            customizedDimensionsDTO.depth = -3;

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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(20)]
        public async Task ensurePostWithEmptyCustomizedDimensionsReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductIsCreatedSuccesfuly();

            CustomizedDimensionsDTO customizedDimensionsDTO = new CustomizedDimensionsDTO();

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

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(21)]
        public async Task ensurePostWithInvalidSlotDimensionsReturnsBadRequest()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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

            //Slot dimensions
            CustomizedDimensionsDTO slotDimension = new CustomizedDimensionsDTO();
            slotDimension.depth = -1;
            slotDimension.width = -2;
            slotDimension.height = -3;

            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.productId = productDTO.id;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.designation = "designation";
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
            customizedProductModelView.slots.Add(slotDimension);

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, createCustomizedProduct.StatusCode);
        }

        [Fact, TestPriority(22)]
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

        [Fact, TestPriority(23)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductWithSlotsIsCreatedSuccessfully()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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
            CustomizedDimensionsDTO slotDimension = new CustomizedDimensionsDTO();
            slotDimension.depth = productDTO.slotDimensions.recommendedSlotDimensions.depth;
            slotDimension.width = productDTO.slotDimensions.recommendedSlotDimensions.width;
            slotDimension.height = productDTO.slotDimensions.recommendedSlotDimensions.height;
            customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
            customizedProductModelView.slots.Add(slotDimension);
            Slot expectedSlot = new Slot(slotDimension.toEntity());

            var createCustomizedProduct = await httpClient.PostAsJsonAsync(baseUri, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, createCustomizedProduct.StatusCode);

            PostCustomizedProductModelView customizedProductModelViewFromPost = JsonConvert.DeserializeObject<PostCustomizedProductModelView>(await createCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, customizedProductModelViewFromPost.reference);
            Assert.Equal(customizedProductModelView.designation, customizedProductModelViewFromPost.designation);
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), customizedProductModelViewFromPost.customizedDimensionsDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), customizedProductModelViewFromPost.customizedMaterialDTO.toEntity());
            Assert.NotEmpty(customizedProductModelViewFromPost.slots);
            Assert.Equal(customizedProductModelView.slots.First().depth, customizedProductModelViewFromPost.slots.First().depth);
            Assert.Equal(customizedProductModelView.slots.First().width, customizedProductModelViewFromPost.slots.First().width);
            Assert.Equal(customizedProductModelView.slots.First().height, customizedProductModelViewFromPost.slots.First().height);

            var fetchCreatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductModelViewFromPost.id));

            Assert.Equal(HttpStatusCode.OK, fetchCreatedCustomizedProduct.StatusCode);

            CustomizedProductDTO fetchedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchCreatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, fetchedCustomizedProductDTO.reference);
            Assert.Equal(customizedProductModelView.designation, fetchedCustomizedProductDTO.designation);
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), fetchedCustomizedProductDTO.customizedDimensionsDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), fetchedCustomizedProductDTO.customizedMaterialDTO.toEntity());
            Assert.NotEmpty(fetchedCustomizedProductDTO.slotListDTO);
            Assert.Equal(expectedSlot.slotDimensions, fetchedCustomizedProductDTO.slotListDTO.First().customizedDimensions.toEntity());
            Assert.Empty(fetchedCustomizedProductDTO.slotListDTO.First().customizedProducts);

            return fetchedCustomizedProductDTO;
        }

        [Fact, TestPriority(24)]
        public async Task ensurePostCustomizedProductToSlotReturnsBadRequestIfTheFatherDoesntExist()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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
            PostCustomizedProductToSlotModelView customizedProductModelView = new PostCustomizedProductToSlotModelView();
            //A customized product requires a valid reference
            customizedProductModelView.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductModelView.designation = productDTO.designation;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.productId = productDTO.id;
            CustomizedDimensionsDTO slotDimension = new CustomizedDimensionsDTO();
            slotDimension.depth = productDTO.slotDimensions.recommendedSlotDimensions.depth;
            slotDimension.width = productDTO.slotDimensions.recommendedSlotDimensions.width;
            slotDimension.height = productDTO.slotDimensions.recommendedSlotDimensions.height;
            customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
            customizedProductModelView.slots.Add(slotDimension);


            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.PostAsJsonAsync(
                                String.Format(
                                                baseUri + "/{0}/slots/{1}", -1,
                                                customizedProductDTO.slotListDTO.First().Id
                                            ),
                                            customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(24)]
        public async Task ensurePostCustomizedProductToSlotReturnsBadRequestIfTheSlotDoesntExist()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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
            PostCustomizedProductToSlotModelView customizedProductModelView = new PostCustomizedProductToSlotModelView();
            //A customized product requires a valid reference
            customizedProductModelView.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductModelView.designation = productDTO.designation;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.productId = productDTO.id;
            CustomizedDimensionsDTO slotDimension = new CustomizedDimensionsDTO();
            slotDimension.depth = productDTO.slotDimensions.recommendedSlotDimensions.depth;
            slotDimension.width = productDTO.slotDimensions.recommendedSlotDimensions.width;
            slotDimension.height = productDTO.slotDimensions.recommendedSlotDimensions.height;
            customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
            customizedProductModelView.slots.Add(slotDimension);


            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.PostAsJsonAsync(
                                String.Format(
                                                baseUri + "/{0}/slots/{1}", customizedProductDTO.id,
                                                -1
                                            ),
                                            customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(25)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductWithoutSlotsIsAddedToASlotOfAnotherCustomizedProduct()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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
            PostCustomizedProductToSlotModelView customizedProductModelView = new PostCustomizedProductToSlotModelView();
            //A customized product requires a valid reference
            customizedProductModelView.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductModelView.designation = productDTO.designation;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.productId = productDTO.id;

            CustomizedProductDTO fatherCustomizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var childCustomizedProduct = await httpClient.PostAsJsonAsync(
                                String.Format(
                                                baseUri + "/{0}/slots/{1}", fatherCustomizedProductDTO.id,
                                                fatherCustomizedProductDTO.slotListDTO.First().Id
                                            ),
                                            customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, childCustomizedProduct.StatusCode);

            PostCustomizedProductModelView childCustomizedProductModelViewFromPost = JsonConvert.DeserializeObject<PostCustomizedProductModelView>(await childCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, childCustomizedProductModelViewFromPost.reference);
            Assert.Equal(customizedProductModelView.designation, childCustomizedProductModelViewFromPost.designation);
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), childCustomizedProductModelViewFromPost.customizedMaterialDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), childCustomizedProductModelViewFromPost.customizedDimensionsDTO.toEntity());
            Assert.Empty(childCustomizedProductModelViewFromPost.slots);

            var fetchCreatedChildCustomizedProduct = await httpClient.GetAsync(
                                                        String.Format(baseUri + "/{0}", childCustomizedProductModelViewFromPost.id)
                                                    );

            Assert.Equal(HttpStatusCode.OK, fetchCreatedChildCustomizedProduct.StatusCode);

            CustomizedProductDTO fetchedCreatedChildCustomizedProduct = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchCreatedChildCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, fetchedCreatedChildCustomizedProduct.reference);
            Assert.Equal(customizedProductModelView.designation, fetchedCreatedChildCustomizedProduct.designation);
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedMaterialDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedDimensionsDTO.toEntity());
            Assert.Empty(fetchedCreatedChildCustomizedProduct.slotListDTO);

            var fetchUpdatedFatherCustomizedProduct = await httpClient.GetAsync(
                                                    String.Format(baseUri + "/{0}", fatherCustomizedProductDTO.id)
                                                );

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedFatherCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedFather = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedFatherCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEmpty(updatedFather.slotListDTO.First().customizedProducts);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().reference, fetchedCreatedChildCustomizedProduct.reference);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().designation, fetchedCreatedChildCustomizedProduct.designation);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().customizedDimensionsDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedDimensionsDTO.toEntity());
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().customizedMaterialDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedMaterialDTO.toEntity());
            Assert.NotEmpty(updatedFather.slotListDTO.First().customizedProducts);
            Assert.Empty(updatedFather.slotListDTO.First().customizedProducts.First().slotListDTO);

            return fetchedCreatedChildCustomizedProduct;
        }

        [Fact, TestPriority(26)]
        public async Task<CustomizedProductDTO> ensureCustomizedProductWithSlotsIsAddedToASlotOfAnotherCustomizedProduct()
        {
            ProductControllerIntegrationTest productControllerTest = new ProductControllerIntegrationTest(fixture);
            ProductDTO productDTO = await productControllerTest.ensureProductWithSlotsIsCreatedSuccesfuly();

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
            PostCustomizedProductToSlotModelView customizedProductModelView = new PostCustomizedProductToSlotModelView();
            //A customized product requires a valid reference
            customizedProductModelView.reference = productDTO.reference;
            //A customized product requires a valid designation
            customizedProductModelView.designation = productDTO.designation;
            customizedProductModelView.customizedDimensionsDTO = customizedDimensionsDTO;
            customizedProductModelView.customizedMaterialDTO = customizedMaterialDTO;
            customizedProductModelView.productId = productDTO.id;
            CustomizedDimensionsDTO slotDimension = new CustomizedDimensionsDTO();
            slotDimension.depth = productDTO.slotDimensions.recommendedSlotDimensions.depth;
            slotDimension.width = productDTO.slotDimensions.recommendedSlotDimensions.width;
            slotDimension.height = productDTO.slotDimensions.recommendedSlotDimensions.height;
            customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
            customizedProductModelView.slots.Add(slotDimension);

            CustomizedProductDTO fatherCustomizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var childCustomizedProduct = await httpClient.PostAsJsonAsync(
                                String.Format(
                                                baseUri + "/{0}/slots/{1}", fatherCustomizedProductDTO.id,
                                                fatherCustomizedProductDTO.slotListDTO.First().Id
                                            ),
                                            customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, childCustomizedProduct.StatusCode);

            PostCustomizedProductModelView childCustomizedProductModelViewFromPost = JsonConvert.DeserializeObject<PostCustomizedProductModelView>(await childCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, childCustomizedProductModelViewFromPost.reference);
            Assert.Equal(customizedProductModelView.designation, childCustomizedProductModelViewFromPost.designation);
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), childCustomizedProductModelViewFromPost.customizedMaterialDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), childCustomizedProductModelViewFromPost.customizedDimensionsDTO.toEntity());
            Assert.NotEmpty(childCustomizedProductModelViewFromPost.slots);

            var fetchCreatedChildCustomizedProduct = await httpClient.GetAsync(
                                                        String.Format(baseUri + "/{0}", childCustomizedProductModelViewFromPost.id)
                                                    );

            Assert.Equal(HttpStatusCode.OK, fetchCreatedChildCustomizedProduct.StatusCode);

            CustomizedProductDTO fetchedCreatedChildCustomizedProduct = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchCreatedChildCustomizedProduct.Content.ReadAsStringAsync());

            Assert.Equal(customizedProductModelView.reference, fetchedCreatedChildCustomizedProduct.reference);
            Assert.Equal(customizedProductModelView.designation, fetchedCreatedChildCustomizedProduct.designation);
            Assert.Equal(customizedProductModelView.customizedMaterialDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedMaterialDTO.toEntity());
            Assert.Equal(customizedProductModelView.customizedDimensionsDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedDimensionsDTO.toEntity());
            Assert.NotEmpty(fetchedCreatedChildCustomizedProduct.slotListDTO);

            var fetchUpdatedFatherCustomizedProduct = await httpClient.GetAsync(
                                                    String.Format(baseUri + "/{0}", fatherCustomizedProductDTO.id)
                                                );

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedFatherCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedFather = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedFatherCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEmpty(updatedFather.slotListDTO.First().customizedProducts);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().reference, fetchedCreatedChildCustomizedProduct.reference);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().designation, fetchedCreatedChildCustomizedProduct.designation);
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().customizedDimensionsDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedDimensionsDTO.toEntity());
            Assert.Equal(updatedFather.slotListDTO.First().customizedProducts.First().customizedMaterialDTO.toEntity(), fetchedCreatedChildCustomizedProduct.customizedMaterialDTO.toEntity());
            Assert.NotEmpty(updatedFather.slotListDTO.First().customizedProducts);
            Assert.NotEmpty(updatedFather.slotListDTO.First().customizedProducts.First().slotListDTO);

            return fetchedCreatedChildCustomizedProduct;
        } */

       /*  [Fact, TestPriority(27)]
        public async Task ensurePutOfInvalidDesignationReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.designation = "";

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(28)]
        public async Task ensurePutOfInvalidReferenceReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.reference = "";

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(29)]
        public async Task ensurePutOfInvalidCustomizedDimensionsReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedDimensions = new CustomizedDimensionsDTO();

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(30)]
        public async Task ensurePutOfInvalidCustomizedMaterialColorReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.color = new ColorDTO();

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(31)]
        public async Task ensurePutOfInvalidCustomizedMaterialFinishReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.finish = new FinishDTO();

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact, TestPriority(32)]
        public async Task ensurePutOfInvalidCustomizedMaterialReturnsBadRequest()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(33)]
        public async Task ensureUpdatingReferenceIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.reference = "new reference";

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.reference, updatedCustomizedProductDTO.reference);
            Assert.Equal(updateCustomizedProductModelView.reference, updatedCustomizedProductDTO.reference);
        }

        [Fact, TestPriority(34)]
        public async Task ensureUpdatingDesignationIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.designation = "new designation";

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.designation, updatedCustomizedProductDTO.designation);
            Assert.Equal(updateCustomizedProductModelView.designation, updatedCustomizedProductDTO.designation);
        }

        [Fact, TestPriority(35)]
        public async Task ensureUpdatingCustomizedMaterialColorIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material = new MaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material.id = customizedProductDTO.productDTO.productMaterials.First().id;
            ColorDTO updateColorDTO = new ColorDTO();
            updateColorDTO.name = customizedProductDTO.productDTO.productMaterials.First().colors.Last().name;
            updateColorDTO.red = customizedProductDTO.productDTO.productMaterials.First().colors.Last().red;
            updateColorDTO.green = customizedProductDTO.productDTO.productMaterials.First().colors.Last().green;
            updateColorDTO.blue = customizedProductDTO.productDTO.productMaterials.First().colors.Last().blue;
            updateColorDTO.alpha = customizedProductDTO.productDTO.productMaterials.First().colors.Last().alpha;
            updateCustomizedProductModelView.customizedMaterial.color = updateColorDTO;

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.customizedMaterialDTO.color.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.color.toEntity());
            Assert.Equal(updateCustomizedProductModelView.customizedMaterial.color.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.color.toEntity());
        }

        [Fact, TestPriority(36)]
        public async Task ensureUpdatingCustomizedMaterialFinishIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material = new MaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material.id = customizedProductDTO.productDTO.productMaterials.First().id;
            FinishDTO updateFinishDTO = new FinishDTO();
            updateFinishDTO.description = customizedProductDTO.productDTO.productMaterials.First().finishes.Last().description;
            updateCustomizedProductModelView.customizedMaterial.finish = updateFinishDTO;

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.customizedMaterialDTO.finish.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.finish.toEntity());
            Assert.Equal(updateCustomizedProductModelView.customizedMaterial.finish.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.finish.toEntity());
        }

        [Fact, TestPriority(37)]
        public async Task ensureUpdatingCustomizedDimensionsIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedDimensions = new CustomizedDimensionsDTO();
            updateCustomizedProductModelView.customizedDimensions.depth = 15;
            updateCustomizedProductModelView.customizedDimensions.width = 15;
            updateCustomizedProductModelView.customizedDimensions.height = 15;

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.customizedDimensionsDTO.toEntity(), updatedCustomizedProductDTO.customizedDimensionsDTO.toEntity());
            Assert.Equal(updateCustomizedProductModelView.customizedDimensions.toEntity(), updatedCustomizedProductDTO.customizedDimensionsDTO.toEntity());
        }

        [Fact, TestPriority(38)]
        public async Task ensureUpdatingCustomizedMaterialIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();

            UpdateCustomizedProductModelView updateCustomizedProductModelView = new UpdateCustomizedProductModelView();

            updateCustomizedProductModelView.customizedMaterial = new CustomizedMaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material = new MaterialDTO();
            updateCustomizedProductModelView.customizedMaterial.material.id = customizedProductDTO.productDTO.productMaterials.Last().id;
            ColorDTO updateColorDTO = new ColorDTO();
            updateColorDTO.name = customizedProductDTO.productDTO.productMaterials.Last().colors.Last().name;
            updateColorDTO.red = customizedProductDTO.productDTO.productMaterials.Last().colors.Last().red;
            updateColorDTO.green = customizedProductDTO.productDTO.productMaterials.Last().colors.Last().green;
            updateColorDTO.blue = customizedProductDTO.productDTO.productMaterials.Last().colors.Last().blue;
            updateColorDTO.alpha = customizedProductDTO.productDTO.productMaterials.Last().colors.Last().alpha;
            updateCustomizedProductModelView.customizedMaterial.color = updateColorDTO;
            FinishDTO updateFinishDTO = new FinishDTO();
            updateFinishDTO.description = customizedProductDTO.productDTO.productMaterials.Last().finishes.Last().description;
            updateCustomizedProductModelView.customizedMaterial.finish = updateFinishDTO;

            var response = await httpClient.PutAsJsonAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id), updateCustomizedProductModelView);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fetchUpdatedCustomizedProduct = await httpClient.GetAsync(String.Format(baseUri + "/{0}", customizedProductDTO.id));

            Assert.Equal(HttpStatusCode.OK, fetchUpdatedCustomizedProduct.StatusCode);

            CustomizedProductDTO updatedCustomizedProductDTO = JsonConvert.DeserializeObject<CustomizedProductDTO>(await fetchUpdatedCustomizedProduct.Content.ReadAsStringAsync());

            Assert.NotEqual(customizedProductDTO.customizedMaterialDTO.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.toEntity());
            Assert.Equal(updateCustomizedProductModelView.customizedMaterial.color.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.color.toEntity());
            Assert.Equal(updateCustomizedProductModelView.customizedMaterial.finish.toEntity(), updatedCustomizedProductDTO.customizedMaterialDTO.finish.toEntity());
        }

        [Fact, TestPriority(39)]
        public async Task ensureDeleteSlotReturnsBadRequestIfCustomizedProductDoesntExist()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.DeleteAsync(String.Format(baseUri+"/{0}/slots/{1}",-1,customizedProductDTO.slotListDTO.First().Id));
        
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(40)]
        public async Task ensureDeleteSlotReturnsBadRequestIfCustomizedProductDoesntSupportSlots()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithoutSlotsIsCreatedSuccessfully();
            CustomizedProductDTO otherCustomizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.DeleteAsync(String.Format(baseUri+"/{0}/slots/{1}",customizedProductDTO.id,otherCustomizedProductDTO.slotListDTO.First().Id));
        
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(41)]
        public async Task ensureDeleteSlotReturnsBadRequestIfSlotDoesntExist()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.DeleteAsync(String.Format(baseUri+"/{0}/slots/{1}",customizedProductDTO.id,-1));
        
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(42)]
        public async Task ensureDeleteSlotIsSuccessful()
        {
            CustomizedProductDTO customizedProductDTO = await ensureCustomizedProductWithSlotsIsCreatedSuccessfully();

            var response = await httpClient.DeleteAsync(String.Format(baseUri+"/{0}/slots/{1}",customizedProductDTO.id,customizedProductDTO.slotListDTO.First().Id));
        
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        } */

  /*       [Fact, TestPriority(43)]
        public async Task ensureDeleteCustomizedProductFromSlotReturnsBadRequestIfFatherDoesntExist()
        {

        }

        [Fact, TestPriority(43)]
        public async Task ensureDeleteCustomizedProductFromSlotReturnsBadRequestIfChildDoesntExist()
        {

        }

        [Fact, TestPriority(43)]
        public async Task ensureDeleteCustomizedProductFromSlotReturnsBadRequestIfSlotDoesntExist()
        {

        }

        [Fact, TestPriority(44)]
        public async Task ensureDeleteCustomizedProductFromSlotReturnsBadRequestIfFatherDoesntSupportSlots()
        {

        } */
    }
}