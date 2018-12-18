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
    canvasControls: {
        canMoveCloset: "false",
        canMoveSlots: "false",
        canMoveComponents: "false",
        componentToRemove: {}
    },

    product: {},

    customizedProduct: {
        designation: "",
        reference: "",
        components: [],
        product: {
            id: ""
        },
        customizedMaterial: {
            id: "",
            reference: "",
            designation: "",
            image: ""
        },
        slots: [
        ],
        customizedDimensions: {
            width: "",
            height: "",
            depth: "",
            unit: ""
        },
        components: []
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
    customizedProductDimensions: state => {
        return state.customizedProduct.customizedDimensions;
    },
    depth: state => {
        return state.customizedProduct.dimensions.depth;
    },
    unit: state => {
        return state.customizedProduct.dimensions.unit;
    },
    customizedProductSlotWidth: state => index => {
        return state.customizedProduct.slots[index];
    },
    customizedProductComponents: state => {
        return state.customizedProduct.components;
    },
    customizedMaterial: state => {
        return state.customizedProduct.customizedMaterial.image;
    },
    canMoveCloset: state => {
        return state.canvasControls.canMoveCloset;
    },
    canMoveSlots: state => {
        return state.canvasControls.canMoveSlots;
    },
    canMoveComponents: state => {
        return state.canvasControls.canMoveComponents;
    },
    componentToRemove: state => {
        return state.canvasControls.componentToRemove;
    }
}

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})