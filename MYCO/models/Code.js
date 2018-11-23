/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
/**
 * Represents a Code Schema
 */
var codeSchema = new Schema({
    value: { type: String, required: true }
});
/**
 * Exports Code Data Model required by Mongoose
 */
var Code = module.exports = mongoose.model('code', codeSchema);