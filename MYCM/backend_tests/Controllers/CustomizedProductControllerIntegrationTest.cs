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
using core.modelview.productcategory;
using core.modelview.productmaterial;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;
using core.modelview.measurement;
using core.modelview.dimension;
using core.modelview.productslotwidths;
using core.modelview.component;
using core.modelview.slot;
using static core.domain.CustomizedProduct;
using core.modelview.material;
using System.Text;

namespace backend_tests.Controllers
{
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public sealed class CustomizedProductControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        private const string BASE_URI = "mycm/api/customizedproducts";

        private const string PRODUCTS_URI = "mycm/api/products";

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

        [Fact, TestPriority(-15)]
        public async void ensureGetAllBaseCustomizedProductsReturnsOkWhenCollectionHasBaseCustomizedProducts()
        {
            string testNumber = "-15";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView addComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var postComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addComponent
                );

            Assert.Equal(HttpStatusCode.Created, postComponent.StatusCode);

            var getAllBases =
                await httpClient.GetAsync(BASE_URI + "/base");

            Assert.Equal(HttpStatusCode.OK, getAllBases.StatusCode);

            GetAllCustomizedProductsModelView getAllBasesModelView =
                await getAllBases.Content.ReadAsAsync<GetAllCustomizedProductsModelView>();

            Assert.NotEmpty(getAllBasesModelView);
            Assert.Equal(1, getAllBasesModelView.Count);
            Assert.Equal(getAllBasesModelView[0].customizedProductId,
                        createdCustomizedProductModelView.customizedProductId);
            Assert.Equal(getAllBasesModelView[0].designation,
                        createdCustomizedProductModelView.designation);
            Assert.Equal(getAllBasesModelView[0].productId,
                        createdCustomizedProductModelView.product.productId);
            Assert.Equal(getAllBasesModelView[0].serialNumber,
                        createdCustomizedProductModelView.serialNumber);

            await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );
        }

        [Fact, TestPriority(-14)]
        public async void ensureGetAllReturnsOkWhenCollectionHasResources()
        {
            string testNumber = "-14";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + " 2", false);

            GetCustomizedProductModelView otherCreatedCustomizedProductModelView =
               await saveCustomizedProduct(otherCustomizedProductModelView, false);

            var response =
                await httpClient.GetAsync(BASE_URI);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            GetAllCustomizedProductsModelView getAll =
                await response.Content.ReadAsAsync<GetAllCustomizedProductsModelView>();

            Assert.NotEmpty(getAll);
            Assert.Equal(getAll[0].customizedProductId,
                        createdCustomizedProductModelView.customizedProductId);
            Assert.Equal(getAll[0].designation,
                        createdCustomizedProductModelView.designation);
            Assert.Equal(getAll[0].productId,
                        createdCustomizedProductModelView.product.productId);
            Assert.Equal(getAll[0].serialNumber,
                        createdCustomizedProductModelView.serialNumber);
            Assert.Equal(getAll[1].customizedProductId,
                        otherCreatedCustomizedProductModelView.customizedProductId);
            Assert.Equal(getAll[1].designation,
                        otherCreatedCustomizedProductModelView.designation);
            Assert.Equal(getAll[1].productId,
                        otherCreatedCustomizedProductModelView.product.productId);
            Assert.Equal(getAll[1].serialNumber,
                        otherCreatedCustomizedProductModelView.serialNumber);

            await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );

            await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + otherCreatedCustomizedProductModelView.customizedProductId
                );
        }

        [Fact, TestPriority(-13)]
        public async void ensureDeletingCustomizedProductReturnsNotFoundIfCollectionIsEmpty()
        {
            var response =
                await httpClient.DeleteAsync(BASE_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(-12)]
        public async void ensureUpdatingCustomizedProductReturnsNotFoundIfCollectionIsEmpty()
        {
            string testNumber = "-12";

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.reference = "PRESSURE BUILDING " + testNumber;

            var response =
                await httpClient.PutAsJsonAsync(BASE_URI + "/1", update);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message 
        }

        [Fact, TestPriority(-11)]
        public async void ensureAddingSlotToCustomizedProductReturnsNotFoundIfCollectionIsEmpty()
        {
            string testNumber = "-11";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            //!Should the code returned here be NotFound?
            Assert.Equal(HttpStatusCode.NotFound, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(-10)]
        public async void ensureGetAllReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-9)]
        public async void ensureGetAllBaseCustomizedProductsReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/base");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-8)]
        public async void ensureGetByIdReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-7)]
        public async void ensureGetSlotReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/1/slots/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-6)]
        public async void ensureDeleteCustomizedProductReturnsNotFoundIfCollectionIsEmpty()
        {
            var response = await httpClient.DeleteAsync(BASE_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-5)]
        public async void ensureGetByIdReturnsNotFoundForNonExistingId()
        {
            string testNumber = "-5";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, false);

            var getResponse = await httpClient.GetAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId + 1
            );

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare message

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(-4)]
        public async void ensureGetSlotByIdReturnsNotFoundForNonExistingCustomizedProductId()
        {
            string testNumber = "-4";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, true);

            var getResponse = await httpClient.GetAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId + 1
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
            );

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare Message

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(-3)]
        public async void ensureGetSlotByIdReturnsNotFoundIfCustomizedProductDoesntSupportSlots()
        {
            string testNumber = "-3";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, false);

            //!Slot ID always needs to have 1 added because a customized product always has
            //!one slot (it's the product itself) What we want to test here is an actual other slot
            var getResponse = await httpClient.GetAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId + 1
            );

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare Message

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(-2)]
        public async void ensureGetSlotByIdReturnsNotFoundIfSlotIdDoesntExist()
        {
            string testNumber = "-2";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, true);

            //!Slot ID always needs to have 1 added because a customized product always has
            //!one slot (it's the product itself) What we want to test here is an actual other slot
            var getResponse = await httpClient.GetAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[1].slotId + 1
            );

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare Message

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(-1)]
        public async void ensureGetSlotByIdReturnsNotFoundIfSlotBelongsToAnotherCustomizedProduct()
        {
            string testNumber = "-1";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);
            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + " 2", true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await saveCustomizedProduct(customizedProductModelView, true);
            GetCustomizedProductModelView otherCreatedCustomizedProductModelView =
               await saveCustomizedProduct(otherCustomizedProductModelView, true);

            var getResponse = await httpClient.GetAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + otherCreatedCustomizedProductModelView.slots[1].slotId
            );

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare Message

            await httpClient.DeleteAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);
            await httpClient.DeleteAsync(BASE_URI + "/" + otherCreatedCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(0)]
        public async void ensureAddingCustomizedProductWithNullBodyReturnsBadRequest()
        {
            AddCustomizedProductModelView customizedProductModelView = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(1)]
        public async void ensureAddingCustomizedProductWithNonExistingProductIdReturnsBadRequest()
        {
            string testNumber = "1";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            customizedProductModelView.productId = customizedProductModelView.productId + 1;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(2)]
        public async void ensureAddingCustomizedProductWithCustomizedDimensionsOnlyReturnsCreated()
        {
            string testNumber = "2";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial = null;
            customizedProductModelView.designation = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(3)]
        public async void ensureAddingCustomizedProductWithNonExistingMaterialIdReturnsBadRequest()
        {
            string testNumber = "3";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.materialId =
                customizedProductModelView.customizedMaterial.materialId + 1;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(4)]
        public async void ensureAddingCustomizedProductWithNullCustomizedDimensionsReturnsBadRequest()
        {
            string testNumber = "4";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedDimensions = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(5)]
        public async void ensureAddingCustomizedProductWithMaterialReferenceFromAnotherProductReturnsBadRequest()
        {
            string testNumber = "5";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);
            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + " 2", true);

            customizedProductModelView.customizedMaterial.materialId =
                otherCustomizedProductModelView.customizedMaterial.materialId;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(6)]
        public async void ensureAddingCustomizedProductWithNullColorAndNullFinishReturnsBadRequest()
        {
            string testNumber = "6";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.color = null;
            customizedProductModelView.customizedMaterial.finish = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(7)]
        public async void ensureAddingCustomizedProductWithColorFromAnotherMaterialReturnsBadRequest()
        {
            string testNumber = "7";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);
            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + " 2", true);

            customizedProductModelView.customizedMaterial.color =
                otherCustomizedProductModelView.customizedMaterial.color;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(8)]
        public async void ensureAddingCustomizedProductWithFinishFromAnotherMaterialReturnsBadRequest()
        {
            string testNumber = "8";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);
            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + " 2", true);

            customizedProductModelView.customizedMaterial.finish =
                otherCustomizedProductModelView.customizedMaterial.finish;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(9)]
        public async void ensureAddingCustomizedProductWithInvalidColorReturnsBadRequest()
        {
            string testNumber = "9";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.color.name = "im an invalid color";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(10)]
        public async void ensureAddingCustomizedProductWithInvalidFinishReturnsBadRequest()
        {
            string testNumber = "10";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.finish.description = "im an invalid finish";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(11)]
        public async void ensureAddingCustomizedProductWithInvalidHeightReturnsBadRequest()
        {
            string testNumber = "11";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedDimensions.height = Double.MinValue;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(12)]
        public async void ensureAddingCustomizedProductWithInvalidWidthReturnsBadRequest()
        {
            string testNumber = "12";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedDimensions.width = Double.MinValue;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(13)]
        public async void ensureAddingCustomizedProductWithInvalidDepthReturnsBadRequest()
        {
            string testNumber = "13";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedDimensions.depth = Double.MinValue;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(14)]
        public async void ensureAddingCustomizedProductWithInvalidDimensionsUnitReturnsBadRequest()
        {
            string testNumber = "14";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedDimensions.unit = "im an invalid measurement unit";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(15)]
        public async void ensureAddingCustomizedProductWithInvalidDesignationReturnsBadRequest()
        {
            string testNumber = "15";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.designation = "";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(16)]
        public async void ensureAddingCustomizedProductWithValidReferenceButNoTokenReturnsBadRequest()
        {
            string testNumber = "16";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "Reference " + testNumber;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(17)]
        public async void ensureAddingCustomizedProductWithInvalidReferenceAndValidTokenReturnsBadRequest()
        {
            string testNumber = "17";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "";
            //!Change this to have the token be in the request header after MVC Controller is updated with this too
            customizedProductModelView.userAuthToken = "Valid Token";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(18)]
        public async void ensureAddingCustomizedProductWithValidReferenceAndInvalidTokenReturnsBadRequest()
        {
            string testNumber = "18";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "Reference " + testNumber;
            //!Change this to have the token be in the request header after MVC Controller is updated with this too
            customizedProductModelView.userAuthToken = "";

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(19)]
        public async void ensureAddingCustomizedProductWithColorOnlyReturnsCreated()
        {
            string testNumber = "19";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.finish = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(20)]
        public async void ensureAddingCustomizedProductWithFinishOnlyReturnsCreated()
        {
            string testNumber = "20";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial.color = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(21)]
        public async void ensureAddingCustomizedProductThatCanBeFinishedReturnsCreated()
        {
            string testNumber = "21";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(22)]
        public async void ensureAddingCustomizedProductWithReferenceAndAuthTokenReturnsCreated()
        {
            string testNumber = "22";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "Reference " + testNumber;

            Uri baseUri = new Uri(httpClient.BaseAddress.ToString() + BASE_URI);

            var request = new HttpRequestMessage()
            {
                RequestUri = baseUri,
                Method = HttpMethod.Post
            };

            string json = JsonConvert.SerializeObject(customizedProductModelView);

            request.Headers.Add("userAuthToken", "Valid auth token " + testNumber);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(23)]
        public async void ensureAddingCustomizedProductDuplicateReturnsBadRequest()
        {
            string testNumber = "23";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "Reference " + testNumber;

            Uri baseUri = new Uri(httpClient.BaseAddress.ToString() + BASE_URI);

            var request = new HttpRequestMessage()
            {
                RequestUri = baseUri,
                Method = HttpMethod.Post
            };

            string json = JsonConvert.SerializeObject(customizedProductModelView);

            request.Headers.Add("userAuthToken", "Valid auth token " + testNumber);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);

            var duplicateResponse = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(24)]
        public async void ensureAddingCustomizedProductWithCustomizedDimensionsAndMaterialOnlyReturnsCreated()
        {
            string testNumber = "24";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.designation = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(25)]
        public async void ensureAddingCustomizedProductWithCustomizedDimensionsAndDesignationOnlyReturnsCreated()
        {
            string testNumber = "25";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.customizedMaterial = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI, customizedProductModelView);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView postContent =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductModelView, postContent);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + postContent.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(postContent, getAfterPostContent);
        }

        [Fact, TestPriority(26)]
        public async void ensureGetByIdReturnsOkForExistingCustomizedProductId()
        {
            string testNumber = "26";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            var getResponse = await httpClient.GetAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            GetCustomizedProductModelView contentFromGet =
                await getResponse.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(createdCustomizedProductModelView, contentFromGet);
        }

        [Fact, TestPriority(27)]
        public async void ensureGetSlotByIdReturnsOkForCustomizedProductsSelfSlot()
        {
            string testNumber = "27";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            var getSlot = await httpClient.GetAsync(
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
            );

            Assert.Equal(HttpStatusCode.OK, getSlot.StatusCode);

            GetSlotModelView getSlotModelView =
                await getSlot.Content.ReadAsAsync<GetSlotModelView>();

            Assert.Equal(
                createdCustomizedProductModelView.slots[0].slotId,
                getSlotModelView.slotId
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[0].slotDimensions.depth,
                getSlotModelView.slotDimensions.depth
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[0].slotDimensions.width,
                getSlotModelView.slotDimensions.width
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[0].slotDimensions.height,
                getSlotModelView.slotDimensions.height
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[0].slotDimensions.unit,
                getSlotModelView.slotDimensions.unit
            );
        }

        [Fact, TestPriority(28)]
        public async void ensureGetSlotByIdReturnsOkForCustomizedProductsActualSlots()
        {
            string testNumber = "28";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            var getSlot = await httpClient.GetAsync(
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[1].slotId
            );

            Assert.Equal(HttpStatusCode.OK, getSlot.StatusCode);

            GetSlotModelView getSlotModelView =
                await getSlot.Content.ReadAsAsync<GetSlotModelView>();

            Assert.Equal(
                createdCustomizedProductModelView.slots[1].slotId,
                getSlotModelView.slotId
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[1].slotDimensions.depth,
                getSlotModelView.slotDimensions.depth
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[1].slotDimensions.width,
                getSlotModelView.slotDimensions.width
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[1].slotDimensions.height,
                getSlotModelView.slotDimensions.height
            );
            Assert.Equal(
                createdCustomizedProductModelView.slots[1].slotDimensions.unit,
                getSlotModelView.slotDimensions.unit
            );
        }

        [Fact, TestPriority(29)]
        public async void ensureAddingSlotToCustomizedProductWithNullBodyReturnsBadRequest()
        {
            string testNumber = "29";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions = null;

            var response = await httpClient.PostAsJsonAsync(BASE_URI + "/" +
                createdCustomizedProductModelView.customizedProductId + "/slots", slotDimensions);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(30)]
        public async void ensureAddingSlotToCustomizedProductReturnsNotFoundForNonExistingId()
        {
            string testNumber = "30";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId + 1
                + "/slots", slotDimensions
            );

            //!Should the code returned here be Not Found?
            Assert.Equal(HttpStatusCode.NotFound, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(31)]
        public async void ensureAddingSlotToCustomizedProductThatDoesntSupportSlotsReturnsBadRequest()
        {
            string testNumber = "31";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(32)]
        public async void ensureAddingSlotToCustomizedProductWithInvalidHeightReturnsBadRequest()
        {
            string testNumber = "32";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = Double.MinValue;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(33)]
        public async void ensureAddingSlotToCustomizedProductWithInvalidWidthReturnsBadRequest()
        {
            string testNumber = "33";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = Double.MinValue;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(34)]
        public async void ensureAddingSlotToCustomizedProductWithInvalidDepthReturnsBadRequest()
        {
            string testNumber = "34";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = Double.MinValue;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(35)]
        public async void ensureAddingSlotToCustomizedProductWithInvalidDimensionsUnitReturnsBadRequest()
        {
            string testNumber = "35";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = "hello im mac and im pc and this is an invalid unit";

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(36)]
        public async void ensureAddingSlotToFinishedCustomizedProductReturnsBadRequest()
        {
            string testNumber = "36";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedProductModelView addMandatoryComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var addComponentToSlot =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addMandatoryComponent
                );

            Assert.Equal(HttpStatusCode.Created, addComponentToSlot.StatusCode);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                     update
                );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.BadRequest, addSlotToCustomizedProduct.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(37)]
        public async void ensureAddingValidSlotToCustomizedProductReturnsCreated()
        {
            string testNumber = "37";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

            slotDimensions.depth = createdCustomizedProductModelView.customizedDimensions.depth / 2;
            slotDimensions.height = createdCustomizedProductModelView.customizedDimensions.height / 2;
            slotDimensions.width = createdCustomizedProductModelView.customizedDimensions.width / 2;
            slotDimensions.unit = createdCustomizedProductModelView.customizedDimensions.unit;

            var addSlotToCustomizedProduct = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots", slotDimensions
            );

            Assert.Equal(HttpStatusCode.Created, addSlotToCustomizedProduct.StatusCode);

            GetCustomizedProductModelView postContent =
                await addSlotToCustomizedProduct.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.NotEqual(createdCustomizedProductModelView.slots, postContent.slots);
            //TODO Should we compare the dimension values since the slots will be resized when adding a new slot?
            /* Assert.Equal(slotDimensions.depth, postContent.slots[0].slotDimensions.depth);
            Assert.Equal(slotDimensions.height, postContent.slots[0].slotDimensions.height);
            Assert.Equal(slotDimensions.width, postContent.slots[0].slotDimensions.width); */
            Assert.Equal(slotDimensions.unit, postContent.slots[0].slotDimensions.unit);
        }

        [Fact, TestPriority(38)]
        public async void ensureAddingCustomizedProductToSlotReturnsBadRequestWithNullBody()
        {
            AddCustomizedProductModelView customizedProductModelView = null;

            var response = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/1/slots/1/customizedproducts",
                customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(39)]
        public async void ensureAddingCustomizedProductToSlotWithNonExistingProductIdReturnsBadRequest()
        {
            string testNumber = "39";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView component =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            component.productId += 3;//!+3 Because the base customized product has 2 components

            var response = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                + "/customizedproducts",
                component
            );

            //!Should the code here be BadRequest?
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(40)]
        public async void ensureAddingCustomizedProductToSlotReturnsBadRequestForNonExistingSlotId()
        {
            string testNumber = "40";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView component =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var response = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId + 2
                + "/customizedproducts",
                component
            );

            //!Should the code here be BadRequest?
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(41)]
        public async void ensureAddingCustomizedProductToFinishedCustomizedProductReturnsBadRequest()
        {
            string testNumber = "41";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView component =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var response = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                + "/customizedproducts",
                component
            );

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                     update
                );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            var addSecondComponent = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[1].slotId
                + "/customizedproducts",
                component
            );
            //!Check if bad request is really due to the status of the product being finished
            Assert.Equal(HttpStatusCode.BadRequest, addSecondComponent.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(42)]
        public async void ensureAddingValidCustomizedProductToSlotReturnsCreated()
        {
            string testNumber = "42";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView component =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var response = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId
                + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                + "/customizedproducts",
                component
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView createdSubCustomizedProduct =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(component, createdSubCustomizedProduct);

            var getAfterPost =
                await httpClient.GetAsync(BASE_URI + "/" + createdSubCustomizedProduct.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductModelView getAfterPostContent =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(createdSubCustomizedProduct, getAfterPostContent);

            var getFatherAfterPostOfChild =
                await httpClient.GetAsync(BASE_URI + "/" + createdCustomizedProductModelView.customizedProductId);

            GetCustomizedProductModelView updatedFather =
                await getFatherAfterPostOfChild.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.NotEmpty(updatedFather.slots[0].customizedProducts);
            Assert.Equal(getAfterPostContent.designation,
                        updatedFather.slots[0].customizedProducts[0].designation);
            Assert.Equal(getAfterPostContent.serialNumber,
                        updatedFather.slots[0].customizedProducts[0].serialNumber);
            Assert.Equal(getAfterPostContent.customizedProductId,
                        updatedFather.slots[0].customizedProducts[0].customizedProductId);
        }

        [Fact, TestPriority(43)]
        public async void ensureUpdatingCustomizedProductWithNullBodyReturnsBadRequest()
        {
            string testNumber = "43";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update = null;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(44)]
        public async void ensureUpdatingCustomizedProductReturnsNotFoundForNonExistingId()
        {
            string testNumber = "44";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.reference = "PROPAGANDA " + testNumber;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId + 1,
                    update
                );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(45)]
        public async void ensureUpdatingCustomizedProductWithNonExistingMaterialReturnsBadRequest()
        {
            string testNumber = "45";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            //TODO Replace with ModelView
            update.customizedMaterial = new AddCustomizedMaterialModelView();
            update.customizedMaterial.materialId = createdCustomizedProductModelView.customizedMaterial.customizedMaterialId + 1;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            //TODO Change DTOs to ModelViews ASAP
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(46)]
        public async void ensureUpdatingCustomizedProductWithMaterialThatBelongsToAnotherProductReturnsBadRequest()
        {
            string testNumber = "46";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView otherCustomizedProductModelView =
                await createFinishedCustomizedProduct(testNumber + "2", true);

            GetCustomizedProductModelView otherCreatedCustomizedProductModelView =
                await saveCustomizedProduct(otherCustomizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            //TODO Replace with ModelView
            update.customizedMaterial = new AddCustomizedMaterialModelView();
            update.customizedMaterial.materialId =
                otherCreatedCustomizedProductModelView.customizedMaterial.customizedMaterialId;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            //TODO Change DTOs to ModelViews ASAP
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(47)]
        public async void ensureUpdatingCustomizedProductWithInvalidColorReturnsBadRequest()
        {
            string testNumber = "47";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            //TODO Replace with ModelView
            update.customizedMaterial = new AddCustomizedMaterialModelView();
            update.customizedMaterial.color = new ColorDTO();
            update.customizedMaterial.color.name = "BREAK IT TO ME";
            update.customizedMaterial.color.red = 200;
            update.customizedMaterial.color.green = 200;
            update.customizedMaterial.color.blue = 200;
            update.customizedMaterial.color.alpha = 1;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            //TODO Change DTOs to ModelViews ASAP
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(48)]
        public async void ensureUpdatingCustomizedProductWithInvalidFinishReturnsBadRequest()
        {
            string testNumber = "48";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            //TODO Replace with ModelView
            update.customizedMaterial = new AddCustomizedMaterialModelView();
            update.customizedMaterial.finish = new FinishDTO();
            update.customizedMaterial.finish.description = "GET UP AND FIGHT";
            update.customizedMaterial.finish.shininess = 20;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            //!This is sending status code 500 due to the use of DTOs for Materials
            //TODO Change DTOs to ModelViews ASAP
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(49)]
        public async void ensureUpdatingCustomizedProductWithInvalidDesignationReturnsBadRequest()
        {
            string testNumber = "49";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.designation = "";

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(50)]
        public async void ensureUpdatingCustomizedProductWithInvalidReferenceReturnsBadRequest()
        {
            string testNumber = "50";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.reference = "";

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(51)]
        public async void ensureUpdatingCustomizedProductWithInvalidStatusReturnsBadRequest()
        {
            string testNumber = "51";

            AddCustomizedProductModelView customizedProductModelView =
                await createUnfinishedCustomizedProduct(testNumber, false);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizationStatus = CustomizationStatus.FINISHED;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(52)]
        public async void ensureReversingFinishedStatusToPendingReturnsBadRequest()
        {
            string testNumber = "52";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView addMandatoryComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var addComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addMandatoryComponent
                );

            Assert.Equal(HttpStatusCode.Created, addComponent.StatusCode);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizationStatus = CustomizationStatus.FINISHED;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            update.customizationStatus = CustomizationStatus.PENDING;

            var reverseStatus =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, reverseStatus.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(53)]
        public async void ensureUpdatingFinishedCustomizedProductDesignationReturnsBadRequest()
        {
            string testNumber = "53";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView addMandatoryComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var addComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addMandatoryComponent
                );

            Assert.Equal(HttpStatusCode.Created, addComponent.StatusCode);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizationStatus = CustomizationStatus.FINISHED;

            var response =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            UpdateCustomizedProductModelView updateAfterFinishing =
                new UpdateCustomizedProductModelView();

            updateAfterFinishing.designation = "SIMULATION THEORY";

            var putAfterFinishing =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    updateAfterFinishing
                );

            Assert.Equal(HttpStatusCode.BadRequest, putAfterFinishing.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(54)]
        public async void ensureUpdatingCustomizedProductThatIsntFinishedReturnsOk()
        {
            string testNumber = "54";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            UpdateCustomizedProductModelView firstUpdate =
                new UpdateCustomizedProductModelView();

            //!We need to update the dimensions before adding components
            //!All dimensions have to go in the body of the request even if you only want to change one of them.
            //!Should this be changed?
            firstUpdate.customizedDimensions = new AddCustomizedDimensionsModelView();
            firstUpdate.customizedDimensions.height =
                createdCustomizedProductModelView.customizedDimensions.height;
            firstUpdate.customizedDimensions.depth =
                createdCustomizedProductModelView.customizedDimensions.depth - 100;
            firstUpdate.customizedDimensions.width =
                createdCustomizedProductModelView.customizedDimensions.width - 100;
            firstUpdate.customizedDimensions.unit =
                createdCustomizedProductModelView.customizedDimensions.unit;

            var firstUpdateResponse =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    firstUpdate
                );

            Assert.Equal(HttpStatusCode.OK, firstUpdateResponse.StatusCode);

            GetCustomizedProductModelView customizedProductAfterFirstUpdate =
                await firstUpdateResponse.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.NotEqual(createdCustomizedProductModelView.customizedDimensions.depth,
                            customizedProductAfterFirstUpdate.customizedDimensions.depth);
            //!No need to compare the height since it is a single value dimension and wasn't changed
            Assert.NotEqual(createdCustomizedProductModelView.customizedDimensions.width,
                            customizedProductAfterFirstUpdate.customizedDimensions.width);

            AddCustomizedProductModelView addMandatoryComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var addComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addMandatoryComponent
                );

            Assert.Equal(HttpStatusCode.Created, addComponent.StatusCode);

            UpdateCustomizedProductModelView secondUpdate =
                new UpdateCustomizedProductModelView();

            secondUpdate.designation = "SOMETHING HUMAN";
            //!Since the customized product has a serial number, it doesnt have a reference
            //TODO Test for a successful update of the reference
            secondUpdate.customizationStatus = CustomizationStatus.FINISHED;
            //TODO Add material update after DTO is changed to Model View

            var secondUpdateResponse =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    secondUpdate
                );

            Assert.Equal(HttpStatusCode.OK, secondUpdateResponse.StatusCode);

            GetCustomizedProductModelView customizedProductAfterSecondUpdate =
                await secondUpdateResponse.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.NotEqual(customizedProductAfterFirstUpdate.designation,
                            customizedProductAfterSecondUpdate.designation);
            Assert.Equal(secondUpdate.designation, customizedProductAfterSecondUpdate.designation);
            Assert.Equal(customizedProductAfterFirstUpdate.customizedDimensions.depth,
                        customizedProductAfterSecondUpdate.customizedDimensions.depth);
            Assert.Equal(customizedProductAfterFirstUpdate.customizedDimensions.height,
                        customizedProductAfterSecondUpdate.customizedDimensions.height);
            Assert.Equal(customizedProductAfterFirstUpdate.customizedDimensions.width,
                        customizedProductAfterSecondUpdate.customizedDimensions.width);

            var getAfterPut =
                await httpClient.GetAsync(BASE_URI + "/" + customizedProductAfterSecondUpdate.customizedProductId);

            Assert.Equal(HttpStatusCode.OK, getAfterPut.StatusCode);

            GetCustomizedProductModelView getAfterPutContent =
                await getAfterPut.Content.ReadAsAsync<GetCustomizedProductModelView>();

            assertCustomizedProductModelView(customizedProductAfterSecondUpdate, getAfterPutContent);
        }

        [Fact, TestPriority(55)]
        public async void ensureDeletingCustomizedProductReturnsNotFoundForNonExistingId()
        {
            string testNumber = "55";

            AddCustomizedProductModelView customizedProductModelView =
               await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            var response =
                await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId + 1
                );

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(56)]
        public async void ensureDeletingExistingCustomizedProductReturnsNoContent()
        {
            string testNumber = "56";

            AddCustomizedProductModelView customizedProductModelView =
               await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, false);

            var response =
                await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getAfterDelete =
                await httpClient.GetAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );

            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(57)]
        public async void ensureDeletingChildCustomizedProductDoesntDeleteTheFather()
        {
            string testNumber = "57";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView addComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var postComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addComponent
                );

            Assert.Equal(HttpStatusCode.Created, postComponent.StatusCode);

            GetCustomizedProductModelView component =
                await postComponent.Content.ReadAsAsync<GetCustomizedProductModelView>();

            var deleteChild =
                await httpClient.DeleteAsync
                (
                    BASE_URI + "/"
                    + component.customizedProductId
                );

            Assert.Equal(HttpStatusCode.NoContent, deleteChild.StatusCode);

            var getParentAfterChildRemoval =
                await httpClient.GetAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );

            Assert.Equal(HttpStatusCode.OK, getParentAfterChildRemoval.StatusCode);

            GetCustomizedProductModelView fatherAfterChildRemoval =
                await getParentAfterChildRemoval.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.Null(fatherAfterChildRemoval.slots[0].customizedProducts);

            var attemptToGetRemovedChild =
                await httpClient.GetAsync
                (
                    BASE_URI + "/"
                    + component.customizedProductId
                );

            Assert.Equal(HttpStatusCode.NotFound, attemptToGetRemovedChild.StatusCode);
        }

        [Fact, TestPriority(58)]
        public async void ensureUpdatingDimensionsOfAFinishedCustomizedProductReturnsBadRequest()
        {
            string testNumber = "58";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await saveCustomizedProduct(customizedProductModelView, true);

            AddCustomizedProductModelView addMandatoryComponent =
                await createMandatoryComponent(createdCustomizedProductModelView, testNumber);

            var addComponent =
                await httpClient.PostAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                    + "/slots/" + createdCustomizedProductModelView.slots[0].slotId
                    + "/customizedproducts",
                    addMandatoryComponent
                );

            Assert.Equal(HttpStatusCode.Created, addComponent.StatusCode);

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.customizedDimensions = new AddCustomizedDimensionsModelView();
            update.customizedDimensions.height =
                createdCustomizedProductModelView.customizedDimensions.height;
            update.customizedDimensions.depth =
                createdCustomizedProductModelView.customizedDimensions.depth - 100;
            update.customizedDimensions.width =
                createdCustomizedProductModelView.customizedDimensions.width - 100;
            update.customizedDimensions.unit =
                createdCustomizedProductModelView.customizedDimensions.unit;

            var updateResponse =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(59)]
        public async void ensureUpdatingReferenceReturnsOkForValidNewReference()
        {
            string testNumber = "59";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber, true);

            customizedProductModelView.reference = "Reference " + testNumber;
            customizedProductModelView.userAuthToken = "Valid auth token " + testNumber;

            Uri baseUri = new Uri(httpClient.BaseAddress.ToString() + BASE_URI);

            var request = new HttpRequestMessage()
            {
                RequestUri = baseUri,
                Method = HttpMethod.Post
            };

            string json = JsonConvert.SerializeObject(customizedProductModelView);

            request.Headers.Add("userAuthToken", "Valid auth token " + testNumber);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            GetCustomizedProductModelView createdCustomizedProductModelView =
                await response.Content.ReadAsAsync<GetCustomizedProductModelView>();

            UpdateCustomizedProductModelView update =
                new UpdateCustomizedProductModelView();

            update.reference = "SUPERMASSIVE BLACK HOLE";

            var updateResponse =
                await httpClient.PutAsJsonAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId,
                    update
                );

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var getAfterPut =
                await httpClient.GetAsync
                (
                    BASE_URI + "/"
                    + createdCustomizedProductModelView.customizedProductId
                );

            Assert.Equal(HttpStatusCode.OK, getAfterPut.StatusCode);

            GetCustomizedProductModelView getAfterPutContent =
                await getAfterPut.Content.ReadAsAsync<GetCustomizedProductModelView>();

            Assert.NotEqual(createdCustomizedProductModelView.reference,
                            getAfterPutContent.reference);
            Assert.Equal(update.reference, getAfterPutContent.reference);
        }

        private void assertCustomizedProductModelView(GetCustomizedProductModelView expectedModelView, GetCustomizedProductModelView actualModelView)
        {
            if (expectedModelView.reference != null)
            {
                Assert.Equal(expectedModelView.reference, actualModelView.reference);
            }

            if (expectedModelView.designation != null)
            {
                Assert.Equal(expectedModelView.designation, actualModelView.designation);
            }

            if (expectedModelView.customizedMaterial != null)
            {
                Assert.Equal(expectedModelView.customizedMaterial.materialId, actualModelView.customizedMaterial.materialId);

                if (expectedModelView.customizedMaterial.color != null)
                {
                    //TODO Why does the GetColorModelView not have the color's name?
                    Assert.Equal(expectedModelView.customizedMaterial.color.red, actualModelView.customizedMaterial.color.red);
                    Assert.Equal(expectedModelView.customizedMaterial.color.green, actualModelView.customizedMaterial.color.green);
                    Assert.Equal(expectedModelView.customizedMaterial.color.blue, actualModelView.customizedMaterial.color.blue);
                }

                if (expectedModelView.customizedMaterial.finish != null)
                {
                    Assert.Equal(expectedModelView.customizedMaterial.finish.description, actualModelView.customizedMaterial.finish.description);
                    Assert.Equal(expectedModelView.customizedMaterial.finish.shininess, actualModelView.customizedMaterial.finish.shininess);
                }
            }

            Assert.Equal(expectedModelView.customizedDimensions.depth, actualModelView.customizedDimensions.depth);
            Assert.Equal(expectedModelView.customizedDimensions.width, actualModelView.customizedDimensions.width);
            Assert.Equal(expectedModelView.customizedDimensions.height, actualModelView.customizedDimensions.height);
            Assert.Equal(expectedModelView.customizedDimensions.unit, actualModelView.customizedDimensions.unit);

            Assert.Equal(expectedModelView.product.productId, actualModelView.product.productId);
        }

        private void assertCustomizedProductModelView(AddCustomizedProductModelView expectedModelView, GetCustomizedProductModelView actualModelView)
        {
            if (expectedModelView.reference != null)
            {
                Assert.Equal(expectedModelView.reference, actualModelView.reference);
            }

            if (expectedModelView.designation != null)
            {
                Assert.Equal(expectedModelView.designation, actualModelView.designation);
            }

            if (expectedModelView.customizedMaterial != null)
            {
                Assert.Equal(expectedModelView.customizedMaterial.materialId, actualModelView.customizedMaterial.materialId);

                if (expectedModelView.customizedMaterial.color != null)
                {
                    //TODO Why does the GetColorModelView not have the color's name?
                    Assert.Equal(expectedModelView.customizedMaterial.color.red, actualModelView.customizedMaterial.color.red);
                    Assert.Equal(expectedModelView.customizedMaterial.color.green, actualModelView.customizedMaterial.color.green);
                    Assert.Equal(expectedModelView.customizedMaterial.color.blue, actualModelView.customizedMaterial.color.blue);
                }

                if (expectedModelView.customizedMaterial.finish != null)
                {
                    Assert.Equal(expectedModelView.customizedMaterial.finish.description, actualModelView.customizedMaterial.finish.description);
                    Assert.Equal(expectedModelView.customizedMaterial.finish.shininess, actualModelView.customizedMaterial.finish.shininess);
                }
            }

            Assert.Equal(expectedModelView.customizedDimensions.depth, actualModelView.customizedDimensions.depth);
            Assert.Equal(expectedModelView.customizedDimensions.width, actualModelView.customizedDimensions.width);
            Assert.Equal(expectedModelView.customizedDimensions.height, actualModelView.customizedDimensions.height);
            Assert.Equal(expectedModelView.customizedDimensions.unit, actualModelView.customizedDimensions.unit);

            Assert.Equal(expectedModelView.productId, actualModelView.product.productId);
        }

        private async Task<AddCustomizedProductModelView> createMandatoryComponent(GetCustomizedProductModelView createdCustomizedProductModelView, string testNumber)
        {
            var getProduct =
                await httpClient.GetAsync
                (
                    PRODUCTS_URI + "/" +
                    createdCustomizedProductModelView.product.productId
                );

            Assert.Equal(HttpStatusCode.OK, getProduct.StatusCode);

            GetProductModelView product =
                await getProduct.Content.ReadAsAsync<GetProductModelView>();

            var getComponent =
                await httpClient.GetAsync
                (
                    PRODUCTS_URI + "/" +
                    product.components[0].productId
                );

            Assert.Equal(HttpStatusCode.OK, getComponent.StatusCode);

            GetProductModelView component =
                await getComponent.Content.ReadAsAsync<GetProductModelView>();

            var getComponentMaterial =
                await httpClient.GetAsync("mycm/api/materials" + "/" + component.materials[0].id);

            Assert.Equal(HttpStatusCode.OK, getComponentMaterial.StatusCode);

            GetMaterialModelView material =
                await getComponentMaterial.Content.ReadAsAsync<GetMaterialModelView>();

            AddCustomizedProductModelView addMandatoryComponent =
                new AddCustomizedProductModelView();

            addMandatoryComponent.productId = component.productId;
            addMandatoryComponent.designation = "component " + testNumber;
            addMandatoryComponent.customizedDimensions = new AddCustomizedDimensionsModelView();
            addMandatoryComponent.customizedDimensions.unit =
                createdCustomizedProductModelView.customizedDimensions.unit;
            addMandatoryComponent.customizedDimensions.depth =
                createdCustomizedProductModelView.customizedDimensions.depth;
            addMandatoryComponent.customizedDimensions.height =
                createdCustomizedProductModelView.customizedDimensions.height;
            addMandatoryComponent.customizedDimensions.width =
                createdCustomizedProductModelView.customizedDimensions.width / 2;
            addMandatoryComponent.customizedMaterial = new AddCustomizedMaterialModelView();
            addMandatoryComponent.customizedMaterial.materialId = material.id;
            addMandatoryComponent.customizedMaterial.color =
                material.colors[0];
            addMandatoryComponent.customizedMaterial.finish =
                material.finishes[0];

            return addMandatoryComponent;
        }

        private async Task<GetCustomizedProductModelView> saveCustomizedProduct(AddCustomizedProductModelView customizedProductModelView, bool addSlots)
        {
            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                BASE_URI, customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetCustomizedProductModelView createdCustomizedProductModelView =
               await postCustomizedProduct.Content.
                   ReadAsAsync<GetCustomizedProductModelView>();

            if (!addSlots)
            {
                return createdCustomizedProductModelView;
            }
            else
            {
                AddCustomizedDimensionsModelView slotDimensions =
                    new AddCustomizedDimensionsModelView();

                slotDimensions.depth =
                    createdCustomizedProductModelView.customizedDimensions.depth / 2;
                slotDimensions.height =
                    createdCustomizedProductModelView.customizedDimensions.height / 2;
                slotDimensions.width =
                    createdCustomizedProductModelView.customizedDimensions.width / 2;
                slotDimensions.unit =
                    createdCustomizedProductModelView.customizedDimensions.unit;

                var addSlotToCustomizedProduct =
                    await httpClient.PostAsJsonAsync
                        (
                            BASE_URI + "/"
                            + createdCustomizedProductModelView.customizedProductId
                            + "/slots", slotDimensions
                        );

                Assert.Equal(HttpStatusCode.Created, addSlotToCustomizedProduct.StatusCode);

                GetCustomizedProductModelView customizedProductWithSlot =
                    await addSlotToCustomizedProduct.Content.
                        ReadAsAsync<GetCustomizedProductModelView>();

                return customizedProductWithSlot;
            }
        }

        private async Task<AddCustomizedProductModelView> createUnfinishedCustomizedProduct(string testNumber, bool hasSlotsAndComponents)
        {
            AddProductCategoryModelView categoryModelView =
                createCategoryModelView(testNumber);

            var postCategory = await httpClient.PostAsJsonAsync(
                "mycm/api/categories", categoryModelView
            );

            GetProductCategoryModelView categoryModelViewFromPost =
                await postCategory.Content.ReadAsAsync<GetProductCategoryModelView>();

            MaterialDTO materialDTO = createMaterialDTO(testNumber);

            var postMaterial = await httpClient.PostAsJsonAsync(
                "mycm/api/materials", materialDTO
            );

            AddProductMaterialModelView materialModelViewFromPost =
                await postMaterial.Content.ReadAsAsync<AddProductMaterialModelView>();

            AddProductModelView productModelView =
                createProductWithoutComponentsAndWithoutSlots(
                    categoryModelViewFromPost,
                    materialModelViewFromPost,
                    testNumber
                );

            var postProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/products", productModelView
            );

            GetProductModelView productModelViewFromPost =
                await postProduct.Content.ReadAsAsync<GetProductModelView>();


            AddCustomizedProductModelView modelView =
                new AddCustomizedProductModelView();

            modelView.designation = "Designation " + testNumber;
            modelView.productId = productModelViewFromPost.productId;
            modelView.customizedDimensions = new AddCustomizedDimensionsModelView();
            modelView.customizedDimensions.depth = 500;
            modelView.customizedDimensions.width = 1000;
            modelView.customizedDimensions.height = 1000;
            modelView.customizedDimensions.unit = "mm";

            return modelView;
        }

        private async Task<AddCustomizedProductModelView> createFinishedCustomizedProduct(string testNumber, bool hasSlotsAndComponents)
        {
            AddProductCategoryModelView categoryModelView =
                createCategoryModelView(testNumber);

            var postCategory = await httpClient.PostAsJsonAsync(
                "mycm/api/categories", categoryModelView
            );

            GetProductCategoryModelView categoryModelViewFromPost =
                await postCategory.Content.ReadAsAsync<GetProductCategoryModelView>();

            MaterialDTO materialDTO = createMaterialDTO(testNumber);

            var postMaterial = await httpClient.PostAsJsonAsync(
                "mycm/api/materials", materialDTO
            );

            AddProductMaterialModelView materialModelViewFromPost =
                await postMaterial.Content.ReadAsAsync<AddProductMaterialModelView>();

            AddProductModelView productModelView = null;
            if (!hasSlotsAndComponents)
            {
                productModelView =
                    createProductWithoutComponentsAndWithoutSlots(
                        categoryModelViewFromPost,
                        materialModelViewFromPost,
                        testNumber
                );
            }
            else
            {
                var firstComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 1 " + testNumber);

                var firstComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, firstComponentModelView);

                Assert.Equal(HttpStatusCode.Created, firstComponentPostResponse.StatusCode);

                GetProductModelView firstComponentModelViewFromPost = await firstComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

                var secondComponentModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelViewFromPost, materialModelViewFromPost, "component 2 " + testNumber);

                var secondComponentPostResponse = await httpClient.PostAsJsonAsync(PRODUCTS_URI, secondComponentModelView);

                Assert.Equal(HttpStatusCode.Created, secondComponentPostResponse.StatusCode);

                GetProductModelView secondComponentModelViewFromPost = await secondComponentPostResponse.Content.ReadAsAsync<GetProductModelView>();

                productModelView =
                    createProductWithComponentsAndWithSlots(
                        categoryModelViewFromPost,
                        materialModelViewFromPost,
                        testNumber,
                        firstComponentModelViewFromPost.productId,
                        secondComponentModelViewFromPost.productId
                    );
            }

            var postProduct = await httpClient.PostAsJsonAsync(
                PRODUCTS_URI, productModelView
            );

            GetProductModelView productModelViewFromPost =
                await postProduct.Content.ReadAsAsync<GetProductModelView>();


            AddCustomizedProductModelView modelView =
                new AddCustomizedProductModelView();

            modelView.designation = "Designation " + testNumber;
            AddCustomizedMaterialModelView customizedMaterialModelView =
                new AddCustomizedMaterialModelView();
            customizedMaterialModelView.color = materialDTO.colors[0];
            customizedMaterialModelView.finish = materialDTO.finishes[0];
            customizedMaterialModelView.materialId = productModelViewFromPost.materials[0].id;
            modelView.customizedMaterial = customizedMaterialModelView;
            modelView.productId = productModelViewFromPost.productId;
            modelView.customizedDimensions = new AddCustomizedDimensionsModelView();
            modelView.customizedDimensions.depth = 500;
            modelView.customizedDimensions.width = 1000;
            modelView.customizedDimensions.height = 1000;
            modelView.customizedDimensions.unit = "mm";

            return modelView;
        }

        private AddProductModelView createProductWithComponentsAndWithSlots(GetProductCategoryModelView categoryModelView, AddProductMaterialModelView materialModelView, string testNumber, long firstComponentId, long secondComponentId)
        {
            AddProductModelView productModelView = createProductWithoutComponentsAndWithoutSlots(categoryModelView, materialModelView, testNumber);
            productModelView.slotWidths = new AddProductSlotWidthsModelView();
            productModelView.slotWidths.maxWidth = 500;
            productModelView.slotWidths.minWidth = 50;
            productModelView.slotWidths.recommendedWidth = 300;
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
    }
}