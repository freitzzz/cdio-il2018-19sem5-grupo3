using System.Runtime.Serialization;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for retrieving Products' basic information.
    /// </summary>
    [DataContract]
    public class GetBasicProductModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        [DataMember(Name = "id", Order = 0)]
        public long id { get; set; }

        /// <summary>
        /// Product's reference.
        /// </summary>
        /// <value>Gets/sets the Product's reference.</value>
        [DataMember(Name = "reference", Order = 1)]
        public string reference { get; set; }

        /// <summary>
        /// Product's designation.
        /// </summary>
        /// <value>Gets/sets the Product's designation.</value>
        [DataMember(Name = "designation", Order = 2)]
        public string designation { get; set; }

        /// <summary>
        /// Product's model's filename.
        /// </summary>
        /// <value>Gets/sets the model's filename.</value>
        [DataMember(Name = "model", Order = 3)]
        public string modelFilename { get; set; }
    }
}