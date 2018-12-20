using System;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// Data Transfer Object that represents an Input
    /// </summary>
    public class InputDTO : DTO, DTOParseable<Input, InputDTO> {
        /// <summary>
        /// Input's id
        /// </summary>
        public long id;
        /// <summary>
        /// Name of the input
        /// </summary>
        public string name;
        /// <summary>
        /// Returns Entity equivalent of the DTO
        /// </summary>
        /// <returns>Entity equivalent of the DTO</returns>
        public Input toEntity() {
            throw new NotImplementedException();
        }
    }
}
