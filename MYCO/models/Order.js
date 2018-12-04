/**
 * Requires Mongoose for Data Modeling
 */
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var city = require('./City');
var factory = require('./Factory');
var package = require('./Package');
/**
 * Requires OrderState enums
 */
var OrderState=require('./OrderState');

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

    packages: { type: [package.schema], required: false, default:[] },

    status: {
        type: String,
        default: OrderState.IN_VALIDATION,
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
});

/**
 * Changes the current order state
 * @param {State} orderState State with the new order state 
 */
orderSchema.methods.changeState=function(orderState){
    if(OrderState.values.includes(orderState)){
        if(this.status==orderState)throw 'The order state being changed is the same';
        this.status=orderState;
    }else{
        throw 'Invalid State!';
    }
}

var Order = module.exports = mongoose.model('Order', orderSchema);