package cdiomyc.support.domain.ddd;

/**
 * Markup interface for Value Objects
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public interface ValueObject {
    /**
     * Returns the hashcode of the value object
     * @return Integer with the hash code of the value object
     */
    @Override
    int hashCode();
    
    /**
     * Checks if a value object is equal to the current one
     * @param otherValueObject ValueObject with the comparing value object
     * @return boolean true if both value objects are equal, false if not
     */
    @Override
    boolean equals(Object otherValueObject);
    
    /**
     * Returns the textual representation of the value object
     * @return String with the textual representation of the value object
     */
    @Override
    String toString();
}
