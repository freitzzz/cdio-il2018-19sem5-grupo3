using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    public class FinishPriceTableEntry : PriceTableEntry<Finish>, AggregateRoot<string>
    {

        /// <summary>
        /// Overrides entity property to allow lazy loading of the same
        /// </summary>
        /// <param name="_entity">entity type of the price table entry</param>
        public override Finish entity { get => LazyLoader.Load(this, ref _entity); protected set => _entity = value; }

        /// <summary>
        /// Constructor used for injecting a LazyLoader
        /// </summary>
        /// <param name="lazyLoader">LazyLoader to be injected</param>
        private FinishPriceTableEntry(ILazyLoader lazyLoader) : base(lazyLoader) { }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected FinishPriceTableEntry() { }

        /// <summary>
        /// Builds a FinishPriceTableEntry with a price, a time period and a finish
        /// </summary>
        /// <param name="price">Table Entry's price</param>
        /// <param name="timePeriod">Table Entry's time period</param>
        /// <param name="finish">Table Entry's finish</param>
        public FinishPriceTableEntry(Finish finish, Price price, TimePeriod timePeriod)
                : base(finish, price, timePeriod)
        {
        }

        protected override void createEID()
        {
            eId = entity.description + String.Format("_{0}-{1}-{2}T{3}:{4}:{5}",
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
            return this.id().Equals(comparingEntity);
        }
    }
}