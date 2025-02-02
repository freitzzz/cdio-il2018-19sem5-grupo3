using core.domain;
using support.dto;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a instance of a Collection
    /// </summary>
    [DataContract]
    public class CustomizedProductCollectionDTO : DTO{
        /**
        <summary>
            long with the id of the Collection in the DB.
        </summary>
        */
        [DataMember(Name="id")]
        public long id { get; set; }


        /**
        <summary>
            String with the Collection name.
        </summary>
        */
        [DataMember(Name="name")]
        public string name { get; set; }

        /// <summary>
        /// List with the collection customized products
        /// </summary>
        [DataMember(Name="customizedProducts")]
        public List<CustomizedProductDTO> customizedProducts{get;set;}

        
      /*   public CustomizedProductCollection toEntity()
        {
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            foreach(CustomizedProductDTO dto in this.customizedProducts){
                list.Add(dto.toEntity());
            }
            CustomizedProductCollection instance = new CustomizedProductCollection(this.name,list);
            instance.Id = this.id;
            return instance;
        } */
    }
}