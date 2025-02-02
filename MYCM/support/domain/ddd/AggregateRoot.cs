using support.domain.ddd;

namespace support.domain.ddd{
    /// <summary>
    /// Generic Interface for marking an entity which is the root of aggregated entities
    /// </summary>
    /// <typeparam name="EID">Generic-Type of the entity identifier</typeparam>
    public interface AggregateRoot<EID>:DomainEntity<EID>{}
}