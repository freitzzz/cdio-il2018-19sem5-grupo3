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
        return latitude>=-90 && latitude<=90;
    },
    message:props=>`${props.value} is not a valid latitude`
}

/**
 * Validates a location longitudde
 */
var longitudeValidator={
    validator:function(longitude){
        return longitude>=-180 && longitude<=180;
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
 * Exports Factory Data Model required by Mongoose
 */
var Location = module.exports = mongoose.model('location', locationSchema);