using System.Collections.Generic;
using System.Linq;
using core.domain;
using Microsoft.EntityFrameworkCore;
using support.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace backend.persistence.ef
{
    /// <summary>
    /// Abstract Repository with basic operations common among repositories.
    /// </summary>
    /// <typeparam name="E">Type of Entity to be used in the repository</typeparam>
    /// <typeparam name="ID">Type of database Entity identifier</typeparam>
    /// <typeparam name="EID">Type of domain Entity identifier</typeparam>
    public abstract class EFBaseRepository<E, ID, EID> : Repository<E, ID, EID> where E : Activatable, AggregateRoot<EID>
    {
        /// <summary>
        /// Current database context.
        /// </summary>
        protected readonly MyCContext dbContext;

        /// <summary>
        /// Constructs a new instance of EFBaseRepository with a given database context.
        /// </summary>
        /// <param name="dbContext"></param>
        protected EFBaseRepository(MyCContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual E find(ID entityPersistenceID)
        {
            E entity = dbContext.Set<E>().Find(entityPersistenceID);

            return entity != null && entity.activated ? entity : null;
        }

        public virtual E find(EID entityID)
        {
            //use the entity's sameAs() method since it might have extra comparison criteria
            return dbContext.Set<E>().Where(e => e.activated).Where(e => e.sameAs(entityID)).SingleOrDefault();
        }

        public virtual IEnumerable<E> findAll()
        {
            return dbContext.Set<E>().Where(e => e.activated).ToList();
        }

        public virtual E remove(E entity)
        {
            entity.deactivate();
            dbContext.SaveChanges();

            return entity;
        }

        public E save(E entity)
        {
            //*Avoid adding entities with the same business identifier
            //*A unique index could also work, but if the business identifier is a string,
            //*it's harder to ensure that field is case insensitive

            /*If there is an already persisted and deactivated entity 
            with a matching business identifier, then simply reactivate that entity*/

            E equalEntity = findDeactivatedEntityWithSameBusinessIdentifier(entity);

            if (equalEntity != null)
            {
                equalEntity.activate();

                return update(equalEntity);
            }

            if (find(entity.id()) != null)
            {
                return null;
            }

            dbContext.Set<E>().Add(entity);
            dbContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Retrieves a previously persisted but deactivated entity with a matching business identifier as the given entity.
        /// </summary>
        /// <param name="entity">Entity being checked.</param>
        /// <returns></returns>
        private E findDeactivatedEntityWithSameBusinessIdentifier(E entity)
        {
            return dbContext.Set<E>().Where(e => e.sameAs(entity.id())).Where(e => !e.activated).SingleOrDefault();
        }

        public virtual E update(E entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();

            return entity;
        }
    }
}