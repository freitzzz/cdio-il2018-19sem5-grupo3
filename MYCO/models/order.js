/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var city = require('../models/City');
var factory = require('../models/Factory');
var package = require('../models/Package');

var orderContentsSchemas = new mongoose.Schema({
    customizedproduct: {
        type: Number,
        validate: {
            validator: function (v) {
                return !isNaN(v);
            },
            message: props => `${props.value} is not a valid customized product ID`
        },
        required: [true, 'Customized Products Required']
    },
    quantity: {
        type: Number,
        validate: {
            validator: function (v) {
                return !isNaN(v) && v > 0;
            },
            message: props => `${props.value} is not a valid quantity`
        },
        required: [true, 'Quantity Required']
    }

}, {
        _id: false
    });

var orderSchema = new Schema({

    orderContents: { type: [orderContentsSchemas], required: true },

    packages: { type: [package.schema], required: true },

    status: {
        type: String,
        enum: ["In Validation", "Validated", "In Production", "Ready To Ship", "Delivered"],
        default: "In Validation",
        required: true
    },
    cityToDeliver: {
        type: city.schema,
        required: true
    },
    factoryOfProduction: {
        type: factory.schema,
        required: false
    }
}, {
        collection: 'orders'
    })

var Order = module.exports = mongoose.model('Order', orderSchema);