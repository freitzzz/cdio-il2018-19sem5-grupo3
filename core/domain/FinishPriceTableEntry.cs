using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    public class FinishPriceTableEntry : PriceTableEntry<Finish>, AggregateRoot<FinishPriceTableEntry>
    {
        /// <summary>
        /// Constant that represents the message that occurs if the Table Entry's finish is null
        /// </summary>
        private const string NULL_FINISH = "The finish can't be null";

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
        public FinishPriceTableEntry(Price price, TimePeriod timePeriod, Finish finish)
                : base(price, timePeriod)
        {
            checkFinish(finish);
            this.entity = finish;
        }

        /// <summary>
        /// Checks if the Table Entry's finish is valid
        /// </summary>
        /// <param name="finish">Finish being checked</param>
        private void checkFinish(Finish finish)
        {
            if (finish == null)
            {
                throw new ArgumentException(NULL_FINISH);
            }
        }

        public FinishPriceTableEntry id()
        {
            return this;
        }

        public bool sameAs(FinishPriceTableEntry comparingEntity)
        {
            return this.Equals(comparingEntity);
        }
    }
}