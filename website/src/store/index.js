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
        slots: [],
        customizedDimensions: {
            width: "",
            height: "",
            depth: "",
            unit: ""
        }
    }
}

export const getters = {
    productId: state => {
        return state.product.id;
    },
    productDimensions: state => {
        return state.product.dimensions;
    },
    productSlotWidths: state => {
        return state.product.slotWidths;
    },
    recommendedSlotWidth: state => {
        return state.product.slotWidths.recommendedWidth;
    }, 
    maxSlotWidth: state => {
        return state.product.slotWidths.maxWidth;
    },
    minSlotWidth: state => {
        return state.product.slotWidths.minWidth;
    },
    productMaterials: state => {
        return state.product.materials;
    },
    productComponents: state => {
        return state.product.components;
    },
     width: state => {
        return state.customizedProduct.dimensions.width;
    },
    height: state => {
        return state.customizedProduct.dimensions.height;
    },
    depth: state => {
        return state.customizedProduct.dimensions.depth;
    },
    unit: state => {
        return state.customizedProduct.dimensions.unit;
    },
    customizedProductSlotWidth: state => {
        return state.customizedProduct.slots.width;
    }
}

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})