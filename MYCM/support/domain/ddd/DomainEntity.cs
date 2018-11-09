using support.domain;
namespace support.domain.ddd{
    /// <summary>
    /// Generic interface for marking domain entities
    /// </summary>
    /// <typeparam name="EID">Generic-Type of the entity identifier</typeparam>
    public interface DomainEntity<EID>:Identifiable<EID>{
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