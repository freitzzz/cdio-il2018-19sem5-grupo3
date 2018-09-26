using support.domain;
using support.domain.ddd;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace core.domain{
    /// <summary>
    /// Class that represents a Product
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product:AggregateRoot<string>{
        /// <summary>
        /// Constant that represents the messange that ocurres if the product reference is invalid
        /// </summary>
        private static readonly string INVALID_PRODUCT_REFERENCE="The product reference is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product designation is invalid
        /// </summary>
        private static readonly string INVALID_PRODUCT_DESIGNATION="The product designation is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product complemented products are invalid
        /// </summary>
        private static readonly string INVALID_PRODUCT_COMPLEMENTED_PRODUCTS="The products which the product can be complemented by are invalid";
        /// <summary>
        /// String with the product reference
        /// </summary>
        [Key]
        private readonly string reference;
        /// <summary>
        /// String with the product designation
        /// </summary>
        private readonly string designation;
        /// <summary>
        /// List with the products which the current product can be complemented by
        /// </summary>
        //TODO: Should complemented products be a list and not a set?
        private readonly List<Product> complementedProducts;
        /// <summary>
        /// Builds a new product with its reference and designation 
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        public Product(string reference,string designation){
            checkProductProperties(reference,designation);
            this.reference=reference;
            this.designation=designation;
            this.complementedProducts=new List<Product>();
        }

        /// <summary>
        /// Builds a new product with its reference, designation and complemented products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
        public Product(string reference,string designation,IEnumerable<Product> complementedProducts){
            checkProductComplementedProducts(complementedProducts);
            checkProductProperties(reference,designation);
            this.reference=reference;
            this.designation=designation;
            this.complementedProducts=new List<Product>(complementedProducts);
        }

        /// <summary>
        /// Adds a new product which the current product can be complemented by
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product</param>
        /// <returns>boolean true if the complemented product was added with success, false if not</returns>
        public bool addComplementedProduct(Product complementedProduct){
            if(!isComplementedProductValidForAddition(complementedProduct))
                return false;
            complementedProducts.Add(complementedProduct);
            return true;
        }

        /// <summary>
        /// Returns the product identity
        /// </summary>
        /// <returns>string with the product identity</returns>
        public string id(){return reference;}

        /// <summary>
        /// Checks if a certain product entity is the same as the current product
        /// </summary>
        /// <param name="comparingEntity">string with the comparing product identity</param>
        /// <returns>boolean true if both entities identity are the same, false if not</returns>
        public bool sameAs(string comparingEntity){return id().Equals(comparingEntity);}
        
        /// <summary>
        /// Represents the textual information of the Product
        /// </summary>
        /// <returns>String with the textual representation of the product</returns>
        public override string ToString(){
            //Should ToString List the Product Complemented Products?
            return String.Format("Product Information\n- Designation: {0}\n- Reference: {1}",designation,reference);
        }

        /// <summary>
        /// Checks if two products are equal
        /// </summary>
        /// <param name="comparingProduct">Product with the product being compared to the current one</param>
        /// <returns>boolean true if both products are equal, false if not</returns>
        public override bool Equals(object comparingProduct){
            if(this==comparingProduct)return true;
            return comparingProduct is Product && this.id().Equals(((Product)comparingProduct).id());
        }

        /// <summary>
        /// Represents the product hashcode
        /// </summary>
        /// <returns>Integer with the current product hashcode</returns>
        public override int GetHashCode(){
            return id().GetHashCode();
        }

        /// <summary>
        /// Checks if a complemented product is valid for additon on the current product
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product being validated</param>
        /// <returns>boolean true if the complemented product is valid for additon, false if not</returns>
        private bool isComplementedProductValidForAddition(Product complementedProduct){
            if(complementedProduct==null||complementedProduct.Equals(this))return false;
            return complementedProducts.Contains(complementedProduct);
        }

        /// <summary>
        /// Checks if the product properties are valid
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        private void checkProductProperties(string reference,string designation){
            if(Strings.isNullOrEmpty(reference))throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
            if(Strings.isNullOrEmpty(designation))throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
        }
        
        /// <summary>
        /// Checks if the products which a product can be complemented by are valid
        /// </summary>
        /// <param name="complementedProducts">IEnumerable with the complemented products</param>
        private void checkProductComplementedProducts(IEnumerable<Product> complementedProducts){
            if(Collections.isEnumerableNullOrEmpty(complementedProducts))
                throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTED_PRODUCTS);
            checkDuplicatedComplementedProducts(complementedProducts);
        }

        /// <summary>
        /// Checks if a enumerable of products have duplicates
        /// </summary>
        /// <param name="complementedProducts">IEnumerable with the complemented products</param>
        private void checkDuplicatedComplementedProducts(IEnumerable<Product> complementedProducts){
            HashSet<string> complementedProductsRefereces=new HashSet<string>();
            IEnumerator<Product> complementedProductsEnumerator=complementedProducts.GetEnumerator();
            Product complementedProduct=complementedProductsEnumerator.Current;
            while(complementedProductsEnumerator.MoveNext()){
                if(!complementedProductsRefereces.Add(complementedProduct.id())){
                    throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTED_PRODUCTS);
                }
                complementedProduct=complementedProductsEnumerator.Current;
            }
        }
    }
}