using System.Collections.Generic;

namespace support.persistence.repositories{
    /// <summary>
    /// Generic Interface for CRUD functionalities on a data repository
    /// </summary>
    /// <typeparam name="E">Generic-Type of the repository entity</typeparam>
    /// <typeparam name="ID">Generic-Type of the repository entity persistence identity</typeparam>
    public interface DataRepository<E,ID>{
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
        /// <summary>
        /// Finds all entities of a repository
        /// </summary>
        /// <returns>IEnumerable with all of the entities of a repository</returns>
        IEnumerable<E> findAll();
    }
}