const express = require('express');
const ordersRoute = express.Router();
const Order = require('../models/Order');
const Factory = require('../models/Factory');
const http = require('http');
const City = require('../models/City');
const axios = require('axios');
const config = require('../config');

//Get all orders in the database
//Handle errors by using the ones available in the mongoose schema
ordersRoute.route('/orders').get(function (req, res) {
    Order.find()
    .then(function(orders) {
        if (orders == null || orders.length == 0) {
            res.status(404).json({
                Error: 'No orders found'
            });
        } else {
            res.header("Access-Control-Allow-Origin", "*");
            res.status(200).json(orders); //return all orders
        }
    }).catch(() => {
        res.status(500).json({
            Error: 'An unexpected error occurred. Please try again'
        })
    });
})

//Gets an order and its details by its id
//TODO Handle errors by using the ones available in the mongoose schema
//TODO add query to route path to know if the order has to be detailed or not
ordersRoute.route('/orders/:id').get(function (req, res, next) {
    const id = req.params.id;
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

            try{
                var result = await fetchOrderContents(orderContentsList);

                var detailedOrder = {
                    status: order.status,
                    orderContents: result
                };

                res.status(200).json(detailedOrder);
            }catch(error){
                res.status(error.response.status).json(error.response.data)
            }
        }
    });
})

//Fetches an orders contents
async function fetchOrderContents(orderContents) {

    var customizedProductArray = [];
    var orderContentsSize = orderContents.length;
    var requests = [];
    var quantities = [];

    for (let i = 0; i < orderContentsSize; i++) {

        var currentOrderContent = orderContents[i];
        var currentOrderContentCustomizedProductId = currentOrderContent.customizedproduct;
        var currentOrderContentCustomizedProductQuantity = currentOrderContent.quantity;
        quantities.push(currentOrderContentCustomizedProductQuantity);
        const requestUrl = `${config.MYCM_URL}mycm/api/customizedproducts/${currentOrderContentCustomizedProductId}`;
        requests.push(axios.get(requestUrl));
    }

    try{
        const responses = await Promise.all(requests);

        for(let i = 0; i < responses.length; i++){
            var customizedProduct = responses[i].data;
            customizedProduct.quantity = quantities[i];
            customizedProductArray.push(customizedProduct);
        }

    }catch(error){
        throw error
    }

    return customizedProductArray;
}

//Creates a new Order
//TODO Handle errors by using the ones available in the mongoose schema
ordersRoute.route('/orders').post(function (req, res, next) {
    City.findById(req.body.cityToDeliverId)
        .then((city) => {
            Factory
                .find()
                .then(async function (availableFactories) {
                    isCityInFactories(city, availableFactories)
                        .then((_shortestFactory) => {
                            checkIfProductsExist(req.body.orderContents)
                                .then(() => {
                                    createOrder(req.body.orderContents, city, _shortestFactory)
                                        .then((_createdOrder) => {
                                            res.status(201).json(_createdOrder);
                                        }).catch((_error) => {
                                            //TODO: REWORK : )
                                            res.status(500).json({ message: _error });
                                        });
                                }).catch((message) => {
                                    res.status(404).json({
                                        message: message
                                    });
                                });
                        })
                        .catch(() => {
                            fetchShortestFactory(city, availableFactories)
                                .then((_shortestFactory) => {
                                    checkIfProductsExist(req.body.orderContents).then(() => {
                                        createOrder(req.body.orderContents, city, _shortestFactory)
                                            .then((_createdOrder) => {
                                                res.status(201).json(_createdOrder);
                                            }).catch((_error) => {
                                                //TODO: REWORK : )
                                                res.status(500).json({ message: 'An internal error occurred while creating the order' });
                                            });
                                    }).catch((message) => {
                                        res.status(404).json({
                                            message: message
                                        });
                                    });
                                }).catch((_error) => {
                                    //TODO: REWORK :) (Happens when there is an error while processing the shortest factory computation in MYCL)
                                    res.status(500).json({ message: 'There are no cities available' });
                                });
                        });
                });
        })
        .catch((_error) => {
            res.status(400).json({ message: 'There is no city with the given id' });
        });
});

/**
 * Routes the update state of an order request
 */
ordersRoute.route('/orders/:id/state').put((request, response) => {
    let orderID = request.params.id;
    orderExists(orderID)
        .then((foundOrder) => {
            changeOrderState(foundOrder, request.body.state)
                .then((changedOrderState) => {
                    Order
                        .findByIdAndUpdate(orderID, changedOrderState, { new: true })
                        .then((updatedOrder) => {
                            response.status(200).json(updatedOrder);
                        })
                        .catch(() => {
                            response.status(500).json({ message: "An error occurd while processing our database :(" });
                        });
                })
                .catch((_error_message) => {
                    response.status(400).json({ message: _error_message });
                })
        })
        .catch((_error_message) => {
            response.status(500).json({ message: _error_message });
        });
});
/**
 * Routes the register the packages of an order request
 */
ordersRoute.route('/orders/:id/packages').patch((request, response) => {
    let orderID = request.params.id;
    orderExists(orderID)
        .then((foundOrder) => {
            registerOrderPackages(foundOrder, request.body)
                .then((registeredOrderPackages) => {
                    Order
                        .findByIdAndUpdate(orderID, registeredOrderPackages, { new: true })
                        .then((updatedOrder) => {
                            response.status(200).json(updatedOrder);
                        })
                        .catch(() => {
                            response.status(500).json({ message: "An error occurd while processing our database :(" });
                        })
                })
                .catch((_error_message) => {
                    response.status(400).json({ message: _error_message });
                })
        })
        .catch((_error_message) => {
            response.status(500).json({ message: _error_message });
        });
});

/**
 * Checks if an order exists, and if so returns it as a callback function
 * @param {String} orderId String with the order persistence id
 */
function orderExists(orderId) {
    return new Promise((resolve, reject) => {
        Order
            .findById(orderId)
            .then((foundOrder) => {
                foundOrder != null ? resolve(foundOrder) : reject("No Order found with the given id")
            })
            .catch(() => {
                reject("An error occurd while processing our data base :(");
            });
    });
}

/**
 * Verifies if a city is located in a collection of factories
 * @param {City.Schema} city City with the city being verified
 * @param {List} factories Collection with the factories being checked
 * @returns Promise with the condition information
 */
function isCityInFactories(city, factories) {
    return new Promise((resolve, reject) => {
        let _factories = [];
        factories.forEach((factory) => {
            if (factory.isLocated(city)) { resolve(factory); }
            //TODO: REWORK : )
        })
        return (_factories != null && _factories.length != 0) ? resolve(_factories) : reject(null);
    });
}

/**
 * Creates an order
 * @param {List} orderContents Collection with the order contents
 * @param {City.Schema} cityToDeliver City with the city to deliver the order
 * @param {Factory.Schema} factoryOfProduction Factory with the factory where the order will be produced
 * @returns Order with the created order
 */
function createOrder(orderContents, cityToDeliver, factoryOfProduction) {
    return Order.create({
        orderContents: orderContents,
        cityToDeliver: cityToDeliver,
        factoryOfProduction: factoryOfProduction
    });
}

/**
 * Changes the state of an order
 * @param {Order.Schema} order Order with the order being changed the state 
 * @param {State} orderState State with the state to update
 */
function changeOrderState(order, orderState) {
    return new Promise((updatedOrderState, errorUpdatingOrderState) => {
        try {
            order.changeState(orderState);
            updatedOrderState(order);
        } catch (_error) {
            errorUpdatingOrderState(_error);
        }
    });
}

/**
 * Registers the packages of an order
 * @param {Order.Schema} order Order with the order being registered the packages
 * @param {Array} packages Array with the packages information
 */
function registerOrderPackages(order, packages) {
    return new Promise((registeredOrderPackages, errorRegisteringOrderPackages) => {
        try {
            order.registerPackages(packages);
            registeredOrderPackages(order);
        } catch (_error) {
            errorRegisteringOrderPackages(_error);
        }
    });
}

/**
 * Checks if all products within orderContents exist
 * @param {List} orderContents Collection with the order contents
 * @returns true if all products exist, false if not
 */
function checkIfProductsExist(orderContents) {
    return new Promise((resolve, reject) => {
        let iteration = 0;
        for (let i = 0; i < orderContents.length; i++) {
            axios.get(config.MYCM_URL + "mycm/api/customizedproducts/" + orderContents[i].customizedproduct)
                .then(() => {
                    iteration++;
                }).catch((asd) => {
                    reject("One of the customized products does not exist!");
                }).then(() => {
                    if (iteration == orderContents.length) {
                        resolve();
                    }
                })
        }
    });
}

/**
 * Fetches the shortest factory between a city
 * @param {City.Schema} city City with the order delivery city
 * @param {List} factories Collection with the available production factories
 * @returns Promise with the fetch callback
 */
async function fetchShortestFactory(city, factories) {
    return new Promise((resolve, reject) => {
        let _requestBody = { city: serializeCity(city), factories: serializeFactories(factories) };
        axios.post(config.MYCL_URL + "mycl/api/factories", _requestBody)
            .then(function (response) {
                resolve(factoryByName(factories, response.data.factory.name));
            })
            .catch(function (_error) {
                reject(_error);
            });
    })
}

/**
 * Serializes a city into a basic city
 * @param {City.Schema} city City with the city being serialized
 */
function serializeCity(city) {
    return {
        name: city.name,
        latitude: city.location.latitude,
        longitude: city.location.longitude
    }
}

/**
 * Serializes a factory into a basic factory
 * @param {Factory.Schema} factory Factory with the factory being serialized
 */
function serializeFactory(factory) {
    return {
        name: factory.reference,
        latitude: factory.location.latitude,
        longitude: factory.location.longitude,
        available: factory.available
    }
}

/**
 * Serializes a collection of factories
 * @param {List} factories Collection with the factories being serialized 
 */
function serializeFactories(factories) {
    let serializedFactories = [];
    factories.forEach((factory) => { serializedFactories.push(serializeFactory(factory)); });
    return serializedFactories;
}

/**
 * Fetches a factory in a collection by its name
 * @param {List} factories Collection with the factories
 * @param {String} name  String with the factory name being fetched
 */
function factoryByName(factories, name) {
    for (let i = 0; i < factories.length; i++)
        if (factories[i].reference == name) return factories[i];
}

/**
 * Exports orders router
 */
module.exports = ordersRoute;