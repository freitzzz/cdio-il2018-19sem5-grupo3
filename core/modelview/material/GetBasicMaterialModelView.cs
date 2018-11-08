using System.Runtime.Serialization;

namespace core.modelview.material{
    /// <summary>
    /// Model View representation for the fetch restriction basic information context
    /// </summary>
    [DataContract]
    public sealed class GetBasicMaterialModelView{
        /// <summary>
        /// Long with the material ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the material reference
        /// </summary>
        [DataMember(Name="reference")]
        public string reference{get;set;}

        /// <summary>
        /// String with the material desigation
        /// </summary>
        [DataMember(Name="designation")]
        public string designation{get;set;}
    }
}