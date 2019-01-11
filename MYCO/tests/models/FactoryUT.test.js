/**
 * Requires Jest testing module framework
 */
require('jest')

/**
 * Requires Location Unit Tests model
 */
const LocationUT=require('./LocationUT.test');

/**
 * Requires Factory Data Model
 */
const Factory=require('../../models/Factory');

//Unit tests for Factory data model

/**
 * Represents a set of valid factory properties
 */
const VALID_FACTORY_PROPERTIES={
    VALID_REFERENCE:'Jevil',
    VALID_DESIGNATION:'You dont wanna mess with a devil Jest',
    VALID_LOCATION:LocationUT.VALID_LOCATION_PROPERTIES,
};

/**
 * Tests the creations of a Factory
 */
describe('create',()=>{
    describe('invalid-creations',()=>{
        describe('invalid-references-creations',()=>{
            test('cant-create-factory-with-null-reference',ensureCantCreateFactoryWithNullReference);
            test('cant-create-factory-with-empty-reference',ensureCantCreateFactoryWithEmptyReference);
        });
        describe('invalid-designations-creations',()=>{
            test('cant-create-factory-with-null-designation',ensureCantCreateFactoryWithNullDesignation);
            test('cant-create-factory-with-empty-designation',ensureCantCreateFactoryWithEmptyDesignation);
        });
        describe('invalid-locations-creations',()=>{
            test('cant-create-factory-with-null-location',ensureCantCreateFactoryWithNullLocation);
            test('cant-create-factory-with-empty-location',ensureCantCreateFactoryWithEmptyLocation);
        });
        describe('invalid-cities-creations',()=>{
            test('cant-create-factory-with-null-city',ensureCantCreateFactoryWithNullCity);
            test('cant-create-factory-with-empty-city',ensureCantCreateFactoryWithEmptyCity);
        });
    });
});

/**
 * Tests the variety of changes of a Factory
 */
describe('changes',()=>{
    describe('invalid-changes',()=>{
        describe('invalid-reference-changes',()=>{
            test('cant-change-factory-reference-to-null-reference',ensureCantChangeFactoryReferenceToNullReference);
            test('cant-change-factory-reference-to-empty-reference',ensureCantChangeFactoryReferenceToEmptyReference);
        });
        describe('invalid-designation-changes',()=>{
            test('cant-change-factory-designation-to-null-designation',ensureCantChangeFactoryDesignationToNullDesignation);
            test('cant-change-factory-designation-to-empty-designation',ensureCantChangeFactoryDesignationToEmptyDesignation);
        });
        describe('invalid-location-changes',()=>{
            test('cant-change-factory-location-latitude-to-null-location-latitude',ensureCantChangeFactoryLocationLatitudeToNullLocationLatitude);
            test('cant-change-factory-location-latitude-to-empty-location-latitude',ensureCantChangeFactoryLocationLatitudeToEmptyLocationLatitude);
            test('cant-change-factory-location-longitude-to-null-location-longitude',ensureCantChangeFactoryLocationLongitudeToNullLocationLongitude);
            test('cant-change-factory-location-longitude-to-empty-location-longitude',ensureCantChangeFactoryLocationLongitudeToEmptyLocationLongitude);
        });
    });
});

//CREATIONS

/**
 * Ensures that its not possible to create a factory with a null reference
 */
function ensureCantCreateFactoryWithNullReference(){
    ensureSchemaIsInvalid(createFactorySchema(null
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with an empty reference
 */
function ensureCantCreateFactoryWithEmptyReference(){
    ensureSchemaIsInvalid(createFactorySchema(""
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with a null designation
 */
function ensureCantCreateFactoryWithNullDesignation(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,null
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with an empty designation
 */
function ensureCantCreateFactoryWithEmptyDesignation(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,""
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with a null location
 */
function ensureCantCreateFactoryWithNullLocation(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,{latitude:null,longitude:null})
    );
}

/**
 * Ensures that its not possible to create a factory with an empty location
 */
function ensureCantCreateFactoryWithEmptyLocation(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,{})
    );
}

/**
 * Ensures that its not possible to create a factory with a null city
 */
function ensureCantCreateFactoryWithNullCity(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation()
        ,null)
    );
}

/**
 * Ensures that its not possible to create a factory with an empty city
 */
function ensureCantCreateFactoryWithEmptyCity(){
    ensureSchemaIsInvalid(createFactorySchema(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation()
        ,{})
    );
}

//CHANGES

/**
 * Ensures that its not possible to change a factory reference to a null reference
 */
function ensureCantChangeFactoryReferenceToNullReference(){
    let factoryChange=()=>{createValidFactorySchema().changeReference(null)};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory reference to an empty reference
 */
function ensureCantChangeFactoryReferenceToEmptyReference(){
    let factoryChange=()=>{createValidFactorySchema().changeReference("")};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory designation to a null designation
 */
function ensureCantChangeFactoryDesignationToNullDesignation(){
    let factoryChange=()=>{createValidFactorySchema().changeDesignation(null)};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory designation to an empty designation
 */
function ensureCantChangeFactoryDesignationToEmptyDesignation(){
    let factoryChange=()=>{createValidFactorySchema().changeDesignation("")};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory location latitude to a null location latitude
 */
function ensureCantChangeFactoryLocationLatitudeToNullLocationLatitude(){
    let factoryChange=()=>{createValidFactorySchema().changeLatitude(null)};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory location latitude to an empty location latitude
 */
function ensureCantChangeFactoryLocationLatitudeToEmptyLocationLatitude(){
    let factoryChange=()=>{createValidFactorySchema().changeLatitude({})};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory location longitude to a null location longitude
 */
function ensureCantChangeFactoryLocationLongitudeToNullLocationLongitude(){
    let factoryChange=()=>{createValidFactorySchema().changeLongitude(null)};
    expect(factoryChange).toThrow();
}

/**
 * Ensures that its not possible to change a factory location longitude to an empty location longitude
 */
function ensureCantChangeFactoryLocationLongitudeToEmptyLocationLongitude(){
    let factoryChange=()=>{createValidFactorySchema().changeLongitude({})};
    expect(factoryChange).toThrow();
}

//TODO: ENSURE CANT CHANGE TO INVALID LOCATION LATITUDE/LONGITUDE VALUES (RESPONSIBILITY OF LOCATIONUT?)

//UTILITIES

/**
 * Ensures that a schema is not valid
 * @param {Schema} schema Schema with the schema being ensured that its not valid
 */
function ensureSchemaIsInvalid(schema){
    schema.validate((_error)=>{
        expect(_error).not.toBeUndefined();
    });
}

/**
 * Creates a valid Factory schema
 */
function createValidFactorySchema(){
    return new Factory(createValidFactory());
}

/**
 * Creates a new Factory schema
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Location} location Location with the factory location
 * @param {City} city City with the factory city
 */
function createFactorySchema(reference,designation,location,city){
    return new Factory(Factory.createFactory(reference,designation,location.latitude,location.longitude,city));
}

/**
 * Creates a valid Factory
 */
function createValidFactory(){
    return createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
                                ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
                                ,VALID_FACTORY_PROPERTIES.VALID_LOCATION);
}

/**
 * Creates a new Factory
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Location} location Location with the factory location
 * @param {City} city City with the factory city
 */
function createFactory(reference,designation,location,city){
    return Factory.createFactory(reference,designation,location.latitude,location.longitude,city);
}