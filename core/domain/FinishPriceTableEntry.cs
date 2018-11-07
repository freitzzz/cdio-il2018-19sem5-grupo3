using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.utils;

namespace core.domain
{
    public class FinishPriceTableEntry : PriceTableEntry
    {
        /// <summary>
        /// Constant that represents the message that occurs if the Table Entry's finish is null
        /// </summary>
        private const string NULL_FINISH = "The finish can't be null";

        /// <summary>
        /// Table Entry's finish
        /// </summary>
        private Finish _finish; //!private field used for lazy loading, do not use this for storing or fetching data
        public Finish finish { get => LazyLoader.Load(this, ref _finish); protected set => _finish = value; }

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
            this.finish = finish;
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

        public override int GetHashCode()
        {
            return base.GetHashCode() + finish.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }

            FinishPriceTableEntry other = (FinishPriceTableEntry)obj;

            return this.finish.Equals(other.finish);
        }

        public override string ToString()
        {
            return String.Format("{0}\nFinish:{1}",
                                base.ToString(), finish.description);
        }
    }
}