const express = require('express');
const ordersRoute = express.Router();
const Order = require('../models/order');
const Factory=require('../models/Factory');
const http = require('http');
const City = require('../models/City');
const axios=require('axios');
const config=require('../config');

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


function get_shortest_factory_between_city(city){
    Factory
        .find()
            .then(function(availableFactories){
                availableFactories.forEach(function(factory){
                    if(factory.isLocated(city)){
                        return city;
                    }
                })
            })
}

//Creates a new Order
//TODO Handle errors by using the ones available in the mongoose schema
ordersRoute.route('/orders').post(function (req, res, next) {
    City.findById(req.body.cityToDeliverId).then(function (city,error) {
        if (error) {
            res.status(404).json({
                Error: 'City not found. Please try again'
            });

        } else {
            Factory
                .find()
                    .then(async function(availableFactories){
                        isCityInFactories(city,availableFactories)
                        .then((_shortestFactory)=>{
                            createOrder(req.body.orderContents,city,_shortestFactory)
                            .then((_createdOrder)=>{
                                res.status(201).json(_createdOrder);
                            }).catch((_error)=>{
                                res.status(420).json(_error);
                            });
                        })
                        .catch(()=>{
                            fetchShortestFactory(city,availableFactories)
                            .then((_shortestFactory)=>{
                                createOrder(req.body.orderContents,city,_shortestFactory)
                                .then((_createdOrder)=>{
                                    res.status(201).json(_createdOrder);
                                }).catch((_error)=>{
                                    res.status(401).json(_error);
                                });
                            }).catch((_error)=>{
                                res.status(402).json(_error);
                            });
                        });                        
                    });
        }
    })
})

/**
 * Verifies if a city is located in a collection of factories
 * @param {City.Schema} city City with the city being verified
 * @param {List} factories Collection with the factories being checked
 * @returns Promise with the condition information
 */
function isCityInFactories(city,factories){
    return new Promise((resolve,reject)=>{
        let _factories=[];
        factories.forEach((factory)=>{
            if(factory.isLocated(city)){resolve(factory);}
            //TODO: REWORK : )
        })
        return _factories ? resolve(_factories) : reject(null);
    });
}

/**
 * Creates an order
 * @param {List} orderContents Collection with the order contents
 * @param {City.Schema} cityToDeliver City with the city to deliver the order
 * @param {Factory.Schema} factoryOfProduction Factory with the factory where the order will be produced
 * @returns Order with the created order
 */
function createOrder(orderContents,cityToDeliver,factoryOfProduction){
    return Order.create({
        orderContents:orderContents,
        cityToDeliver:cityToDeliver,
        factoryOfProduction:factoryOfProduction
    });
}

async function fetchShortestFactory(city,factories){
    return new Promise((resolve,reject)=>{
        let _requestBody={city:serializeCity(city),factories:serializeFactories(factories)};
        axios.post(config.MYCL_URL+"mycl/api/factories",_requestBody)
            .then(function(response){
                resolve(factoryByName(factories,response.data.factory.name));
            })
            .catch(function(_error){
                reject(_error);
            });
    })
}

function serializeCity(city){
    return {
        name:city.name,
        latitude:city.location.latitude,
        longitude:city.location.longitude
    }
}

function serializeFactory(factory){
    return {
        name:factory.reference,
        latitude:factory.location.latitude,
        longitude:factory.location.longitude
    }
}

function serializeFactories(factories){
    let serializedFactories=[];
    factories.forEach((factory)=>{serializedFactories.push(serializeFactory(factory));});
    return serializedFactories;
}

function factoryByName(factories,name){
    for(let i=0;i<factories.length;i++)
        if(factories[i].reference==name)return factories[i];
}

module.exports = ordersRoute;