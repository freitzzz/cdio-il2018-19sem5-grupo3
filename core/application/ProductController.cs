using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;

namespace core.application {
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController {

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        private readonly ProductCategoryRepository productCategoryRepository;
        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository) {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="productAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public ProductDTO addProduct(ProductDTO productAsDTO) {
            Product newProduct = productAsDTO.toEntity();

            Product createdProduct = productRepository.save(newProduct);

            if (createdProduct == null) return null;
            return createdProduct.toDTO();
        }

        /// <summary>
        /// Updates the category of a product
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product dto which category is going to be changed</param>
        /// <param name="productCategoryDTO">ProductCategoryDTO</param>
        /// <returns></returns>
        public bool updateProductCategory(ProductDTO productDTO,ProductCategoryDTO productCategoryDTO){
            Product productToUpdate=productRepository.find(productDTO.id);
            if(productToUpdate==null)return false;
            ProductCategory productNewCategory=productCategoryRepository.find(productCategoryDTO.id);
            if(!productToUpdate.changeProductCategory(productNewCategory))return false;
            return productRepository.update(productToUpdate)!=null;
        }

        /// <summary>
        /// Removes (Disables) a product
        /// </summary>
        /// <param name="productDTO">DTO with the product information</param>
        /// <returns>boolean true if the product was removed (disabled) with success</returns>
        public bool removeProduct(ProductDTO productDTO) {
            Product productBeingRemoved = productRepository.find(productDTO.id);
            return productBeingRemoved != null && productBeingRemoved.disable() && productRepository.update(productBeingRemoved) != null;
        }

        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<ProductDTO> findAllProducts() {
            List<ProductDTO> productDTOList = new List<ProductDTO>();

            IEnumerable<Product> productList = productRepository.findAll();

            if (productList == null) {
                return null;
            }

            foreach (Product product in productList) {
                productDTOList.Add(product.toDTO());
            }

            return productDTOList;
        }

        /// <summary>
        /// Fetches a product from the product repository given an ID
        /// </summary>
        /// <param name="productID">the product's ID</param>
        /// <returns></returns>
        public ProductDTO findProductByID(long productID) {
            return productRepository.find(productID).toDTO();
        }

        public ProductDTO findByReference(string reference) {
            return productRepository.find(reference).toDTO();
        }

        /// <summary>
        /// Updates a product with new dimensions and/or new materials
        /// </summary>
        /// <param name="updatesDTO"></param>
        /// <returns></returns>
        /// TODO Refactor method 
        public ProductDTO updateProduct(ProductDTO updatesDTO) {
            Product oldProduct = productRepository.find(updatesDTO.id);
            if (oldProduct == null) {
                return null;
            }

            IEnumerable<Dimension> heightDimensions = getProductDTOEnumerableDimensions(updatesDTO.heightDimensions);
            IEnumerable<Dimension> widthDimensions = getProductDTOEnumerableDimensions(updatesDTO.widthDimensions);
            IEnumerable<Dimension> depthDimensions = getProductDTOEnumerableDimensions(updatesDTO.depthDimensions);

            foreach (Dimension heightDimension in heightDimensions) { if (!oldProduct.addHeightDimension(heightDimension)) return null; }
            foreach (Dimension widthDimension in widthDimensions) { if (!oldProduct.addWidthDimension(widthDimension)) return null; }
            foreach (Dimension depthDimension in depthDimensions) { if (!oldProduct.addDepthDimension(depthDimension)) return null; }
            addMaterials(updatesDTO, oldProduct);

            Product prod = productRepository.update(oldProduct);
            if(prod == null) {
                return null;
            } else {
                return prod.toDTO();
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
                String dimensionType = dimensionDTO.type;
                if (dimensionType.Equals("discrete")) {
                    DiscreteDimensionIntervalDTO ddiDTO = (DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if (dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO)) {
                    ContinuousDimensionIntervalDTO cdiDTO = (ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else {
                    SingleValueDimensionDTO svdDTO = (SingleValueDimensionDTO)dimensionDTO;
                    SingleValueDimension dimensionValue = SingleValueDimension.valueOf(svdDTO.value);
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
        private void addMaterials(ProductDTO productDTO, Product oldProduct) {
            foreach (ProductMaterialDTO prodMDTO in productDTO.productMaterials) {
                long matID = prodMDTO.material.id;
                oldProduct.addMaterial(materialRepository.find(matID));
            }
        }

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
                products.Add(productRepository.find(nextProductID));
            }
            return products;
        }

    }

}