using System.Runtime.Serialization;

namespace core.modelview.material
{
    /// <summary>
    /// ModelView that represents a material finish
    /// </summary>
    [DataContract]
    public class GetMaterialFinishModelView
    {
        /// <summary>
        /// Material's PID
        /// </summary>
        /// <value>Gets/Sets the PID</value>
        [DataMember(Name = "materialId")]
        public long materialId { get; set; }

        /// <summary>
        /// Material Finish's PID
        /// </summary>
        /// <value>Gets/Sets the PID</value>
        [DataMember(Name = "id")]
        public long id { get; set; }

        /// <summary>
        /// Finish's description
        /// </summary>
        /// <value>Gets/Sets the description</value>
        [DataMember(Name = "description")]
        public string description { get; set; }

        /// <summary>
        /// Finish's shininess
        /// </summary>
        /// <value>Gets/Sets the shininess</value>
        [DataMember(Name = "shininess")]
        public float shininess { get; set; }
    }
}