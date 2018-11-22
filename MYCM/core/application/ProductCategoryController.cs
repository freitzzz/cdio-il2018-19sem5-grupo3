using System.Collections.Generic;
using core.persistence;
using core.domain;
using System;
using System.Linq;
using core.modelview.productcategory;
using core.exceptions;

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
        /// Constant representing an error message that should be presented when no instances of ProductCategory have been added to the repository.
        /// </summary>
        private const string ERROR_NO_CATEGORIES_FOUND = "No categories have been added, please add a category prior to performing this action.";

        /// <summary>
        /// Constant representing an error message that should be presented when a ProductCategory is attempted to be updated with the name of another ProductCategory.
        /// </summary>
        private const string ERROR_DUPLICATE_NAME = "A category already exists with the specified name.";

        /// <summary>
        /// Constant representing an error message that should be presented when the new name is not valid.
        /// </summary>
        private const string ERROR_INVALID_NAME = "The new name is not valid";


        /// <summary>
        /// Constructs a new instance of ProductCategory application controller.
        /// </summary>
        public ProductCategoryController() { }

        /// <summary>
        /// Adds a new ProductCategory to the repository.
        /// </summary>
        /// <param name="modelView">ModelView containing ProductCategory information.</param>
        /// <returns>Returns the added ProductCategory's ModelView.</returns>
        public GetProductCategoryModelView addProductCategory(AddProductCategoryModelView modelView)
        {
            ProductCategoryRepository repository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory category = new ProductCategory(modelView.name);

            category = repository.save(category);

            //category was not able to be added (probably due to a violation of business identifiers)
            if (category == null)
            {
                throw new ArgumentException(ERROR_UNABLE_TO_ADD_CATEGORY);
            }

            return ProductCategoryModelViewService.fromEntity(category);
        }

        /// <summary>
        /// Adds a new ProductCategory with the ProductCategory with the matching given identifier as its parent.
        /// </summary>
        /// <param parentId="parentId"></param>
        /// <param name="modelView">ModelView containing ProductCategory information.</param>
        /// <returns>Returns the added ProductCategory's ModelView.</returns>
        public GetProductCategoryModelView addSubProductCategory(long parentId, AddProductCategoryModelView modelView)
        {
            ProductCategoryRepository repository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory parentCategory = repository.find(parentId);

            if (parentCategory == null)
            {
                throw new ArgumentException(ERROR_PARENT_NOT_FOUND);
            }

            ProductCategory category = new ProductCategory(modelView.name, parentCategory);

            category = repository.save(category);

            //category was not able to be added (probably due to a violation of business identifiers)
            if (category == null)
            {
                throw new ArgumentException(ERROR_UNABLE_TO_ADD_CATEGORY);
            }

            return ProductCategoryModelViewService.fromEntity(category);
        }

        /// <summary>
        /// Removes a ProductCategory from the repository.
        /// </summary>
        /// <param name="id">Database identifier of the ProductCategory to be removed.</param>
        /// <returns>A ModelView representation of the removed ProductCategory.</returns>
        public GetProductCategoryModelView removeProductCategory(long id)
        {
            ProductCategoryRepository categoryRepository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory category = categoryRepository.find(id);

            //category does not exist
            if (category == null)
            {
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_ID);
            } 

            category = categoryRepository.remove(category);

            return ProductCategoryModelViewService.fromEntity(category);
        }


        /// <summary>
        /// Retrieves all instances of ProductCategory that are currently present within the repository.
        /// </summary>
        /// <returns>Returns a list with ModelViews of all the instances of ProductCategory in the repository. </returns>
        public GetAllProductCategoriesModelView findAllCategories()
        {
            IEnumerable<ProductCategory> categories = PersistenceContext.repositories().createProductCategoryRepository().findAll();

            //check if any categories have been added
            if (!categories.Any())
            {
                throw new ArgumentException(ERROR_NO_CATEGORIES_FOUND);
            }

            return ProductCategoryModelViewService.fromCollection(categories);
        }


        /// <summary>
        /// Retrieves all instances of ProductCategory that are subcategories of the ProductCategory with the given identifier.
        /// </summary>
        /// <param name="parentId">Parent ProductCategory's database identifier.</param>
        /// <returns>Returns a list with ModelViews of all the instances of ProductCategory that are subcategories of the given ProductCategory.</returns>
        public GetAllProductCategoriesModelView findAllSubCategories(long parentId)
        {
            ProductCategoryRepository repository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory parentCategory = repository.find(parentId);

            if (parentCategory == null)
            {
                throw new ArgumentException(ERROR_PARENT_NOT_FOUND);
            }

            IEnumerable<ProductCategory> subCategories = repository.findSubCategories(parentCategory);

            //check if any categories have been added
            if (!subCategories.Any())
            {
                throw new ArgumentException(ERROR_NO_CATEGORIES_FOUND);
            }

            return ProductCategoryModelViewService.fromCollection(subCategories);
        }

        /// <summary>
        /// Retrieves all instances of ProductCategory that are leaves.
        /// </summary>
        /// <returns>GetAllProductCategoriesModelView with data regarding all of the leaf ProductCategory.</returns>
        /// <exception cref="ResourceNotFoundException">Throw when no leaf ProductCategory is found.</exception>
        public GetAllProductCategoriesModelView findLeaves(){
            
            IEnumerable<ProductCategory> leaves = PersistenceContext.repositories().createProductCategoryRepository().findLeaves();

            if(!leaves.Any()){
                throw new ResourceNotFoundException(ERROR_NO_CATEGORIES_FOUND);
            }

            return ProductCategoryModelViewService.fromCollection(leaves);
        }

        /// <summary>
        /// Retrieves a ModelView representation of the instance of ProductCategory with a matching database identifier.
        /// </summary>
        /// <param name="id">Database identifier.</param>
        /// <returns>ModelView representation of the ProductCategory with the matching database identifier 
        /// or null if no ProductCategory with a matching id was found.</returns>
        public GetProductCategoryModelView findByDatabaseId(long id)
        {
            ProductCategory category = PersistenceContext.repositories().createProductCategoryRepository().find(id);

            if (category == null)
            {   //category might not exist
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_ID);
            }

            return ProductCategoryModelViewService.fromEntity(category);
        }


        /// <summary>
        /// Retrieves a ModelView representation of the instance of ProductCategory with a matching business identifier (name).
        /// </summary>
        /// <param name="name">Business identifier (name).</param>
        /// <returns>ModelView representation of the instance of ProductCategory with a matching business identifier.</returns>
        public GetProductCategoryModelView findByName(string name)
        {
            ProductCategory category = PersistenceContext.repositories().createProductCategoryRepository().find(name);

            if (category == null)
            {   //category might not exist
                throw new ArgumentException(ERROR_CATEGORY_NOT_FOUND_NAME);
            }

            return ProductCategoryModelViewService.fromEntity(category);
        }


        /// <summary>
        /// Updates a ProductCategory with a given database identifier with the data in given ModelView.
        /// </summary>
        /// <param name="id">ProductCategory's database identifier.</param>
        /// <param name="modelView">ModelView containing the data being updated.</param>
        /// <returns>A ModelView with the updated ProductCategory data.</returns>
        public GetProductCategoryModelView updateProductCategory(long id, UpdateProductCategoryModelView modelView)
        {
            ProductCategoryRepository repository = PersistenceContext.repositories().createProductCategoryRepository();

            ProductCategory category = repository.find(id);

            string newName = modelView.name;

            //check what attributes are to be updated
            if (newName != null)
            {
                if (repository.find(newName) != null)
                {
                    throw new ArgumentException(ERROR_DUPLICATE_NAME);
                }

                if (!category.changeName(newName))
                {
                    throw new ArgumentException(ERROR_INVALID_NAME);
                }
            }

            category = repository.update(category);

            return ProductCategoryModelViewService.fromEntity(category);
        }
    }
}