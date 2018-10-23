var express = require('express');
var ordersRoute = express.Router();
var Order = require('../models/order');

//Get all orders in the database
ordersRoute.route('/orders').get(function (req, res, next) {
    Order.find(function (err, orders) {
        if (err) {
            return next(new Error(err));
        }
        res.status(200).json(orders); //return all orders
    })
})

ordersRoute.route('/orders/:id').get(function (req, res, next) {
    //Communicate with MYCM
    Order.findById(function (err, order) {
        if (err) {
            return next(new Error(err));
        }
        res.status(200).json(order);
    })
})

ordersRoute.route('/orders').post(function (req, res, next) {
    //Communicate with MYCM
    Order.create(function (err, order) {
        if (err) {
            return next(new Error(err));
        }
        res.status(201).json(order);
    })
})



module.exports = ordersRoute;