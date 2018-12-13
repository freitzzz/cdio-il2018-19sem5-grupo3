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

  /**
   * Changes the states's customized product's slot width
   * @param {*} state The store's state
   * @param {*} payload Payload with the new slot width 
   */
  [types.SET_SLOT_DIMENSIONS](state, payload) {
    if(payload){
    state.customizedProduct.slots.push({
      idSlot: payload.idSlot,
      depth: payload.depth,
      width: payload.width,
      height: payload.height,
      unit: payload.unit,
      components: []
    })
  }else{state.customizedProduct.slots=[];}
  },
  
/**
   * Changes the states's customized product's material 
   * @param {*} state The store's state
   * @param {*} payload Payload with the new material 
   */
  [types.SET_CUSTOMIZED_PRODUCT_MATERIAL](state, payload) {
    state.customizedProduct.customizedMaterial.id = payload.id,
    state.customizedProduct.customizedMaterial.reference = payload.reference,
    state.customizedProduct.customizedMaterial.designation =  payload.designation,
    state.customizedProduct.customizedMaterial.image = payload.image
    },

  /**
   * Adds a component to the state's customized product's
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to add
   */
  [types.SET_CUSTOMIZED_PRODUCT_COMPONENTS](state, payload) {
    if (state.customizedProduct.slots.length >= payload.component.slot) {
        state.customizedProduct.components.push(payload.component);
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
