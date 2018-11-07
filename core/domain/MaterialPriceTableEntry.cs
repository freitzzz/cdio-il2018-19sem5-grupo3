using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Represents an Entry in a Material Price Table
    /// </summary>
    public class MaterialPriceTableEntry : PriceTableEntry<Material>, AggregateRoot<MaterialPriceTableEntry>
    {
        /// <summary>
        /// Constant that represents the message that occurs if a material is null
        /// </summary>
        private const string NULL_MATERIAL = "Material can't be null";

        /// <summary>
        /// Overrides entity property to allow lazy loading of the same
        /// </summary>
        /// <param name="_entity">entity type of the price table</param>
        public override Material entity { get => LazyLoader.Load(this, ref _entity); protected set => _entity = value; }

        /// <summary>
        /// Constructor used for injecting the LazyLoader
        /// </summary>
        /// <param name="lazyLoader">LazyLoader to be injected</param>
        private MaterialPriceTableEntry(ILazyLoader lazyLoader) : base(lazyLoader) { }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected MaterialPriceTableEntry() { }

        /// <summary>
        /// Builds a MaterialPriceTableEntry with a price, time period, and material
        /// </summary>
        /// <param name="price">Table Entry's price</param>
        /// <param name="timePeriod">Price's time period</param>
        /// <param name="material">Table Entry's material</param>
        public MaterialPriceTableEntry(Price price, TimePeriod timePeriod, Material material)
                                        : base(price, timePeriod)
        {
            checkMaterial(material);
            this.entity = material;
        }

        /// <summary>
        /// Checks if a material is valid for a TableEntry
        /// </summary>
        /// <param name="material">Material being checked</param>
        private void checkMaterial(Material material)
        {
            if (material == null)
            {
                throw new ArgumentException(NULL_MATERIAL);
            }
        }

        public MaterialPriceTableEntry id()
        {
            return this;
        }

        public bool sameAs(MaterialPriceTableEntry comparingEntity)
        {
            return this.Equals(comparingEntity);
        }
    }
}