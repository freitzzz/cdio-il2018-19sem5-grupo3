const express = require('express');
const ordersRoute = express.Router();
const Order = require('../models/order');
const http = require('http');

//Get all orders in the database
//TODO Pretty things up
ordersRoute.route('/orders').get(function (req, res, next) {
    Order.find(function (err, orders) {
        if (err) {
            return next(new Error(err));
        }
        res.status(200).json(orders); //return all orders
    })
})

ordersRoute.route('/orders/:id').get(/*async*/ function (req, res, next) {
    var id = req.params.id;
    //Communicate with MYCM
    var customizedProductsInfo = '';
    Order.findById(id, function (err, order) {
        console.log('hello');

        console.log(res.json(order));
        for (var orderContentSchema in order.orderContents) {
            console.log('ola');
            //TODO dinamically change localhosts
            http.get('http://localhost:5000/myc/api/customizedProduct/' + orderContentSchema.customizedproduct, (resp) => {
                let data = '';

                resp.on('data', (chunk) => {
                    data += chunk;
                });

                resp.on('end', () => {
                    customizedProductsInfo += JSON.parse(data + orderContentSchema.quantity);
                });

            }).on("error", (err) => {
                //TODO don't log on console
                console.log("Error: " + err.message);
            });

        }
        //customizedProductsInfo += JSON.parse(order.status);
        console.log(customizedProductsInfo);
        if (err) {
            return next(new Error(err));
        }
        console.log('123');
        //res.status(200).json(customizedProductsInfo);
    });
})

//TODO Pretty things up
ordersRoute.route('/orders').post(function (req, res, next) {
    Order.create(function (err, order) {
        if (err) {
            return next(new Error(err));
        }
        res.status(201).json(order);
    })
})


module.exports = ordersRoute;