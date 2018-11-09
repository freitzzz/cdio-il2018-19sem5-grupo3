using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Represents an Entry in a Material Price Table
    /// </summary>
    public class MaterialPriceTableEntry : PriceTableEntry<Material>
    {

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
        public MaterialPriceTableEntry(Material material, Price price, TimePeriod timePeriod)
                                        : base(material, price, timePeriod)
        {
            createEID();
        }

        protected override void createEID(){
            eId = entity.id() + String.Format("_{0}-{1}-{2}T{3}:{4}:{5}",
                                 timePeriod.startingDate.Year,
                                 timePeriod.startingDate.Month,
                                 timePeriod.startingDate.Day,
                                 timePeriod.startingDate.Hour,
                                 timePeriod.startingDate.Minute,
                                 timePeriod.startingDate.Second);
        }

        public override string id()
        {
            return eId;
        }

        public override bool sameAs(string comparingEntity)
        {
            return eId.Equals(comparingEntity);
        }
    }
}