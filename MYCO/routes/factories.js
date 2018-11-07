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
 * Exports Production Factories API router
 */
module.exports = factoriesRoute;