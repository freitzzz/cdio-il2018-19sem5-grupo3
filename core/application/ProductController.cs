using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.services;
using support.dto;
using support.utils;

namespace core.application {
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController {
        /// <summary>
        /// Constant that represents the message that occures if the materials being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_MATERIALS_FETCH = "The materials being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the components being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_COMPONENTS_FETCH = "The components being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the products being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_PRODUCTS_FETCH = "The products being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the dimensions being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_DIMENSIONS_FETCH = "The dimensions being fetched are invalid";

        /// <summary>
        /// Builds a new ProductController
        /// </summary>
        public ProductController() { }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="productAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public ProductDTO addProduct(ProductDTO productAsDTO) {
            Product newProduct = new ProductDTOService().transform(productAsDTO);
            Product createdProduct = PersistenceContext.repositories().createProductRepository().save(newProduct);
            if (createdProduct == null) return null;
            return createdProduct.toDTO();
        }

        /// <summary>
        /// Updates basic information of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductBasicInformation(UpdateProductDTO updateProductDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated = productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;
            if (updateProductDTO.reference != null) {
                updatedWithSuccess &= productBeingUpdated.changeProductReference(updateProductDTO.reference);
                perfomedAtLeastOneUpdate = true;
            }
            if (updateProductDTO.designation != null) {
                updatedWithSuccess &= productBeingUpdated.changeProductDesignation(updateProductDTO.designation);
                perfomedAtLeastOneUpdate = true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess &= productRepository.update(productBeingUpdated) != null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Updates the materials of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductMaterials(UpdateProductDTO updateProductDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Product productBeingUpdated = productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            if (updateProductDTO.materialsToAdd != null) {
                IEnumerable<Material> materialsBeingAdded = materialRepository.getMaterialsByIDS(updateProductDTO.materialsToAdd);
                ensureMaterialsFetchWasSuccessful(updateProductDTO.materialsToAdd, materialsBeingAdded);
                foreach (Material material in materialsBeingAdded)
                    updatedWithSuccess &= productBeingUpdated.addMaterial(material);
                perfomedAtLeastOneUpdate = true;
            }

            if (!updatedWithSuccess) return false;

            if (updateProductDTO.materialsToRemove != null) {
                IEnumerable<Material> materialsBeingRemoved = materialRepository.getMaterialsByIDS(updateProductDTO.materialsToRemove);
                ensureMaterialsFetchWasSuccessful(updateProductDTO.materialsToRemove, materialsBeingRemoved);
                foreach (Material material in materialsBeingRemoved)
                    updatedWithSuccess &= productBeingUpdated.removeMaterial(material);
                perfomedAtLeastOneUpdate = true;
            }

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= productRepository.update(productBeingUpdated) != null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Updates the components of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductComponents(UpdateProductDTO updateProductDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated = productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            if (updateProductDTO.componentsToAdd != null) {
                IEnumerable<ProductDTO> productsDTO = extractProductsDTOFromComponentsDTO(updateProductDTO.componentsToAdd);
                IEnumerable<Product> complementedProducts = PersistenceContext.repositories().createProductRepository().fetchProductsByID(productsDTO);
                ensureProductsFetchWasSuccesful(productsDTO, complementedProducts);
                foreach (Product complementedProduct in complementedProducts)
                    updatedWithSuccess &= productBeingUpdated.addComplementedProduct(complementedProduct);
                perfomedAtLeastOneUpdate = true;
            }

            if (!updatedWithSuccess) return false;


            if (updateProductDTO.componentsToRemove != null) {
                IEnumerable<ProductDTO> productsDTO = extractProductsDTOFromComponentsDTO(updateProductDTO.componentsToRemove);
                IEnumerable<Product> complementedProducts = PersistenceContext.repositories().createProductRepository().fetchProductsByID(productsDTO);
                ensureProductsFetchWasSuccesful(productsDTO, complementedProducts);
                foreach (Product complementedProduct in complementedProducts)
                    updatedWithSuccess &= productBeingUpdated.removeComplementedProduct(complementedProduct);
                perfomedAtLeastOneUpdate = true;
            }

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= productRepository.update(productBeingUpdated) != null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductDimensions(UpdateProductDTO updateProductDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated = productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;
            if (updateProductDTO.dimensionsToAdd != null && updateProductDTO.dimensionsToAdd.widthDimensionDTOs != null) {
                IEnumerable<Dimension> widthDimensionsBeingAdded = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.widthDimensionDTOs);
                foreach (Dimension widthDimension in widthDimensionsBeingAdded)
                    updatedWithSuccess &= productBeingUpdated.addWidthDimension(widthDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToAdd != null && updateProductDTO.dimensionsToAdd.heightDimensionDTOs != null) {
                IEnumerable<Dimension> heightDimensionsBeingAdded = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.heightDimensionDTOs);
                foreach (Dimension heightDimension in heightDimensionsBeingAdded)
                    updatedWithSuccess &= productBeingUpdated.addHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToAdd != null && updateProductDTO.dimensionsToAdd.depthDimensionDTOs != null) {
                IEnumerable<Dimension> depthDimensionsBeingAdded = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.depthDimensionDTOs);
                foreach (Dimension depthDimension in depthDimensionsBeingAdded)
                    updatedWithSuccess &= productBeingUpdated.addDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove != null && updateProductDTO.dimensionsToRemove.widthDimensionDTOs != null) {
                IEnumerable<Dimension> widthDimensionsBeingRemoved = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.widthDimensionDTOs);
                if (widthDimensionsBeingRemoved != null)
                    foreach (Dimension widthDimension in widthDimensionsBeingRemoved)
                        updatedWithSuccess &= productBeingUpdated.removeWidthDimension(widthDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove != null && updateProductDTO.dimensionsToRemove.heightDimensionDTOs != null) {
                IEnumerable<Dimension> heightDimensionsBeingRemoved = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.heightDimensionDTOs);
                if (heightDimensionsBeingRemoved != null)
                    foreach (Dimension heightDimension in heightDimensionsBeingRemoved)
                        updatedWithSuccess &= productBeingUpdated.removeHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove != null && updateProductDTO.dimensionsToRemove.depthDimensionDTOs != null) {
                IEnumerable<Dimension> depthDimensionsBeingRemoved = DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.depthDimensionDTOs);
                if (depthDimensionsBeingRemoved != null)
                    foreach (Dimension depthDimension in depthDimensionsBeingRemoved)
                        updatedWithSuccess &= productBeingUpdated.removeDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate = true;
                if (!updatedWithSuccess) return false;
            }

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= productRepository.update(productBeingUpdated) != null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Updates the category of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductCategory(UpdateProductDTO updateProductDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated = productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;
            if (updateProductDTO.productCategoryToUpdate != null) {
                ProductCategory productCategory = PersistenceContext.repositories().createProductCategoryRepository().find(updateProductDTO.productCategoryToUpdate.id);
                updatedWithSuccess &= productBeingUpdated.changeProductCategory(productCategory);
                perfomedAtLeastOneUpdate = true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess &= productRepository.update(productBeingUpdated) != null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product data being disabled</param>
        /// <returns>boolean true if the product was disabled with success, false if not</returns>
        public bool disableProduct(ProductDTO productDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingDisabled = productRepository.find(productDTO.id);
            return productBeingDisabled != null && productBeingDisabled.disable() && productRepository.update(productBeingDisabled) != null;
        }

        /// <summary>
        /// Removes (Disables) a product
        /// </summary>
        /// <param name="productDTO">DTO with the product information</param>
        /// <returns>boolean true if the product was removed (disabled) with success</returns>
        public bool removeProduct(ProductDTO productDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product productBeingRemoved = productRepository.find(productDTO.id);
            return productBeingRemoved != null && productBeingRemoved.disable() && productRepository.update(productBeingRemoved) != null;
        }

        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<ProductDTO> findAllProducts() {
            List<ProductDTO> productDTOList = new List<ProductDTO>();

            IEnumerable<Product> productList = PersistenceContext.repositories().createProductRepository().findAll();

            if (productList == null || !productList.GetEnumerator().MoveNext()) {
                return null;
            }

            foreach (Product product in productList) {
                productDTOList.Add(product.toDTO());
            }

            return productDTOList;
        }

        /// <summary>
        /// Returns a product which has a certain persistence id
        /// </summary>
        /// <param name="productDTO">FetchProductDTO with the product fetch information</param>
        /// <returns>ProductDTO with the product which has a certain persistence id</returns>
        public ProductDTO findProductByID(FetchProductDTO fetchProductDTO) {
            return PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.id).toDTO(fetchProductDTO.productDTOOptions);
        }

        /// <summary>
        /// Returns a product which has a certain reference
        /// </summary>
        /// <param name="productDTO">FetchProductDTO with the product fetch information</param>
        /// <returns>ProductDTO with the product which has a certain reference</returns>
        public ProductDTO findByReference(FetchProductDTO fetchProductDTO) {
            return PersistenceContext.repositories().createProductRepository().find(fetchProductDTO.reference).toDTO(fetchProductDTO.productDTOOptions);
        }

        public bool defineProductDimensions(ProductDTO productDTO) {
            ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
            Product product = productRepository.find(productDTO.id);

            IEnumerable<Dimension> heightDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> widthDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> depthDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.depthDimensionDTOs);

            foreach (Dimension heightDimension in heightDimensions) { if (!product.addHeightDimension(heightDimension)) return false; }
            foreach (Dimension widthDimension in widthDimensions) { if (!product.addWidthDimension(widthDimension)) return false; }
            foreach (Dimension depthDimension in depthDimensions) { if (!product.addDepthDimension(depthDimension)) return false; }

            return productRepository.save(product) != null;
        }

        /// <summary>
        /// Adds a restriction to a product's component and returns the restriction's algorithm list of inputs
        /// </summary>
        /// <param name="productID">product's id</param>
        /// <param name="productComponentID">product's component id</param>
        /// <param name="restDTO">Data Transfer Object of the restriction to add</param>
        /// <returns>list of inputs for the restriction's algorithm</returns>
        public RestrictionDTO addComponentRestriction(long productID, long productComponentID, RestrictionDTO restDTO) {
            if (Collections.isEnumerableNullOrEmpty(restDTO.inputs)) {
                //gets required list of inputs for the algorithm
                List<Input> inputs = new AlgorithmFactory().createAlgorithm(restDTO.algorithm).getRequiredInputs();
                //if the algorithm did not need any inputs then persist
                if (Collections.isEnumerableNullOrEmpty(inputs)) {
                    ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
                    Product product = productRepository.find(productID);
                    Product component = productRepository.find(productComponentID);
                    Restriction restriction = restDTO.toEntity();
                    product.addComponentRestriction(component, restriction);
                    productRepository.update(product);
                    return restDTO;
                }
                List<InputDTO> inputDTOs = (List<InputDTO>)DTOUtils.parseToDTOS(inputs);
                RestrictionDTO restrictionDTO = new RestrictionDTO();
                restrictionDTO.algorithm = restDTO.algorithm;
                restrictionDTO.description = restDTO.description;
                restrictionDTO.inputs = inputDTOs;
                return restrictionDTO;
            } else {
                ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
                Product product = productRepository.find(productID);
                Product component = productRepository.find(productComponentID);
                List<Input> inputs = new List<Input>(DTOUtils.reverseDTOS(restDTO.inputs));
                if (new AlgorithmFactory().createAlgorithm(restDTO.algorithm).isWithinDataRange(inputs)) {
                    Restriction restriction = restDTO.toEntity();
                    product.addComponentRestriction(component, restriction);
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
        internal IEnumerable<Dimension> getProductDTOEnumerableDimensions(List<DimensionDTO> dimensionsDTOs) {
            List<Dimension> dimensions = new List<Dimension>();
            foreach (DimensionDTO dimensionDTO in dimensionsDTOs) {
                if (dimensionDTO.GetType() == typeof(DiscreteDimensionIntervalDTO)) {
                    DiscreteDimensionIntervalDTO ddiDTO = (DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval = new DiscreteDimensionInterval(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if (dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO)) {
                    ContinuousDimensionIntervalDTO cdiDTO = (ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval = new ContinuousDimensionInterval(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else if (dimensionDTO.GetType() == typeof(SingleValueDimensionDTO)) {
                    SingleValueDimensionDTO svdDTO = (SingleValueDimensionDTO)dimensionDTO;
                    SingleValueDimension dimensionValue = new SingleValueDimension(svdDTO.value);
                    dimensions.Add(dimensionValue);
                }
            }
            return dimensions;
        }

        /// <summary>
        /// Adds new materials to updated product
        /// </summary>
        /// <param name="productDTO">list of updates in DTO</param>
        /// <param name="oldProduct">product to be updated</param>
        /// TODO:REFACTOR THIS 
        /* private void addMaterials(ProductDTO productDTO, Product oldProduct) {
            foreach (ProductMaterialDTO prodMDTO in productDTO.productMaterials) {
                long matID = prodMDTO.material.id;
                oldProduct.addMaterial(materialRepository.find(matID));
            }
        } */

        /// <summary>
        /// Parses an enumerable of products persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="productsIDS">Enumerable with the products persistence identifiers</param>
        /// <returns>IEnumerable with the products ids as entities</returns>
        internal IEnumerable<Product> enumerableOfProductsIDSAsEntities(IEnumerable<long> productsIDS) {
            if (productsIDS == null) return null;
            List<Product> products = new List<Product>();
            IEnumerator<long> productsIDSIterator = productsIDS.GetEnumerator();
            long nextProductID = productsIDSIterator.Current;
            while (productsIDSIterator.MoveNext()) {
                nextProductID = productsIDSIterator.Current;
                products.Add(PersistenceContext.repositories().createProductRepository().find(nextProductID));
            }
            return products;
        }

        /// <summary>
        /// Extracts an enumerable of products dto from an enumerable of components dto
        /// </summary>
        /// <param name="componentDTO">IEnumerable with the components dto</param>
        /// <returns>IEnumerable with the extracted products dto</returns>
        private IEnumerable<ProductDTO> extractProductsDTOFromComponentsDTO(IEnumerable<ComponentDTO> componentsDTO) {
            List<ProductDTO> productsDTO = new List<ProductDTO>();
            foreach (ComponentDTO componentDTO in componentsDTO) productsDTO.Add(componentDTO.product);
            return productsDTO;
        }

        /// <summary>
        /// Ensures that the materials fetch was successful
        /// </summary>
        /// <param name="materialsToFetch">IEnumerable with the materials dtos to fetch</param>
        /// <param name="fetchedMaterials">IEnumerable with the fetched materials</param>
        private void ensureMaterialsFetchWasSuccessful(IEnumerable<MaterialDTO> materialsToFetch, IEnumerable<Material> fetchedMaterials) {
            if (Collections.isEnumerableNullOrEmpty(materialsToFetch) || Collections.getEnumerableSize(materialsToFetch) != Collections.getEnumerableSize(fetchedMaterials))
                throw new InvalidOperationException(INVALID_MATERIALS_FETCH);
        }

        /// <summary>
        /// Ensures that the produts components fetch was successful
        /// </summary>
        /// <param name="componentsToFetch">IEnumerable with the components dtos to fetch</param>
        /// <param name="fetchedComponents">IEnumerable with the fetched components</param>
        private void ensureProductsComponentsFetchWasSuccesful(IEnumerable<ComponentDTO> componentsToFetch, IEnumerable<Component> fetchedComponents) {
            if (Collections.isEnumerableNullOrEmpty(componentsToFetch) || Collections.getEnumerableSize(componentsToFetch) != Collections.getEnumerableSize(fetchedComponents))
                throw new InvalidOperationException(INVALID_COMPONENTS_FETCH);
        }

        /// <summary>
        /// Ensures that the products fetch was successful
        /// </summary>
        /// <param name="productsToFetch">IEnumerable with the products dtos to fetch</param>
        /// <param name="fetchedProducts">IEnumerable with the fetched products</param>
        private void ensureProductsFetchWasSuccesful(IEnumerable<ProductDTO> productsToFetch, IEnumerable<Product> fetchedProducts) {
            if (Collections.isEnumerableNullOrEmpty(productsToFetch) || Collections.getEnumerableSize(productsToFetch) != Collections.getEnumerableSize(fetchedProducts))
                throw new InvalidOperationException(INVALID_PRODUCTS_FETCH);
        }

        /// <summary>
        /// Ensures that the products dimensions fetch was successful
        /// </summary>
        /// <param name="dimensionsToFetch">IEnumerable with the dimensions dtos to fetch</param>
        /// <param name="fetchedDimensions">IEnumerable with the fetched dimensions</param>
        private void ensureProductsDimensionsFetchWasSuccesful(IEnumerable<DimensionDTO> dimensionsToFetch, IEnumerable<Dimension> fetchedDimensions) {
            if (Collections.isEnumerableNullOrEmpty(dimensionsToFetch) || Collections.getEnumerableSize(dimensionsToFetch) != Collections.getEnumerableSize(fetchedDimensions))
                throw new InvalidOperationException(INVALID_DIMENSIONS_FETCH);
        }
    }

}