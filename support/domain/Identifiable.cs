namespace support.domain{
    /// <summary>
    /// Generic Interface for marking a domain entity identity
    /// </summary>
    /// <typeparam name="EID">Generic-Type which represents the entity identifier</typeparam>
    public interface Identifiable<EID>{
        /// <summary>
        /// Returns the entity identifier
        /// </summary>
        /// <returns>I with the entity identifier</returns>
        EID id();
        /// <summary>
        /// Checks if a certain entity identiy is the same as the current one
        /// </summary>
        /// <param name="comparingEntity">I with the entity identity being compared to the current one</param>
        /// <returns>bool if both entities identities are the same</returns>
        //#TODO See Version that .NET is using for C#, default implementations are available since C# 8
        bool sameAs(EID comparingEntity);
    }
}