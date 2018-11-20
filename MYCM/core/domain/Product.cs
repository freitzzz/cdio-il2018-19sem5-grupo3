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
using System.Linq;

namespace core.domain {

    /// <summary>
    /// Class that represents a Product.
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="Product"></typeparam>
    /// <typeparam name="ProductDTO">Type of DTO being used</typeparam>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product : Activatable, AggregateRoot<string>, DTOAble<ProductDTO>, DTOAbleOptions<ProductDTO, ProductDTOOptions> {
        /// <summary>
        /// Constant that represents the message that occurs if the product reference is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The product reference is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product designation is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The product designation is invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product complementary products are invalid
        /// </summary>
        private const string INVALID_PRODUCT_COMPLEMENTARY_PRODUCTS = "The products which the product can be complemented by are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the product component is not valid
        /// </summary>
        private const string INVALID_COMPONENT = "The component is not valid!";
        /// <summary>
        /// Constant that represents the message that occurs if the product complementary products are invalid
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
        /// Constant that represents the message that ocurrs if the product reference change is invalid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE_CHANGE="The product reference being changed is the same as the actual one";

        /// <summary>
        /// Constant that represents the message that ocurrs if the product designation change is invalid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION_CHANGE="The product designation being changed is the same as the actual one";

        /// <summary>
        /// Constant that represents the message that ocurrs if the product category change is invalid
        /// </summary>
        private const string INVALID_PRODUCT_CATEGORY_CHANGE="The product category being changed is the same as the actual one";

        /// <summary>
        /// Constant that represents the message that is presented if the given maximum slot dimensions are null.
        /// </summary>
        private const string MAXIMUM_SLOT_DIMENSIONS_NULL = "The maximum slot dimensions can not be null";
        /// <summary>
        /// Constant that represents the message that is presented if the given minimum slot dimensions are null.
        /// </summary>
        private const string MINIMUM_SLOT_DIMENSONS_NULL = "The minimum slot dimensions can not be null";
        /// <summary>
        /// Constant that represents the message that is presented if the given recommended slot dimensions are null.
        /// </summary>
        private const string RECOMMENDED_SLOT_DIMENSIONS_NULL = "The recommended slot dimensions can not be null";

        /// <summary>
        /// Constant that represents the message that is presented if a null Material is attempted to be added.
        /// </summary>
        private const string MATERIAL_NULL = "The given material can not be null.";
        /// <summary>
        /// Constant that represents the message that is presented if a duplicate Material is attempted to be added.
        /// </summary>
        private const string MATERIAL_ALREADY_ADDED = "An equal material has already been added.";
        /// <summary>
        /// Constant that represents the message that is presented if the last Material is attempted to be removed.
        /// </summary>
        private const string MATERIAL_UNABLE_TO_REMOVE_LAST = "Unable to remove the last material, the product needs to have atleast one material.";
        /// <summary>
        /// Constant that represents the message that is presented if the Material could not be removed.
        /// </summary>
        private const string MATERIAL_UNABLE_TO_REMOVE = "The given material could not be removed.";

        /// <summary>
        /// Constant that represents the message that is presented if a null complementary Product is attempted to be added.
        /// </summary>
        private const string COMPLEMENTARY_PRODUCT_NULL = "The given complementary product can not be null.";
        /// <summary>
        /// Constant that represents the message that is presented if the complementary Product being added is equal to the parent Product.
        /// </summary>
        private const string COMPLEMENTARY_PRODUCT_EQUALS_PRODUCT = "The given complementary product can not be equal to the parent product";
        /// <summary>
        /// Constant that represents the messsage that is presented if a duplicate complementary Product is attempted to be added.
        /// </summary>
        private const string COMPLEMENTARY_PRODUCT_ALREADY_ADDED = "An equal complementary product has already been added.";
        /// <summary>
        /// Constant that represents the message that is presented if a complementary Product could not be removed.
        /// </summary>
        private const string COMPLEMENTARY_PRODUCT_UNABLE_TO_REMOVE = "The given complementary product could not be removed.";

        /// <summary>
        /// Constant that represents the message that is presented if a null Measurement is attempted to be added.
        /// </summary>
        private const string MEASUREMENT_NULL = "The given measurement can not be null.";
        /// <summary>
        /// Constant that represents the message that is presented if a duplicate Measurement is attempted to be added.
        /// </summary>
        private const string MEASUREMENT_ALREADY_ADDED = "An equal measurement has already been added.";
        /// <summary>
        /// Constant that represents the message that is presented if the last Measurement is attempted to be removed.
        /// </summary>
        private const string MEASUREMENT_UNABLE_TO_REMOVE_LAST = "Unable to remove the last measurement, the product needs to have atleast one measurement.";
        /// <summary>
        /// Constant that represents the message that is presented if the Measurement could not be removed.
        /// </summary>
        private const string MEASUREMENT_UNABLE_TO_REMOVE = "The given measurement could not be removed.";

        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id { get; internal set; }   //the id should have an internal set, since DTO's have to be able to set them

        /// <summary>
        /// String with the product reference
        /// </summary>
        /// <value>Gets/protected sets the reference value.</value>
        public string reference { get; protected set; }
        /// <summary>
        /// String with the product designation
        /// </summary>
        /// <value>Gets/protected sets the designation value.</value>
        public string designation { get; protected set; }
        /// <summary>
        /// List with the components which the current product can be complemented by
        /// </summary>
        /// <value>Gets/protected sets the value of the Component list.</value>
        //TODO: Should complementary products be a list and not a set?
        private List<Component> _components;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<Component> components { get => LazyLoader.Load(this, ref _components); protected set => _components = value; }
        /// <summary>
        /// List with the materials which the product can be made of
        /// </summary>
        /// <value>Gets/protected sets the value of the ProductMaterial list.</value>
        //TODO: Should product materials be a list or a set?
        private List<ProductMaterial> _productMaterials;//!private field used for lazy loading, do not use this for storing or fetching data
        public List<ProductMaterial> productMaterials { get => LazyLoader.Load(this, ref _productMaterials); protected set => _productMaterials = value; }

        /// <summary>
        /// List containg all of the Product's measurements.
        /// </summary>
        /// <value>Gets/sets the measurements list value.</value>
        private List<ProductMeasurement> _productMeasurements;
        public List<ProductMeasurement> productMeasurements {get => LazyLoader.Load(this, ref _productMeasurements); protected set => _productMeasurements = value;}

        /// <summary>
        /// ProductCategory with the category which the product belongs to
        /// </summary>
        /// <value>Gets/protected sets the ProductCategory's value.</value>
        private ProductCategory _productCategory;//!private field used for lazy loading, do not use this for storing or fetching data
        public ProductCategory productCategory { get => LazyLoader.Load(this, ref _productCategory); protected set => _productCategory = value; }

        /// <summary>
        /// CustomizedDimensions that represents the maximum size of the slots
        /// </summary>
        /// <value>Gets/protected sets the CustomizedDimension's value.</value>
        private CustomizedDimensions _maxSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions maxSlotSize { get => LazyLoader.Load(this, ref _maxSlotSize); protected set => _maxSlotSize = value; }

        /// <summary>
        /// CustomizedDimensions that represents the minimum size of the slots
        /// </summary>
        /// <value>Gets/protected sets the CustomizedDimension's value.</value>
        private CustomizedDimensions _minSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions minSlotSize { get => LazyLoader.Load(this, ref _minSlotSize); protected set => _minSlotSize = value; }

        /// <summary>
        /// CustomizedDimensions that represents the recommended size of the slots
        /// </summary>
        /// <value>Gets/protected sets the CustomizedDimension's value.</value>
        private CustomizedDimensions _recommendedSlotSize;//!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions recommendedSlotSize { get => LazyLoader.Load(this, ref _recommendedSlotSize); protected set => _recommendedSlotSize = value; }

        /// <summary>
        /// Booelan that indicates if the product can hold slots
        /// </summary>
        /// <value>Gets/protected sets the value of the supportsSlots flag.</value>
        public bool supportsSlots { get; protected set; }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructror used by the framework for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">ILazyLoader being injected.</param>
        public Product(ILazyLoader lazyLoader) 
        {
            this.LazyLoader = lazyLoader;
               
        }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Product() { }

        //*BASE CONSTRUCTOR (NO SLOT DIMENSIONS, NO COMPONENTS) */
        /// <summary>
        /// Builds a new product with its reference, designation and materials which it can be made of
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="measurements">IEnumerable with the product measurements</param>
        public Product(string reference, string designation,
                        ProductCategory productCategory,
                        IEnumerable<Material> materials,
                        IEnumerable<Measurement> measurements) {
            checkProductProperties(reference, designation);
            checkProductMaterials(materials);
            checkProductMeasurements(measurements);
            checkProductCategory(productCategory);
            this.reference = reference;
            this.designation = designation;
            this.productMaterials = new List<ProductMaterial>();
            foreach (Material mat in materials) {
                this.productMaterials.Add(new ProductMaterial(this, mat));
            }
            this.components = new List<Component>();
            this.productMeasurements = new List<ProductMeasurement>();
            foreach(Measurement measurement in measurements){
                this.productMeasurements.Add(new ProductMeasurement(this, measurement));
            }
            this.productCategory = productCategory;
            //!MaxValue assigned here because customized dimensions can't have value 0
            //TODO see if there's a better alternative to using Double.MaxValue
            this.maxSlotSize = CustomizedDimensions.valueOf(Double.MaxValue, Double.MaxValue, Double.MaxValue);
            this.minSlotSize = CustomizedDimensions.valueOf(Double.MaxValue, Double.MaxValue, Double.MaxValue);
            this.recommendedSlotSize = CustomizedDimensions.valueOf(Double.MaxValue, Double.MaxValue, Double.MaxValue);
        }

        //*CONSTRUCTOR WITH COMPONENTS */
        /// <summary>
        /// Builds a new product with its reference, designation and complementary products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="measurements">IEnumerable with the product measurements</param>
        /// <param name="complementaryProducts">IEnumerable with the product complementary products</param>
        public Product(string reference, string designation,
                        ProductCategory productCategory,
                        IEnumerable<Material> materials,
                        IEnumerable<Measurement> measurements,
                        IEnumerable<Product> complementaryProducts) :
                        this(reference, designation, productCategory, materials, measurements) {
            checkComplementaryProducts(complementaryProducts);
            this.components = new List<Component>();
            foreach (Product complementaryProduct in complementaryProducts) {
                this.components.Add(new Component(this, complementaryProduct));
            }
        }

        //*CONSTRUCTOR WITH SLOT DIMENSIONS */
        /// <summary>
        /// Builds a new product with its reference, designation, maximum number of slots, its category,
        /// the materials it can be made of and its dimensions.
        /// </summary>
        /// <param name="reference">Reference of the Product</param>
        /// <param name="designation">Designation of the Product</param>
        /// <param name="productCategory">ProductCategory with the product's category</param>
        /// <param name="materials">Materials the product can be made of</param>
        /// <param name="measurements">Product measurements</param>
        /// <param name="minSlotSize">Minimum slot dimensions</param>
        /// <param name="maxSlotSize">Maximum slot dimensions</param>
        /// <param name="recommendedSlotSize">Recommended slot dimensions</param>
        public Product(string reference, string designation,
                        ProductCategory productCategory,
                        IEnumerable<Material> materials, IEnumerable<Measurement> measurements,
                        CustomizedDimensions minSlotSize, CustomizedDimensions maxSlotSize, 
                        CustomizedDimensions recommendedSlotSize) :
                        this(reference, designation, productCategory, materials, measurements) {
            checkProductSlotsDimensions(maxSlotSize, minSlotSize, recommendedSlotSize);
            this.supportsSlots = true;
            this.minSlotSize = minSlotSize;
            this.maxSlotSize = maxSlotSize;
            this.recommendedSlotSize = recommendedSlotSize;
        }

        //*CONSTRUCTOR WITH SLOT DIMENSIONS AND COMPONENTS */
        /// <summary>
        /// Builds a new product with its reference, designation and complementary products
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <param name="maxSlotSize">Maximum slot dimensions</param>
        /// <param name="minSlotSize">Minimum slot dimensions</param>
        /// <param name="recommendedSlotSize">Recommended slot dimensions</param>
        /// <param name="productCategory">ProductCategory with the product category</param>
        /// <param name="materials">IEnumerable with the product materials which it can be made of</param>
        /// <param name="complementaryProducts">IEnumerable with the product complementary products</param>
        /// <param name="measurements">IEnumerable with the product measurements</param>
        public Product(string reference, string designation,
                        ProductCategory productCategory, IEnumerable<Material> materials, 
                        IEnumerable<Measurement> measurements, IEnumerable<Product> complementaryProducts,
                        CustomizedDimensions minSlotSize, CustomizedDimensions maxSlotSize,
                        CustomizedDimensions recommendedSlotSize) : 
                        this(reference, designation, productCategory, materials, measurements, complementaryProducts) {
            checkProductSlotsDimensions(maxSlotSize, minSlotSize, recommendedSlotSize);
            this.supportsSlots = true;
            this.minSlotSize = minSlotSize;
            this.maxSlotSize = maxSlotSize;
            this.recommendedSlotSize = recommendedSlotSize;
        }

        //*BEGINING OF ADD METHODS */

        /// <summary>
        /// Adds a new instance of Product which complements this instance.
        /// </summary>
        /// <param name="complementaryProduct">Instance of Product representing the complementary product.</param>
        /// <exception cref="System.ArgumentException">Thrown when the </exception>
        public void addComplementaryProduct(Product complementaryProduct) {
            checkIfComplementaryProductIsValidForAddition(complementaryProduct);
            components.Add(new Component(this, complementaryProduct));
        }

        /// <summary>
        /// Adds a new instance of Product which mandatorily complements this instance.
        /// </summary>
        /// <param name="complementaryProduct">Instance of Product representing the complementary product.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public void addMandatoryComplementaryProduct(Product complementaryProduct) {
            checkIfComplementaryProductIsValidForAddition(complementaryProduct);
            components.Add(new Component(this, complementaryProduct, true));
        }

        /// <summary>
        /// Adds a restriction to a component
        /// </summary>
        /// <param name="component">component to add restriction to</param>
        /// <param name="restriction">restriction to be added</param>
        /// <exception cref="System.ArgumentException">Thrown when the restriction could not be applied</exception>
        public void addComponentRestriction(Product component, Restriction restriction) {
            //TODO: validate restriction
            if (component == null) {
                throw new ArgumentNullException(INVALID_COMPONENT);
            }
            Component comp = this.components.Where(c => c.complementaryProduct.Equals(component)).SingleOrDefault();
            if(comp == null){
                
            }
            comp.addRestriction(restriction);
        }

        /// <summary>
        /// Adds a new material which the product can be made of
        /// </summary>
        /// <param name="productMaterial">Material with the product material</param>
        /// <returns>boolean true if the product material was added with success, false if not</returns>
        public void addMaterial(Material productMaterial) {
            checkIfMaterialIsValidForAddition(productMaterial);
            this.productMaterials.Add(new ProductMaterial(this, productMaterial));
        }

        public void addMaterialRestriction(Material material, Restriction restriction){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a Measurement to the Product's list of Measurement.
        /// </summary>
        /// <param name="measurement">Measurement being added.</param>
        /// <returns>Returns true if the Measurement is not null nor has it been previosuly added; false otherwise.</returns>
        public void addMeasurement(Measurement measurement){
            checkIfMeasurementIsValidForAddition(measurement);
            this.productMeasurements.Add(new ProductMeasurement(this, measurement));
        }

        public void addMeasurementRestriction(Measurement measurement, Restriction restriction){
            throw new NotImplementedException();
        }

        //*END OF ADD METHODS */


        //*BEGINNING OF CHANGE METHODS */

        /// <summary>
        /// Changes the current product reference
        /// </summary>
        /// <param name="reference">String with the reference being updated</param>
        public void changeProductReference(string reference) {
            checkIfProductReferenceIsValidForChange(reference);
            this.reference = reference;
        }

        /// <summary>
        /// Changes the current product designation
        /// </summary>
        /// <param name="designation">String with the designation being updated</param>
        public void changeProductDesignation(string designation) {
            checkIfProductDesignationIsValidForChange(designation);
            this.designation=designation;
        }

        /// <summary>
        /// Changes the current product category
        /// </summary>
        /// <param name="productCategory">ProductCategory with the new product category</param>
        public void changeProductCategory(ProductCategory productCategory) {
            checkIfProductCategoryIsValidForChange(productCategory);
            this.productCategory = productCategory;
        }

        //*END OF CHANGE METHODS */

        //*BEGINNING OF REMOVE METHODS */

        /// <summary>
        /// Removes an instance of Measurement from the Product's list of Measurement.
        /// </summary>
        /// <param name="measurement">Measurement being removed.</param>
        /// <exception cref="System.InvalidOperationException">If this instance does not contain more than one Measurement.</exception>
        /// <exception cref="System.ArgumentException">If the given instance of Measurement could not be removed.</exception>
        public void removeMeasurement(Measurement measurement){

            if(this.containsMeasurement(measurement) && this.productMeasurements.Count <= 1){
                //this operation is not allowed no matter the validity of the argument
                throw new InvalidOperationException(MEASUREMENT_UNABLE_TO_REMOVE_LAST);
            }

            if(!this.productMeasurements.Remove(
                this.productMeasurements.Where(pm => pm.measurement.Equals(measurement)).SingleOrDefault()
                )){
                    throw new ArgumentException(MEASUREMENT_UNABLE_TO_REMOVE);
                }
        }

        /// <summary>
        /// Removes a material which the current product can be made of
        /// </summary>
        /// <param name="material">Material with the material being removed</param>
        /// <exception cref="System.InvalidOperationException">If this instance does not contain more than one Material.</param>
        /// <exception cref="System.ArgumentException">If the given instance of Material could not be removed.</param>
        public void removeMaterial(Material material) {

            if(this.containsMaterial(material) && this.productMaterials.Count <= 1){
                //this operation is not valid no matter the validity of the argument
                throw new InvalidOperationException(MATERIAL_UNABLE_TO_REMOVE_LAST);
            }

            if(!this.productMaterials.Remove(
                this.productMaterials.Where(pm => pm.material.Equals(material)).SingleOrDefault()
                )){
                    throw new ArgumentException(MATERIAL_UNABLE_TO_REMOVE);
            }
        }

        /// <summary>
        /// Removes a complementary Product which the current Product can be complemented by
        /// </summary>
        /// <param name="complementaryProduct">Instance of Product which complements this instance.</param>
        /// <exception cref="System.ArgumentException">If the given instance of Product could not be removed.</exception>
        public void removecomplementaryProduct(Product complementaryProduct) {
            if(!this.components.Remove(
                this.components.Where(pc => pc.complementaryProduct.Equals(complementaryProduct)).SingleOrDefault()
                )){
                    throw new ArgumentException(COMPLEMENTARY_PRODUCT_UNABLE_TO_REMOVE);
            } 
        }

        //*END OF REMOVE METHODS */

        //*BEGINNING OF CONTAINS METHODS */

        /// <summary>
        /// Checks if this instance of Product has a given Material.
        /// </summary>
        /// <param name="material">Instance of Material being checked.</param>
        /// <returns>true if the product has the given Material; false, otherwise</returns>
        public bool containsMaterial(Material material) {
            foreach (ProductMaterial prodM in this.productMaterials) {
                if (prodM.hasMaterial(material)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if this instance of Product is complemented by a given Product.
        /// </summary>
        /// <param name="complementaryProduct">Instance of Product being checked.</param>
        /// <returns>true if this instance is complemented by the given Product; false, otherwise.</returns>
        public bool containsComplementaryProduct(Product complementaryProduct){
            return this.components.Where(comp => comp.complementaryProduct.Equals(complementaryProduct)).Any();
        }

        /// <summary>
        /// Checks if this instance of Product has a given Measurement.
        /// </summary>
        /// <param name="measurement">Instance of Measurement being checked.</param>
        /// <returns>true if this instance has the given Measurement; false, otherwise.</returns>
        public bool containsMeasurement(Measurement measurement){
            return this.productMeasurements.Where(pm => pm.measurement.Equals(measurement)).Any();
        }

        //*END OF CONTAINS METHODS */

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
        /// Represents the product hashcode
        /// </summary>
        /// <returns>Integer with the current product hashcode</returns>
        public override int GetHashCode() {
            return id().GetHashCode();
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
        /// Represents the textual information of the Product
        /// </summary>
        /// <returns>String with the textual representation of the product</returns>
        public override string ToString() {
            //Should ToString List the Product complementary Products?
            return String.Format("Product Information\n- Designation: {0}\n- Reference: {1}", designation, reference);
        }

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
        public ProductDTO toDTO(ProductDTOOptions dtoOptions) {
            ProductDTO dto = new ProductDTO();

            dto.id = this.Id;
            dto.designation = this.designation;
            dto.reference = this.reference;
            dto.productCategory = productCategory.toDTO();

            if (dtoOptions.requiredUnit == null) {
                dto.dimensions = new List<MeasurementDTO>(DTOUtils.parseToDTOS(productMeasurements.Select(pm => pm.measurement)));
            } else {
                dto.dimensions = new List<MeasurementDTO>();

                foreach(ProductMeasurement measurement in this.productMeasurements){
                    dto.dimensions.Add(measurement.measurement.toDTO(dtoOptions.requiredUnit));
                }
            }

            dto.productMaterials = new List<MaterialDTO>();
            foreach (ProductMaterial pm in this.productMaterials) {
                dto.productMaterials.Add(pm.material.toDTO());
            }
            //TODO: remove null check once complement database mappping is complete
            if (components != null && components.Count >= 0) {
                List<ComponentDTO> complementDTOList = new List<ComponentDTO>();

                foreach (Component complement in components) {
                    complementDTOList.Add(complement.toDTO());
                }
                dto.complements = complementDTOList;
            }

            if (this.supportsSlots) {
                SlotDimensionSetDTO slotDimensionSetDTO = new SlotDimensionSetDTO();
                slotDimensionSetDTO.maximumSlotDimensions = this.maxSlotSize.toDTO();
                slotDimensionSetDTO.minimumSlotDimensions = this.minSlotSize.toDTO();
                slotDimensionSetDTO.recommendedSlotDimensions = this.recommendedSlotSize.toDTO();
                dto.slotDimensions = slotDimensionSetDTO;
            }

            return dto;

        }

        //*BEGINNING OF CONSTRUCTOR CHECK METHODS */

        /// <summary>
        /// Checks if the product properties are valid
        /// </summary>
        /// <param name="reference">String with the product reference</param>
        /// <param name="designation">String with the product designation</param>
        /// <exception cref="System.ArgumentException">If any of the given arguments are null or empty.</exception>
        private void checkProductProperties(string reference, string designation) {
            checkProductReference(reference);
            checkProductDesignation(designation);
        }

        /// <summary>
        /// Checks if the product category is valid
        /// </summary>
        /// <param name="productCategory">Instance of ProductCategory being checked.</param>
        /// <exception cref="System.ArgumentNullException">If the given instance of ProductCategory is null.</exception>
        private void checkProductCategory(ProductCategory productCategory) {
            if (productCategory == null) throw new ArgumentNullException(INVALID_PRODUCT_CATEGORY);
        }

        /// <summary>
        /// Checks if the materials which a product can be made of are valid
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials</param>
        /// <exception cref="System.ArgumentException">If the IEnumerable is null, empty or contains duplicates.</exception>
        private void checkProductMaterials(IEnumerable<Material> productMaterials) {
            if (Collections.isEnumerableNullOrEmpty(productMaterials))
                throw new ArgumentException(INVALID_PRODUCT_MATERIALS);
            checkDuplicatedMaterials(productMaterials);
        }

        /// <summary>
        /// Checks if the IEnumerable of Measurement in which this instance of Product can be made of are valid.
        /// </summary>
        /// <param name="measurements">IEnumerable of Measurement being checked.</param>
        /// <exception cref="System.ArgumentException">If the IEnumerable is null, empty or contains duplicates.</exception>
        private void checkProductMeasurements(IEnumerable<Measurement> measurements){
            if(Collections.isEnumerableNullOrEmpty(measurements)){
                throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
            }
            checkDuplicatedMeasurements(measurements);
        }

        /// <summary>
        /// Checks if the products which a product can be complementary by are valid
        /// </summary>
        /// <param name="complementaryProducts">IEnumerable with the complementary products</param>
        /// <exception cref="System.ArgumentException">If the IEnumerable is null, empty or contains duplicates.</exception>
        private void checkComplementaryProducts(IEnumerable<Product> complementaryProducts) {
            if (Collections.isEnumerableNullOrEmpty(complementaryProducts))
                throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTARY_PRODUCTS);
            checkDuplicatedComplementaryProducts(complementaryProducts);
        }

        /// <summary>
        /// Checks if the product's slot sizes are valid (non-negative values with proper ratios between them).
        /// </summary>
        /// <param name="maxSlotSize">CustomizedDimensions with the maximum size of the slots</param>
        /// <param name="minSlotSize">CustomizedDimensions with the minimum size of the slots</param>
        /// <param name="recommendedSlotSize">CustomizedDimensions with the recommended size of the slots</param>
        /// <exception cref="System.ArgumentException">Thrown when any of the instances of CustomizedDimensions are</exception>
        private void checkProductSlotsDimensions(CustomizedDimensions maxSlotSize, CustomizedDimensions minSlotSize, CustomizedDimensions recommendedSlotSize) {
            if(maxSlotSize == null){
                throw new ArgumentNullException(MAXIMUM_SLOT_DIMENSIONS_NULL);
            }
            if(minSlotSize == null){
                throw new ArgumentNullException(MINIMUM_SLOT_DIMENSONS_NULL);
            }
            if(recommendedSlotSize == null){
                throw new ArgumentNullException(RECOMMENDED_SLOT_DIMENSIONS_NULL);
            }
            if (minSlotSize.depth > maxSlotSize.depth || minSlotSize.height > maxSlotSize.height || minSlotSize.width > maxSlotSize.width)
                throw new ArgumentException(INVALID_MIN_TO_MAX_SLOT_SIZE_RATIO);

            if (recommendedSlotSize.depth > maxSlotSize.depth || recommendedSlotSize.height > maxSlotSize.height || recommendedSlotSize.width > maxSlotSize.width)
                throw new ArgumentException(INVALID_RECOMMENDED_TO_MAX_SLOT_SIZE_RATIO);

            if (recommendedSlotSize.depth < minSlotSize.depth || recommendedSlotSize.height < minSlotSize.height || recommendedSlotSize.width < minSlotSize.width)
                throw new ArgumentException(INVALID_RECOMMENDED_TO_MIN_SLOT_SIZE_RATIO);
        }

        //*END OF CONSTRUCTOR CHECK METHODS */

        //*BEGINNING OF DUPLICATE CHECK METHODS */

        //TODO: Inform in Exception which one of the elements was duplicated

        /// <summary>
        /// Checks if an IEnumerable of Material contains duplicates.
        /// </summary>
        /// <param name="productMaterials">IEnumerable with the product materials.</param>
        /// <exception cref="System.ArgumentException">Thrown when the IEnumerable has any duplicates.</exception>
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
        /// Checks if a IEnumerable of Product contains duplicates.
        /// </summary>
        /// <param name="complementaryProducts">IEnumerable of Product being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the IEnumerable has any duplicates.</exception>
        private void checkDuplicatedComplementaryProducts(IEnumerable<Product> complementaryProducts) {
            HashSet<string> complementaryProductsRefereces = new HashSet<string>();
            IEnumerator<Product> complementaryProductsEnumerator = complementaryProducts.GetEnumerator();
            Product complementaryProduct = complementaryProductsEnumerator.Current;
            while (complementaryProductsEnumerator.MoveNext()) {
                complementaryProduct = complementaryProductsEnumerator.Current;
                if (!complementaryProductsRefereces.Add(complementaryProduct.id())) {
                    throw new ArgumentException(INVALID_PRODUCT_COMPLEMENTARY_PRODUCTS);
                }
            }
        }

        /// <summary>
        /// Checks if an IEnumerable of Measurement contains duplicates.
        /// </summary>
        /// <param name="measurements">IEnumerable of Measurement being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the IEnumerable has any duplicates.</exception>
        private void checkDuplicatedMeasurements(IEnumerable<Measurement> measurements){
            HashSet<Measurement> measurementsSet = new HashSet<Measurement>();
            foreach(Measurement measurement in measurements){
                if(!measurementsSet.Add(measurement)){
                    throw new ArgumentException(INVALID_PRODUCT_DIMENSIONS);
                }
            }
        }

        //*END OF DUPLICATE CHECK METHODS */

        //*BEGINNING OF AUXILIARY CHECK METHODS */

        /// <summary>
        /// Checks if the product reference is valid (not null nor empty).
        /// </summary>
        /// <param name="reference">String with the product reference being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given reference is null or empty.</exception>
        private void checkProductReference(string reference){
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
        }

        /// <summary>
        /// Checks if the product designation is valid (not null nor empty).
        /// </summary>
        /// <param name="designation">String with the product designation being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given designation is null or empty.</exception>
        private void checkProductDesignation(string designation){
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
        }

        /// <summary>
        /// Checks if the given product reference is valid for change (not null, empty nor equal to the current reference).
        /// </summary>
        /// <param name="designation">String with the product reference being changed.</param>
        /// <exception cref="System.ArgumentException">Throw when the given reference is null, empty or equal to the current reference.</exception>
        private void checkIfProductReferenceIsValidForChange(string reference){
            checkProductReference(reference);
            if(this.reference.Equals(reference))
                throw new ArgumentException(INVALID_PRODUCT_REFERENCE_CHANGE);
        }

        /// <summary>
        /// Checks if the given product designation is valid for change (not null, empty nor equal to the current reference).
        /// </summary>
        /// <param name="designation">String with the product designation being changed.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given designation is null, empty or equal to the current designation.</exception>
        private void checkIfProductDesignationIsValidForChange(string designation){
            checkProductDesignation(designation);
            if(this.designation.Equals(designation))
                throw new ArgumentException(INVALID_PRODUCT_DESIGNATION_CHANGE);
        }

        /// <summary>
        /// Checks if the given instance of Productcategory is valid for change (not null nor equal to the current instance of ProductCategory).
        /// </summary>
        /// <param name="category">ProductCategory with the product category being changed</param>
        /// <exception cref="System.ArgumentException">Throw when the given instance of ProductCategory is null or equal to the current instance of ProductCategory.</exception>
        private void checkIfProductCategoryIsValidForChange(ProductCategory category){
            checkProductCategory(category);
            if(this.productCategory.Equals(category))
                throw new ArgumentException(INVALID_PRODUCT_CATEGORY_CHANGE);
        }

        /// <summary>
        /// Checks if an instance of Material can be added to this instance of Product.
        /// </summary>
        /// <param name="productMaterial">Instance of Material being checked..</param>
        /// <exception cref="System.ArgumentException">Thrown when the given instance of Material is null or has been added previously.</exception>
        private void checkIfMaterialIsValidForAddition(Material productMaterial) {
            if (productMaterial == null) {
                throw new ArgumentNullException(MATERIAL_NULL);
            }
            if(containsMaterial(productMaterial)){
                throw new ArgumentException(MATERIAL_ALREADY_ADDED);
            }
        }

        /// <summary>
        /// Checks if a complementary product is valid for additon on the current product.
        /// </summary>
        /// <param name="complementaryProduct">Product with the complementary product being validated</param>
        /// <exception cref="System.ArgumentException">Thrown when the given instance of Product is null,
        ///  is equal to this instance of Product or an equal instance of Product has been added previously.</exception>
        private void checkIfComplementaryProductIsValidForAddition(Product complementaryProduct) {
            if(complementaryProduct == null){
                throw new ArgumentNullException(COMPLEMENTARY_PRODUCT_NULL);
            }
            if(complementaryProduct.Equals(this)){
                throw new ArgumentException(COMPLEMENTARY_PRODUCT_EQUALS_PRODUCT);
            }
            if(this.containsComplementaryProduct(complementaryProduct)){
                throw new ArgumentException(COMPLEMENTARY_PRODUCT_ALREADY_ADDED);
            }
        }

        /// <summary>
        /// Checks if an instance of Measurement is valid for addition on the current Product's list of Measurement.
        /// </summary>
        /// <param name="measurement">Measurement being validated.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given instance of Measurement is null or an equal instance of Measurement has been added previously.</exception>
        private void checkIfMeasurementIsValidForAddition(Measurement measurement){
            if(measurement == null){
                throw new ArgumentNullException(MEASUREMENT_NULL);
            }
            if(containsMeasurement(measurement)){
                throw new ArgumentException(MEASUREMENT_ALREADY_ADDED);
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
            /// Adds complementary products to the current product builder
            /// </summary>
            /// <param name="complementaryProducts">IEnumerable with the product complementary products</param>
            /// <returns>ProductBuilder with the updated builder</returns>
            public ProductBuilder withcomplementaryProducts(IEnumerable<ComponentDTO> complementaryProducts) {
                builderDTO.complements = new List<ComponentDTO>(complementaryProducts);
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
                IEnumerable<ComponentDTO> complementaryProducts = builderDTO.complements;
                if (complementaryProducts == null) {
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
                                    , DTOUtils.reverseDTOS(complementaryProducts)
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