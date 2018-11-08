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
const City = require('../models/City');
/**
 * Routes the POST of a city
 */
citiesRoute.route('/cities').post(function(request,response,mw){
    city
        .create(serializeCity(request.body))
            .then(function(createdCitySchema){
                response.status(201).json(deserializeCity(createdCitySchema));
            })
            .catch(function(error){
                let exceptionName=Object.keys(error.errors)[0];
                response.status(400).json({error: error.errors[exceptionName].message});
            })
})
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
module.exports = (citiesRoute,City);