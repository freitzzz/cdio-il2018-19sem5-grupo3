package cdiomyc.support.persistence.repositories;

/**
 * Generic Interface that represents all simple functionalities regarding data 
 * actions in repositories
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <E> Generic-Type of the repository entity
 * @param <ID> Generic-Type of the repository entity persistence identifier
 */
public interface DataRepository<E,ID> {
    /**
     * Method that add's a certain entity to the current entity repository
     * @param entity T with the Generic entity being added to the repository
     * @return T with the added entity
     */
    public abstract E save(E entity);
    
    /**
     * Method that updates a certain entity from the current entity repository
     * @param entity T with the Generic entity being updated of the repository
     * @return T with the updated entity
     */
    public abstract E update(E entity);
    /**
     * Method that removes a certain entity from the current entity repository
     * @param entity T with Generic entity being removed from the repository
     * @return T with the removed entity
     */
    public abstract E remove(E entity);
    /**
     * Method that finds a certain entity from the current entity repository 
     * using it's persistence identifier
     * @param id ID with the Generic entity persistence identifier
     * @return T with the entity which is represented by a certain persistence 
     * identifier
     */
    public abstract E find(ID id);
    /**
     * Method that finds all entities from the current entity repository
     * @return Iterable with all the entities from the current repository
     */
    public abstract Iterable<E> findAll();
}
