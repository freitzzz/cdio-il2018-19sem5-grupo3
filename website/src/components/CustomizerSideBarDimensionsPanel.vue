<template>
    <div>
      <!--Fetch minimums from server-->
      <input type="number" min="1" name="height" v-model="height" @change="updateHeight">
      <input type="number" min="1" name="width" v-model="width" @change="updateWidth">
      <input type="number" min="1" name="depth" v-model="depth" @change="updateDepth">
      <select>
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
  SET_CUSTOMIZED_PRODUCT_DEPTH
} from "./../store/mutation-types.js";

import { error } from "three";

export default {
  name: "CustomizerSideBarDimensionsPanel",
  data() {
    return {
      // //TODO: replace hardcoded values ASAP
      height: 0,
      width: 0,
      depth: 0
    };
  },
  created() {
    store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: this.width });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: this.height });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: this.depth });
  },
  methods: {
    updateHeight(e) {
      alert(e.target.value);
      store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: e.target.value });
    },
    updateWidth(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: e.target.value });
    },
    updateDepth(e) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: e.target.value });
    }
  }
};
</script>

