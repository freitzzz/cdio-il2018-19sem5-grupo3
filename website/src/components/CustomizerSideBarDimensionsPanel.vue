<template>
  <div class="containerDimensions" style="margin-left:auto;margin-right:auto;">
    <div class="text-entry" style="font-family: 'Roboto', sans-serif;">Select a option:</div>
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">Please choose a option for the different type of dimensions.</span>
    </div>
    <select class="dropdown" v-model="dimensionOp" @change="updateUnit">
      <option
        v-for="option in availableOptionsDimensions"
        :key="option.id"
        :value="option"
      >{{"Option: "+option.id}}</option>
    </select>
    <!--Fetch minimums from server-->
    <div class="text-entry">Height:</div>
    <vue-slider class="slider" v-model="height" @change="updateHeight"></vue-slider>
    <div class="text-entry">Width:</div>
    <vue-slider class="slider" v-model="width" @change="updateWidth"></vue-slider>
    <div class="text-entry">Depth:</div>
    <vue-slider class="slider" v-model="depth" @change="updateDepth"></vue-slider>

    <div class="text-entry">Choose the available units:</div>
    <select class="dropdown" v-model="unit" @change="updateUnit">
      <option
        v-for="optionUnit in availableOptionsUnits"
        :key="optionUnit.id"
        :value="optionUnit"
      >{{optionUnit.unit}}</option>
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
      dimensionOp: "",
      availableOptionsDimensions:[],
      availableOptionsUnits:[],
      DISCRETE_INTERVAL:0,
      CONTINUOUS_INTERVAL:1,
      DISCRETE_VALUE:2
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

   /*Get all available units of measurement*/
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
    
    identifyTypeDimensions(dimensionObj){
       
      if( dimensionObj.values !=null){ //Discrete interval
        return DISCRETE_INTERVAL;
      }else if(dimensionObj.value !=null){ //DIscrete value
        return DISCRETE_VALUE;
      }else if(dimensionObj.minValue !=null && dimensionObj.maxValue!=null && dimensionObj.increment !=null){
        return CONTINUOUS_INTERVAL;
      }//Not yet implemented dimension
      return -1;
      
    },
    //Get all available options
    populateAvailableOptions() {
      //Get information of the chosed option
      var op = this.dimensionOp;

      //Create array of different dimensions h,l,d
      //Create for to identify each type of dimension
      //Set slider of different dimension
      //Stop if the dimension is invalid
      //Populate Height:

      //Populate Length

      //Populate Depth:

      
    } 
  }
};
</script>
<style>
.containerDimensions {
  margin: 3%;
  margin-left: 22.5%;
  margin-right: 22.5%;
  margin-bottom: 3%;
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
  top: 17px;
  left: 30px;
  margin-left: 100px;
  position: absolute;
}
.dropdown {
  margin-left: 15%;
  width: 60%;
  margin-bottom: 3%;
  margin-right: 15%;
}
.slider {
  margin-bottom: 5%;
  width: 15px;
}
</style>


