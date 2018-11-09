using core.modelview.algorithm;
using System.Runtime.Serialization;

namespace core.modelview.restriction{
    /// <summary>
    /// Model View representation for the fetch restriction basic information context
    /// </summary>
    [DataContract]
    public sealed class GetBasicRestrictionModelView{
        /// <summary>
        /// Long with the restriction ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the restriction description
        /// </summary>
        [DataMember(Name="description")]
        public string description{get;set;}

        /// <summary>
        /// GetBasicAlgorithmModelView with the restriction algorithm
        /// </summary>
        [DataMember(Name="algorithm")]
        public GetBasicAlgorithmModelView algorithm{get;set;}
    }
}