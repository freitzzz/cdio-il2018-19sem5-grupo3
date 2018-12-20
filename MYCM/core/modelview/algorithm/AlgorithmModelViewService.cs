using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.input;

namespace core.modelview.algorithm
{
    /// <summary>
    /// Static class representing the service used for converting instances of Algorithm into ModelView representations.
    /// </summary>
    public static class AlgorithmModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when the provided Algorithm is null.
        /// </summary>
        private const string NULL_ALGORITHM = "The provided algorithm can't be converted into a view.";

        /// <summary>
        /// Constant representing the message presented when the provided IEnumerable is null.
        /// </summary>
        private const string NULL_ALGORITHM_COLLECTION = "The provided collection of algorithms can't be converted into views.";

        /// <summary>
        /// Creates an instance of GetBasicAlgorithmModelView from an instance of Algorithm.
        /// </summary>
        /// <param name="algorithm">Instance of Algorithm being converted.</param>
        /// <returns>Instance of GetBasicModelView with the Algorithm's basic information.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Algorithm is null.</exception>
        public static GetBasicAlgorithmModelView fromEntityAsBasic(Algorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException(NULL_ALGORITHM);
            }

            GetBasicAlgorithmModelView basicAlgorithmModelView = new GetBasicAlgorithmModelView();
            basicAlgorithmModelView.algorithm = algorithm.restrictionAlgorithm;
            basicAlgorithmModelView.name = algorithm.name;

            return basicAlgorithmModelView;
        }

        /// <summary>
        /// Creates an instance of GetAlgorithm from an instance of Algorithm.
        /// </summary>
        /// <param name="algorithm">Instance of Algorithm being converted.</param>
        /// <returns>Instance of GetAlgorithmModelView with the Algorithm's data.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Algorithm is null.</exception>
        public static GetAlgorithmModelView fromEntity(Algorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentException(NULL_ALGORITHM);
            }

            GetAlgorithmModelView algorithmModelView = new GetAlgorithmModelView();
            algorithmModelView.name = algorithm.name;
            algorithmModelView.description = algorithm.description;

            IEnumerable<Input> requiredInputs = algorithm.getRequiredInputs();

            if (requiredInputs.Any())
            {
                algorithmModelView.requiredInputs = InputModelViewService.fromCollection(requiredInputs);
            }

            return algorithmModelView;
        }

        /// <summary>
        /// Creates an instance of GetAllAlgorithmsModelView from an IEnumerable of Algorithm.
        /// </summary>
        /// <param name="algorithms">IEnumerable of Algorithm being converted.</param>
        /// <returns>Instance of GeAllAlgorithmsModelView with the IEnumerable's data.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of Algorithm.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the instances of Algorithm in the IEnumerable is null.</exception>
        public static GetAllAlgorithmsModelView fromCollection(IEnumerable<Algorithm> algorithms)
        {
            if (algorithms == null)
            {
                throw new ArgumentNullException(NULL_ALGORITHM_COLLECTION);
            }

            GetAllAlgorithmsModelView allAlgorithmsModelView = new GetAllAlgorithmsModelView();

            foreach (Algorithm algorithm in algorithms)
            {
                allAlgorithmsModelView.Add(fromEntityAsBasic(algorithm));
            }

            return allAlgorithmsModelView;
        }
    }
}