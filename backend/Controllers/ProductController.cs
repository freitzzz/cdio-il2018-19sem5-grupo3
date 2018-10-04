using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using core.application;
using core.domain;
using support.dto;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using support.utils;
using core.persistence;
using core.dto;
using backend.utils;

namespace backend.Controllers {

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("myc/products")]
    public class ProductController : Controller {
        /// <summary>
        /// Constant that represents the 400 Bad Request message for when a product
        /// is not found
        /// </summary>
        private const string PRODUCT_NOT_FOUND_REFERENCE = "Product not found";

        /// <summary>
        /// Constant that represents the 400 Bad Request message for when no products
        /// are found
        /// </summary>
        private const string NO_PRODUCTS_FOUND_REFERENCE = "No products found";

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository) {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// Finds a product by ID
        /// </summary>
        /// <param name="productID"> id of the product</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}")]
        public ActionResult<ProductDTO> findProductByID(long productID) {
            ProductDTO productDTO = new core.application.ProductController(productRepository, materialRepository).findProductByID(productID);
            if (productDTO == null) {
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }

            return Ok(productDTO);
        }




        /// <summary>
        /// Finds all products
        /// </summary>
        /// <returns>HTTP Response 400 Bad Request if no products are found;
        /// HTTP Response 200 Ok with the info of all products in JSON format </returns>
        [HttpGet]
        public ActionResult<List<ProductDTO>> findAll() {
            List<ProductDTO> allProductsDTO = new core.application.ProductController(productRepository, materialRepository).findAllProducts();

            if (allProductsDTO == null) {
                return BadRequest(NO_PRODUCTS_FOUND_REFERENCE);
            }

            return Ok(allProductsDTO);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="jsonData">JObject with the product information in JSON</param>
        /// <returns>HTTP Response 200 Ok if the product was created with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while creating the product
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpPost]
        public ActionResult<ProductDTO> addProduct([FromBody] ProductDTO jsonData) {

            ProductDTO productDTO = new core.application.ProductController(productRepository, materialRepository).addProduct(jsonData);

            if (productDTO != null) {
                return Ok(productDTO);
            } else {
                //TODO: INFORM BETTER BAD REQUESTES
                return BadRequest("{\"Message\":\"An error ocurred while creating the product\"}");
            }
        }

        /// <summary>
        /// Updates a product with new restrictions
        /// </summary>
        /// <param name="jsonData"> JObject that contains all updates in JSON format</param>
        /// <param name="productID"> Product's ID</param>
        /// <returns>HTTP Response 400 Bad Request if the product can't be found or if the
        /// JSON body is flawed;
        /// HTTP Response 200 Ok if the product is updated successfully</returns>
        /// TODO Refactor method
        [HttpPut("{id}")]
        public ActionResult updateProduct([FromBody] ProductDTO jsonData) {
            ProductDTO updatedProductDTO = new core.application.ProductController(productRepository, materialRepository).updateProduct(jsonData);
            if (updatedProductDTO == null) {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Updates the category of a product
        /// </summary>
        /// <param name="productID">Long with the product ID being updated</param>
        /// <param name="productCategoryDTO">ProductCategoryDTO with the category being updated on the product information</param>
        /// <returns>HTTP Response 200;OK if the product category was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product category
        /// </returns>
        [HttpPut("{id}/category")]
        public ActionResult<JSONMessageService> updateProductCategory(long productID,[FromBody]ProductCategoryDTO productCategoryDTO){
            ProductDTO productDTO=new ProductDTO();
            productDTO.id=productID;
            bool updatedWithSucess=new core.application.ProductController(productRepository,materialRepository).updateProductCategory(productDTO,productCategoryDTO);
            //TODO: Updates should throw an exception with the detailed error if it wasn't possible to update
            if(updatedWithSucess){
                return Ok(new JSONMessageService("Product category was updated with success"));
            }else{
                return BadRequest(new JSONMessageService("An error occured while updating the product category"));
            }
        }
        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="productID">Long with the product being disabled ID</param>
        /// <returns>HTTP Response 204;No Content if the product was disabled with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while disabling the product
        /// </returns>
        /// 
        [HttpDelete("{id}")]
        public ActionResult disableProduct(long productID){
            ProductDTO productDTO=new ProductDTO();
            productDTO.id=productID;
            bool disabledWithSuccess=new core.application.ProductController(productRepository,materialRepository).disableProduct(productDTO);
            if(disabledWithSuccess){
                return NoContent();
            }else{
                return BadRequest();
            }
        }

        [HttpPut("{id}/dimensions")]
        public ActionResult defineProductDimensions([FromBody] DimensionsListDTO dimensionsDTO, long productID){
            ProductDTO productDTO = new ProductDTO();
            productDTO.id = productID;
            productDTO.heightDimensions = dimensionsDTO.heightDimensionDTOs;
            productDTO.widthDimensions = dimensionsDTO.widthDimensionDTOs;
            productDTO.depthDimensions = dimensionsDTO.depthDimensionDTOs;
            bool dimensionsDefinedWithSuccess = new core.application.ProductController(productRepository,materialRepository).defineProductDimensions(productDTO);
            if(dimensionsDefinedWithSuccess){
                return Ok();
            }else{
                return BadRequest();
            }
        }

        /// <summary>
        /// ProductObject class to help the deserialization of a product's updates from JSON format
        /// </summary>
        public class DimensionsListDTO {
            public List<DimensionDTO> heightDimensionDTOs { get; set; }
            public List<DimensionDTO> widthDimensionDTOs {get;set;}
            public List<DimensionDTO> depthDimensionDTOs {get;set;}
        }

    }
}