using System.Runtime.Serialization;
using core.modelview.area;
using core.modelview.customizeddimensions;
using core.modelview.price;

namespace core.modelview.customizedproduct.customizedproductprice
{
    /// <summary>
    /// ModelView that represents a customized product with details about its price
    /// </summary>
    [DataContract]
    public class CustomizedProductPriceModelView
    {
        /// <summary>
        /// CustomizedProduct's PID
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "id")]
        public long customizedProductId { get; set; }

        /// <summary>
        /// Product's PID
        /// </summary>
        /// <value>Gets/Sets the instance of GetBasicProductModelView.</value>
        [DataMember(Name = "productId")]
        public long productId { get; set; }

        /// <summary>
        /// CustomizedProduct's designation
        /// </summary>
        /// <value>Gets/Sets the designation</value>
        [DataMember(EmitDefaultValue = false)] //since this value is optional, do not display null values
        public string designation { get; set; }

        /// <summary>
        /// CustomizedProduct's reference
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember(EmitDefaultValue = false)]  //if serial number is set, reference is null
        public string reference { get; set; }

        /// <summary>
        /// CustomizedProduct's serial number.
        /// </summary>
        /// <value>Gets/Sets the serial number</value>
        [DataMember(EmitDefaultValue = false)]  //if reference is set, serial number is null
        public string serialNumber { get; set; }

        /// <summary>
        /// CustomizedProduct's customized dimensions
        /// </summary>
        /// <value>Gets/Sets the customized dimensions</value>
        [DataMember(Name = "customizedDimensions")]
        public GetCustomizedDimensionsModelView customizedDimensions { get; set; }

        /// <summary>
        /// CustomizedProduct's priced customized material
        /// </summary>
        /// <value>Gets/Set the customized material</value>
        [DataMember(Name = "customizedMaterial")]
        public CustomizedMaterialPriceModelView customizedMaterial {get; set;}

        /// <summary>
        /// CustomizedProduct's total area
        /// </summary>
        /// <value>Gets/Sets the area</value>
        [DataMember(Name = "totalArea")]
        public AreaModelView totalArea {get; set;}

        /// <summary>
        /// CustomizedProduct's Price
        /// </summary>
        /// <value>Gets/Sets the price</value>
        [DataMember(Name = "price")]
        public PriceModelView price {get; set;}
    }
}