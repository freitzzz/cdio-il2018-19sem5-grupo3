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

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository)
        {
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
        public ActionResult<ProductDTO> findProductByID(long productID)
        {
            ProductDTO productDTO = new core.application.ProductController(productRepository, materialRepository).findProductByID(productID);
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
        public ActionResult<List<ProductDTO>> findAll()
        {
            List<ProductDTO> allProductsDTO = new core.application.ProductController(productRepository, materialRepository).findAllProducts();

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
        public ActionResult<ProductDTO> addProduct([FromBody] ProductDTO jsonData){

            ProductDTO productDTO = new core.application.ProductController(productRepository, materialRepository).addProduct(jsonData);

            if(productDTO!=null){
                return Ok(productDTO);
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
        public ActionResult updateProduct([FromBody] JObject jsonData, long productID)
        {
            ProductObject instance = JsonConvert.DeserializeObject<ProductObject>(jsonData.ToString());
            instance.persistenceID=productID;
            GenericDTO productDTO = productObjectToProductDTO(instance);

            List<GenericDTO> heightDimensionDTOList = (List<GenericDTO>)productDTO.get(Product.Properties.HEIGHT_VALUES_PROPERTIES);
            List<GenericDTO> widthDimensionDTOList = (List<GenericDTO>)productDTO.get(Product.Properties.WIDTH_VALUES_PROPERTIES);
            List<GenericDTO> depthDimensionDTOList = (List<GenericDTO>)productDTO.get(Product.Properties.DEPTH_VALUES_PROPERTIES);

            if (Collections.isListEmpty(heightDimensionDTOList) || Collections.isListEmpty(widthDimensionDTOList)
                    || Collections.isListEmpty(depthDimensionDTOList))
            {
                    return BadRequest();
            }

            if (!new core.application.ProductController(productRepository, materialRepository).updateProduct(productDTO))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("{id}/category")]
        public ActionResult updateProductCategory(long productID,[FromBody]ProductCategoryDTO productCategoryDTO){
            
            return null;
        }

        /// <summary>
        /// Removes a product from it collection
        /// </summary>
        /// <param name="productID">long with the product ID</param>
        /// <returns>HTTP Response 200 Ok if the product was removed with successs
        ///         <br>HTTP Response 400 Bad Request if an error occured while removing the product
        ///         <br>See MyC REST API documentation for a better overview
        /// </returns>
        [HttpDelete("{id}")]
        public ActionResult removeProduct(long productID){
            ProductDTO productDTO=new ProductDTO();
            productDTO.id=productID;
            bool removedWithSuccess=new core.application.ProductController(productRepository, materialRepository).removeProduct(productDTO);
            if(removedWithSuccess){
                return Ok("{\"Message\":\"The product was removed with success\"}");
            }else{
                return BadRequest("{\"Message\":\"An error ocurred while creating the product\"}");
            }
        }

        /// <summary>
        /// Parses a ProductObject into a product DTO
        /// </summary>
        /// <param name="productObject">ProductObject with the product information</param>
        /// <returns>DTO with the parsed ProductObject</returns>
        private GenericDTO productObjectToProductDTO(ProductObject productObject){
            GenericDTO productDTO=new GenericDTO(Product.Properties.CONTEXT);
            productDTO.put(Product.Properties.REFERENCE_PROPERTY,productObject.reference);
            productDTO.put(Product.Properties.DESIGNATION_PROPERTY,productObject.designation);
            productDTO.put(Product.Properties.COMPLEMENTED_PRODUCTS_PROPERTY,productObject.complementedProducts);
            productDTO.put(Product.Properties.MATERIALS_PROPERTY,productObject.materials);
            List<GenericDTO> heightRestrictionDTOList = new List<GenericDTO>();
            List<GenericDTO> widthRestrictionDTOList = new List<GenericDTO>();
            List<GenericDTO> depthRestrictionDTOList = new List<GenericDTO>();

            if (!Collections.isListEmpty(productObject.restrictions))
            {
                foreach (Restriction restriction in productObject.restrictions)
                {
                    GenericDTO restrictionDTO = new GenericDTO("restriction");
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
                productDTO.put(Product.Properties.HEIGHT_VALUES_PROPERTIES, heightRestrictionDTOList);
                productDTO.put(Product.Properties.WIDTH_VALUES_PROPERTIES, widthRestrictionDTOList);
                productDTO.put(Product.Properties.DEPTH_VALUES_PROPERTIES, depthRestrictionDTOList);
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