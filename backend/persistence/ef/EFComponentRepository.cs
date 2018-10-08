

using System.Collections.Generic;
using core.domain;
using core.dto;
using core.persistence;

namespace backend.persistence.ef
{
    public class EFComponentRepository : EFBaseRepository<Component, long, Product>, ComponentRepository
    {
        public EFComponentRepository(MyCContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Component> fetchComponentsByIDS(IEnumerable<ComponentDTO> componentsDTO)
        {
            List<long> componentsIDs = new List<long>();
            foreach(ComponentDTO componentDTO in componentsDTO){
                componentsIDs.Add(componentDTO.id);
            }
            return (from component in base.dbContext.Set<Component>()
                    where componentsIDs.Contains(component.id)
                    select component).ToList();
        }

        public Component find(Product entityID)
        {
            throw new System.NotImplementedException();
        }
    }
}