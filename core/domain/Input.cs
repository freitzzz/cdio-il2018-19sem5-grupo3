using support.utils;
using System;

namespace core.domain {
    /// <summary>
    /// Class that represents an input for an algorithm
    /// </summary>
    public class Input {
        private const string INVALID_INPUT_NAME = "Input name is not valid!";
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
    }
}
