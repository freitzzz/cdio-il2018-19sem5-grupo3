using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;
using support.utils;

namespace core.dto
{
    /// <summary>
    /// DTO representation of a Slot
    /// </summary>
    /// <typeparam name="Slot">Type of domain entity</typeparam>
    /// <typeparam name="SlotDTO">Type of DTO</typeparam>
    [DataContract]
    public class SlotDTO : DTO, DTOParseable<Slot, SlotDTO>
    {
        /// <summary>
        /// Slots database identifier
        /// </summary>
        /// <value>Gets/Sets the value of the identifier</value>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Slots customized dimensions
        /// </summary>
        /// <value>Gets/Sets the value of the instance</value>
        [DataMember]
        public CustomizedDimensionsDTO customizedDimensions { get; set; }

        /// <summary>
        /// Slots list of customized products
        /// </summary>
        /// <value>Gets/Sets the value of the list</value>
        [DataMember]
        public List<CustomizedProductDTO> customizedProducts { get; set; }

        public Slot toEntity()
        {
            throw new NotSupportedException();
        }
    }
}