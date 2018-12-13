//Integration Tests for Orders Route
const {
    MongoClient
} = require('mongodb');
const axios = require('axios');


describe('Tests', () => {
    let connection;
    let db;

    beforeEach(async () => {
        connection = await MongoClient.connect(global.__MONGO_URI__, {
            useNewUrlParser: true
        });
        db = await connection.db(global.__MONGO_DB_NAME__);
    });

    afterEach(async () => {
        await connection.close();
        await db.close();
    });

    it('Post city should succeed if data is correct', done => {
        var cityReq = {
            "name": "Porto",
            "latitude": 41.1496100,
            "longitude": -8.6109900
        }
        return axios.post('http://localhost:4001/myco/api/cities', cityReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            done();
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    it('Post city should fail if city already exists in the provided location', done => {
        var cityReq = {
            "name": "Porto",
            "latitude": 41.1496100,
            "longitude": -8.6109900
        }
        return axios.post('http://localhost:4001/myco/api/cities', cityReq).then((postRes) => {
            expect(postRes.response.status).toBe(201);
            axios.post('http://localhost:4001/myco/api/cities', cityReq).then(() => {
                expect(true).toBeFalsy();
            }).catch((post2Res) => {
                expect(post2Res.response.status).toBe(400);
                done();
            })
        }).catch(() => {
            expect(true).toBeFalsy();
        });
    });
    it('Post city should fail if latitude is invalid', done => {
        var cityReq = {
            "name": "Porto",
            "latitude": 4111111.1496100,
            "longitude": -8.6109900
        }
        axios.post('http://localhost:4001/myco/api/cities', cityReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((post2Res) => {
            expect(post2Res.response.status).toBe(400);
            done();
        })
    });
    it('Post city should fail if longitude is invalid', done => {
        var cityReq = {
            "name": "Porto",
            "latitude": 41.1496100,
            "longitude": -811111.6109900
        }
        axios.post('http://localhost:4001/myco/api/cities', cityReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((post2Res) => {
            expect(post2Res.response.status).toBe(400);
            done();
        })
    });
    it('Post city should fail if name is invalid', done => {
        var cityReq = {
            "name": "",
            "latitude": 41.1496100,
            "longitude": -8.6109900
        }
        axios.post('http://localhost:4001/myco/api/cities', cityReq).then(() => {
            expect(true).toBeFalsy();
        }).catch((post2Res) => {
            expect(post2Res.response.status).toBe(400);
            done();
        })
    })
})