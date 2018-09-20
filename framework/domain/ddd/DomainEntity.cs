using framework.domain.Identifiable;
namespace framework.domain.ddd{
    /// <summary>
    /// Generic interface for marking domain entities
    /// </summary>
    /// <typeparam name="I">Generic-Type of the entity identifier</typeparam>
    public interface DomainEntity<I>:Identifiable<I>{
        ///<summary>Represents the entity hashcode</summary>
        ///<returns>Integer with the entity hashcode</returns>
        ///
        int GetHashCode();
        
        ///
        ///<summary>Checks if two entities are equal</summary>
        ///<param name="obj">DomainEntity with the entity being compared to the current one</param>
        ///<returns>boolean true if both entities are equal, false if not</returns>
        ///
        bool Equals(object obj);
    }
}