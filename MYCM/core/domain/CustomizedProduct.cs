using System.Collections.Generic;
using support.domain.ddd;
using System;
using support.dto;
using core.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain;

namespace core.domain
{
    /// <summary>
    /// Class that represents a Customized Product.
    /// </summary>
    public class CustomizedProduct : Activatable, AggregateRoot<string>, DTOAble<CustomizedProductDTO>
    {

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's material is not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_MATERIAL = "The chosen material is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's dimensions are not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS = "The inserted dimension is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product reference is not valid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The inserted product reference is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's designation is not valid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The inserted designation is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProducts slot is not valid
        /// </summary>
        private const string INVALID_INSERTED_IN_SLOT = "The customized products own slot is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProducts slots is null
        /// </summary>
        private const string NULL_SLOT = "The customized products slots cannot be null";

        /// <summary>
        /// Constant that represents the message that occurs if one of the CustomizedProducts slots has invalid dimensions
        /// </summary>
        private const string INVALID_SLOT_DIMENSIONS = "One of the customized products slots has invalid dimensions";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProducts product doesn't support slots
        /// </summary>
        private const string PRODUCT_DOES_NOT_SUPPORT_SLOTS = "This customized product doesn't support slots";

        /// <summary>
        /// Long that represents the CustomizedProduct's persistence ID.
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// String with the CustomizedProduct's reference
        /// </summary>
        public string reference { get; protected set; }

        /// <summary>
        /// String with the CustomizedProduct's designation
        /// </summary>
        public string designation { get; protected set; }

        /// <summary>
        /// CustomizedMaterial that represents the CustomizedProduct's material
        /// </summary>
        private CustomizedMaterial _customizedMaterial; //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedMaterial customizedMaterial { get => LazyLoader.Load(this, ref _customizedMaterial); protected set => _customizedMaterial = value; }


        /// <summary>
        /// CustomizedDimensions that represents the CustomizedProduct's dimensions
        /// </summary>
        private CustomizedDimensions _customizedDimensions; //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions customizedDimensions { get => LazyLoader.Load(this, ref _customizedDimensions); protected set => _customizedDimensions = value; }

        /// <summary>
        /// Product that represents the product that the CustomizedProduct refers to
        /// </summary>
        /// <value></value>
        private Product _product;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Product product { get => LazyLoader.Load(this, ref _product); protected set => _product = value; }

        /// <summary>
        /// List of Slots that the CustomizedProduct has
        /// </summary>
        private List<Slot> _slots;  //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Slot> slots { get => LazyLoader.Load(this, ref _slots); protected set => _slots = value; }

        /// <summary>
        /// Identifier of the slot that this customized product belongs to
        /// </summary>
        /// <value>Get/Internal set of the identifier</value>
        public long? insertedInSlotId { get; internal set; }

        /// <summary>
        /// Slot that this customized product belongs to
        /// </summary>
        private Slot _insertedInSlot; //!private field used for lazy loading, do not use this for storing or fetching data 

        public Slot insertedInSlot { get => LazyLoader.Load(this, ref _insertedInSlot); protected set => _insertedInSlot = value; }

        /// <summary>
        /// LazyLoader being injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the value of the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private CustomizedProduct(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected CustomizedProduct() { }

        /// <summary>
        /// Builds a new instance of CustomizedProduct, receiving its reference,
        /// designation, dimensions, material and the Product it refers to
        /// <param name = "reference">String with the new CustomizedProduct's reference</param>
        /// <param name = "designation">String with the new CustomizedProduct's designation</param>
        /// <param name = "customizedDimensions">String with the new CustomizedProduct's CustomizedDimensions</param>
        /// <param name = "customizedMaterial">String with the new CustomizedProduct's CustomizedMaterial</param>
        /// <param name = "product">String with the new CustomizedProduct's Product</param>DDD
        /// </summary>
        public CustomizedProduct(string reference, string designation, CustomizedMaterial customizedMaterial,
        CustomizedDimensions customizedDimensions, Product product)
        {
            checkProduct(product);
            checkString(reference, INVALID_PRODUCT_REFERENCE);
            checkString(designation, INVALID_PRODUCT_DESIGNATION);
            checkCustomizedMaterial(customizedMaterial);
            checkCustomizedDimensions(customizedDimensions, product);

            this.reference = reference;
            this.designation = designation;
            this.customizedDimensions = customizedDimensions;
            this.customizedMaterial = customizedMaterial;
            this.product = product;
            this.slots = new List<Slot>();
        }

        /// <summary>
        /// Builds a new instance of CustomizedProduct, receiving its reference,
        /// designation, dimensions, material and the Product it refers to
        /// <param name = "reference">String with the new CustomizedProduct's reference</param>
        /// <param name = "designation">String with the new CustomizedProduct's designation</param>
        /// <param name = "customizedDimensions">String with the new CustomizedProduct's CustomizedDimensions</param>
        /// <param name = "customizedMaterial">String with the new CustomizedProduct's CustomizedMaterial</param>
        /// <param name = "product">CustomizedProduct's Product</param>DDD
        /// <param name="slots">List containing instances of Slot.</param>
        /// </summary>
        public CustomizedProduct(string reference, string designation, CustomizedMaterial customizedMaterial,
        CustomizedDimensions customizedDimensions, Product product, List<Slot> slots) :
            this(reference, designation, customizedMaterial, customizedDimensions, product)
        {
            checkAndAddSlots(slots);
        }

        /// <summary>
        /// Builds a new instance of CustomizedProduct, receiving it's reference,
        /// designation, dimensions, material, the product it refers to, it's slots
        /// and the slot that the customized product belongs to
        /// </summary>
        /// <param name="reference">String with the new CustomizedProduct's reference</param>
        /// <param name="designation">String with the new CustomizedProduct's designation</param>
        /// <param name="customizedMaterial">String with the new CustomizedProduct's CustomizedMaterial</param>
        /// <param name="customizedDimensions">String with the new CustomizedProduct's CustomizedDimensions</param>
        /// <param name="product">CustomizedProduct's Product</param>
        /// <param name="slots">CustomizedProduct's Slots</param>
        /// <param name="insertedInSlot">Slot that the customied product belongs to</param>
        public CustomizedProduct(string reference, string designation, CustomizedMaterial customizedMaterial,
        CustomizedDimensions customizedDimensions, Product product, List<Slot> slots, Slot insertedInSlot) :
            this(reference, designation, customizedMaterial, customizedDimensions, product, slots)
        {
            checkInsertedInSlot(insertedInSlot);
            insertedInSlot.addCustomizedProduct(this);
        }

        /// <summary>
        /// Builds a new instance of CustomizedProduct, receiving it's reference,
        /// designation, dimensions, material, the product it refers to, it's slots
        /// and the slot that the customized product belongs to
        /// </summary>
        /// <param name="reference">String with the new CustomizedProduct's reference</param>
        /// <param name="designation">String with the new CustomizedProduct's designation</param>
        /// <param name="customizedMaterial">String with the new CustomizedProduct's CustomizedMaterial</param>
        /// <param name="customizedDimensions">String with the new CustomizedProduct's CustomizedDimensions</param>
        /// <param name="product">CustomizedProduct's Product</param>
        /// <param name="insertedInSlot">Slot that the customied product belongs to</param>
        public CustomizedProduct(string reference, string designation, CustomizedMaterial customizedMaterial,
        CustomizedDimensions customizedDimensions, Product product, Slot insertedInSlot) :
            this(reference, designation, customizedMaterial, customizedDimensions, product)
        {
            checkInsertedInSlot(insertedInSlot);
            insertedInSlot.addCustomizedProduct(this);
        }
        /// <summary>
        /// Returns the CustomizedProduct's identity
        /// </summary>
        /// <returns>String with the CustomizedProduct's identity</returns>
        public string id()
        {
            return reference;
        }

        /// <summary>
        /// Changes the CustomizedProduct's reference
        /// </summary>
        /// <param name="reference">New reference</param>
        /// <returns>true if the reference was changed succesfully, false if otherwise</returns>
        public bool changeReference(string reference)
        {
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
            this.reference = reference;
            return true;
        }

        /// <summary>
        /// Changes the CustomizedProduct's designation
        /// </summary>
        /// <param name="designation">New designation</param>
        /// <returns>true if the designation was changed successfully, false if otherwise</returns>
        public bool changeDesignation(string designation)
        {
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
            this.designation = designation;
            return true;
        }

        /// <summary>
        /// Changes the CustomizedProduct's customized dimensions
        /// </summary>
        /// <param name="customizedDimensions">New customized dimensions</param>
        /// <returns>true if the customized dimensions were changed successfully</returns>
        public bool changeCustomizedDimensions(CustomizedDimensions customizedDimensions)
        {
            checkCustomizedDimensions(customizedDimensions, product);
            this.customizedDimensions = customizedDimensions;
            return true;
        }

        /// <summary>
        /// Changes the CustomizedProduct's customized material
        /// </summary>
        /// <param name="customizedMaterial">New customized material</param>
        /// <returns>true if the customized material was changed successfully</returns>
        public bool changeCustomizedMaterial(CustomizedMaterial customizedMaterial)
        {
            checkCustomizedMaterial(customizedMaterial);
            this.customizedMaterial = customizedMaterial;
            return true;
        }

        /// <summary>
        /// Changes the Finish of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="finish">new finish</param>
        /// <returns>true if the finish was changed succesfully</returns>
        public bool changeFinish(Finish finish)
        {
            return this.customizedMaterial.changeFinish(finish);
        }

        /// <summary>
        /// Changes the Color of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="color">new color</param>
        /// <returns>true if the color successfully</returns>
        public bool changeColor(Color color)
        {
            return this.customizedMaterial.changeColor(color);
        }

        /// <summary>
        /// Adds a given Slot from the CustomizedProduct's Slot list
        /// </summary>
        /// <param name="slot">Slot to add</param>
        /// <returns>true if the Slot is added, false if not</returns>
        public bool addSlot(Slot slot)
        {
            if (slot == null) throw new ArgumentException(NULL_SLOT);
            if (!product.supportsSlots) throw new ArgumentException(PRODUCT_DOES_NOT_SUPPORT_SLOTS);
            if (slot.slotDimensions.width >= product.minSlotSize.width
            && slot.slotDimensions.depth >= product.minSlotSize.depth
            && slot.slotDimensions.height >= product.minSlotSize.height
            && slot.slotDimensions.width <= product.maxSlotSize.width
            && slot.slotDimensions.depth <= product.maxSlotSize.depth
            && slot.slotDimensions.height <= product.maxSlotSize.height)
            {
                slots.Add(slot);
                return true;
            }
            throw new ArgumentException(INVALID_SLOT_DIMENSIONS);
        }

        /// <summary>
        /// Removes a given Slot from the CustomizedProduct's Slot list
        /// </summary>
        /// <param name="slot">Slot to remove</param>
        /// <returns>true if the Slot is removed, false if not</returns>
        public bool removeSlot(Slot slot)
        {
            if (slot == null) return false;
            return product.supportsSlots && slots.Remove(slot);
        }

        /// <summary>
        /// Returns the number of Slots in the CustomizedProduct's Slot list
        /// </summary>
        /// <returns>Number of Slots within the CustomizedProduct</returns>
        public int numberOfSlots()
        {
            return slots.Count;
        }

        /// <summary>
        /// Checks if all Slots from a received List are valid and adds them to the CustomizedProduct's list of Slots
        /// </summary>
        /// <param name="slots"></param>
        private void checkAndAddSlots(List<Slot> slots)
        {
            foreach (Slot slot in slots)
            {
                addSlot(slot);
            }
        }

        /// <summary>
        /// Checks if the slot in which the customized product is inserted in is valid 
        /// </summary>
        /// <param name="insertedInSlot">customized products own slot</param>
        private void checkInsertedInSlot(Slot insertedInSlot)
        {
            if (insertedInSlot == null) throw new ArgumentException(INVALID_INSERTED_IN_SLOT);
            this.insertedInSlot = insertedInSlot;
        }

        /// <summary>
        /// Checks if the Product is valid (not null)
        /// </summary>
        /// <param name="product">Product to check</param>
        private void checkProduct(Product product)
        {
            if (product == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_MATERIAL);
        }

        /// <summary>
        /// Checks if the CustomizedMaterial is valid
        /// </summary>
        /// <param name="customizedMaterial">CustomizedMaterial to check</param>
        private void checkCustomizedMaterial(CustomizedMaterial customizedMaterial)
        {
            if (customizedMaterial == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_MATERIAL);
            if (!product.containsMaterial(customizedMaterial.material)) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_MATERIAL);
        }

        /// <summary>
        /// Checks if the CustomizedDimensions are valid
        /// </summary>
        /// <param name="customizedDimensions">CustomizedDimensions to check</param>
        private void checkCustomizedDimensions(CustomizedDimensions customizedDimensions, Product product)
        {
            if (customizedDimensions == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS);

            List<Measurement> possibleMeasurements = product.measurements.Select(m => m.measurement).ToList();

            foreach (Measurement measurement in possibleMeasurements)
            {
                //TODO Implement method
            }
        }

        /// <summary>
        /// Checks if a given string is valid
        /// </summary>
        /// <param name="obj">String to check</param>
        private void checkString(string obj, string message)
        {
            if (String.IsNullOrEmpty(obj)) throw new ArgumentException(message);
        }

        /// <summary>
        /// Checks if the CustomizedProduct's identity is the same as the one received as a parameter
        /// </summary>
        /// <param name="comparingEntity">Entity to compare to the CustomizedProduct</param>
        /// <returns></returns>
        public bool sameAs(string comparingEntity)
        {
            return reference.Equals(comparingEntity, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a textual description of the CustomizedProduct
        /// </summary>
        /// <returns>String that describes the CustomizedProduct</returns>
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /// <summary>
        /// Returns the generated hash code of the CustomizedProduct
        /// </summary>
        /// <returns>Generated hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = 17;
            return (hashCode * 23) + this.reference.GetHashCode();
        }

        /// <summary>
        /// Checks if a certain CustomizedProduct is the same as a received object
        /// </summary>
        /// <param name="obj">Object to compare with the CustomizedProduct</param>
        /// <returns>true if both objects are equal, false if not</returns>
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CustomizedProduct customizedProduct = (CustomizedProduct)obj;
                return reference.Equals(customizedProduct.reference) &&
                designation.Equals(customizedProduct.designation) &&
                customizedDimensions.Equals(customizedProduct.customizedDimensions) &&
                customizedMaterial.Equals(customizedProduct.customizedMaterial) &&
                product.Equals(customizedProduct.product);
            }
        }

        /// <summary>
        /// Returns the current CustomizedProduct as a DTO
        /// </summary>
        /// <returns>CustomizedProductDTO with the current representation of the CustomizedProduct</returns>
        public CustomizedProductDTO toDTO()
        {
            CustomizedProductDTO dto = new CustomizedProductDTO();
            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.productDTO = this.product.toDTO();
            dto.customizedDimensionsDTO = this.customizedDimensions.toDTO();
            dto.customizedMaterialDTO = this.customizedMaterial.toDTO();
            dto.slotListDTO = DTOUtils.parseToDTOS(this.slots).ToList();
            dto.id = this.Id;
            return dto;
        }
    }
}