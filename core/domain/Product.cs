using support.domain;
using support.domain.ddd;
using support.dto;
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
    public class Product:AggregateRoot<string>,DTOAble{
        /// <summary>
        /// Constant that represents the messange that ocurres if the product reference is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE="The product reference is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product designation is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION="The product designation is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_COMPLEMENTED_PRODUCTS="The products which the product can be complemented by are invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_MATERIALS="The materials which the product can be made of are invalid";
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
        /// List with the materials which the product can be made of
        /// </summary>
        //TODO: Should product materials be a list or a set?
        private readonly List<Material> materials;
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
            this.materials=new List<Material>();
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
            this.materials=new List<Material>();
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
        /// Adds a new material which the product can be made of
        /// </summary>
        /// <param name="productMaterial">Material with the product material</param>
        /// <returns>boolean true if the product material was added with success, false if not</returns>
        public bool addMaterial(Material productMaterial){
            if(!isProductMaterialValidForAddition(productMaterial))
                return false;
            materials.Add(productMaterial);
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
        /// Returns the current product as a DTO
        /// </summary>
        /// <returns>DTO with the current DTO representation of the product</returns>
        public DTO toDTO(){
            GenericDTO dto=new GenericDTO(Product.Properties.CONTEXT);
            dto.put(Properties.DESIGNATION_PROPERTY,designation);
            dto.put(Properties.REFERENCE_PROPERTY,reference);
            return dto;
        }

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
        /// <returns>boolean true if the complemented product is valid for addition, false if not</returns>
        private bool isComplementedProductValidForAddition(Product complementedProduct){
            if(complementedProduct==null||complementedProduct.Equals(this))return false;
            return complementedProducts.Contains(complementedProduct);
        }

        /// <summary>
        /// Checks if a product material is valid for addition on the current product
        /// </summary>
        /// <param name="productMaterial">Material with the product material being validated</param>
        /// <returns>boolean true if the product material is valid for addition, false if not</returns>
        private bool isProductMaterialValidForAddition(Material productMaterial){
            return productMaterial!=null && materials.Contains(productMaterial);
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
        /// Checks if the materials which a product can be made of are valid
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials</param>
        private void checkProductMaterials(IEnumerable<Material> productMaterials){
            if(Collections.isEnumerableNullOrEmpty(productMaterials))
                throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
            checkDuplicatedMaterials(productMaterials);
        }

        /// <summary>
        /// Checks if a enumerable of products has duplicates
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
        
        /// <summary>
        /// Checks if an enumerable of materials has duplicates
        /// </summary>
        /// <param name="productMaterials"></param>
        private void checkDuplicatedMaterials(IEnumerable<Material> productMaterials){
            HashSet<string> productMaterialsReferences=new HashSet<string>();
            IEnumerator<Material> productMaterialsEnumerator=productMaterials.GetEnumerator();
            Material productMaterial=productMaterialsEnumerator.Current;
            while(productMaterialsEnumerator.MoveNext()){
                if(!productMaterialsReferences.Add(productMaterial.id())){
                    throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
                }
                productMaterial=productMaterialsEnumerator.Current;
            }
        }

        /// <summary>
        /// Inner static class which represents the Product properties used to map on data holders (e.g. DTO)
        /// </summary>
        public static class Properties{
            /// <summary>
            /// Constant that represents the context of the properties
            /// </summary>
            public const string CONTEXT="Product";
            /// <summary>
            /// Constant that represents the name of the property which maps the product designation
            /// </summary>
            public const string DESIGNATION_PROPERTY="designation";
            /// <summary>
            /// Constant that represents the name of the property which maps the product reference
            /// </summary>
            public const string REFERENCE_PROPERTY="reference";
        }
    }
}