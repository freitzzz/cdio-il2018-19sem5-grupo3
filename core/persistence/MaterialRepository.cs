using core.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence
{ 
    ///<summary>Interface that represents the repository functionalities for Material entities.</summary>
    ///<typeparam name = "Material">Generic-Type of the repository entity</typeparam>
    ///<typeparam name = "string">Generic-Type of the repository persistence ID</typeparam>
    public interface MaterialRepository : Repository<Material, long, string>, DataRepository<Material, long> {}
}