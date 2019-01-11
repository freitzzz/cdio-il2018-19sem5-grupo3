using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.utils;

namespace core.domain {
    /// <summary>
    /// Abstract class representing an Algorithm.
    /// </summary>
    public abstract class Algorithm {
        /// <summary>
        /// Constant representing the message presented when an instance of Algorithm is attempted to be created without a valid name.
        /// </summary>
        private const string INVALID_ALGORITHM_NAME = "The algorithm's name is not valid.";

        /// <summary>
        /// Constant representing the message presented when an instance of Algorithm is attempted to be created without a valid description.
        /// </summary>
        private const string INVALID_ALGORITHM_DESCRIPTION = "The algorithm's description is not valid.";

        /// <summary>
        /// Constant representing the message presented when the Algorithm's Collection of Input is not valid.
        /// </summary>
        private const string INVALID_INPUT_LIST = "The algorithm's input list is not valid.";

        /// <summary>
        /// Constant representing the message presented when a provided input is a duplicate.
        /// </summary>
        private const string DUPLICATE_INPUT = "The input with the name '{0}' is a duplicate";

        /// <summary>
        /// Constant representing the message presented when an input has no value.
        /// </summary>
        private const string INPUT_NOT_SET = "The input with the name '{0}' has no value yet";

        /// <summary>
        /// Constant representing the message presented when an Input is not valid.
        /// </summary>
        private const string SET_INVALID_INPUT = "The input being set is not valid.";

        /// <summary>
        /// Constant representing the message presented when the Dictionary of Input, String pairs is not valid.
        /// </summary>
        private const string SET_INVALID_INPUTS = "The inputs being set are not valid";

        /// <summary>
        /// Constant representing the message presented when an Input being set does not exist in the Algorithm's Collection of Input.
        /// </summary>
        private const string INPUT_NOT_FOUND = "The algorithm does not have the input being set.";

        /// <summary>
        /// Algorithm's persistence identifier.
        /// </summary>
        /// <value>Gets/Protected sets the persistence identifier.</value>
        public long Id { get; protected set; }

        /// <summary>
        /// Enumerate element representing the Algorithm.
        /// </summary>
        /// <value>Gets/Protected sets the enumerate element.</value>
        public RestrictionAlgorithm restrictionAlgorithm { get; protected set; }

        /// <summary>
        /// Algorithm's name.
        /// </summary>
        /// <value>Gets/Protected sets the Algorithm's name.</value>
        public string name { get; protected set; }

        /// <summary>
        /// Algorithm's description.
        /// </summary>
        /// <value>Gets/Protected sets the Algorithm's description.</value>
        public string description { get; protected set; }

        /// <summary>
        /// Algorithm's List of Input.
        /// </summary>
        /// <value>Gets/Protected Sets the Algorithm's inputs.</value>
        protected List<InputValue> _inputValues;
        public List<InputValue> inputValues { get => LazyLoader.Load(this, ref _inputValues); protected set => _inputValues = value; }

        /// <summary>
        /// ILazyLoader injected by the framework.
        /// </summary>
        /// <value>Gets/Sets the ILazyLoader.</value>
        [NotMapped]
        protected ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">Instance of ILazyLoader being injected.</param>
        protected Algorithm(ILazyLoader lazyLoader) {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor so that subclasses can have en empty constructor.
        /// </summary>
        protected Algorithm() { }

        /// <summary>
        /// Checks if the provided name is not null nor empty.
        /// </summary>
        /// <param name="name">String representing the Algorithm's name.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided name is null or empty.</exception>
        protected void checkName(string name) {
            if (Strings.isNullOrEmpty(name)) throw new ArgumentException(INVALID_ALGORITHM_NAME);
        }

        /// <summary>
        /// Checks if the provided description is not null nor empty.
        /// </summary>
        /// <param name="description">String representing the Algorithm's description.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided description is null or empty.</exception>
        protected void checkDescription(string description) {
            if (Strings.isNullOrEmpty(description)) throw new ArgumentException(INVALID_ALGORITHM_DESCRIPTION);
        }

        /// <summary>
        /// Checks if the IEnumerable of Input is null or has any duplicate Input element.
        /// </summary>
        /// <param name="inputs">IEnumerable of Input being checked.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of Input is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when any of the istances of Input in the IEnumerable is a duplicate.</exception>
        protected void checkInputs(IEnumerable<Input> inputs) {
            if (inputs == null) {
                throw new ArgumentNullException(INVALID_INPUT_LIST);
            }

            HashSet<Input> inputHashSet = new HashSet<Input>();
            foreach (Input input in inputs) {
                if (!inputHashSet.Add(input)) {
                    throw new ArgumentException(string.Format(DUPLICATE_INPUT, input.name));
                }
            }
        }

        /// <summary>
        /// Returns a list of the required inputs to apply the algorithm
        /// </summary>
        /// <returns>list of the required inputs</returns>
        public List<Input> getRequiredInputs() {
            return this.inputValues.Select(iv => iv.input).ToList();
        }

        /// <summary>
        /// Sets the value of the InputValue with a matching Input.
        /// </summary>
        /// <param name="input">Instance of Input.</param>
        /// <param name="value">String representing the value being set.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when either the provided Input is null, the value is null or empty or the Input is not one of the Algorithm's inputs.
        /// </exception>
        public void setInputValue(Input input, string value) {
            if (input == null) throw new ArgumentNullException(SET_INVALID_INPUT);

            InputValue inputValue = this.inputValues.Where(iv => iv.input.Equals(input)).SingleOrDefault();

            if (inputValue == null) throw new ArgumentException(INPUT_NOT_FOUND);

            //check if the value is allowed (this is the Algorithm's responsibility)
            checkValue(input, value);

            inputValue.value = value;
        }

        /// <summary>
        /// Sets the values for multiple Inputs.
        /// </summary>
        /// <param name="inputValues">Dictionary containing the Inputs and their new values.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided Dictionary is null.</exception>
        public void setInputValues(Dictionary<Input, string> inputValues) {
            if (inputValues == null) throw new ArgumentNullException(SET_INVALID_INPUTS);

            foreach (KeyValuePair<Input, string> inputValue in inputValues) {
                setInputValue(inputValue.Key, inputValue.Value);
            }
        }

        /// <summary>
        /// Checks if an algorithm is ready to be applied (if all inputs have values)
        /// </summary>
        public void ready() {
            foreach (InputValue inputValue in inputValues) {
                if (inputValue.value == null) {
                    throw new ArgumentNullException(string.Format(INPUT_NOT_SET, inputValue.input.name));
                }
            }
        }

        /// <summary>
        /// Check if the value is valid for a given Input.
        /// </summary>
        /// <param name="input">Input whose value is being set.</param>
        /// <param name="value">Value being set.</param>
        protected abstract void checkValue(Input input, string value);

        /// <summary>
        /// Applies the algorithm
        /// </summary>
        /// <param name="customProduct">product customized by the user</param>
        /// <param name="product">product to which the restriction will apply</param>
        /// <returns>modified version of the product received by parameter with the restricted data</returns>
        public abstract Product apply(CustomizedProduct customProduct, Product product);
    }
}
