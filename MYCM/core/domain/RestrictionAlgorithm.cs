using System;
namespace core.domain {
    /// <summary>
    /// Enum with the names of every restriction algorithm
    /// </summary>
    public enum RestrictionAlgorithm {
        WIDTH_PERCENTAGE_ALGORITHM = 1,
        SAME_MATERIAL_AND_FINISH_ALGORITHM = 2

    }
    public static class AlgorithmAttributes {
        private const string WIDTH_PERCENTAGE_ALGORITHM_NAME = "Width Percentage Algorithm";
        private const string WIDTH_PERCENTAGE_ALGORITHM_DESCRIPTION = "Restrains the component to occupy a certain percentage of the father product's width.";
        private const string SAME_MATERIAL_AND_FINISH_ALGORITHM_NAME = "Same Material and Finish Algorithm";
        private const string SAME_MATERIAL_AND_FINISH_ALGORITHM_DESCRIPTION = "Limits the component's material and finish to the same material and finish as its father product.";
        public static string getName(RestrictionAlgorithm algorithm) {
            switch (algorithm) {
                case RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM:
                    return WIDTH_PERCENTAGE_ALGORITHM_NAME;
                case RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM:
                    return SAME_MATERIAL_AND_FINISH_ALGORITHM_NAME;
                default:
                    throw new ArgumentException();
            }
        }
        public static string getDescription(RestrictionAlgorithm algorithm) {
            switch (algorithm) {
                case RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM:
                    return WIDTH_PERCENTAGE_ALGORITHM_DESCRIPTION;
                case RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM:
                    return SAME_MATERIAL_AND_FINISH_ALGORITHM_DESCRIPTION;
                default:
                    throw new ArgumentException();
            }
        }
    }
}