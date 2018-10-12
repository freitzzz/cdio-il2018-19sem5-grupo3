using core.dto;
using core.persistence;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace backend.Controllers{
    /// <summary>
    /// MVC Controller for CustomizedProduct operations
    /// </summary>
    [Route("/myc/api/customizedproducts")]
    public class CustomizedProductController:Controller{
        /// <summary>
        /// Constant that represents the message that ocurres if there are no customized products available
        /// </summary>
        private const string NO_CUSTOMIZED_PRODUCTS_AVAILABLE="There are no customized products available";

        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE="The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the message that ocurres if a client attemps to fetch a resource which doesn't exist or is not available
        /// </summary>
        private const string RESOURCE_NOT_FOUND_MESSAGE="The resource being fetched could not be found";

        /// <summary>
        /// Constant that represents the message that ocurres if an update is invalid
        /// </summary>
        private const string INVALID_UPDATE_MESSAGE="An error ocurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that ocurres if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE="The resource was updated with success";

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        public CustomizedProductController(CustomizedProductRepository customizedProductRepository){
            this.customizedProductRepository=customizedProductRepository;
        }

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult<List<CustomizedProductDTO>> findAll(){
            List<CustomizedProductDTO> customizedProductsDTOS=new core.application.CustomizedProductController().findAllCustomizedProducts();
            if(!Collections.isEnumerableNullOrEmpty(customizedProductsDTOS))
                return Ok(customizedProductsDTOS);
            return BadRequest(NO_CUSTOMIZED_PRODUCTS_AVAILABLE);
        }

        /// <summary>
        /// Fetches the information of a customized product by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products resource id</param>
        /// <returns>ActionResult with the customized product information</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomizedProductDTO> findByID(long id){
            try{
                CustomizedProductDTO customizedProductDTO=new CustomizedProductDTO();
                customizedProductDTO.id=id;
                CustomizedProductDTO customizedProduct=new core.application.CustomizedProductController().findCustomizedProductByID(customizedProductDTO);
                if(customizedProduct!=null)
                    return Ok(customizedProduct);
                return BadRequest(RESOURCE_NOT_FOUND_MESSAGE);
            }catch(NullReferenceException){
                return BadRequest(RESOURCE_NOT_FOUND_MESSAGE);
            }
        }

        /// <summary>
        /// Creates a new customized product
        /// </summary>
        /// <param name="customizedProductDTO">CustomizedProductDTO with the customized product being added</param>
        /// <returns>ActionResult with the created customized product</returns>
        [HttpPost]
        public ActionResult<CustomizedProductDTO> addCustomizedProduct([FromBody]CustomizedProductDTO customizedProductDTO){
            try{
                CustomizedProductDTO customizedProduct=new core.application.CustomizedProductController().addCustomizedProduct(customizedProductDTO);
                return Created(Request.Path,customizedProduct);
            }catch(NullReferenceException){
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                return BadRequest(invalidArgumentsException.Message);
            }
        }
    }
}