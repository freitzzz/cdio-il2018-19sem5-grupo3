using System.Collections.Generic;
using core.persistence;
using core.domain;
using support.dto;
using core.dto;
using System;

namespace core.application
{
    /// <summary>
    /// Application controller for ProductCategory.
    /// </summary>
    public class ProductCategoryController
    {
        /// <summary>
        /// Current instance of ProductCategoryRepository.
        /// </summary>
        private readonly ProductCategoryRepository categoryRepository;

        /// <summary>
        /// Constructs a new instance of ProductCategory application controller.
        /// </summary>
        /// <param name="productCategoryRepository">injected ProductCategoryRepository.</param>
        public ProductCategoryController(ProductCategoryRepository productCategoryRepository)
        {
            categoryRepository = productCategoryRepository;
        }

        /// <summary>
        /// Adds a new ProductCategory to the repository.
        /// </summary>
        /// <param name="productCategoryDTO">DTO containing ProductCategory information.</param>
        /// <returns>Returns the added ProductCategory's DTO or null, if it was not added.</returns>
        public ProductCategoryDTO addProductCategory(ProductCategoryDTO productCategoryDTO){

            ProductCategory category = categoryRepository.save(productCategoryDTO.toEntity());

            //category was not able to be added (probably due to a violation of business identifiers)
            if(category == null){
                return null;
            }

            return category.toDTO();
        }

        /// <summary>
        /// Removes a ProductCategory from the repository.
        /// </summary>
        /// <param name="id">Database identifier of the ProductCategory to be removed.</param>
        /// <returns>A DTO representation of the removed ProductCategory.</returns>
        public ProductCategoryDTO removeProductCategory(long id){
            
            ProductCategory categoryToBeRemoved = categoryRepository.find(id);

            //category does not exist
            if(categoryToBeRemoved == null){
                return null;
            }

            return categoryRepository.remove(categoryToBeRemoved).toDTO();
        }

        /// <summary>
        /// Retrieves all instances of ProductCategory that are currently present within the repository.
        /// </summary>
        /// <returns>Returns a list with DTO's of all the instances of ProductCategory in the repository. </returns>
        public List<ProductCategoryDTO> findAllCategories()
        {
            List<ProductCategoryDTO> categoryDTOList = new List<ProductCategoryDTO>();

            IEnumerable<ProductCategory> categories = categoryRepository.findAll();

            foreach (ProductCategory category in categories)
            {
                categoryDTOList.Add(category.toDTO());
            }


            return categoryDTOList;
        }


        /// <summary>
        /// Retrieves a DTO representation of the instance of ProductCategory with a matching database identifier.
        /// </summary>
        /// <param name="id">Database identifier.</param>
        /// <returns>DTO representation of the ProductCategory with the matching database identifier 
        /// or null if no ProductCategory with a matching id was found.</returns>
        public ProductCategoryDTO findByDatabaseId(long id)
        {
            ProductCategory category = categoryRepository.find(id);

            if(category == null){   //category might not exist
                return null;
            }

            return category.toDTO();
        }

        /// <summary>
        /// Retrieves a DTO representation of the instance of ProductCategory with a matching business identifier (name).
        /// </summary>
        /// <param name="id">Business identifier (name).</param>
        /// <returns>DTO representation of the instance of ProductCategory with a matching business identifier.</returns>
        public ProductCategoryDTO findByName(string id)
        {
            ProductCategory category = categoryRepository.find(id);

            if(category == null){   //category might not exist
                return null;
            }

            return category.toDTO();
        }
    }
}