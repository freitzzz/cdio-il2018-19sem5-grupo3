using core.dto;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.material{
    /// <summary>
    /// Model View representation for the fetch restriction information context
    /// </summary>
    [DataContract]
    public sealed class GetMaterialModelView{
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
        public string imageFilename { get; set; }

        /// <summary>
        /// Material's list of available colors.
        /// </summary>
        /// <value>Gets/sets the value of the colors field.</value>
        [DataMember(Name="colors")]
        public List<ColorDTO> colors { get; set; }

        /// <summary>
        /// Material's list of available finishes.
        /// </summary>
        /// <value>Gets/sets the value of the finishes field.</value>
        [DataMember(Name="finishes")]
        public List<FinishDTO> finishes { get; set; }
    }
}