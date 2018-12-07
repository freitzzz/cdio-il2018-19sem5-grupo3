using core.domain;

namespace core.persistence
{
    /// <summary>
    /// Interface representing the general behaviour of CustomizedProductSerialNumberRepository implementations.
    /// </summary>
    public interface CustomizedProductSerialNumberRepository
    {
        /// <summary>
        /// Retrieves the single instance of CustomizedProductSerialNumber stored in the database.
        /// </summary>
        /// <returns>Instance of CustomizedProductSerialNumber stored in the database.</returns>
        CustomizedProductSerialNumber findSerialNumber();

        /// <summary>
        /// Increments the value of the CustomizedProductSerialNumber and updates the entry.
        /// </summary>
        /// <returns>Updated instance of CustomizedProductSerialNumber.</returns>
        CustomizedProductSerialNumber increment();
    }
}