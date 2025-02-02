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
        canMoveComponents: "false",
        canMoveCloset: "false",
        canMoveSlots: "false",
        componentToRemove: {},
        componentToEdit: {},
        componentToAdd: {},
        componentMaterial: {},
        doorsFlag: "",
        slostSlider: []
    },

    product: {},

    customizedProduct: {
        id: undefined,
        reference: "",
        designation: "",
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
        slots: [],
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
    },

    resizeVectorGlobal: {
        width: "",
        height: "",
        depth: "",
    },
    /**
     * Global User details
     */
    user:{
        /**
         * User name
         */
        name:String,
        /**
         * User Roles
         */
        roles:{
            /**
             * Boolean true if the user is an administrator
             */
            isAdministrator:Boolean,
            /**
             * Boolean true if the user is a content manager
             */
            isContentManager:Boolean,
            /**
             * Boolean true if the user is a logistic manager
             */
            isLogisticManager:Boolean
        }
    }
}

export const getters = {
    resizeVectorGlobal: state =>{
        return state.resizeVectorGlobal;
    },
    resizeFactorDimensions: state =>{
        return state.resizeFactorDimensions;
    },
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
    customizedProductReference: state => {
        return state.customizedProduct.reference;
    },
    customizedProductDesignation: state => {
        return state.customizedProduct.designation;
    },
    customizedProductDimensions: state => {
        return state.customizedProduct.customizedDimensions;
    },
    customizedProductSlot: state => index => {
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
    doorsFlag: state => {
        return state.canvasControls.doorsFlag;
    },
    componentToRemove: state => {
        return state.canvasControls.componentToRemove;
    },
    /**
     * Returns the current user details
     */
    userDetails: state=>{
        return Object.assign({},state.user);
    },
    componentToAdd: state => {
        return state.canvasControls.componentToAdd;
    },
    componentToEdit: state => {
        return state.canvasControls.componentToEdit;
    },
    componentToEditMaterial: state => {
        return state.canvasControls.componentMaterial;
    }
}

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations
})