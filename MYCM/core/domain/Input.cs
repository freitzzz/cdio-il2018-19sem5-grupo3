using core.dto;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;

namespace core.domain
{
    /// <summary>
    /// Class that represents an input for an algorithm
    /// </summary>
    public class Input : ValueObject, DTOAble<InputDTO>
    {
        /// <summary>
        /// Message when input name is not valid
        /// </summary>
        private const string INVALID_INPUT_NAME = "Input name is not valid!";

        /// <summary>
        /// Input's persistence identifier.
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// Input's name.
        /// </summary>
        /// <value>Gets/Protected sets the name.</value>
        public string name { get; protected set; }

        /// <summary>
        /// String representation of all the Input's allowed values.
        /// </summary>
        /// <value>Gets/Protected sets the range.</value>
        //*Please note that this is only used for visual aid when inputting data and should not be used to enforce values*/
        public string range { get; protected set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        private Input() { }

        /// <summary>
        /// Creates a new instance of Input with the provided data
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="range">Input's allowed value range.</param>
        private Input(string name, string range)
        {
            checkName(name);
            this.name = name;
            this.range = range;
        }

        /// <summary>
        /// Creates a new instance of Input.
        /// </summary>
        /// <param name="name">Input's name.</param>
        /// <param name="range">Input's allowed value range.</param>
        /// <returns>Created instance of Input.</returns>
        public static Input valueOf(string name, string range)
        {
            return new Input(name, range);
        }

        /// <summary>
        /// Checks if the name is not null nor empty.
        /// </summary>
        /// <param name="name">Name being checked.</param>
        private void checkName(string name)
        {
            if (Strings.isNullOrEmpty(name)) throw new ArgumentException(INVALID_INPUT_NAME);
        }

        /// <summary>
        /// Checks if the range is not null nor empty.
        /// </summary>
        /// <param name="range">Range being checked.</param>
        private void checkRange(string range)
        {
            if (Strings.isNullOrEmpty(range)) throw new ArgumentException();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            Input other = (Input)obj;

            return this.name.Equals(other.name);
        }

        public override int GetHashCode()
        {
            int hash = 21;
            hash = hash * 37 + this.name.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}; Range: {1}", this.name, this.range);
        }

        /// <summary>
        /// Returns DTO equivalent of the Entity
        /// </summary>
        /// <returns>DTO equivalent of the Entity</returns>
        public InputDTO toDTO()
        {
            InputDTO dto = new InputDTO();
            dto.id = Id;
            dto.name = name;
            return dto;
        }
    }
}
