const express = require('express');
const ordersRoute = express.Router();
const Order = require('../models/order');
const http = require('http');

//Get all orders in the database
//Handle errors by using the ones available in the mongoose schema
ordersRoute.route('/orders').get(function (req, res, next) {
    Order.find(function (err, orders) {
        if (!orders) {
            return next(res.status(404).json({
                Error: 'No orders found'
            }));
        } else if (err) {
            return next(res.status(500).json({
                Error: 'An unexpected error occurred. Please try again'
            }));
        } else {
            res.status(200).json(orders); //return all orders
        }
    })
})

//Gets an order and its details by its id
//TODO Handle errors by using the ones available in the mongoose schema
//TODO add query to route path to know if the order has to be detailed or not
ordersRoute.route('/orders/:id').get( /*async*/ function (req, res, next) {
    var id = req.params.id;
    //Communicate with MYCM

    //Mongoose query
    Order.findById(id, async function (err, order) {
        if (!order) {
            return next(res.status(404).json({
                Error: 'Order not found'
            }));
        } else if (err) {
            return next(res.status(500).json({
                Error: 'An unexpected error occurred. Please try again'
            }));
        } else {
            var orderContentsList = order.orderContents;

            var result = await fetchOrderContents(orderContentsList);
            var detailedOrder = {
                status: order.status,
                orderContents: result
            };
            res.status(200).json(detailedOrder);
        }
    });
})

//Fetches an orders contents
async function fetchOrderContents(orderContents) {

    var customizedProductArray = [];

    var orderContentsSize = orderContents.length;

    for (var i = 0; i < orderContentsSize; i++) {

        var currentOrderContent = orderContents[i];
        var currentOrderContentCustomizedProductId = currentOrderContent.customizedproduct;
        var currentOrderContentCustomizedProductQuantity = currentOrderContent.quantity;
        var customizedProduct = await getCustomizedProduct(currentOrderContentCustomizedProductId);
        customizedProduct.quantity = currentOrderContentCustomizedProductQuantity;
        customizedProductArray.push(customizedProduct);
    }

    return customizedProductArray;
}

//Fetches a CustomizedProduct DTO by making a GET Request to MYCMs API
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

//Creates a new Order
//TODO Handle errors by using the ones available in the mongoose schema
ordersRoute.route('/orders').post(function (req, res, next) {
    Order.create({
            orderContents: req.body.orderContents,
        },
        function (error, order) {
            if (error) {
                res.status(400).json({
                    Error: 'The order body is invalid. Please try again'
                });
            } else {
                res.status(201).json(order);
            }
        })
})


module.exports = ordersRoute;