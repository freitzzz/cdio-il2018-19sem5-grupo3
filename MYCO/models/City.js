/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
/**
 * Requires Location for representing locations
 */
var location=require('../models/Location');

/**
 * Validates a City name
 */
var nameValidator={
    validator:function(name){
        return checkNameBusinessRule(name);
    },
    message:props=>`${props.value} is not a valid name`
}
/**
 * Represents a City Schema
 */
var citySchema = new Schema({

    name:{type: String, validate: nameValidator, required:true},
    location:{type: location.schema, required:true},

});

/**
 * Checks if two cities are equal
 * @param {City.Schema} city City with the comparing city
 * @returns Boolean true if both cities are equal, false if not 
 */
citySchema.methods.equals=function(city){
    return city!=null ? this.name.valueOf()==city.name.valueOf() : false;
}

/**
 * Creates a City object
 * @param {String} name String with the city name
 * @param {Number} locationLatitude Number with the city latitude
 * @param {Number} locationLongitude Number with the city longitude
 */
citySchema.statics.createCity=function (name,locationLatitude,locationLongitude){
    return {
        name:name,
        location:location.createLocation(locationLatitude,locationLongitude)
    }
}
/**
 * Checks if a name is valid according to business rules
 * @param {String} name String with the name being checked
 */
function checkNameBusinessRule(name){
    return name.trim().length>0;
}
var City = module.exports = mongoose.model('City', citySchema);