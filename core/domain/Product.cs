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

namespace core.domain {

    /// <summary>
    /// Class that represents a Product.
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="Product"></typeparam>
    /// <typeparam name="ProductDTO">Type of DTO being used</typeparam>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product : AggregateRoot<string>, DTOAble<ProductDTO> {
        /// <summary>
        /// Constant that represents the messange that ocurres if the product reference is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The product reference is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product designation is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The product designation is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_COMPLEMENTED_PRODUCTS = "The products which the product can be complemented by are invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_MATERIALS = "The materials which the product can be made of are invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product restrinctions are invalid
        /// </summary>
        private const string INVALID_PRODUCT_DIMENSIONS = "The product dimensions are invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the product category is invalid
        /// </summary>
        private const string INVALID_PRODUCT_CATEGORY = "The product category is invalid";

        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id { get; internal set; }   //the id should have an internal set, since DTO's have to be able to set them

        /// <summary>
        /// String with the product reference
        /// </summary>
        public string reference { get; protected set; }
        /// <summary>
        /// String with the product designation
        /// </summary>
        public string designation { get; protected set; }
        /// <summary>
        /// List with the components which the current product can be complemented by
        /// </summary>
        //TODO: Should complemented products be a list and not a set?
        [NotMapped]
        public virtual List<Component> complementedProducts { get; protected set; }
        /// <summary>
        /// List with the materials which the product can be made of
        /// </summary>
        //TODO: Should product materials be a list or a set?
        [NotMapped]
        public virtual List<ProductMaterial> productMaterials { get; protected set; }
        /// <summary>
        /// List with the product heigth dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        public virtual List<Dimension> heightValues { get; protected set; }
        /// <summary>
        /// List with the product width dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        public virtual List<Dimension> widthValues { get; protected set; }
        /// <summary>
        /// List with the product depth dimensions
        /// </summary>
        //TODO: Should product restrinctions be a list or a set
        public virtual List<Dimension> depthValues { get; protected set; }
        /// <summary>
        /// ProductCategory with the category which the product belongs to
        /// </summary>
        public virtual ProductCategory productCategory {get; protected set;}

        /// <summary>
        /// Boolean that controls if the current product is available or not
        /// </summary>
        public bool isAvailable { get; protected set; }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Product() { }

        /// <summary>
        /// Builds a new product with its reference, designation and materials which it can be made of
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="heightDimensions">IEnumerable with the product height dimensions</param>
        /// <param name="widthDimensions">IEnumerable with the product width dimensions</param>
        /// <param name="depthDimensions">IEnumerable with the product depth dimensions</param>
        public Product(string reference, string designation, ProductCategory productCategory, IEnumerable<Material> materials,
                        IEnumerable<Dimension> heightDimensions,
                        IEnumerable<Dimension> widthDimensions,
                        IEnumerable<Dimension> depthDimensions) {
            checkProductProperties(reference, designation);
            checkProductMaterials(materials);
            checkProductDimensions(heightDimensions);
            checkProductDimensions(widthDimensions);
            checkProductDimensions(depthDimensions);
            checkProductCategory(productCategory);
            this.reference = reference;
            this.designation = designation;
            this.productMaterials = new List<ProductMaterial>();
            foreach (Material mat in materials) {
                this.productMaterials.Add(new ProductMaterial(this, mat));
            }
            this.complementedProducts = new List<Component>();
            this.heightValues = new List<Dimension>(heightDimensions);
            this.widthValues = new List<Dimension>(widthDimensions);
            this.depthValues = new List<Dimension>(depthDimensions);
            this.productCategory = productCategory;
        }

        /// <summary>
        /// Builds a new product with its reference, designation and complemented products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
        /// <param name="heightDimensions">IEnumerable with the product height dimensions</param>
        /// <param name="widthDimensions">IEnumerable with the product width dimensions</param>
        /// <param name="depthDimensions">IEnumerable with the product depth dimensions</param>
        public Product(string reference, string designation, ProductCategory productCategory, IEnumerable<Material> materials, IEnumerable<Component> complementedProducts,
                        IEnumerable<Dimension> heightValues,
                        IEnumerable<Dimension> widthValues,
                        IEnumerable<Dimension> depthValues) {
            checkProductComplementedProducts(complementedProducts);
            checkProductMaterials(materials);
            checkProductProperties(reference, designation);
            checkProductDimensions(heightValues);
            checkProductDimensions(widthValues);
            checkProductDimensions(depthValues);
            checkProductCategory(productCategory);
            this.reference = reference;
            this.designation = designation;
            this.productMaterials = new List<ProductMaterial>();
            foreach (Material mat in materials) {
                this.productMaterials.Add(new ProductMaterial(this, mat));
            }
            this.complementedProducts = new List<Component>(complementedProducts);
            this.heightValues = new List<Dimension>(heightValues);
            this.widthValues = new List<Dimension>(widthValues);
            this.depthValues = new List<Dimension>(depthValues);
            this.productCategory = productCategory;
        }

        /// <summary>
        /// Adds a new product which the current product can be complemented by
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product</param>
        /// <returns>boolean true if the complemented product was added with success, false if not</returns>
        public bool addComplementedProduct(Component complementedProduct) {
            if (!isComplementedProductValidForAddition(complementedProduct))
                return false;
            complementedProducts.Add(complementedProduct);
            return true;
        }

        /// <summary>
        /// Adds a new material which the product can be made of
        /// </summary>
        /// <param name="productMaterial">Material with the product material</param>
        /// <returns>boolean true if the product material was added with success, false if not</returns>
        public bool addMaterial(Material productMaterial) {
            if (!isProductMaterialValidForAddition(productMaterial)) {
                return false;
            }
            this.productMaterials.Add(new ProductMaterial(this, productMaterial));
            return true;
        }

        /// <summary>
        /// Adds new height value(s) to the product
        /// </summary>
        /// <param name="heightDimension">Height's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addHeightDimension(Dimension heightDimension) {
            if (!isProductDimensionValidForAddition(heightDimension, heightValues))
                return false;
            heightValues.Add(heightDimension);
            return true;
        }

        /// <summary>
        /// Adds new width value(s) to the product
        /// </summary>
        /// <param name="widthDimension">Width's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addWidthDimension(Dimension widthDimension) {
            if (!isProductDimensionValidForAddition(widthDimension, widthValues))
                return false;
            widthValues.Add(widthDimension);
            return true;
        }

        /// <summary>
        /// Adds new depth value(s) to the product
        /// </summary>
        /// <param name="depthDimension">Depth's value(s)</param>
        /// <returns>boolean true if the value(s) were added with success, false if not</returns>
        public bool addDepthDimension(Dimension depthDimension) {
            if (!isProductDimensionValidForAddition(depthDimension, depthValues))
                return false;
            depthValues.Add(depthDimension);
            return true;
        }

        /// <summary>
        /// Changes the current product category
        /// </summary>
        /// <param name="productCategory">ProductCategory with the new product category</param>
        /// <returns>boolean true if the product category was changed, false if not</returns>
        public bool changeProductCategory(ProductCategory productCategory) {
            if (productCategory == null || this.productCategory.Equals(productCategory))
                return false;
            this.productCategory = productCategory;
            return true;
        }

        /// <summary>
        /// Changes the current product reference
        /// </summary>
        /// <param name="reference">String with the reference being updated</param>
        /// <returns>boolean true if the reference update was valid, false if not</returns>
        public bool changeProductReference(string reference){
            if(Strings.isNullOrEmpty(reference))return false;
            this.reference=reference;
            return true;
        }

        /// <summary>
        /// Changes the current product designation
        /// </summary>
        /// <param name="designation">String with the designation being updated</param>
        /// <returns>boolean true if the designation update was valid, false if not</returns>
        public bool changeProductDesignation(string designation){
            if(Strings.isNullOrEmpty(designation))return false;
            this.designation=designation;
            return true;
        }

        /// <summary>
        /// Removes a specified width dimension from the current product
        /// </summary>
        /// <param name="widthDimension">Dimension with the width dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeWidthDimension(Dimension widthDimension){return widthValues.Remove(widthDimension);}

        /// <summary>
        /// Removes a specified height dimension from the current product
        /// </summary>
        /// <param name="heightDimension">Dimension with the height dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeHeightDimension(Dimension heightDimension){return heightValues.Remove(heightDimension);}

        /// <summary>
        /// Removes a specified depth dimension from the current product
        /// </summary>
        /// <param name="depthDimension">Dimension with the depth dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeDepthDimension(Dimension depthDimension){return depthValues.Remove(depthDimension);}

        /// <summary>
        /// Removes a material which the current product can be made of
        /// </summary>
        /// <param name="material">Material with the material being removed</param>
        /// <returns>boolean true if the material was removed with success, false if not</returns>
        public bool removeMaterial(Material material){return productMaterials.Remove(new ProductMaterial(this,material));}

        /// <summary>
        /// Removes a component which the current product can be complemented with
        /// </summary>
        /// <param name="component">Component with the component being removed</param>
        /// <returns>boolean true if the component was removed with success, false if not</returns>
        public bool removeComplementedProduct(Component component){return complementedProducts.Remove(component);}

        /// <summary>
        /// Disables the current product
        /// </summary>
        /// <returns>boolean true if the product was disabled with success, false if not</returns>
        public bool disable() {
            if (!isAvailable) return false;
            isAvailable = false;
            return true;
        }

        /// <summary>
        /// Returns the product identity
        /// </summary>
        /// <returns>string with the product identity</returns>
        public string id() { return reference; }

        /// <summary>
        /// Checks if a certain product entity is the same as the current product
        /// </summary>
        /// <param name="comparingEntity">string with the comparing product identity</param>
        /// <returns>boolean true if both entities identity are the same, false if not</returns>
        public bool sameAs(string comparingEntity) { return id().Equals(comparingEntity); }

        /// <summary>
        /// Returns the current product as a DTO
        /// </summary>
        /// <returns>DTO with the current DTO representation of the product</returns>
        public ProductDTO toDTO() {
            ProductDTO dto = new ProductDTO();

            dto.id = this.Id;
            dto.designation = this.designation;
            dto.reference = this.reference;
            dto.dimensions.heightDimensionDTOs = new List<DimensionDTO>(DTOUtils.parseToDTOS(heightValues)); 
            dto.dimensions.widthDimensionDTOs = new List<DimensionDTO>(DTOUtils.parseToDTOS(widthValues));
            dto.dimensions.depthDimensionDTOs = new List<DimensionDTO>(DTOUtils.parseToDTOS(depthValues));
            dto.productCategory = productCategory.toDTO();

            //TODO: remove null check once complement database mappping is complete
            if(complementedProducts != null && complementedProducts.Count >= 0){
                List<ComponentDTO> complementDTOList = new List<ComponentDTO>();

                foreach (Component complement in complementedProducts) {
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
        public override string ToString() {
            //Should ToString List the Product Complemented Products?
            return String.Format("Product Information\n- Designation: {0}\n- Reference: {1}", designation, reference);
        }

        /// <summary>
        /// Checks if two products are equal
        /// </summary>
        /// <param name="comparingProduct">Product with the product being compared to the current one</param>
        /// <returns>boolean true if both products are equal, false if not</returns>
        public override bool Equals(object comparingProduct) {
            if (this == comparingProduct) return true;
            return comparingProduct is Product && this.id().Equals(((Product)comparingProduct).id());
        }

        /// <summary>
        /// Represents the product hashcode
        /// </summary>
        /// <returns>Integer with the current product hashcode</returns>
        public override int GetHashCode() {
            return id().GetHashCode();
        }

        /// <summary>
        /// Checks if a complemented product is valid for additon on the current product
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product being validated</param>
        /// <returns>boolean true if the complemented product is valid for addition, false if not</returns>
        private bool isComplementedProductValidForAddition(Component complementedProduct) {
            if (complementedProduct == null || complementedProduct.Equals(this)) return false;
            return !complementedProducts.Contains(complementedProduct);
        }

        /// <summary>
        /// Checks if a product material is valid for addition on the current product
        /// </summary>
        /// <param name="productMaterial">Material with the product material being validated</param>
        /// <returns>boolean true if the product material is valid for addition, false if not</returns>
        private bool isProductMaterialValidForAddition(Material productMaterial) {
            if (productMaterial == null) {
                return false;
            }
            foreach (ProductMaterial prodM in this.productMaterials) {
                if (prodM.hasMaterial(productMaterial)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a product dimension is valid for addition on the current product
        /// </summary>
        /// <param name="productDimension">Dimension being validated</param>
        /// <param name="productDimensions">IEnumerable with the product dimensions</param>
        /// <returns>boolean true if the dimension is valid for addition, false if not</returns>
        private bool isProductDimensionValidForAddition(Dimension productDimension, ICollection<Dimension> productDimensions) {
            return productDimension != null && !productDimensions.Contains(productDimension);
        }

        /// <summary>
        /// Checks if the product properties are valid
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        private void checkProductProperties(string reference, string designation) {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
        }

        /// <summary>
        /// Checks if the products which a product can be complemented by are valid
        /// </summary>
        /// <param name="complementedProducts">IEnumerable with the complemented products</param>
        private void checkProductComplementedProducts(IEnumerable<Component> complementedProducts) {
            if (Collections.isEnumerableNullOrEmpty(complementedProducts))
                throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTED_PRODUCTS);
            checkDuplicatedComplementedProducts(complementedProducts);
        }

        /// <summary>
        /// Checks if the materials which a product can be made of are valid
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials</param>
        private void checkProductMaterials(IEnumerable<Material> productMaterials) {
            if (Collections.isEnumerableNullOrEmpty(productMaterials))
                throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
            checkDuplicatedMaterials(productMaterials);
        }

        /// <summary>
        /// Checks if the product dimensions are valid
        /// </summary>
        /// <param name="productDimensions">IEnumerable with the product dimensions</param>
        private void checkProductDimensions(IEnumerable<Dimension> productDimensions) {
            if (Collections.isEnumerableNullOrEmpty(productDimensions))
                throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
            checkDuplicatedDimensions(productDimensions);
        }

        /// <summary>
        /// Checks if the product category is valid
        /// </summary>
        /// <param name="productCategory">ProductCategory with the product category</param>
        private void checkProductCategory(ProductCategory productCategory) {
            if (productCategory == null) throw new ArgumentException(INVALID_PRODUCT_CATEGORY);
        }

        /// <summary>
        /// Checks if a enumerable of products has duplicates
        /// </summary>
        /// <param name="complementedProducts">IEnumerable with the complemented products</param>
        private void checkDuplicatedComplementedProducts(IEnumerable<Component> complementedProducts) {
            HashSet<string> complementedProductsRefereces = new HashSet<string>();
            IEnumerator<Component> complementedProductsEnumerator = complementedProducts.GetEnumerator();
            Component complementedProduct = complementedProductsEnumerator.Current;
            while (complementedProductsEnumerator.MoveNext()) {
                complementedProduct = complementedProductsEnumerator.Current;
                if (!complementedProductsRefereces.Add(complementedProduct.id())) {
                    throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTED_PRODUCTS);
                }
            }
        }

        /// <summary>
        /// Checks if an enumerable of materials has duplicates
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials</param>
        private void checkDuplicatedMaterials(IEnumerable<Material> productMaterials) {
            HashSet<string> productMaterialsReferences = new HashSet<string>();
            IEnumerator<Material> productMaterialsEnumerator = productMaterials.GetEnumerator();
            Material productMaterial = productMaterialsEnumerator.Current;
            while (productMaterialsEnumerator.MoveNext()) {
                productMaterial = productMaterialsEnumerator.Current;
                if (!productMaterialsReferences.Add(productMaterial.id())) {
                    throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
                }
            }
        }

        /// <summary>
        /// Checks if an enumerable of dimensions has duplicates
        /// </summary>
        /// <param name="productDimension">IEnumerable with product dimensions</param>
        private void checkDuplicatedDimensions(IEnumerable<Dimension> productDimensions) {
            HashSet<int> productDimensionsHashCodes = new HashSet<int>();
            IEnumerator<Dimension> productDimensionsEnumerator = productDimensions.GetEnumerator();
            Dimension nextDimension = productDimensionsEnumerator.Current;
            while (productDimensionsEnumerator.MoveNext()) {
                nextDimension = productDimensionsEnumerator.Current;
                if (!productDimensionsHashCodes.Add(nextDimension.GetHashCode())) {
                    throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
                }
            }
        }

        /// <summary>
        /// Inner static class which represents the Product properties used to map on data holders (e.g. DTO)
        /// </summary>
        public static class Properties {
            /// <summary>
            /// Constant that represents the context of the properties
            /// </summary>
            public const string CONTEXT = "Product";
            /// <summary>
            /// Constant that represents the name of the property which maps the product designation
            /// </summary>
            public const string DESIGNATION_PROPERTY = "designation";
            /// <summary>
            /// Constant that represents the name of the property which maps the product reference
            /// </summary>
            public const string REFERENCE_PROPERTY = "reference";
            /// <summary>
            /// Constant that represents the name of the property which maps the product persistence identifier
            /// </summary>
            public const string PERSISTENCE_ID_PROPERTY = "persistence_id";
            /// <summary>
            /// Constant that represents the name of the property which maps the product materials
            /// </summary>
            public const string MATERIALS_PROPERTY = "materials";
            /// <summary>
            /// Constant that represents the name of the property which maps the product complemented products
            /// </summary>
            public const string COMPLEMENTED_PRODUCTS_PROPERTY = "complemented_products";
            /// <summary>
            /// Constant that represents the name of the property which maps the product height values
            /// </summary>
            public const string HEIGHT_VALUES_PROPERTIES = "height_values";
            /// <summary>
            /// Constant that represents the name of the property which maps the product width values
            /// </summary>
            public const string WIDTH_VALUES_PROPERTIES = "width_values";
            /// <summary>
            /// Constant that represents the name of the property which maps the product depth values
            /// </summary>
            public const string DEPTH_VALUES_PROPERTIES = "depth_values";
        }
        /*
        /// <summary>
        /// Represents a builder of products
        /// </summary>
        /// <typeparam name="Product">Generic-Type of the Product entity</typeparam>
        public class ProductBuilder : Builder<Product> {
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
            public static ProductBuilder create() { return new ProductBuilder(); }

            /// <summary>
            /// Adds a reference to the current product builder
            /// </summary>
            /// <param name="reference">string with the product reference</param>
            /// <returns>ProductBuilder with the updated builder</returns>            
            public ProductBuilder withReference(string reference) {
                builderDTO.reference = reference;
                return this;
            }

            /// <summary>
            /// Adds a designation to the current product builder
            /// </summary>
            /// <param name="designation">string with the product designation</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withDesignation(string designation) {
                builderDTO.designation = designation;
                return this;
            }

            /// <summary>
            /// Adds complemented products to the current product builder
            /// </summary>
            /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withComplementedProducts(IEnumerable<ComponentDTO> complementedProducts) {
                builderDTO.complements = new List<ComponentDTO>(complementedProducts);
                return this;
            }

            /// <summary>
            /// Adds product category to the current product builder
            /// </summary>
            /// <param name="productCategory">ProductCategory with the product category</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withProductCategory(ProductCategoryDTO productCategory) {
                builderDTO.productCategory = productCategory;
                return this;
            }

            /// <summary>
            /// Adds materials to the product builder
            /// </summary>
            /// <param name="materials">IEnumerable with the current product materials</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withMaterials(IEnumerable<MaterialDTO> materials) {
                builderDTO.productMaterials = new List<MaterialDTO>(materials);
                return this;
            }

            /// <summary>
            /// Adds height dimensions to the product builder
            /// </summary>
            /// <param name="heightDimensions">IEnumerable with the current product height dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withHeightDimensions(IEnumerable<DimensionDTO> heightDimensions) {
                builderDTO.heightDimensions = new List<DimensionDTO>(heightDimensions);
                return this;
            }

            /// <summary>
            /// Adds depth dimensions to the product builder
            /// </summary>
            /// <param name="depthDimensions">IEnumerable with the current product depth dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withDepthDimensions(IEnumerable<DimensionDTO> depthDimensions) {
                builderDTO.depthDimensions = new List<DimensionDTO>(depthDimensions);
                return this;
            }

            /// <summary>
            /// Adds width dimensions to the product builder
            /// </summary>
            /// <param name="widthDimensions">IEnumerable with the current product width dimensions</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withWidthDimensions(IEnumerable<DimensionDTO> widthDimensions) {
                builderDTO.widthDimensions = new List<DimensionDTO>(widthDimensions);
                return this;
            }

            /// <summary>
            /// Builds a new Product based on the builder input
            /// </summary>
            /// <returns>Product with the product based on the builder input</returns>
            public Product build() {
                IEnumerable<ComponentDTO> complementedProducts = builderDTO.complements;
                if (complementedProducts == null) {
                    return new Product(builderDTO.reference
                                    , builderDTO.designation
                                    , DTOUtils.reverseDTO(builderDTO.productCategory)
                                    , DTOUtils.reverseDTOS(builderDTO.productMaterials)
                                    , DTOUtils.reverseDTOS(builderDTO.heightDimensions)
                                    , DTOUtils.reverseDTOS(builderDTO.widthDimensions)
                                    , DTOUtils.reverseDTOS(builderDTO.depthDimensions));
                } else {
                    return new Product(builderDTO.reference
                                    , builderDTO.designation
                                    , DTOUtils.reverseDTO(builderDTO.productCategory)
                                    , DTOUtils.reverseDTOS(builderDTO.productMaterials)
                                    , DTOUtils.reverseDTOS(complementedProducts)
                                    , DTOUtils.reverseDTOS(builderDTO.heightDimensions)
                                    , DTOUtils.reverseDTOS(builderDTO.widthDimensions)
                                    , DTOUtils.reverseDTOS(builderDTO.depthDimensions));
                }
            }
            /// <summary>
            /// Hides default constructor
            /// </summary>
            private ProductBuilder() { builderDTO = new ProductDTO(); }
        }*/
    }
}