/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var Code = require('../models/Code');
/**
 * Represents a Tag Schema
 */
var tagSchema = new Schema({
    code: {
        type: String,
        required: true
    }
});
tagSchema.statics.createTag = function (tag) {
    return { code: tag };
}
/**
 * Exports Tag Data Model required by Mongoose
 */
var Tag = module.exports = mongoose.model('tag', tagSchema);