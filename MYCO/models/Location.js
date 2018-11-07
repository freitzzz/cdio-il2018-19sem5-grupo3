/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

/**
 * Represents a Location Schema
 */
var locationSchema=new Schema({
    latitude:{type:number,required=true},
    longitude:{type:number,required=true},
    _id=false
});

/**
 * Exports Factory Data Model required by Mongoose
 */
var Location = module.exports = mongoose.model('location', locationSchema);