
namespace support.domain.ddd{
    ///
    ///<summary>Markup interface for Value Objects</summary>
    ///
    public interface ValueObject{
        ///
        ///<summary>Represents the textual information of the Value Object</summary>
        ///<returns>String with the textual representation of the Value Object</returns>
        ///
        string ToString();

        ///
        ///<summary>Represents the Value Object hashcode</summary>
        ///<returns>Integer with the Value Object hashcode</returns>
        ///
        int GetHashCode();

        ///
        ///<summary>Checks if two Value Objects are equal</summary>
        ///<param name="obj">ValueObject with the Value Object being compared to the current one</param>
        ///<returns>boolean true if both value objects are equal, false if not</returns>
        ///
        bool Equals(object obj);
    }
}