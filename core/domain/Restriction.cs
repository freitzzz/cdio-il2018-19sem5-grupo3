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

namespace core.domain {
    /// <summary>
    /// Represents a Product Restriction
    /// </summary>
    public class Restriction : DTOAble<RestrictionDTO> {
        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id { get; internal set; }
        /// <summary>
        /// Description of the restriction
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Algorithm aplied by this restriction
        /// </summary>
        [NotMapped]
        private Algorithm algorithm;
        /// <summary>
        /// Returns DTO equivalent of the Restriction
        /// </summary>
        /// <returns>DTO equivalent of the Restriction</returns>
        public RestrictionDTO toDTO() {
            RestrictionDTO dto = new RestrictionDTO();
            dto.id = Id;
            dto.description = description;
            return dto;
        }
    }
}
