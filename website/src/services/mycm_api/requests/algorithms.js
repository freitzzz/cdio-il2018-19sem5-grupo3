//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const ALGORITHMS_URL = `${MYCM_API_URL}/algorithms`;


export default {

    /**
     * Retrieves all of the available Algorithms.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available algorithms.
     */
    getAlgorithms() {
        return Axios.get(`${ALGORITHMS_URL}`);
    },

    /**
     * Retrieves the Algorithm with a matching identifier.
     * @param {*} algorithmId - Algorithm's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Algorithm.
     */
    getAlgorithm(algorithmId) {
        return Axios.get(`${ALGORITHMS_URL}/${algorithmId}`);
    },

    /**
     * Retrieves the Algorithm's inputs.
     * @param {*} algorithmId - Algorithm's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the Algorithm's inputs.
     */
    getAlgorithmInputs(algorithmId) {
        return Axios.get(`${ALGORITHMS_URL}/${algorithmId}/inputs`);
    }
}