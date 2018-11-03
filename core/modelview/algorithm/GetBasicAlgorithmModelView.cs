using System;
using System.Runtime.Serialization;
using core.domain;

namespace core.modelview.algorithm {
    /// <summary>
    /// Basic algorithm information modelview
    /// </summary>
    public class GetBasicAlgorithmModelView {
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
    }
}