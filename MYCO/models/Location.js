/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

/**
 * Validates a location latitude
 */
var latitudeValidator={
    validator:function(latitude){
        return checkLatitudeBusinessRule(latitude);
    },
    message:props=>`${props.value} is not a valid latitude`
}

/**
 * Validates a location longitudde
 */
var longitudeValidator={
    validator:function(longitude){
        return checkLongitudeBusinessRule(longitude);
    },
    message:props=>`${props.value} is not a valid longitude`
}


/**
 * Represents a Location Schema
 */
var locationSchema=new Schema({
    latitude:{type:Number,validate:latitudeValidator,required:true},
    longitude:{type:Number,validate:longitudeValidator,required:true},
    _id:false
});

/**
 * Changes the current location latitude
 * @param {Number} latitude Number with the updating latitude
 */
locationSchema.methods.changeLatitude=function(latitude){
    grantLatitudeIsValidForUpdate(latitude);
    this.latitude=latitude;
}

/**
 * Changes the current location longitude
 * @param {Number} latitude Number with the updating longitude
 */
locationSchema.methods.changeLongitude=function(longitude){
    grantLongitudeIsValidForUpdate(longitude);
    this.longitude=longitude;
}

/**
 * Creates a new Location object
 * @param {Number} latitude Number with the location latitude
 * @param {Number} longitude Number with the location longitude
 */
locationSchema.statics.createLocation= function(latitude,longitude){
    return {latitude,longitude};
}

/**
 * Grants that a latitude is valid for update
 * @param {Number} latitude Number with the latitude to be updated
 */
function grantLatitudeIsValidForUpdate(latitude){
    if(!checkLatitudeBusinessRule(latitude))throw `${latitude} is not a valid latitude`;
}

/**
 * Grants that a longitude is valid for update
 * @param {Number} longitude Number with the longitude to be updated
 */
function grantLongitudeIsValidForUpdate(longitude){
    if(!checkLongitudeBusinessRule(longitude))throw `${longitude} is not a valid longitude`;
}

/**
 * Checks if a latitude is valid according to business rules
 * @param {Number} latitude Number with the latitude being checked
 */
function checkLatitudeBusinessRule(latitude){
    return latitude && latitude>=-90 && latitude<=90;
}

/**
 * Checks if a longitue is valid according to business rules
 * @param {Number} longitude Number with the longitude being checked
 */
function checkLongitudeBusinessRule(longitude){
    return longitude && longitude>=-180 && longitude<=180;
}

/**
 * Exports Factory Data Model required by Mongoose
 */
var Location = module.exports = mongoose.model('location', locationSchema);