using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Join class representing the connection between Product and Measurement.
    /// </summary>
    public class ProductMeasurement
    {
        /// <summary>
        /// 
        /// </summary>
        private const string ERROR_NULL_PRODUCT = "The provided product can not be null.";

        /// <summary>
        /// 
        /// </summary>
        private const string ERROR_NULL_MEASUREMENT = "The provided measurement can not be null.";

        /// <summary>
        /// Product's database identifier (Foreign key, part of primary key).
        /// </summary>
        /// <value>Gets/protected sets the value of the Product database identifier.</value>
        public long productId { get; protected set; }

        /// <summary>
        /// Product to which the ProductMeasurement belongs.
        /// </summary>
        /// <value>Gets/protected sets the product.</value>
        private Product _product;
        public Product product { get => LazyLoader.Load(this, ref _product); protected set => _product = value; }

        /// <summary>
        /// Measurement's database identifier (Foreign key, part of primary key).
        /// </summary>
        /// <value>Get/protected sets the value of the Measurement database identifier.</value>
        public long measurementId { get; protected set; }

        /// <summary>
        /// Measurement contained in the ProductMeasurement.
        /// </summary>
        /// <value>Gets/protected sets the measurement.ro</value>
        private Measurement _measurement;
        public Measurement measurement { get => LazyLoader.Load(this, ref _measurement); protected set => _measurement = value; }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructror used by the framework for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">ILazyLoader being injected.</param>
        private ProductMeasurement(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected ProductMeasurement() { }

        /// <summary>
        /// Creates a new instance of ProductMeasurement with a given Product and Measurement.
        /// </summary>
        /// <param name="product">Instance of Product.</param>
        /// <param name="measurement">Instance of Product.</param>
        /// <exception cref="System.ArgumentException">If either of the arguments are null.</exception>
        public ProductMeasurement(Product product, Measurement measurement)
        {
            checkAttributes(product, measurement);
            this.product = product;
            this.measurement = measurement;
        }

        /// <summary>
        /// Checks if all arguments are not null.
        /// </summary>
        /// <exception cref="System.ArgumentException">If either of the arguments are null.</exception>
        private void checkAttributes(Product product, Measurement measurement)
        {
            if (product == null)
            {
                throw new ArgumentException(ERROR_NULL_PRODUCT);
            }
            if (measurement == null)
            {
                throw new ArgumentException(ERROR_NULL_MEASUREMENT);
            }
        }
    }
}