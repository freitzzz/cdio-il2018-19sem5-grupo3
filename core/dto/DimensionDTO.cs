using System;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto{
    [DataContract]
    public abstract class DimensionDTO : DTO, DTOParseable<Dimension,DimensionDTO>{

        /// <summary>
        /// Dimension's database identifier
        /// </summary>
        /// <value>Get/Set of the dimension's database identifier</value>
        [DataMember]
        public long id {get;set;}

        /// <summary>
        /// Dimension's type (e.g. continuous)
        /// </summary>
        /// <value>Get/Set of the dimension's type</value>
        [DataMember]
        public string type {get;set;}

        /// <summary>
        /// Builds a Dimension instance from a DimensionDTO
        /// </summary>
        /// <returns>Dimension instance</returns>
        public abstract Dimension toEntity();
    }
}