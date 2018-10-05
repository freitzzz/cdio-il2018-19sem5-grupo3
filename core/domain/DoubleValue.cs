using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Class used for wrapping the primitive type double, so that lists of double can be persisted.
    /// </summary>
    public class DoubleValue : ValueObject
    {

        //*Should value correctness be done here or on the dimensions that use this? */

        /// <summary>
        /// Database identifier.
        /// </summary>
        /// <value></value>
        public long Id { get; protected set; }

        /// <summary>
        /// Current value.
        /// </summary>
        /// <value></value>
        public double value { get; protected set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected DoubleValue() { }

        public static DoubleValue valueOf(double value)
        {
            return new DoubleValue(value);
        }

        /// <summary>
        /// Constructs a new instance of DoubleValue with a given value.
        /// </summary>
        /// <param name="value"></param>
        private DoubleValue(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Implicitly converts an instance of DoubleValue to the primitive type double.
        /// </summary>
        /// <param name="obj">Instance of DoubleValue to be converted.</param>
        public static implicit operator double(DoubleValue obj)
        {
            return obj.value;
        }

        /// <summary>
        /// Implicitly converts an instance of the primitive type double to DoubleValue.
        /// </summary>
        /// <param name="value">double to be converted.</param>
        public static implicit operator DoubleValue(double value)
        {
            return DoubleValue.valueOf(value);
        }

        public override bool Equals(object obj){

            if (this == obj)
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if(obj.GetType().Equals(this.GetType())){
                DoubleValue other = (DoubleValue)obj;
                return this.value.Equals(other.value);
            }
            else if(obj.GetType().Equals(typeof(double))){
                return this.value.Equals((double)obj);
            }

            return false;
        }

        public override int GetHashCode(){
            return value.GetHashCode();
        }
    }
}