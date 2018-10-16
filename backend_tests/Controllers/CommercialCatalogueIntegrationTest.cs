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
    [TestCaseOrderer("backend_tests.Setup.TestPriorityOrderer","backend_tests")]
    public class CommercialCatalogueIntegrationTest: IClassFixture<TestFixture<TestStartupSQLite>>
    {
        // /// <summary>
        // /// Materials URI for HTTP Requests
        // /// </summary>
        // private const string urlBase = "/myc/api/commercialcatalogue";

        // /// <summary>
        // /// HTTP Client to perform HTTP Requests to the test server
        // /// </summary>
        // private HttpClient client;
        
        // /// <summary>
        // /// Injected Mock Server
        // /// </summary>
        // private TestFixture<TestStartupSQLite> fixture;

        // /// <summary>
        // /// Builds a CommercialCatalogueIntegrationTest instance with an injected mocked server
        // /// </summary>
        // /// <param name="fixture">injected mocked server</param>
        // public CommercialCatalogueIntegrationTest(TestFixture<TestStartupSQLite> fixture)
        // {
        //     client = fixture.httpClient;
        //     this.fixture = fixture;
        // }

        //  /// <summary>
        // /// Ensures that a product is created succesfuly
        // /// </summary>
        // /// <returns>ProductDTO with the created product</returns>
        // public async Task<CommercialCatalogueDTO> ensureCommercialCatalogueIsSuccesfullyCreated(){
        //     //We are going to create a valid CommercialCatalogue
        //     //A valid product creation requires a valid reference, a valid desgination
        //     //A valid category, valid dimensions and valid materials
        //     //Components are not required
        //     //To ensure atomicity, our reference will be generated with a timestamp (We have no bussiness rules so far as how they should be so its valid at this point)
        //     string reference="#666"+DateTime.Now;
        //     //Designation can be whatever we decide
        //     string designation="Time N Place";
        //     List<CustomizedProductCollectionDTO> list = new List<CustomizedProductCollectionDTO>();
        //     CustomizedProductCollectionDTO customizedProductCollectionDTO = new CustomizedProductCollectionDTO(reference,designation,listCu);

        //     List<CustomizedProductDTO> listCustom = new List<CustomizedProductDTO>();
        //     Color color = Color.valueOf("Azul", 1, 1, 1, 1);
        //     Finish finish = Finish.valueOf("Acabamento polido");
        //     CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

        //     CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

        //     ProductCategory prodCat = new ProductCategory("Category 1");
        //     List<Double> values2 = new List<Double>();
        //     values2.Add(500.0); //Width
        //     DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
        //     List<Dimension> valuest = new List<Dimension>();
        //     valuest.Add(d2);
        //     IEnumerable<Dimension> heightDimensions = valuest;
        //     IEnumerable<Dimension> widthDimensions = valuest;
        //     IEnumerable<Dimension> depthDimensions = valuest;
        //     List<Color> colors = new List<Color>();
        //     colors.Add(color);

        //     List<Finish> finishes = new List<Finish>();
        //     finishes.Add(finish);

        //     Material material = new Material("1234", "Material", colors, finishes);
        //     List<Material> listMaterial = new List<Material>();
        //     listMaterial.Add(material);
        //     IEnumerable<Material> materials = listMaterial;
        //     Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

        //     CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

        //     listCustom.Add(custProduct.toDTO());
        //     //CustomizedProductCollection must previously exist as they can be shared in various products
            
        //     //TODO fix return statementreturn httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
        //     return null;
        // }


    }
}