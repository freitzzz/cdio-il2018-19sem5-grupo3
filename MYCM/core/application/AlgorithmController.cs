using System;
using System.Collections.Generic;
using core.domain;
using core.dto;
using core.modelview.algorithm;
using support.dto;

namespace core.application {
    /// <summary>
    /// Core AlgorithmControler class
    /// </summary>
    public class AlgorithmController {
        /// <summary>
        /// Creates a new instance of AlgorithmController
        /// </summary>
        public AlgorithmController() { }
        /// <summary>
        /// Returns all available algorithms
        /// </summary>
        /// <returns>list of AlgorithmDTO containing the algorithm's id and name</returns>
        public List<GetBasicAlgorithmModelView> getAllAlgorithms() {
            List<GetBasicAlgorithmModelView> dtos = new List<GetBasicAlgorithmModelView>();
            foreach (RestrictionAlgorithm resAlg in Enum.GetValues(typeof(RestrictionAlgorithm))) {
                GetBasicAlgorithmModelView dto = new GetBasicAlgorithmModelView();
                dto.id = resAlg;
                dto.name = AlgorithmAttributes.getName(resAlg);
                dtos.Add(dto);
            }
            return dtos;
        }
        /// <summary>
        /// Returns the details of one algorithm
        /// </summary>
        /// <param name="alg">id of the algorithm</param>
        /// <returns>AlgorithmDTO containing the details of the algorithm</returns>
        public AlgorithmDTO getAlgorithm(RestrictionAlgorithm alg) {
            AlgorithmDTO dto = new AlgorithmDTO();
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(alg);
            dto.id = alg;
            dto.name = AlgorithmAttributes.getName(alg);
            dto.description = AlgorithmAttributes.getDescription(alg);
            dto.inputs = (List<InputDTO>)DTOUtils.parseToDTOS(algorithm.getRequiredInputs());
            return dto;
        }
        /// <summary>
        /// Returns a list of the algorithm inputs
        /// </summary>
        /// <param name="alg">algorithm to fetch the inputs from</param>
        /// <returns>list of inputDTO containing all the inputs of an algorithm</returns>
        public List<InputDTO> getAlgorithmInputs(RestrictionAlgorithm alg) {
            return (List<InputDTO>)DTOUtils.parseToDTOS(new AlgorithmFactory().createAlgorithm(alg).getRequiredInputs());
        }
    }
}