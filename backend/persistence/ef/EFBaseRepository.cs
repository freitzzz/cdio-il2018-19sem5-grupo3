using System.Collections.Generic;
using System.Linq;
using core.domain;
using Microsoft.EntityFrameworkCore;
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
    public abstract class EFBaseRepository<E, ID, EID> : Repository<E, ID, EID> where E : class, AggregateRoot<EID>
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

        public E find(ID entityPersistenceID)
        {
            return dbContext.Set<E>().Find(entityPersistenceID);
        }

        public E find(EID entityID)
        {
            //use the entity's sameAs() method since it might have extra comparison criteria
            return dbContext.Set<E>().Where(e => e.sameAs(entityID)).SingleOrDefault();
        }

        public IEnumerable<E> findAll()
        {
            return dbContext.Set<E>().ToList();
        }

        public E remove(E entity)
        {
            //TODO: change to soft/logical delete
            dbContext.Set<E>().Remove(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public E save(E entity)
        {
            //*Avoid adding entities with the same business identifier
            //*A unique index could also work, but if the business identifier is a string,
            //*it's harder to ensure that field is case insensitive
            if(find(entity.id()) != null){
                return null;
            }
            dbContext.Set<E>().Add(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public E update(E entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();

            return entity;
        }
    }
}