var mongoose = require('mongoose');

var orderSchema = new mongoose.Schema({
    
    orderContents: {type: Map,
                    of: Number
    },

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