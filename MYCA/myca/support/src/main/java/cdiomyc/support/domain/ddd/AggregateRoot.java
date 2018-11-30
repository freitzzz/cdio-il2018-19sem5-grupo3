package cdiomyc.support.domain.ddd;

/**
 * Generic Interface for marking an aggregate root
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 * @param <EID> Generic-Type of the aggregate root domain entity identifier
 */
public interface AggregateRoot<EID> extends DomainEntity<EID>{}
