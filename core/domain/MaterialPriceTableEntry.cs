using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Represents an Entry in a Material Price Table
    /// </summary>
    public class MaterialPriceTableEntry : PriceTableEntry
    {
        /// <summary>
        /// Constant that represents the message that occurs if a material is null
        /// </summary>
        private const string NULL_MATERIAL = "Material can't be null";

        /// <summary>
        /// Table Entry's Material
        /// </summary>
        private Material _material; //!private field used for lazy loading, do not use this for storing or fetching data
        public Material material { get => LazyLoader.Load(this, ref _material); protected set => _material = value; }

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
            this.material = material;
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

        public override int GetHashCode()
        {
            return base.GetHashCode() + material.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }

            MaterialPriceTableEntry other = (MaterialPriceTableEntry)obj;

            return this.material.Equals(other.material);
        }

        public override string ToString()
        {
            return String.Format("{0}\nMaterial:{1}",
                                base.ToString(), material.designation);
        }
    }
}