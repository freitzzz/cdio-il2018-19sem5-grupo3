package cdiomyc.core.domain;

/**
 * Represents a User role
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public enum Role {
    
    /**
     * Represents the "Administrator" role
     */
    ADMINISTRATOR_MANAGER{@Override public String toString(){return "Administrator";}},
    
    /**
     * Represents the "Client" role
     */
    CLIENT{@Override public String toString(){return "Client";}},
    
    /**
     * Represents the "Content Manager" role
     */
    CONTENT_MANAGER{@Override public String toString(){return "Content Manager";}},
    
    /**
     * Represents the "Logistic Manager" role
     */
    LOGISTIC_MANAGER{@Override public String toString(){return "Logistic Manager";}}
    
}
