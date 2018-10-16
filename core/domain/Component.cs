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

namespace core.domain
{
    public class Component : AggregateRoot<Product>, DTOAble<ComponentDTO>
    {
        /**
           <summary>
               Constant that represents the message that ocurrs if the Component's product is not valid.
           </summary>
           */
        private const string INVALID_COMPONENT_PRODUCT = "The Component's product is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Component's restrictions is not valid.
        </summary>
        */
        private const string INVALID_COMPONENT_RESTRICTIONS = "The Component's restrictions is not valid!";

        /// <summary>
        /// Long with the persistence ID
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Product with the product 
        /// </summary>
        public virtual Product product { get; set; }
        /// <summary>
        /// List with the restrictions which the current component can be have
        /// </summary>
        [NotMapped] //!remove this annotation once we figure out how to persist interfaces
        public virtual List<Restriction> restrictions { get; set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Component() { }
        /// <summary>
        /// Builds a new component with its product and list of the restrictions.
        /// </summary>
        /// <param name="restricitions">List with the restrictions of the component</param>
        public Component(Product product, List<Restriction> restrictions)
        {
            checkComponentProperties(product, restrictions);
            this.product = product;
            this.restrictions = restrictions;

        }
        /// <summary>
        /// Builds a new component with its product and list of the restrictions.
        /// </summary>
        /// <param name="restricitions">List with the restrictions of the component</param>
        public Component(Product product)
        {
            checkComponentProduct(product);
            this.product = product;
        }
        /**
       <summary>
           Checks if the Component's properties are valid.
       </summary>
       <param name = "product">Product with the Material's product</param>
       <param name = "restrictions">List of the restrictions of the Component.</param>
       */
        private void checkComponentProperties(Product product, List<Restriction> restrictions)
        {
            checkComponentProduct(product);
            if (Collections.isListNull(restrictions) || Collections.isListEmpty(restrictions)) throw new ArgumentException(INVALID_COMPONENT_RESTRICTIONS);
        }
        /**
       <summary>
           Checks if the Component's product are valid.
       </summary>
       <param name = "product">Product with the Material's product</param>
       */
        private void checkComponentProduct(Product product)
        {
            if (product == null) throw new ArgumentException(INVALID_COMPONENT_PRODUCT);
        }
        /// <summary>
        /// Returns the component identity
        /// </summary>
        /// <returns>Product with the component identity</returns>
        public Product id() { return product; }

        /// <summary>
        /// Checks if a certain component entity is the same as the current component
        /// </summary>
        /// <param name="comparingEntity">Product with the comparing component identity</param>
        /// <returns>boolean true if both entities identity are the same, false if not</returns>
        public bool sameAs(Product comparingEntity) { return id().Equals(comparingEntity); }
        /// <summary>
        /// Returns the current component as a DTO
        /// </summary>
        /// <returns>DTO with the current DTO representation of the component</returns>
        public ComponentDTO toDTO()
        {
            ComponentDTO dto = new ComponentDTO();
            dto.product=product.toDTO();

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

        /// <summary>
        /// Checks if two components are equal
        /// </summary>
        /// <param name="comparingComponent">Component with the component being compared to the current one</param>
        /// <returns>boolean true if both components are equal, false if not</returns>
        public override bool Equals(object comparingComponent)
        {
            if (this == comparingComponent) return true;
            return comparingComponent is Component && this.id().Equals(((Component)comparingComponent).id());
        }

        /// <summary>
        /// Represents the component hashcode
        /// </summary>
        /// <returns>Integer with the current component hashcode</returns>
        public override int GetHashCode()
        {
            return id().GetHashCode();
        }
        /// <summary>
        /// Represents the textual information of the Component
        /// </summary>
        /// <returns>String with the textual representation of the component</returns>
        public override string ToString()
        {
            //Should ToString List the Component Complemented Component?
            return String.Format("Component Information\n- List of restrictions: {0}\n", restrictions);
        }

    }
}