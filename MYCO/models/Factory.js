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
 * Validates a factory reference
 */
var referenceValidator={
    validator:function(reference){
        return reference.trim().length>0;
    },
    message:props=>`${props.value} is not a valid reference`
}

/**
 * Validates a factory designation
 */
var designationValidator={
    validator:function(designation){
        return designation.trim().length>0;
    },
    message:props=>`${props.value} is not a valid designation`
}

/**
 * Represents a Factory Schema
 */
var factorySchema=new Schema({
    reference:{type: String, validate:referenceValidator, required:true},
    designation:{type: String, validate: designationValidator, required:true},
    location:{type: location.schema, required:true}
});

/**
 * Creates a new Factory object
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Number} locationLatitude Number with the factory latitude
 * @param {Number} locationLongitude Number with the factory longitude
 */
factorySchema.statics.createFactory=function (reference,designation,locationLatitude,locationLongitude){
    return {
        reference:reference,
        designation:designation,
        location:location.createLocation(locationLatitude,locationLongitude)
    }
}


/**
 * Exports Factory Data Model required by Mongoose
 */
var Factory = module.exports = mongoose.model('factory', factorySchema);