
/**
 * Represents a "create" event
 */
const CREATE="create";

/**
 * Represents a "render" event
 */
const RENDER="render";

/**
 * Represents an enum of the various event types watched on Watcher
 */
const WatcherEventsTypes={
    CREATE:CREATE,
    RENDER:RENDER,
    values:[CREATE,RENDER]
};

/**
 * Exports WatcherEventsTypes enum values
 */
export default WatcherEventsTypes;