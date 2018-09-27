using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using core.application;
using support.dto;
using System.Web;
using Newtonsoft.Json;

namespace backend.Controllers
{

    /// <summary>
    /// Backend ProductController class
    /// </summary>
    [Route("myc/products")]
    public class ProductController : Controller
    {
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
        /// Finds a product by ID
        /// </summary>
        /// <param name="productID"> id of the product</param>
        /// <returns>HTTP Response 400 Bad Request if a product with the id isn't found;
        /// HTTP Response 200 Ok with the product's info in JSON format </returns>
        [HttpGet("{id}")]
        public ActionResult<DTO> findByProductByID(string productID)
        {
            DTO productDTO = new core.application.ProductController().findProductByID(productID);

            if (productDTO == null)
            {
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
        public ActionResult<List<DTO>> findAll(){
            List<DTO> allProductsDTO = new core.application.ProductController().findAllProducts();

            if(allProductsDTO == null){
                return BadRequest(NO_PRODUCTS_FOUND_REFERENCE);
            }

            return Ok(allProductsDTO);
        }
    }
}