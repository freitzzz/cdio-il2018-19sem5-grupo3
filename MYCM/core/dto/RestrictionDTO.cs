using core.domain;
using support.dto;
using System;
using System.Collections.Generic;

namespace core.dto {
    /// <summary>
    /// DTO of Restriction
    /// </summary>
    public class RestrictionDTO : DTO, DTOParseable<Restriction, RestrictionDTO> {
        /// <summary>
        /// Restriction's id
        /// </summary>
        public long id;
        /// <summary>
        /// Restriction's description
        /// </summary>
        public string description;
        /// <summary>
        /// List of Restriction algorithm inputs
        /// </summary>
        public List<InputDTO> inputs;
        /// <summary>
        /// Algorithm of the restriction
        /// </summary>
        public RestrictionAlgorithm algorithm;
        /// <summary>
        /// Returns Entity equivalent of the DTO
        /// </summary>
        /// <returns>Entity equivalent of the DTO</returns>
        public Restriction toEntity() {
            throw new NotImplementedException();
        }
    }
}