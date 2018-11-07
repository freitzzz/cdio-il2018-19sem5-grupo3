using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain;
using support.domain.ddd;
using support.utils;

namespace core.domain
{
    //TODO Should we wrap the price, time period and type of price table entry in order to created an AggregateRoot EID?
    /// <summary>
    /// Represents a generic price table entry
    /// </summary>
    /// <typeparam name="string">EID of the entity that belongs to it's respective price table</typeparam>
    public abstract class PriceTableEntry<T> : Activatable
    {
        /// <summary>
        /// Constant that represents the message that occurs if the price is null
        /// </summary>
        private const string NULL_PRICE = "Price can't be null";

        /// <summary>
        /// Constant that represents the message that occurs if the time period is null
        /// </summary>
        private const string NULL_TIME_PERIOD = "Time Period can't be null";

        /// <summary>
        /// PID of the Entry
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        public long Id { get; internal set; }

        /// <summary>
        /// Entity Type of the table entry
        /// </summary>
        protected T _entity;  //!private field used for lazy loading, do not use this for storing or fetching data
        public virtual T entity { get; protected set; }

        /// <summary>
        /// The entry's price
        /// </summary>
        private Price _price;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Price price { get => LazyLoader.Load(this, ref _price); protected set => _price = value; }

        /// <summary>
        /// Time Period for which the price is active
        /// </summary>
        private TimePeriod _timePeriod;
        public TimePeriod timePeriod { get => LazyLoader.Load(this, ref _timePeriod); protected set => _timePeriod = value; }

        /// <summary>
        /// Injected LazyLoader
        /// </summary>
        /// <value>Gets/Sets the LazyLoader</value>
        [NotMapped]
        protected ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting the LazyLoader
        /// </summary>
        /// <param name="lazyLoader">LazyLoader to be injected</param>
        protected PriceTableEntry(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected PriceTableEntry() { }

        /// <summary>
        /// Super constructor receiving a price and a time period
        /// </summary>
        /// <param name="price">Entry's price</param>
        /// <param name="timePeriod">Entry's time period</param>
        protected PriceTableEntry(Price price, TimePeriod timePeriod)
        {
            checkPriceAndTimePeriod(price, timePeriod);
            this.price = price;
            this.timePeriod = timePeriod;
        }

        /// <summary>
        /// Checks if a table entry's price and time period are valid
        /// </summary>
        /// <param name="price">Price to be checked</param>
        /// <param name="timePeriod">TimePeriod to be checked</param>
        private void checkPriceAndTimePeriod(Price price, TimePeriod timePeriod)
        {
            if (price == null)
            {
                throw new ArgumentException(NULL_PRICE);
            }

            if (timePeriod == null)
            {
                throw new ArgumentException(NULL_TIME_PERIOD);
            }
        }

        public override int GetHashCode()
        {
            return price.GetHashCode() + timePeriod.GetHashCode() + entity.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (!obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            PriceTableEntry<T> other = (PriceTableEntry<T>)obj;

            if (!this.price.Equals(other.price))
            {
                return false;
            }

            if (!this.timePeriod.Equals(other.timePeriod))
            {
                return false;
            }

            return this.entity.Equals(other.entity);
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}",
                                price.ToString(), timePeriod.ToString(), entity.ToString());
        }
    }
}