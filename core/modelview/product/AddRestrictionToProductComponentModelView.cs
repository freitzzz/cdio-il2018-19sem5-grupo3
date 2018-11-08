using core.dto;
using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the add restriction to a product component context
    /// </summary>
    [DataContract]
    public sealed class AddRestrictionToProductComponentModelView{
        /// <summary>
        /// Long with the product resource ID which restriction will be applied to its component
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the component resource ID which restriction will be applied to
        /// </summary>
        public long componentID{get;set;}
        
        /// <summary>
        /// RestrictionDTO with the restriction information which will be applied to the product component
        /// </summary>
        public RestrictionDTO restriction{get;set;}
    }
}