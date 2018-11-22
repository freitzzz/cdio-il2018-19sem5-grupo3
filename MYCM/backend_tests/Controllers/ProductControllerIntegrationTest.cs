//TODO: Implement all these tests
// using backend_tests.Setup;
// using backend_tests.utils;
// using core.dto;
// using Newtonsoft.Json;
// using System;
// using System.Collections.Generic;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Xunit;
// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc.Testing;
// using core.modelview.productcategory;
// using core.modelview.product;
// using core.modelview.measurement;
// using core.modelview.dimension;
// using core.services;

// namespace backend_tests.Controllers{
    
//     /// <summary>
//     /// Integration Tests for Products Collection API
//     /// </summary>
//     [Collection("Integration Collection")]
//     [TestCaseOrderer(TestPriorityOrderer.TYPE_NAME, TestPriorityOrderer.ASSEMBLY_NAME)]
//     public sealed class ProductControllerIntegrationTest:IClassFixture<TestFixture<TestStartupSQLite>>{
//         /// <summary>
//         /// String with the URI where the API Requests will be performed
//         /// </summary>
//         private const string PRODUCTS_URI="mycm/api/products";
//         /// <summary>
//         /// Current HTTP Client being used to perform API requests
//         /// </summary>
//         private HttpClient httpClient;
//         /// <summary>
//         /// Injected Mock Server
//         /// </summary>
//         private TestFixture<TestStartupSQLite> fixture;
//         /// <summary>
//         /// Builds a new ProductControllerIntegrationTest with the mocked server injected by parameters
//         /// </summary>
//         /// <param name="fixture">Injected Mocked Server</param>
//         public ProductControllerIntegrationTest(TestFixture<TestStartupSQLite> fixture)
//         {
//             this.fixture = fixture;
//             this.httpClient = fixture.CreateClient(new WebApplicationFactoryClientOptions
//             {
//                 AllowAutoRedirect = false,
//                 BaseAddress =  new Uri("http://localhost:5001")
//                 });
//             }

//         /// <summary>
//         /// Ensures that the product collection fetch is empty
//         /// </summary>
//         [Fact,TestPriority(0)]
//         public async void ensureEmptyProductCollectionReturnsNotFound(){
//             //If we haven't created any products yet, the product collection 
//             //fetch should be empty
//             var fetchProductCollection = await httpClient.GetAsync(PRODUCTS_URI);
//             //We performed a GET of the products collection
//             //Since there are no products available
//             //The response status code should be a Not Found
//             //Since there aren't any products available
//             Assert.Equal(HttpStatusCode.NotFound, fetchProductCollection.StatusCode);
//         }

//         /// <summary>
//         /// Ensures that a product is created succesfuly
//         /// </summary>
//         /// <returns>ProductDTO with the created product</returns>
//         [Fact, TestPriority(1)]
//         public async Task<GetProductModelView> ensureProductIsCreatedSuccesfuly(){
//             //We are going to create a valid product
//             //A valid product creation requires a valid reference, a valid desgination
//             //A valid category, valid dimensions and valid materials
//             //Components are not required
//             //Materials must previously exist as they can be shared in various products
//             AddProductModelView addProductMV = createAddProductModelView();
//             GetProductCategoryModelView categoryModelView = await new ProductCategoryControllerIntegrationTest(fixture)
//                 .ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
//             MaterialDTO materialDTO = await new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();

//             //NOTE: ModelView is being converted to DTO for now
//             //TODO: update material controller and tests so that ModelViews are used

//             AddMaterialToProductModelView addMaterialMV = new AddMaterialToProductModelView();
//             addMaterialMV.materialID = materialDTO.id;

//             addProductMV.materials = new List<AddMaterialToProductModelView>() {addMaterialMV};
//             addProductMV.categoryId = categoryModelView.id;

//             var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI, addProductMV);
//             Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//             GetProductModelView getProductMV = JsonConvert.DeserializeObject<GetProductModelView>(await response.Content.ReadAsStringAsync());


//             Assert.Equal(addProductMV.designation, getProductMV.designation);
//             Assert.Equal(addProductMV.reference, getProductMV.reference);
//             //TODO Fix Slot Asserts
//             /* Assert.Equal(addProductMV.slotSizes.maxSize.height, getProductMV.slotSizes.maxSize.height);
//             Assert.Equal(addProductMV.slotSizes.maxSize.width, getProductMV.slotSizes.maxSize.width);
//             Assert.Equal(addProductMV.slotSizes.maxSize.depth, getProductMV.slotSizes.maxSize.depth); */

            
//             return JsonConvert.DeserializeObject<GetProductModelView>(await response.Content.ReadAsStringAsync());
//         }



//         /// <summary>
//         /// Ensures that the product collection fetch isnt empty when previously added a product
//         /// </summary>
//         [Fact,TestPriority(2)]
//         public async void ensureProductCollectionFetchIsNotEmpty(){
//             // Since we have created a product, the product collection fetch should not be empty
//             var response = await httpClient.GetAsync(PRODUCTS_URI);
            
//             string responseContent = await response.Content.ReadAsStringAsync();

//             GetAllProductsModelView products = JsonConvert.DeserializeObject<GetAllProductsModelView>(responseContent);

//             Assert.True(response.StatusCode==HttpStatusCode.OK);
//             Assert.NotEmpty(products);
//         }

//         // / <summary>
//         // / Ensures that the fetch of a product resource is invalid
//         // / </summary>
//         [Fact,TestPriority(3)]
//         public async void ensureProductResourceFetchIsInvalid(){
//             // If we haven't created any products yet, the product collection 
//             // fetch should be empty
//             long invalidResourceID=0;
//             var fetchProduct=await httpClient.GetAsync(PRODUCTS_URI+"/"+invalidResourceID);
//             // We performed a GET of a certain product by its resource
//             // Since the resource doesn't exist, the fetch should be invalid
//             // And the response status code should be a Not Found
//             Assert.True(fetchProduct.StatusCode==HttpStatusCode.BadRequest);
//         }

// //         / <summary>
// //         / Ensures that the fetch of a product resource is valid
// //         / </summary>
// //         [Fact,TestPriority(3)]
// //         public async void ensureProductResourceFetchIsValid(){
// //             Task<ProductDTO> createdProductDTORequest=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTORequest.Wait();
// //             ProductDTO createdProductDTO=createdProductDTORequest.Result;
// //             We just performed a POST request to create a new product
// //             We deserialized the response, and it has a resource ID
// //             By performing a GET request on the resource, the product fetched
// //             Should be the same one as the created one
// //             var fetchProduct=await httpClient.GetAsync(PRODUCTS_URI+"/"+createdProductDTO.id);
// //             ProductDTO fetchedProductDTO=JsonConvert.DeserializeObject<ProductDTO>(await fetchProduct.Content.ReadAsStringAsync());
// //             Assert.True(fetchProduct.StatusCode==HttpStatusCode.OK);
// //             Assert.Equal(createdProductDTO.id,fetchedProductDTO.id);
// //             Assert.Equal(createdProductDTO.reference,fetchedProductDTO.reference);
// //         }

// //         / <summary>
// //         / Ensures that a product reference cant be updated if the reference is invalid
// //         / </summary>
// //         [Fact,TestPriority(4)]
// //         public async void ensureProductReferenceCantBeUpdatedIfInvalid(){
// //              We're are going to created two different products, X & Y
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             If we try to update it to a reference that is invalid, then it should fail
// //             createdProductDTOX.Wait();
// //             UpdateProductModelView updatedProductX=new UpdateProductModelView();
// //             var updateReference=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateReference.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product reference cant be updated if the reference is duplicated
// //         / </summary>
// //         [Fact,TestPriority(5)]
// //         public async void ensureProductReferenceCantBeUpdatedIfDuplicated(){
// //             We're are going to created two different products, X & Y
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             Task<ProductDTO> createdProductDTOY=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOY.Wait();
// //             Product X has different reference to Y
// //             Since the reference of a product is the identity of a product
// //             If we try to update it to a reference that already exists, then it should fail
// //             ProductDTO productDTOX=createdProductDTOX.Result;
// //             ProductDTO productDTOY=createdProductDTOY.Result;
// //             UpdateProductModelView updatedProductY=new UpdateProductModelView();
// //             updatedProductY.reference=productDTOX.reference;
// //             DONT UNCOMMENT UNTIL INTEGRATION DATABASE PROBLEMS IS SOLVED
// //             PROBLEM HERE IS THAT SINCE WE ARE USING AN IN MEMORY DATABASE PROVIDER
// //             IT LITERALLY IS IN MEMORY SO IF WE CHANGE AN OBJECT WIHTOUT UPDATING IT
// //             WITH THE RESPECTIVE REPOSITORY, ITS STILL AN UPDATE SINCE OBJECTS HAVE THE SAME MEMORY REFERENCE
// //             :(
// //             /* var updateReference=await httpClient.PutAsync(PRODUCTS_URI+"/"+productDTOY.id
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductY));
// //             Assert.True(updateReference.StatusCode==HttpStatusCode.BadRequest); */
// //         }
        
// //         / <summary>
// //         / Ensures that a product designation cant be updated if the designation is invalid
// //         / </summary>
// //         [Fact,TestPriority(6)]
// //         public async void ensureProductDesignationCantBeUpdatedIfInvalid(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             If we try to update it to a designation that is invalid, then it should fail
// //             createdProductDTOX.Wait();
// //             UpdateProductModelView updatedProductX=new UpdateProductModelView();
// //             var updateDesignation=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateDesignation.StatusCode==HttpStatusCode.BadRequest);
// //         }

// // /*         /// <summary>
// //         / Ensures that a product materials cant be updated if the materials are invalid
// //         / </summary>
// //         [Fact,TestPriority(7)]
// //         public async void ensureProductMaterialsCantBeUpdatedIfInvalid(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductModelView updatedProductX=new UpdateProductModelView();
// //             var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/materials"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
// //         } */

// // /*         /// <summary>
// //         / Ensures that a product materials cant be added if the materials are duplicated
// //         / </summary>
// //         [Fact,TestPriority(8)]
// //         public async void ensureProductMaterialsCantBeAddedIfDuplicated(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductModelView updatedProductX=new UpdateProductModelView();
// //             updatedProductX.materialsToAdd=new List<MaterialDTO>(createdProductDTOX.Result.productMaterials);
// //             var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/materials"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
// //         } */

// // /*         /// <summary>
// //         / Ensures that a product materials cant be added if the materials don't exist/are not found
// //         / </summary>
// //         [Fact,TestPriority(9)]
// //         public async void ensureProductMaterialsCantBeAddedIfNotFound(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             updatedProductX.materialsToAdd=new List<MaterialDTO>(new []{new MaterialDTO()});
// //             var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/materials"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
// //         } */

// // /*         /// <summary>
// //         / Ensures that a product materials cant be removed if the materials don't exist/are not found
// //         / </summary>
// //         [Fact,TestPriority(10)]
// //         public async void ensureProductMaterialsCantBeRemovedIfNotFound(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             updatedProductX.materialsToRemove=new List<MaterialDTO>(new []{new MaterialDTO()});
// //             var updateProduct=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/materials"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));
// //             Assert.True(updateProduct.StatusCode==HttpStatusCode.BadRequest);
// //         } */

// //         TODO: TEST ADD/REMOVE MEASUREMENTS

// // /*         /// <summary>
// //         / Ensures that a product category can't be updated if invalid (null/empty category)
// //         / </summary>
// //         [Fact,TestPriority(15)]
// //         public async void ensureCantUpdateTheCategoryOfAProductIfInvalid(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             Our category will be an "empty" category
// //             updatedProductX.productCategoryToUpdate=new ProductCategoryDTO();
// //             var updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/category"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);
// //         } */

// // /*         /// <summary>
// //         / Ensures that a product category can't be updated if the category is nonexisting
// //         / </summary>
// //         [Fact,TestPriority(16)]
// //         public async void ensureCantUpdateTheCategoryOfAProductIfNonExisting(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             Our category will be a non existing category (still not persisted)
// //             ProductCategoryDTO productCategoryDTO=new ProductCategoryDTO();
// //             productCategoryDTO.id=0;
// //             updatedProductX.productCategoryToUpdate=productCategoryDTO;
            
// //             var updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/category"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);
// //         } */

// // /*         /// <summary>
// //         / Ensures that its not possible to add an invalid component (null / empty) to a product
// //         / </summary>
// //         [Fact,TestPriority(17)]
// //         public async void ensureCantAddInvalidComponentsToProduct(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             First lets test to add components to the product with no components (empty list)
// //             updatedProductX.componentsToAdd=new List<ComponentDTO>();
            
// //             var updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);

// //             Now lets add an empty component (empty body)
// //             updatedProductX.componentsToAdd=new List<ComponentDTO>(new []{new ComponentDTO()});
// //             updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);
// //         } */

// // /*         /// <summary>
// //         / Ensures that its not possible to add a duplicated component (null / empty) to a product
// //         / </summary>
// //         [Fact,TestPriority(18)]
// //         public async void ensureCantAddDuplicatedComponentsToProduct(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             First lets add a valid component to the product
// //             Task<ProductDTO> complementedProductDTOTask=ensureProductIsCreatedSuccesfuly();
// //             complementedProductDTOTask.Wait();
// //             ComponentDTO componentDTO=new ComponentDTO();
// //             componentDTO.product=complementedProductDTOTask.Result;
// //             updatedProductX.componentsToAdd=new List<ComponentDTO>(new []{componentDTO});
            
// //             var updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.OK,updateProductX.StatusCode);

// //             Now lets send the same request so we try to add the same component
// //             updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);
// //         } */
        
// // /*         /// <summary>
// //         / Ensures that its not possible to remove an invalid component (null / empty) to a product
// //         / </summary>
// //         [Fact,TestPriority(19)]
// //         public async void ensureCantRemoveInvalidComponentsToProduct(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             UpdateProductDTO updatedProductX=new UpdateProductDTO();
// //             First lets test to add components to the product with no components (empty list)
// //             updatedProductX.componentsToRemove=new List<ComponentDTO>();
            
// //             var updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);

// //             Now lets add an empty component (empty body)
// //             updatedProductX.componentsToRemove=new List<ComponentDTO>(new []{new ComponentDTO()});
// //             updateProductX=await httpClient.PutAsync(PRODUCTS_URI+"/"+createdProductDTOX
// //                                                                             .Result.id
// //                                                                         +"/components"
// //                                         ,HTTPContentCreator.contentAsJSON(updatedProductX));

// //             Assert.Equal(HttpStatusCode.BadRequest,updateProductX.StatusCode);
// //         } */

// //         / <summary>
// //         / Ensures that a product cant be disabled if it doesn't exist (resource doesn't exist)
// //         / </summary>
// //         [Fact,TestPriority(20)]
// //         public async void ensureCantDisableAProductWhichDoesntExist(){
// //             var disableProduct=await httpClient.DeleteAsync(PRODUCTS_URI+"/"+"0");
// //             Assert.Equal(HttpStatusCode.BadRequest,disableProduct.StatusCode);
// //         }

// //         / <summary>
// //         / Ensures that a product cant be disabled if its already disabled
// //         / </summary>
// //         [Fact,TestPriority(21)]
// //         public async void ensureCantDisableAProductWhichIsAlreadyDisabled(){
// //             We need to create a product for the test
// //             Task<ProductDTO> createdProductDTOX=ensureProductIsCreatedSuccesfuly();
// //             createdProductDTOX.Wait();
// //             First lets disable the product we just created
// //             var disableProduct=await httpClient.DeleteAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id);
// //             Assert.Equal(HttpStatusCode.NoContent,disableProduct.StatusCode);

// //             Now lets try to disable the product we just disabled
// //             disableProduct=await httpClient.DeleteAsync(PRODUCTS_URI+"/"+createdProductDTOX.Result.id);
// //             Assert.Equal(HttpStatusCode.BadRequest,disableProduct.StatusCode);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if the request body is empty
// //         / </summary>
// //         [Fact, TestPriority(22)]
// //         public async void ensureProductCantBeCreatedWithEmptyRequestBody(){
// //             We are attempting to create an object with an empty request body
// //             var createProductEmptyRequestBody=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON("{}"));
// //             Since we performed a request to create a product with empty request body
// //             Then the response should be a Bad Request
// //             Assert.True(createProductEmptyRequestBody.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if it has no reference
// //         / </summary>
// //         [Fact, TestPriority(23)]
// //         public async void ensureProductCantBeCreatedWithNoReference(){
// //             We are attempting to created a product with no referene
// //             ProductDTO productDTO=new ProductDTO();
// //             productDTO.designation="Valid Designation";
// //             var createProductNoReference=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
// //             Since there was an attempt to create a product with no reference
// //             Then the response should be a Bad Request
// //             Assert.True(createProductNoReference.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if it has no designation
// //         / </summary>
// //         [Fact, TestPriority(24)]
// //         public async void ensureProductCantBeCreatedWithNoDesignation(){
// //             We are attempting to created a product with no designation
// //             ProductDTO productDTO=new ProductDTO();
// //             productDTO.reference="Valid Reference";
// //             var createProductNoDesignation=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
// //             Since there was an attempt to create a product with no designation
// //             Then the response should be a Bad Request
// //             Assert.True(createProductNoDesignation.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if it has no category
// //         / </summary>
// //         [Fact, TestPriority(25)]
// //         public async void ensureProductCantBeCreatedWithNoCategory(){
// //             We are attempting to created a product with no category
// //             ProductDTO productDTO=new ProductDTO();
// //             productDTO.reference="Valid Reference";
// //             productDTO.designation="Valid Designation";
// //             var createProductNoCategory=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
// //             Since there was an attempt to create a product with no category
// //             Then the response should be a Bad Request
// //             Assert.True(createProductNoCategory.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if it has no materials
// //         / </summary>
// //         [Fact, TestPriority(26)]
// //         public async void ensureProductCantBeCreatedWithNoMaterials(){
// //             We are attempting to created a product with no materials
// //             ProductDTO productDTO=new ProductDTO();
// //             productDTO.reference="Valid Reference";
// //             productDTO.designation="Valid Designation";
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
            
// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             productDTO.productCategory=new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name}; 

// //             var createProductNoMaterials=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
// //             Since there was an attempt to create a product with no materials
// //             Then the response should be a Bad Request
// //             Assert.True(createProductNoMaterials.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product can't be created if it has no dimensions
// //         / </summary>
// //         [Fact, TestPriority(27)]
// //         public async void ensureProductCantBeCreatedWithNoDimensions(){
// //             We are attempting to created a product with no dimensions
// //             ProductDTO productDTO=new ProductDTO();
// //             productDTO.reference="Valid Reference";
// //             productDTO.designation="Valid Designation";
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();

// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;

// //             productDTO.productCategory=new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name}; 
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});
// //             var createProductNoDimensions=await httpClient.PostAsync(PRODUCTS_URI,HTTPContentCreator.contentAsJSON(productDTO));
// //             Since there was an attempt to create a product with no dimensions
// //             Then the response should be a Bad Request
// //             Assert.True(createProductNoDimensions.StatusCode==HttpStatusCode.BadRequest);
// //         }

// //         / <summary>
// //         / Ensures that a product cant be created with invalid components
// //         / </summary>
// //         [Fact,TestPriority(28)]
// //         public async void ensureProductCantBeCreatedWithInvalidComponents(){
// //             ProductDTO productDTO=createAddProductModelView();
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});
            
// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             productDTO.productCategory=new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name}; 

// //             Our invalid component is a "blank" component
// //             ComponentDTO componentDTO=new ComponentDTO();
// //             productDTO.complements=new List<ComponentDTO>();
// //             var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productDTO);
// //             Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
// //         }

// //         / <summary>
// //         / Ensures that a product cant be created with invalid slots (null)
// //         / </summary>
// //         [Fact,TestPriority(29)]
// //         public async void ensureProductCantBeCreatedWithInvalidSlots(){
// //             ProductDTO productDTO=createAddProductModelView();
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});

// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             productDTO.productCategory=new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name}; 
            
// //             Our invalid slots are "empty" slots
// //             productDTO.slotDimensions=new SlotDimensionSetDTO();
// //             var response = await httpClient.PostAsJsonAsync(PRODUCTS_URI, productDTO);
// //             Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
// //         }

// //         / <summary>
// //         / Ensures that a product can be created with componets
// //         / </summary>
// //         [Fact,TestPriority(31)]
// //         public async Task<ProductDTO> ensureProductWithComponentsIsCreatedSuccesfuly(){
// //             To save time lets just create a new product which will serve as the aggregate
// //             ProductDTO productDTO=createAddProductModelView();
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});
            
// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             productDTO.productCategory=new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name}; 
            
// //             Lets now adds the components to the product
// //             We are going to create a product which will serve as complemented product for a product (component)
// //             Task<ProductDTO> complementedProductDTOTask=ensureProductIsCreatedSuccesfuly();
// //             complementedProductDTOTask.Wait();
// //             ComponentDTO componentDTO=new ComponentDTO();
// //             componentDTO.product=complementedProductDTOTask.Result;
// //             productDTO.complements=new List<ComponentDTO>(new []{componentDTO});
// //             var response=await httpClient.PostAsJsonAsync(PRODUCTS_URI,productDTO);
// //             Assert.Equal(HttpStatusCode.Created,response.StatusCode);
// //             return JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
// //         }

// //         / <summary>
// //         / Ensures that a product can be created with slots
// //         / </summary>
// //         [Fact,TestPriority(32)]
// //         public async Task<ProductDTO> ensureProductWithSlotsIsCreatedSuccesfuly(){
// //             ProductDTO productDTO=createAddProductModelView();
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});

// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             productDTO.productCategory= new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name};
            
// //             Now lets create valid slot dimensions and add it to the product
// //             SlotDimensionSetDTO slotDimensionSetDTO=new SlotDimensionSetDTO();
// //             CustomizedDimensionsDTO customizedDimensionsDTO=new CustomizedDimensionsDTO();
// //             customizedDimensionsDTO.width=10;
// //             customizedDimensionsDTO.height=10;
// //             customizedDimensionsDTO.depth=10;
// //             slotDimensionSetDTO.recommendedSlotDimensions=customizedDimensionsDTO;
// //             slotDimensionSetDTO.minimumSlotDimensions=customizedDimensionsDTO;
// //             slotDimensionSetDTO.maximumSlotDimensions=customizedDimensionsDTO;
// //             productDTO.slotDimensions=slotDimensionSetDTO;
// //              var response=await httpClient.PostAsJsonAsync(PRODUCTS_URI,productDTO);
// //             Assert.Equal(HttpStatusCode.Created,response.StatusCode);
// //             return JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
// //         }

// //         / <summary>
// //         / Ensures that a product can be created with slots and components
// //         / </summary>
// //         [Fact,TestPriority(33)]
// //         public async Task<ProductDTO> ensureProductWithSlotAndComponentsIsCreatedSuccesfuly(){
// //             ProductDTO productDTO=createAddProductModelView();
// //             Task<GetProductCategoryModelView> categoryMVTask=new ProductCategoryControllerIntegrationTest(fixture).ensureAddProductCategoryReturnsCreatedIfCategoryWasAddedSuccessfully();
// //             categoryMVTask.Wait();
// //             Materials must previously exist as they can be shared in various products
// //             Task<MaterialDTO> materialDTO=new MaterialsControllerIntegrationTest(fixture).ensurePostMaterialWorks();
// //             materialDTO.Wait();
// //             productDTO.productMaterials=new List<MaterialDTO>(new[]{materialDTO.Result});

// //             NOTE: ModelView is being converted to DTO for now
// //             GetProductCategoryModelView categoryModelView = categoryMVTask.Result;
// //             ProductCategoryDTO categoryDTO = new ProductCategoryDTO(){id = categoryModelView.id, name = categoryModelView.name};

// //             productDTO.productCategory=categoryDTO;
// //             Now lets create valid slot dimensions and add it to the product
// //             SlotDimensionSetDTO slotDimensionSetDTO=new SlotDimensionSetDTO();
// //             CustomizedDimensionsDTO customizedDimensionsDTO=new CustomizedDimensionsDTO();
// //             customizedDimensionsDTO.width=10;
// //             customizedDimensionsDTO.height=10;
// //             customizedDimensionsDTO.depth=10;
// //             slotDimensionSetDTO.recommendedSlotDimensions=customizedDimensionsDTO;
// //             slotDimensionSetDTO.minimumSlotDimensions=customizedDimensionsDTO;
// //             slotDimensionSetDTO.maximumSlotDimensions=customizedDimensionsDTO;
// //             productDTO.slotDimensions=slotDimensionSetDTO;
// //             Lets now adds the components to the product
// //             We are going to create a product which will serve as complemented product for a product (component)
// //             Task<ProductDTO> complementedProductDTOTask=ensureProductIsCreatedSuccesfuly();
// //             complementedProductDTOTask.Wait();
// //             ComponentDTO componentDTO=new ComponentDTO();
// //             componentDTO.product=complementedProductDTOTask.Result;
// //             productDTO.complements=new List<ComponentDTO>(new []{componentDTO});
// //              var response=await httpClient.PostAsJsonAsync(PRODUCTS_URI,productDTO);
// //             Assert.Equal(HttpStatusCode.Created,response.StatusCode);
// //             return JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
// //         }

//         /// <summary>
//         /// Creates a product with valid properties (reference, designation and dimensions)
//         /// </summary>
//         /// <returns>ProductDTO with the product with valid properties</returns>
//         private AddProductModelView createAddProductModelView(){
//             //To ensure atomicity, our reference will be generated with a timestamp (We have no bussiness rules so far as how they should be so its valid at this point)
//             string reference="#666"+Guid.NewGuid().ToString("n");
//             //Designation can be whatever we decide
//             string designation="Time N Place";
            
//             AddDiscreteDimensionIntervalModelView addDiscreteMV = new AddDiscreteDimensionIntervalModelView();
//             addDiscreteMV.values = new List<double>(){1.0,2.0,30.0};
//             addDiscreteMV.unit = MeasurementUnitService.getMinimumUnit();

//             AddContinuousDimensionIntervalModelView addContinuousMV = new AddContinuousDimensionIntervalModelView();
//             addContinuousMV.minValue = 10;
//             addContinuousMV.maxValue = 100;
//             addContinuousMV.increment = 1;
//             addContinuousMV.unit = MeasurementUnitService.getMinimumUnit();

//             AddSingleValueDimensionModelView addSingleMV = new AddSingleValueDimensionModelView();
//             addSingleMV.value = 50;
//             addSingleMV.unit = MeasurementUnitService.getMinimumUnit();
            
//             AddMeasurementModelView addMeasurementMV = new AddMeasurementModelView();
//             addMeasurementMV.heightDimension = addContinuousMV;
//             addMeasurementMV.widthDimension = addSingleMV;
//             addMeasurementMV.depthDimension = addDiscreteMV;

//             AddProductModelView addProductMV=new AddProductModelView();
//             addProductMV.reference=reference;
//             addProductMV.designation=designation;
//             addProductMV.measurements = new List<AddMeasurementModelView>(){addMeasurementMV};

//             return addProductMV;
//         }
//     }
// }