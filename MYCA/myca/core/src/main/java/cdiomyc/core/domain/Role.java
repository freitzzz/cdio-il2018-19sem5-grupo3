package cdiomyc.core.domain;

/**
 * Represents a User role
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public enum Role {
    /**
     * Represents the "Client" role
     */
    CLIENT{@Override public String toString(){return "Client";}},
    /**
     * Represents the "Content Manager" role
     */
    CONTENT_MANAGER{@Override public String toString(){return "Content Manager";}}
}
