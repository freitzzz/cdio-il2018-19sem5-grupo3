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

            addHeightRestrictions(updatesDTO, oldProduct);
            addWidthRestrictions(updatesDTO, oldProduct);
            addDepthRestrictions(updatesDTO, oldProduct);
            addMaterials(updatesDTO, oldProduct);

            return repository.save(oldProduct) != null;
        }

        private void addHeightRestrictions(DTO productDTO, Product oldProduct)
        {
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
                    oldProduct.addHeightRestriction(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    oldProduct.addHeightRestriction(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    oldProduct.addHeightRestriction(dimensionValue);
                }
            }
        }

        private void addWidthRestrictions(DTO productDTO, Product oldProduct)
        {
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
                    oldProduct.addWidthRestriction(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    oldProduct.addWidthRestriction(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    oldProduct.addWidthRestriction(dimensionValue);
                }
            }
        }

        private void addDepthRestrictions(DTO productDTO, Product oldProduct)
        {
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
                    oldProduct.addDepthRestriction(discreteInterval);
                }
                else if (restrictionType.Equals("continuous"))
                {
                    double[] array = doubleValues.ToArray();
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(array[0], array[1], array[2]);
                    oldProduct.addDepthRestriction(continuousInterval);
                }
                else
                {
                    double[] array = doubleValues.ToArray();
                    Dimension dimensionValue = Dimension.valueOf(array[0]);
                    oldProduct.addDepthRestriction(dimensionValue);
                }
            }
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