using backend_tests.Setup;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using core.modelview.customizedproductcollection;
using core.modelview.customizedproduct;
using System.Collections.Generic;
using core.modelview.productcategory;
using core.dto;
using core.modelview.customizedmaterial;
using System.Threading.Tasks;
using core.modelview.productmaterial;
using core.modelview.product;
using core.modelview.measurement;
using core.modelview.dimension;
using core.modelview.customizeddimensions;
using support.utils;
using static core.domain.CustomizedProduct;

namespace backend_tests.Controllers
{

    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    //TODO Test CRUD Operation flow
    public sealed class CustomizedProductsCollectionControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string BASE_URI = "mycm/api/collections";

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

        [Fact, TestPriority(-11)]
        public async void ensureAddingCollectionReturnsBadRequestIfNoCustomizedProductsExist()
        {
            string testNumber = "-11";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNonExistingCustomizedProduct(testNumber);

            var response = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(-10)]
        public async void ensureGetByNameReturnsOk()
        {
            string testNumber = "-10";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var getById = await httpClient.GetAsync(BASE_URI + "?name=" + collectionModelView.name);

            Assert.Equal(HttpStatusCode.OK, getById.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromGet =
                await getById.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(collectionModelView, modelViewFromGet);

            await httpClient.DeleteAsync(BASE_URI + "/" + modelViewFromGet.id);
        }

        [Fact, TestPriority(-9)]
        public async void ensureGetByIdReturnsOk()
        {
            string testNumber = "-9";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var getById = await httpClient.GetAsync(BASE_URI + "/" + collectionModelView.id);

            Assert.Equal(HttpStatusCode.OK, getById.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromGet =
                await getById.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(collectionModelView, modelViewFromGet);

            await httpClient.DeleteAsync(BASE_URI + "/" + modelViewFromGet.id);
        }

        [Fact, TestPriority(-8)]
        public async void ensureGetAllReturnsAllAvailableCollections()
        {
            string testNumber = "-8";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);
            AddCustomizedProductCollectionModelView otherModelView = createCollectionWithNoCustomizedProducts(testNumber + " 2");

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            otherModelView.customizedProducts =
                new List<GetBasicCustomizedProductModelView>() { createdCustomizedProductModelView };

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            var otherPostCollection = await httpClient.PostAsJsonAsync(BASE_URI, otherModelView);

            Assert.Equal(HttpStatusCode.Created, otherPostCollection.StatusCode);

            GetCustomizedProductCollectionModelView otherCollectionModelView =
                await otherPostCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var getAll = await httpClient.GetAsync(BASE_URI);

            Assert.Equal(HttpStatusCode.OK, getAll.StatusCode);

            GetAllCustomizedProductCollectionsModelView getAllCustomizedProductCollections =
                await getAll.Content.ReadAsAsync<GetAllCustomizedProductCollectionsModelView>();

            Assert.Equal(modelView.name, getAllCustomizedProductCollections[0].name);
            Assert.Equal(otherModelView.name, getAllCustomizedProductCollections[1].name);
            Assert.False(getAllCustomizedProductCollections[0].hasCustomizedProducts);
            Assert.True(getAllCustomizedProductCollections[1].hasCustomizedProducts);

            await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id);
            await httpClient.DeleteAsync(BASE_URI + "/" + otherCollectionModelView.id);
            await httpClient.DeleteAsync("mycm/api/customizedproducts/" + createdCustomizedProductModelView.customizedProductId);
        }

        [Fact, TestPriority(-7)]
        public async void ensureDisablingCollectionReturnsNotFoundIfNoCollectionsExist()
        {
            var deleteCollection = await httpClient.DeleteAsync(BASE_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, deleteCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(-6)]
        public async void ensureDeletingCustomizedProductFromCollectionReturnsNotFoundIfNoCollectionsExist()
        {
            string testNumber = "-6";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            var deleteFromCollection = await httpClient.DeleteAsync(
                BASE_URI + "/1/customizedproducts/" + createdCustomizedProductModelView.customizedProductId
            );

            Assert.Equal(HttpStatusCode.NotFound, deleteFromCollection.StatusCode);

            await httpClient.DeleteAsync("mycm/api/customizedproducts/" + createdCustomizedProductModelView.customizedProductId);
            //TODO Compare Message
        }

        [Fact, TestPriority(-5)]
        public async void ensureUpdatingCollectionReturnsNotFoundIfNoCollectionsExist()
        {
            string testNumber = "-5";

            UpdateCustomizedProductCollectionModelView modelView =
                createUpdateCollectionModelView(testNumber);

            var putCollection = await httpClient.PutAsJsonAsync
            (
                BASE_URI + "/1",
                modelView
            );

            Assert.Equal(HttpStatusCode.NotFound, putCollection.StatusCode);
        }

        [Fact, TestPriority(-4)]
        public async void ensureAddingCustomizedProductToCollectionReturnsNotFoundIfNoCollectionsExist()
        {
            string testNumber = "-4";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductToCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            modelView.customizedProductId = createdCustomizedProductModelView.customizedProductId;


            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/1/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.NotFound, postCustomizedProductToCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-3)]
        public async void ensureGetByNameReturnsNotFoundWhenNoCollectionsExist()
        {
            var response = await httpClient.GetAsync(BASE_URI + "?name=winter");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-2)]
        public async void ensureGetAllReturnsNotFoundWhenNoCollectionsExist()
        {
            var response = await httpClient.GetAsync(BASE_URI);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(-1)]
        public async void ensureGetByIdReturnsNotFoundWhenNoCollectionsExist()
        {
            var response = await httpClient.GetAsync(BASE_URI + "/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(0)]
        public async void ensureUpdatingCollectionReturnsNotFoundForNonExistingCollectionId()
        {
            string testNumber = "0";

            AddCustomizedProductCollectionModelView addCollectionModelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, addCollectionModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();


            UpdateCustomizedProductCollectionModelView modelView =
                createUpdateCollectionModelView(testNumber);

            var putCollection = await httpClient.PutAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + 1,
                modelView
            );

            Assert.Equal(HttpStatusCode.NotFound, putCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(1)]
        public async void ensureAddingCustomizedProductToCollectionReturnsBadRequestIfNoCustomizedProductsExist()
        {
            string testNumber = "1";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView addCustomizedProductToCustomizedProductCollectionModelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            addCustomizedProductToCustomizedProductCollectionModelView.customizedProductId = 1;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + "/customizedproducts",
                addCustomizedProductToCustomizedProductCollectionModelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postCustomizedProductToCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(3)]
        public async void ensureDeletingCustomizedProductFromCollectionReturnsNotFoundIfNoCustomizedProductsExist()
        {
            string testNumber = "4";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var deleteFromCollection = await httpClient.DeleteAsync
            (
                BASE_URI + "/" + collectionModelView.id + "/customizedproducts/1"
            );

            Assert.Equal(HttpStatusCode.NotFound, deleteFromCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(34)]
        public async void ensureDeletingCustomizedProductFromCollectionReturnsNotFoundForNonExistingCollectionId()
        {
            string testNumber = "34";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();


            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductToCustomizedProductCollectionModelView addProductToCollection =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            addProductToCollection.customizedProductId = createdCustomizedProductModelView.customizedProductId;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + "/customizedproducts",
                createdCustomizedProductModelView.customizedProductId
            );

            var deleteFromCollection = await httpClient.DeleteAsync(
                BASE_URI + "/" + collectionModelView.id + 1 + "customizedproducts/" + createdCustomizedProductModelView.customizedProductId
            );

            Assert.Equal(HttpStatusCode.NotFound, deleteFromCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(5)]
        public async void ensureAddingCustomizedProductToCollectionReturnsNotFoundForNonExistingCollectionId()
        {
            string testNumber = "5";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView addCollectionModelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, addCollectionModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            modelView.customizedProductId = createdCustomizedProductModelView.customizedProductId;


            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + 1 + "/customizedproducts",
                addCollectionModelView
            );

            Assert.Equal(HttpStatusCode.NotFound, postCustomizedProductToCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(6)]
        public async void ensureDisablingCollectionReturnsNotFoundForNonExistingCollectionId()
        {
            string testNumber = "6";

            AddCustomizedProductCollectionModelView addCollectionModelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, addCollectionModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var deleteCollection = await httpClient.DeleteAsync(BASE_URI + "/" + modelView.id + 1);

            Assert.Equal(HttpStatusCode.NotFound, deleteCollection.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async void ensureAddingCustomizedProductToCollectionReturnsBadRequestForNonExistingCustomizedProductId()
        {
            string testNumber = "7";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductToCustomizedProductCollectionModelView addCustomizedProductToCustomizedProductCollectionModelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            addCustomizedProductToCustomizedProductCollectionModelView.customizedProductId = createdCustomizedProductModelView.customizedProductId + 1;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + "/customizedproducts",
                addCustomizedProductToCustomizedProductCollectionModelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postCustomizedProductToCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(8)]
        public async void ensureGetByIdReturnsNotFoundForNonExistingId()
        {
            string testNumber = "8";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postResponse = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            GetCustomizedProductCollectionModelView postResponseModelView =
                await postResponse.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var getResponse = await httpClient.GetAsync(BASE_URI + "/" + postResponseModelView.id + 1);

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(9)]
        public async void ensureGetByNameReturnsNotFoundForNonExistingName()
        {
            string testNumber = "9";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postResponse = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            var getResponse = await httpClient.GetAsync(BASE_URI + "?name=thisnamedoesntexist");

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(10)]
        public async void ensureGetByNameReturnsNotFoundForInvalidName()
        {
            var response = await httpClient.GetAsync(BASE_URI + "?name=       sadf    ");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(11)]
        public async void ensureAddingCollectionReturnsBadRequestForNonExistingCustomizedProductId()
        {
            string testNumber = "11";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView collectionModelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            GetBasicCustomizedProductModelView nonExistingCustomizedProductModelView =
                new GetBasicCustomizedProductModelView();

            nonExistingCustomizedProductModelView.reference = "nonexisting reference";
            nonExistingCustomizedProductModelView.designation = "nonexisting designation";
            nonExistingCustomizedProductModelView.serialNumber = "nonexisting serial number";
            nonExistingCustomizedProductModelView.productId = createdCustomizedProductModelView.productId;
            nonExistingCustomizedProductModelView.customizedProductId = createdCustomizedProductModelView.customizedProductId + 1;

            collectionModelView.customizedProducts =
                new List<GetBasicCustomizedProductModelView>();

            collectionModelView.customizedProducts.Add(
                nonExistingCustomizedProductModelView
            );

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, collectionModelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(12)]
        public async void ensureDeletingCustomizedProductFromCollectionReturnsNotFoundForNonExistingCustomizedProductId()
        {
            string testNumber = "12";

            AddCustomizedProductCollectionModelView modelView =
                createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(
                BASE_URI, modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();


            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductToCustomizedProductCollectionModelView addProductToCollection =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            addProductToCollection.customizedProductId = createdCustomizedProductModelView.customizedProductId;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelView.id + "/customizedproducts",
                createdCustomizedProductModelView.customizedProductId
            );

            var deleteFromCollection = await httpClient.DeleteAsync(
                BASE_URI + "/" + collectionModelView.id + "customizedproducts/" + createdCustomizedProductModelView.customizedProductId + 1
            );

            Assert.Equal(HttpStatusCode.NotFound, deleteFromCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(13)]
        public async void ensureAddingCollectionWithNullBodyReturnsBadRequest()
        {
            AddCustomizedProductCollectionModelView modelView = null;

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, postCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(14)]
        public async void ensureAddingCollectionWithNullNameReturnsBadRequest()
        {
            AddCustomizedProductCollectionModelView modelView = new AddCustomizedProductCollectionModelView();

            modelView.name = null;

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, postCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(15)]
        public async void ensureAddingCollectionWithInvalidNameReturnsBadRequest()
        {
            AddCustomizedProductCollectionModelView modelView = new AddCustomizedProductCollectionModelView();

            modelView.name = "     ";

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, postCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(16)]
        public async void ensureAddingCollectionWithUnfinishedCustomizedProductReturnsBadRquest()
        {
            string testNumber = "16";

            AddCustomizedProductModelView customizedProductModelView =
                await createUnfinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            modelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();
            modelView.customizedProducts.Add(createdCustomizedProductModelView);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, postCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(17)]
        public async void ensureAddingDuplicateCollectionReturnsBadRequest()
        {
            AddCustomizedProductCollectionModelView modelView = new AddCustomizedProductCollectionModelView();

            modelView.name = "Hi";

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            var duplicateCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.BadRequest, duplicateCollection.StatusCode);

            //TODO Compare message
        }

        [Fact, TestPriority(18)]
        public async void ensureAddingValidCollectionWithNoCustomizedProductsReturnsCreated()
        {
            string testNumber = "18";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(modelView, modelViewFromPost);

            var getCollection = await httpClient.GetAsync(BASE_URI + "/" + modelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, getCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromGet =
                await getCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(modelViewFromGet, modelViewFromPost);
        }

        [Fact, TestPriority(19)]
        public async void ensureAddingValidCollectionWithFinishedCustomizedProductsReturnsCreated()
        {
            string testNumber = "19";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);
            modelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();

            modelView.customizedProducts.Add(createdCustomizedProductModelView);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(modelView, modelViewFromPost);

            var getCollection = await httpClient.GetAsync(BASE_URI + "/" + modelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, getCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromGet =
                await getCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(modelViewFromGet, modelViewFromPost);
        }

        [Fact, TestPriority(20)]
        public async void ensureAddingCustomizedProductToCollectionReturnsBadRequestIfBodyIsEmpty()
        {
            string testNumber = "20";

            AddCustomizedProductCollectionModelView createCollectionModelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, createCollectionModelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView modelView = null;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelViewFromPost.id + "/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postCustomizedProductToCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(21)]
        public async void ensureAddingUnfinishedCustomizedProductToCollectionReturnsBadRequest()
        {
            string testNumber = "21";

            AddCustomizedProductModelView customizedProductModelView =
                await createUnfinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            AddCustomizedProductCollectionModelView createCollectionModelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, createCollectionModelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            modelView.customizedProductId = createdCustomizedProductModelView.customizedProductId;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelViewFromPost.id + "/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postCustomizedProductToCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(22)]
        public async void ensureAddingDuplicateCustomizedProductToCollectionReturnsBadRequest()
        {
            string testNumber = "22";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView createCollectionModelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, createCollectionModelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            modelView.customizedProductId = createdCustomizedProductModelView.customizedProductId;

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelViewFromPost.id + "/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProductToCollection.StatusCode);

            var postDuplicate = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelViewFromPost.id + "/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.BadRequest, postDuplicate.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(23)]
        public async void ensureAddingFinishedCustomizedProductToCollectionReturnsOk()
        {
            string testNumber = "23";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView createCollectionModelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, createCollectionModelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            AddCustomizedProductToCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductToCustomizedProductCollectionModelView();

            modelView.customizedProductId = createdCustomizedProductModelView.customizedProductId;

            Assert.Null(collectionModelViewFromPost.customizedProducts);

            var postCustomizedProductToCollection = await httpClient.PostAsJsonAsync
            (
                BASE_URI + "/" + collectionModelViewFromPost.id + "/customizedproducts",
                modelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProductToCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionAfterCustomizedProductPost =
                await postCustomizedProductToCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            Assert.Equal(collectionModelViewFromPost.name, collectionAfterCustomizedProductPost.name);
            Assert.NotEmpty(collectionAfterCustomizedProductPost.customizedProducts);

            var getAfterPost = await httpClient.GetAsync(BASE_URI + "/" + collectionAfterCustomizedProductPost.id);

            Assert.Equal(HttpStatusCode.OK, getAfterPost.StatusCode);

            GetCustomizedProductCollectionModelView collectionAfterGetAfterPost =
                await getAfterPost.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            assertCustomizedProductCollection(collectionAfterCustomizedProductPost, collectionAfterGetAfterPost);
        }

        [Fact, TestPriority(24)]
        public async void ensureUpdatingCollectionWithNullBodyReturnsBadRequest()
        {
            string testNumber = "24";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            UpdateCustomizedProductCollectionModelView updateModelView = null;

            var putCollection = await httpClient.PutAsJsonAsync(BASE_URI + "/" + modelViewFromPost.id, updateModelView);

            Assert.Equal(HttpStatusCode.BadRequest, putCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(25)]
        public async void ensureUpdatingCollectionWithSameNameReturnsBadRequest()
        {
            string testNumber = "25";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            UpdateCustomizedProductCollectionModelView updateModelView =
                new UpdateCustomizedProductCollectionModelView();

            updateModelView.name = modelViewFromPost.name;

            var putCollection = await httpClient.PutAsJsonAsync(BASE_URI + "/" + modelViewFromPost.id, updateModelView);

            Assert.Equal(HttpStatusCode.BadRequest, putCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(26)]
        public async void ensureUpdatingCollectionWithNullNameReturnsBadRequest()
        {
            string testNumber = "26";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            UpdateCustomizedProductCollectionModelView updateModelView =
                new UpdateCustomizedProductCollectionModelView();

            updateModelView.name = null;

            var putCollection = await httpClient.PutAsJsonAsync(BASE_URI + "/" + modelViewFromPost.id, updateModelView);

            Assert.Equal(HttpStatusCode.BadRequest, putCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(27)]
        public async void ensureUpdatingCollectionWithInvalidNameReturnsBadRequest()
        {
            string testNumber = "27";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            UpdateCustomizedProductCollectionModelView updateModelView =
                new UpdateCustomizedProductCollectionModelView();

            updateModelView.name = "";

            var putCollection = await httpClient.PutAsJsonAsync(BASE_URI + "/" + modelViewFromPost.id, updateModelView);

            Assert.Equal(HttpStatusCode.BadRequest, putCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(28)]
        public async void ensureUpdatingCollectionWithValidNameReturnsOk()
        {
            string testNumber = "28";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView modelViewFromPost =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            UpdateCustomizedProductCollectionModelView updateModelView =
                new UpdateCustomizedProductCollectionModelView();

            updateModelView.name = "New really cool name";

            var putCollection = await httpClient.PutAsJsonAsync(BASE_URI + "/" + modelViewFromPost.id, updateModelView);

            Assert.Equal(HttpStatusCode.OK, putCollection.StatusCode);

            GetBasicCustomizedProductCollectionModelView modelViewFromPut =
                await putCollection.Content.ReadAsAsync<GetBasicCustomizedProductCollectionModelView>();

            Assert.Equal(updateModelView.name, modelViewFromPut.name);

            var getAfterPut = await httpClient.GetAsync(BASE_URI + "/" + modelViewFromPost.id);

            Assert.Equal(HttpStatusCode.OK, getAfterPut.StatusCode);

            GetBasicCustomizedProductCollectionModelView modelViewFromGetAfterPut =
                await getAfterPut.Content.ReadAsAsync<GetBasicCustomizedProductCollectionModelView>();

            Assert.Equal(modelViewFromPut.name, modelViewFromGetAfterPut.name);
            Assert.Equal(modelViewFromPut.id, modelViewFromGetAfterPut.id);
        }

        [Fact, TestPriority(29)]
        public async void ensureDeletingCustomizedProductFromWrongCollectionReturnsBadRequest()
        {
            string testNumber = "29";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);
            modelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();

            modelView.customizedProducts.Add(createdCustomizedProductModelView);

            AddCustomizedProductCollectionModelView otherModelView = createCollectionWithNoCustomizedProducts(testNumber + " 2");

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);
            var postOtherCollection = await httpClient.PostAsJsonAsync(BASE_URI, otherModelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);
            Assert.Equal(HttpStatusCode.Created, postOtherCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();
            GetCustomizedProductCollectionModelView otherCollectionModelView =
                await postOtherCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var deleteFromWrongCollection = await httpClient.DeleteAsync(BASE_URI + "/" + otherCollectionModelView.id + "/customizedproducts/" + collectionModelView.customizedProducts[0].customizedProductId);

            Assert.Equal(HttpStatusCode.NotFound, deleteFromWrongCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(30)]
        public async void ensureDeletingCustomizedProductFromCollectionThatDoesntHaveAnyReturnsBadRequest()
        {
            string testNumber = "30";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var deleteFromCollection = await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id + "/customizedproducts" + createdCustomizedProductModelView.customizedProductId);

            Assert.Equal(HttpStatusCode.NotFound, deleteFromCollection.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(31)]
        public async void ensureDeletingCustomizedProductFromCollectionReturnsNoContent()
        {
            string testNumber = "31";

            AddCustomizedProductModelView customizedProductModelView =
                await createFinishedCustomizedProduct(testNumber);

            var postCustomizedProduct = await httpClient.PostAsJsonAsync(
                "mycm/api/customizedproducts", customizedProductModelView
            );

            Assert.Equal(HttpStatusCode.Created, postCustomizedProduct.StatusCode);

            GetBasicCustomizedProductModelView createdCustomizedProductModelView =
                await postCustomizedProduct.Content.
                    ReadAsAsync<GetBasicCustomizedProductModelView>();

            UpdateCustomizedProductModelView updateCustomizedProductModelView =
                new UpdateCustomizedProductModelView();
            updateCustomizedProductModelView.customizationStatus = CustomizationStatus.FINISHED;

            var finishCustomizedProduct = await httpClient.PutAsJsonAsync(
                "mycm/api/customizedProducts/" + createdCustomizedProductModelView.customizedProductId,
                updateCustomizedProductModelView
            );

            Assert.Equal(HttpStatusCode.OK, finishCustomizedProduct.StatusCode);

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);
            modelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();

            modelView.customizedProducts.Add(createdCustomizedProductModelView);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            Assert.NotNull(collectionModelView.customizedProducts);

            var deleteFromCollection = await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id + "/customizedproducts/" + collectionModelView.customizedProducts[0].customizedProductId);

            Assert.Equal(HttpStatusCode.NoContent, deleteFromCollection.StatusCode);

            var getAfterDelete = await httpClient.GetAsync(BASE_URI + "/" + collectionModelView.id);

            GetCustomizedProductCollectionModelView modelViewAfterGetAfterDelete =
                await getAfterDelete.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            Assert.Null(modelViewAfterGetAfterDelete.customizedProducts);
        }

        [Fact, TestPriority(32)]
        public async void ensureDisablingDisabledCollectionReturnsNotFound()
        {
            string testNumber = "32";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var disableCollection = await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id);

            Assert.Equal(HttpStatusCode.NoContent, disableCollection.StatusCode);

            var disableAfterDisable = await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id);

            Assert.Equal(HttpStatusCode.NotFound, disableAfterDisable.StatusCode);

            //TODO Compare Message
        }

        [Fact, TestPriority(33)]
        public async void ensureDisablingActiveCollectionReturnsNoContent()
        {
            string testNumber = "33";

            AddCustomizedProductCollectionModelView modelView = createCollectionWithNoCustomizedProducts(testNumber);

            var postCollection = await httpClient.PostAsJsonAsync(BASE_URI, modelView);

            Assert.Equal(HttpStatusCode.Created, postCollection.StatusCode);

            GetCustomizedProductCollectionModelView collectionModelView =
                await postCollection.Content.ReadAsAsync<GetCustomizedProductCollectionModelView>();

            var disableCollection = await httpClient.DeleteAsync(BASE_URI + "/" + collectionModelView.id);

            Assert.Equal(HttpStatusCode.NoContent, disableCollection.StatusCode);

            var getAfterDelete = await httpClient.GetAsync(BASE_URI + "/" + collectionModelView.id);

            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            //TODO Compare message
        }

        private void assertCustomizedProductCollection(AddCustomizedProductCollectionModelView expectedModelView, GetCustomizedProductCollectionModelView actualModelView)
        {
            Assert.Equal(expectedModelView.name, actualModelView.name);

            if (!Collections.isEnumerableNullOrEmpty(expectedModelView.customizedProducts))
            {
                for (int i = 0; i < expectedModelView.customizedProducts.Count; i++)
                {
                    Assert.Equal(expectedModelView.customizedProducts[i].designation,
                                 actualModelView.customizedProducts[i].designation);
                    Assert.Equal(expectedModelView.customizedProducts[i].reference,
                                 actualModelView.customizedProducts[i].reference);
                }
            }
        }

        private void assertCustomizedProductCollection(GetCustomizedProductCollectionModelView expectedModelView, GetCustomizedProductCollectionModelView actualModelView)
        {
            Assert.Equal(expectedModelView.name, actualModelView.name);

            if (!Collections.isEnumerableNullOrEmpty(expectedModelView.customizedProducts))
            {
                for (int i = 0; i < expectedModelView.customizedProducts.Count; i++)
                {
                    Assert.Equal(expectedModelView.customizedProducts[i].designation,
                                 actualModelView.customizedProducts[i].designation);
                    Assert.Equal(expectedModelView.customizedProducts[i].reference,
                                 actualModelView.customizedProducts[i].reference);
                    Assert.Equal(expectedModelView.customizedProducts[i].productId,
                                 actualModelView.customizedProducts[i].productId);
                    Assert.Equal(expectedModelView.customizedProducts[i].customizedProductId,
                                 actualModelView.customizedProducts[i].customizedProductId);
                }
            }
        }

        private AddCustomizedProductCollectionModelView createCollectionWithNoCustomizedProducts(string testNumber)
        {
            AddCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductCollectionModelView();

            modelView.name = "Collection " + testNumber;

            return modelView;
        }

        private AddCustomizedProductCollectionModelView createCollectionWithNonExistingCustomizedProduct(string testNumber)
        {
            AddCustomizedProductCollectionModelView modelView =
                new AddCustomizedProductCollectionModelView();

            modelView.name = "Collection " + testNumber;
            GetBasicCustomizedProductModelView customizedProductModelView =
                new GetBasicCustomizedProductModelView();
            customizedProductModelView.designation = "designation";
            customizedProductModelView.productId = 1;
            customizedProductModelView.customizedProductId = 1;
            customizedProductModelView.reference = "reference";
            customizedProductModelView.serialNumber = "serialnumber";
            modelView.customizedProducts = new List<GetBasicCustomizedProductModelView>()
            {
                customizedProductModelView
            };

            return modelView;
        }

        private UpdateCustomizedProductCollectionModelView createUpdateCollectionModelView(string testNumber)
        {
            UpdateCustomizedProductCollectionModelView modelView =
                new UpdateCustomizedProductCollectionModelView();

            modelView.name = "new name " + testNumber;

            return modelView;
        }

        private async Task<AddCustomizedProductModelView> createUnfinishedCustomizedProduct(string testNumber)
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

            //modelView.reference = "Reference " + testNumber;
            modelView.designation = "Designation " + testNumber;
            modelView.productId = productModelViewFromPost.productId;
            modelView.customizedDimensions = new AddCustomizedDimensionsModelView();
            modelView.customizedDimensions.depth = 500;
            modelView.customizedDimensions.width = 1000;
            modelView.customizedDimensions.height = 1000;
            modelView.customizedDimensions.unit = "mm";
            modelView.userAuthToken = "reallycooltoken";

            return modelView;
        }

        private async Task<AddCustomizedProductModelView> createFinishedCustomizedProduct(string testNumber)
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