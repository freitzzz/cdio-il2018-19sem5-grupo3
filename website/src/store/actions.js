import * as types from './mutation-types'

export const actions = {
    /**
     * Action used to commit the mutation INIT_PRODUCT
     */
    [types.INIT_PRODUCT]: ({ commit }, payload) => {
        commit(types.INIT_PRODUCT, payload)
    },

    // /**
    //  * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_WIDTH
    //  */
    // [types.SET_CUSTOMIZED_PRODUCT_WIDTH]: ({ commit }, payload) => {
    //     commit(types.SET_CUSTOMIZED_PRODUCT_WIDTH, payload);
    // },

    // /**
    //  * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_HEIGHT
    //  */
    // [types.SET_CUSTOMIZED_PRODUCT_HEIGHT]: ({ commit }, payload) => {
    //     commit(types.SET_CUSTOMIZED_PRODUCT_HEIGHT, payload);
    // },

    // /**
    //  * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_DEPTH
    //  */
    // [types.SET_CUSTOMIZED_PRODUCT_DEPTH]: ({ commit }, payload) => {
    //     commit(types.SET_CUSTOMIZED_PRODUCT_DEPTH, payload);
    // },

    // /**
    //  * Action used to commit the mutation SET_CUSTOMIZED_PRODUCT_UNIT
    //  */
    // [types.SET_CUSTOMIZED_PRODUCT_UNIT]: ({ commit }, payload) => {
    //     commit(types.SET_CUSTOMIZED_PRODUCT_UNIT, payload);
    // },
    /**
     * Action used to commit the mutation SET_SLOT_WIDTH
     */
    [types.SET_SLOT_DIMENSIONS]: ({ commit }, payload) => {
        commit(types.SET_SLOT_DIMENSIONS, payload);
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
    }
}