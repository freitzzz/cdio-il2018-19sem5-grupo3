using System.Runtime.Serialization;
using core.modelview.dimension.converters;
using Newtonsoft.Json;

namespace core.modelview.dimension{
    /// <summary>
    /// Model View representation for the fetch dimension information context
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(GetDimensionModelViewConverter))]
    public abstract class GetDimensionModelView{
        /// <summary>
        /// Long with the dimension ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the dimension unit
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string unit{get;set;}
    }
}