using System;
using System.Collections.Generic;
using System.Linq;
using support.dto;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain {
    /// <summary>
    /// Represents a product slot
    /// </summary>
    public class Slot : DomainEntity<string>, DTOAble<SlotDTO> {
        /// <summary>
        /// Constant that represents the message presented when the provided identifier is invalid.
        /// </summary>
        private const string ERROR_INVALID_IDENTIFIER = "The provided identifier is invalid.";

        /// <summary>
        /// Constant that represents the message that occurs if the slot dimensions are null
        /// </summary>
        private const string ERROR_NULL_DIMENSIONS = "Slot Dimensions can't be null";

        /// <summary>
        /// Constant that represents the message presented when the Slot is attempted to be resized while containing CustomizedProducts.
        /// </summary>
        private const string ERROR_CHANGE_DIMENSIONS_AFTER_ADDING_CUSTOMIZED_PRODUCTS = "Unable to resize the slot if it already has products.";

        /// <summary>
        /// Constant that represents the message presented when a null CustomizedProduct is attempted to be added.
        /// </summary>
        private const string ERROR_ADD_NULL_CUSTOMIZED_PRODUCT = "Unable to add invalid product.";

        /// <summary>
        /// Constant that represents the message presented when a duplicate instance of CustomizedProduct is attempted to be added.
        /// </summary>
        private const string ERROR_ADD_DUPLICATE_CUSTOMIZED_PRODUCT = "Unable to add a duplicate product.";

        /// <summary>
        /// Constant that represents the message presented when an instance of CustomizedProduct being added does not fit.
        /// </summary>
        private const string ERROR_ADD_CUSTOMIZED_PRODUCT_DOES_NOT_FIT = "The product does not fit.";

        /// <summary>
        /// Constant that represents the message presented when an instance of CustomizedProduct could not be removed.
        /// </summary>
        private const string ERROR_REMOVE_CUSTOMIZED_PRODUCT = "Unable to remove product.";

        /// <summary>
        /// Database Identifier
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// Slot's identifier.
        /// </summary>
        /// <value>Gets/protected sets the slot's identifier.</value>
        public string identifier { get; protected set; }

        /// <summary>
        /// DoubleValue with the width of the slot
        /// </summary>
        private CustomizedDimensions _slotDimensions;   //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions slotDimensions { get => LazyLoader.Load(this, ref _slotDimensions); protected set => _slotDimensions = value; }

        /// <summary>
        /// Customized Products that are inside a slot
        /// </summary>
        private List<CustomizedProduct> _customizedProducts;    //!private field used for lazy loading, do not use this for storing or fetching data
        public List<CustomizedProduct> customizedProducts { get => LazyLoader.Load(this, ref _customizedProducts); protected set => _customizedProducts = value; }

        /// <summary>
        /// Injected LazyLoader.
        /// </summary>
        /// <value>Gets/sets the value of the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Slot(ILazyLoader lazyLoader) {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected Slot() { }

        /// <summary>
        /// Builds a Slot instance with three DoubleValues that represent the slots
        /// width, height and depth
        /// </summary>
        /// <param name="identifier">Slot's identifier.</param>
        /// <param name="slotDimensions">Slot's customized dimensions.</param>
        public Slot(string identifier, CustomizedDimensions slotDimensions) {
            checkIdentifier(identifier);
            checkSlotDimensions(slotDimensions);
            this.identifier = identifier;
            this.slotDimensions = slotDimensions;
            customizedProducts = new List<CustomizedProduct>();
        }

        /// <summary>
        /// Checks if the given identifier is valid.
        /// </summary>
        /// <param name="identifier">Identifier being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided identifier is invalid(null or empty).</exception>
        private void checkIdentifier(string identifier) {
            if (Strings.isNullOrEmpty(identifier)) {
                throw new ArgumentException(ERROR_INVALID_IDENTIFIER);
            }
        }

        /// <summary>
        /// Checks if the given instance of CustomizedDimensions is valid.
        /// </summary>
        /// <param name="slotDimensions">Instance of CustomizedDimensions representing the Slot's dimensions.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of CustomizedDimensions is null.</exception>
        private void checkSlotDimensions(CustomizedDimensions slotDimensions) {
            if (slotDimensions == null) {
                throw new ArgumentException(ERROR_NULL_DIMENSIONS);
            }
        }


        /// <summary>
        /// Changes the Slot's identifier.
        /// </summary>
        /// <param name="identifier">Slot's new identifier.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided identifier is invalid(null or empty).</exception>
        public void changeIdentifier(string identifier) {
            checkIdentifier(identifier);
            this.identifier = identifier;
        }

        /// <summary>
        /// Changes the Slot's dimensions.
        /// </summary>
        /// <param name="slotDimensions">Instance of CustomizedDimensions.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the Slot already has instances of CustomizedProduct.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the instance of CustomizedDimensions is null.</exception>
        public void changeDimensions(CustomizedDimensions slotDimensions) {
            if (hasCustomizedProducts()) {
                //can't resize slot if it already holds products
                throw new InvalidOperationException(ERROR_CHANGE_DIMENSIONS_AFTER_ADDING_CUSTOMIZED_PRODUCTS);
            }
            checkSlotDimensions(slotDimensions);
            this.slotDimensions = slotDimensions;
        }

        /// <summary>
        /// Adds a customized product to the slot
        /// </summary>
        /// <param name="productToAdd">customized product to be added</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided instance of CustomizedProduct is null, a duplicate or does not fit in the Slot.
        /// </exception>
        public void addCustomizedProduct(CustomizedProduct productToAdd) {
            //TODO take restrictions into account
            if (productToAdd == null) {
                throw new ArgumentException(ERROR_ADD_NULL_CUSTOMIZED_PRODUCT);
            }
            if (this.customizedProducts.Contains(productToAdd)) {
                throw new ArgumentException(ERROR_ADD_DUPLICATE_CUSTOMIZED_PRODUCT);
            }
            if (!customizedDimensionsFit(productToAdd.customizedDimensions)) {
                throw new ArgumentException(ERROR_ADD_CUSTOMIZED_PRODUCT_DOES_NOT_FIT);
            }

            customizedProducts.Add(productToAdd);
        }

        /// <summary>
        /// Removes a customized product from the slot
        /// </summary>
        /// <param name="productToRemove">customized product to be removed</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance can not be removed.</exception>
        public void removeCustomizedProduct(CustomizedProduct productToRemove) {
            if (!customizedProducts.Remove(productToRemove)) {
                throw new ArgumentException(ERROR_REMOVE_CUSTOMIZED_PRODUCT);
            }
        }

        /// <summary>
        /// Checks if the Slot holds any instance of CustomizedProduct.
        /// </summary>
        /// <returns>true if the Slot has any CustomizedProduct; false, otherwise.</returns>
        public bool hasCustomizedProducts() {
            return this.customizedProducts.Any();
        }

        /// <summary>
        /// Checks if the Slot holds a given instance of CustomizedProduct.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct being checked.</param>
        /// <returns>true if the Slot contains the CustomizedProduct; false, otherwise.</returns>
        public bool hasCustomizedProduct(CustomizedProduct customizedProduct) {
            return this.customizedProducts.Contains(customizedProduct);
        }

        /// <summary>
        /// Checks if customized dimensions fit into the slot
        /// </summary>
        /// <param name="componentDimensions">customized dimensions to check</param>
        /// <returns>true if dimensions fit, false if not</returns>
        public bool customizedDimensionsFit(CustomizedDimensions componentDimensions) {
            if (componentDimensions.height > slotDimensions.height || componentDimensions.width > slotDimensions.width || componentDimensions.depth > slotDimensions.depth) {
                return false;
            }
            double remainingVolume = slotDimensions.height * slotDimensions.width * slotDimensions.depth;
            foreach (CustomizedProduct custom in customizedProducts) {
                CustomizedDimensions customDimensions = custom.customizedDimensions;
                remainingVolume -= (customDimensions.height * customDimensions.width * customDimensions.depth);
            }
            double componentVolume = componentDimensions.height * componentDimensions.width * componentDimensions.depth;
            if (componentVolume <= remainingVolume) {
                return true;
            }
            return false;
        }

        public string id() {
            return this.identifier;
        }

        public bool sameAs(string comparingEntity) {
            return this.identifier.Equals(comparingEntity);
        }

        /// <summary>
        /// Equals of Slot
        /// </summary>
        /// <param name="obj">object to be compared</param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj) {
            if (this == obj) {
                return true;
            }

            if (obj == null || !obj.GetType().Equals(this.GetType())) {
                return false;
            }

            Slot other = (Slot)obj;

            return this.identifier.Equals(other.identifier);
        }

        /// <summary>
        /// Hash Code of Slot
        /// </summary>
        /// <returns>hash code of a slot instance</returns>
        public override int GetHashCode() {
            int hash = 21;
            hash = hash * 97 + identifier.GetHashCode();
            return hash;
        }

        /// <summary>
        /// ToString of Slot
        /// </summary>
        /// <returns>string description of a slot instance</returns>
        public override string ToString() {
            return String.Format("Identifier: {0}\nSlot Dimensions: {1}\nSlot Products:{2}", identifier, slotDimensions.ToString(), customizedProducts.ToString());
        }

        public SlotDTO toDTO() {
            SlotDTO slotDTO = new SlotDTO();
            slotDTO.Id = Id;
            slotDTO.customizedDimensions = slotDimensions.toDTO();
            slotDTO.customizedProducts = new List<CustomizedProductDTO>();
            foreach (CustomizedProduct customizedProduct in customizedProducts) {
                slotDTO.customizedProducts.Add(customizedProduct.toDTO());
            }
            return slotDTO;
        }
    }
}