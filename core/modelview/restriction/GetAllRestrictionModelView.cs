using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.restriction{
    /// <summary>
    /// Model View representation for the fetch all restriction basic information context
    /// </summary>
    /// <typeparam name="GetBasicRestrictionModelView">Generic-Type param of the restriction basic information model view</typeparam>
    [CollectionDataContract]
    public sealed class GetAllRestrictionsModelView:List<GetBasicRestrictionModelView>{}
}