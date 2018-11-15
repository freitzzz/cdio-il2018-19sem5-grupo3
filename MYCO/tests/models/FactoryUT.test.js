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
 * Tests the creation of a Factory
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
    });
});

/**
 * Ensures that its not possible to create a factory with a null reference
 */
function ensureCantCreateFactoryWithNullReference(){
    ensureSchemaIsInvalid(createFactory(null
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with an empty reference
 */
function ensureCantCreateFactoryWithEmptyReference(){
    ensureSchemaIsInvalid(createFactory(""
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with a null designation
 */
function ensureCantCreateFactoryWithNullDesignation(){
    ensureSchemaIsInvalid(createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,null
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with an empty designation
 */
function ensureCantCreateFactoryWithEmptyDesignation(){
    ensureSchemaIsInvalid(createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,""
        ,LocationUT.createValidLocation())
    );
}

/**
 * Ensures that its not possible to create a factory with a null location
 */
function ensureCantCreateFactoryWithNullLocation(){
    ensureSchemaIsInvalid(createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,{latitude:null,longitude:null})
    );
}

/**
 * Ensures that its not possible to create a factory with an empty location
 */
function ensureCantCreateFactoryWithEmptyLocation(){
    ensureSchemaIsInvalid(createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,{})
    );
}

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
 * Creates a new Factory
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Location} location Location with the factory location
 * @param {City} city City with the factory city
 */
function createFactory(reference,designation,location,city){
    return new Factory(Factory.createFactory(reference,designation,location.latitude,location.longitude,city));
}