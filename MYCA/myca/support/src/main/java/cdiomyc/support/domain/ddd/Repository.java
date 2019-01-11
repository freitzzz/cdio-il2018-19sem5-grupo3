package cdiomyc.support.domain.ddd;

import cdiomyc.support.persistence.repositories.DataRepository;

/**
 * Generic Interface for marking aggregate root repositories
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <E> Generic-Type of the repository aggregate root
 * @param <ID> Generic-Type of the repository aggregate root persistence identifier
 * @param <EID> Generic-Type of the repository aggregate root identifier
 */
public interface Repository<E extends AggregateRoot<EID>,ID,EID> extends DataRepository<E,ID>{
    /**
     * Finds a domain entity by its identifier
     * @param domainEntityIdentifier EID with the domain entity identifier
     * @return E with the domain entity which is identified by a certain identifier
     */
    E findEID(EID domainEntityIdentifier);
}