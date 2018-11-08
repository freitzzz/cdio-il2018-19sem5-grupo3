using backend.config;
using core.persistence;

namespace backend.persistence.ef{
    public sealed class EFRepositoryFactoryImpl : RepositoryFactory
    {/*
        public ComponentRepository createComponentRepository() {
            throw new System.NotImplementedException();
        }*/

                /// <summary>
        /// Creates a CommercialCatalogueRepository
        /// </summary>
        /// <returns>CommercialCatalogueRepository with the repository for CommercialCatalogue entities</returns>
        public CommercialCatalogueRepository createCommercialCatalogueRepository()
        {
            return new EFCommercialCatalogueRepository(BackendConfiguration.entityFrameworkContext);
        }

        /// <summary>
        /// Creates a CustomizedProductCollectionRepository
        /// </summary>
        /// <returns>CustomizedProductCollectionRepository with the repository for customized products collections</returns>
        public CustomizedProductCollectionRepository createCustomizedProductCollectionRepository()
        {
            return new EFCustomizedProductCollectionRepository(BackendConfiguration.entityFrameworkContext);
        }

        /// <summary>
        /// Creates a CustomizedProductRepository
        /// </summary>
        /// <returns>CustomizedProductRepository with the repository for customized products</returns>
        public CustomizedProductRepository createCustomizedProductRepository()
        {
            return new EFCustomizedProductRepository(BackendConfiguration.entityFrameworkContext);
        }

        public FinishPriceTableRepository createFinishPriceTableRepository()
        {
            return new EFFinishPriceTableRepository(BackendConfiguration.entityFrameworkContext);
        }

        public MaterialPriceTableRepository createMaterialPriceTableRepository()
        {
            return new EFMaterialPriceTableRepository(BackendConfiguration.entityFrameworkContext);
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