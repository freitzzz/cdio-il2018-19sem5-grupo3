using core.dto;
using System.Runtime.Serialization;

namespace core.modelview.material
{ /// <summary>
  /// Model View representation for the add finish in material
  /// </summary>
    [DataContract]
    public sealed class AddFinishModelView
    {
        /// <summary>
        /// Material's finish.
        /// </summary>
        [DataMember(Name = "finish")]
        public FinishDTO finish { get; set; }

    }
}