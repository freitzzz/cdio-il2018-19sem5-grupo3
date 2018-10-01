using support.domain.ddd;
using support.persistence.repositories;
using System;
namespace support.domain.ddd{
    /// <summary>
    /// Generic Interface for marking up repositories of domain aggregate roots
    /// Only Aggregate Roots can have a domain entity repository since 
    /// they are the ones who aggregate other entities
    /// </summary>
    /// <typeparam name="E">Generic-Type of the aggregate root which repository is held</typeparam>
    /// <typeparam name="ID">Generic-Type of the persistence entitiy identifier</typeparam>
    /// <typeparam name="EID">Generic-Type of the domain identifier</typeparam>
    //TODO: We have a problem on generic identifiers
    //C# doesn't have an "anonymous generic tag" so we can't do stuff like AggregateRoot<?>
    public interface Repository<E,ID,EID>:DataRepository<E,ID> where E:AggregateRoot<EID>{
        /// <summary>
        /// Finds an entity based on its identitifer
        /// </summary>
        /// <param name="entityID">EID with the entity identifier</param>
        /// <returns>E with the entity which is represend by a certain entity identifier</returns>
        E find(EID entityID);
    }
}