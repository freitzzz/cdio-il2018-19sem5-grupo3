using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.modelview.component;
using core.modelview.material;
using core.modelview.product;
using core.modelview.restriction;
using core.services;
using support.dto;
using support.utils;
using core.services.ensurance;
using core.modelview.measurement;
using System.Linq;
using core.exceptions;
using core.modelview.productmaterial;

namespace core.application
{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController{
        /// <summary>
        /// Constant that represents the message that occurs if the user does not provide inputs
        /// </summary>
        private const string LIST_OF_INPUTS_MISSING = "The selected algorithm requires inputs!";
        /// <summary>
        /// Constant that represents error message presented when no instances of Product are found.
        /// </summary>
        private const string ERROR_NO_PRODUCTS_FOUND = "No products found.";
        /// <summary>
        /// Constant representing the message presented when no Product is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID = "Unable to find a product with an identifier of: {0}";
        /// <summary>
        /// Constant representing the message presented when no Product is found with a given reference.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_PRODUCT_BY_REFERENCE = "Unable to find a product with a reference of: {0}";
        /// <summary>
        /// Constant representing the message presented when no ProductCategory is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_CATEGORY_BY_ID = "Unable to find a category with an identifier of: {0}";
        /// <summary>
        /// Constant representing the message presented when no Material is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID = "Unable to find a material with an identifier of: {0}";
        /// <summary>
        /// Constant representing the message presented when no Measurement is found with a given identifier.
        /// </summary>
        /// <value></value>
        private const string ERROR_UNABLE_TO_FIND_MEASUREMENT_BY_ID = "Unable to find dimensions with an identifier of: {0}";
        /// <summary>
        /// Constant representing the message presented when no Restriction is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_RESTRICTION_BY_ID = "Unable to find restrictions with an identifier of: {0}";
        /// <summary>
        /// Constant representing the message presented when no Components are found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_COMPONENTS = "Unable to find components.";
        /// <summary>
        /// Cosntant representing the message presented when no Restrictions are found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_RESTRICTIONS = "Unable to find restrictions";
        /// <summary>
        /// Constant representing the message presented when the new Product could not be saved.
        /// </summary>
        private const string ERROR_UNABLE_TO_SAVE_PRODUCT = "Unable to save the product, make sure the reference is unique.";
        /// <summary>
        /// Constant representing the message presented when an instance of Product could not be updated. 
        /// </summary>
        private const string ERROR_UNABLE_TO_UPDATE_PRODUCT = "Unable to update the product, make sure the reference is unique.";
        /// <summary>
        /// Constant representing the message presented when none of the Product's properties are updated.
        /// </summary>
        private const string ERROR_NO_UPDATE_PERFORMED = "The request did not perform any update.";

        /// <summary>
        /// Builds a new ProductController
        /// </summary>
        public ProductController(){ }

        /// <summary>
        /// Retrieves a Collection of all the Products in the Product Repository.
        /// </summary>
        /// <returns>An instance of GetAllProductsModelView with all the Products.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instance of Product exists in the repository.</exception>
        public GetAllProductsModelView findAllProducts(){

            IEnumerable<Product> products = PersistenceContext.repositories().createProductRepository().findAll();

            if(!products.Any()){
                throw new ResourceNotFoundException(ERROR_NO_PRODUCTS_FOUND);
            }

            return ProductModelViewService.fromCollection(products);
        }

        /// <summary>
        /// Retrieves a Collection of all the base Products in the Product Repository.
        /// A base Product is a Product that is not owned by any other Product.
        /// </summary>
        /// <returns>An instance of GetAllProductsModelView with all the base Products.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instance of Product exists in the repository.</exception>
        public GetAllProductsModelView findBaseProducts(){
            IEnumerable<Product> baseProducts = PersistenceContext.repositories().createProductRepository().findBaseProducts();

            if(!baseProducts.Any()){
                throw new ResourceNotFoundException(ERROR_NO_PRODUCTS_FOUND);
            }

            return ProductModelViewService.fromCollection(baseProducts);
        }

        /// <summary>
        /// Finds a Product by an identifier, be it a business identifier or a persistence identifier.
        /// </summary>
        /// <param name="fetchProductDTO">DTO containing information used for querying and data conversion.</param>
        /// <returns>An instance of GetProductModelView with the Product's information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product is not found.</exception>
        public GetProductModelView findProduct(FetchProductDTO fetchProductDTO){

            Product product = null;

            //if no reference value is specified, search by id
            if(Strings.isNullOrEmpty(fetchProductDTO.reference)){
                product = PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.id);

                if(product == null){
                    throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, fetchProductDTO.id));
                }
            }
            else{
                product = PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.reference);

                if(product == null){
                    throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_REFERENCE, fetchProductDTO.reference));
                }
            }

            return ProductModelViewService.fromEntity(product, fetchProductDTO.productDTOOptions.requiredUnit);
        }

        /// <summary>
        /// Finds a Product's Collection of Measurement.
        /// </summary>
        /// <param name="fetchProductDTO">DTO containing information used for querying and data conversion.</param>
        /// <returns>GetAllMeasurementsModelView with all of the elements in the Product's Collection of Measurement.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product could not be found.</exception>
        public GetAllMeasurementsModelView findProductMeasurements(FetchProductDTO fetchProductDTO){
            
            Product product = PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.id);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, fetchProductDTO.id));
            }

            //allow unit conversion
            return MeasurementModelViewService.fromCollection(product.productMeasurements.Select(pm => pm.measurement), fetchProductDTO.productDTOOptions.requiredUnit);
        }

        /// <summary>
        /// Finds a Product's collection of Component.
        /// </summary>
        /// <param name="fetchProductDTO">DTO containing information used for querying.</param>
        /// <returns>GetAllComponentsModelView with all of the elements in the Product's Collection of Component.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product could not be found.</exception>
        public GetAllComponentsModelView findProductComponents(FetchProductDTO fetchProductDTO){
            
            Product product = PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.id);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, fetchProductDTO.id));
            }

            //if no components are found, throw an exception so that a 404 code is sent
            if(!product.components.Any()){
                throw new ResourceNotFoundException(ERROR_UNABLE_TO_FIND_COMPONENTS);
            }

            return ComponentModelViewService.fromCollection(product.components);
        }

        /// <summary>
        /// Finds a Product's Collection of Material.
        /// </summary>
        /// <param name="fetchProductDTO">DTO containing information used for querying.</param>
        /// <returns>GetAllMaterialsModelView with all of the elements in the Product's Collection of Material.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product could not be found.</exception>
        public GetAllMaterialsModelView findProductMaterials(FetchProductDTO fetchProductDTO){
            Product product = PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.id);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, fetchProductDTO.id));
            }
            return MaterialModelViewService.fromCollection(product.productMaterials.Select(pm => pm.material));
        }

        /// <summary>
        /// Finds a Product's Measurement's Collection of Restriction.
        /// </summary>
        /// <param name="productMeasurementModelView">GetProductMeasurementModelView with the Product's and the Measurement's persistence identifier.</param>
        /// <returns>An instance of GetAllRestrictionsModelView containing the information of all the Measurement's restrictions.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when either the Product or the Measurement could not be found.</exception>
        public GetAllRestrictionsModelView findMeasurementRestrictions(GetMeasurementModelView productMeasurementModelView){
            
            Product product = PersistenceContext.repositories().createProductRepository().find(productMeasurementModelView.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, productMeasurementModelView.productId));
            }

            //filter through the product's measurements
            Measurement measurement = product.productMeasurements
                .Where(productMeasurement => productMeasurement.measurementId == productMeasurementModelView.measurementId)
                    .Select(productMeasurement => productMeasurement.measurement).SingleOrDefault();
            
            if(measurement == null){
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_MEASUREMENT_BY_ID, productMeasurementModelView.measurementId)
                    );
            }

            //if no restrictions are found, throw an exception so that a 404 code is sent
            if(!measurement.restrictions.Any()){
                throw new ResourceNotFoundException(ERROR_UNABLE_TO_FIND_RESTRICTIONS);
            }

            return RestrictionModelViewService.fromCollection(measurement.restrictions);
        }

        /// <summary>
        /// Finds a Product's Component's Collection of Restriction.
        /// </summary>
        /// <param name="componentModelView">GetComponentModelView with the parent and child Products' persistence identifiers.</param>
        /// <returns>An instance of GetAllRestrictionsModelView containing the information of all the Component's restrictions.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when either of the Products could not be found.</exception>
        public GetAllRestrictionsModelView findComponentRestrictions(GetComponentModelView componentModelView){
            Product parentProduct = PersistenceContext.repositories().createProductRepository().find(componentModelView.fatherProductId);

            if(parentProduct == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, componentModelView.fatherProductId));
            }

            Component component = parentProduct.components.Where(c => c.complementaryProductId == componentModelView.id).SingleOrDefault();

            if(component == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, componentModelView.id));
            }

            //if no restrictions are found, throw an exception so that a 404 code is sent
            if(!component.restrictions.Any()){
                throw new ResourceNotFoundException(ERROR_UNABLE_TO_FIND_RESTRICTIONS);
            }

            return RestrictionModelViewService.fromCollection(component.restrictions);
        }

        /// <summary>
        /// Finds a Product's Material's Collection of Restriction.
        /// </summary>
        /// <param name="productMaterialModelView">GetProductMaterialModelView with the Product and Material persistence identifiers.</param>
        /// <returns>An instance of GetAllRestrictionsModelView containing the information of all the Materials's restrictions.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when either the Product or the Material could not be found.</exception>
        public GetAllRestrictionsModelView findMaterialRestrictions(GetProductMaterialModelView productMaterialModelView){
            Product product = PersistenceContext.repositories().createProductRepository().find(productMaterialModelView.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, productMaterialModelView.productId));
            }

            ProductMaterial productMaterial = product.productMaterials
                .Where(pm => pm.materialId == productMaterialModelView.id).SingleOrDefault();

            if(productMaterial == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID, productMaterialModelView.id));
            }

            //if no restrictions are found, throw an exception so that a 404 code is sent
            if(!productMaterial.restrictions.Any()){
                throw new ResourceNotFoundException(ERROR_UNABLE_TO_FIND_RESTRICTIONS);
            }

            return RestrictionModelViewService.fromCollection(productMaterial.restrictions);
        }

        /// <summary>
        /// Adds a new instance of Product.
        /// </summary>
        /// <param name="addProductMV">AddProductModelView with the product information</param>
        /// <returns>GetProductModelView with the created product, null if the product was not created</returns>
        public GetProductModelView addProduct(AddProductModelView addProductMV){
            Product newProduct= CreateProductService.create(addProductMV);
            newProduct = PersistenceContext.repositories().createProductRepository().save(newProduct);
            //an entity will be null after save if an equal entity was found in the repository
            if(newProduct == null){
                throw new ArgumentException(ERROR_UNABLE_TO_SAVE_PRODUCT);
            }

            return ProductModelViewService.fromEntity(newProduct);
        }

        /// <summary>
        /// Adds a Measurement to a Product.
        /// </summary>
        /// <param name="addMeasurementToProductMV">AddMeasurementToProductModelView containing the data of the Measurement instance being added.</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product could not be found.</exception>
        public GetProductModelView addMeasurementToProduct(AddMeasurementModelView addMeasurementToProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product product = PersistenceContext.repositories().createProductRepository().find(addMeasurementToProductMV.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addMeasurementToProductMV.productId));
            }

            Measurement measurement = MeasurementModelViewService.fromModelView(addMeasurementToProductMV);

            product.addMeasurement(measurement);

            product = productRepository.update(product);

            return ProductModelViewService.fromEntity(product);
        }


        /// <summary>
        /// Adds a complementary Product to a Product.
        /// </summary>
        /// <param name="addComponentToProductMV">AddComponentToProductModelView containing the data of the complementary Product being added.</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        /// <exception cref="ResourceNotFoundException">Throw when either of the Products could not be found.</exception>
        public GetProductModelView addComponentToProduct(AddComponentModelView addComponentToProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddComponent=productRepository.find(addComponentToProductMV.fatherProductId);

            if(productToAddComponent == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addComponentToProductMV.fatherProductId));
            }

            Product componentBeingAdded=productRepository.find(addComponentToProductMV.childProductId);

            if(componentBeingAdded == null){
               throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addComponentToProductMV.childProductId));
            }

            if(addComponentToProductMV.mandatory){
                productToAddComponent.addMandatoryComplementaryProduct(componentBeingAdded);
            }else{
                productToAddComponent.addComplementaryProduct(componentBeingAdded);
            }
            
            productToAddComponent = productRepository.update(productToAddComponent);

            return ProductModelViewService.fromEntity(productToAddComponent);
        }

        /// <summary>
        /// Adds a Material to a Product.
        /// </summary>
        /// <param name="addMaterialToProductMV">AddMaterialToProductModelView with the material addition information</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        ///<exception cref="ResourceNotFoundException">Thrown when either the Product or the Material could not be found.</exception>
        public GetProductModelView addMaterialToProduct(AddProductMaterialModelView addMaterialToProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddMaterial=productRepository.find(addMaterialToProductMV.productId);

            if(productToAddMaterial == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addMaterialToProductMV.productId));
            }

            Material materialBeingAdded=PersistenceContext.repositories().createMaterialRepository().find(addMaterialToProductMV.materialId);

            if(materialBeingAdded == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID, addMaterialToProductMV.materialId));
            }
            
            productToAddMaterial.addMaterial(materialBeingAdded);
            
            productToAddMaterial = productRepository.update(productToAddMaterial);

            return ProductModelViewService.fromEntity(productToAddMaterial);
        }

        /// <summary>
        /// Adds a Restriction to a Product's Measurement.
        /// </summary>
        /// <param name="addRestrictionToProductMeasurementMV">AddRestrictionToProductMeasurementModelView with the Product's and Measurement's persistence identifiers
        /// as well as the Restriction's data.</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when either the Product or the Measurement could not be found.</exception>
        public GetProductModelView addRestrictionToProductMeasurement(AddMeasurementRestrictionModelView addRestrictionToProductMeasurementMV){
            
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
        
            Product product = productRepository.find(addRestrictionToProductMeasurementMV.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addRestrictionToProductMeasurementMV.productId));
            }

            Measurement measurement = product.productMeasurements
                .Where(pm => pm.measurementId == addRestrictionToProductMeasurementMV.measurementId).Select(pm => pm.measurement).SingleOrDefault();

            if(measurement == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MEASUREMENT_BY_ID, addRestrictionToProductMeasurementMV.measurementId));
            }

            //TODO: remove this check from here
            if(Collections.isEnumerableNullOrEmpty(addRestrictionToProductMeasurementMV.restriction.inputs)){
                throw new ArgumentException();
            }

            Restriction restriction = addRestrictionToProductMeasurementMV.restriction.toEntity();

            product.addMeasurementRestriction(measurement, restriction);

            product = productRepository.update(product);

            return ProductModelViewService.fromEntity(product);
        }

        /// <summary>
        /// Adds a Restriction to a Product's complementary Product.
        /// </summary>
        /// <param name="addRestrictionToProductComponentMV">AddRestrictionToProductComponentModelView containing the data of the Restriction instance being added.</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when either of the Products could not be found.</exception>
        public GetProductModelView addRestrictionToProductComponent(AddComponentRestrictionModelView addRestrictionToProductComponentMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product parentProduct=productRepository.find(addRestrictionToProductComponentMV.fatherProductId);

            if(parentProduct == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addRestrictionToProductComponentMV.fatherProductId));
            }

            //filter product's components rather than accessing the repository
            Product childProduct = parentProduct.components
                .Where(component => component.complementaryProduct.Id == addRestrictionToProductComponentMV.childProductId)
                .Select(component => component.complementaryProduct).SingleOrDefault();

            if(childProduct == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addRestrictionToProductComponentMV.childProductId));
            }

            //TODO: remove this check from here
            if(Collections.isEnumerableNullOrEmpty(addRestrictionToProductComponentMV.restriction.inputs)){
                throw new ArgumentException();
            }
            Restriction restriction = addRestrictionToProductComponentMV.restriction.toEntity();
            parentProduct.addComplementaryProductRestriction(childProduct, restriction);
            parentProduct = productRepository.update(parentProduct);
            return ProductModelViewService.fromEntity(parentProduct);
        }

        /// <summary>
        /// Adds a Restriction to a Product's Material.
        /// </summary>
        /// <param name="addRestrictionModelView">AddRestrictionToProductMaterialModelView containing the data of the Restriction instance being added.</param>
        /// <returns>GetProductModelView with updated Product information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product or the Material could not be found.</exception>
        public GetProductModelView addRestrictionToProductMaterial(AddProductMaterialRestrictionModelView addRestrictionModelView){
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product product = productRepository.find(addRestrictionModelView.productId);
        
            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, addRestrictionModelView.productId));
            }

            //filter product's materials rather than accessing the repository
            Material material = product.productMaterials
                .Where(productMaterial => productMaterial.materialId == addRestrictionModelView.materialId)
                .Select(productMaterial => productMaterial.material).SingleOrDefault();

            if(material == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID, addRestrictionModelView.materialId));
            }

            //TODO: remove this check from here
            if(Collections.isEnumerableNullOrEmpty(addRestrictionModelView.restriction.inputs)){
                throw new ArgumentException();
            }

            Restriction restriction = addRestrictionModelView.restriction.toEntity();

            product.addMaterialRestriction(material, restriction);
            product = productRepository.update(product);
            return ProductModelViewService.fromEntity(product);
        }

        /// <summary>
        /// Updates the properties of a product
        /// </summary>
        /// <param name="updateProductPropertiesModelView">UpdateProductPropertiesModelView with the data regarding the product update</param>
        /// <returns>GetProductModelView with the updated product information</returns>
        public GetProductModelView updateProductProperties(UpdateProductPropertiesModelView updateProductPropertiesModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductPropertiesModelView.id);

            if(productBeingUpdated == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, updateProductPropertiesModelView.id));
            }

            //FetchEnsurance.ensureProductFetchWasSuccessful(productBeingUpdated);
            bool perfomedAtLeastOneUpdate=false;
            
            if(updateProductPropertiesModelView.reference!=null){
                productBeingUpdated.changeProductReference(updateProductPropertiesModelView.reference);
                perfomedAtLeastOneUpdate=true;
            }
            
            if(updateProductPropertiesModelView.designation!=null){
                productBeingUpdated.changeProductDesignation(updateProductPropertiesModelView.designation);
                perfomedAtLeastOneUpdate=true;
            }

            if(updateProductPropertiesModelView.modelFilename!=null){
                productBeingUpdated.changeModelFilename(updateProductPropertiesModelView.modelFilename);
                perfomedAtLeastOneUpdate=true;
            }
            
            if(updateProductPropertiesModelView.categoryId.HasValue){
                ProductCategory categoryToUpdate=PersistenceContext.repositories().createProductCategoryRepository().find(updateProductPropertiesModelView.categoryId.Value);
                
                if(categoryToUpdate == null){
                    throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_CATEGORY_BY_ID, updateProductPropertiesModelView.categoryId.Value));
                }

                productBeingUpdated.changeProductCategory(categoryToUpdate);
                perfomedAtLeastOneUpdate=true;
            }


            //?Should an error be thrown if no update was performed or should it just carry on?
            //UpdateEnsurance.ensureAtLeastOneUpdateWasPerformed(perfomedAtLeastOneUpdate);
            if(!perfomedAtLeastOneUpdate){
                throw new ArgumentException(ERROR_NO_UPDATE_PERFORMED);
            }

            productBeingUpdated=productRepository.update(productBeingUpdated);

            //the updated product will only be null, if the reference was changed to match a previosuly added product
            if(productBeingUpdated == null){
                throw new ArgumentException(ERROR_UNABLE_TO_UPDATE_PRODUCT);
            }

            //UpdateEnsurance.ensureProductUpdateWasSuccessful(productBeingUpdated);
            return ProductModelViewService.fromEntity(productBeingUpdated);
        }

        /// <summary>
        /// Disables a Product.
        /// </summary>
        /// <param name="deleteProductMV">DeleteProductModelView with the Product data being disabled.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product is not found.</exception>
        public void disableProduct(DeleteProductModelView deleteProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingDisabled=productRepository.find(deleteProductMV.productId);

            if(productBeingDisabled == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteProductMV.productId));
            }

            productRepository.remove(productBeingDisabled);
        }

        /// <summary>
        /// Deletes an instance of Measurement from a Product.
        /// </summary>
        /// <param name="deleteMeasurementModelView">DeleteMeasurementFromProductModelView with the Product's and the Measurement's persistence identifiers.</param>
        /// <exception cref="ResourceNotFoundException">Throw when either the Product or the Measurement could not be found.</exception>
        public void deleteMeasurementFromProduct(DeleteMeasurementModelView deleteMeasurementModelView){
            Product product = PersistenceContext.repositories().createProductRepository().find(deleteMeasurementModelView.productId);
    
            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteMeasurementModelView.productId));
            }

            Measurement measurement = product.productMeasurements.Where(pm => pm.measurementId == deleteMeasurementModelView.measurementId)
                .Select(pm => pm.measurement).SingleOrDefault();

            if(measurement == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MEASUREMENT_BY_ID, deleteMeasurementModelView.measurementId));
            }

            product.removeMeasurement(measurement);
        }
        
        /// <summary>
        /// Deletes a Product's complementary Product.
        /// </summary>
        /// <param name="deleteComponentFromProductMV">DeleteComponentFromProductDTO with the component deletion information<</param>
        ///<exception cref="ResourceNotFoundException">Thrown when either of the Products could not be found.</exception>
        public void deleteComponentFromProduct(DeleteComponentModelView deleteComponentFromProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveComponent=productRepository.find(deleteComponentFromProductMV.fatherProductId);

            if(productToRemoveComponent == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteComponentFromProductMV.fatherProductId));
            }

            //filter product's components rather than accessing the repository
            Product productBeingDeleted = productToRemoveComponent.components
                .Where(component => component.complementaryProduct.Id == deleteComponentFromProductMV.childProductId)
                .Select(component => component.complementaryProduct).SingleOrDefault();

            if(productBeingDeleted == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteComponentFromProductMV.childProductId));
            }
            
            productToRemoveComponent.removeComplementaryProduct(productBeingDeleted);

            productToRemoveComponent = productRepository.update(productToRemoveComponent);
        }

        /// <summary>
        /// Deletes a Material from a Product's Collection of Material.
        /// </summary>
        /// <param name="deleteMaterialFromProductMV">DeleteMaterialFromProductDTO with the material deletion information</param>
        /// <exception cref="ResourceNotFoundException">Thrown when either Product or the Material could not be found.</exception>
        public void deleteMaterialFromProduct(DeleteProductMaterialModelView deleteMaterialFromProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveMaterial = productRepository.find(deleteMaterialFromProductMV.productId);

            if(productToRemoveMaterial == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteMaterialFromProductMV.productId));
            }

            //filter through the product's current materials
            Material materialBeingDeleted = productToRemoveMaterial.productMaterials
                .Where(productMaterial => productMaterial.materialId == deleteMaterialFromProductMV.materialId)
                .Select(productMaterial => productMaterial.material).SingleOrDefault();

            if(materialBeingDeleted == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID, deleteMaterialFromProductMV.materialId));
            }
            
            productToRemoveMaterial.removeMaterial(materialBeingDeleted);

            productToRemoveMaterial = productRepository.update(productToRemoveMaterial);
        }
        

        /// <summary>
        /// Deletes a Restriction from a Product's Measurement.
        /// </summary>
        /// <param name="deleteRestrictionFromProductMeasurementMV">DeleteRestrictionFromProductMeasurementModelView with the Product's, the Measurement's and the Restriction's persistence identifier.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product, the Measurement or the Restriction could not be found.</exception>
        public void deleteRestrictionFromProductMeasurement(DeleteMeasurementRestrictionModelView deleteRestrictionFromProductMeasurementMV){
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository(); 
            Product product = productRepository.find(deleteRestrictionFromProductMeasurementMV.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteRestrictionFromProductMeasurementMV.productId));
            }

            Measurement measurement = product.productMeasurements.Where(pm => pm.measurementId == deleteRestrictionFromProductMeasurementMV.measurementId)
                .Select(pm => pm.measurement).SingleOrDefault();

            if(measurement == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MEASUREMENT_BY_ID, deleteRestrictionFromProductMeasurementMV.measurementId));
            }

            Restriction restriction = measurement.restrictions.Where(r => r.Id == deleteRestrictionFromProductMeasurementMV.restrictionId).SingleOrDefault();

            if(restriction == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_RESTRICTION_BY_ID, deleteRestrictionFromProductMeasurementMV.restrictionId));
            }

            measurement.restrictions.Remove(restriction);
            product = productRepository.update(product);
        }


        /// <summary>
        /// Deletes an instance of Restriction from a Product's Component.
        /// </summary>
        /// <param name="deleteRestrictionFromProductComponentMV">DeleteRestrictionFromProductComponentDTO with the restriction deletion information</param>
        /// <exception cref="ResourceNotFoundException">Thrown when either of the Products or the Restriction could not be found.</exception>
        public void deleteRestrictionFromProductComponent(DeleteComponentRestrictionModelView deleteRestrictionFromProductComponentMV){
            
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithComponentBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductComponentMV.fatherProductId);

            if(productWithComponentBeingDeletedRestriction == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteRestrictionFromProductComponentMV.fatherProductId));
            }

            Component productComponentBeingDeletedRestriction=productWithComponentBeingDeletedRestriction.components
                .Where(component => component.complementaryProduct.Id == deleteRestrictionFromProductComponentMV.childProductId).SingleOrDefault();

            if(productComponentBeingDeletedRestriction == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteRestrictionFromProductComponentMV.childProductId));
            }

            Restriction restriction = productComponentBeingDeletedRestriction.restrictions
                .Where(r => r.Id == deleteRestrictionFromProductComponentMV.restrictionId).SingleOrDefault();

            if(restriction == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_RESTRICTION_BY_ID, deleteRestrictionFromProductComponentMV.restrictionId));
            }

            productComponentBeingDeletedRestriction.restrictions.Remove(restriction);

            productWithComponentBeingDeletedRestriction = productRepository.update(productWithComponentBeingDeletedRestriction);
        }
        
        /// <summary>
        /// Deletes an instance of Restriction from a Product's Material.
        /// </summary>
        /// <param name="deleteRestrictionFromProductMaterialMV">DeleteRestrictionFromProductMaterialModelView containing the Product's, the Materials's and the Restriction's persistence identifiers.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when the Product, the Material or the Restriction could not be found.</exception>
        public void deleteRestrictionFromProductMaterial(DeleteProductMaterialRestrictionModelView deleteRestrictionFromProductMaterialMV){
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();

            Product product = productRepository.find(deleteRestrictionFromProductMaterialMV.productId);

            if(product == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_PRODUCT_BY_ID, deleteRestrictionFromProductMaterialMV.productId));
            }

            ProductMaterial productMaterial = product.productMaterials.
                Where(pm => pm.materialId == deleteRestrictionFromProductMaterialMV.materialId).SingleOrDefault();

            if(productMaterial == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL_BY_ID, deleteRestrictionFromProductMaterialMV.materialId));
            }

            Restriction restriction = productMaterial.restrictions.Where(r => r.Id == deleteRestrictionFromProductMaterialMV.restrictionId).SingleOrDefault();

            if(restriction == null){
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_RESTRICTION_BY_ID, deleteRestrictionFromProductMaterialMV.restrictionId));
            }

            productMaterial.restrictions.Remove(restriction);

            product = productRepository.update(product);
        }

        /// <summary>
        /// Adds a restriction to a product's component and returns the restriction's algorithm list of inputs
        /// </summary>
        /// <param name="productID">product's id</param>
        /// <param name="productComponentID">product's component id</param>
        /// <param name="restDTO">Data Transfer Object of the restriction to add</param>
        /// <returns>list of inputs for the restriction's algorithm</returns>
        [Obsolete]
        public RestrictionDTO addComponentRestriction(long productID, long productComponentID, RestrictionDTO restDTO){
            if(Collections.isEnumerableNullOrEmpty(restDTO.inputs)){
                //gets required list of inputs for the algorithm
                List<Input> inputs=new AlgorithmFactory().createAlgorithm(restDTO.algorithm).getRequiredInputs();
                //if the algorithm did not need any inputs then persist
                if(Collections.isEnumerableNullOrEmpty(inputs)){
                    ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
                    Product product=productRepository.find(productID);
                    Product component=productRepository.find(productComponentID);
                    Restriction restriction=restDTO.toEntity();
                    product.addComplementaryProductRestriction(component, restriction);
                    productRepository.update(product);
                    return restDTO;
                }
                throw new ArgumentException(LIST_OF_INPUTS_MISSING);
            } else {
                ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
                Product product = productRepository.find(productID);
                Product component = productRepository.find(productComponentID);
                List<Input> inputs = new List<Input>(DTOUtils.reverseDTOS(restDTO.inputs));
                if(new AlgorithmFactory().createAlgorithm(restDTO.algorithm).isWithinDataRange(inputs)) {
                    Restriction restriction = restDTO.toEntity();
                    product.addComplementaryProductRestriction(component, restriction);
                    productRepository.update(product);
                }
                return restDTO;
            }
        }

        /// <summary>
        /// Returns an enumerable of dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the dimensions found on a product DTO</returns>
        [Obsolete]
        internal IEnumerable<Dimension> getProductDTOEnumerableDimensions(List<DimensionDTO> dimensionsDTOs){
            List<Dimension> dimensions=new List<Dimension>();
            foreach (DimensionDTO dimensionDTO in dimensionsDTOs){
                if(dimensionDTO.GetType() == typeof(DiscreteDimensionIntervalDTO)){
                    DiscreteDimensionIntervalDTO ddiDTO=(DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval=new DiscreteDimensionInterval(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if(dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO)){
                    ContinuousDimensionIntervalDTO cdiDTO=(ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval=new ContinuousDimensionInterval(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else if(dimensionDTO.GetType() == typeof(SingleValueDimensionDTO)){
                    SingleValueDimensionDTO svdDTO=(SingleValueDimensionDTO)dimensionDTO;
                    SingleValueDimension dimensionValue=new SingleValueDimension(svdDTO.value);
                    dimensions.Add(dimensionValue);
                }
            }
            return dimensions;
        }

        /// <summary>
        /// Extracts an enumerable of products dto from an enumerable of components dto
        /// </summary>
        /// <param name="componentDTO">IEnumerable with the components dto</param>
        /// <returns>IEnumerable with the extracted products dto</returns>
        [Obsolete]
        private IEnumerable<ProductDTO> extractProductsDTOFromComponentsDTO(IEnumerable<ComponentDTO> componentsDTO){
            List<ProductDTO> productsDTO=new List<ProductDTO>();
            foreach (ComponentDTO componentDTO in componentsDTO) productsDTO.Add(componentDTO.product);
            return productsDTO;
        }
    }

}