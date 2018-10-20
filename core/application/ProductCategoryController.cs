using System.Collections.Generic;
using core.persistence;
using core.domain;
using core.dto;
using System;
using support.dto;
using System.Linq;

namespace core.application
{
    /// <summary>
    /// Application controller for ProductCategory.
    /// </summary>
    public class ProductCategoryController
    {
        /// <summary>
        /// Constant representing an error message that should be presented when no ProductCategory matches the given identifier.
        /// </summary>
        private const string ERROR_CATEGORY_NOT_FOUND_ID = "No category was found matching the given identifier, please enter a valid category identifier and try again.";

        /// <summary>
        /// Constant representing an error message that should be presented when no ProductCategory matches the given name.
        /// </summary>
        private const string ERROR_CATEGORY_NOT_FOUND_NAME = "No category was found matching the given name, please enter a valid category name and try again.";
        
        /// <summary>
        /// Constant representing an error message that should be presented when no parent ProductCategory matches the given identifier.
        /// </summary>
        private const string ERROR_PARENT_NOT_FOUND = "No category was found matching the given parent identifier, please enter a valid category identifier and try again.";

        /// <summary>
        /// Constant representing an error message that should be presented when the ProductCategory was unable to be added.
        /// </summary>
        private const string ERROR_UNABLE_TO_ADD_CATEGORY = "An error while attempting to add the category, please check category data and try again.";

        /// <summary>
        /// Constant representing an erro message that should be presented when no instances of ProductCategory have been added to the repository.
        /// </summary>
        private const string ERROR_NO_CATEGORIES_FOUND = "No categories have been added, please add a category prior to performing this action.";


        /// <summary>
        /// Constructs a new instance of ProductCategory application controller.
        /// </summary>
        public ProductCategoryController() { }

        /// <summary>
        /// Adds a new ProductCategory to the repository.
        /// </summary>
        /// <param name="productCategoryDTO">DTO containing ProductCategory information.</param>
        /// <returns>Returns the added ProductCategory's DTO or null, if it was not added.</returns>
        public ProductCategoryDTO addProductCategory(ProductCategoryDTO productCategoryDTO)
        {
            ProductCategoryRepository repository = PersistenceContext.repositories().createProductCategoryRepository();

            long? parentId = productCategoryDTO.parentId;

            ProductCategory category = null;

            if (parentId != null)   //if a parent id is defined, add it as a subcategory
            {
                ProductCategory parentCategory = repository.find((long)parentId);

                if (parentCategory == null)
                {
                    throw new ArgumentException(ERROR_PARENT_NOT_FOUND);
                }

                category = new ProductCategory(productCategoryDTO.name, parentCategory);
            }
            else                //if no parent id is defined then add it as a root category 
            {
                category = new ProductCategory(productCategoryDTO.name);
            }

            ProductCategory addedCategory = repository.save(category);

            //category was not able to be added (probably due to a violation of business identifiers)
            if (addedCategory == null)
            {
                throw new ArgumentException(ERROR_UNABLE_TO_ADD_CATEGORY);
            }

            return addedCategory.toDTO();
        }

        /// <summary>
        /// Removes a ProductCategory from the repository.
        /// </summary>
        /// <param name="id">Database identifier of the ProductCategory to be removed.</param>
        /// <returns>A DTO representation of the removed ProductCategory.</returns>
        public ProductCategoryDTO removeProductCategory(long id)
        {
            ProductCategoryRepository categoryRepository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory categoryToBeRemoved = categoryRepository.find(id);

            //category does not exist
            if (categoryToBeRemoved == null)
            {
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_ID);
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

            IEnumerable<ProductCategory> categories = PersistenceContext.repositories().createProductCategoryRepository().findAll();

            //check if any categories have been added
            if(!categories.GetEnumerator().MoveNext()){
                throw new ArgumentException(ERROR_NO_CATEGORIES_FOUND);
            }

            return DTOUtils.parseToDTOS(categories).ToList();
        }


        /// <summary>
        /// Retrieves a DTO representation of the instance of ProductCategory with a matching database identifier.
        /// </summary>
        /// <param name="id">Database identifier.</param>
        /// <returns>DTO representation of the ProductCategory with the matching database identifier 
        /// or null if no ProductCategory with a matching id was found.</returns>
        public ProductCategoryDTO findByDatabaseId(long id)
        {
            ProductCategory category = PersistenceContext.repositories().createProductCategoryRepository().find(id);

            if (category == null)
            {   //category might not exist
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_ID);
            }

            return category.toDTO();
        }


        /// <summary>
        /// Retrieves a DTO representation of the instance of ProductCategory with a matching business identifier (name).
        /// </summary>
        /// <param name="name">Business identifier (name).</param>
        /// <returns>DTO representation of the instance of ProductCategory with a matching business identifier.</returns>
        public ProductCategoryDTO findByName(string name)
        {
            ProductCategory category = PersistenceContext.repositories().createProductCategoryRepository().find(name);

            if (category == null)
            {   //category might not exist
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_NAME);
            }

            return category.toDTO();
        }
    }
}