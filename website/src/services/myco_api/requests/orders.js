//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCO_API_URL } from "./../../../config";

const ORDERS_URL = `${MYCO_API_URL}/orders`;

export default {
    /**
     * Retrieves all of the existent orders.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the existent orders.
     */
    getOrders() {
        return Axios.get(`${ORDERS_URL}`);
    },

    /**
     * Retrieves order details of a specific order
     * @param {string} orderId id of the order to retrieve
     * @returns {AxiosPromise<any>} Axios Promise representing the orders
     */
    getOrder(orderId){
        return Axios.get(`${ORDERS_URL}/${orderId}`);
    }
}