using System;

namespace core.domain {
    /// <summary>
    /// Class responsible for creating instances of Algorithm
    /// </summary>
    public class AlgorithmFactory {
        /// <summary>
        /// Message stating the algorithm does not exist
        /// </summary>
        private const string ALGORITHM_DOES_NOT_EXIST_MESSAGE = "Algorithm does not exist!";
        /// <summary>
        /// Empty constructor
        /// </summary>
        public AlgorithmFactory() {
        }
        /// <summary>
        /// Creates an Algorithm using the data received by parameter
        /// </summary>
        /// <param name="reAlg">Type of algorithm to create</param>
        /// <returns>instance of Algorithm</returns>
        public Algorithm createAlgorithm(RestrictionAlgorithm reAlg) {
            Algorithm algorithm;
            switch (reAlg) {
                case RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM:
                    algorithm = new WidthPercentageAlgorithm();
                    break;
                case RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM:
                    algorithm = new SameMaterialAndFinishAlgorithm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(ALGORITHM_DOES_NOT_EXIST_MESSAGE);
            }
            return algorithm;
        }
    }
}
