using System;
using System.Runtime.Serialization;
using core.domain;
using support.dto;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.dto
{
    [DataContract]
    [JsonConverter(typeof(DimensionDTOJsonConverter))]
    public abstract class DimensionDTO : DTO, DTOParseable<Dimension, DimensionDTO>
    {

        /// <summary>
        /// Dimension's database identifier
        /// </summary>
        /// <value>Get/Set of the dimension's database identifier</value>
        [DataMember(Order = 1)]
        public long id { get; set; }

        /// <summary>
        /// Dimension's measurement unit.
        /// </summary>
        /// <value>Gets/Sets the measurement unit.</value>
        [DataMember(EmitDefaultValue = false)]
        public string unit { get; set; }

        /// <summary>
        /// Dimension's list of restrictions.
        /// </summary>
        /// <value>Gets/Sets the list of restrictions.</value>
        [DataMember(EmitDefaultValue = false)]
        public List<RestrictionDTO> restrictions {get; set;}

        /// <summary>
        /// Builds a Dimension instance from a DimensionDTO
        /// </summary>
        /// <returns>Dimension instance</returns>
        public abstract Dimension toEntity();        
    }
}