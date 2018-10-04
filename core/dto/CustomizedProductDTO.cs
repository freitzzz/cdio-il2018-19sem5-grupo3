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
        /// Finish's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }


        /**
        <summary>
            String with the ConfiguredProduct's reference.
        </summary>
        */
        public string reference { get; set; }

        /** 
        <summary>
            String with the ConfiguredProduct's designation.
        </summary>
        */
        public string designation { get; set; }


        /**
        <summary>
            The CustomizedProduct Customized Material
        </summary>
         */
        private readonly CustomizedMaterial customizedMaterial;

        /**
        <summary>
            The CustomizedProduct Customized Dimensions
        </summary>
         */
        private readonly CustomizedDimensions customizedDimensions;

        /**
        <summary>
            List of Products from CustomizedProduct
        </summary>
         */
        private List<Product> list;

        public  CustomizedProduct toEntity()
        {
        
            CustomizedProduct instanceFromDTO = CustomizedProduct.valueOf(reference, designation, customizedMaterial, customizedDimensions, list);
            instanceFromDTO.Id = this.id;
            return instanceFromDTO;
            
        }

    }
}