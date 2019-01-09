import * as types from './mutation-types'

export const mutations = {

  //*Product mutations

  /**
   * Saves the retrieved product from MYCM into the state's product
   * @param {*} state The store's state
   * @param {*} payload Received payload with the product info
   */
  [types.INIT_PRODUCT](state, payload) {
    state.product = payload.product;
  },

  //*CustomizedProduct mutations

  /**
   * Saves the id of customized product 
   * @param {*} state The store's state
   * @param {*} payload Received payload with the id
   */
  [types.SET_ID_CUSTOMIZED_PRODUCT](state, payload) {
    state.customizedProduct.id = payload;
  },

  /**
   * Sets the main CustomizedProduct's reference.
   * @param {*} state - The store's state.
   * @param {string} payload - CustomizedProduct's reference.
   */
  [types.SET_CUSTOMIZED_PRODUCT_REFERENCE](state, payload) {
    state.customizedProduct.reference = payload;
  },

  /**
   * Sets the main CustomizedProduct's designation.
   * @param {*} state - The store's state.
   * @param {string} payload - CustomizedProduct's designation.
   */
  [types.SET_CUSTOMIZED_PRODUCT_DESIGNATION](state, payload) {
    state.customizedProduct.designation = payload;
  },

  /**
   * Changes the state's customized product's dimension
   * @param {*} state The store's state
   * @param {*} payload Payload with the CustomizedProduct's dimensions
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
   * Changes the state's customized product's material 
   * @param {*} state The store's state
   * @param {*} payload Payload with the new material 
   */
  [types.SET_CUSTOMIZED_PRODUCT_MATERIAL](state, payload) {
    state.customizedProduct.customizedMaterial.id = payload.id;
    state.customizedProduct.customizedMaterial.reference = payload.reference;
    state.customizedProduct.customizedMaterial.designation = payload.designation;
    state.customizedProduct.customizedMaterial.image = payload.image;
  },

  /**
   * Changes the state's customized product's finish 
   * @param {*} state The store's state
   * @param {*} payload Payload with the new finish 
   */
  [types.SET_CUSTOMIZED_PRODUCT_FINISH](state, payload) {
    state.customizedProduct.customizedMaterial.finish.description = payload.description;
    state.customizedProduct.customizedMaterial.finish.shininess = payload.shininess;
  },

  /**
   * Changes the state's customized product's color 
   * @param {*} state The store's state
   * @param {*} payload Payload with the new color 
   */
  [types.SET_CUSTOMIZED_PRODUCT_COLOR](state, payload) {
    state.customizedProduct.customizedMaterial.color.name = payload.name;
    state.customizedProduct.customizedMaterial.color.red = payload.red;
    state.customizedProduct.customizedMaterial.color.green = payload.green;
    state.customizedProduct.customizedMaterial.color.blue = payload.blue;
    state.customizedProduct.customizedMaterial.color.alpha = payload.alpha;
  },

  /**
   * Saves the id of slot 
   * @param {*} state The store's state
   * @param {*} payload Received payload with the id
   */
  [types.SET_ID_SLOT](state, payload) {
    state.customizedProduct.slots[payload.position].idSlot = payload.idSlot;
  },

  /**
   * Changes the state's customized product's slot width
   * @param {*} state The store's state
   * @param {*} payload Payload with the new slot width 
   */
  [types.ADD_SLOT_DIMENSIONS](state, payload) {
    if (payload) {
      if (payload.height == 0) {
        state.customizedProduct.slots[payload.idSlot].idSlot = payload.idSlot,
          state.customizedProduct.slots[payload.idSlot].depth = payload.depth,
          state.customizedProduct.slots[payload.idSlot].width = payload.width,
          state.customizedProduct.slots[payload.idSlot].height = payload.height,
          state.customizedProduct.slots[payload.idSlot].unit = payload.unit
      } else {
        state.customizedProduct.slots.push({
          idSlot: payload.idSlot,
          depth: payload.depth,
          width: payload.width,
          height: payload.height,
          unit: payload.unit
        })
      }
    } else { state.customizedProduct.slots = []; }
  },

  /**
   * Adds a component to the state's customized product components
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to add
   */
  [types.SET_CUSTOMIZED_PRODUCT_COMPONENTS](state, payload) {
    if(!payload) state.customizedProduct.components = [];
    else if (state.customizedProduct.slots.length >= payload.component.slot) {
      let copiedArray = state.customizedProduct.components.slice(0);
      copiedArray.push(payload);
      state.customizedProduct.components = copiedArray;
    }
  },

  /**
   * Removes a component from the state's customized product's
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to remove
   */
  [types.REMOVE_CUSTOMIZED_PRODUCT_COMPONENT](state, payload) {
    let index = state.customizedProduct.components.indexOf(payload.component);
    let copiedArray = state.customizedProduct.components.slice(0);
    copiedArray.splice(index, 1);

    state.customizedProduct.components = copiedArray;
  },

  //*CanvasControls mutations

  /**
   * Sets the state's canvas controls product to be removed on further confirmation
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to be removed if confirmed
   */
  [types.SET_COMPONENT_TO_REMOVE](state, payload) {
    state.canvasControls.componentToRemove = payload;
  },

  /**
   * Sets the state's canvas controls product to be added 
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to be added
   */
  [types.SET_COMPONENT_TO_ADD](state, payload){
    state.canvasControls.componentToAdd = payload;
  },

  /**
   * Sets the state's canvas controls product to be edited 
   * @param {*} state The store's state
   * @param {*} payload Payload with the component to be edited
   */
  [types.SET_COMPONENT_TO_EDIT](state, payload){
    state.canvasControls.componentToAdd = payload;
  },

  /**
   * Changes the flag that controls the doors that can be applied to the customized product
   * @param {*} state The store's state
   */
  [types.SET_DOORS_FLAG](state, payload) {
    state.canvasControls.doorsFlag = payload.flag;
  },

  /**
   * Activates the flag that allows the user to move the closet slots
   * @param {*} state The store's state
   */
  [types.ACTIVATE_CAN_MOVE_SLOTS](state) {
    state.canvasControls.canMoveSlots = true;
  },

  /**
   * Deactivates the flag that allows the user to move the closet slots
   * @param {*} state The store's state
   */
  [types.DEACTIVATE_CAN_MOVE_SLOTS](state) {
    state.canvasControls.canMoveSlots = false;
  },

  /**
   * Activates the flag that allows the user to move the closet faces
   * @param {*} state The store's state
   */
  [types.ACTIVATE_CAN_MOVE_CLOSET](state) {
    state.canvasControls.canMoveCloset = true;
  },

  /**
   * Deactivates the flag that allows the user to move the closet faces
   * @param {*} state The store's state
   */
  [types.DEACTIVATE_CAN_MOVE_CLOSET](state) {
    state.canvasControls.canMoveCloset = false;
  },

  /**
   * Activates the flag that allows the user to move the closet components
   * @param {*} state The store's state
   */
  [types.ACTIVATE_CAN_MOVE_COMPONENTS](state) {
    state.canvasControls.canMoveComponents = true;
  },

  /**
   * Deactivates the flag that allows the user to move the closet components
   * @param {*} state The store's state
   */
  [types.DEACTIVATE_CAN_MOVE_COMPONENTS](state) {
    state.canvasControls.canMoveComponents = false;
  },

  //*Resize mutations

  [types.SET_RESIZE_FACTOR_DIMENSIONS](state, payload) {
    if (payload) {
      state.resizeFactorDimensions = payload;
    } else { state.resizeFactorDimensions = []; }

  },

  [types.SET_RESIZE_VECTOR_GLOBAL](state, payload) {
    if (payload) {
      state.resizeVectorGlobal = payload;
    } else {
      state.resizeVectorGlobal = [];
    }
  }
}
