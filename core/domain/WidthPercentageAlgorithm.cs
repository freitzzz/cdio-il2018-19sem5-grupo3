using System;
using System.Collections.Generic;

namespace core.domain {
    /// <summary>
    /// Class that represents the algorithm that checks if complement occupies a certain percentage of the parent product
    /// </summary>
    public class WidthPercentageAlgorithm : Algorithm {
        /// <summary>
        /// Name of minimum percentage input
        /// </summary>
        private const string MINIMUM_PERCENTAGE_INPUT_NAME = "Minimum Percentage";
        /// <summary>
        /// Name of maximum percentage input
        /// </summary>
        private const string MAXIMUM_PERCENTAGE_INPUT_NAME = "Maximum Percentage";
        /// <summary>
        /// Invalid input message
        /// </summary>
        private const string INVALID_INPUT = "Input is invalid!";
        /// <summary>
        /// Input value invalid message
        /// </summary>
        private const string INPUT_OUTSIDE_RANGE = "Input is not within the established range!";
        /// <summary>
        /// Minimum percentage of occupation
        /// </summary>
        private double minPercentage;
        /// <summary>
        /// Maximum percentage of occupation
        /// </summary>
        private double maxPercentage;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public WidthPercentageAlgorithm() {
        }
        /// <summary>
        /// Returns a list of the required inputs to apply the algorithm
        /// </summary>
        /// <returns>list of the required inputs</returns>
        public List<Input> getRequiredInputs() {
            List<Input> inputs = new List<Input>();
            Input input1 = new Input(MINIMUM_PERCENTAGE_INPUT_NAME);
            Input input2 = new Input(MAXIMUM_PERCENTAGE_INPUT_NAME);
            inputs.Add(input1);
            inputs.Add(input2);
            return inputs;
        }
        /// <summary>
        /// Sets the input values for the algorithm to be applied
        /// </summary>
        /// <param name="inputs">list of inputs with values</param>
        /// <returns>true if the values were successfully set, throws ArgumentException if any value was not valid</returns>
        public bool setInputValues(List<Input> inputs) {
            if (inputs == null || inputs.Count == 0 || inputs.Count != 2) {
                throw new ArgumentException(INVALID_INPUT);
            }
            isWithinDataRange(inputs);
            foreach (Input input in inputs) {
                switch (input.name) {
                    case MINIMUM_PERCENTAGE_INPUT_NAME:
                        minPercentage = Convert.ToDouble(input.value);
                        break;
                    case MAXIMUM_PERCENTAGE_INPUT_NAME:
                        maxPercentage = Convert.ToDouble(input.value);
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if input values are within the allowed range
        /// </summary>
        /// <param name="inputs">list of inputs with values to check</param>
        /// <returns>true if values are within allowed range, throws ArgumentException if any value was not within the allowed range, throws FormatException if any input value is not a double</returns>
        public bool isWithinDataRange(List<Input> inputs) {
            if (inputs == null || inputs.Count == 0 || inputs.Count != 2) {
                throw new ArgumentException(INVALID_INPUT);
            }
            double minPercentage = -1;
            double maxPercentage = -1;
            foreach (Input input in inputs) {
                if (String.IsNullOrEmpty(input.name)) {
                    throw new ArgumentException(INVALID_INPUT);
                }
                switch (input.name) {
                    case MINIMUM_PERCENTAGE_INPUT_NAME:
                        minPercentage = Convert.ToDouble(input.value);
                        break;
                    case MAXIMUM_PERCENTAGE_INPUT_NAME:
                        maxPercentage = Convert.ToDouble(input.value);
                        break;
                    default:
                        throw new ArgumentException(INVALID_INPUT);
                }
            }
            return minPercentage >= 0 && minPercentage <= 1 && maxPercentage >= minPercentage && maxPercentage <= 1 ? true : throw new ArgumentException(INPUT_OUTSIDE_RANGE);
        }
    }
}
