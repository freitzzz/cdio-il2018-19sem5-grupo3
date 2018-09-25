using support.domain.ddd;
using System;
namespace support.domain.ddd{
    /// <summary>
    /// Generic Interface for marking up repositories of domain aggregate roots
    /// Only Aggregate Roots can have a domain entity repository since 
    /// they are the ones who aggregate other entities
    /// </summary>
    /// <typeparam name="E">Generic-Type of the aggregate root which repository is held</typeparam>
    /// <typeparam name="I">Generic-Type of the domain entitiy identifier</typeparam>
    /// <typeparam name="ID">Generic-Type of the entity persistence identifier</typeparam>
    //TODO: We have a problem on generic identifiers
    //C# doesn't have an "anonymous generic tag" so we can't do stuff like AggregateRoot<?>
    public interface Repository<E,ID> where E:AggregateRoot<object>,ID{
        /// <summary>
        /// Saves an entity on the repository
        /// </summary>
        /// <param name="entity">E with the entity being saved on the repository</param>
        /// <returns>E with the saved entity</returns>
        E save(E entity);

        /// <summary>
        /// Updates the state of an entity on the repository
        /// </summary>
        /// <param name="entity">E with the entity being updated on the repository</param>
        /// <returns>E with the updated entity</returns>
        E update(E entity);
        
        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">E with the entity being removed from the repository</param>
        /// <returns>E with the removed entity</returns>
        E remove(E entity);
        
        /// <summary>
        /// Finds an entity based on persistence identifier
        /// </summary>
        /// <param name="entityPersistenceID">ID with the entity persistence identifier</param>
        /// <returns>E with the entity which is represented by a certain persistence identifier</returns>
        E find(ID entityPersistenceID);
    }
}