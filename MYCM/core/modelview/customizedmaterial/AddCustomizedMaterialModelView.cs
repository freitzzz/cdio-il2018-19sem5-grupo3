using System.Runtime.Serialization;
using core.dto;
using core.modelview.material;

namespace core.modelview.customizedmaterial
{
    /// <summary>
    /// Class representing the ModelView used for adding 
    /// </summary>
    [DataContract]
    public class AddCustomizedMaterialModelView
    {
        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the Material's persistence identifier.</value>
        [DataMember]
        public long materialId { get; set; }

        /// <summary>
        /// FinishDTO containing the CustomizedMaterial's Finish's information.
        /// </summary>
        /// <value>Gets/Sets the instance of FinishDTO.</value>
        [DataMember]
        public FinishDTO finish { get; set; }

        /// <summary>
        /// ColorDTO containing the CustomizedMaterial's Color's information.
        /// </summary>
        /// <value>Gets/Sets the instance of ColorDTO.</value>
        [DataMember]
        public ColorDTO color { get; set; }
    }
}