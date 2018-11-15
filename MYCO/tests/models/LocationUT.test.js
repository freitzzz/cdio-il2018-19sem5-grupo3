/**
 * Requires Location Data Model module
 */
const Location=require('../../models/Location');

/**
 * Represents a set of valid location properties
 */
const VALID_LOCATION_PROPERTIES={
    LATITUDE:-25.0352,
    LONGITUDE:60.0245,
};

describe('',()=>{
    test('',()=>{});
});

/**
 * Creates a valid Location Schema
 */
function createValidLocationSchema(){
    return new Location(createValidLocation());
}

/**
 * Creates a Location Schema
 * @param {Number} latitude Number with the location latitude
 * @param {Number} longitude Number with the location longitude
 */
function createLocationSchema(latitude,longitude){
    return new Location(createLocation(latitude,longitude));
}

/**
 * Creates a valid Location
 */
function createValidLocation(){
    return createLocation(VALID_LOCATION_PROPERTIES.LATITUDE,VALID_LOCATION_PROPERTIES.LONGITUDE);
}

/**
 * Creates a Location
 * @param {Number} latitude Number with the location latitude
 * @param {Number} longitude Number with the location longitude
 */
function createLocation(latitude,longitude){
    return Location.createLocation(latitude,longitude);
}

/**
 * Exports needed utility properties and functions
 */
module.exports={VALID_LOCATION_PROPERTIES,createValidLocationSchema,createLocationSchema,createValidLocation,createLocation};