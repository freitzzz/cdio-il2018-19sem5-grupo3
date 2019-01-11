using backend.config;
using core.persistence;
using System.Threading;

namespace backend.persistence.ef
{
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
            return new EFCommercialCatalogueRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        /// <summary>
        /// Creates a CustomizedProductCollectionRepository
        /// </summary>
        /// <returns>CustomizedProductCollectionRepository with the repository for customized products collections</returns>
        public CustomizedProductCollectionRepository createCustomizedProductCollectionRepository()
        {
            return new EFCustomizedProductCollectionRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        /// <summary>
        /// Creates a CustomizedProductRepository
        /// </summary>
        /// <returns>CustomizedProductRepository with the repository for customized products</returns>
        public CustomizedProductRepository createCustomizedProductRepository()
        {
            return new EFCustomizedProductRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        public CustomizedProductSerialNumberRepository createCustomizedProductSerialNumberRepository()
        {
            return new EFCustomizedProductSerialNumberRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        public FinishPriceTableRepository createFinishPriceTableRepository()
        {
            return new EFFinishPriceTableRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        public MaterialPriceTableRepository createMaterialPriceTableRepository()
        {
            return new EFMaterialPriceTableRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        /// <summary>
        /// Creates a MaterialRepository
        /// </summary>
        /// <returns>MaterialRepository with the repository for material entities</returns>
        public MaterialRepository createMaterialRepository()
        {
            return new EFMaterialRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        /// <summary>
        /// Creates a ProductCategoryRepository
        /// </summary>
        /// <returns>ProductCategoryRepository with the repository for product category entities</returns>
        public ProductCategoryRepository createProductCategoryRepository()
        {
            return new EFProductCategoryRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }

        // <summary>
        /// Creates a ProductRepository
        /// </summary>
        /// <returns>ProductRepository with the repository for product entities</returns>
        public ProductRepository createProductRepository()
        {
            return new EFProductRepository(
                (MyCContext)BackendConfiguration.entityFrameworkContexts.get(Thread.CurrentThread.ManagedThreadId)
            );
        }
    }
}