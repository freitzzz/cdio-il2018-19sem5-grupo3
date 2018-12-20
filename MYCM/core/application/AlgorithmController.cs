using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.exceptions;
using core.modelview.algorithm;
using core.modelview.input;
using support.dto;

namespace core.application
{
    /// <summary>
    /// Core AlgorithmControler class
    /// </summary>
    public class AlgorithmController
    {
        /// <summary>
        /// Constant that represents the message presented when no Algorithms are available.
        /// </summary>
        private const string NO_ALGORITHMS_AVAILABLE = "There are no algorithms available.";

        /// <summary>
        /// Constant representing the message presented when the Algorithm has no required inputs.
        /// </summary>
        private const string NO_REQUIRED_INPUTS = "The algorithm has no required inputs.";

        /// <summary>
        /// Creates a new instance of AlgorithmController
        /// </summary>
        public AlgorithmController() { }

        /// <summary>
        /// Returns an instance of GetAllAlgorithmsModelView representing all the available types of Algorithm.
        /// </summary>
        /// <returns>GetAllAlgorithmsModelView representing all the available types of Algorithm.</returns>
        public GetAllAlgorithmsModelView getAllAlgorithms()
        {
            Array availableAlgorithms = Enum.GetValues(typeof(RestrictionAlgorithm));

            if (availableAlgorithms.Length == 0)
            {
                throw new ResourceNotFoundException(NO_ALGORITHMS_AVAILABLE);
            }

            GetAllAlgorithmsModelView allAlgorithmsModelView = new GetAllAlgorithmsModelView();

            foreach (RestrictionAlgorithm restrictionAlgorithm in availableAlgorithms)
            {
                Algorithm algorithm = new AlgorithmFactory().createAlgorithm(restrictionAlgorithm);
                GetBasicAlgorithmModelView algorithmModelView = AlgorithmModelViewService.fromEntityAsBasic(algorithm);
                allAlgorithmsModelView.Add(algorithmModelView);
            }

            return allAlgorithmsModelView;
        }


        /// <summary>
        /// Returns an instance of GetAlgorithmModelView representing the instance of Algorithm.
        /// </summary>
        /// <param name="restrictionAlgorithm">RestrictionAlgorithm that matches the corresponding Algorithm.</param>
        /// <returns>GetAlgorithmModelView representing the instance of Algorithm.</returns>
        public GetAlgorithmModelView getAlgorithm(RestrictionAlgorithm restrictionAlgorithm)
        {
            //this throws ArgumentOutOfRangeException if the element is not recognized by the factory
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(restrictionAlgorithm);

            return AlgorithmModelViewService.fromEntity(algorithm);
        }

        /// <summary>
        /// Returns an instance of GetAllInputsModelView representing the Algorithm's required inputs. 
        /// </summary>
        /// <param name="restrictionAlgorithm">RestrictionAlgorithm that matches the corresponding Algorithm.</param>
        /// <returns>GetAllInputsModelView representing the Algorithm's required inputs. </returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the Algorithm has no required inputs.</exception>
        public GetAllInputsModelView getAlgorithmRequiredInputs(RestrictionAlgorithm restrictionAlgorithm)
        {
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(restrictionAlgorithm);

            List<Input> requiredInputs = algorithm.getRequiredInputs();

            if (!requiredInputs.Any()) throw new ResourceNotFoundException(NO_REQUIRED_INPUTS);

            return InputModelViewService.fromCollection(requiredInputs);
        }
    }
}