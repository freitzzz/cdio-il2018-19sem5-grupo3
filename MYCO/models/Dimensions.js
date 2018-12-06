/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');

/**
 * Requires Mongoose Schema for modeling data as a schema
 */
var Schema = mongoose.Schema;

/**
 * Validates the dimensions of a Dimensions Schema
 */
var dimensionsValidator={
    validator:function(dimensionValue){
        return isValidDimensionValue(dimensionValue);
    },
    message:props=>`${props.value} is not a valid dimension`
};

/**
 * Represents a Dimensions Schema
 */
var dimensionsSchema=new Schema({
    width:{
        type:Number,
        required:true,
        validate:dimensionsValidator
    },
    height:{
        type:Number,
        required:true,
        validate:dimensionsValidator
    },
    depth:{
        type:Number,
        required:true,
        validate:dimensionsValidator
    },
    _id:false
});

/**
 * Creates a new Dimensions model
 */
dimensionsSchema.statics.createDimensions=function(width,height,depth){
    grantDimensionValueIsValid(width);
    grantDimensionValueIsValid(height);
    grantDimensionValueIsValid(depth);
    return {
        width:width,
        height:height,
        depth:depth
    };
};

/**
 * Grants that a dimension value is valid
 * @param {Number} dimensionValue Number with the dimension value being granted
 */
function grantDimensionValueIsValid(dimensionValue){
    if(!isValidDimensionValue(dimensionValue))throw `${dimensionValue} is not a valid dimension value`;
}

/**
 * Checks if a dimension value is valid
 * @param {Number} dimensionValue Number with the dimension value being validated
 */
function isValidDimensionValue(dimensionValue){
    return dimensionValue > 0;
}

/**
 * Exports Dimension Schema modeled by Mongoose
 */
module.exports=mongoose.model('dimensions',dimensionsSchema);