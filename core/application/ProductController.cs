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