//Integration Tests for Orders Route
const {
    MongoClient
} = require('mongodb');

describe('insert', () => {

    let connection;
    let db;

    beforeAll(async () => {
        connection = await MongoClient.connect(global.__MONGO_URI__, {
            useNewUrlParser: true
        });
        db = await connection.db(global.__MONGO_DB_NAME__);
    });

    afterAll(async () => {
        await connection.close();
        await db.close();
    });

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

        const insertedOrder = await orders.findOne({_id: 'orderId'});
        expect(insertedOrder).toEqual(mockOrder);
    });

})