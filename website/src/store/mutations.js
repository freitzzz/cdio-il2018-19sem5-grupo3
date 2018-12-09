import * as types from './mutation-types'
import store from '.';

export const mutations = {

  /**
   * Saves the retrieved product from MYCM into the state's product
   * @param {*} state The store's state
   * @param {*} payload Received payload with the product info
   */
  [types.INIT_PRODUCT](state, payload) {
    state.product = payload.product;
  },

  /**
   * Changes the states's customized product's dimension
   * @param {*} state The store's state
   * @param {*} payload Payload with the new width
   */
  [types.SET_CUSTOMIZED_PRODUCT_DIMENSIONS](state, payload) {
    state.customizedProduct.customizedDimensions = {
      width: payload.width,
      height: payload.height,
      depth: payload.depth,
      unit: payload.unit,
    }
  },

  // /* *
  //  * Changes the states's customized product's width
  //  * @param {*} state The store's state
  //  * @param {*} payload Payload with the new width
  //  */
  // [types.SET_CUSTOMIZED_PRODUCT_WIDTH](state, payload) {
  //   state.customizedProduct.customizedDimensions.width = payload.width;
  // },

  // /**
  //  * Changes the states's customized product's heigth
  //  * @param {*} state The store's state
  //  * @param {*} payload Payload with the new height
  //  */
  // [types.SET_CUSTOMIZED_PRODUCT_HEIGHT](state, payload) {
  //   state.customizedProduct.customizedDimensions.height = payload.height;
  // },

  // /**
  //  * Changes the states's customized product's depth
  //  * @param {*} state The store's state
  //  * @param {*} payload Payload with the new depth 
  //  */
  // [types.SET_CUSTOMIZED_PRODUCT_DEPTH](state, payload) {
  //   state.customizedProduct.customizedDimensions.depth = payload.depth;
  // },

  // /**
  //  * Changes the states's customized product's unit
  //  * @param {*} state The store's state
  //  * @param {*} payload Payload with the new unit 
  //  */
  // [types.SET_CUSTOMIZED_PRODUCT_UNIT](state, payload) {
  //   state.customizedProduct.customizedDimensions.unit = payload.unit;
  // },
  // /**
  //  * Changes the states's customized product's unit
  //  * @param {*} state The store's state
  //  * @param {*} payload Payload with the new unit 
  //  */
  // [types.SET_CUSTOMIZED_PRODUCT_UNIT](state, payload) {
  //   state.customizedProduct.customizedDimensions.unit = payload.unit;
  // },

  /**
   * Changes the states's customized product's slot width
   * @param {*} state The store's state
   * @param {*} payload Payload with the new slot width 
   */
  [types.SET_SLOT_DIMENSIONS](state, payload) {
    state.customizedProduct.slots.push({
      idSlot: payload.idSlot,
      depth: payload.depth,
      width: payload.width,
      height: payload.height,
      unit: payload.unit,
      components: []
    })
  },

  /**
   * Adds a component to a slot from the state's customized product's
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to add
   */
  [types.SET_CUSTOMIZED_PRODUCT_COMPONENTS](state, payload) {
    if (state.customizedProduct.slots.length >= payload.component.slot) {
      if (payload.component.slot > 0) {
        state.customizedProduct.slots[payload.component.slot - 1].components = [];
        state.customizedProduct.slots[payload.component.slot - 1].components.push(payload.component);
      } else {
        state.customizedProduct.slots[payload.component.slot].components = [];
        state.customizedProduct.slots[payload.component.slot].components.push(payload.component);
      }
    }
  },

  /**
  * Activates the flag that allows the user to move the closet faces
  * @param {*} state The store's state
  */
  [types.ACTIVATE_CAN_MOVE_CLOSET](state) {
    state.canvasControls.canMoveCloset = true;
  },

  /**
  * Activates the flag that allows the user to move the closet slots
  * @param {*} state The store's state
  */
  [types.ACTIVATE_CAN_MOVE_SLOTS](state) {
    state.canvasControls.canMoveSlots = true;
  },

  /**
  * Activates the flag that allows the user to move the closet components
  * @param {*} state The store's state
  */
  [types.ACTIVATE_CAN_MOVE_COMPONENTS](state) {
    state.canvasControls.canMoveComponents = true;
  },

  /**
 * Deactivates the flag that allows the user to move the closet faces
 * @param {*} state The store's state
 */
  [types.DEACTIVATE_CAN_MOVE_CLOSET](state) {
    state.canvasControls.canMoveCloset = false;
  },

  /**
  * Deactivates the flag that allows the user to move the closet slots
  * @param {*} state The store's state
  */
  [types.DEACTIVATE_CAN_MOVE_SLOTS](state) {
    state.canvasControls.canMoveSlots = false;
  },

  /**
  * Deactivates the flag that allows the user to move the closet components
  * @param {*} state The store's state
  */
  [types.DEACTIVATE_CAN_MOVE_COMPONENTS](state) {
    state.canvasControls.canMoveComponents = false;
  }
}
