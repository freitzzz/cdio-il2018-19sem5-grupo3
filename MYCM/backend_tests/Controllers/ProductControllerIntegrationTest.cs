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
using Microsoft.AspNetCore.Mvc.Testing;
using core.modelview.productcategory;
using core.modelview.product;
using core.modelview.measurement;
using core.modelview.dimension;
using core.services;
using core.modelview;
using backend.utils;
using core.modelview.productmaterial;
using core.domain;
using core.modelview.component;
using core.modelview.productslotwidths;
using core.modelview.material;

namespace backend_tests.Controllers
{

    /// <summary>
    /// Integration Tests for Products Collection API
    /// </summary>
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class assertDimensionModelViewFromPost : IClassFixture<TestFixture<TestStartupSQLite>>
    {
        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string PRODUCTS_URI = "mycm/api/products";

        /// <summary>
        /// Constant that represents the message that occurs if the product reference is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The product reference is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product designation is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The product designation is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product's model file name is invalid.
        /// </summary>
        private const string INVALID_PRODUCT_MODEL_FILENAME = "The model's filename is invalid";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no products
        /// are found
        /// </summary>
        private const string NO_PRODUCTS_FOUND_REFERENCE = "No products found.";

        /// <summary>
        /// Constant that represents the message that occurs if the product complementary products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_MATERIALS = "The materials which the product can be made of are invalid";

        /// <summary>
        /// Constant that represents the message that occurs if the product restrinctions are invalid
        /// </summary>
        private const string INVALID_PRODUCT_DIMENSIONS = "The product dimensions are invalid";

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
        public assertDimensionModelViewFromPost(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });
        }

        [Fact, TestPriority(-6)]
        public async void ensureDeletingProductMaterialReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/1/materials/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService deleteMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, deleteMessage.message);
        }

        [Fact, TestPriority(-5)]
        public async void ensureDeletingComponentReturnsNotFoundIfNoProductsAreAvailable()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/1/components/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService deleteMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, deleteMessage.message);
        }

        [Fact, TestPriority(-4)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfNoProductsAreAvailable()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/1/dimensions/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService deleteMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, deleteMessage.message);
        }

        [Fact, TestPriority(-3)]
        public async void ensureDeletingProductReturnsNotFoundIfThereAreNoProductsAvailable()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(-2)]
        public async void ensureUpdateProductReturnsNotFoundWhenThereAreNoProductsAvailable()
        {
            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = "new designation -2";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/1", updateProduct);

            Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, updateMessage.message);
        }

        [Fact, TestPriority(-1)]
        public async void ensureGetAllBaseProductsReturnsOkWithCorrectContent()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("31");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("31");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 31");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 31");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 31", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getAllBaseProductsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/base");

            GetAllProductsModelView getAllResponseContent = await getAllBaseProductsResponse.Content.ReadAsAsync<GetAllProductsModelView>();

            Assert.Equal(productModelViewFromGet.reference, getAllResponseContent[0].reference);
            Assert.Equal(productModelViewFromGet.designation, getAllResponseContent[0].designation);
            Assert.Equal(productModelViewFromGet.modelFilename, getAllResponseContent[0].modelFilename);
            Assert.Equal(productModelViewFromGet.id, getAllResponseContent[0].id);
        }

        [Fact, TestPriority(0)]
        public async void testCRUDOperations()
        {

        }

        [Fact, TestPriority(1)]
        public async void ensureGetAllProductsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        //!This one fails because test 2 posts a product
        [Fact, TestPriority(2)]
        public async void ensureGetProductByIdReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        //!This one fails because test 2 posts a product
        [Fact, TestPriority(3)]
        public async void ensureGetAllBaseProductsReturnsNotFoundWhenNoBaseProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/base");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(4)]
        public async void ensureGetAllProductsReturnsAllAvailableProducts()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("2");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("2");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "2");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getAllResponse = await httpClient.GetAsync(PRODUCTS_URI);

            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

            GetAllProductsModelView getAllProductsModelView = await getAllResponse.Content.ReadAsAsync<GetAllProductsModelView>();

            foreach (GetBasicProductModelView modelView in getAllProductsModelView)
            {
                Assert.Equal(productModelViewFromPost.id, modelView.id);
                Assert.Equal(productModelViewFromPost.designation, modelView.designation);
                Assert.Equal(productModelViewFromPost.reference, modelView.reference);
                Assert.Equal(productModelViewFromPost.modelFilename, modelView.modelFilename);
            }
        }

        [Fact, TestPriority(5)]
        public async void ensureGetProductByIdReturnsNotFoundWhenIdIsInvalid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            /*             SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

                        Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message); */
        }

        [Fact, TestPriority(6)]
        public async void ensureCantAddANewProductMeasurementIfThereAreNoProductsAvailable()
        {
            AddMeasurementModelView modelView = createNewMeasurementModelView();

            var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/1/dimensions", modelView);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(7)]
        public async void ensureCantAddNewProductComponentsIfThereAreNoProductsAvailable()
        {
        }

        [Fact, TestPriority(8)]
        public async void ensureGetProductByReferenceReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/?reference=#666");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(11)]
        public async void ensureGetProductByReferenceReturnsNotFoundWhenReferenceIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/?reference=");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(12)]
        public async void ensureGetProductMeasurementsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/dimensions/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(13)]
        public async void ensureGetProductMeasurementsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/dimensions/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(16)]
        public async void ensureGetProductComponentsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/components/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(17)]
        public async void ensureGetProductComponentsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/components/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(18)]
        public async void ensureGetProductMaterialsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/materials/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(19)]
        public async void ensureGetProductMaterialsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/materials/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(20)]
        public async void ensureGetProductDimensionRestrictionsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/dimensions/1/restrictions");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(21)]
        public async void ensureGetProductDimensionRestrictionsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/dimensions/1/restrictions");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(22)]
        public async void ensureGetProductDimensionRestrictionsReturnsNotFoundWhenProductDimensionIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/dimensions/-1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(23)]
        public async void ensureGetProductComponentRestrictionsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/components/1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(24)]
        public async void ensureGetProductComponentRestrictionsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/components/1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(25)]
        public async void ensureGetProductComponentRestrictionsReturnsNotFoundWhenProductComponentIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/components/-1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(26)]
        public async void ensureGetProductMaterialRestrictionsReturnsNotFoundWhenNoProductsAreAvailable()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/materials/1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(27)]
        public async void ensureGetProductMaterialRestrictionsReturnsNotFoundWhenProductIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/-1/materials/1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(28)]
        public async void ensureGetProductMaterialRestrictionsReturnsNotFoundWhenMaterialIdIsntValid()
        {
            var response = await httpClient.GetAsync(PRODUCTS_URI + "/1/materials/-1/restrictions");

            SimpleJSONMessageService responseMessage = await response.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(29)]
        public async void ensureCreationOfProductWithoutComponentsAndWithoutSlotsIsPossible()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("29");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("29");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "29");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(30)]
        public async void ensureCreationOfProductWithSlotsAndNoComponentsIsPossible()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("30");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("30");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "30");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(31)]
        public async void ensureCreationOfProductWithNoSlotsAndWithComponentsIsPossible()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("31");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("31");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 31");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 31");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 31", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(32)]
        public async void ensureCreationOfProductWithInvalidReferenceReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("32");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("32");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "32");

            productModelView.reference = "";

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(33)]
        public async void ensureCreationOfProductWithInvalidDesignationReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("33");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("33");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "33");

            productModelView.designation = "";

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_DESIGNATION, responseMessage.message);
        }

        [Fact, TestPriority(34)]
        public async void ensureCreationOfProductWithInvalidModelFilenameReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("34");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("34");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "34");

            productModelView.modelFilename = "reallycool.flac";

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_MODEL_FILENAME, responseMessage.message);
        }

        [Fact, TestPriority(35)]
        public async void ensureCreationOfProductWithInvalidCategoryIdReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("35");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("35");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "35");

            productModelView.categoryId = -1;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Should we compare any messages here?
        }

        [Fact, TestPriority(36)]
        public async void ensureCantAddANewProductMeasurementIfTheProudctIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("36");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("36");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "36");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + -1 + "/dimensions", newMeasurementModelView);

            //!Bad Request or Not Found here?
            //TODO Add message comparison
            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);
        }

        [Fact, TestPriority(37)]
        public async void ensureGetProductByIdReturnsNotFoundIfTheIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("6");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("6");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "6");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByIdResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + 1);

            Assert.Equal(HttpStatusCode.NotFound, getByIdResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await getByIdResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(38)]
        public async void ensureGetProductByIdReturnsOkIfTheIdIsValidAndExists()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("6");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("6");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "6");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByIdResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, getByIdResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getByIdResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(39)]
        public async void ensureGetProductByReferenceReturnsNotFoundIfReferenceDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("9");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("9");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "9");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByReferenceResponse = await httpClient.GetAsync(PRODUCTS_URI + "/?reference=" + productModelViewFromPost.reference + "doesntexist");

            Assert.Equal(HttpStatusCode.NotFound, getByReferenceResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await getByReferenceResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(40)]
        public async void ensureGetProductByReferencesReturnsOkIfReferenceExistsAndIsValid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("10");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("10");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "10");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByReferenceResponse = await httpClient.GetAsync(PRODUCTS_URI + "/?reference=" + productModelViewFromPost.reference);

            Assert.Equal(HttpStatusCode.OK, getByReferenceResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getByReferenceResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(41)]
        public async void ensureGetProductMeasurementsReturnsNotFoundWhenProductIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("14");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("14");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "14");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByIdResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + 1 + "/dimensions");

            Assert.Equal(HttpStatusCode.NotFound, getByIdResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await getByIdResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(42)]
        public async void ensureGetProductMeasurementsReturnsOkWhenProductIdExistsAndIsValid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("15");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("15");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "15");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getByIdResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions");

            Assert.Equal(HttpStatusCode.OK, getByIdResponse.StatusCode);

            GetAllMeasurementsModelView modelViewFromGet = await getByIdResponse.Content.ReadAsAsync<GetAllMeasurementsModelView>();

            for (int i = 0; i < modelViewFromGet.Count; i++)
            {
                assertDimensionModelView(productModelView.measurements[i].depthDimension, modelViewFromGet[i].depth);
                assertDimensionModelView(productModelView.measurements[i].widthDimension, modelViewFromGet[i].width);
                assertDimensionModelView(productModelView.measurements[i].heightDimension, modelViewFromGet[i].height);
            }
        }

        [Fact, TestPriority(43)]
        public async void ensureAddingProductMeasurementToNonExistingProductReturnsNotFound()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("43");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("43");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "43");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + 1 + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.NotFound, postNewMeasurementResponse.StatusCode);
            //TODO Compare message
        }

        [Fact, TestPriority(44)]
        public async void ensureAddingProductMeasurementWithInvalidHeightReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("44");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("44");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "44");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelViewWithInvalidHeight();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(45)]
        public async void ensureAddingProductMeasurementWithInvalidWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("45");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("45");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "45");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelViewWithInvalidWidth();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(46)]
        public async void ensureAddingProductMeasurementWithInvalidDepthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("46");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("46");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "46");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelViewWithInvalidDepth();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(47)]
        public async void ensureAddingProductMeasurementWithInvalidUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("47");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("47");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "47");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            newMeasurementModelView.depthDimension.unit = "bajoras";

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(48)]
        public async void ensureAddingValidProductMeasurementToProductReturnsOk()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("43");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("43");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "43");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.OK, postNewMeasurementResponse.StatusCode);

            GetProductModelView modelViewFromNewMeasurementPost = await postNewMeasurementResponse.Content.ReadAsAsync<GetProductModelView>();

            assertDimensionModelView(newMeasurementModelView.depthDimension, modelViewFromNewMeasurementPost.measurements[1].depth);
            assertDimensionModelView(newMeasurementModelView.heightDimension, modelViewFromNewMeasurementPost.measurements[1].height);
            assertDimensionModelView(newMeasurementModelView.widthDimension, modelViewFromNewMeasurementPost.measurements[1].width);
        }

        [Fact, TestPriority(49)]
        public async void ensureCreationOfProductWithNullBodyReturnsBadRequest()
        {
            AddProductModelView productModelView = null;
            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(50)]
        public async void ensureCreationOfProductWithNullReferenceReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("50");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("50");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "50");

            productModelView.reference = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(51)]
        public async void ensureCreationOfProductWithNullDesignationReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("51");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("51");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "51");

            productModelView.designation = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_DESIGNATION, responseMessage.message);
        }

        [Fact, TestPriority(52)]
        public async void ensureCreationOfProductWithNullModelFilenameReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("52");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("52");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "52");

            productModelView.modelFilename = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_MODEL_FILENAME, responseMessage.message);
        }

        [Fact, TestPriority(53)]
        public async void ensureCreationOfProductWithNullMaterialListReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("53");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("53");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "53");

            productModelView.materials = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            Assert.Equal(INVALID_PRODUCT_MATERIALS, responseMessage.message);
        }

        [Fact, TestPriority(54)]
        public async void ensureCreationOfProductWithNullHeightReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("54");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("54");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "54");

            productModelView.measurements[0].heightDimension = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();
            //TODO Check if this is the correct message
            Assert.Equal(INVALID_PRODUCT_DIMENSIONS, responseMessage.message);
        }

        [Fact, TestPriority(55)]
        public async void ensureCreationOfProductWithNullWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("55");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("55");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "55");

            productModelView.measurements[0].widthDimension = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();
            //TODO Check if this is the correct message
            Assert.Equal(INVALID_PRODUCT_DIMENSIONS, responseMessage.message);
        }

        [Fact, TestPriority(56)]
        public async void ensureCreationOfProductWithNullDepthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("56");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("56");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "56");

            productModelView.measurements[0].depthDimension = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await postResponse.Content.ReadAsAsync<SimpleJSONMessageService>();
            //TODO Check if this is the correct message
            Assert.Equal(INVALID_PRODUCT_DIMENSIONS, responseMessage.message);
        }

        [Fact, TestPriority(57)]
        public async void ensureCreationOfProductWithNullDimensionUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("57");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("57");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "57");

            productModelView.measurements[0].heightDimension.unit = null;
            productModelView.measurements[0].widthDimension.unit = null;
            productModelView.measurements[0].depthDimension.unit = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(58)]
        public async void ensureCreationOfProductWithInvalidHeightReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("58");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("58");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "58");

            productModelView.measurements[0] = createNewMeasurementModelViewWithInvalidHeight();

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(59)]
        public async void ensureCreationOfProductWithInvalidWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("59");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("59");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "59");

            productModelView.measurements[0] = createNewMeasurementModelViewWithInvalidWidth();

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(60)]
        public async void ensureCreationOfProductWithInvalidDepthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("60");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("60");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "60");

            productModelView.measurements[0] = createNewMeasurementModelViewWithInvalidDepth();

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(61)]
        public async void ensureCreationOfProductWithInvalidDimensionUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("61");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("61");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "61");

            productModelView.measurements[0].heightDimension.unit = "bajoras";
            productModelView.measurements[0].widthDimension.unit = "flac is the best";
            productModelView.measurements[0].depthDimension.unit = "HA";

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(62)]
        public async void ensureCreationOfProductWithInvalidMinimumSlotWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("62");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("62");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "62");

            productModelView.slotWidths.minWidth = Double.MaxValue;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(63)]
        public async void ensureCreationOfProductWithInvalidMaximumSlotWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("63");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("63");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "63");

            productModelView.slotWidths.maxWidth = Double.MinValue;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(64)]
        public async void ensureCreationOfProductWithInvalidRecommendedSlotWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("64");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("64");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "64");

            productModelView.slotWidths.recommendedWidth = 0;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(65)]
        public async void ensureCreationOfProductWithInvalidSlotWidthUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("65");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("65");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "65");

            productModelView.slotWidths.unit = "i'm not a rapper";

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(66)]
        public async void ensureCreationOfProductWithNullSlotWidthUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("66");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("66");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "66");

            productModelView.slotWidths.unit = null;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(67)]
        public async void ensureCreationOfProductWithInvalidMaterialsReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("67");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("67");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "67");

            productModelView.materials[0].materialId = -1;

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(68)]
        public async void ensureAddingProductMeasurementWithNullBodyReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("68");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("68");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "68");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = null;

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(69)]
        public async void ensureAddingProductMeasurementWithNullHeightReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("69");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("69");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "69");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            newMeasurementModelView.heightDimension = null;

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(70)]
        public async void ensureAddingProductMeasurementWithNullDepthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("70");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("70");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "70");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            newMeasurementModelView.depthDimension = null;

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(71)]
        public async void ensureAddingProductMeasurementWithNullWidthReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("71");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("71");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "71");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            newMeasurementModelView.widthDimension = null;

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(72)]
        public async void ensureAddingProductMeasurementWithNullDimensionUnitReturnsBadRequest()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("72");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("72");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "72");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            AddMeasurementModelView newMeasurementModelView = createNewMeasurementModelView();

            newMeasurementModelView.widthDimension.unit = null;
            newMeasurementModelView.depthDimension.unit = null;
            newMeasurementModelView.heightDimension.unit = null;

            var postNewMeasurementResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id + "/dimensions", newMeasurementModelView);

            Assert.Equal(HttpStatusCode.BadRequest, postNewMeasurementResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(73)]
        public async void ensureCreationOfProductWithSlotsAndComponentsIsPossible()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("73");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("73");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 73");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 73");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponentsAndWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "father product 73", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
        }

        [Fact, TestPriority(74)]
        public async void ensureGetProductComponentsForNonExistingIdReturnsNotFound()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("74");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("74");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 74");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 74");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 74", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getComponentsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1 + "/components");

            Assert.Equal(HttpStatusCode.NotFound, getComponentsResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(75)]
        public async void ensureGetProductComponentsForProductWithNoComponentsReturnsNotFound()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("75");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("75");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "75");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getComponentsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components");

            //!BadRequest or NotFound?
            Assert.Equal(HttpStatusCode.NotFound, getComponentsResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(76)]
        public async void ensureGetProductComponentsReturnsOkForValidProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("76");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("76");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 76");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 76");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 76", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getComponentsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components");

            Assert.Equal(HttpStatusCode.OK, getComponentsResponse.StatusCode);

            GetAllComponentsModelView getAllComponentsModelView = await getComponentsResponse.Content.ReadAsAsync<GetAllComponentsModelView>();

            for (int i = 0; i < getAllComponentsModelView.Count; i++)
            {
                Assert.Equal(productModelViewFromGet.components[i].mandatory, getAllComponentsModelView[i].mandatory);
                Assert.Equal(productModelViewFromGet.components[i].reference, getAllComponentsModelView[i].reference);
                Assert.Equal(productModelViewFromGet.components[i].designation, getAllComponentsModelView[i].designation);
                Assert.Equal(productModelViewFromGet.components[i].modelFilename, getAllComponentsModelView[i].modelFilename);
                Assert.Equal(productModelViewFromGet.components[i].fatherProductId, getAllComponentsModelView[i].fatherProductId);
            }
        }

        [Fact, TestPriority(77)]
        public async void ensureGetProductMaterialsReturnsNotFoundForNonExistingId()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("75");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("75");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "75");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getMaterialsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1 + "/materials");

            Assert.Equal(HttpStatusCode.NotFound, getMaterialsResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(78)]
        public async void ensureGetProductMaterialsReturnsOkForValidProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("78");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("78");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "78");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var getMaterialsResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/materials");

            Assert.Equal(HttpStatusCode.OK, getMaterialsResponse.StatusCode);

            GetAllMaterialsModelView getMaterialsResponseContent = await getMaterialsResponse.Content.ReadAsAsync<GetAllMaterialsModelView>();

            for (int i = 0; i < getMaterialsResponseContent.Count; i++)
            {
                Assert.Equal(productModelViewFromGet.materials[i].id, getMaterialsResponseContent[i].id);
                Assert.Equal(productModelViewFromGet.materials[i].designation, getMaterialsResponseContent[i].designation);
                Assert.Equal(productModelViewFromGet.materials[i].reference, getMaterialsResponseContent[i].reference);
            }
        }

        [Fact, TestPriority(79)]
        public async void ensureUpdateProductReturnsNotFoundIfIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("79");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("79");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "79");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = "new designation 79";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, updateMessage.message);
        }

        [Fact, TestPriority(80)]
        public async void ensureUpdateProductReturnsNotFoundIfIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("80");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("80");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "80");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = "new designation 80";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1, updateProduct);

            Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, updateMessage.message);
        }

        [Fact, TestPriority(81)]
        public async void ensureUpdateProductReturnsBadRequestIfNewReferenceIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("81");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("81");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "81");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.reference = "";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_REFERENCE, updateMessage.message);
        }

        [Fact, TestPriority(82)]
        public async void ensureUpdateProductReturnsBadRequestIfBodyIsNull()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("82");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("82");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "82");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = null;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //TODO Compare message
        }

        [Fact, TestPriority(83)]
        public async void ensureUpdateProductReturnsBadRequestIfNewReferenceIsNull()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("83");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("83");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "83");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.reference = null;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_REFERENCE, updateMessage.message);
        }


        [Fact, TestPriority(84)]
        public async void ensureUpdateProductReturnsBadRequestIfNewDesignationIsNull()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("84");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("84");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "84");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = null;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_DESIGNATION, updateMessage.message);
        }

        [Fact, TestPriority(85)]
        public async void ensureUpdateProductReturnsBadRequestIfNewDesignationIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("85");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("85");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "85");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = "";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_DESIGNATION, updateMessage.message);
        }

        [Fact, TestPriority(86)]
        public async void ensureUpdateProductReturnsBadRequestIfNewModelFilenameIsNull()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("86");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("86");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "86");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.modelFilename = null;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_MODEL_FILENAME, updateMessage.message);
        }

        [Fact, TestPriority(87)]
        public async void ensureUpdateProductReturnsBadRequestIfNewModelFilenameIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("87");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("87");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "87");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.modelFilename = "";

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(INVALID_PRODUCT_MODEL_FILENAME, updateMessage.message);
        }

        [Fact, TestPriority(88)]
        public async void ensureUpdateProductReturnsBadRequestIfNewCategoryIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("88");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("88");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "88");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.categoryId = -1;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //TODO Compare messages
        }

        [Fact, TestPriority(89)]
        public async void ensureUpdateProductReturnsBadRequestIfNewCategoryIdDoesNotExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("89");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("89");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "89");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.categoryId = categoryModelViewFromPost.id + 1;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            SimpleJSONMessageService updateMessage = await updateResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //TODO Compare messages
        }

        [Fact, TestPriority(90)]
        public async void ensureUpdateProductIsSuccessfulIfAllFieldsAreValid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("90");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            AddProductCategoryModelView otherCategoryModelView = createCategoryModelView("90 New");
            var otherCategoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", otherCategoryModelView);
            GetProductCategoryModelView otherCategoryModelViewFromPost = await otherCategoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();


            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("90");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "90");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            UpdateProductPropertiesModelView updateProduct = new UpdateProductPropertiesModelView();
            updateProduct.designation = "new designation 90";
            updateProduct.reference = "new reference 90";
            updateProduct.modelFilename = "newFile90.obj";
            updateProduct.categoryId = otherCategoryModelViewFromPost.id;

            var updateResponse = await httpClient.PutAsJsonAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id, updateProduct);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            GetProductModelView updateResponseContent = await updateResponse.Content.ReadAsAsync<GetProductModelView>();

            Assert.Equal(updateProduct.designation, updateResponseContent.designation);
            Assert.Equal(updateProduct.reference, updateResponseContent.reference);
            Assert.Equal(updateProduct.modelFilename, updateResponseContent.modelFilename);
            Assert.Equal(updateProduct.categoryId, updateResponseContent.category.id);
        }

        [Fact, TestPriority(91)]
        public async void ensureDeletingProductReturnsNotFoundIfIdIsInvalid()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/-1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService responseMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, responseMessage.message);
        }

        [Fact, TestPriority(92)]
        public async void ensureDeletingProductReturnsNotFoundIfIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("92");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("92");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "92");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService deleteMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, deleteMessage.message);
        }

        [Fact, TestPriority(93)]
        public async void ensureDeletingProductReturnsNoContentAndDisablesIt()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("93");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("93");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "93");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id);

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var isProductDisabled = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id);

            Assert.Equal(HttpStatusCode.NotFound, isProductDisabled.StatusCode);

            SimpleJSONMessageService disableMessage = await isProductDisabled.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, disableMessage.message);
        }

        [Fact, TestPriority(94)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfProductIdIsInvalid()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/-1/dimensions/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            SimpleJSONMessageService disableMessage = await deleteResponse.Content.ReadAsAsync<SimpleJSONMessageService>();

            //!Check if this is correct
            Assert.Equal(NO_PRODUCTS_FOUND_REFERENCE, disableMessage.message);
        }

        [Fact, TestPriority(95)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("95");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("95");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "95");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1 + "/dimensions/" + productModelViewFromGet.measurements[0].measurementId);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(96)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfMeasurementIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("96");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("96");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "96");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/dimensions/-1");

            //!BadRequest or NotFound?
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(97)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfMeasurementIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("97");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("97");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "97");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/dimensions/" + productModelViewFromGet.measurements[0].measurementId + 1);

            //!BadRequest or NotFound?
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(98)]
        public async void ensureDeletingProductMeasurementReturnsNotFoundIfMeasurementBelongsToAnotherProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("98");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("98");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "98");
            var otherProductModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "98 other");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);
            var otherPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, otherProductModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherPostResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherProductModelViewFromPost = await otherPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);
            assertProductModelView(otherProductModelView, otherProductModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);
            var otherGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + otherProductModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherProductModelViewFromGet = await otherGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
            assertProductModelView(otherProductModelViewFromPost, otherProductModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/dimensions/" + otherProductModelViewFromGet.measurements[0].measurementId);

            //!BadRequest or NotFound?
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(99)]
        public async void ensureDeletingProductMeasurementFromProductReturnsNoContent()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("96");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("96");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "96");

            productModelView.measurements.Add(createNewMeasurementModelView());

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/dimensions/" + productModelViewFromGet.measurements[1].measurementId);

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact, TestPriority(100)]
        public async void ensureDeletingComponentReturnsNotFoundIfProductIdIsInvalid()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/-1/components/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(101)]
        public async void ensureDeletingComponentReturnsNotFoundIfProductIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("101");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("101");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 101");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 101");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 101", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1 + "/components/" + productModelViewFromGet.components[0].id);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(102)]
        public async void ensureDeletingComponentReturnsNotFoundIfProductComponentIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("102");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("102");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 102");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 102");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 102", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components/-1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(103)]
        public async void ensureDeletingComponentReturnsNotFoundIfProductComponentIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("103");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("103");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 103");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 103");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 103", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components/" + productModelViewFromGet.components[0].id + 1);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(104)]
        public async void ensureDeletingComponentFromProductWithNoComponentsReturnsNotFound()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("104");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("104");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "104");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(105)]
        public async void ensureDeletingComponentFromAnotherProductReturnsNotFound()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("105");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("105");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 105");
            var otherFirstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "other component 1 105");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);
            var otherFirstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, otherFirstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherFirstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherFirstComponentModelViewFromPost = await otherFirstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);
            assertProductModelView(otherFirstComponentModelView, otherFirstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);
            var otherFirstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + otherFirstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherFirstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherFirstComponentModelViewFromGet = await otherFirstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);
            assertProductModelView(otherFirstComponentModelViewFromPost, otherFirstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 105");
            var otherSecondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "other component 2 105");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);
            var otherSecondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, otherSecondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherSecondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherSecondComponentModelViewFromPost = await otherSecondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);
            assertProductModelView(otherSecondComponentModelView, otherSecondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);
            var otherSecondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + otherSecondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherSecondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherSecondComponentModelViewFromGet = await otherSecondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);
            assertProductModelView(otherSecondComponentModelViewFromPost, otherSecondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 105", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);
            var otherProductModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "other father product 105", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);
            var otherProductPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, otherProductModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherProductPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();
            var otherProductModelViewFromPost = await otherProductPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);
            assertProductModelView(otherProductModelView, otherProductModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);
            var otherProductGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + otherProductModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, otherProductGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherProductModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
            assertProductModelView(otherProductModelViewFromPost, otherProductModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components/" + otherProductModelViewFromGet.components[0].id);

            //!Bad Request or Not Found?
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare messages
        }

        [Fact, TestPriority(106)]
        public async void ensureDeletingComponentFromProductReturnsNoContentAndThatTheComponentWasDisabled()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("103");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("103");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 103");

            var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

            Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelView, firstComponentModelViewFromPost);

            var firstComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + firstComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, firstComponentGetResponse.StatusCode);

            GetProductModelView firstComponentModelViewFromGet = await firstComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(firstComponentModelViewFromPost, firstComponentModelViewFromGet);

            var secondComponentModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 103");

            var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

            Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelView, secondComponentModelViewFromPost);

            var secondComponentGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + secondComponentModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, secondComponentGetResponse.StatusCode);

            GetProductModelView secondComponentModelViewFromGet = await secondComponentGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(secondComponentModelViewFromPost, secondComponentModelViewFromGet);

            var productModelView = createProductWithComponents(categoryModelViewFromPost, materialModelViewFromPost, "father product 103", firstComponentModelViewFromGet.id, secondComponentModelViewFromGet.id);

            var productPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, productPostResponse.StatusCode);

            var productModelViewFromPost = await productPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var productGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, productGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await productGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/components/" + productModelViewFromGet.components[0].id);

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDeleteResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id);

            Assert.Equal(HttpStatusCode.OK, getAfterDeleteResponse.StatusCode);

            GetProductModelView getAfterDeleteContent = await getAfterDeleteResponse.Content.ReadAsAsync<GetProductModelView>();

            Assert.NotEqual(productModelViewFromGet.components.Count, getAfterDeleteContent.components.Count);
            Assert.Equal(productModelViewFromGet.components[1].id, getAfterDeleteContent.components[0].id);
            Assert.Equal(productModelViewFromGet.components[1].fatherProductId, getAfterDeleteContent.components[0].fatherProductId);
            Assert.Equal(productModelViewFromGet.components[1].reference, getAfterDeleteContent.components[0].reference);
            Assert.Equal(productModelViewFromGet.components[1].designation, getAfterDeleteContent.components[0].designation);
            Assert.Equal(productModelViewFromGet.components[1].modelFilename, getAfterDeleteContent.components[0].modelFilename);
            Assert.Equal(productModelViewFromGet.components[1].mandatory, getAfterDeleteContent.components[0].mandatory);
        }

        [Fact, TestPriority(107)]
        public async void ensureDeletingProductMaterialReturnsNotFoundWhenProductIdIsInvalid()
        {
            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/-1/materials/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(108)]
        public async void ensureDeletingProductMaterialReturnsNotFoundIfProductIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("108");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("108");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "108");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + 1 + "/materials/" + productModelViewFromGet.materials[0].id);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(109)]
        public async void ensureDeletingProductMaterialReturnsNotFoundIfMaterialIdIsInvalid()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("109");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("109");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "109");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/materials/-1");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(110)]
        public async void ensureDeletingProductMaterialReturnsNotFoundIfMaterialIdDoesntExist()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("110");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("110");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "110");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/materials/" + productModelViewFromGet.materials[0].id + 1);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(111)]
        public async void ensureDeletingProductMaterialReturnsNotFoundIfMaterialBelongsToAnotherProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("111");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("111");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            MaterialDTO otherMaterialDTO = createMaterialDTO("other material 111");
            var otherMaterialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", otherMaterialDTO);
            AddProductMaterialModelView otherMaterialModelViewFromPost = await otherMaterialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "111");
            var otherProductModelView = createProductWithSlots(categoryModelViewFromPost, otherMaterialModelViewFromPost, "other product 111");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);
            var otherPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, otherProductModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherPostResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherProductModelViewFromPost = await otherPostResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);
            assertProductModelView(otherProductModelView, otherProductModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);
            var otherGetResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + otherProductModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Created, otherGetResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();
            GetProductModelView otherProductModelViewFromGet = await otherGetResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);
            assertProductModelView(otherProductModelViewFromPost, otherProductModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + otherProductModelViewFromGet.id + "/materials/" + productModelViewFromGet.materials[0].id);

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(112)]
        public async void ensureDeleteProductMaterialReturnsBadRequestIfItsDeletingTheOnlyMaterialOfTheProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("112");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("112");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();

            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "112");

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/materials/" + productModelViewFromGet.materials[0].id);

            Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);

            //TODO Compare Messages
        }

        [Fact, TestPriority(113)]
        public async void ensureDeleteProductMaterialReturnsNoContentAndMaterialIsntAssociatedWithProduct()
        {
            AddProductCategoryModelView categoryModelView = createCategoryModelView("113");
            var categoryResponse = await httpClient.PostAsJsonAsync("mycm/api/categories", categoryModelView);
            GetProductCategoryModelView categoryModelViewFromPost = await categoryResponse.Content.ReadAsAsync<GetProductCategoryModelView>();

            //!Update this when MaterialDTOs are replaced with Model Views
            MaterialDTO materialDTO = createMaterialDTO("113");
            var materialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", materialDTO);
            AddProductMaterialModelView materialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();
            MaterialDTO otherMaterialDTO = createMaterialDTO("113");
            var otherMaterialResponse = await httpClient.PostAsJsonAsync("mycm/api/materials", otherMaterialDTO);
            AddProductMaterialModelView otherMaterialModelViewFromPost = await materialResponse.Content.ReadAsAsync<AddProductMaterialModelView>();


            var productModelView = createProductWithSlots(categoryModelViewFromPost, materialModelViewFromPost, "113");

            productModelView.materials.Add(otherMaterialModelViewFromPost);

            var postResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productModelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetProductModelView productModelViewFromPost = await postResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelView, productModelViewFromPost);

            var getResponse = await httpClient.GetAsync(PRODUCTS_URI + "/" + productModelViewFromPost.id);

            Assert.Equal(HttpStatusCode.Created, getResponse.StatusCode);

            GetProductModelView productModelViewFromGet = await getResponse.Content.ReadAsAsync<GetProductModelView>();

            assertProductModelView(productModelViewFromPost, productModelViewFromGet);

            var deleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id + "/materials/" + productModelViewFromGet.materials[0].id);

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDeleteResponse = await httpClient.DeleteAsync(PRODUCTS_URI + "/" + productModelViewFromGet.id);

            Assert.Equal(HttpStatusCode.OK, getAfterDeleteResponse.StatusCode);

            GetProductModelView getAfterDeleteContent = await getAfterDeleteResponse.Content.ReadAsAsync<GetProductModelView>();

            Assert.NotEqual(productModelViewFromGet.materials.Count, getAfterDeleteContent.materials.Count);
            Assert.Equal(productModelViewFromGet.materials[1].id, getAfterDeleteContent.materials[0].id);
            Assert.Equal(productModelViewFromGet.materials[1].designation, getAfterDeleteContent.materials[0].designation);
            Assert.Equal(productModelViewFromGet.materials[1].reference, getAfterDeleteContent.materials[0].reference);
        }

        private void assertProductModelView(GetProductModelView modelViewFromPost, GetProductModelView modelViewFromGet)
        {

            Assert.Equal(modelViewFromPost.designation, modelViewFromGet.designation);
            Assert.Equal(modelViewFromPost.reference, modelViewFromGet.reference);
            Assert.Equal(modelViewFromPost.modelFilename, modelViewFromGet.modelFilename);
            Assert.Equal(modelViewFromPost.category.id, modelViewFromGet.category.id);
            for (int i = 0; i < modelViewFromPost.materials.Count; i++)
            {
                Assert.Equal(modelViewFromPost.materials[i].id, modelViewFromGet.materials[i].id);
            }
            for (int j = 0; j < modelViewFromPost.measurements.Count; j++)
            {
                assertDimensionModelView(modelViewFromPost.measurements[j].depth, modelViewFromGet.measurements[j].depth);
                assertDimensionModelView(modelViewFromPost.measurements[j].width, modelViewFromGet.measurements[j].width);
                assertDimensionModelView(modelViewFromPost.measurements[j].height, modelViewFromGet.measurements[j].height);
            }
            if (modelViewFromPost.components != null)
            {
                for (int k = 0; k < modelViewFromPost.components.Count; k++)
                {
                    assertProductComponentModelView(modelViewFromPost.components[k], modelViewFromGet.components[k]);
                }
            }
            if (modelViewFromPost.slotWidths != null)
            {
                assertProductSlotWidthsModelView(modelViewFromPost.slotWidths, modelViewFromGet.slotWidths);
            }
        }

        private void assertProductModelView(AddProductModelView sentModelView, GetProductModelView modelViewFromPost)
        {
            Assert.Equal(sentModelView.designation, modelViewFromPost.designation);
            Assert.Equal(sentModelView.reference, modelViewFromPost.reference);
            Assert.Equal(sentModelView.modelFilename, modelViewFromPost.modelFilename);
            Assert.Equal(sentModelView.categoryId, modelViewFromPost.category.id);
            for (int i = 0; i < sentModelView.materials.Count; i++)
            {
                Assert.Equal(sentModelView.materials[i].materialId, modelViewFromPost.materials[i].id);
            }
            for (int j = 0; j < sentModelView.measurements.Count; j++)
            {
                assertDimensionModelView(sentModelView.measurements[j].depthDimension, modelViewFromPost.measurements[j].depth);
                assertDimensionModelView(sentModelView.measurements[j].widthDimension, modelViewFromPost.measurements[j].width);
                assertDimensionModelView(sentModelView.measurements[j].heightDimension, modelViewFromPost.measurements[j].height);
            }
            if (sentModelView.components != null)
            {
                for (int k = 0; k < sentModelView.components.Count; k++)
                {
                    assertProductComponentModelView(sentModelView.components[k], modelViewFromPost.components[k]);
                }
            }
            if (sentModelView.slotWidths != null)
            {
                assertProductSlotWidthsModelView(sentModelView.slotWidths, modelViewFromPost.slotWidths);
            }
        }

        private void assertProductComponentModelView(AddComponentModelView sentModelView, GetBasicComponentModelView modelViewFromPost)
        {
            Assert.Equal(sentModelView.mandatory, modelViewFromPost.mandatory);
            Assert.Equal(sentModelView.childProductId, modelViewFromPost.id);
        }

        private void assertProductComponentModelView(GetBasicComponentModelView modelViewFromPost, GetBasicComponentModelView modelViewFromGet)
        {
            Assert.Equal(modelViewFromPost.mandatory, modelViewFromGet.mandatory);
            Assert.Equal(modelViewFromPost.id, modelViewFromGet.id);
        }

        private void assertProductSlotWidthsModelView(AddProductSlotWidthsModelView sentModelView, GetProductSlotWidthsModelView modelViewFromPost)
        {
            sentModelView.maxWidth = convertUnit(sentModelView.maxWidth, sentModelView.unit);
            sentModelView.minWidth = convertUnit(sentModelView.minWidth, sentModelView.unit);
            sentModelView.recommendedWidth = convertUnit(sentModelView.recommendedWidth, sentModelView.unit);

            Assert.Equal(sentModelView.maxWidth, modelViewFromPost.maxWidth);
            Assert.Equal(sentModelView.minWidth, modelViewFromPost.minWidth);
            Assert.Equal(sentModelView.recommendedWidth, modelViewFromPost.recommendedWidth);
        }

        private void assertProductSlotWidthsModelView(GetProductSlotWidthsModelView modelViewFromPost, GetProductSlotWidthsModelView modelViewFromGet)
        {
            Assert.Equal(modelViewFromPost.maxWidth, modelViewFromGet.maxWidth);
            Assert.Equal(modelViewFromPost.minWidth, modelViewFromGet.minWidth);
            Assert.Equal(modelViewFromPost.recommendedWidth, modelViewFromGet.recommendedWidth);
        }

        private void assertDimensionModelView(AddDimensionModelView sentModelView, GetDimensionModelView modelViewFromPost)
        {
            if (sentModelView.GetType().Equals(typeof(AddSingleValueDimensionModelView)))
            {
                var sentSingleDimensionModelView = (AddSingleValueDimensionModelView)sentModelView;
                sentSingleDimensionModelView.value =
                    convertUnit(sentSingleDimensionModelView.value,
                                sentSingleDimensionModelView.unit);
                var singleDimensionModelViewFromPost = (GetSingleValueDimensionModelView)modelViewFromPost;
                Assert.Equal(sentSingleDimensionModelView.value, singleDimensionModelViewFromPost.value);
            }
            else if (sentModelView.GetType().Equals(typeof(AddContinuousDimensionIntervalModelView)))
            {
                var sentContinuousDimensionIntervalModelView = (AddContinuousDimensionIntervalModelView)sentModelView;
                sentContinuousDimensionIntervalModelView.increment =
                    convertUnit(sentContinuousDimensionIntervalModelView.increment,
                                sentContinuousDimensionIntervalModelView.unit);
                sentContinuousDimensionIntervalModelView.minValue =
                    convertUnit(sentContinuousDimensionIntervalModelView.minValue,
                                sentContinuousDimensionIntervalModelView.unit);
                sentContinuousDimensionIntervalModelView.maxValue =
                    convertUnit(sentContinuousDimensionIntervalModelView.maxValue,
                                sentContinuousDimensionIntervalModelView.unit);
                var continuousDimensionIntervalModelViewFromPost = (GetContinuousDimensionIntervalModelView)modelViewFromPost;
                Assert.Equal(sentContinuousDimensionIntervalModelView.minValue, continuousDimensionIntervalModelViewFromPost.minValue, 1);
                Assert.Equal(sentContinuousDimensionIntervalModelView.maxValue, continuousDimensionIntervalModelViewFromPost.maxValue);
                Assert.Equal(sentContinuousDimensionIntervalModelView.increment, continuousDimensionIntervalModelViewFromPost.increment);
            }
            else if (sentModelView.GetType().Equals(typeof(AddDiscreteDimensionIntervalModelView)))
            {
                var sentDiscreteDimensionIntervalModelView = (AddDiscreteDimensionIntervalModelView)sentModelView;
                var discreteDimensionIntervalModelViewFromPost = (GetDiscreteDimensionIntervalModelView)modelViewFromPost;
                for (int i = 0; i < sentDiscreteDimensionIntervalModelView.values.Count; i++)
                {
                    Assert.Equal(sentDiscreteDimensionIntervalModelView.values[i], discreteDimensionIntervalModelViewFromPost.values[i]);
                }
            }
        }

        private void assertDimensionModelView(GetDimensionModelView modelViewFromPost, GetDimensionModelView modelViewFromGet)
        {
            if (modelViewFromPost.GetType().Equals(typeof(GetSingleValueDimensionModelView)))
            {
                var singleDimensionModelViewFromPost = (GetSingleValueDimensionModelView)modelViewFromPost;
                var singleDimensionModelViewFromGet = (GetSingleValueDimensionModelView)modelViewFromGet;
                Assert.Equal(singleDimensionModelViewFromPost.value, singleDimensionModelViewFromGet.value);
                Assert.Equal(singleDimensionModelViewFromPost.id, singleDimensionModelViewFromGet.id);
            }
            else if (modelViewFromPost.GetType().Equals(typeof(GetContinuousDimensionIntervalModelView)))
            {
                var continuousDimensionIntervalModelViewFromPost = (GetContinuousDimensionIntervalModelView)modelViewFromPost;
                var continuousDimensionIntervalModelViewFromGet = (GetContinuousDimensionIntervalModelView)modelViewFromGet;
                Assert.Equal(continuousDimensionIntervalModelViewFromPost.increment, continuousDimensionIntervalModelViewFromGet.increment);
                Assert.Equal(continuousDimensionIntervalModelViewFromPost.minValue, continuousDimensionIntervalModelViewFromGet.minValue);
                Assert.Equal(continuousDimensionIntervalModelViewFromPost.maxValue, continuousDimensionIntervalModelViewFromGet.maxValue);
            }
            else if (modelViewFromPost.GetType().Equals(typeof(GetDiscreteDimensionIntervalModelView)))
            {
                var discreteDimensionIntervalModelViewFromPost = (GetDiscreteDimensionIntervalModelView)modelViewFromPost;
                var discreteDimensionIntervalModelViewFromGet = (GetDiscreteDimensionIntervalModelView)modelViewFromGet;
                for (int i = 0; i < discreteDimensionIntervalModelViewFromPost.values.Count; i++)
                {
                    Assert.Equal(discreteDimensionIntervalModelViewFromPost.values[i], discreteDimensionIntervalModelViewFromGet.values[i]);
                }
            }
        }

        /* Auxiliary methods */
        private AddProductModelView createProductWithComponentsAndWithSlots(GetProductCategoryModelView categoryModelView, AddProductMaterialModelView materialModelView, string testNumber, long firstComponentId, long secondComponentId)
        {
            AddProductModelView productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelView, materialModelView, testNumber);
            productModelView.slotWidths = new AddProductSlotWidthsModelView();
            productModelView.slotWidths.maxWidth = 40;
            productModelView.slotWidths.minWidth = 20;
            productModelView.slotWidths.recommendedWidth = 30;
            productModelView.slotWidths.unit = "cm";
            AddComponentModelView componentModelView = new AddComponentModelView();
            AddComponentModelView otherComponentModelView = new AddComponentModelView();
            componentModelView.mandatory = true;
            componentModelView.childProductId = firstComponentId;
            otherComponentModelView.mandatory = false;
            otherComponentModelView.childProductId = secondComponentId;
            productModelView.components = new List<AddComponentModelView>() { componentModelView, otherComponentModelView };
            return productModelView;
        }



        private AddProductModelView createProductWithoutComponentsAndWithoutSlots(GetProductCategoryModelView categoryModelView, AddProductMaterialModelView materialModelView, string testNumber)
        {
            AddProductModelView productModelView = new AddProductModelView();
            productModelView.categoryId = categoryModelView.id;
            productModelView.materials = new List<AddProductMaterialModelView>();
            productModelView.materials.Add(materialModelView);
            productModelView.modelFilename = "ModelTest" + testNumber + ".glb";
            productModelView.reference = "ReferenceTest" + testNumber;
            productModelView.designation = "DesignationTest" + testNumber;
            productModelView.measurements = new List<AddMeasurementModelView>();
            productModelView.measurements.Add(createMeasurementModelView());

            return productModelView;
        }

        private AddProductModelView createProductWithSlots(GetProductCategoryModelView categoryModelView, AddProductMaterialModelView materialModelView, string testNumber)
        {
            AddProductModelView productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelView, materialModelView, testNumber);

            productModelView.slotWidths = new AddProductSlotWidthsModelView();
            productModelView.slotWidths.maxWidth = 40;
            productModelView.slotWidths.minWidth = 20;
            productModelView.slotWidths.recommendedWidth = 30;
            productModelView.slotWidths.unit = "cm";

            return productModelView;
        }

        private AddProductModelView createProductWithComponents(GetProductCategoryModelView categoryModelView, AddProductMaterialModelView materialModelView, string testNumber, long firstComponentId, long secondComponentId)
        {
            AddProductModelView productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelView, materialModelView, testNumber);

            AddComponentModelView componentModelView = new AddComponentModelView();
            AddComponentModelView otherComponentModelView = new AddComponentModelView();
            componentModelView.mandatory = true;
            componentModelView.childProductId = firstComponentId;
            otherComponentModelView.mandatory = false;
            otherComponentModelView.childProductId = secondComponentId;

            productModelView.components = new List<AddComponentModelView>() { componentModelView, otherComponentModelView };

            return productModelView;
        }

        private AddProductCategoryModelView createCategoryModelView(string testNumber)
        {
            AddProductCategoryModelView modelView = new AddProductCategoryModelView();
            modelView.name = "TestCategory" + testNumber;
            return modelView;
        }

        //!Replace this with createMaterialModelView after refactor is made
        private MaterialDTO createMaterialDTO(string testNumber)
        {
            MaterialDTO materialDTO = new MaterialDTO();
            materialDTO.designation = "Material Designation Test" + testNumber;
            materialDTO.reference = "Material Reference Test" + testNumber;
            materialDTO.image = "Material Image Test" + testNumber + ".jpeg";
            ColorDTO colorDTO = new ColorDTO();
            ColorDTO otherColorDTO = new ColorDTO();
            FinishDTO finishDTO = new FinishDTO();
            FinishDTO otherFinishDTO = new FinishDTO();
            colorDTO.red = 100;
            colorDTO.blue = 100;
            colorDTO.green = 100;
            colorDTO.alpha = 10;
            colorDTO.name = "Color Test" + testNumber;
            otherColorDTO.red = 150;
            otherColorDTO.blue = 150;
            otherColorDTO.green = 150;
            otherColorDTO.alpha = 100;
            otherColorDTO.name = "Other Color Test" + testNumber;
            finishDTO.description = "Finish Test" + testNumber;
            otherFinishDTO.description = "Other Finish Test" + testNumber;
            materialDTO.colors = new List<ColorDTO>() { colorDTO, otherColorDTO };
            materialDTO.finishes = new List<FinishDTO>() { finishDTO, otherFinishDTO };
            return materialDTO;
        }

        private AddMeasurementModelView createMeasurementModelView()
        {
            AddMeasurementModelView measurementModelView = new AddMeasurementModelView();
            AddSingleValueDimensionModelView heightModelView = new AddSingleValueDimensionModelView();
            AddContinuousDimensionIntervalModelView widthModelView = new AddContinuousDimensionIntervalModelView();
            AddDiscreteDimensionIntervalModelView depthModelView = new AddDiscreteDimensionIntervalModelView();
            heightModelView.unit = "dm";
            heightModelView.value = 10;
            widthModelView.unit = "cm";
            widthModelView.minValue = 50;
            widthModelView.maxValue = 100;
            widthModelView.increment = 5;
            depthModelView.unit = "mm";
            depthModelView.values = new List<double>() { 100, 200, 300, 400, 500 };
            measurementModelView.depthDimension = depthModelView;
            measurementModelView.heightDimension = heightModelView;
            measurementModelView.widthDimension = widthModelView;

            return measurementModelView;
        }

        private AddMeasurementModelView createNewMeasurementModelView()
        {
            AddMeasurementModelView newMeasurementModelView = new AddMeasurementModelView();
            AddSingleValueDimensionModelView newDepth = new AddSingleValueDimensionModelView();
            newDepth.unit = "cm";
            newDepth.value = 10;
            AddContinuousDimensionIntervalModelView newWidth = new AddContinuousDimensionIntervalModelView();
            newWidth.unit = "mm";
            newWidth.increment = 50;
            newWidth.minValue = 1000;
            newWidth.maxValue = 2000;
            AddDiscreteDimensionIntervalModelView newHeight = new AddDiscreteDimensionIntervalModelView();
            newHeight.unit = "cm";
            newHeight.values = new List<double>() { 60, 70, 80, 90, 101, 55, 52 };
            newMeasurementModelView.depthDimension = newDepth;
            newMeasurementModelView.heightDimension = newHeight;
            newMeasurementModelView.widthDimension = newWidth;
            return newMeasurementModelView;
        }

        private AddMeasurementModelView createNewMeasurementModelViewWithInvalidHeight()
        {
            AddMeasurementModelView newMeasurementModelView = new AddMeasurementModelView();
            AddSingleValueDimensionModelView newDepth = new AddSingleValueDimensionModelView();
            newDepth.unit = "cm";
            newDepth.value = 10;
            AddContinuousDimensionIntervalModelView newWidth = new AddContinuousDimensionIntervalModelView();
            newWidth.unit = "mm";
            newWidth.increment = 50;
            newWidth.minValue = 1000;
            newWidth.maxValue = 2000;
            AddDiscreteDimensionIntervalModelView newHeight = new AddDiscreteDimensionIntervalModelView();
            newHeight.unit = "cm";
            newHeight.values = new List<double>() { -60, 70, 80, 90, 101, 55, 52 };
            newMeasurementModelView.depthDimension = newDepth;
            newMeasurementModelView.heightDimension = newHeight;
            newMeasurementModelView.widthDimension = newWidth;
            return newMeasurementModelView;
        }

        private AddMeasurementModelView createNewMeasurementModelViewWithInvalidWidth()
        {
            AddMeasurementModelView newMeasurementModelView = new AddMeasurementModelView();
            AddSingleValueDimensionModelView newDepth = new AddSingleValueDimensionModelView();
            newDepth.unit = "cm";
            newDepth.value = 10;
            AddContinuousDimensionIntervalModelView newWidth = new AddContinuousDimensionIntervalModelView();
            newWidth.unit = "mm";
            newWidth.increment = 50;
            newWidth.minValue = Double.PositiveInfinity;
            newWidth.maxValue = 2000;
            AddDiscreteDimensionIntervalModelView newHeight = new AddDiscreteDimensionIntervalModelView();
            newHeight.unit = "cm";
            newHeight.values = new List<double>() { 60, 70, 80, 90, 101, 55, 52 };
            newMeasurementModelView.depthDimension = newDepth;
            newMeasurementModelView.heightDimension = newHeight;
            newMeasurementModelView.widthDimension = newWidth;
            return newMeasurementModelView;
        }

        private AddMeasurementModelView createNewMeasurementModelViewWithInvalidDepth()
        {
            AddMeasurementModelView newMeasurementModelView = new AddMeasurementModelView();
            AddSingleValueDimensionModelView newDepth = new AddSingleValueDimensionModelView();
            newDepth.unit = "cm";
            newDepth.value = 0;
            AddContinuousDimensionIntervalModelView newWidth = new AddContinuousDimensionIntervalModelView();
            newWidth.unit = "mm";
            newWidth.increment = 50;
            newWidth.minValue = 1000;
            newWidth.maxValue = 2000;
            AddDiscreteDimensionIntervalModelView newHeight = new AddDiscreteDimensionIntervalModelView();
            newHeight.unit = "cm";
            newHeight.values = new List<double>() { 60, 70, 80, 90, 101, 55, 52 };
            newMeasurementModelView.depthDimension = newDepth;
            newMeasurementModelView.heightDimension = newHeight;
            newMeasurementModelView.widthDimension = newWidth;
            return newMeasurementModelView;
        }

        private static double convertUnit(double value, string unit)
        {
            return MeasurementUnitService.convertToUnit(value, unit);
        }
    }
}