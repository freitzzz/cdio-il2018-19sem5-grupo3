using System.Runtime.Serialization;
using core.modelview.color;
using core.modelview.finish;

namespace core.modelview.customizedmaterial
{
    /// <summary>
    /// Represents a CustomizedMaterial for GetCustomizedProductById ModelView
    /// </summary>
    [DataContract]
    public class GetCustomizedMaterialModelView
    {
        /// <summary>
        /// CustomizedMaterials PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "id")]
        public long customizedMaterialId { get; set; }

        /// <summary>
        /// Materials PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "materialId")]
        public long materialId { get; set; }


        /// <summary>
        /// CustomizedMaterials finish
        /// </summary>
        /// <value>Gets/Sets the finish</value>
        [DataMember(Name = "finish")]
        public GetFinishModelView finish { get; set; }

        /// <summary>
        /// CustomizedMaterials color
        /// </summary>
        /// <value>Gets/Sets the color</value>
        [DataMember(Name = "color")]
        public GetColorModelView color { get; set; }
    }
}