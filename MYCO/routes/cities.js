/**
 * Requires Express for treating MYCO Cities API requests
 */
const express = require('express');
/**
 * Exposes the cities router
 */
const citiesRoute = express.Router();
/**
 * Requires City Mongoose Data Model
 */
const city = require('../models/City');

/**
 * Routes the POST of a city
 */
citiesRoute.route('/cities').post(function(request,response){
    let cityModel=request.body;
    city.validateCityModelAsCallback(cityModel)
    .then(()=>{
        grantCityDoesntAlreadyExist(cityModel)
        .then(()=>{
            city
            .create(serializeCity(cityModel))
                .then(function(createdCitySchema){
                    response.status(201).json(deserializeCity(createdCitySchema));
                })
                .catch(function(error){
                    let exceptionName=Object.keys(error.errors)[0];
                    response.status(400).json({error: error.errors[exceptionName].message});
                });
        })
        .catch((_error_message)=>{
            response.status(400).json({error:_error_message});
        });
    })
    .catch((_error_message)=>{
        response.status(400).json({error:_error_message});
    });
});

/**
 * Grants that a city doesnt already exist
 * @param {Object} cityDetails Object with the city details
 */
function grantCityDoesntAlreadyExist(cityDetails){
    return new Promise((accept,reject)=>{
        city.findOne({'name':cityDetails.name},'name ')
        .then((foundCityByName)=>{
            if(foundCityByName)reject('There is already a city with the name '+cityDetails.name);
            city.findOne({
                'location':{
                    latitude:cityDetails.latitude,longitude:cityDetails.longitude
                }})
                .then((foundCityByLocation)=>{
                    if(foundCityByLocation)reject('There is already a city based on the given location');
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
 * Serializes the request body into a City Object
 * @param {Object} requestBody Object with the request body
 */
function serializeCity(requestBody){
    return city.createCity
        (requestBody.name
        ,requestBody.latitude
        ,requestBody.longitude);
}
/**
 * Deserializes a city schema into an object
 * @param {City.Schema} citySchema City.Schema with the city schema being deserialized
 */
function deserializeCity(citySchema){
    return {
        id:citySchema.id,
        name:citySchema.name,
        latitude:citySchema.location.latitude,
        longitude:citySchema.location.longitude
    }
}
module.exports = citiesRoute;