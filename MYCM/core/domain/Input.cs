using core.dto;
using support.dto;
using support.utils;
using System;

namespace core.domain {
    /// <summary>
    /// Class that represents an input for an algorithm
    /// </summary>
    public class Input : DTOAble<InputDTO> {
        /// <summary>
        /// Message when input name is not valid
        /// </summary>
        private const string INVALID_INPUT_NAME = "Input name is not valid!";
        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id { get; internal set; }
        /// <summary>
        /// Name of the input
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Value of the input
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// Empty constructor
        /// </summary>
        private Input() {
        }
        /// <summary>
        /// Creates a new instance of Input with the provided data
        /// </summary>
        /// <param name="name">name of the input</param>
        public Input(string name) {
            checkName(name);
            this.name = name;
        }
        /// <summary>
        /// Checks if name is not null nor empty
        /// </summary>
        /// <param name="name">name to be checked</param>
        private void checkName(string name) {
            if (Strings.isNullOrEmpty(name)) {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// Returns DTO equivalent of the Entity
        /// </summary>
        /// <returns>DTO equivalent of the Entity</returns>
        public InputDTO toDTO() {
            InputDTO dto = new InputDTO();
            dto.id = Id;
            dto.name = name;
            dto.value = value;
            return dto;
        }
    }
}
