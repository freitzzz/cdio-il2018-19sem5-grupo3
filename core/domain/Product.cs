using support.builders;
using support.domain;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;

namespace core.domain{

    /// <summary>
    /// Class that represents a Product.
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="Product"></typeparam>
    /// <typeparam name="ProductDTO">Type of DTO being used</typeparam>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product: AggregateRoot<string>, DTOAble<ProductDTO>{
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
        /// Constant that represents the messange that ocurres if the product restrinctions are invalid
        /// </summary>
        private const string INVALID_PRODUCT_DIMENSIONS="The product dimensions are invalid";

        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id{get; private set;}

        /// <summary>
        /// String with the product reference
        /// </summary>
        public string reference {get; set;}
        /// <summary>
        /// String with the product designation
        /// </summary>
        public string designation {get; set;}
        /// <summary>
        /// List with the products which the current product can be complemented by
        /// </summary>
        //TODO: Should complemented products be a list and not a set?
        public List<Product> complementedProducts {get; set;}
        /// <summary>
        /// List with the materials which the product can be made of
        /// </summary>
        //TODO: Should product materials be a list or a set?
        [NotMapped]
        public List<Material> materials {get; set;}
        /// <summary>
        /// List with the product heigth dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        [NotMapped] //! NotMapped annotation is only temporary, should be removed once Dimension mapping is configure
        public List<Dimension> heightValues {get; set;}
        /// <summary>
        /// List with the product width dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        [NotMapped] //! NotMapped annotation is only temporary, should be removed once Dimension mapping is configured
        public List<Dimension> widthValues {get; set;}
        /// <summary>
        /// List with the product depth dimensions
        /// </summary>
        //TODO: Should product restrinctions be a list or a set
        [NotMapped] //! NotMapped annotation is only temporary, should be removed once Dimension mapping is configured
        public List<Dimension> depthValues {get; set;}

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Product(){}

        /// <summary>
        /// Builds a new product with its reference, designation and materials which it can be made of
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        public Product(string reference,string designation,IEnumerable<Material> materials,
                        IEnumerable<Dimension> heightDimensions,
                        IEnumerable<Dimension> widthDimensions,
                        IEnumerable<Dimension> depthDimensions){
            checkProductProperties(reference,designation);
            checkProductMaterials(materials);
            checkProductDimensions(heightDimensions);
            checkProductDimensions(widthDimensions);
            checkProductDimensions(depthDimensions);
            this.reference=reference;
            this.designation=designation;
            this.materials=new List<Material>(materials);
            this.complementedProducts=new List<Product>();
            this.heightValues=new List<Dimension>(heightDimensions);
            this.widthValues=new List<Dimension>(widthDimensions);
            this.depthValues=new List<Dimension>(depthDimensions);
        }

        /// <summary>
        /// Builds a new product with its reference, designation and complemented products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
        public Product(string reference,string designation,IEnumerable<Material> materials,IEnumerable<Product> complementedProducts,
                        IEnumerable<Dimension> heightValues,
                        IEnumerable<Dimension> widthValues,
                        IEnumerable<Dimension> depthValues){
            checkProductComplementedProducts(complementedProducts);
            checkProductMaterials(materials);
            checkProductProperties(reference,designation);
            checkProductDimensions(heightValues);
            checkProductDimensions(widthValues);
            checkProductDimensions(depthValues);
            this.reference=reference;
            this.designation=designation;
            this.materials=new List<Material>(materials);
            this.complementedProducts=new List<Product>(complementedProducts);
            this.heightValues=new List<Dimension>(heightValues);
            this.widthValues=new List<Dimension>(widthValues);
            this.depthValues=new List<Dimension>(depthValues);
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
        /// Adds new height value(s) to the product
        /// </summary>
        /// <param name="heightDimension">Height's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addHeightDimension(Dimension heightDimension){
            if(!isProductDimensionValidForAddition(heightDimension,heightValues))
                return false;
            heightValues.Add(heightDimension);
            return true;
        }

        /// <summary>
        /// Adds new width value(s) to the product
        /// </summary>
        /// <param name="widthDimension">Width's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addWidthDimension(Dimension widthDimension){
            if(!isProductDimensionValidForAddition(widthDimension,widthValues))
                return false;
            widthValues.Add(widthDimension);
            return true;
        }

        /// <summary>
        /// Adds new depth value(s) to the product
        /// </summary>
        /// <param name="depthDimension">Depth's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addDepthDimension(Dimension depthDimension){
            if(!isProductDimensionValidForAddition(depthDimension,depthValues))
                return false;
            depthValues.Add(depthDimension);
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
        public ProductDTO toDTO(){
            ProductDTO dto = new ProductDTO();

            dto.id = this.Id;
            dto.designation = this.designation;
            dto.reference = this.reference;

            if(this.complementedProducts != null){
                List<ProductDTO> complementDTOList = new List<ProductDTO>();

                foreach(Product complement in complementedProducts){
                    complementDTOList.Add(complement.toDTO()); 
                }
                dto.complements = complementDTOList;
            }

            //TODO: add missing DTO's


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
            return !complementedProducts.Contains(complementedProduct);
        }

        /// <summary>
        /// Checks if a product material is valid for addition on the current product
        /// </summary>
        /// <param name="productMaterial">Material with the product material being validated</param>
        /// <returns>boolean true if the product material is valid for addition, false if not</returns>
        private bool isProductMaterialValidForAddition(Material productMaterial){
            return productMaterial!=null && !materials.Contains(productMaterial);
        }

        /// <summary>
        /// Checks if a product dimension is valid for addition on the current product
        /// </summary>
        /// <param name="productDimension">Dimension being validated</param>
        /// <param name="productDimensions">IEnumerable with the product dimensions</param>
        /// <returns>boolean true if the dimension is valid for addition, false if not</returns>
        private bool isProductDimensionValidForAddition(Dimension productDimension,ICollection<Dimension> productDimensions){
            return productDimension!=null && !productDimensions.Contains(productDimension);
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
        /// Checks if the product dimensions are valid
        /// </summary>
        /// <param name="productDimensions">IEnumerable with the product dimensions</param>
        private void checkProductDimensions(IEnumerable<Dimension> productDimensions){
            if(Collections.isEnumerableNullOrEmpty(productDimensions))
                throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
            checkDuplicatedDimensions(productDimensions);
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
                complementedProduct=complementedProductsEnumerator.Current;
                if(!complementedProductsRefereces.Add(complementedProduct.id())){
                    throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTED_PRODUCTS);
                }
            }
        }
        
        /// <summary>
        /// Checks if an enumerable of materials has duplicates
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials</param>
        private void checkDuplicatedMaterials(IEnumerable<Material> productMaterials){
            HashSet<string> productMaterialsReferences=new HashSet<string>();
            IEnumerator<Material> productMaterialsEnumerator=productMaterials.GetEnumerator();
            Material productMaterial=productMaterialsEnumerator.Current;
            while(productMaterialsEnumerator.MoveNext()){
                productMaterial=productMaterialsEnumerator.Current;
                if(!productMaterialsReferences.Add(productMaterial.id())){
                    throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
                }
            }
        }

        /// <summary>
        /// Checks if an enumerable of dimensions has duplicates
        /// </summary>
        /// <param name="productDimension">IEnumerable with product dimensions</param>
        private void checkDuplicatedDimensions(IEnumerable<Dimension> productDimensions){
            HashSet<int> productDimensionsHashCodes=new HashSet<int>();
            IEnumerator<Dimension> productDimensionsEnumerator=productDimensions.GetEnumerator();
            Dimension nextDimension=productDimensionsEnumerator.Current;
            while(productDimensionsEnumerator.MoveNext()){
                nextDimension=productDimensionsEnumerator.Current;
                if(!productDimensionsHashCodes.Add(nextDimension.GetHashCode())){
                    throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
                }
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
            /// <summary>
            /// Constant that represents the name of the property which maps the product persistence identifier
            /// </summary>
            public const string PERSISTENCE_ID_PROPERTY="persistence_id";
            /// <summary>
            /// Constant that represents the name of the property which maps the product materials
            /// </summary>
            public const string MATERIALS_PROPERTY="materials";
            /// <summary>
            /// Constant that represents the name of the property which maps the product complemented products
            /// </summary>
            public const string COMPLEMENTED_PRODUCTS_PROPERTY="complemented_products";
            /// <summary>
            /// Constant that represents the name of the property which maps the product height values
            /// </summary>
            public const string HEIGHT_VALUES_PROPERTIES="height_values";
            /// <summary>
            /// Constant that represents the name of the property which maps the product width values
            /// </summary>
            public const string WIDTH_VALUES_PROPERTIES="width_values";
            /// <summary>
            /// Constant that represents the name of the property which maps the product depth values
            /// </summary>
            public const string DEPTH_VALUES_PROPERTIES="depth_values";
        }
        /// <summary>
        /// Represents a builder of products
        /// </summary>
        /// <typeparam name="Product">Generic-Type of the Product entity</typeparam>
        public class ProductBuilder : Builder<Product>{
            /// <summary>
            /// DTO with the builder content
            /// </summary>
            private readonly ProductDTO builderDTO;

            /// <summary>
            /// Adds a reference to the current product builder
            /// </summary>
            /// <param name="reference">string with the product reference</param>
            /// <returns>ProductBuilder with the product builder with the new reference added</returns>

            /// <summary>
            /// Creates a new ProductBuilder
            /// </summary>
            /// <returns>ProductBuilder with the builder for products</returns>
            public static ProductBuilder create(){return new ProductBuilder();}

            /// <summary>
            /// Adds a reference to the product builder
            /// </summary>
            /// <param name="reference">string with the product reference</param>
            /// <returns>ProductBuilder with the updated builder</returns>            
            public ProductBuilder withReference(string reference){
                builderDTO.reference=reference;
                return this;
            }

            /// <summary>
            /// Adds a designation to the product builder
            /// </summary>
            /// <param name="designation">string with the product designation</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withDesignation(string designation){
                builderDTO.designation=designation;
                return this;
            }

            /// <summary>
            /// Adds complemented product to the product builder
            /// </summary>
            /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withComplementedProducts(IEnumerable<ProductDTO> complementedProducts){
                builderDTO.complements=new List<ProductDTO>(complementedProducts);
                return this;
            }

            /// <summary>
            /// Adds materials to the product builder
            /// </summary>
            /// <param name="materials">IEnumerable with the product materials</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withMaterials(IEnumerable<MaterialDTO> materials){
                builderDTO.materials=new List<MaterialDTO>(materials);
                return this;
            }

            /// <summary>
            /// Adds a height dimensions to the product builder
            /// </summary>
            /// <param name="heightDimensions">IEnumerable with the product height dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withHeightDimensions(IEnumerable<DimensionDTO> heightDimensions){
                builderDTO.heightDimensions=new List<DimensionDTO>(heightDimensions);
                return this;
            }

            /// <summary>
            /// Adds a depth dimensions to the product builder
            /// </summary>
            /// <param name="depthDimensions">IEnumerable with the product depth dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withDepthDimensions(IEnumerable<DimensionDTO> depthDimensions){
                builderDTO.depthDimensions=new List<DimensionDTO>(depthDimensions);
                return this;
            }

            /// <summary>
            /// Adds a width dimensions to the product builder
            /// </summary>
            /// <param name="widthDimensions">IEnumerable with the product width dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withWidthDimensions(IEnumerable<DimensionDTO> widthDimensions){
                builderDTO.widthDimensions=new List<DimensionDTO>(widthDimensions);
                return this;
            }

            /// <summary>
            /// Builds a new Product based on the builder input
            /// </summary>
            /// <returns>Product with the product based on the builder input</returns>
            public Product build(){
                IEnumerable<ProductDTO> complementedProducts=builderDTO.complements;
                if(complementedProducts==null){
                    return new Product(builderDTO.reference
                                    ,builderDTO.designation
                                    ,DTOUtils.reverseDTOS(builderDTO.materials)
                                    ,DTOUtils.reverseDTOS(builderDTO.heightDimensions)
                                    ,DTOUtils.reverseDTOS(builderDTO.widthDimensions)
                                    ,DTOUtils.reverseDTOS(builderDTO.depthDimensions));
                }else{
                    return new Product(builderDTO.reference
                                    ,builderDTO.designation
                                    ,DTOUtils.reverseDTOS(builderDTO.materials)
                                    ,DTOUtils.reverseDTOS(complementedProducts)
                                    ,DTOUtils.reverseDTOS(builderDTO.heightDimensions)
                                    ,DTOUtils.reverseDTOS(builderDTO.widthDimensions)
                                    ,DTOUtils.reverseDTOS(builderDTO.depthDimensions));
                }
            }
            /// <summary>
            /// Hides default constructor
            /// </summary>
            private ProductBuilder(){builderDTO=new ProductDTO();}
        }
    }
}