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
        public ActionResult<List<DTO>> findAll()
        {
            List<DTO> allProductsDTO = new core.application.ProductController().findAllProducts();

            if (allProductsDTO == null)
            {
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
        public ActionResult<DTO> addProduct([FromBody] JObject jsonData){
            ProductObject productObject=JsonConvert.DeserializeObject<ProductObject>(jsonData.ToString());
            DTO productDTO=productObjectToProductDTO(productObject);
            DTO createdProductDTO=new core.application.ProductController().addProduct(productDTO);
            if(createdProductDTO!=null){
                return Ok(createdProductDTO);
            }else{
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
        public ActionResult updateProduct([FromBody] JObject jsonData, String productID)
        {
            ProductObject instance = JsonConvert.DeserializeObject<ProductObject>(jsonData.ToString());
            instance.persistenceID=long.Parse(productID);
            DTO productDTO = productObjectToProductDTO(instance);

            List<DTO> heightRestrictionDTOList = (List<DTO>)productDTO.get(Product.Properties.HEIGHT_RESTRICTIONS_PROPERTIES);
            List<DTO> widthRestrictionDTOList = (List<DTO>)productDTO.get(Product.Properties.WIDTH_RESTRICTIONS_PROPERTIES);
            List<DTO> depthRestrictionDTOList = (List<DTO>)productDTO.get(Product.Properties.DEPTH_RESTRICTIONS_PROPERTIES);

            if (Collections.isListEmpty(heightRestrictionDTOList) || Collections.isListEmpty(widthRestrictionDTOList)
                    || Collections.isListEmpty(depthRestrictionDTOList))
            {
                    return BadRequest();
            }

            if (!new core.application.ProductController().updateProduct(productDTO))
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Parses a ProductObject into a product DTO
        /// </summary>
        /// <param name="productObject">ProductObject with the product information</param>
        /// <returns>DTO with the parsed ProductObject</returns>
        private DTO productObjectToProductDTO(ProductObject productObject){
            DTO productDTO=new GenericDTO(Product.Properties.CONTEXT);
            productDTO.put(Product.Properties.REFERENCE_PROPERTY,productObject.reference);
            productDTO.put(Product.Properties.DESIGNATION_PROPERTY,productObject.designation);
            productDTO.put(Product.Properties.COMPLEMENTED_PRODUCTS_PROPERTY,productObject.complementedProducts);
            productDTO.put(Product.Properties.MATERIALS_PROPERTY,productObject.materials);
            List<DTO> heightRestrictionDTOList = new List<DTO>();
            List<DTO> widthRestrictionDTOList = new List<DTO>();
            List<DTO> depthRestrictionDTOList = new List<DTO>();

            if (!Collections.isListEmpty(productObject.restrictions))
            {
                foreach (Restriction restriction in productObject.restrictions)
                {
                    DTO restrictionDTO = new GenericDTO("restriction");
                    String restrictionDimension = restriction.dimension;
                    restrictionDTO.put("type", restriction.type);
                    restrictionDTO.put("values", restriction.values);
                    if (restrictionDimension.Equals("height"))
                    {
                        heightRestrictionDTOList.Add(restrictionDTO);
                    }
                    if (restrictionDimension.Equals("width"))
                    {
                        widthRestrictionDTOList.Add(restrictionDTO);
                    }
                    else
                    {
                        depthRestrictionDTOList.Add(restrictionDTO);
                    }

                }
                productDTO.put(Product.Properties.HEIGHT_RESTRICTIONS_PROPERTIES, heightRestrictionDTOList);
                productDTO.put(Product.Properties.WIDTH_RESTRICTIONS_PROPERTIES, widthRestrictionDTOList);
                productDTO.put(Product.Properties.DEPTH_RESTRICTIONS_PROPERTIES, depthRestrictionDTOList);
            }

            if (!Collections.isListEmpty(productObject.materials))
            {
                productDTO.put(Product.Properties.MATERIALS_PROPERTY, productObject.materials);
            }
            productDTO.put(Product.Properties.PERSISTENCE_ID_PROPERTY,productObject.persistenceID);
            return productDTO;
        }
    }

    /// <summary>
    /// ProductObject class to help the deserialization of a product's updates from JSON format
    /// </summary>
    public class Restriction
    {
        public string dimension { get; set; }
        public string type { get; set; }
        public List<string> values { get; set; }
    }

    /// <summary>
    /// ProductObject class to help the deserialization of a product's updates from JSON format
    /// </summary>
    public class ProductObject
    {
        public string reference{get;set;}
        public string designation{get;set;}
        public long persistenceID{get;set;}
        public List<long> complementedProducts{get;set;}
        public List<Restriction> restrictions { get; set; }
        public List<string> materials { get; set; }
    }

}