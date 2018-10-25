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

//Express request
ordersRoute.route('/orders/:id').get(/*async*/ function (req, res, next) {
    var id = req.params.id;
    //Communicate with MYCM

    //Mongoose query
    Order.findById(id, async function (err, order) {

        var orderContentsList = order.orderContents;

        var result = await fetchOrderContents(orderContentsList);

        if (err) {
            return next(new Error(err));
        }
        res.status(200).json(result);
    });
})

async function fetchOrderContents(orderContents) {

    var stringResult = '';

    var orderContentsSize = orderContents.length;

    for (var i = 0; i < orderContentsSize; i++) {

        var currentOrderContent = orderContents[i];
        var currentOrderContentCustomizedProductId = currentOrderContent.customizedproduct;

        var customizedProduct = await getCustomizedProduct(currentOrderContentCustomizedProductId);

        stringResult += JSON.stringify(customizedProduct);
    }

    return stringResult;
}

function getCustomizedProduct(customizedProductId) {

    return new Promise((resolve, reject) => {

        var req = http.get('http://localhost:5000/myc/api/customizedproducts/' + customizedProductId, (resp) => {
            let data = '';

            resp.on('data', (chunk) => {
                data += chunk;
            });

            resp.on('end', () => {
                resolve(JSON.parse(data));
            });

        }).on("error", reject);


        req.end();
    })
}

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