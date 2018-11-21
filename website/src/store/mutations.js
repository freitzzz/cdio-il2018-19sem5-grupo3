import * as types from './mutation-types'

export const mutations = {

    /**
     * Mutation used to save the retrieved product from MYCM into the state's product
     * @param {*} state The store's state
     * @param {*} payload Received payload with the product info
     */
    [types.INIT_PRODUCT](state, payload) {
        state.product = payload.product;
    }
}