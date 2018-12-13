//Integration Tests for Orders Route
const {
    MongoClient
} = require('mongodb');
const axios = require('axios');


describe('Tests with city and factory setup', () => {
    let connection;
    let db;
    var cityID;

    beforeEach(async () => {
        connection = await MongoClient.connect(global.__MONGO_URI__, {
            useNewUrlParser: true
        });
        db = await connection.db(global.__MONGO_DB_NAME__);
        var cityReq = {
            "name": "Porto",
            "latitude": 41.1496100,
            "longitude": -8.6109900
        };
        await axios.post('http://localhost:4001/myco/api/cities', cityReq).then((response) => {
            cityID = response.data.id;
        }).catch((data1) => {
        });
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        await axios.post('http://localhost:4001/myco/api/factories', factReq).then((data) => {
        }).catch((data) => {
        });
    });

    afterEach(async () => {
        await connection.close();
        await db.close();
    });



    /*
        it('should insert an order into collection', async () => {
            expect(db).toBeDefined();
            const orders = await db.collection('orders');
    
            const mockOrder = ({
                _id: "orderId",
                orderContents: [{
                    customizedproduct: 1,
                    quantity: 1
                }, {
                    customizedproduct: 2,
                    quantity: 2
                }]
            });
            await orders.insertOne(mockOrder);
    
            const insertedOrder = await orders.findOne({ _id: 'orderId' });
            expect(insertedOrder).toEqual(mockOrder);
        });*/

    /*it('Get all orders should return 404 if no orders exist', done => {

        return axios.get('http://localhost:4001/myco/api/orders').then((data) => {
            expect(true).toBeFalsy();
        }).catch((data) => {
            expect(data.response.status).toBe(404);
            done();
        });
    });*/
    it('Get all orders should return all orders in the database', done => {
        console.log('ENTER TEST');
        return axios.get('http://localhost:4001/myco/api/orders').then((data) => {
            expect(true).toBeFalsy();
        }).catch((res) => {
            console.log('NO ORDERS FOUND AS EXPECTED Status: ' + res.response.status);
            expect(res.response.status).toBe(404);
            var postOrdersReq = {
                "orderContents": [
                    {
                        "customizedproduct": 1,
                        "quantity": 1
                    }
                ],
                "cityToDeliverId": cityID
            };
            console.log(postOrdersReq);
            console.log('POST PREPARED');
            axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
                console('POST SUCCESS');
                console.log('POSTED ORDERS Status: ' + postRes);
                axios.get('http://localhost:4001/myco/api/orders').then((data) => {
                    console.log('2nd GET FAILED');
                    expect(true).toBeFalsy();
                }).catch((getRes) => {
                    console.log('I GOT HERE');
                    console.log(getRes.response.data);
                    expect(getRes.response.status).toBe(200);
                    expect(getRes.response.data).toBe([
                        {
                            "id": 1,
                            "reference": "#Fabrica Alentejo"
                        }
                    ]);
                    done();
                })
            }).catch((postRes) => {
                console.log('TOU? ' + postRes);
                expect(true).toBeFalsy();
            });
        });
    });
    /*
    it('Get order by id should fail if no such order exists', done => {
        return axios.get('http://localhost:4001/myco/api/orders/' + '1').then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(404);
            done();
        })
    });*/
    /*
    it('Get order by id should succeeds if order exists' ,done => {
        return axios.get('http://localhost:4001/myco/api/orders/' + '1').then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(404);
            var postOrdersReq = {
                "orderContents": [
                    {
                        "customizedproduct": 1,
                        "quantity": 1
                    }
                ],
                "cityToDeliverId": cityID
            };
            axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes)=>{
                axios.get('http://localhost:4001/myco/api/orders/' + '1').then((getIDRes)=>{
                    expect(getIDRes.response.status).toBe(200);
                    done();
                }).catch(()=>{
                    expect(true).toBeFalsy();
                })
            })
        })
    });*/
    /*
    it('POST order should fail if city does not exist', done => {
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": -1
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((postRes) => {
            expect(postRes.response.status).toBe(400);
            done();
        });
    });*/
    /*
    it('POST order should fail if any of the customized products do not exist', done => (){
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                },
                {
                    "customizedproduct": 2,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((postRes) => {
            expect(postRes.response.status).toBe(404);
            done();
        });
    });*/
    /*
    it('POST order should succeed if data is correct', done => (){
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                },
                {
                    "customizedproduct": 2,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            done();
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    })*/
    /*
    it('PUT state should fail if order does not exist', done => {
        var putReq = {
            "state": "In Production"
        };
        return axios.put('http://localhost:4001/myco/api/orders/' + 1 + '/state', putReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((putRes) => {
            expect(putRes.response.status).toBe(500);
        });
    });
    */
    /*
     it('PUT state should fail if state is not valid', done => {
         var postOrdersReq = {
             "orderContents": [
                 {
                     "customizedproduct": 1,
                     "quantity": 1
                 },
                 {
                     "customizedproduct": 2,
                     "quantity": 1
                 }
             ],
             "cityToDeliverId": cityID
         };
         return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
             expect(postRes.response.status).toBe(201);
             var putReq = {
                 "state": ""
             };
             axios.put('http://localhost:4001/myco/api/orders/' + 1 + '/state', putReq).then(() => {
                 expect(true).toBeFalsy();
             }).catch((putRes) => {
                 expect(putRes.response.status).toBe(400);
                 done();
             });
         }).catch(() => {
             expect(true).toBeFalsy();
         });
     });
     */
    /*
    it('PUT state should fail if order cant pass from current state to new one',done =>{
        //TODO: IMPLEMENT THIS VALIDATION
    });
    */
    /*
     it('PUT state should succeed', done => {
         var postOrdersReq = {
             "orderContents": [
                 {
                     "customizedproduct": 1,
                     "quantity": 1
                 },
                 {
                     "customizedproduct": 2,
                     "quantity": 1
                 }
             ],
             "cityToDeliverId": cityID
         };
         return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
             expect(postRes.response.status).toBe(201);
             var putReq = {
                 "state": "Validated"
             };
             axios.put('http://localhost:4001/myco/api/orders/' + 1 + '/state', putReq).then(() => {
                 expect(putRes.response.status).toBe(200);
                 done();
             }).catch((putRes) => {
                 expect(true).toBeFalsy();
             });
         }).catch(() => {
             expect(true).toBeFalsy();
         });
     });
     */
    /*
    it('PATCH packages should fail if order does not exist', done => {
        var patchReq = [
            {
                "size": "S",
                "count": 5
            },
            {
                "size": "M",
                "count": 4
            },
            {
                "size": "L",
                "count": 1
            }
        ]
        return axios.patch('http://localhost:4001/myco/api/orders/' + 1 + '/packages').then(() => {
            expect(true).toBeFalsy();
        }).catch((patchRes) => {
            expect(patchRes.response.status).toBe(500);
        });
    });
    */
    /*
    it('PATCH packages should fail if package size is invalid', done => {
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                },
                {
                    "customizedproduct": 2,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            var patchReq = [
                {
                    "size": "IT WAS I ",
                    "count": 5
                },
                {
                    "size": "M",
                    "count": 4
                },
                {
                    "size": "L",
                    "count": 1
                }
            ]
            return axios.patch('http://localhost:4001/myco/api/orders/' + 1 + '/packages').then(() => {
                expect(true).toBeFalsy();
            }).catch((patchRes) => {
                expect(patchRes.response.status).toBe(400);
            });
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    */
    /*
    it('PATCH packages should fail if package count is less than 1', done => {
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                },
                {
                    "customizedproduct": 2,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            var patchReq = [
                {
                    "size": "S",
                    "count": -1
                },
                {
                    "size": "M",
                    "count": 4
                },
                {
                    "size": "L",
                    "count": 1
                }
            ]
            return axios.patch('http://localhost:4001/myco/api/orders/' + 1 + '/packages').then(() => {
                expect(true).toBeFalsy();
            }).catch((patchRes) => {
                expect(patchRes.response.status).toBe(400);
            });
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    */
    /*
    it('PATCH packages should succeed if data is correct', done => {
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                },
                {
                    "customizedproduct": 2,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            var patchReq = [
                {
                    "size": "S",
                    "count": 5
                },
                {
                    "size": "M",
                    "count": 4
                },
                {
                    "size": "L",
                    "count": 1
                }
            ]
            return axios.patch('http://localhost:4001/myco/api/orders/' + 1 + '/packages').then(() => {
                expect(patchRes.response.status).toBe(200);
            }).catch((patchRes) => {
                expect(true).toBeFalsy();
            });
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    */
})
/*
describe('no factory initialization', () => {
    let connection;
    let db;
    var cityID;

    beforeEach(async () => {
        connection = await MongoClient.connect(global.__MONGO_URI__, {
            useNewUrlParser: true
        });
        db = await connection.db(global.__MONGO_DB_NAME__);
        var cityReq = {
            "name": "Porto",
            "latitude": 41.1496100,
            "longitude": -8.6109900
        };
        await axios.post('http://localhost:4001/myco/api/cities', cityReq).then((response) => {
            cityID = response.data.id;
        }).catch((data1) => {
        });
    });

    afterEach(async () => {
        await connection.close();
        await db.close();
    });
    it('POST order should fail if there are no factories', done => {
        var postOrdersReq = {
            "orderContents": [
                {
                    "customizedproduct": 1,
                    "quantity": 1
                }
            ],
            "cityToDeliverId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/orders', postOrdersReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((postRes) => {
            expect(postRes.response.status).toBe(500);
            done();
        });
    });
    */
})


/*
describe('noDB', () => {
    it('Get all orders should return 500 if connection is not established', done => {
        return axios.get('http://localhost:4001/myco/api/orders').then((data) => {
            expect(true).toBeFalsy();
        }).catch((data) => {
            expect(data.response.status).toBe(500);
            done();
        });
    });
})*/