using System.Collections.Generic;

namespace core.domain {
    /// <summary>
    /// Algorithm interface
    /// </summary>
    public interface Algorithm {
        /// <summary>
        /// Returns a list of the required inputs to apply the algorithm
        /// </summary>
        /// <returns>list of the required inputs</returns>
        List<Input> getRequiredInputs();
        /// <summary>
        /// Sets the input values for the algorithm to be applied
        /// </summary>
        /// <param name="inputs">list of inputs with values</param>
        /// <returns>true if the values were successfully set, throws ArgumentException if any value was not valid</returns>
        bool setInputValues(List<Input> inputs);
        /// <summary>
        /// Checks if input values are within the allowed range
        /// </summary>
        /// <param name="inputs">list of inputs with values to check</param>
        /// <returns>true if values are within allowe range, throws ArgumentException if any value was not within the allowed range</returns>
        bool isWithinDataRange(List<Input> inputs);
    }
}
