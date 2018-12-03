package cdiomyc.core.application.grants;

import cdiomyc.support.domain.ddd.AggregateRoot;
import cdiomyc.support.domain.ddd.DomainEntity;

/**
 * Service for granting CRUD operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class CRUDGrants {
    
    /**
     * Grants that a create operation was successful
     * @param createdEntity AggregateRoot with the created entity
     */
    public static void grantCreateWasSuccessful(AggregateRoot<?> createdEntity){
        if(createdEntity==null)
            throw new InternalError("A problem occurd during the create operation");
    }
    
    /**
     * Grants that a find operation was successful
     * @param foundEntity DomainEntity with the found entity
     */
    public static void grantFindWasSuccessful(DomainEntity<?> foundEntity){
        if(foundEntity==null)
            throw new InternalError("A problem occurd during the find operation");
    }
    
    /**
     * Grants that a update operation was successful
     * @param updatedEntity AggregateRoot with the updated entity
     */
    public static void grantUpdateWasSuccessful(AggregateRoot<?> updatedEntity){
        if(updatedEntity==null)
            throw new InternalError("A problem occurd during the update operation");
    }
    
    /**
     * Grants that a delete operation was successful
     * @param deletedEntity AggregateRoot with the deleted entity
     */
    public static void grantDeleteWasSuccessful(AggregateRoot<?> deletedEntity){
        if(deletedEntity==null)
            throw new InternalError("A problem occurd during the delete operation");
    }
}
