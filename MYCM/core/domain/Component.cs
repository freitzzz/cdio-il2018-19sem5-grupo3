using support.builders;
using support.domain;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using core.dto;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    public class Component : Restrictable, DTOAble<ComponentDTO>
    {

        /// <summary>
        /// Constant that represents the message that ocurrs if the Component's product is not valid.
        /// </summary>
        private const string INVALID_COMPONENT_PRODUCT = "The Component's product is not valid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if the Component's restrictions is not valid.
        ///</summary>
        private const string INVALID_COMPONENT_RESTRICTIONS = "The Component's restrictions is not valid!";

        /// <summary>
        /// Long with the product which has the complemented product ID
        /// </summary>
        public long fatherProductId { get; protected set; }
        /// <summary>
        /// Boolean value that dictates whether the component is mandatory or not
        /// </summary>
        public bool mandatory { get; protected set; }

        /// <summary>
        /// Product with the product which has the current complemented product
        /// </summary>
        private Product _fatherProduct;             //!private field used for lazy loading, do not use this for storing or fetching data
        public Product fatherProduct { get => LazyLoader.Load(this, ref _fatherProduct); protected set => _fatherProduct = value; }

        /// <summary>
        /// Long with the product which has the complemented product ID
        /// </summary>
        public long complementaryProductId { get; protected set; }

        /// <summary>
        /// Product with the complemented product 
        /// </summary>
        private Product _complementaryProduct;       //!private field used for lazy loading, do not use this for storing or fetching data
        public Product complementaryProduct { get => LazyLoader.Load(this, ref _complementaryProduct); protected set => _complementaryProduct = value; }

        /// <summary>
        /// Private constructor used for injecting a LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Component(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Component() { }

        /// <summary>
        /// Builds a new Component with the father and complemented product
        /// </summary>
        /// <param name="fatherProduct">Product with the father product</param>
        /// <param name="complementedProduct">Product with the complemented product</param>
        public Component(Product fatherProduct, Product complementedProduct)
        {
            checkComponentProduct(complementedProduct);
            checkComponentProduct(fatherProduct);
            this.fatherProduct = fatherProduct;
            this.complementaryProduct = complementedProduct;
            this.restrictions = new List<Restriction>();
            this.mandatory = false;
        }

        /// <summary>
        /// Builds a new component with its product and list of the restrictions.
        /// </summary>
        /// <param name="restricitions">List with the restrictions of the component</param>
        public Component(Product parentProduct, Product childProduct,
        List<Restriction> restrictions)
        : this(parentProduct, childProduct)
        {
            checkRestrictions(restrictions);
            this.restrictions = restrictions;
        }

        /// <summary>
        /// Builds a new component with its product ,list of the restrictions and the obligatoriness.
        /// </summary>
        /// <param name="restricitions">List with the restrictions of the component</param>
        public Component(Product parentProduct, Product childProduct,
        List<Restriction> restrictions, bool mandatory) : this(parentProduct, childProduct)
        {
            checkRestrictions(restrictions);
            this.restrictions = restrictions;
            this.mandatory = mandatory;
        }

        /// <summary>
        /// Builds a new Component with the father ,complemented product and the obligatoriness
        /// </summary>
        /// <param name="fatherProduct">Product with the father product</param>
        /// <param name="complementedProduct">Product with the complemented product</param>
        public Component(Product fatherProduct, Product complementedProduct, bool mandatory)
        : this(fatherProduct, complementedProduct)
        {
            this.mandatory = mandatory;
        }

        /// <summary>
        /// Checks if the Component's properties are valid.
        /// </summary>
        /// <param name="restrictions">List of the restrictions of the Component.</param>
        private void checkRestrictions(List<Restriction> restrictions)
        {
            if (Collections.isListNull(restrictions) || Collections.isListEmpty(restrictions))
                throw new ArgumentException(INVALID_COMPONENT_RESTRICTIONS);
        }

        /// <summary>
        /// Checks if the Component's product are valid.
        /// </summary>
        /// <param name="product">Product with the Material's product</param>
        private void checkComponentProduct(Product product)
        {
            if (product == null) throw new ArgumentException(INVALID_COMPONENT_PRODUCT);
        }
        
        /// <summary>
        /// Returns the current component as a DTO
        /// </summary>
        /// <returns>DTO with the current DTO representation of the component</returns>
        public ComponentDTO toDTO()
        {
            ComponentDTO dto = new ComponentDTO();
            dto.product = complementaryProduct.toDTO();
            dto.mandatory = mandatory;

            if (this.restrictions != null)
            {
                List<RestrictionDTO> complementDTOList = new List<RestrictionDTO>();

                foreach (Restriction restriction in restrictions)
                {
                    complementDTOList.Add(restriction.toDTO());
                }
                dto.restrictions = complementDTOList;
            }
            return dto;
        }
    }
}