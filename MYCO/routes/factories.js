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
        .find({available:true})
        .then(function(factories){
            if(factories.length>0){
                response.status(200).json(schemasToBasicFactories(factories));
            }else{
                response.status(404).json(noAvailableFactories());
            }
        })
})

/**
 * Routes the GET of a production factory
 */
factoriesRoute.route('/factories/:id').get(function(request,response){
    factory
        .findById(request.params.id,{available:true})
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
    let factoryModel=Object.assign({},request.body);
    factory.validateFactoryModelAsCallback(factoryModel)
    .then(()=>{
        grantFactoryDoesntAlreadyExist(factoryModel)
        .then(()=>{
            cityExists(factoryModel.cityId)
            .then((foundCity)=>{
                factoryModel.city=foundCity;
                factory
                    .create(serializeFactory(factoryModel))
                    .then((createdFactory)=>{
                        response.status(201).json(deserializeFactory(createdFactory));
                    })
                    .catch((_error_message)=>{
                        response.status(500).json({message:"An expected error has occurd! Please contact the developers and send them this message",cause:_error_message})
                    });
            })
            .catch((possible_error)=>{
                if(possible_error!=null)response.status(400).json({message:possible_error});
                factory
                    .create(serializeFactory(factoryModel))
                    .then((createdFactory)=>{
                        response.status(201).json(deserializeFactory(createdFactory));
                    })
                    .catch((_error_message)=>{
                        response.status(500).json({message:"An expected error has occurd! Please contact the developers and send them this message",cause:_error_message})
                    });
            });
        })
        .catch((_error_message)=>{
            response.status(400).json({error:_error_message});
        });
    })
    .catch((_error_message)=>{
        response.status(400).json({error:_error_message});
    });
})

/**
 * Routes the PUT of the factory properties update
 */
factoriesRoute.route('/factories/:id').put(function(request,response){
    let factoryId=request.params.id;
    factoryExists(factoryId)
    .then((foundFactory)=>{
        updateFactorySchema(request.body,foundFactory)
        .then((updatedFactory)=>{
            factory
                .findByIdAndUpdate(factoryId,updatedFactory,{new:true})
                .then((savedUpdatedFactory)=>{
                    response.status(200).json(savedUpdatedFactory);
                })
                .catch(()=>{
                    response.status(500).json({message:"An internal error occurd while accesing our database :("});
                });
        })
        .catch((_error_message)=>{
            response.status(400).json({message:_error_message});
        });
    })
    .catch((_error_message)=>{
        response.status(400).json({message:_error_message});
    });
});

/**
 * Routes the DELETE of the factory disable
 */
factoriesRoute.route('/factories/:id').delete(function(request,response){
    let factoryId=request.params.id;
    factoryExists(factoryId)
    .then((foundFactory)=>{
        disableFactorySchema(foundFactory)
        .then((updatedFactory)=>{
            factory
                .findByIdAndUpdate(factoryId,updatedFactory)
                .then(()=>{
                    response.status(204).json();
                })
                .catch(()=>{
                    response.status(500).json({message:"An internal error occurd while accesing our database :("});
                });
        })
        .catch((_error_message)=>{
            response.status(400).json({message:_error_message});
        });
    })
    .catch((_error_message)=>{
        response.status(400).json({message:_error_message});
    });
});

/**
 * Grants that a factory doesnt already exist
 * @param {Object} factoryDetails Object with the factory details
 */
function grantFactoryDoesntAlreadyExist(factoryDetails){
    return new Promise((accept,reject)=>{
        factory.findOne({'reference':factoryDetails.reference},'reference ')
        .then((foundFactoryByReference)=>{
            if(foundFactoryByReference)reject('There is already a factory with the reference '+factoryDetails.reference);
            factory.findOne({
                'location':{
                    latitude:factoryDetails.latitude,longitude:factoryDetails.longitude
                }})
                .then((foundFactoryByLocation)=>{
                    if(foundFactoryByLocation)reject('There is already a factory based on the given location');
                    accept();
                })
                .catch(()=>{
                    reject('An internal error occurd while accesing our database :(')
                });
        })
        .catch(()=>{
            reject('An internal error occurd while accesing our database :(')
        });
    });
}

/**
 * Checks if a city exists, and if exists returns it as a callback function
 * @param {String} cityId String with the city persistence identifier
 */
function cityExists(cityId){
    return new Promise((accept,reject)=>{
        if(cityId==null)reject();
        city
        .findById(cityId)
        .then((foundCity)=>{
            foundCity!=null ? accept(foundCity) : reject('There is no city with the given id');
        })
        .catch(()=>{
            reject('An internal error occurd while processing our database :(');
        });
    });
}

/**
 * Checks if a factory exists and if exists, returns it as a callback function
 * @param {String} factoryId String with the factory id
 */
function factoryExists(factoryId){
    return new Promise((accept,reject)=>{
        factory
            .findById(factoryId,{available:true})
            .then((foundFactory)=>{
                foundFactory!=null ? accept(foundFactory) : reject("No factory found with the given id");
            })
            .catch(()=>{
                reject('An internal error occurd while processing our database :(');
            });
    });
}

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
    if(factorySchema.city!=null)deserializedFactory.cityId=factorySchema.city.id;
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
    return new Promise((accept,reject)=>{
        try{
            let atLeastOneUpdate=false;
            if(factoryUpdate.reference!=null){
                factorySchema.changeReference(factoryUpdate.reference);
                atLeastOneUpdate=true;
            }
            
            if(factoryUpdate.designation!=null){
                factorySchema.changeDesignation(factoryUpdate.designation);
                atLeastOneUpdate=true;
            }
        
            if(factoryUpdate.latitude!=null){
                factorySchema.changeLatitude(factoryUpdate.latitude);
                atLeastOneUpdate=true;
            }
        
            if(factoryUpdate.longitude!=null){
                factorySchema.changeLongitude(factoryUpdate.longitude);
                atLeastOneUpdate=true;
            }
            if(!atLeastOneUpdate)reject('No updates were made');
            accept(factoryUpdate);
        }catch(_error_message){
            reject(_error_message);
        }
    });
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
    return new Promise((accept,reject)=>{
        try{
            factorySchema.disable();
            accept(factorySchema);
        }catch(_error_message){
            reject(_error_message);
        }
    });
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