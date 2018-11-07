/**
 * Requires Express for treating MYCO Factories API requests
 */
const express = require('express');
/**
 * Exposes the factories router
 */
const factoriesRoute = express.Router();

/**
 * Requires Factory Mongoose Data Model
 */
const factory = require('../models/Factory');

/**
 * Routes the GET of all available factories
 */
factoriesRoute.route('/factories').get(function(request,response){
    factory
        .find()
        .then(function(factories){
            if(factories.length>0){
                response.status(200).json(schemasToBasicFactories(factories));
            }else{
                response.status(400).json(noAvailableFactories());
            }
        })
})

/**
 * Routes the POST of a new factory
 */
factoriesRoute.route('/factories').post(function(request,response,mw){
    factory
        .create(serializeFactory(request.body))
            .then(function(createdFactorySchema){
                response.status(201).json(deserializeFactory(createdFactorySchema));
            })
            .catch(function(error){
                let exceptionName=Object.keys(error.errors)[0];
                response.status(400).json({error: error.errors[exceptionName].message});
            })
})

/**
 * Serializes the request body into a Factory Object
 * @param {Object} requestBody Object with the request body
 */
function serializeFactory(requestBody){
    return factory.createFactory
        (requestBody.reference
        ,requestBody.designation
        ,requestBody.latitude
        ,requestBody.longitude);
}

/**
 * Deserializes a factory schema into an object
 * @param {Factory.Schema} factorySchema Factory.Schema with the factory schema being deserialized
 */
function deserializeFactory(factorySchema){
    return {
        id:factorySchema.id,
        reference:factorySchema.reference,
        designation:factorySchema.designation,
        latitude:factorySchema.location.latitude,
        longitude:factorySchema.location.longitude
    }
}

/**
 * Transforms a collection of factory schemas into a collection of basic information factory objects
 * @param {Factory.Schema} factorySchemas List with all factory schemas being transformed
 * @returns {List} List with the transformed collection of basic information factory objects
 */
function schemasToBasicFactories(factorySchemas){
    let basicFactories=[];
    factorySchemas.forEach(function(factorySchema){basicFactories.push(schemaToBasicFactory(factorySchema))})
    return basicFactories;
}

/**
 * Transforms a Factory Schema into a basic information factory object
 * @param {Factory.Schema} factorySchema 
 */
function schemaToBasicFactory(factorySchema){
    return {
        id:factorySchema.id,
        reference:factorySchema.reference
    }
}

/**
 * Provides a message object for justifying that there are no available production factories
 */
function noAvailableFactories(){return {message:"There are no production factories available"}}

/**
 * Exports Production Factories API router
 */
module.exports = factoriesRoute;