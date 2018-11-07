using core.modelview.component;
using core.modelview.dimension;
using core.modelview.material;
using core.modelview.measurement;
using core.modelview.productcategory;
using core.modelview.slotdimensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the fetch product information context
    /// </summary>
    [DataContract]
    public sealed class GetProductModelView{
        /// <summary>
        /// Long with the product ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the product reference
        /// </summary>
        [DataMember(Name="reference")]
        public string reference{get;set;}

        /// <summary>
        /// String with the product designation
        /// </summary>
        [DataMember(Name="designation")]
        public string designation{get;set;}

        /// <summary>
        /// GetBasicMaterialModelView with the product category
        /// </summary>
        [DataMember(Name="category")]
        public GetBasicProductCategoryModelView category{get;set;}

        /// <summary>
        /// GetBasicMaterialModelView with the product material
        /// </summary>
        [DataMember(Name="material")]
        public GetAllMaterialsModelView materials{get;set;}

        /// <summary>
        /// ModelView containing Product's components' information.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember(EmitDefaultValue = false)]  //since this is optional, don't show null values
        public GetAllComponentsModelView components {get; set;}

        /// <summary>
        /// List of ModelViews containg the Product's measurements' information.
        /// </summary>
        /// <value>Get/set the list of ModelView.</value>
        [DataMember(Name="dimensions")]
        public List<GetMeasurementModelView> measurements {get; set;}

        /// <summary>
        /// ModelView containing Product's slot sizes.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember(EmitDefaultValue = false)]  //since this is optional, don't show null values
        public GetSlotDimensionsModelView slotSizes {get; set;}
    }
}