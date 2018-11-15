/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
/**
 * Represents a Package Schema
 */
var packageSchema = new Schema({
    size: {
        type: String,
        enum: ["S", "M", "L"],
        default: "M",
        required: true
    }
});
/**
 * Exports Package Data Model required by Mongoose
 */
var Package = module.exports = mongoose.model('package', packageSchema);