using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
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
    public abstract class PriceTableEntry<T> : Activatable, AggregateRoot<string>
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
        /// Constant that represents the message that occurs if the entity is null
        /// </summary>
        private const string NULL_ENTITY = "Entity can't be null";

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

        public string eId { get; protected set; }

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
        protected PriceTableEntry(T entity, Price price, TimePeriod timePeriod)
        {
            checkEntityAndPriceAndTimePeriod(entity, price, timePeriod);
            this.price = price;
            this.timePeriod = timePeriod;
            this.entity = entity;
        }

        /// <summary>
        /// Checks if a table entry's price and time period are valid
        /// </summary>
        /// <param name="price">Price to be checked</param>
        /// <param name="timePeriod">TimePeriod to be checked</param>
        private void checkEntityAndPriceAndTimePeriod(T entity, Price price, TimePeriod timePeriod)
        {
            checkEntity(entity);
            checkPrice(price);
            checkTimePeriod(timePeriod);
        }

        /// <summary>
        /// Checks if a given entity is valid
        /// </summary>
        /// <param name="entity">entity being checked</param>
        private void checkEntity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(NULL_ENTITY);
            }
        }

        /// <summary>
        /// Checks if a given price is valid
        /// </summary>
        /// <param name="price">price being checked</param>
        private void checkPrice(Price price)
        {
            if (price == null)
            {
                throw new ArgumentException(NULL_PRICE);
            }
        }

        /// <summary>
        /// Checks if a given time period is valid
        /// </summary>
        /// <param name="timePeriod">time period being checked</param>
        private void checkTimePeriod(TimePeriod timePeriod)
        {
            if (timePeriod == null)
            {
                throw new ArgumentException(NULL_TIME_PERIOD);
            }
        }

        /// <summary>
        /// Changes the price of a price table entry
        /// </summary>
        /// <param name="newPrice">new price</param>
        public void changePrice(Price newPrice){
            checkPrice(newPrice);
            this.price = newPrice;
        }

        /// <summary>
        /// Changes the time period of a price table entry
        /// </summary>
        /// <param name="newStartingDate">new time period</param>
        public void changeTimePeriod(TimePeriod newTimePeriod){
            checkTimePeriod(newTimePeriod);
            this.timePeriod = newTimePeriod;
        }

        /// <summary>
        /// Creates an EID for each concrete PriceTableEntry
        /// </summary>
        protected abstract void createEID();

        public abstract string id();
        public abstract bool sameAs(string comparingEntity);

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