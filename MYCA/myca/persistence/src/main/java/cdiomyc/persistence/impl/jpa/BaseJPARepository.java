package cdiomyc.persistence.impl.jpa;

import cdiomyc.support.persistence.repositories.DataRepository;
import java.io.Serializable;
import java.lang.reflect.ParameterizedType;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.EntityTransaction;
import javax.persistence.Persistence;
import javax.persistence.PersistenceUnit;

/**
 * Generic Abstract class that implements simple data repository functionalities 
 * using JPA
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <T> Generic-Type of the repository entity being implemented
 * @param <ID> Generic-Type of the repository entity persistence identifier
 */
public abstract class BaseJPARepository<T,ID extends Serializable> implements DataRepository<T,ID>{
    /**
     * Constant that represents the message that occurres whenever an entity is 
     * classified as invalid for the current repository state
     */
    private static final String INVALID_ENTITY_MESSAGE="The entity is invalid";
    /**
     * Static EntityManagerFactory that manages all entity managers injected 
     * by persistence unit
     */
    @PersistenceUnit
    private static EntityManagerFactory entityManagerFactory;
    /**
     * EntityManager with the entity manager that manages the current repository 
     * entity
     */
    private EntityManager entityManager;
    /**
     * Class with the current repository entity class
     */
    private final Class<T> entityClass;
    /**
     * String with the name of the persistence unit that holds it's managed 
     * entities
     */
    private final String entityPersistenceUnitName;
    /**
     * Builds a new BaseJPARepository with the entity persistence unit name 
     * allowing simple data repository functionalities using JPA
     * @param persistenceUnitName String with the repository entity 
     * persistence unit name
     */
    public BaseJPARepository(String persistenceUnitName){
        this.entityPersistenceUnitName=persistenceUnitName;
        this.entityClass=disassembleEntityGenericClass();
        entityManagerFactory=getEntityManagerFactory();
    }
    /**
     * Method that add's a certain entity to the current entity repository
     * @param entity T with the Generic entity being added to the repository
     * @return T with the added entity, null if an error occurred
     */
    @Override
    public T save(T entity) {
        if(entity==null)throw new IllegalArgumentException(INVALID_ENTITY_MESSAGE);
        EntityManager repositoryEntityManager=getEntityManager();
        EntityTransaction entityTransaction=repositoryEntityManager.getTransaction();
        entityTransaction.begin();
        repositoryEntityManager.persist(entity);
        entityTransaction.commit();
        repositoryEntityManager.close();
        return entity;
    }
    /**
     * Method that removes a certain entity from the current entity repository
     * @param entity T with Generic entity being removed from the repository
     * @return T with the removed entity, null if an error occurred
     */
    @Override
    public T remove(T entity) {
        if(entity==null)throw new IllegalArgumentException(INVALID_ENTITY_MESSAGE);
        EntityManager repositoryEntityManager=getEntityManager();
        EntityTransaction entityTransaction=repositoryEntityManager.getTransaction();
        entityTransaction.begin();
        repositoryEntityManager.remove(entity);
        entityTransaction.commit();
        repositoryEntityManager.close();
        return entity;
    }
    /**
     * Method that updates a certain entity from the current entity repository
     * @param entity T with the Generic entity being updated of the repository
     * @return T with the updated entity, null if an error occurred
     */
    @Override
    public T update(T entity) {
        if(entity==null)throw new IllegalArgumentException(INVALID_ENTITY_MESSAGE);
        EntityManager repositoryEntityManager=getEntityManager();
        EntityTransaction entityTransaction=repositoryEntityManager.getTransaction();
        entityTransaction.begin();
        repositoryEntityManager.merge(entity);
        entityTransaction.commit();
        repositoryEntityManager.close();
        return entity;
    }
    /**
     * Method that finds a certain entity from the current entity repository 
     * using it's persistence identifier
     * @param id ID with the Generic entity persistence identifier
     * @return T with the entity which is represented by a certain persistence 
     * identifier
     */
    @Override
    public T find(ID id) {
        return getEntityManager().find(entityClass,id);
    }
    /**
     * Method that finds all entities from the current entity repository
     * @return Iterable with all the entities from the current repository
     */
    @Override
    public Iterable<T> findAll(){
        return getEntityManager()
                .createQuery("SELECT T FROM "
                        .concat(entityClass.getSimpleName())
                        .concat(" T")).getResultList();
    }
    /**
     * Method that returns the current repository entity manager
     * @return EntityManager with the current repository entity manager
     */
    protected EntityManager getEntityManager(){
        if(entityManager==null||!entityManager.isOpen()){
            entityManager=entityManagerFactory.createEntityManager();
        }
        return entityManager;
    }
    /**
     * Method that returns the current repository entity manager factory
     * @return EntityManagerFactory with the current repository entity manager 
     * factory
     */
    private EntityManagerFactory getEntityManagerFactory(){
        if(entityManagerFactory==null){
            entityManagerFactory=Persistence.createEntityManagerFactory(entityPersistenceUnitName);
        }
        return entityManagerFactory;
    }
    /**
     * Method that disassembles the generic entity repository class in order to 
     * retrieve the class of the entity
     * @return Class with the repository entity class
     */
    private Class<T> disassembleEntityGenericClass(){
        return (Class<T>)((ParameterizedType)getClass().getGenericSuperclass())
                .getActualTypeArguments()[0];
    }
}
