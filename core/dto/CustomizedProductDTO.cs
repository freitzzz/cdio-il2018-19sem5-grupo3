using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;

namespace core.dto
{

    /// <summary>
    /// DTO that represents a Configured Product instance
    /// </summary>
    [DataContract]
    public class CustomizedProductDTO : DTO, DTOParseable<CustomizedProduct, CustomizedProductDTO>
    {
        // <summary>
        /// Customized Products's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }


        /**
        <summary>
            String with the Customized Products's reference.
        </summary>
        */
        public string reference { get; set; }

        /** 
        <summary>
            String with the Customized Products's designation.
        </summary>
        */
        public string designation { get; set; }


        /**
        <summary>
            The CustomizedProduct Customized Material
        </summary>
         */
        public  CustomizedMaterial customizedMaterial;

        /**
        <summary>
            The CustomizedProduct Customized Dimensions
        </summary>
         */
        public CustomizedDimensions customizedDimensions;

        /**
        <summary>
            Product from CustomizedProduct
        </summary>
         */
        public ProductDTO productDTO;

        public  CustomizedProduct toEntity()
        {
        
            CustomizedProduct instanceFromDTO = new CustomizedProduct(reference, designation, customizedMaterial, customizedDimensions, productDTO.toEntity());
            instanceFromDTO.Id = this.id;
            return instanceFromDTO;
            
        }

    }
}