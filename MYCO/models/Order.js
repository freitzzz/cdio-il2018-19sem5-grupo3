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

/**
 * Requires PackageSize enums
 */
var PackageSize=require('./PackageSize');

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

/**
 * Registers the current order packages
 * @param {Array} packages with the packages information
 */
orderSchema.methods.registerPackages=function(packages){
    if(packages==null ||packages.length==0)throw 'Invalid packages information';
    grantOrderStateIsValidForPackageRegister(this.status);
    let newPackages=[];
    for(let i=0;i<packages.length;i++){
        let nextPackage=packages[i];
        grantOrderPackageCountIsValid(nextPackage.count);
        for(let j=0;j<nextPackage.count;j++)newPackages.push(package.createPackage(nextPackage.size,[],nextPackage.weight,nextPackage.dimensions));
    }
    this.packages=newPackages;
}

/**
 * Grants that a order package count is valid
 * @param {Number} count Number with the package count
 */
function grantOrderPackageCountIsValid(count){
    if(count<=0)
        throw `{count} is not a valid package count`;
}

/**
 * Grants that a order state is valid for registering packages
 * @param {OrderState} state OrderState with the order state
 */
function grantOrderStateIsValidForPackageRegister(state){
    if(!(state==OrderState.PRODUCTED||state==OrderState.READY_TO_SHIP))
        throw `{state} is not a valid state to register packages`;
}

var Order = module.exports = mongoose.model('Order', orderSchema);