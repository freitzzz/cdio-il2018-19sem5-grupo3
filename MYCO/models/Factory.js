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
        return checkReferenceBusinessRule(reference);
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
    reference:{type: String, validate: referenceValidator, required:true},
    designation:{type: String, validate: designationValidator, required:true},
    location:{type: location.schema, required:true}
});

/**
 * Changes the current factory reference
 * @param {String} reference String with the updating reference
 */
factorySchema.methods.changeReference=function(reference){
    grantReferenceIsValidForUpdate(reference);
    this.reference=reference;
}

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
 * Grants that a reference is valid for update
 * @param {string} reference String with the reference to be updated
 */
function grantReferenceIsValidForUpdate(reference){
    if(!checkReferenceBusinessRule(reference))throw `${reference} is not a valid reference`;
}

/**
 * Checks if a reference is valid according to business rules
 * @param {String} reference String with the reference being checked
 */
function checkReferenceBusinessRule(reference){
    return reference.trim().length>0;
}

/**
 * Checks if a designation is valid according to business rules
 * @param {String} designation String with the designation being checked
 */
function checkDesignationBusinessRule(designation){
    return designation.trim().length>0;
}

/**
 * Exports Factory Data Model required by Mongoose
 */
var Factory = module.exports = mongoose.model('factory', factorySchema);