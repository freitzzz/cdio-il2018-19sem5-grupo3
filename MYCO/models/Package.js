/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var Tag = require('../models/Tag');

/**
 * Requires PackageSize enums
 */
var PackageSize=require('./PackageSize');

/**
 * Represents a Package Schema
 */
var packageSchema = new Schema({
    size: {
        type: String,
        enum: ["S", "M", "L"],
        default: PackageSize.MEDIUM,
        required: true
    },
    tag: {
        type: Tag.schema,
        required: true
    }
});

/**
 * Creates a new package
 * @param {PackageSize} size PackageSize with the package size 
 * @param {Array} contents Array with the package contents
 */
packageSchema.statics.createPackage = function (size, contents) {
    if(!PackageSize.values.includes(size))
        throw `{size} is not a valid package size`;
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