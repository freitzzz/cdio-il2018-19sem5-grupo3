using System;
using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Class representing the CustomizedProduct's 
    /// </summary>
    /// <typeparam name="string"></typeparam>
    public class CustomizedProductSerialNumber : AggregateRoot<string>
    {
        /// <summary>
        /// Serial number's persistence identifier.
        /// </summary>
        /// <value>Gets/protected sets the persistence identifier.</value>
        public long Id { get; protected set; }

        /// <summary>
        /// Serial number's value.
        /// </summary>
        /// <value>Gets/protected sets the serial number.</value>
        public string serialNumber { get; protected set; }

        /// <summary>
        /// Creates a new instance of CustomizedProductSerialNumber.
        /// </summary>
        public CustomizedProductSerialNumber()
        {
            ulong initialSerialNumberValue = 0;
            this.serialNumber = initialSerialNumberValue.ToString();
        }

        /// <summary>
        /// Increments the serial number's value.
        /// </summary>
        public void incrementSerialNumber()
        {
            ulong serialNumberAsUnsignedLong = Convert.ToUInt64(this.serialNumber);
            serialNumberAsUnsignedLong++;
            this.serialNumber = serialNumberAsUnsignedLong.ToString();
        }

        public string id()
        {
            return serialNumber;
        }

        public bool sameAs(string comparingEntity)
        {
            return this.serialNumber.Equals(comparingEntity);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            CustomizedProductSerialNumber other = (CustomizedProductSerialNumber)obj;

            return this.serialNumber.Equals(other.serialNumber);
        }

        public override int GetHashCode()
        {
            int hash = 47;
            hash = hash * 53 + serialNumber.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("Serial number: {0}", serialNumber);
        }
    }
}