//Integration Tests for Orders Route
const {
    MongoClient
} = require('mongodb');
const axios = require('axios');


describe('Tests with city setup', () => {
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

    it('Get all should fail if no factories exist', done => {
        return axios.get('http://localhost:4001/myco/api/factories').then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(404);
            done();
        });
    });
    it('Get all should succeed if at least one factory exists', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
            expect(postRrs.response.data).toBe(201);
            axios.get('http://localhost:4001/myco/api/factories').then(() => {
                expect(getRes.response.status).toBe(200);
                done();
            }).catch((getRes) => {
                expect(true).toBeFalsy();
            });
        }).catch((data) => {
            expect(true).toBeFalsy();
        });
    });
    it('Get by id should fail if there is not a factory with provided id', done => {
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            done();
        });
    });
    it('Get by id should succeed if a factory with provided id exists', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
            expect(postRes.response.data).toBe(201);
            var facID = postRes.response.data.id;
            axios.get('http://localhost:4001/myco/api/factories/' + facID).then((getRes) => {
                expect(getRes.response.status).toBe(200);
                done();
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    it('Post factory should succeed if city does not exist', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID + 1
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    done();
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Post factory should fail if reference is duplicated', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.post('http://localhost:4001/myco/api/factories', factReq).then(() => {
                        expect(true).toBeFalsy();
                    }).catch((post2Res) => {
                        expect(post2Res.response.status).toBe(500);
                        done();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Post factory should fail if reference is invalid', done => {
        var factReq = {
            "reference": "",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((postRes) => {
                expect(postRes.response.data).toBe(500);
                done();
            });
        });
    });
    it('Post factory should fail if designation is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((postRes) => {
                expect(postRes.response.data).toBe(500);
                done();
            });
        });
    });
    it('Post factory should fail if latitude is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 555550.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((postRes) => {
                expect(postRes.response.data).toBe(500);
                done();
            });
        });
    });
    it('Post factory should fail if longitude is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 2300.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((postRes) => {
                expect(postRes.response.data).toBe(500);
                done();
            });
        });
    });
    it('Post factory should succeed if data is correct', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    done();
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should fail if factory does not exist', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.put('http://localhost:4001/myco/api/factories/' + 1, factReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((putRes) => {
                expect(putRes.response.data), toBe(400);
                done();
            })
        });
    });
    it('Put factory properties should fail if reference is duplicated', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        var factReq2 = {
            "reference": "#Nova Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 51.55335,
            "longitude": 24.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.post('http://localhost:4001/myco/api/factories', factReq2).then(() => {
                        expect(post2Res.response.status).toBe(201);
                        axios.get('http://localhost:4001/myco/api/factories/' + 2).then((get3Res) => {
                            expect(get3Res.response.status).toBe(200);
                            axios.put('http://localhost:4001/myco/api/factories/' + 2, factReq).then(() => {
                                expect(true).toBeFalsy();
                            }).catch((putRes) => {
                                expect(putRes.response.status).toBe(400);
                                done();
                            })
                        }).catch(() => {
                            expect(true).toBeFalsy();
                        })
                    }).catch((post2Res) => {
                        expect(true).toBeFalsy();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should fail if reference is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        var factPutReq = {
            "reference": "",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.put('http://localhost:4001/myco/api/factories/' + 1, factPutReq).then(() => {
                        expect(true).toBeFalsy();
                    }).catch((putRes) => {
                        expect(putRes.response.data), toBe(400);
                        done();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should fail if designation is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        var factPutReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.put('http://localhost:4001/myco/api/factories/' + 1, factPutReq).then(() => {
                        expect(true).toBeFalsy();
                    }).catch((putRes) => {
                        expect(putRes.response.data), toBe(400);
                        done();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should fail if latitude is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        var factPutReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50000.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.put('http://localhost:4001/myco/api/factories/' + 1, factPutReq).then(() => {
                        expect(true).toBeFalsy();
                    }).catch((putRes) => {
                        expect(putRes.response.data), toBe(400);
                        done();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should fail if longitude is invalid', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        var factPutReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 230000.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.put('http://localhost:4001/myco/api/factories/' + 1, factPutReq).then(() => {
                        expect(true).toBeFalsy();
                    }).catch((putRes) => {
                        expect(putRes.response.data), toBe(400);
                        done();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Put factory properties should succeed if data is correct', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.put('http://localhost:4001/myco/api/factories/' + 1, factReq).then((putRes) => {
                        expect(putRes.response.data), toBe(200);
                        done();
                    }).catch(() => {
                        expect(true).toBeFalsy();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
    it('Delete factory should fail if factory does not exist', done => {
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.delete('http://localhost:4001/myco/api/factories/' + 1).then(() => {
                expect(true).toBeFalsy();
            }).catch((delRes) => {
                expect(delRes.response.status).toBe(400);
                done();
            });
        });
    });
    it('Delete factory should succeed if factory exists', done => {
        var factReq = {
            "reference": "#Fabrica Alentejo",
            "designation": "Fabrica Velha",
            "latitude": 50.55335,
            "longitude": 23.030512,
            "cityId": cityID
        };
        return axios.get('http://localhost:4001/myco/api/factories/' + 1).then(() => {
            expect(true).toBeFalsy();
        }).catch((getRes) => {
            expect(getRes.response.status).toBe(400);
            axios.post('http://localhost:4001/myco/api/factories', factReq).then((postRes) => {
                expect(postRes.response.data).toBe(201);
                axios.get('http://localhost:4001/myco/api/factories/' + 1).then((get2Res) => {
                    expect(get2Res.response.status).toBe(200);
                    axios.delete('http://localhost:4001/myco/api/factories/' + 1).then((delRes) => {
                        expect(delRes.response.status).toBe(204);
                        done();
                    }).catch(() => {
                        expect(true).toBeFalsy();
                    })
                }).catch(() => {
                    expect(true).toBeFalsy();
                })
            }).catch(() => {
                expect(true).toBeFalsy();
            });
        });
    });
})