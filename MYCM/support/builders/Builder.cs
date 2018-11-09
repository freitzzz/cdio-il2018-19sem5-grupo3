namespace support.builders{
    /// <summary>
    /// Generic Functional Interface which represents a builder
    /// </summary>
    /// <typeparam name="B">Generic-Type of the entity being built</typeparam>
    public interface Builder<B>{
        /// <summary>
        /// Builds the entity
        /// </summary>
        /// <returns>B with the built entity</returns>
        B build();
    }
}