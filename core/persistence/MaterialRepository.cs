using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

namespace core.persistence
{
    ///<summary>Interface that represents the repository functionalities for Material entities.</summary>
    ///<typeparam name = "Material">Generic-Type of the repository entity</typeparam>
    ///<typeparam name = "string">Generic-Type of the repository persistence ID</typeparam>
    public interface MaterialRepository : Repository<Material, long, string> {

        public abstract List<Finish> findFinishes(Material mat, List<FinishDTO> finishes);

        public abstract Finish findFinish(Material mat, long id);
    }
}