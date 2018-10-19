using System;
using System.Collections.Generic;
using System.Linq;
using support.dto;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Represents a product slot
    /// </summary>
    public class Slot : DTOAble<SlotDTO>
    {

        /// <summary>
        /// Constant that represents the message that occurs if the slot dimensions are null
        /// </summary>
        private const string NULL_DIMENSIONS = "Slot Dimensions can't be null";

        /// <summary>
        /// Database Identifier
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// DoubleValue with the width of the slot
        /// </summary>
        private CustomizedDimensions _slotDimensions;
        public CustomizedDimensions slotDimensions {get => LazyLoader.Load(this, ref _slotDimensions); protected set => _slotDimensions = value;}

        /// <summary>
        /// Customized Products that are inside a slot
        /// </summary>
        private List<CustomizedProduct> _customizedProducts;
        public List<CustomizedProduct> customizedProducts {get => LazyLoader.Load(this, ref _customizedProducts); protected set => _customizedProducts = value;}

        /// <summary>
        /// Injected LazyLoader.
        /// </summary>
        /// <value>Gets/sets the value of the LazyLoader.</value>
        private ILazyLoader LazyLoader{get; set;}

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Slot(ILazyLoader lazyLoader){
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
        /// <param name="slotDimensions">Slots customized dimensions</param>
        public Slot(CustomizedDimensions slotDimensions)
        {
            checkSlotDimensions(slotDimensions);
            this.slotDimensions = slotDimensions;
            //TODO check if this list needs to be passed as a parameter or not
            customizedProducts = new List<CustomizedProduct>();
        }

        /// <summary>
        /// Checks if the DoubleValues used to instantiate a Slot are valid
        /// </summary>
        /// <param name="slotDimensions">Slots customized dimensions</param>
        private void checkSlotDimensions(CustomizedDimensions slotDimensions)
        {
            if (slotDimensions == null)
            {
                throw new ArgumentException(NULL_DIMENSIONS);
            }
        }

        /// <summary>
        /// Adds a customized product to the slot
        /// </summary>
        /// <param name="productToAdd">customized product to be added</param>
        /// <returns>true if the customized product is added, false if otherwise</returns>
        public bool addCustomizedProduct(CustomizedProduct productToAdd)
        {
            //TODO take restrictions into account
            if (productToAdd == null)
            {
                return false;
            }
            int previousCount = customizedProducts.Count;
            customizedProducts.Add(productToAdd);
            return customizedProducts.Count == previousCount + 1;
        }

        /// <summary>
        /// Removes a customized product from the slot
        /// </summary>
        /// <param name="productToRemove">customized product to be removed</param>
        /// <returns>true if the customized product is removed, false if otherwise</returns>
        public bool removeCustomizedProduct(CustomizedProduct productToRemove) => productToRemove == null ? false : customizedProducts.Remove(productToRemove);

        /// <summary>
        /// Equals of Slot
        /// </summary>
        /// <param name="obj">object to be compared</param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            Slot other = (Slot)obj;

            return this.slotDimensions.Equals(other.slotDimensions) && this.customizedProducts.SequenceEqual(other.customizedProducts);
        }

        /// <summary>
        /// Hash Code of Slot
        /// </summary>
        /// <returns>hash code of a slot instance</returns>
        public override int GetHashCode()
        {
            int hashCode = slotDimensions.GetHashCode();
            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                hashCode *= customizedProduct.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// ToString of Slot
        /// </summary>
        /// <returns>string description of a slot instance</returns>
        public override string ToString()
        {
            return String.Format("Slot Dimensions: {0}\nSlot Products:{1}", slotDimensions.ToString(), customizedProducts.ToString());
        }

        public SlotDTO toDTO()
        {
            SlotDTO slotDTO = new SlotDTO();
            slotDTO.Id = Id;
            slotDTO.customizedDimensions = slotDimensions.toDTO();
            slotDTO.customizedProducts = new List<CustomizedProductDTO>();
            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                slotDTO.customizedProducts.Add(customizedProduct.toDTO());
            }
            return slotDTO;
        }
    }
}