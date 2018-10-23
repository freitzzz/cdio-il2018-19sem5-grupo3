var express = require('express');
var ordersRoute = express.Router();
var Order = require('../models/order')

//Get all orders in the database
ordersRoute.route('/').get(function (req, res, next) {
    Order.find(function (err, orders) {
        if (err) {
            return next(new Error(err))
        }

        res.status(200).json(orders) //return all orders
    })
})



module.exports = ordersRoute;