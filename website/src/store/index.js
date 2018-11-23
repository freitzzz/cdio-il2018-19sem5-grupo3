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
        designation: "",
        reference: "",
        product: {
            id: ""
        },
        customizedMaterial: {
            material: {
                id: ""
            },
            color: {
                name: "",
                red: "",
                green: "",
                blue: "",
                alpha: ""
            },
            finish: {
                description: ""
            }
        },
        slots: [
            {
                depth: "",
                width: "",
                height: "",
                unit: ""
            }
        ],
        customizedDimensions: {
            width: "",
            height: "",
            depth: "",
            unit: ""
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