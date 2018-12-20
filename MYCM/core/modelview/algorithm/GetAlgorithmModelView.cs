using System.Runtime.Serialization;
using core.domain;
using core.modelview.input;

namespace core.modelview.algorithm
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of Algorithm.
    /// </summary>
    [DataContract]
    public class GetAlgorithmModelView
    {
        /// <summary>
        /// Algorithm's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Algorithm's description.
        /// </summary>
        /// <value>Gets/Sets the description.</value>
        [DataMember]
        public string description { get; set; }

        /// <summary>
        /// Algorithm's required inputs.
        /// </summary>
        /// <value>Gets/Sets the required inputs.</value>
        [DataMember(EmitDefaultValue = false)] //not all algorithm's require inputs
        public GetAllInputsModelView requiredInputs { get; set; }
    }
}