using System;
using System.Runtime.Serialization;
using core.domain;

namespace core.modelview.algorithm
{
    /// <summary>
    /// Class representing the ModelView used for retrieving basic informationf from an instance of Algorithm.
    /// </summary>
    public class GetBasicAlgorithmModelView
    {
        /// <summary>
        /// Algorithm's RestrictionAlgorithm enumerate member.
        /// </summary>
        /// <value>Gets/Sets the RestrictionAlgorithm.</value>
        [DataMember(Name = "id")]
        public RestrictionAlgorithm algorithm { get; set; }

        /// <summary>
        /// Algorithm's name.
        /// </summary>
        /// <value>Gets/Sets the name.</value>
        [DataMember]
        public string name { get; set; }
    }
}