using core.modelview.component;
using core.modelview.dimension;
using core.modelview.material;
using core.modelview.measurement;
using core.modelview.productcategory;
using core.modelview.productmaterial;
using core.modelview.productslotwidths;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for retrieving Products' information.
    /// </summary>
    [DataContract]
    public sealed class GetProductModelView : GetBasicProductModelView
    {
        /// <summary>
        /// GetBasicMaterialModelView with the product category
        /// </summary>
        [DataMember(Name = "category", Order = 4)]
        public GetBasicProductCategoryModelView category { get; set; }

        /// <summary>
        /// GetBasicMaterialModelView with the product material
        /// </summary>
        [DataMember(Name = "materials", Order = 5)]
        public GetAllProductMaterialsModelView materials { get; set; }

        /// <summary>
        /// ModelView containing Product's components' information.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember(EmitDefaultValue = false, Order = 6)]  //since this is optional, don't show null values
        public GetAllComponentsModelView components { get; set; }

        /// <summary>
        /// List of ModelViews containg the Product's measurements' information.
        /// </summary>
        /// <value>Get/set the list of ModelView.</value>
        [DataMember(Name = "dimensions", Order = 7)]
        public GetAllMeasurementsModelView measurements { get; set; }

        /// <summary>
        /// ModelView containing Product's slot widths.
        /// </summary>
        /// <value>Gets/sets the ModelView.</value>
        [DataMember(EmitDefaultValue = false, Order = 8)]  //since this is optional, don't show null values
        public GetProductSlotWidthsModelView slotWidths { get; set; }
    }
}