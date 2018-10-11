using backend.config;
using core.persistence;

namespace backend.persistence.ef{
    public sealed class EFRepositoryFactoryImpl : RepositoryFactory
    {/*
        public ComponentRepository createComponentRepository() {
            throw new System.NotImplementedException();
        }*/
        public CommercialCatalogueRepository createCommercialCatalogueRepository()
        {
            return new EFCommercialCatalogueRepository(BackendConfiguration.entityFrameworkContext);
        }

        /// <summary>
        /// Creates a MaterialRepository
        /// </summary>
        /// <returns>MaterialRepository with the repository for material entities</returns>
        public MaterialRepository createMaterialRepository()
        {
            return new EFMaterialRepository(BackendConfiguration.entityFrameworkContext);
        }

        /// <summary>
        /// Creates a ProductCategoryRepository
        /// </summary>
        /// <returns>ProductCategoryRepository with the repository for product category entities</returns>
        public ProductCategoryRepository createProductCategoryRepository()
        {
            return new EFProductCategoryRepository(BackendConfiguration.entityFrameworkContext);
        }

        // <summary>
        /// Creates a ProductRepository
        /// </summary>
        /// <returns>ProductRepository with the repository for product entities</returns>
        public ProductRepository createProductRepository()
        {
            return new EFProductRepository(BackendConfiguration.entityFrameworkContext);
        }
    }
}