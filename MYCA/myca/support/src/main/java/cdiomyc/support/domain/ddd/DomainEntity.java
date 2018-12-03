package cdiomyc.support.domain.ddd;

import cdiomyc.support.domain.Identifiable;

/**
 * Generic Interface for marking domain entities
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <EID> Generic-Type of the domain entity identifier
 */
public interface DomainEntity<EID> extends Identifiable<EID>{
    /**
     * Returns the hashcode of the domain entity
     * @return Integer with the hash code of the domain entity
     */
    @Override
    int hashCode();
    
    /**
     * Checks if a domain entity is equal to the current one
     * @param otherDomainEntity DomainEntity with the comparing domain entity
     * @return boolean true if both domain entities are equal, false if not
     */
    @Override
    boolean equals(Object otherDomainEntity);
}
