using core.dto;
using System.Runtime.Serialization;

namespace core.modelview.material
{ /// <summary>
  /// Model View representation for the add color in material
  /// </summary>
    [DataContract]
    public sealed class AddColorModelView
    {
        /// <summary>
        /// Material's color.
        /// </summary>
        [DataMember(Name = "color")]
        public ColorDTO color { get; set; }

    }
}