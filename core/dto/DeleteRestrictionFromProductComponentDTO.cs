using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// Model View representation for the delete restriction from a product component context
    /// </summary>
    public sealed class DeleteRestrictionFromProductComponentDTO{
        /// <summary>
        /// Long with the product resource ID which restriction will be deleted from its component
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the component resource ID which restriction will be deleted from
        /// </summary>
        public long componentID{get;set;}

        /// <summary>
        /// Long with the restriction resource ID which will be deleted
        /// </summary>
        public long restrictionID{get;set;}
    }
}