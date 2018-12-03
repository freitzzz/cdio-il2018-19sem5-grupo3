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
                id: "",
                width: "",
                unit: "",
                components: []
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
    productId: state => {
        return state.product.id;
    },
    productDimensions: state => {
        return state.product.dimensions;
    },
    productSlotSizes: state => {
        return state.product.slotSizes;
    },
    recommendedSlotSize: state => {
        return state.product.slotSizes.recommendedSize;
    },
    maxSlotSize: state => {
        return state.product.slotSizes.maxSize;
    },
    minSlotSize: state => {
        return state.product.slotSizes.minSize;
    },
    productMaterials: state => {
        return state.product.materials;
    },
    productComponents: state => {
        return state.product.components;
    },
    width: state => {
        return state.customizedProduct.customizedDimensions.width;
    },
    height: state => {
        return state.customizedProduct.customizedDimensions.height;
    },
    depth: state => {
        return state.customizedProduct.customizedDimensions.depth;
    },
    unit: state => {
        return state.customizedPrdocut.customizedDimensions.unit;
    }
}

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})