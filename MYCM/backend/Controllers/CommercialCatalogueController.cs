using System;
using Microsoft.AspNetCore.Mvc;
using core.persistence;
using backend.utils;
using Microsoft.Extensions.Logging;
using core.modelview.commercialcatalogue;
using core.exceptions;
using core.modelview.cataloguecollection;
using core.modelview.customizedproduct;
using core.modelview.cataloguecollectionproduct;

namespace backend.Controllers
{
    ///<summary>
    ///Backend CommercialCatalogueController class.
    ///</summary>
    [Route("mycm/api/commercialcatalogues")]
    public class CommercialCatalogueController : Controller
    {
        /// <summary>
        /// Constant representing the message presented when an unexpected error occurs.
        /// </summary>
        private const string UNEXPECTED_ERROR = "An unexpected error occurred, please try again later.";

        /// <summary>
        /// Constant that represents the message that occurs if a client attemps to create a commercialCatalogue with an invalid request body
        /// </summary>
        private const string INVALID_REQUEST_BODY_MESSAGE = "The request body is invalid. Check documentation for more information";

        /// <summary>
        /// Repository used to manipulate CommercialCatalogue instances
        /// </summary>
        private readonly CommercialCatalogueRepository commercialCatalogueRepository;

        /// <summary>
        /// Constructor with injected type of repository and logger
        /// </summary>
        /// <param name="commercialCatalogueRepository">repository used to manipulate CommercialCatalogue instances</param>
        public CommercialCatalogueController(CommercialCatalogueRepository commercialCatalogueRepository)
        {
            this.commercialCatalogueRepository = commercialCatalogueRepository;
        }


        /// <summary>
        /// Retrieves all instances of CommercialCatalogue.
        /// </summary>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance GetAllCommercialCataloguesModelView representing all isntances of CommercialCatalogue.null
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue was found.
        /// </returns>
        [HttpGet]
        public ActionResult findAll()
        {
            try
            {
                GetAllCommercialCataloguesModelView allCommercialCataloguesModelView = new core.application.CommercialCatalogueController().findAll();
                return Ok(allCommercialCataloguesModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Retrieves an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="id">CommercialCatalogue's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetCommercialCatalogueModelView representing the CommercialCatalogue.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue was found with the given identifier.
        /// </returns>
        [HttpGet("{id}", Name = "GetCommercialCatalogue")]
        public ActionResult findById(long id)
        {
            try
            {
                FindCommercialCatalogueModelView findCommercialCatalogueModelView = new FindCommercialCatalogueModelView();
                findCommercialCatalogueModelView.commercialCatalogueId = id;

                GetCommercialCatalogueModelView getCommercialCatalogueModelView = new core.application.CommercialCatalogueController().findCommercialCatalogue(findCommercialCatalogueModelView);

                return Ok(getCommercialCatalogueModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Retrieves a CommercialCatalogue's list of CatalogueCollection.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetAllCatalogueCollectionsModelView representing the CommercialCatalogue's list of CatalogueCollection.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue was found with the given identifier or the list is empty.
        /// </returns>
        [HttpGet("{commercialCatalogueId}/collections")]
        public ActionResult findCatalogueCollections(long commercialCatalogueId)
        {
            try
            {
                FindCommercialCatalogueModelView findCommercialCatalogueModelView = new FindCommercialCatalogueModelView();
                findCommercialCatalogueModelView.commercialCatalogueId = commercialCatalogueId;

                GetAllCatalogueCollectionsModelView allCatalogueCollectionsModelView = new core.application.CommercialCatalogueController().findCatalogueCollections(findCommercialCatalogueModelView);

                return Ok(allCatalogueCollectionsModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Retrieves a CommercialCatalogue's CatalogueCollection.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="customizedProductCollectionId">CatalogueCollection's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetCatalogueCollectionModelView representing the CatalogueCollection.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue or CatalogueCollection were found with the given identifiers.
        /// </returns>
        [HttpGet("{commercialCatalogueId}/collections/{customizedProductCollectionId}")]
        public ActionResult findCatalogueCollection(long commercialCatalogueId, long customizedProductCollectionId)
        {
            try
            {
                FindCatalogueCollectionModelView findCatalogueCollectionModelView = new FindCatalogueCollectionModelView();
                findCatalogueCollectionModelView.commercialCatalogueId = commercialCatalogueId;
                findCatalogueCollectionModelView.customizedProductCollectionId = customizedProductCollectionId;

                GetCatalogueCollectionModelView getCatalogueCollectionModelView = new core.application.CommercialCatalogueController().findCatalogueCollection(findCatalogueCollectionModelView);

                return Ok(getCatalogueCollectionModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Retrieves a CatalogueCollection's list of CustomizedProduct.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="customizedProductCollectionId">CatalogueCollection's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetAllCustomizedProductsModelView representing the CatalogueCollection's list of CustomizedProduct.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue or CatalogueCollection were found with the given identifiers or if the CatalogueCollection is empty.
        /// </returns>
        [HttpGet("{commercialCatalogueId}/collections/{customizedProductCollectionId}/customizedproducts")]
        public ActionResult findCatalogueCollectionProducts(long commercialCatalogueId, long customizedProductCollectionId)
        {
            try
            {
                FindCatalogueCollectionModelView findCatalogueCollectionModelView = new FindCatalogueCollectionModelView();
                findCatalogueCollectionModelView.commercialCatalogueId = commercialCatalogueId;
                findCatalogueCollectionModelView.customizedProductCollectionId = customizedProductCollectionId;

                GetAllCustomizedProductsModelView getCustomizedProductsModelView = new core.application.CommercialCatalogueController().findCatalogueCollectionProducts(findCatalogueCollectionModelView);

                return Ok(getCustomizedProductsModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Adds a new CommercialCatalogue.
        /// </summary>
        /// <param name="addCommercialCatalogueModelView">AddCommercialCatalogueModelView with the CommercialCatalogue's data.</param>
        /// <returns>
        /// ActionResult with the 201 HTTP Code and an instance of GetCommercialCatalogueModelView representing the added CommercialCatalogue.
        /// or
        /// ActionResult with the 400 HTTP Code if the request body is null or has invalid information.
        /// </returns>
        [HttpPost]
        public ActionResult addCommercialCatalogue([FromBody]AddCommercialCatalogueModelView addCommercialCatalogueModelView)
        {

            if (addCommercialCatalogueModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                GetCommercialCatalogueModelView getCommercialCatalogueModelView = new core.application.CommercialCatalogueController()
                    .addCommercialCatalogue(addCommercialCatalogueModelView);

                return CreatedAtRoute("GetCommercialCatalogue", new { id = getCommercialCatalogueModelView.commercialCatalogueId }, getCommercialCatalogueModelView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Adds a CatalogueCollection to a CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="addCatalogueCollectionModelView">AddCatalogueCollectionModelView with the new CatalogueCollection's data.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetCommercialCatalogueModelView representing the updated CommercialCatalogue.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue is found.
        /// or
        /// ActionResult with the 400 HTTP Code if the request body contains invalid information.
        /// </returns>
        [HttpPost("{commercialCatalogueId}/collections")]
        public ActionResult addCatalogueCollection(long commercialCatalogueId, [FromBody]AddCatalogueCollectionModelView addCatalogueCollectionModelView)
        {
            if (addCatalogueCollectionModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                addCatalogueCollectionModelView.commercialCatalogueId = commercialCatalogueId;
                GetCommercialCatalogueModelView commercialCatalogueModelView = new core.application.CommercialCatalogueController()
                    .addCatalogueCollection(addCatalogueCollectionModelView);

                return Ok(commercialCatalogueModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Adds a CustomizedProduct to a CatalogueCollection.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="customizedProductCollectionId">CustomizedProductCollection's persistence identifier.</param>
        /// <param name="addCatalogueCollectionProductModelView">AddCatalogueCollectionProductModelView with the CustomizedProduct's data.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetCommercialCatalogueModelView representing the updated CommercialCatalogue.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue or CatalogueCollection were found with the given identifiers.
        /// or
        /// ActionResult with the 400 HTTP Code if the request body contains invalid information.
        /// </returns>
        [HttpPost("{commercialCatalogueId}/collections/{customizedProductCollectionId}/customizedproducts")]
        public ActionResult addCatalogueCollectionProduct(long commercialCatalogueId, long customizedProductCollectionId,
            [FromBody] AddCatalogueCollectionProductModelView addCatalogueCollectionProductModelView)
        {
            if (addCatalogueCollectionProductModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                addCatalogueCollectionProductModelView.commercialCatalogueId = commercialCatalogueId;
                addCatalogueCollectionProductModelView.customizedProductCollectionId = customizedProductCollectionId;

                GetCommercialCatalogueModelView commercialCatalogueModelView = new core.application.CommercialCatalogueController()
                    .addCatalogueCollectionProduct(addCatalogueCollectionProductModelView);

                return Ok(commercialCatalogueModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception )
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Updates a CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="updateCommercialCatalogueModelView">UpdateCommercialCatalogueModelView with the CommercialCatalogue's updated data.</param>
        /// <returns>
        /// ActionResult with the 200 HTTP Code and an instance of GetCommercialCatalogueModelView representing the updated CommercialCatalogue.
        /// or
        /// ActionResult with the 404 HTTP Code if no CommercialCatalogue was found with the given identifier.
        /// or
        /// ActionResult with the 400 HTTP Code if the request body contains invalid information.
        /// </returns>
        [HttpPut("{commercialCatalogueId}")]
        public ActionResult updateCommercialCatalogue(long commercialCatalogueId,
            [FromBody] UpdateCommercialCatalogueModelView updateCommercialCatalogueModelView)
        {
            if (updateCommercialCatalogueModelView == null)
            {
                return BadRequest(new SimpleJSONMessageService(INVALID_REQUEST_BODY_MESSAGE));
            }

            try
            {
                updateCommercialCatalogueModelView.commercialCatalogueId = commercialCatalogueId;

                GetCommercialCatalogueModelView getCommercialCatalogueModelView = new core.application.CommercialCatalogueController()
                    .updateCommercialCatalogue(updateCommercialCatalogueModelView);

                return Ok(getCommercialCatalogueModelView);
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Deletes an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 204 HTTP Code if the CommercialCatalogue was deleted successfully.
        /// or
        /// ActionResult with the 404 HTTP Code if the CommercialCatalogue with the given identifier could not be found.
        /// </returns>
        [HttpDelete("{commercialCatalogueId}")]
        public ActionResult deleteCommercialCatalogue(long commercialCatalogueId)
        {
            try
            {
                DeleteCommercialCatalogueModelView deleteCommercialCatalogueModelView = new DeleteCommercialCatalogueModelView();
                deleteCommercialCatalogueModelView.commercialCatalogueId = commercialCatalogueId;

                new core.application.CommercialCatalogueController().deleteCommercialCatalogue(deleteCommercialCatalogueModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }


        /// <summary>
        /// Deletes a CommercialCatalogue's CatalogueCollection.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="customizedProductCollectionId">CustomizedProductCollection's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 204 HTTP Code if the CatalogueCollection was deleted successfully.
        /// or
        /// ActionResult with the 404 HTTP Code if the CommercialCatalogue or the CatalogueCollection could not be found with the given identifiers.
        /// </returns>
        [HttpDelete("{commercialCatalogueId}/collections/{customizedProductCollectionId}")]
        public ActionResult deleteCatalogueCollection(long commercialCatalogueId, long customizedProductCollectionId)
        {
            try
            {
                DeleteCatalogueCollectionModelView deleteCatalogueCollectionModelView = new DeleteCatalogueCollectionModelView();
                deleteCatalogueCollectionModelView.commercialCatalogueId = commercialCatalogueId;
                deleteCatalogueCollectionModelView.customizedProductCollectionId = customizedProductCollectionId;

                new core.application.CommercialCatalogueController().deleteCatalogueCollection(deleteCatalogueCollectionModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }

        /// <summary>
        /// Deletes a CatalogueCollection's CatalogueCollectionProduct.
        /// </summary>
        /// <param name="commercialCatalogueId">CommercialCatalogue's persistence identifier.</param>
        /// <param name="customizedProductCollectionId">CustomizedProductCollection's persistence identifier.</param>
        /// <param name="customizedProductId">CustomizedProduct's persistence identifier.</param>
        /// <returns>
        /// ActionResult with the 204 HTTP Code if the CatalogueCollectionProduct was deleted successfully.
        /// or
        /// ActionResult with the 404 HTTP Code if the CommercialCatalogue, the CatalogueCollection or the CatalogueCollectionProduct could not be found with the given identifiers.
        /// </returns>
        [HttpDelete("{commercialCatalogueId}/collections/{customizedProductCollectionId}/customizedproducts/{customizedProductId}")]
        public ActionResult deleteCatalogueCollectionProduct(long commercialCatalogueId, long customizedProductCollectionId, long customizedProductId)
        {
            try
            {
                DeleteCatalogueCollectionProductModelView deleteCatalogueCollectionProductModelView = new DeleteCatalogueCollectionProductModelView();
                deleteCatalogueCollectionProductModelView.commercialCatalogueId = commercialCatalogueId;
                deleteCatalogueCollectionProductModelView.customizedProductCollectionId = customizedProductCollectionId;
                deleteCatalogueCollectionProductModelView.customizedProductId = customizedProductId;

                new core.application.CommercialCatalogueController().deleteCatalogueCollectionProduct(deleteCatalogueCollectionProductModelView);

                return NoContent();
            }
            catch (ResourceNotFoundException e)
            {
                return NotFound(new SimpleJSONMessageService(e.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new SimpleJSONMessageService(UNEXPECTED_ERROR));
            }
        }
    }
}