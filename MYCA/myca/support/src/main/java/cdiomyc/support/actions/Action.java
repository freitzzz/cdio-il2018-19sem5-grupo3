package cdiomyc.support.actions;

/**
 * Represents an action
 * @author freitas
 */
@FunctionalInterface
public interface Action {
    
    /**
     * Executes the action
     */
    public void execute();
    
}
