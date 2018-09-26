namespace core.persistence{
    /// <summary>
    /// Interface that represents a "factory" for all core module repositories
    /// </summary>
    public interface RepositoryFactory{
        /// <summary>
        /// Creates a ProductRepository
        /// </summary>
        /// <returns>ProductRepository with the repository for product entities</returns>
        ProductRepository createProductRepository();
    }
}