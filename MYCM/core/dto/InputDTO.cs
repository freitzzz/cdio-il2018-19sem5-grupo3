using System;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// Data Transfer Object that represents an Input
    /// </summary>
    [DataContract]
    public class InputDTO : DTO, DTOParseable<Input, InputDTO> {
        /// <summary>
        /// Input's id
        /// </summary>
        [DataMember]
        public long id { get; set; }
        /// <summary>
        /// Name of the input
        /// </summary>
        [DataMember]
        public string name { get; set; }
        /// <summary>
        /// Range of the input
        /// </summary>
        [DataMember]
        public string range { get; set; }
        /// <summary>
        /// Returns Entity equivalent of the DTO
        /// </summary>
        /// <returns>Entity equivalent of the DTO</returns>
        public Input toEntity() {
            return Input.valueOf(name, range);
        }
    }
}
