<template>
  <div class="containerDimensions" style="margin-left:auto;margin-right:auto;">
    <div class="text-entry" style="font-family: 'Roboto', sans-serif;">Select a option:</div>
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">Please choose a option for the different type of dimensions.</span>
    </div>
    <select class="dropdown" v-model="dimensionOp" @click="populateDimensions">
      <option
        v-for="option in availableOptionsDimensions"
        :key="option.id"
        :value="option"
      >{{"Option: "+option.id}}</option>
    </select>
    <!--Fetch minimums from server-->
    <div class="text-entry">Height:</div>
    <vue-slider
      class="slider"
      :min="this.heightMin"
      :max="this.heightMax"
      :interval="this.heightIncrement"
      v-model="height"
      @change="updateHeight"
    ></vue-slider>
    <div class="text-entry">Width:</div>
    <vue-slider
      class="slider"
      :min="this.widthMin"
      :max="this.widthMax"
      :interval="this.widthIncrement"
      v-model="width"
      @change="updateWidth"
    ></vue-slider>
    <div class="text-entry">Depth:</div>
    <vue-slider
      class="slider"
      :min="this.depthMin"
      :max="this.depthMax"
      :increment="this.depthIncrement"
      v-model="depth"
      @change="updateDepth"
    ></vue-slider>

    <div class="text-entry">Choose the available unit:</div>
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
import vueSlider from "vue-slider-component";
import {
  SET_CUSTOMIZED_PRODUCT_WIDTH,
  SET_CUSTOMIZED_PRODUCT_HEIGHT,
  SET_CUSTOMIZED_PRODUCT_DEPTH,
  SET_CUSTOMIZED_PRODUCT_UNIT
} from "./../store/mutation-types.js";

import { error } from "three";
const MIN_DEFAULT=1;
const MAX_DEFAULT=2;
const INCREMENT_DEFAULT=1;
const DISCRETE_INTERVAL= 0;
const CONTINUOUS_INTERVAL= 1;
const DISCRETE_VALUE = 2;
const ERROR_DIMENSION_TYPE = "No available dimension please try the other option.";
const NO_OPTION = -1;

export default {
  name: "CustomizerSideBarDimensionsPanel",
  data() {
    return {
      heightMin: MIN_DEFAULT,
      heightMax: MAX_DEFAULT,
      heightIncrement: INCREMENT_DEFAULT,

      widthMin: MIN_DEFAULT,
      widthMax: MAX_DEFAULT,
      widthIncrement: INCREMENT_DEFAULT,

      depthMin:  MIN_DEFAULT,
      depthMax:  MAX_DEFAULT,
      depthIncrement: INCREMENT_DEFAULT,

      height: this.heightMin,
      width: this.widthMin,
      depth: this.depthMin,

      unit: "cm",
      dimensionOp: 0,
      availableOptionsDimensions: [],
      availableOptionsUnits: [],
      availableDimensionsHLD: [],

      heightType:NO_OPTION, ////No type of dimension until it's choosen an option
      widthType: NO_OPTION, ///No type of dimension until it's choosen an option
      depthType: NO_OPTION, //No type of dimension until it's choosen an option
      
    };
  },
  components: {
    vueSlider
  },
  created() {
    store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, { width: this.width });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, { height: this.height });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, { depth: this.depth });
    store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, { unit: this.unit });

    /*Get all available dimensions of the given product of the array*/
    Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/dimensions`)
      .then(response => this.availableOptionsDimensions.push(...response.data))
      .catch(error => {
        this.$toast.open(error.response.status + "An error occurred");
      });

    /*Get all available units of measurement*/
    Axios.get(`${MYCM_API_URL}/units`)
      .then(response => this.availableOptionsUnits.push(...response.data))
      .catch(error => {
        this.$toast.open(error.response.status + "An error occurred");
      });
  },
  methods: {
    /*  */
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

    //Method that identifies different types of dimensios
    //There are three types of dimensions: Discrete Interval, Discrete Value, Continuous Interval
    identifyTypeDimensions: function(dimensionObj) {
      if (dimensionObj.values != null) {
        //Discrete interval
        return this.DISCRETE_INTERVAL;
      } else if (dimensionObj.value != null) {
        //DIscrete value
        return this.DISCRETE_VALUE;
      } else if (
        dimensionObj.minValue != null &&
        dimensionObj.maxValue != null &&
        dimensionObj.increment != null
      ) {
        return this.CONTINUOUS_INTERVAL;
      } //Not yet implemented dimension
      return this.ERROR_DIMENSION_TYPE;
    },
    //Populate
    populateDimensions: function() {
      //Get information of the chosed option
      var op = this.dimensionOp;

      //Populate Height:
      this.heightType = this.identifyTypeDimensions(op.height);

      if (this.heightType == this.DISCRETE_INTERVAL) {
        this.organizeCrescentOrder(op.height.values);
      }

      //Populate Width
      this.widthType = this.identifyTypeDimensions(op.width);
      if (this.widthType == this.DISCRETE_INTERVAL) {
        this.organizeCrescentOrder(op.width.values);
      }
      /* alert(widthType); */
      //Populate Depth:
      this.depthType = this.identifyTypeDimensions(op.depth);
      /* alert(depthType); */
      if (this.depthType == this.DISCRETE_INTERVAL) {
        this.organizeCrescentOrder(op.depth.values);
      }
    },
    //The following methods determine the min,max and increment to populate the height,width and depth slider
    determineMinOfInterval: function(typeOfInterval, dimensionJson) {
      var min;
      if (typeOfInterval == this.DISCRETE_INTERVAL) {
        return 0; //index of
      } else if (typeOfInterval == this.CONTINUOUS_INTERVAL) {
        min = dimensionJson.minValue;
      } else {
        //DISCRETE VALUE
        min = dimensionJson.value;
      }
      return min;
    },
    determineMaxOfInterval: function(typeOfInterval, dimensionJson) {
      var max = -1;
      if (typeOfInterval == this.DISCRETE_INTERVAL) {
        /* for(var i=0; i< dimensionJson.values.length;i++){
          if(max < dimensionJson.values[i]){
            max = dimensionJson.values[i];
          }
        } */
        return dimensionJson.length - 1;
      } else if (typeOfInterval == this.CONTINUOUS_INTERVAL) {
        min = dimensionJson.maxValue;
      } else {
        max = dimensionJson.value;
      }
      return max;
    },
    determineIncrementOfInterval: function(typeOfInterval, dimensionJson) {
      var increment;
      if (typeOfInterval == this.DISCRETE_INTERVAL) {
        increment = 1;
      } else if (typeOfInterval == this.CONTINUOUS_INTERVAL) {
        increment = dimensionJson.increment;
      } else {
        //DISCRETE VALUE
        increment = 0;
      }
    },
    //Organizes vector to crescent order.
    organizeCrescentOrder: function(vec) {
      var tmp, minTmp;

      for (var i = 0; i < vec.length; i++) {
        tmp = vec[i];
        for (var j = i + 1; j < vec.length; j++) {
          if (tmp > vec[j]) {
            tmp = vec[j];
          }
        }
        if (tmp != vec[i]) {
          minTmp = vec[i];
          vec[i] = tmp;
          vec[j] = minTmp;
        }
      }
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
  margin-left: 15%;
  margin-right: 15%;
}
</style>


