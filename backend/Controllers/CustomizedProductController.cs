using core.dto;
using core.persistence;
using support.utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace backend.Controllers{
    /// <summary>
    /// MVC Controller for CustomizedProduct operations
    /// </summary>
    [Route("/myc/api/customizedproducts")]
    public class CustomizedProductController:Controller{
        /// <summary>
        /// Constant that represents the message that occurs if there are no customized products available
        /// </summary>
        private const string NO_CUSTOMIZED_PRODUCTS_AVAILABLE="There are no customized products available";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a product with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE="The request body is invalid! Check documentation for more information";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to fetch a resource which doesn't exist or is not available
        /// </summary>
        private const string RESOURCE_NOT_FOUND_MESSAGE="The resource being fetched could not be found";

        /// <summary>
        /// Constant that represents the message that occurs if an update is invalid
        /// </summary>
        private const string INVALID_UPDATE_MESSAGE="An error ocurred during the update of the resource";

        /// <summary>
        /// Constant that represents the message that occurs if an update is valid
        /// </summary>
        private const string VALID_UPDATE_MESSAGE="The resource was updated with success";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request starts
        /// </summary>
        private const string LOG_GET_ALL_START="GET All Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request starts
        /// </summary>
        private const string LOG_GET_BY_ID_START="GET By ID Request started";

        /// <summary>
        /// Constant that represents the log message for when a POST Request starts
        /// </summary>
        private const string LOG_POST_START="POST Request started";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request is successful
        /// </summary>
        private const string LOG_GET_ALL_SUCCESS="Customized Products {@customizedProducts} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request is successful
        /// </summary>
        private const string LOG_GET_BY_ID_SUCCESS="Customized Product {@customizedProduct} retrieved";

        /// <summary>
        /// Constant that represents the log message for when a POST Request is successful
        /// </summary>
        private const string LOG_POST_SUCCESS="Customized Product {@customizedProduct} created";

        /// <summary>
        /// Constant that represents the log message for when a GET All Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_ALL_BAD_REQUEST="GET All BadRequest (No Customized Products Found)";

        /// <summary>
        /// Constant that represents the log message for when a GET By ID Request returns a BadRequest
        /// </summary>
        private const string LOG_GET_BY_ID_BAD_REQUEST="GETByID({id}) BadRequest";

        /// <summary>
        /// Constant that represents the log message for when a POST Request returns a BadRequest
        /// </summary>
        private const string LOG_POST_BAD_REQUEST="POST {@customizedProduct} BadRequest";

        /// <summary>
        /// This repository attribute is only here due to entity framework injection
        /// </summary>
        private readonly CustomizedProductRepository customizedProductRepository;

        /// <summary>
        /// Controllers logger
        /// </summary>
        private readonly ILogger<CustomizedProductController> logger;

        /// <summary>
        /// This constructor is only here due to entity framework injection
        /// </summary>
        /// <param name="customizedProductRepository">Injected repository of customized products</param>
        /// <param name="logger">Controllers logger to log any information regarding HTTP Requests and Responses</param>
        public CustomizedProductController(CustomizedProductRepository customizedProductRepository, ILogger<CustomizedProductController> logger){
            this.customizedProductRepository=customizedProductRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>ActionResult with all available customized products</returns>
        [HttpGet]
        public ActionResult<List<CustomizedProductDTO>> findAll(){
            logger.LogInformation(LOG_GET_ALL_START);
            List<CustomizedProductDTO> customizedProductsDTOS=new core.application.CustomizedProductController().findAllCustomizedProducts();
            if(!Collections.isEnumerableNullOrEmpty(customizedProductsDTOS))
            {
                logger.LogInformation(LOG_GET_ALL_SUCCESS,customizedProductsDTOS);
                return Ok(customizedProductsDTOS);
            }
            logger.LogWarning(LOG_GET_ALL_BAD_REQUEST);
            return BadRequest(NO_CUSTOMIZED_PRODUCTS_AVAILABLE);
        }

        /// <summary>
        /// Fetches the information of a customized product by its resource id
        /// </summary>
        /// <param name="id">Long with the customized products resource id</param>
        /// <returns>ActionResult with the customized product information</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomizedProductDTO> findByID(long id){
            logger.LogInformation(LOG_GET_BY_ID_START);
            try{
                CustomizedProductDTO customizedProductDTO=new CustomizedProductDTO();
                customizedProductDTO.id=id;
                CustomizedProductDTO customizedProduct=new core.application.CustomizedProductController().findCustomizedProductByID(customizedProductDTO);
                if(customizedProduct!=null)
                {
                    logger.LogInformation(LOG_GET_BY_ID_SUCCESS);
                    return Ok(customizedProduct);
                }
                logger.LogWarning(LOG_GET_BY_ID_BAD_REQUEST,id);
                return BadRequest(RESOURCE_NOT_FOUND_MESSAGE);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_GET_BY_ID_BAD_REQUEST,id);
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
            logger.LogInformation(LOG_POST_START);
            try{
                CustomizedProductDTO customizedProduct=new core.application.CustomizedProductController().addCustomizedProduct(customizedProductDTO);
                logger.LogInformation(LOG_POST_SUCCESS,customizedProduct);
                return Created(Request.Path,customizedProduct);
            }catch(NullReferenceException nullReferenceException){
                logger.LogWarning(nullReferenceException,LOG_POST_BAD_REQUEST,customizedProductDTO);
                return BadRequest(INVALID_REQUEST_BODY_MESSAGE);
            }catch(InvalidOperationException invalidOperationException){
                logger.LogWarning(invalidOperationException,LOG_POST_BAD_REQUEST,customizedProductDTO);
                return BadRequest(invalidOperationException.Message);
            }catch(ArgumentException invalidArgumentsException){
                logger.LogWarning(invalidArgumentsException,LOG_POST_BAD_REQUEST,customizedProductDTO);
                return BadRequest(invalidArgumentsException.Message);
            }
        }
    }
}