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

        /// <summary>
        /// Creates a MaterialRepository
        /// </summary>
        /// <returns>MaterialRepository with the repository for material entities</returns>
        MaterialRepository createMaterialRepository();
        
        /// <summary>
        /// Creates a ProductCategoryRepository
        /// </summary>
        /// <returns>ProductCategoryRepository with the repository for product category entities</returns>
        ProductCategoryRepository createProductCategoryRepository();

        /// <summary>
        /// Creates a CustomizedProductCollectionRepository
        /// </summary>
        /// <returns>CustomizedProductCollectionRepository with the repository for customized products collections</returns>
        CustomizedProductCollectionRepository createCustomizedProductCollectionRepository();

        
    }
}