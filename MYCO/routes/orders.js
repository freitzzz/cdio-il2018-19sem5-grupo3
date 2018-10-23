const express = require('express');
const ordersRoute = express.Router();
const Order = require('../models/order');
const http = require('http');

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
    Order.create(function (err, order) {
        if (err) {
            return next(new Error(err));
        }
        res.status(201).json(order);
    })
})


module.exports = ordersRoute;