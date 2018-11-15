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
    });
});

/**
 * Ensures that its not possible to create a factory with a null reference
 */
function ensureCantCreateFactoryWithNullReference(){
    let factory=()=>{
        createFactory(null
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,VALID_FACTORY_PROPERTIES.VALID_LOCATION)
    };
    expect(factory).toThrow();
}

/**
 * Ensures that its not possible to create a factory with an empty reference
 */
function ensureCantCreateFactoryWithEmptyReference(){
    let factory=()=>{
        createFactory(""
        ,VALID_FACTORY_PROPERTIES.VALID_DESIGNATION
        ,VALID_FACTORY_PROPERTIES.VALID_LOCATION)
    };
    expect(factory).toThrow();
}

/**
 * Ensures that its not possible to create a factory with a null designation
 */
function ensureCantCreateFactoryWithNullDesignation(){
    let factory=()=>{
        createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,null
        ,VALID_FACTORY_PROPERTIES.VALID_LOCATION)
    };
    expect(factory).toThrow();
}

/**
 * Ensures that its not possible to create a factory with an empty designation
 */
function ensureCantCreateFactoryWithEmptyDesignation(){
    let factory=()=>{
        createFactory(VALID_FACTORY_PROPERTIES.VALID_REFERENCE
        ,""
        ,VALID_FACTORY_PROPERTIES.VALID_LOCATION)
    };
    expect(factory).toThrow();
}

/**
 * Creates a new Factory
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Location} location String with the factory location
 */
function createFactory(reference,designation,location){
    return new Factory.schema(Factory.createFactory(reference,designation,location));
}

/**
 * Creates a new Factory
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Location} location Location with the factory location
 * @param {City} city City with the factory city
 */
function createFactory(reference,designation,location,city){
    return new Factory.schema(Factory.createFactory(reference,designation,location,city));
}