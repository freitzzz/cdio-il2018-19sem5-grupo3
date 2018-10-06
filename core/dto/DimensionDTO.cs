using System;
using System.Runtime.Serialization;
using core.domain;
using support.dto;
using Newtonsoft.Json;

namespace core.dto
{
    [DataContract]
    [JsonConverter(typeof(DimensioDTOJsonConverter))]
    public abstract class DimensionDTO : DTO, DTOParseable<Dimension, DimensionDTO>
    {

        /// <summary>
        /// Dimension's database identifier
        /// </summary>
        /// <value>Get/Set of the dimension's database identifier</value>
        [DataMember(Order = 1)]
        public long id { get; set; }

        /// <summary>
        /// Builds a Dimension instance from a DimensionDTO
        /// </summary>
        /// <returns>Dimension instance</returns>
        public abstract Dimension toEntity();
    }
}