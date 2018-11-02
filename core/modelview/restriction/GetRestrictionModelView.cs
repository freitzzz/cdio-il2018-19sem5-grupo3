using System.Runtime.Serialization;

namespace core.modelview.restriction{
    /// <summary>
    /// Model View representation for the fetch restriction information context
    /// </summary>
    //TODO: COMPLETE MODEL VIEW
    [DataContract]
    public sealed class GetRestrictionModelView{
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
        /// Short with the restriction algorithm ID
        /// </summary>
        [DataMember(Name="algorithm")]
        public short algorithm{get;set;}
    }
}