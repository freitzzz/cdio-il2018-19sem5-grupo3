/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

/**
 * Represents a schema for a Factory
 */
var factorySchema=new Schema({
    reference:{type: string, required=true},
    designation:{type: string, required=true}
});

/**
 * Exports Factory Data Model required by Mongoose
 */
var Factory = module.exports = mongoose.model('Factory', factorySchema);