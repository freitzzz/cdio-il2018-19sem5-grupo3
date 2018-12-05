<template>
  <div class="containerDimensions" style="margin-left:auto;margin-right:auto;">
    <div class="text-entry" style="font-family: 'Roboto', sans-serif;">Select a option:</div>
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">Please choose a option for the different type of dimensions.</span>
    </div>
    <select class="dropdown" v-model="dimensionOp" @change="populateDimensions">
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
      v-if="!this.discreteValueFlags[this.HEIGHT]"
      :min="this.heightMin"
      :max="this.heightMax"
      :interval="this.heightIncrement"
      v-model="height"
      @change="updateHeight"
    ></vue-slider>
    <input class="slider" v-else type="text" :readonly="true" v-model="this.heightMin">

    <div class="text-entry">Width:</div>
    <vue-slider
      class="slider"
      v-if="!this.discreteValueFlags[this.WIDTH]"
      :min="this.widthMin"
      :max="this.widthMax"
      :interval="this.widthIncrement"
      v-model="width"
      @change="updateWidth"
    ></vue-slider>
    <input class="slider" v-else type="text" :readonly="true" v-model="this.widthMin">

    <div class="text-entry">Depth:</div>
    <vue-slider
      class="slider"
      v-if="!this.discreteValueFlags[this.DEPTH]"
      :min="this.depthMin"
      :max="this.depthMax"
      :interval="this.depthIncrement"
      v-model="depth"
      @change="updateDepth"
    ></vue-slider>
    <input class="slider" v-else type="text" :readonly="true" v-model="this.depthMin">

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
  import {
    MYCM_API_URL
  } from "./../config.js";
  import vueSlider from "vue-slider-component";
  import {
    SET_CUSTOMIZED_PRODUCT_WIDTH,
    SET_CUSTOMIZED_PRODUCT_HEIGHT,
    SET_CUSTOMIZED_PRODUCT_DEPTH,
    SET_CUSTOMIZED_PRODUCT_UNIT
  } from "./../store/mutation-types.js";
  
  import {
    error
  } from "three";
  const MIN_DEFAULT = 1;
  const MAX_DEFAULT = 2;
  const INCREMENT_DEFAULT = 1;
  
  const DISCRETE_INTERVAL = 0;
  const CONTINUOUS_INTERVAL = 1;
  const DISCRETE_VALUE = 2;
  const ERROR_DIMENSION_TYPE =
    "No available dimension please try the other option.";
  const NO_OPTION = -1;
  const N_DIMENSIONS = 3;

  
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
  
        depthMin: MIN_DEFAULT,
        depthMax: MAX_DEFAULT,
        depthIncrement: INCREMENT_DEFAULT,
  
        height: this.heightMin,
        width: this.widthMin,
        depth: this.depthMin,
  
        unit: "cm",
  
        dimensionOp: 0,
  
        availableOptionsDimensions: [],
        availableOptionsUnits: [],
        availableDimensionsHLD: [],
  
        heightType: NO_OPTION, ////No type of dimension until it's choosen an option
        widthType: NO_OPTION, ///No type of dimension until it's choosen an option
        depthType: NO_OPTION, //No type of dimension until it's choosen an option
  
        /*Flags: */
        discreteValueFlags:[false,false,false],

        HEIGHT: 0,
        WIDTH :1,       
        DEPTH :2,

      };
    },
    components: {
      vueSlider
    },
    created() {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, {
        width: this.width
      });
      store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, {
        height: this.height
      });
      store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, {
        depth: this.depth
      });
      store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, {
        unit: this.unit
      });
  
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
     
      setFalseDiscreteFlags:function(){
        for(var i = 0; i < N_DIMENSIONS -1; i++){
          this.discreteValueFlags[i] = false;
        }
      },
      /*  */
      updateHeight(e) {
        store.dispatch(SET_CUSTOMIZED_PRODUCT_HEIGHT, {
          height: e.target.value
        });
      },
      updateWidth(e) {
        store.dispatch(SET_CUSTOMIZED_PRODUCT_WIDTH, {
          width: e.target.value
        });
      },
      updateDepth(e) {
        store.dispatch(SET_CUSTOMIZED_PRODUCT_DEPTH, {
          depth: e.target.value
        });
      },
      updateUnit(e) {
        store.dispatch(SET_CUSTOMIZED_PRODUCT_UNIT, {
          unit: e.target.value
        });
      },
  
      //Method that identifies different types of dimensios
      //There are three types of dimensions: Discrete Interval, Discrete Value, Continuous Interval
      identifyTypeDimensions: function(dimensionObj) {
        if (dimensionObj.values != null) {
          //Discrete interval
          return DISCRETE_INTERVAL;
        } else if (dimensionObj.value != null) {
          //DIscrete value
          return DISCRETE_VALUE;
        } else if (
          dimensionObj.minValue != null &&
          dimensionObj.maxValue != null &&
          dimensionObj.increment != null
        ) {
          return CONTINUOUS_INTERVAL;
        } else { //Not yet implemented dimension
          return ERROR_DIMENSION_TYPE;
        }
      },
      //Populate
      populateDimensions: function() {

        this.setFalseDiscreteFlags();

        //Get information of the chosed option
        var op = this.dimensionOp;
  
        //Populate Height:
        this.heightType = this.identifyTypeDimensions(op.height);
        if(this.heightType == DISCRETE_VALUE){
          this.heightMin = this.determineMinOfInterval(this.heightType,op.height);
          this.discreteValueFlags[this.HEIGHT] = true;
        }else{
          this.heightMin = this.determineMinOfInterval(this.heightType, op.height);
          this.heightMax = this.determineMaxOfInterval(this.heightType, op.height);
          this.heightIncrement = this.determineIncrementOfInterval(this.heightType, op.height);
        }

        //Populate Width
        this.widthType = this.identifyTypeDimensions(op.width);
        if (this.widthType == DISCRETE_VALUE) {
          this.widthMin = this.determineMinOfInterval(this.widthType, op.width);
          this.discreteValueFlags[this.WIDTH] = true;
        } else {
          this.widthMin = this.determineMinOfInterval(this.widthType, op.width);
          this.widthMax = this.determineMaxOfInterval(this.widthType, op.width);
          this.widthIncrement = this.determineIncrementOfInterval(this.widthType, op.width);
        }

        //Populate Depth:
        this.depthType = this.identifyTypeDimensions(op.depth);
        if (this.depthType == DISCRETE_VALUE) {
          this.depthMin = this.determineMinOfInterval(this.depthType, op.depth);
          this.discreteValueFlags[this.DEPTH] = true;
        } else {
          this.depthMin = this.determineMinOfInterval(this.depthType, op.depth);
          this.depthMax = this.determineMaxOfInterval(this.depthType, op.depth);
          this.depthIncrement = this.determineIncrementOfInterval(this.depthType, op.depth);
        }
       
      },
      //The following methods determine the min,max and increment to populate the height,width and depth slider
      determineMinOfInterval: function(
        typeOfInterval,
        dimensionJson
      ) {
        if (typeOfInterval == DISCRETE_INTERVAL) {
          return 0; //index of
        } else if (typeOfInterval == CONTINUOUS_INTERVAL) {
          return dimensionJson.minValue;
        } else {
          //DISCRETE VALUE
          return dimensionJson.value;
        }
      },
      determineMaxOfInterval: function(
        typeOfInterval,
        dimensionJson
      ) {
        var max = -1;
        if (typeOfInterval == DISCRETE_INTERVAL) {
          /* for(var i=0; i< dimensionJson.values.length;i++){
            if(max < dimensionJson.values[i]){
              max = dimensionJson.values[i];
            }
          } */
          return dimensionJson.length - 1;
        } else if (typeOfInterval == CONTINUOUS_INTERVAL) {
          return dimensionJson.maxValue;
        } else {
          return dimensionJson.value;
        }
      },
      determineIncrementOfInterval: function(
        typeOfInterval,
        dimensionJson
      ) {
        if (typeOfInterval == DISCRETE_INTERVAL) {
          return 1;
        } else if (typeOfInterval == CONTINUOUS_INTERVAL) {
          return dimensionJson.increment;
        } else {
          //DISCRETE VALUE
          return 1;
        }
      }
      /*  //Organizes vector to crescent order.
        organizeCrescentOrder: function(vec) {
          var tmp, minTmp;
    
          for (var i = 0; i < vec.length; i++) {
            tmp = this.vec[i];
            for (var j = i + 1; j < this.vec.length; j++) {
              if (tmp > this.vec[j]) {
                tmp = this.vec[j];
              }
            }
            if (tmp != this.vec[i]) {
              minTmp = this.vec[i];
              this.vec[i] = tmp;
              this.vec[j] = minTmp;
            }
          }
        } */
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


