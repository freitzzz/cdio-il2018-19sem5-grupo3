<template>
  <div class="containerDimensions" style="margin-left:auto;margin-right:auto;">
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span
        class="tooltiptext">TEXTO TEXTO</span>
    </div>
    <select class="dropdown" v-model="dimensionOp" @change="updateUnit">
      <option v-for="option in availableOptionsDimensions" :key="option.id" :value="option">{{"Option: "+option.id}}</option>
    </select>
    <!--Fetch minimums from server-->
    <vue-slider class="slider" v-model="height" @change="updateHeight"></vue-slider>
    <vue-slider class="slider" v-model="width" @change="updateWidth"></vue-slider>
    <vue-slider class="slider" v-model="depth" @change="updateDepth"></vue-slider>
    
    <select class="dropdown" v-model="unit" @change="updateUnit">
      <option v-for="optionUnit in availableOptionsUnits" :key="optionUnit.id" :value="optionUnit">{{optionUnit.unit}}</option>
    </select>
  </div>
</template>


<script>
import store from "./../store";
import Axios from "axios";
import { MYCM_API_URL } from "./../config.js";
import vueSlider from 'vue-slider-component';
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
      dimensionOp: "option",
      availableOptionsDimensions:[],
      availableOptionsUnits:[],
      

    };
  },
  components:{
    vueSlider
  },
  created() {
    store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: this.width });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: this.height });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: this.depth });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, { unit: this.unit });

    /*Get all available dimensions of the given product of the array*/
    Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/dimensions`
    )
      .then(response => this.availableOptionsDimensions.push(...response.data))
      .catch(console.log(error));

   
    Axios.get(`${MYCM_API_URL}/units`)
    .then(response => this.availableOptionsUnits.push(...response.data))
    .catch(console.log(error));
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
  position:relative;
}

.icon-div-top {
  top: 16.5px;
  left: 27px;
  margin-left: 100px;
  position: absolute;
}
.dropdown{
  margin-left: 15%;
  width: 60%;
  margin-bottom:10%;
}
.slider{
  width:5%;
  margin-bottom:10%;
  width:50%;
}
</style>


