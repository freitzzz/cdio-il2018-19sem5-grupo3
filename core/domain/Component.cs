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
    public class Component : DTOAble<ComponentDTO>
    {
         /// <summary>
        /// Constant that represents the messange that ocurres if the component reference is invalid
        /// </summary>
        private const string INVALID_COMPONENT_REFERENCE="The component reference is invalid";
        /// <summary>
        /// Constant that represents the messange that ocurres if the component designation is invalid
        /// </summary>
        private const string INVALID_COMPONENT_DESIGNATION="The component designation is invalid";
        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id{get; private set;}

        /// <summary>
        /// String with the component reference
        /// </summary>
        public string reference {get; set;}
        /// <summary>
        /// String with the component designation
        /// </summary>
        public string designation {get; set;}
        /// <summary>
        /// String with the component designation
        /// </summary>
        public Product product {get; set;}
        /// <summary>
        /// List with the restrictions which the current component can be have
        /// </summary>
        [NotMapped] //!remove this annotation once we figure out how to persist interfaces
        public List<Restriction> restrictions {get; set;}
        
        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Component(){}
        /// <summary>
        /// Builds a new component with its reference, designation and list of the restrictions.
        /// </summary>
        /// <param name="reference">String with the component reference</param>
        /// <param name="designation">String with the component designation</param>
        /// <param name="restricitions">List with the restrictions of the component</param>
        public Component(string reference,string designation,List<Restriction> restrictions){
            checkComponentProperties(reference,designation);
            this.reference=reference;
            this.designation=designation;
            this.restrictions=restrictions;
        }
        /// <summary>
        /// Checks if the component properties are valid
        /// </summary>
        /// <param name="reference">String with the component reference</param>
        /// <param name="designation">String with the component designation</param>
        private void checkComponentProperties(string reference,string designation){
            if(Strings.isNullOrEmpty(reference))throw new ArgumentException(INVALID_COMPONENT_REFERENCE);
            if(Strings.isNullOrEmpty(designation))throw new ArgumentException(INVALID_COMPONENT_DESIGNATION);
        }
         /// <summary>
        /// Returns the component identity
        /// </summary>
        /// <returns>string with the component identity</returns>
        public string id(){return reference;}

        /// <summary>
        /// Checks if a certain component entity is the same as the current component
        /// </summary>
        /// <param name="comparingEntity">string with the comparing component identity</param>
        /// <returns>boolean true if both entities identity are the same, false if not</returns>
        public bool sameAs(string comparingEntity){return id().Equals(comparingEntity);}
        /// <summary>
        /// Returns the current component as a DTO
        /// </summary>
        /// <returns>DTO with the current DTO representation of the component</returns>
        public ComponentDTO toDTO(){
            ComponentDTO dto = new ComponentDTO();

            dto.id = this.Id;
            dto.designation = this.designation;
            dto.reference = this.reference;

            if(this.restrictions != null){
                List<RestrictionDTO> complementDTOList = new List<RestrictionDTO>();

                foreach(Restriction restriction in restrictions){
                    complementDTOList.Add(restriction.toDTO()); 
                }
                dto.restrictions = complementDTOList;
            }

            //TODO: add missing DTO's


            return dto;
        }

        /// <summary>
        /// Checks if two components are equal
        /// </summary>
        /// <param name="comparingComponent">Component with the component being compared to the current one</param>
        /// <returns>boolean true if both components are equal, false if not</returns>
        public override bool Equals(object comparingComponent){
            if(this==comparingComponent)return true;
            return comparingComponent is Component && this.id().Equals(((Component)comparingComponent).id());
        }

        /// <summary>
        /// Represents the component hashcode
        /// </summary>
        /// <returns>Integer with the current component hashcode</returns>
        public override int GetHashCode(){
            return id().GetHashCode();
        }
        /// <summary>
        /// Represents the textual information of the Component
        /// </summary>
        /// <returns>String with the textual representation of the component</returns>
        public override string ToString(){
            //Should ToString List the Component Complemented Component?
            return String.Format("Component Information\n- Designation: {0}\n- Reference: {1}",designation,reference);
        }
        
    }
}