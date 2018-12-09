using System.Runtime.Serialization;

namespace core.modelview.material{
    /// <summary>
    /// Model View representation for the fetch restriction basic information context
    /// </summary>
    [DataContract]
    public class GetBasicMaterialModelView{
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

        /// <summary>
        /// String with material image's file name.
        /// </summary>
        /// <value>Gets/sets the image's file name.</value>
        [DataMember(Name="image")]
        public string imageFilename {get;set;}
    }
}