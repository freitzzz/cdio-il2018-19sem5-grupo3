using support.builders;
using support.domain;
using support.domain.ddd;
using support.dto;
using support.options;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;
using core.dto.options;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain {

    /// <summary>
    /// Class that represents a Product.
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="Product"></typeparam>
    /// <typeparam name="ProductDTO">Type of DTO being used</typeparam>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product : AggregateRoot<string>, DTOAble<ProductDTO>,DTOAbleOptions<ProductDTO,ProductDTOOptions> {
        /// <summary>
        /// Constant that represents the message that occurs if the product reference is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The product reference is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product designation is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The product designation is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_COMPLEMENTED_PRODUCTS = "The products which the product can be complemented by are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product complemented products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_MATERIALS = "The materials which the product can be made of are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product restrinctions are invalid
        /// </summary>
        private const string INVALID_PRODUCT_DIMENSIONS = "The product dimensions are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product category is invalid
        /// </summary>
        private const string INVALID_PRODUCT_CATEGORY = "The product category is invalid";

        /// <summary>
        /// Constant that represents the message that occurs if the slots minimum size is larger than the slots maximum size
        /// </summary>
        private const string INVALID_MIN_TO_MAX_SLOT_SIZE_RATIO = "The product's minimum slot size can't be larger than the maximum slot size";

        /// <summary>
        /// Constant that represents the message that occurs if the slots recommended size is larger than the slots maximum size
        /// </summary>
        private const string INVALID_RECOMMENDED_TO_MAX_SLOT_SIZE_RATIO = "The product's recommended slot size can't be larger than the maximum slot size";

        /// <summary>
        /// Constant that represents the message that occurs if the slots recommended size is smaller than the slots minimum size
        /// </summary>
        private const string INVALID_RECOMMENDED_TO_MIN_SLOT_SIZE_RATIO = "The product's recommended slot size can't be smaller than the minimum slot size";

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
        private List<Component> _complementedProducts;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<Component> complementedProducts{get=>LazyLoader.Load(this,ref _complementedProducts);protected set=>_complementedProducts=value;}
        /// <summary>
        /// List with the materials which the product can be made of
        /// </summary>
        //TODO: Should product materials be a list or a set?
        private List<ProductMaterial> _productMaterials;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<ProductMaterial> productMaterials{get=>LazyLoader.Load(this,ref _productMaterials);protected set=>_productMaterials=value;}
        /// <summary>
        /// List with the product heigth dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        private List<Dimension> _heightValues;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<Dimension> heightValues{get=>LazyLoader.Load(this,ref _heightValues);protected set=>_heightValues=value;}
        /// <summary>
        /// List with the product width dimensions
        /// </summary>
        //TODO: Should product dimensions be a list or a set
        private List<Dimension> _widthValues;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<Dimension> widthValues{get=>LazyLoader.Load(this,ref _widthValues);protected set=>_widthValues=value;}
        /// <summary>
        /// List with the product depth dimensions
        /// </summary>
        //TODO: Should product restrinctions be a list or a set
        private List<Dimension> _depthValues;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<Dimension> depthValues{get=>LazyLoader.Load(this,ref _depthValues);protected set=>_depthValues=value;}
        /// <summary>
        /// ProductCategory with the category which the product belongs to
        /// </summary>
        private ProductCategory _productCategory;//!private field used for lazy loading, do not use this for storing or fetching data
        public ProductCategory productCategory{get=>LazyLoader.Load(this,ref _productCategory);protected set=>_productCategory=value;}

        /// <summary>
        /// Boolean that controls if the current product is available or not
        /// </summary>
        public bool isAvailable { get; protected set; }

        /// <summary>
        /// CustomizedDimensions that represents the maximum size of the slots
        /// </summary>
        private CustomizedDimensions _maxSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public  CustomizedDimensions maxSlotSize{get=>LazyLoader.Load(this,ref _maxSlotSize);protected set=>_maxSlotSize=value;}

        /// <summary>
        /// CustomizedDimensions that represents the minimum size of the slots
        /// </summary>
        private CustomizedDimensions _minSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public  CustomizedDimensions minSlotSize{get=>LazyLoader.Load(this,ref _minSlotSize);protected set=>_minSlotSize=value;}

        /// <summary>
        /// CustomizedDimensions that represents the recommended size of the slots
        /// </summary>
        private CustomizedDimensions _recommendedSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public  CustomizedDimensions recommendedSlotSize{get=>LazyLoader.Load(this,ref _recommendedSlotSize);protected set=>_recommendedSlotSize=value;}

        /// <summary>
        /// Booelan that indicates if the product can hold slots
        /// </summary>
        public bool supportsSlots {get;protected set;}

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader.</value>
        private ILazyLoader LazyLoader{get;set;}

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Product() { }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Product(ILazyLoader lazyLoader){this.LazyLoader=lazyLoader;}

        /// <summary>
        /// Builds a new product with its reference, designation, maximum number of slots, its category,
        /// the materials it can be made of and its dimensions.
        /// </summary>
        /// <param name="reference">Reference of the Product</param>
        /// <param name="designation">Designation of the Product</param>
        /// <param name="supportsSlots">Indicates if the product can hold slots</param>
        /// <param name="maxSlotSize">Maximum slot dimensions</param>
        /// <param name="minSlotSize">Minimum slot dimensions</param>
        /// <param name="recommendedSlotSize">Recommended slot dimensions</param>
        /// <param name="productCategory">ProductCategory with the product's category</param>
        /// <param name="materials">Materials the product can be made of</param>
        /// <param name="heightDimensions">Product height dimensions</param>
        /// <param name="widthDimensions">Product width dimensions</param>
        /// <param name="depthDimensions">Product depth dimensions</param>
        public Product(string reference, string designation, bool supportsSlots,
                        CustomizedDimensions maxSlotSize, CustomizedDimensions minSlotSize,
                        CustomizedDimensions recommendedSlotSize, ProductCategory productCategory,
                        IEnumerable<Material> materials, IEnumerable<Dimension> heightDimensions,
                        IEnumerable<Dimension> widthDimensions, IEnumerable<Dimension> depthDimensions) :
                        this(reference,designation,productCategory,
                        materials,heightDimensions,widthDimensions,
                        depthDimensions){
                            this.supportsSlots = supportsSlots;
                            checkProductSlotsDimensions(maxSlotSize, minSlotSize, recommendedSlotSize);
                            this.maxSlotSize = maxSlotSize;
                            this.minSlotSize = minSlotSize;
                            this.recommendedSlotSize = recommendedSlotSize;
                        }

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
        public Product(string reference, string designation,
                        ProductCategory productCategory,
                        IEnumerable<Material> materials,
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
            this.isAvailable = true;
            this.supportsSlots = false;
            this.maxSlotSize = CustomizedDimensions.valueOf(0, 0, 0);
            this.minSlotSize = CustomizedDimensions.valueOf(0, 0, 0);
            this.recommendedSlotSize = CustomizedDimensions.valueOf(0, 0, 0);
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
        public Product(string reference, string designation,
                        ProductCategory productCategory, 
                        IEnumerable<Material> materials,
                        IEnumerable<Product> complementedProducts,
                        IEnumerable<Dimension> heightValues,
                        IEnumerable<Dimension> widthValues,
                        IEnumerable<Dimension> depthValues) : 
                        this (reference, designation, productCategory, materials, heightValues, widthValues, depthValues) {
            checkProductComplementedProducts(complementedProducts);
            this.complementedProducts = new List<Component>();
            foreach(Product complementedProduct in complementedProducts){
                this.complementedProducts.Add(new Component(this,complementedProduct));
            }
        }

        /// <summary>
        /// Builds a new product with its reference, designation and complemented products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="supportsSlots">Indicates if the product can hold slots</param>
        /// <param name="maxSlotSize">Maximum slot dimensions</param>
        /// <param name="minSlotSize">Minimum slot dimensions</param>
        /// <param name="recommendedSlotSize">Recommended slot dimensions</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="complementedProducts">IEnumerable with the product complemented products</param>
        /// <param name="heightDimensions">IEnumerable with the product height dimensions</param>
        /// <param name="widthDimensions">IEnumerable with the product width dimensions</param>
        /// <param name="depthDimensions">IEnumerable with the product depth dimensions</param>
        public Product(string reference, string designation,
                        bool supportsSlots,
                        CustomizedDimensions maxSlotSize,
                        CustomizedDimensions minSlotSize,
                        CustomizedDimensions recommendedSlotSize,
                        ProductCategory productCategory, 
                        IEnumerable<Material> materials,
                        IEnumerable<Product> complementedProducts,
                        IEnumerable<Dimension> heightValues,
                        IEnumerable<Dimension> widthValues,
                        IEnumerable<Dimension> depthValues) : 
                        this (reference, designation, supportsSlots, maxSlotSize, minSlotSize,
                        recommendedSlotSize, productCategory, materials, heightValues, widthValues,
                        depthValues) {
            checkProductComplementedProducts(complementedProducts);
            this.complementedProducts = new List<Component>();
            foreach(Product complementedProduct in complementedProducts){
                this.complementedProducts.Add(new Component(this,complementedProduct));
            }
        }

        /// <summary>
        /// Adds a new product which the current product can be complemented by
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product</param>
        /// <returns>boolean true if the complemented product was added with success, false if not</returns>
        public bool addComplementedProduct(Product complementedProduct) {
            if (!isComplementedProductValidForAddition(complementedProduct))
                return false;
            complementedProducts.Add(new Component(this,complementedProduct));
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
        public bool changeProductReference(string reference) {
            if (Strings.isNullOrEmpty(reference) || this.reference.Equals(reference)) return false;
            this.reference = reference;
            return true;
        }

        /// <summary>
        /// Changes the current product designation
        /// </summary>
        /// <param name="designation">String with the designation being updated</param>
        /// <returns>boolean true if the designation update was valid, false if not</returns>
        public bool changeProductDesignation(string designation) {
            if (Strings.isNullOrEmpty(designation) || this.designation.Equals(designation)) return false;
            this.designation = designation;
            return true;
        }

        /// <summary>
        /// Removes a specified width dimension from the current product
        /// </summary>
        /// <param name="widthDimension">Dimension with the width dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeWidthDimension(Dimension widthDimension) { return !Collections.isEnumerableNullOrEmpty(widthValues) && widthValues.Remove(widthDimension); }

        /// <summary>
        /// Removes a specified height dimension from the current product
        /// </summary>
        /// <param name="heightDimension">Dimension with the height dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeHeightDimension(Dimension heightDimension) { return !Collections.isEnumerableNullOrEmpty(heightValues) && heightValues.Remove(heightDimension); }

        /// <summary>
        /// Removes a specified depth dimension from the current product
        /// </summary>
        /// <param name="depthDimension">Dimension with the depth dimension being removed</param>
        /// <returns>boolean true if the dimension was removed with success, false if not</returns>
        public bool removeDepthDimension(Dimension depthDimension) { return !Collections.isEnumerableNullOrEmpty(depthValues) && depthValues.Remove(depthDimension); }

        /// <summary>
        /// Removes a material which the current product can be made of
        /// </summary>
        /// <param name="material">Material with the material being removed</param>
        /// <returns>boolean true if the material was removed with success, false if not</returns>
        public bool removeMaterial(Material material) { return !Collections.isEnumerableNullOrEmpty(productMaterials) && productMaterials.Remove(new ProductMaterial(this, material)); }

        /// <summary>
        /// Removes a complemented which the current product can be complemented with
        /// </summary>
        /// <param name="complementedProduct">Product with the complemented product being removed</param>
        /// <returns>boolean true if the complemented product was removed with success, false if not</returns>
        public bool removeComplementedProduct(Product complementedProduct) { return complementedProducts.Remove(new Component(this,complementedProduct)); }

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
            return toDTO(new ProductDTOOptions());
        }

        /// <summary>
        /// Returns the DTO representation of the current product with a set of options
        /// </summary>
        /// <param name="dtoOptions">O with the set of options being applied</param>
        /// <returns>D with the DTO of the current product with the applied options</returns>
        public ProductDTO toDTO(ProductDTOOptions dtoOptions){
            ProductDTO dto = new ProductDTO();

            dto.id = this.Id;
            dto.designation = this.designation;
            dto.reference = this.reference;
            dto.productCategory = productCategory.toDTO();

            DimensionsListDTO dimensionsListDTO=new DimensionsListDTO();
            if(dtoOptions.requiredUnit==null){
                dimensionsListDTO.heightDimensionDTOs=new List<DimensionDTO>(DTOUtils.parseToDTOS(heightValues));
                dimensionsListDTO.widthDimensionDTOs=new List<DimensionDTO>(DTOUtils.parseToDTOS(widthValues));
                dimensionsListDTO.depthDimensionDTOs=new List<DimensionDTO>(DTOUtils.parseToDTOS(depthValues));
            }else{
                dimensionsListDTO.heightDimensionDTOs=new List<DimensionDTO>();
                dimensionsListDTO.widthDimensionDTOs=new List<DimensionDTO>();
                dimensionsListDTO.depthDimensionDTOs=new List<DimensionDTO>();
                foreach(Dimension dimension in heightValues)dimensionsListDTO.heightDimensionDTOs.Add(dimension.toDTO(dtoOptions.requiredUnit));
                foreach(Dimension dimension in widthValues)dimensionsListDTO.widthDimensionDTOs.Add(dimension.toDTO(dtoOptions.requiredUnit));
                foreach(Dimension dimension in depthValues)dimensionsListDTO.depthDimensionDTOs.Add(dimension.toDTO(dtoOptions.requiredUnit));
            }
            dto.dimensions=dimensionsListDTO;
            
            dto.productMaterials=new List<MaterialDTO>();
            foreach (ProductMaterial pm in this.productMaterials) {
                dto.productMaterials.Add(pm.material.toDTO());
            }
            //TODO: remove null check once complement database mappping is complete
            if (complementedProducts != null && complementedProducts.Count >= 0) {
                List<ComponentDTO> complementDTOList = new List<ComponentDTO>();

                foreach (Component complement in complementedProducts) {
                    complementDTOList.Add(complement.toDTO());
                }
                dto.complements = complementDTOList;
            }

            if(this.supportsSlots){
                dto.slotDimensions.maximumSlotDimensions = this.maxSlotSize.toDTO();
                dto.slotDimensions.minimumSlotDimensions = this.minSlotSize.toDTO();
                dto.slotDimensions.recommendedSlotDimensions = this.recommendedSlotSize.toDTO();
            }

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
        private bool isComplementedProductValidForAddition(Product complementedProduct) {
            if (complementedProduct == null || complementedProduct.Equals(this)) return false;
            return !complementedProducts.Contains(new Component(this,complementedProduct));
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
        /// Checks if the product's slot sizes are valid (non-negative values with proper ratios between them).
        /// </summary>
        /// <param name="maxSlotSize">CustomizedDimensions with the maximum size of the slots</param>
        /// <param name="minSlotSize">CustomizedDimensions with the minimum size of the slots</param>
        /// <param name="recommendedSlotSize">CustomizedDimensions with the recommended size of the slots</param>
        private void checkProductSlotsDimensions(CustomizedDimensions maxSlotSize, CustomizedDimensions minSlotSize, CustomizedDimensions recommendedSlotSize){
            if(minSlotSize.depth > maxSlotSize.depth || minSlotSize.height > maxSlotSize.height || minSlotSize.width > maxSlotSize.width)
                throw new ArgumentException(INVALID_MIN_TO_MAX_SLOT_SIZE_RATIO);

            if(recommendedSlotSize.depth > maxSlotSize.depth || recommendedSlotSize.height > maxSlotSize.height || recommendedSlotSize.width > maxSlotSize.width)
                throw new ArgumentException(INVALID_RECOMMENDED_TO_MAX_SLOT_SIZE_RATIO);

            if(recommendedSlotSize.depth < minSlotSize.depth || recommendedSlotSize.height < minSlotSize.height || recommendedSlotSize.width < minSlotSize.width)
                throw new ArgumentException(INVALID_RECOMMENDED_TO_MIN_SLOT_SIZE_RATIO);
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
        private void checkProductComplementedProducts(IEnumerable<Product> complementedProducts) {
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
        private void checkDuplicatedComplementedProducts(IEnumerable<Product> complementedProducts) {
            HashSet<string> complementedProductsRefereces = new HashSet<string>();
            IEnumerator<Product> complementedProductsEnumerator = complementedProducts.GetEnumerator();
            Product complementedProduct = complementedProductsEnumerator.Current;
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