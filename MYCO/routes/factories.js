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
 * Requires City Mongoose Data Model
 */
const city=require('../models/City');

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
 * Routes the GET of a production factory
 */
factoriesRoute.route('/factories/:id').get(function(request,response){
    factory
        .findById(request.params.id)
        .then(function(_factory){
            response.status(200).json(deserializeFactory(_factory));
        }).catch(function(_error){
            response.status(400).json(noFactoryFound());
        })
})

/**
 * Routes the POST of a new factory
 */
factoriesRoute.route('/factories').post(function(request,response){
    city
        .findById(request.body.cityId)
            .then(function(_city){
                request.body.city=_city;
                factory
                    .create(serializeFactory(request.body))
                    .then(function(createdFactorySchema){
                        response.status(201).json(deserializeFactory(createdFactorySchema));
                    })
                    .catch(function(error){
                        let exceptionName=Object.keys(error.errors)[0];
                        response.status(400).json({error: error.errors[exceptionName].message});
                    })
            }).catch(function(){
                response.status(400).json({error:'There is no city with the given id'});
            })
})

/**
 * Routes the PUT of the factory properties update
 */
factoriesRoute.route('/factories/:id').put(function(request,response){
    factory
        .findById(request.params.id)
            .then(function(_factory){
                updateFactorySchema(request.body,_factory);
                factory.update(_factory);
                response.status(200).json(deserializeFactory(_factory));
            }).catch(function(_error){
                response.status(400).json(_error);
            })
})

/**
 * Routes the DELETE of the factory disable
 */
factoriesRoute.route('/factories/:id').delete(function(request,response){
    factory
        .findById(request.params.id)
            .then(function(_factory){
                disableFactorySchema(_factory);
                factory.update(_factory);
                response.status(204).json();
            }).catch(function(_error){
                response.status(400).json(_error);
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
        ,requestBody.longitude
        ,requestBody.city);
}

/**
 * Deserializes a factory schema into an object
 * @param {Factory.Schema} factorySchema Factory.Schema with the factory schema being deserialized
 */
function deserializeFactory(factorySchema){
    let deserializedFactory={
        id:factorySchema.id,
        reference:factorySchema.reference,
        designation:factorySchema.designation,
        latitude:factorySchema.location.latitude,
        longitude:factorySchema.location.longitude
    };
    if(factorySchema.city)deserializedFactory.cityId=factorySchema.city.id;
    return deserializedFactory
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
 * Updates a factory schema properties
 * @param {Object} factoryUpdate Object with the factory properties to update
 * @param {Factory.Schema} factorySchema Factory.Schema with the factory schema being updated
 */
function updateFactorySchema(factoryUpdate,factorySchema){
    if(factoryUpdate.reference){
        factorySchema.changeReference(factoryUpdate.reference);
    }
    
    if(factoryUpdate.designation){
        factorySchema.changeDesignation(factoryUpdate.designation);
    }

    if(factoryUpdate.latitude){
        factorySchema.changeLatitude(factoryUpdate.latitude);
    }

    if(factoryUpdate.longitude){
        factorySchema.changeLongitude(factoryUpdate.longitude);
    }
}

/**
 * Enables a factory schema
 * @param {Factory.Schema} factorySchema Factory.Schema with the factory schema being enabled
 */
function enableFactorySchema(factorySchema){
    factorySchema.enable();
}

/**
 * Disables a factory schema
 * @param {Factory.Schema} factorySchema Factory.Schema with the factory schema being disabled
 */
function disableFactorySchema(factorySchema){
    factorySchema.disable();
}

/**
 * Provides a message object for justifying that there are no available production factories
 */
function noAvailableFactories(){return {message:"There are no production factories available"}}

/**
 * Provides a message object for justifying that there is no production factory being fetched
 */
function noFactoryFound(){return {message:"No production factory was found"}}

/**
 * Exports Production Factories API router
 */
module.exports = factoriesRoute;