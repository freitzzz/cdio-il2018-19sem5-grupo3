using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;

namespace core.application
{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController
    {

        private readonly ProductRepository productRepository;

        private readonly MaterialRepository materialRepository;

        public ProductController(ProductRepository productRepository, MaterialRepository materialRepository)
        {
            this.productRepository = productRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="productAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public ProductDTO addProduct(ProductDTO productAsDTO)
        {
            Product newProduct = productAsDTO.toEntity();

            Product createdProduct = productRepository.save(newProduct); 
            
            if (createdProduct == null) return null;
            return createdProduct.toDTO();
        }

        /// <summary>
        /// Removes (Disables) a product
        /// </summary>
        /// <param name="productDTO">DTO with the product information</param>
        /// <returns>boolean true if the product was removed (disabled) with success</returns>
        public bool removeProduct(ProductDTO productDTO)
        {
            Product productBeingRemoved=productRepository.find(productDTO.id);
            return productBeingRemoved != null && productBeingRemoved.disable() && productRepository.update(productBeingRemoved) != null;
        }

        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<ProductDTO> findAllProducts()
        {
            List<ProductDTO> productDTOList = new List<ProductDTO>();

            IEnumerable<Product> productList = productRepository.findAll();

            if (productList == null)
            {
                return null;
            }

            foreach (Product product in productList)
            {
                productDTOList.Add(product.toDTO());
            }

            return productDTOList;
        }

        /// <summary>
        /// Fetches a product from the product repository given an ID
        /// </summary>
        /// <param name="productID">the product's ID</param>
        /// <returns></returns>
        public ProductDTO findProductByID(long productID)
        {
            return productRepository.find(productID).toDTO();
        }

        public ProductDTO findByReference(string reference){
            return productRepository.find(reference).toDTO();
        }

        /// <summary>
        /// Updates a product with new dimensions and/or new materials
        /// </summary>
        /// <param name="updatesDTO"></param>
        /// <returns></returns>
        /// TODO Refactor method 
        public bool updateProduct(GenericDTO updatesDTO)
        {
            Product oldProduct = productRepository.find((string)updatesDTO.get(Product.Properties.PERSISTENCE_ID_PROPERTY));
            if (oldProduct == null)
            {
                return false;
            }

            IEnumerable<Dimension> heightDimensions = getProductDTOEnumerableOfHeightDimensions(updatesDTO);
            IEnumerable<Dimension> widthDimensions = getProductDTOEnumerableOfWidthDimensions(updatesDTO);
            IEnumerable<Dimension> depthDimensions = getProductDTOEnumerableOfDepthDimensions(updatesDTO);

            foreach (Dimension heightDimension in heightDimensions) { if (!oldProduct.addHeightDimension(heightDimension)) return false; }
            foreach (Dimension widthDimension in widthDimensions) { if (!oldProduct.addWidthDimension(widthDimension)) return false; }
            foreach (Dimension depthDimension in depthDimensions) { if (!oldProduct.addDepthDimension(depthDimension)) return false; }
            addMaterials(updatesDTO, oldProduct);

            return productRepository.update(oldProduct) != null;
        }

        /// <summary>
        /// Returns an enumerable of height dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the height dimensions found on a product DTO</returns>
        internal IEnumerable<Dimension> getProductDTOEnumerableOfHeightDimensions(GenericDTO productDTO)
        {
            List<Dimension> heightDimensions = new List<Dimension>();
            foreach (GenericDTO heightDimensionDTO in (List<GenericDTO>)productDTO.get(Product.Properties.HEIGHT_VALUES_PROPERTIES))
            {
                String dimensionType = (string)heightDimensionDTO.get("type");
                List<string> values = (List<string>)heightDimensionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (dimensionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    heightDimensions.Add(discreteInterval);
                }
                else if (dimensionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    heightDimensions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    SingleValueDimension dimensionValue = SingleValueDimension.valueOf(array[0]);
                    heightDimensions.Add(dimensionValue);
                }
            }
            return heightDimensions;
        }

        /// <summary>
        /// Returns an enumerable of width dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the width dimensions found on a product DTO</returns>
        internal IEnumerable<Dimension> getProductDTOEnumerableOfWidthDimensions(GenericDTO productDTO)
        {
            List<Dimension> widthDimensions = new List<Dimension>();
            foreach (GenericDTO widthDimensionDTO in (List<GenericDTO>)productDTO.get(Product.Properties.WIDTH_VALUES_PROPERTIES))
            {
                String dimensionType = (string)widthDimensionDTO.get("type");
                List<string> values = (List<string>)widthDimensionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (dimensionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    widthDimensions.Add(discreteInterval);
                }
                else if (dimensionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    widthDimensions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    SingleValueDimension dimensionValue = SingleValueDimension.valueOf(array[0]);
                    widthDimensions.Add(dimensionValue);
                }
            }
            return widthDimensions;
        }

        /// <summary>
        /// Returns an enumerable of depth dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the depth dimensions found on a product DTO</returns>
        internal IEnumerable<Dimension> getProductDTOEnumerableOfDepthDimensions(GenericDTO productDTO)
        {
            List<Dimension> depthDimensions = new List<Dimension>();
            foreach (GenericDTO depthDimensionDTO in (List<GenericDTO>)productDTO.get(Product.Properties.DEPTH_VALUES_PROPERTIES))
            {
                String dimensionType = (string)depthDimensionDTO.get("type");
                List<string> values = (List<string>)depthDimensionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (dimensionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    depthDimensions.Add(discreteInterval);
                }
                else if (dimensionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    depthDimensions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    SingleValueDimension dimensionValue = SingleValueDimension.valueOf(array[0]);
                    depthDimensions.Add(dimensionValue);
                }
            }
            return depthDimensions;
        }

        /// <summary>
        /// Adds new materials to updated product
        /// </summary>
        /// <param name="productDTO">list of updates in DTO</param>
        /// <param name="oldProduct">product to be updated</param>
        private void addMaterials(GenericDTO productDTO, Product oldProduct)
        {
            foreach (string str in (List<string>)productDTO.get(Product.Properties.MATERIALS_PROPERTY))
            {
                oldProduct.addMaterial(materialRepository.find(str));
            }
        }

        /// <summary>
        /// Parses an enumerable of products persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="productsIDS">Enumerable with the products persistence identifiers</param>
        /// <returns>IEnumerable with the products ids as entities</returns>
        internal IEnumerable<Product> enumerableOfProductsIDSAsEntities(IEnumerable<long> productsIDS)
        {
            if (productsIDS == null) return null;
            List<Product> products = new List<Product>();
            IEnumerator<long> productsIDSIterator = productsIDS.GetEnumerator();
            long nextProductID = productsIDSIterator.Current;
            while (productsIDSIterator.MoveNext())
            {
                nextProductID = productsIDSIterator.Current;
                products.Add(productRepository.find(nextProductID));
            }
            return products;
        }

    }

}