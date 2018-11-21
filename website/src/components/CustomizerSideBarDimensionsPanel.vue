<template>
    <div>
      <!--Fetch minimums from server-->
      <input type="number" min="1" name="height" v-model="height" @change="updateHeight">
      <input type="number" min="1" name="width" v-model="width" @change="updateWidth">
      <input type="number" min="1" name="depth" v-model="depth" @change="updateDepth">
      <select v-model="unit" @change="updateUnit">
        <option value="mm">Milimeters</option>
        <option value="cm">Centimeters</option>
        <option value="dm">Decimeters</option>
        <option value="m">Meters</option>
      </select>
    </div>
</template>


<script>
import store from "./../store";
import {
  SET_CUSTOMIZED_PRODUCT_WIDTH,
  SET_CUSTOMIZED_PRODUCT_HEIGHT,
  SET_CUSTOMIZED_PRODUCT_DEPTH,
  SET_CUSTOMIZED_PRODUCT_UNIT
} from "./../store/mutation-types.js";

import { error } from "three";

export default {
  name: "CustomizerSideBarDimensionsPanel",
  data() {
    return {
      // //TODO: replace hardcoded values ASAP
      height: 100,
      width: 100,
      depth: 100,
      unit: "cm"
    };
  },
  created() {
    store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: this.width });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: this.height });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: this.depth });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, { unit: this.unit });
  },
  methods: {
    updateHeight(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: e.target.value });
    },
    updateWidth(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: e.target.value });
    },
    updateDepth(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: e.target.value });
    },
    updateUnit(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, { unit: e.target.value });
    }
  }
};
</script>

