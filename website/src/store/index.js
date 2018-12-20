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
        id: "",
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
            image: "",
            finish: {
                description: "",
                shininess: ""
            },
            color: {
                name: "",
                red: "",
                green: "",
                blue: "",
                alpha: ""
            }
        },
        slots: [
        ],
        customizedDimensions: {
            width: "",
            height: "",
            depth: "",
            unit: ""
        },
    },

    resizeFactorDimensions: {
        width: "",
        height: "",
        depth: "",
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
    customizedProductId: state => {
        return state.customizedProduct.id;
    },
    customizedProductDimensions: state => {
        return state.customizedProduct.customizedDimensions;
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
    customizedMaterialColor: state => {
        return state.customizedProduct.customizedMaterial.color.red + "-" +
        state.customizedProduct.customizedMaterial.color.green + "-" +
        state.customizedProduct.customizedMaterial.color.blue + "-" +
        state.customizedProduct.customizedMaterial.color.alpha;
    },
    customizedMaterialColorName: state => {
        return state.customizedProduct.customizedMaterial.color.name;
    },
    customizedMaterialFinish: state => {
        return state.customizedProduct.customizedMaterial.finish.shininess;
    },
    customizedMaterialFinishDescription: state => {
        return state.customizedProduct.customizedMaterial.finish.description;
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