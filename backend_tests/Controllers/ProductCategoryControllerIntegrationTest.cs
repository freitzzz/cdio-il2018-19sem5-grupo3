using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using backend_tests.Setup;
using backend_tests.utils;
using core.dto;
using Newtonsoft.Json;
using Xunit;

namespace backend_tests.Controllers
{
    [Collection("Integration Collection")]
    [TestCaseOrderer("backend_tests.Setup.PriorityOrderer", "backend_tests.Setup")]
    public class ProductCategoryControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {

        //!Do not compare response's DTO's ids since they may be different from expected depending on the used provider

        private const string baseUrl = "/myc/api/categories";

        private HttpClient client;

        public ProductCategoryControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            client = fixture.httpClient;
        }

        [Fact, TestPriority(0)]
        public async Task ensureGetAllProductCategoriesReturnsNotFoundIfNoCategoriesHaveBeenAdded()
        {
            var response = await client.GetAsync(baseUrl);

            var bodyContent = response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact, TestPriority(1)]
        public async Task<ProductCategoryDTO> ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully()
        {  
            ProductCategoryDTO categoryDTO = new ProductCategoryDTO() { name = "Drawers"+Guid.NewGuid().ToString("n") };

            var response = await client.PostAsJsonAsync(baseUrl, categoryDTO);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return JsonConvert.DeserializeObject<ProductCategoryDTO>(await response.Content.ReadAsStringAsync());
        }


        [Fact, TestPriority(2)]
        public async Task ensureAddProductCategoryReturnsCreatedCategoryDTOInBody()
        {
            ProductCategoryDTO categoryDTO = new ProductCategoryDTO() { name = "Doors" };

            var response = await client.PostAsJsonAsync(baseUrl, categoryDTO);

            Task<ProductCategoryDTO> responseDTO = response.Content.ReadAsAsync<ProductCategoryDTO>();

            ProductCategoryDTO actual = responseDTO.Result;
            ProductCategoryDTO expected = new ProductCategoryDTO() { name = "Doors", parentId = null };

            Assert.Equal(expected.name, actual.name);
            Assert.Equal(expected.parentId, actual.parentId);
        }


        [Fact, TestPriority(3)]
        public async Task ensureAddProductCategoryReturnsBadRequestIfCategoryHasDuplicateName()
        {
            ProductCategoryDTO categoryDTO = new ProductCategoryDTO() { name = "Doors" };

            var response = await client.PostAsJsonAsync(baseUrl, categoryDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task ensureAddProductCategoryReturnsBadRequestIfCategoryDoesNotHaveName()
        {
            ProductCategoryDTO categoryDTO = new ProductCategoryDTO();

            var response = await client.PostAsJsonAsync(baseUrl, categoryDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact, TestPriority(5)]
        public async Task ensureFindProductCategoryReturnsOkIfCategoryExists()
        {
            var response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, 1));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(6)]
        public async Task ensureFindProductCategoryReturnsNotFoundIfCategoryDoesNotExist()
        {
            var response = await client.GetAsync(string.Format("{0}/{1}", baseUrl, 99));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, TestPriority(7)]
        public async Task ensureDeleteProductCategoryReturnsNotFoundIfCategoryDoesNotExist()
        {
            var response = await client.DeleteAsync(string.Format("{0}/{1}", baseUrl, 99));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}