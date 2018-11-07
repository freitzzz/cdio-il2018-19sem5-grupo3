/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;

/**
 * Represents a Factory Schema
 */
var factorySchema=new Schema({
    reference:{type: String, required:true},
    designation:{type: String, required:true}
});

/**
 * Exports Factory Data Model required by Mongoose
 */
var Factory = module.exports = mongoose.model('factory', factorySchema);