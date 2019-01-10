using core.domain;
using core.modelview.algorithm;
using core.modelview.inputvalue;
using System.Runtime.Serialization;

namespace core.modelview.restriction {
    /// <summary>
    /// Model View representation for the fetch restriction information context
    /// </summary>
    [DataContract]
    public sealed class AddRestrictionModelView {

        /// <summary>
        /// String with the restriction description
        /// </summary>
        [DataMember(Name = "description")]
        public string description { get; set; }

        /// <summary>
        /// Integer with the algorithm's id
        /// </summary>
        [DataMember(Name = "algorithmId")]
        public RestrictionAlgorithm algorithmId { get; set; }

        /// <summary>
        /// Input values for the restriction's algorithm
        /// </summary>
        [DataMember(Name = "inputValues")]
        public AddInputValuesModelView inputValues { get; set; }
    }
}