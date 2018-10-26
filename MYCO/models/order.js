var mongoose = require('mongoose');

var orderContentsSchemas = new mongoose.Schema({
    customizedproduct: {
        type: Number,
        validate: {
            validator: function(v){
                return !isNaN(v);
            },
            message : props => `${props.value} is not a valid customized product ID`
        },
        required: [true, 'Customized Products Required']
    },
    quantity: {
        type: Number,
        validate: {
            validator: function(v){
                return !isNaN(v) && v > 0;
            },
            message : props => `${props.value} is not a valid quantity`
        },
        required: [true, 'Quantity Required']
    }
}, {
    _id: false
});

var orderSchema = new mongoose.Schema({

    orderContents: {type : [orderContentsSchemas], required : true},

    status: {
        type: String,
        enum: ["In Validation", "Validated", "In Production", "Ready To Ship", "Delivered"],
        default: "In Validation",
        required: true
    }
}, {
    collection: 'orders'
})

var Order = module.exports = mongoose.model('Order', orderSchema);