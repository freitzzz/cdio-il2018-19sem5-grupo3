using System;
using Xunit;
using System.Net.Http;
using backend_tests.Setup;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend_tests.Controllers
{
    public class MaterialsControllerIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>{

        private readonly string urlBase = "/myc/materials";
        public HttpClient client {get;}

        public MaterialsControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture){
            client = fixture.httpClient;
        }

        [Fact]
        public async Task ensureFindAllMaterialsWorks(){

            var response = await client.GetAsync(urlBase);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Object>>(responseString);

            Assert.NotNull(result);
            Assert.IsType<List<Object>>(result);
        }
    }
}