using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using backend_tests.Setup;
using backend_tests.utils;
using core.modelview.productcategory;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace backend_tests.Controllers
{
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public class ProductCategoryControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        //!Do not compare response's DTO's ids since they may be different from expected depending on the used provider

        private const string baseUrl = "/mycm/api/categories";

        private HttpClient client;

        private TestFixture<TestStartupSQLite> fixture;

        public ProductCategoryControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            this.fixture = fixture;
            this.client = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("http://localhost:5001")
            });

        }

        [Fact, TestPriority(0)]
        public async Task ensureGetAllProductCategoriesReturnsNotFoundIfNoCategoriesHaveBeenAdded()
        {
            var response = await client.GetAsync(baseUrl);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact, TestPriority(1)]
        public async Task<GetProductCategoryModelView> ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully()
        {
            AddProductCategoryModelView addCategoryMV = new AddProductCategoryModelView()
            { name = "Drawers" + Guid.NewGuid().ToString("n") };

            var response = await client.PostAsJsonAsync(baseUrl, addCategoryMV);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return JsonConvert.DeserializeObject<GetProductCategoryModelView>(await response.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(2)]
        public async Task ensureAddProductCategoryReturnsCreatedCategoryDTOInBody()
        {
            string categoryName = "Drawers" + Guid.NewGuid().ToString("n");

            AddProductCategoryModelView addCategoryMV = new AddProductCategoryModelView() { name = categoryName };

            var response = await client.PostAsJsonAsync(baseUrl, addCategoryMV);

            GetProductCategoryModelView actual = await response.Content.ReadAsAsync<GetProductCategoryModelView>();
            GetProductCategoryModelView expected = new GetProductCategoryModelView() { name = categoryName };

            Assert.Equal(expected.name, actual.name);
        }


        [Fact, TestPriority(3)]
        public async Task ensureAddProductCategoryReturnsBadRequestIfCategoryHasDuplicateName()
        {
            GetProductCategoryModelView getCategoryMV = await ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();

            var response = await client.PostAsJsonAsync(baseUrl, getCategoryMV);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task ensureAddProductCategoryReturnsBadRequestIfCategoryDoesNotHaveName()
        {
            AddProductCategoryModelView addCategoryMV = new AddProductCategoryModelView();

            var response = await client.PostAsJsonAsync(baseUrl, addCategoryMV);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(5)]
        public async Task ensureAddProductCategoryReturnsBadRequestIfDTOIsNull()
        {
            AddProductCategoryModelView addCategoryMV = null;

            var response = await client.PostAsJsonAsync(baseUrl, addCategoryMV);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact, TestPriority(6)]
        public async Task ensureFindProductCategoryReturnsOkIfCategoryExists()
        {
            GetProductCategoryModelView getCategoryMV = await ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();

            var response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, getCategoryMV.id));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async Task ensureFindProductCategoryReturnsNotFoundIfCategoryDoesNotExist()
        {
            var response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, 0));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(8)]
        public async Task ensureDeleteProductCategoryReturnsNotFoundIfCategoryDoesNotExist()
        {
            var response = await client.DeleteAsync(string.Format("{0}/{1}", baseUrl, 0));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(9)]
        public async Task ensureDeleteProductCategoryReturnsNoContentIfCategoryWasRemoved()
        {
            GetProductCategoryModelView getCategoryMV = await ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();

            var response = await client.DeleteAsync(string.Format("{0}/{1}", baseUrl, getCategoryMV.id));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, TestPriority(10)]
        public async Task ensureDeleteProductCategoryReallyDeletes()
        {
            GetProductCategoryModelView getCategoryMV = await ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();

            var response = await client.DeleteAsync(string.Format("{0}/{1}", baseUrl, getCategoryMV.id));

            response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, getCategoryMV.id));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact, TestPriority(11)]
        public async Task ensureGetProductCategoryByNameReturnsOkIfCategoryExists()
        {
            string categoryName = "Shelves" + Guid.NewGuid().ToString("n");
            AddProductCategoryModelView addCategoryMV = new AddProductCategoryModelView() { name = categoryName };

            var response = await client.PostAsJsonAsync(baseUrl, addCategoryMV);

            response = await client.GetAsync(string.Format("{0}?name={1}", baseUrl, categoryName));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(12)]
        public async Task ensureGetProductCategoryByNameReturnsNotFoundIfCategoryDoesNotExist()
        {
            string categoryName = "Mirrors" + Guid.NewGuid().ToString("n");

            var response = await client.GetAsync(string.Format("{0}?name={1}", baseUrl, categoryName));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(13)]
        public async Task ensureGetAllProductCategoriesWorks()
        {

            var response = await client.GetAsync(baseUrl);

            string contentString = await response.Content.ReadAsStringAsync();

            List<GetBasicProductCategoryModelView> list = JsonConvert.DeserializeObject<List<GetBasicProductCategoryModelView>>(contentString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(list);
        }
    }
}