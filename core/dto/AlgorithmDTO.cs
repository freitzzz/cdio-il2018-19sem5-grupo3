using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// DTO that represents an algorithm
    /// </summary>
    public class AlgorithmDTO : DTO {
        /// <summary>
        /// Algorithm id
        /// </summary>
        /// <value>gets and sets the value of the id</value>
        [DataMember]
        public RestrictionAlgorithm id { get; set; }
        /// <summary>
        /// Name of the algorithm
        /// </summary>
        /// <value>gets and sets the value of the name</value>
        [DataMember]
        public string name { get; set; }
        /// <summary>
        /// Description of the algorithm
        /// </summary>
        /// <value>gets and sets the value of the description</value>
        [DataMember]
        public string description { get; set; }
        /// <summary>
        /// List of inputs for the algorithm to work
        /// </summary>
        /// <value>gets and sets the input list</value>
        public List<InputDTO> inputs { get; set; }
    }
}