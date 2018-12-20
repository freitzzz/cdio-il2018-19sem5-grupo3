import * as types from './mutation-types'

export const actions = {
    /**
     * Action used to commit the mutation SET_RESIZE_FACTOR_DIMENSIONS
     */
    [types.SET_RESIZE_FACTOR_DIMENSIONS]: ({ commit }, payload) => {
        commit(types.SET_RESIZE_FACTOR_DIMENSIONS, payload)
    },

    /**
     * Action used to commit the mutation INIT_PRODUCT
     */
    [types.INIT_PRODUCT]: ({ commit }, payload) => {
        commit(types.INIT_PRODUCT, payload)
    },
    /**
     * Action used to commit the mutation SET_ID_CUSTOMIZED_PRODUCT
     */
    [types.SET_ID_CUSTOMIZED_PRODUCT]: ({ commit }, payload) => {
        commit(types.SET_ID_CUSTOMIZED_PRODUCT, payload)
    },
    /**
     * Action used to commit the mutation SET_ID_SLOT
     */
    [types.SET_ID_SLOT]: ({ commit }, payload) => {
        commit(types.SET_ID_SLOT, payload)
    },

    /**
     * Action used to commit the mutation SET_SLOT_WIDTH
     */
    [types.ADD_SLOT_DIMENSIONS]: ({ commit }, payload) => {
        commit(types.ADD_SLOT_DIMENSIONS, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_MATERIAL
     */
    [types.SET_CUSTOMIZED_PRODUCT_MATERIAL]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_MATERIAL, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_FINISH
     */
    [types.SET_CUSTOMIZED_PRODUCT_FINISH]: ({commit}, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_FINISH, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_COLOR
     */
    [types.SET_CUSTOMIZED_PRODUCT_COLOR]: ({commit}, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_COLOR, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_DIMENSIONS
     */
    [types.SET_CUSTOMIZED_PRODUCT_DIMENSIONS]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_DIMENSIONS, payload);
    },

    /**
     * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_COMPONENTS
     */
    [types.SET_CUSTOMIZED_PRODUCT_COMPONENTS]: ({ commit }, payload) => {
        commit(types.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    },

    /**
     * Action used to commit the mutation REMOVE_CUSTOMIZED_PRODUCT_COMPONENT
     */
    [types.REMOVE_CUSTOMIZED_PRODUCT_COMPONENT]: ({ commit }, payload) => {
        commit(types.REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, payload);
    },

    /**
    * Action used to commit the mutation ACTIVATE_CAN_MOVE_CLOSET
    */
    [types.ACTIVATE_CAN_MOVE_CLOSET]: ({ commit }) => {
        commit(types.ACTIVATE_CAN_MOVE_CLOSET);
    },

    /**
    * Action used to commit the mutation ACTIVATE_CAN_MOVE_SLOTS
    */
    [types.ACTIVATE_CAN_MOVE_SLOTS]: ({ commit }) => {
        commit(types.ACTIVATE_CAN_MOVE_SLOTS);
    },

    /**
      * Action used to commit the mutation ACTIVATE_CAN_MOVE_COMPONENTS
      */
    [types.ACTIVATE_CAN_MOVE_COMPONENTS]: ({ commit }) => {
        commit(types.ACTIVATE_CAN_MOVE_COMPONENTS);
    },

    /**
      * Action used to commit the mutation DEACTIVATE_CAN_MOVE_CLOSET
      */
    [types.DEACTIVATE_CAN_MOVE_CLOSET]: ({ commit }) => {
        commit(types.DEACTIVATE_CAN_MOVE_CLOSET);
    },

    /**
      * Action used to commit the mutation DEACTIVATE_CAN_MOVE_SLOTS
      */
    [types.DEACTIVATE_CAN_MOVE_SLOTS]: ({ commit }) => {
        commit(types.DEACTIVATE_CAN_MOVE_SLOTS);
    },

    /**
      * Action used to commit the mutation DEACTIVATE_CAN_MOVE_COMPONENTS
      */
    [types.DEACTIVATE_CAN_MOVE_COMPONENTS]: ({ commit }) => {
        commit(types.DEACTIVATE_CAN_MOVE_COMPONENTS);
    },
}