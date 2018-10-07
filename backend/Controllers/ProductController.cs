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
using System.Runtime.Serialization;

namespace backend.Controllers {

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("myc/api/products")]
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

        /// <summary>
        /// Constant that represents the message that ocurres if the update of a product fails
        /// </summary>
        private const string INVALID_PRODUCT_UPDATE_MESSAGE="An error occured while updating the product";

        /// <summary>
        /// Constant that represents the message that ocurres if the update of a product is successful
        /// </summary>
        private const string VALID_PRODUCT_UPDATE_MESSAGE="Product was updated with success";
        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE="The request body is invalid\nCheck documentation for more information";

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository) {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// Finds all products
        /// </summary>
        /// <returns>HTTP Response 400 Bad Request if no products are found;
        /// HTTP Response 200 Ok with the info of all products in JSON format </returns>
        [HttpGet]
        public ActionResult<List<ProductDTO>> findAll() {
            List<ProductDTO> allProductsDTO = new core.application.ProductController().findAllProducts();

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
        public ActionResult<ProductDTO> addProduct([FromBody]ProductDTO productData){
            try{
                ProductDTO createdProductDTO=new core.application.ProductController().addProduct(productData);
                if(createdProductDTO!=null){
                    return Created(Request.Path,createdProductDTO);
                }else{
                    return BadRequest();
                }
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(ArgumentException){
                throw new NotImplementedException();
                //Treat Product Creation exception
            }
        }

        /// <summary>
        /// Finds a product by ID
        /// </summary>
        /// <param name="id"> id of the product</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}",Name="GetProduct")]
        public ActionResult<ProductDTO> findById(long id) {
            ProductDTO productDTOX=new ProductDTO();
            productDTOX.id=id;
            ProductDTO productDTOY = new core.application.ProductController().findProductByID(productDTOX);
            if (productDTOY == null) {
                return BadRequest(PRODUCT_NOT_FOUND_REFERENCE);
            }
            return Ok(productDTOY);
        }

        /// <summary>
        /// Updates a product basic information
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the basic information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}")]
        public ActionResult updateProductBasicInformation(long id,[FromBody] UpdateProductDTO updateProductData) {
            updateProductData.id=id;
            if(new core.application.ProductController().updateProductBasicInformation(updateProductData))
                return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Updates the materials which a product can be made of
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/materials")]
        public ActionResult updateProductMaterials(long id,[FromBody] UpdateProductDTO updateProductData) {
            updateProductData.id=id;
            try{
                if(new core.application.ProductController().updateProductMaterials(updateProductData))
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            }catch(NullReferenceException){
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(new SimpleJSONMessageService(invalidOperationException.Message));
            }
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Updates the components which a product can be complemented with
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/components")]
        public ActionResult updateProductComponents(long id,[FromBody] UpdateProductDTO updateProductData) {
            updateProductData.id=id;
            try{
                if(new core.application.ProductController().updateProductComponents(updateProductData))
                    return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/dimensions")]
        public ActionResult updateProductDimensions(long id,[FromBody] UpdateProductDTO updateProductData) {
            updateProductData.id=id;
            if(new core.application.ProductController().updateProductDimensions(updateProductData))
                return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductData">UpdateProductDTO with the information of the product being updated</param>
        /// <returns>HTTP Response 200;OK if the product was updated with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while updating the product
        /// </returns>
        [HttpPut("{id}/category")]
        public ActionResult updateProductCategory(long id,[FromBody] UpdateProductDTO updateProductData) {
            updateProductData.id=id;
            if(new core.application.ProductController().updateProductCategory(updateProductData))
                return Ok(new SimpleJSONMessageService(VALID_PRODUCT_UPDATE_MESSAGE));
            return BadRequest(new SimpleJSONMessageService(INVALID_PRODUCT_UPDATE_MESSAGE));
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="id">Long with the product being disabled ID</param>
        /// <returns>HTTP Response 204;No Content if the product was disabled with success
        ///      <br>HTTP Response 400;Bad Request if an error occured while disabling the product
        /// </returns>
        /// 
        [HttpDelete("{id}")]
        public ActionResult disableProduct(long id){
            ProductDTO productDTO=new ProductDTO();
            productDTO.id=id;
            bool disabledWithSuccess=new core.application.ProductController().disableProduct(productDTO);
            if(disabledWithSuccess){
                return NoContent();
            }else{
                return BadRequest();
            }
        }
    }
}