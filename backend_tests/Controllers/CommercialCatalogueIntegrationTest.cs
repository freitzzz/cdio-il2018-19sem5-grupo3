using System;
using Xunit;
using System.Net.Http;
using backend_tests.Setup;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using backend_tests.utils;
using core.dto;
using core.domain;

namespace backend_tests.Controllers
{
    /// <summary>
    /// Integration Tests for Commercial Catalogue API
    /// </summary>
    /// <typeparam name="TestStartupSQLite">class that handles database startup</typeparam>
    [Collection("Integration Collection")]
    [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
    public class CommercialCatalogueIntegrationTest : IClassFixture<TestFixture<TestStartupSQLite>>
    {
        /// <summary>
        /// Materials URI for HTTP Requests
        /// </summary>
        private const string urlBase = "/myc/api/commercialcatalogues";

        /// <summary>
        /// HTTP Client to perform HTTP Requests to the test server
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Injected Mock Server
        /// </summary>
        private TestFixture<TestStartupSQLite> fixture;

        /// <summary>
        /// Builds a CommercialCatalogueIntegrationTest instance with an injected mocked server
        /// </summary>
        /// <param name="fixture">injected mocked server</param>
        public CommercialCatalogueIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        {
            client = fixture.httpClient;
            this.fixture = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task ensurePostCommercialCatalogueFailsWithEmptyRequestBody()
        {
            var response = await client.PostAsJsonAsync(urlBase, "{}");
            Console.WriteLine(response.StatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

//      [Fact, TestPriority(2)]
//         public async Task<CommercialCatalogueDTO> ensurePostCommercialCatalogueWorks()
//         {

//             ProductCategoryDTO categoryDTO = new ProductCategoryDTO();
//             categoryDTO.name = "Category";

//             ColorDTO colorDTO = new ColorDTO();
//             colorDTO.name = "Bule";
//             colorDTO.alpha = 1;
//             colorDTO.blue = 1;
//             colorDTO.green = 1;
//             colorDTO.red = 1;
//             List<ColorDTO> colorsDTO = new List<ColorDTO>();
//             colorsDTO.Add(colorDTO);
//             FinishDTO finishDTO = new FinishDTO();
//             finishDTO.description = "Finish";
//             List<FinishDTO> finishesDTO = new List<FinishDTO>();
//             finishesDTO.Add(finishDTO);
//             MaterialDTO materialDTO = new MaterialDTO();
//             materialDTO.reference = "1";
//             materialDTO.designation = "Material";
//             materialDTO.finishes = finishesDTO;
//             materialDTO.colors = colorsDTO;

//             List<MaterialDTO> materialsDTO = new List<MaterialDTO>();
//             materialsDTO.Add(materialDTO);
//             IEnumerable<MaterialDTO> materialsIEnum = materialsDTO;

//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionIntervalDTO d2 = new DiscreteDimensionIntervalDTO();
//             d2.values = values2;
//             List<DimensionDTO> valuest = new List<DimensionDTO>();
//             valuest.Add(d2);

//             IEnumerable<DimensionDTO> heightDimensions = valuest;
//             IEnumerable<DimensionDTO> widthDimensions = valuest;
//             IEnumerable<DimensionDTO> depthDimensions = valuest;
//             ProductDTO productDTO = new ProductDTO();
//             productDTO.designation = "Product";
//             productDTO.reference = "4";
//             productDTO.productMaterials = materialsDTO;
//             productDTO.productCategory = categoryDTO;
//             productDTO.dimensions =

//             ("2", "Product", category, materialsIEnum, heightDimensions, widthDimensions, depthDimensions);

//             CustomizedMaterialDTO custMaterialDTO = new CustomizedMaterialDTO();
//             custMaterialDTO.color = colorDTO;
//             custMaterialDTO.finish = finishDTO;

//             CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(23.4, 4.5, 6.0);


//             CustomizedProduct custProduct = new CustomizedProduct("3", "Customized Product", custMaterial, custDimensions, product);
//             List<CustomizedProduct> custProducts = new List<CustomizedProduct>();
//             custProducts.Add(custProduct);

//             CustomizedProductCollection productsCollection = new CustomizedProductCollection("CustomizedProductsCollection", custProducts);

//             CatalogueCollection catalogueCollection = new CatalogueCollection(custProducts, productsCollection);

//             List<CatalogueCollection> listCatalogueCollection = new List<CatalogueCollection>();
//             listCatalogueCollection.Add(catalogueCollection);

//             CommercialCatalogue commercialCatalogue = new CommercialCatalogue("6", "Catalogue", listCatalogueCollection);
//             CommercialCatalogueDTO commercialCatalogueDTO = commercialCatalogue.toDTO();
// S            var response = await client.PostAsJsonAsync(urlBase, commercialCatalogueDTO);

//             Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//             return JsonConvert.DeserializeObject<CommercialCatalogueDTO>(await response.Content.ReadAsStringAsync());
//         }


    }
}