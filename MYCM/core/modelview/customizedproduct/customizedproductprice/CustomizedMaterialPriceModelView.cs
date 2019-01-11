using System.Runtime.Serialization;
using core.modelview.color;
using core.modelview.price;

namespace core.modelview.customizedproduct.customizedproductprice
{
    /// <summary>
    /// ModelView to represent a priced customized material
    /// </summary>
    [DataContract]
    public class CustomizedMaterialPriceModelView
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
        /// Material's price
        /// </summary>
        /// <value>Gets/Sets the price</value>
        [DataMember(Name = "price")]
        public PriceModelView price { get; set; }

        /// <summary>
        /// CustomizedMaterials finish
        /// </summary>
        /// <value>Gets/Sets the finish</value>
        [DataMember(Name = "finish", EmitDefaultValue = false)]
        public FinishPriceModelView finish { get; set; }

        /// <summary>
        /// CustomizedMaterials color
        /// </summary>
        /// <value>Gets/Sets the color</value>
        [DataMember(Name = "color", EmitDefaultValue = false)]
        public GetColorModelView color { get; set; }
    }
}