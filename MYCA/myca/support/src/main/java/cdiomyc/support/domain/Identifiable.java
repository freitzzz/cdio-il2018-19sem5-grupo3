package cdiomyc.support.domain;

/**
 * Generic interface for identifying domain entities
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <EID> Generic-Type of the domain entity identifier
 */
public interface Identifiable<EID> {
    /**
     * Returns the domain entity identifier
     * @return EID with the domain entity identifier
     */
    EID id();
    /**
     * Checks if a comparing domain entity identifier is the same as 
     * the current domain entity identifier
     * @param comparingIdentifier EID with the comparing domain entity identifier
     * @return boolean true if both identifiers are equal, false if not
     */
    default boolean sameAs(EID comparingIdentifier){return id().equals(comparingIdentifier);};
}
