<template>
  <div class="containerDimensions" style="margin-left:30%;margin-right:auto;">
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span
        class="tooltiptext">In this step, you must choose one of our base products in order to start customizing it.</span>
    </div>
    <select v-model="dimensionOp" @change="updateUnit" style="margin-bottom:10%">
      <option  v-for="option in availableOptions" :key="option.id" :value="option">{{"Option: "+option.id}}</option>
    </select>
    <!--Fetch minimums from server-->
    <input
      class="slider"
      type="range"
      min="1"
      name="height"
      v-model="height"
      @change="updateHeight"
    >
    <input
      class="slider"
      type="range"
      min="1"
      name="width"
      v-model="width"
      @change="updateWidth"
    >
    <input
      class="slider"
      type="range"
      min="1"
      name="depth"
      v-model="depth"
      @change="updateDepth"
    >
    
    <select class="dropdown" v-model="unit" @change="updateUnit">
      <option value="mm">Milimeters</option>
      <option value="cm">Centimeters</option>
      <option value="dm">Decimeters</option>
      <option value="m">Meters</option>
    </select>
  </div>
</template>


<script>
import store from "./../store";
import Axios from "axios";
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
      unit: "cm",
      dimensionOp: "",
      availableOptions: []
    };
  },
  created() {
    store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: this.width });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: this.height });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: this.depth });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, { unit: this.unit });

    /*Get all available dimensions of the array*/
    Axios.get(
      `http://localhost:5000/mycm/api/products/${
        store.state.product.id
      }/dimensions`
    )
      .then(response => this.availableOptions.push(...response.data))
      .catch(console.log());
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
    },
 /*    //Get all available options
    populateAvailableOptions() {} */
  }
};
</script>
<style>
.containerDimensions {
  margin: 5%;/*
  margin-left: 22.5%;
  margin-right: 22.5%; */
 border-bottom: 10%;
  font-family: "Roboto", sans-serif;
}

.icon-div-center {
  text-align: center;
}

.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 120px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
}

.icon-div-top:hover .tooltiptext {
  visibility: visible;
}

.icon-div-top {
  top: 16.5px;
  left: 27px;
  margin-left: 100px;
  position: absolute;
}
.dropdown{
  width: 60%;
  margin-bottom:10%;
}
.slider{
  width:60%;
  margin-bottom:5%;
}
</style>


