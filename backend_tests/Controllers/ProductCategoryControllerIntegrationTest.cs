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


namespace backend_tests.Controllers{
    /// <summary>
    /// Integration Tests for ProductCategory Collection API
    /// </summary>
    public class ProductCategoryControllerIntegrationTest:IClassFixture<TestFixture<TestStartupSQLite>>{
        /// <summary>
        /// String with the URI where the API Requests will be performed
        /// </summary>
        private const string PRODUCT_CATEGORIES_URI="myc/api/categories";
        /// <summary>
        /// Current HTTP Client being used to perform API requests
        /// </summary>
        private HttpClient httpClient;
        /// <summary>
        /// Builds a new ProductCategoryControllerIntegrationTest with the mocked server injected by parameters
        /// </summary>
        /// <param name="fixture">Injected Mocked Server</param>
        public ProductCategoryControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture){
            this.httpClient=fixture.httpClient;
        }

        /// <summary>
        /// Ensures that a product category is created sucessfuly
        /// </summary>
        /// <returns>ProductCategoryDTO with the created category</returns>
        [Fact]
        public async Task<ProductCategoryDTO> ensureProductCategoryIsCreatedSucessfuly(){
            throw new NotImplementedException();
        }
    }

}