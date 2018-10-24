var mongoose = require('mongoose');

var orderContentsSchemas = new mongoose.Schema({
    customizedproduct: Number,
    quantity: Number
}, { _id: false });

var orderSchema = new mongoose.Schema({

    orderContents: [orderContentsSchemas],

    status: {
        type: String,
        enum: ['In Validation', 'Validated', 'In Production', 'Ready To Ship', 'Delivered'],
        default: 'In Validation'
    }
},
    {
        collection: 'orders'
    }
)

module.exports = mongoose.model('Order', orderSchema);