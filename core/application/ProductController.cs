using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;


namespace core.application
{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController
    {
        public bool addProduct(DTO productAsDTO){
            Product.ProductBuilder productBuilder=Product.ProductBuilder.create();
            productBuilder.withReference((string)productAsDTO.get(Product.Properties.REFERENCE_PROPERTY));
            productBuilder.withDesignation((string)productAsDTO.get(Product.Properties.DESIGNATION_PROPERTY));
            productBuilder.withComplementedProducts(enumerableOfProductsIDSAsEntities((IEnumerable<long>)productAsDTO.get(Product.Properties.COMPLEMENTED_PRODUCTS_PROPERTY)));
            productBuilder.withMaterials(new MaterialsController().enumerableOfMaterialsIDSAsEntities((IEnumerable<long>)productAsDTO.get(Product.Properties.MATERIALS_PROPERTY)));
            
            return PersistenceContext.repositories().createProductRepository().save(productBuilder.build())!=null;
        }
        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<DTO> findAllProducts()
        {
            List<DTO> productDTOList = new List<DTO>();

            IEnumerable<Product> productList = PersistenceContext.repositories().createProductRepository().findAll();

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
        public DTO findProductByID(string productID)
        {
            return PersistenceContext.repositories().createProductRepository().find(productID).toDTO();
        }

        /// <summary>
        /// Updates a product with new restrictions and/or new materials
        /// </summary>
        /// <param name="updatesDTO"></param>
        /// <returns></returns>
        /// TODO Refactor method 
        public bool updateProduct(DTO updatesDTO)
        {
            ProductRepository repository = PersistenceContext.repositories().createProductRepository();
            Product oldProduct = repository.find((string)updatesDTO.get(Product.Properties.PERSISTENCE_ID_PROPERTY));
            if (oldProduct == null)
            {
                return false;
            }

            IEnumerable<Restriction> heightRestrictions=getProductDTOEnumerableOfHeightRestrictions(updatesDTO);
            IEnumerable<Restriction> widthRestrictions=getProductDTOEnumerableOfWidthRestrictions(updatesDTO);
            IEnumerable<Restriction> depthRestrictions=getProductDTOEnumerableOfDepthRestrictions(updatesDTO);
            
            foreach(Restriction heightRestriction in heightRestrictions){if(!oldProduct.addHeightRestriction(heightRestriction))return false;}
            foreach(Restriction widthRestriction in widthRestrictions){if(!oldProduct.addWidthRestriction(widthRestriction))return false;}
            foreach(Restriction depthRestriction in depthRestrictions){if(!oldProduct.addDepthRestriction(depthRestriction))return false;}
            addMaterials(updatesDTO, oldProduct);

            return repository.update(oldProduct) != null;
        }

        /// <summary>
        /// Returns an enumerable of height restrictions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the height restrictions found on a product DTO</returns>
        internal IEnumerable<Restriction> getProductDTOEnumerableOfHeightRestrictions(DTO productDTO)
        {
            List<Restriction> heightRestrictions=new List<Restriction>();
            foreach (DTO heightRestrictionDTO in (List<DTO>)productDTO.get(Product.Properties.HEIGHT_RESTRICTIONS_PROPERTIES))
            {
                String restrictionType = (string)heightRestrictionDTO.get("type");
                List<string> values = (List<string>)heightRestrictionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (restrictionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    heightRestrictions.Add(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    heightRestrictions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    heightRestrictions.Add(dimensionValue);
                }
            }
            return heightRestrictions;
        }

        /// <summary>
        /// Returns an enumerable of width restrictions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the width restrictions found on a product DTO</returns>
        internal IEnumerable<Restriction> getProductDTOEnumerableOfWidthRestrictions(DTO productDTO)
        {
            List<Restriction> widthRestrictions=new List<Restriction>();
            foreach (DTO widthRestrictionDTO in (List<DTO>)productDTO.get(Product.Properties.WIDTH_RESTRICTIONS_PROPERTIES))
            {
                String restrictionType = (string)widthRestrictionDTO.get("type");
                List<string> values = (List<string>)widthRestrictionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (restrictionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    widthRestrictions.Add(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    widthRestrictions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    widthRestrictions.Add(dimensionValue);
                }
            }
            return widthRestrictions;
        }

        /// <summary>
        /// Returns an enumerable of depth restrictions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the depth restrictions found on a product DTO</returns>
        internal IEnumerable<Restriction> getProductDTOEnumerableOfDepthRestrictions(DTO productDTO)
        {
            List<Restriction> depthRestrictions=new List<Restriction>();
            foreach (DTO depthRestrictionDTO in (List<DTO>)productDTO.get(Product.Properties.DEPTH_RESTRICTIONS_PROPERTIES))
            {
                String restrictionType = (string)depthRestrictionDTO.get("type");
                List<string> values = (List<string>)depthRestrictionDTO.get("values");
                List<double> doubleValues = new List<double>();
                foreach (string str in values)
                {
                    doubleValues.Add(Double.Parse(str));
                }
                if (restrictionType.Equals("discrete"))
                {
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(doubleValues);
                    depthRestrictions.Add(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    depthRestrictions.Add(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    depthRestrictions.Add(dimensionValue);
                }
            }
            return depthRestrictions;
        }

        /// <summary>
        /// Adds new materials to updated product
        /// </summary>
        /// <param name="productDTO">list of updates in DTO</param>
        /// <param name="oldProduct">product to be updated</param>
        private void addMaterials(DTO productDTO, Product oldProduct)
        {
            MaterialRepository repository = PersistenceContext.repositories().createMaterialRepository();
            foreach (string str in (List<string>)productDTO.get(Product.Properties.MATERIALS_PROPERTY))
            {
                oldProduct.addMaterial(repository.find(str));
            }
        }

         /// <summary>
        /// Parses an enumerable of products persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="productsIDS">Enumerable with the products persistence identifiers</param>
        /// <returns>IEnumerable with the products ids as entities</returns>
        internal IEnumerable<Product> enumerableOfProductsIDSAsEntities(IEnumerable<long> productsIDS){
            if(productsIDS==null)return null;
            List<Product> products=new List<Product>();
            IEnumerator<long> productsIDSIterator=productsIDS.GetEnumerator();
            long nextProductID=productsIDSIterator.Current;
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            while(productsIDSIterator.MoveNext()){
                nextProductID=productsIDSIterator.Current;
                products.Add(productRepository.find(nextProductID));
            }
            return products;
        }

    }

}