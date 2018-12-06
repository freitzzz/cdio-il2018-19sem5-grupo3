import * as types from './mutation-types'

export const actions = {

    /**
     * Action used to commit the mutation INIT_PRODUCT
     */
    [types.INIT_PRODUCT]: ({ commit }, payload) => {
        commit(types.INIT_PRODUCT, payload)
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_WIDTH
     */
    [types.SET_CUSTOMIZED_PRODUCT_WIDTH]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_WIDTH, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_HEIGHT
     */
    [types.SET_CUSTOMIZED_PRODUCT_HEIGHT]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_HEIGHT, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_DEPTH
     */
    [types.SET_CUSTOMIZED_PRODUCT_DEPTH]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_DEPTH, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_UNIT
     */
    [types.SET_CUSTOMIZED_PRODUCT_UNIT]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_UNIT, payload);
    },
    /**
     * Action used to commit the mutation SET_SLOT_WIDTH
     */
    [types.SET_SLOT_WIDTH]: ({ commit }, payload) => {
        commit(types.SET_SLOT_WIDTH, payload);
    },
    /**
     * Action used to commit the mutation SET_SLOT_DEPTH
     */
    [types.SET_SLOT_DEPTH]: ({ commit }, payload) => {
        commit(types.SET_SLOT_DEPTH, payload);
    },
    /**
     * Action used to commit the mutation SET_SLOT_HEIGHT
     */
    [types.SET_SLOT_HEIGHT]: ({ commit }, payload) => {
        commit(types.SET_SLOT_HEIGHT, payload);
    },
    /**
     * Action used to commit the mutation SET_SLOT_UNIT
     */
    [types.SET_SLOT_UNIT]: ({ commit }, payload) => {
        commit(types.SET_SLOT_UNIT, payload);
    },
    [types.SET_CUSTOMIZED_PRODUCT_COMPONENTS]: ({commit},payload)=>{
        commit(types.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    }
}