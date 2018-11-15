/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var Tag = require('../models/Tag');
/**
 * Represents a Package Schema
 */
var packageSchema = new Schema({
    size: {
        type: String,
        enum: ["S", "M", "L"],
        default: "M",
        required: true
    },
    tag: {
        type: Tag.schema,
        required: true
    }
});

packageSchema.statics.createPackage = function (size, contents) {
    let tag = size + "-" + S4() + "CS";

    contents.forEach(element => {
        tag += "-" + element;
    });
    console.log(tag);
    return  {
        size: size,
        tag: Tag.createTag(tag)
    }
}

function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}


/**
 * Exports Package Data Model required by Mongoose
 */
var Package = module.exports = mongoose.model('package', packageSchema);