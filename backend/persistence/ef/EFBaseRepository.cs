using System.Collections.Generic;
using System.Linq;
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
        protected readonly MyCContext dbContext;

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
            return dbContext.Set<E>().SingleOrDefault(e => e.id().Equals(entityID));
        }

        public IEnumerable<E> findAll()
        {
            return dbContext.Set<E>().ToList();
        }

        public E remove(E entity)
        {
            dbContext.Set<E>().Remove(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public E save(E entity)
        {
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