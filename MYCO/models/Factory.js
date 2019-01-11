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
 * Requires City for representing cities
 */
var city=require('../models/City');

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
        return checkDesignationBusinessRule(designation);
    },
    message:props=>`${props.value} is not a valid designation`
}

/**
 * Represents a Factory Schema
 */
var factorySchema=new Schema({
    reference:{type: String, validate: referenceValidator, unique:true, required:true},
    designation:{type: String, validate: designationValidator, required:true},
    location:{type: location.schema, required:true},
    city:{type: city.schema, required:false},
    available:{type: Boolean,required:false}
});

/**
 * Changes the current factory reference
 * @param {String} reference String with the updating reference
 */
factorySchema.methods.changeReference=function(reference){
    grantReferenceIsValidForUpdate(reference);
    if(this.reference==reference)throw 'Factory reference is the same as the updating one';
    this.reference=reference;
}

/**
 * Changes the current factory designation
 * @param {String} designation String with the updating designation
 */
factorySchema.methods.changeDesignation=function(designation){
    grantDesignationIsValidForUpdate(designation);
    if(this.designation==designation)throw 'Factory designation is the same as the updating one';
    this.designation=designation;
}

/**
 * Changes the current factory latitude
 * @param {Number} latitude Number with the updating latitude
 */
factorySchema.methods.changeLatitude=function(latitude){
    this.location.changeLatitude(latitude);
}

/**
 * Changes the current factory longitude
 * @param {Number} latitude Number with the updating longitude
 */
factorySchema.methods.changeLongitude=function(longitude){
    this.location.changeLongitude(longitude);
}

/**
 * Enables the current factory
 */
factorySchema.methods.enable=function(){
    grantFactoryIsDisabled(this.available);
    this.available=true;
}

/**
 * Disables the current factory
 */
factorySchema.methods.disable=function(){
    grantFactoryIsEnabled(this.available);
    this.available=false;
}

/**
 * Checks if the current factory is located on a certain city
 * @param {City.Schema} city City with the city being checked
 * @returns Boolean true if the factory is located at a certain city, false if not
 */
factorySchema.methods.isLocated=function(city){
    return this.city ? this.city.equals(city) : false;
}

/**
 * Creates a new Factory object
 * @param {String} reference String with the factory reference
 * @param {String} designation String with the factory designation
 * @param {Number} locationLatitude Number with the factory latitude
 * @param {Number} locationLongitude Number with the factory longitude
 * @param {City.Schema} city City.Schema with the city where the factory is located
 */
factorySchema.statics.createFactory=function (reference,designation,locationLatitude,locationLongitude,city){
    let factory={
        reference:reference,
        designation:designation,
        location:location.createLocation(locationLatitude,locationLongitude),
        available:true
    }
    if(city)factory.city=city;
    return factory; 
}

/**
 * Validates a factory model as a callback function
 * @param {Object} factoryModel Object with the factory model being validated 
 */
factorySchema.statics.validateFactoryModelAsCallback=function(factoryModel){
    return new Promise((accept,reject)=>{
        try{
            factorySchema.statics.validateFactoryModel(factoryModel);
            accept();
        }catch(_error_message){
            reject(_error_message);
        }
    });
}

/**
 * Grants that a factory model is valid
 * @param {Object} factoryModel Object with the factory model being validated
 */
factorySchema.statics.validateFactoryModel=function(factoryModel){
    if(!factoryModel)throw 'Invalid factory details';
    if(!checkReferenceBusinessRule(factoryModel.reference))throw `${reference} is not a valid reference`;
    if(!checkDesignationBusinessRule(factoryModel.designation))throw `${designation} is not a valid designation`;
    location.validateLocationModel({latitude:factoryModel.latitude,longitude:factoryModel.longitude});
};

/**
 * Grants that a reference is valid for update
 * @param {string} reference String with the reference to be updated
 */
function grantReferenceIsValidForUpdate(reference){
    if(!checkReferenceBusinessRule(reference))throw `${reference} is not a valid reference`;
}

/**
 * Grants that a designation is valid for update
 * @param {string} designation String with the designation to be updated
 */
function grantDesignationIsValidForUpdate(designation){
    if(!checkDesignationBusinessRule(designation))throw `${designation} is not a valid designation`;
}

/**
 * Grants that a factory is enabled
 * @param {Boolean} available Boolean with the factory availability
 */
function grantFactoryIsEnabled(available){
    if(!available)throw `Factory is disabled`;
}

/**
 * Grants that a factory is disabled
 * @param {Boolean} available Boolean with the factory availability
 */
function grantFactoryIsDisabled(available){
    if(available)throw `Factory is enabled`;
}

/**
 * Checks if a reference is valid according to business rules
 * @param {String} reference String with the reference being checked
 */
function checkReferenceBusinessRule(reference){
    return reference && reference.trim().length>0;
}

/**
 * Checks if a designation is valid according to business rules
 * @param {String} designation String with the designation being checked
 */
function checkDesignationBusinessRule(designation){
    return designation && designation.trim().length>0;
}

/**
 * Exports Factory Data Model required by Mongoose
 */
var Factory = module.exports = mongoose.model('factory', factorySchema);