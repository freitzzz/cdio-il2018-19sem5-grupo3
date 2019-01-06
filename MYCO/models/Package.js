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
 * Requires Dimensions Schema Model
 */
var Dimensions=require('./Dimensions');

/**
 * Represents a Package Schema
 */
var packageSchema = new Schema({
    size: {
        type: String,
        default: PackageSize.MEDIUM,
        required: true
    },
    tag: {
        type: Tag.schema,
        required: true
    },
    weight:{
        type: Number,
        required: true
    },
    dimensions:{
        type:Dimensions.schema,
        required:true
    }
});

/**
 * Creates a new package
 * @param {PackageSize} size PackageSize with the package size 
 * @param {Array} contents Array with the package contents
 * @param {Number} weight Number with the package weight
 * @param {Object} dimensions Dimensions model with the package dimensions
 */
packageSchema.statics.createPackage = function (size, contents,weight,dimensions) {
    if(!PackageSize.values.includes(size))
        throw `${size} is not a valid package size`;

    if(weight<=0)
        throw `${weight} is not a valid package weight`;
    let tag = size + "-" + S4() + "CS";

    contents.forEach(element => {
        tag += "-" + element;
    });
    console.log(tag);
    return  {
        size: size,
        tag: Tag.createTag(tag),
        weight:weight,
        dimensions:Dimensions.createDimensions(dimensions.width,dimensions.height,dimensions.depth)
    };
}

function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}


/**
 * Exports Package Data Model required by Mongoose
 */
var Package = module.exports = mongoose.model('package', packageSchema);