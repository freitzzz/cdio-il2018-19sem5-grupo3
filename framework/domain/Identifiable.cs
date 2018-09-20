namespace framework.domain{
    /// <summary>
    /// Generic Interface for marking a domain entity identity
    /// </summary>
    /// <typeparam name="I">Generic-Type which represents the entity identifier</typeparam>
    public interface Identifiable<I>{
        /// <summary>
        /// Returns the entity identifier
        /// </summary>
        /// <returns>I with the entity identifier</returns>
        public I id();
        /// <summary>
        /// Checks if a certain entity is the same as the current one
        /// </summary>
        /// <param name="comparingEntity">I with the entity being compared to the current one</param>
        /// <returns>bool if both entities are the same</returns>
        public bool sameAs(I comparingEntity){return id().Equals(comparingEntity);}
    }
}