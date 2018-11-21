import 'es6-promise/auto'
import Vue from 'vue'
import Vuex from 'vuex'
import { mutations } from './mutations'
import { actions } from './actions'

Vue.use(Vuex)

/**
 * State that contains the retrieved product from MYCM and the builded customized product
 */
const state = {
    product: {},
    customizedProduct: {
        customizedDimensions: {
            width: "",
            height: "",
            depth: ""
        }
    }
}

const getters = {
    width: state => {
        return state.customizedProduct.customizedDimensions.width;
    },
    height: state => {
        return state.customizedProduct.customizedDimensions.height;
    },
    depth: state => {
        return state.customizedProduct.customizedDimensions.depth;
    }
}

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})