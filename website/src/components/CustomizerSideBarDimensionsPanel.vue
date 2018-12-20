<template>
  <div class="containerDimensions" style="margin-left:auto;margin-right:auto;">
    <div class="text-entry">Select a option:</div>
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
  
    <!-- HEIGHT: -->
    <div class="text-entry">Height:</div>
    <vue-slider class="slider" v-if="this.discreteIntervalFlags[this.HEIGHT]" v-model="height" @callback="updateDimensions" :interval="this.heightIncrement" :data="this.discreteIntervalHeight"></vue-slider>
    <vue-slider class="slider" v-if="this.continousIntervalFlags[this.HEIGHT]" :min="this.heightMin" :max="this.heightMax" :interval="this.heightIncrement" v-model="height" @callback="updateDimensions"></vue-slider>
    <input class="slider" v-if="this.discreteValueFlags[this.HEIGHT]" type="text" :readonly="true" v-model="height">
  
    <!-- WIDTH: -->
    <div class="text-entry">Width:</div>
    <vue-slider class="slider" v-if="this.discreteIntervalFlags[this.WIDTH]" :interval="this.widthIncrement" :data="this.discreteIntervalWidth" v-model="width" @callback="this.updateDimensions"></vue-slider>
    <vue-slider class="slider" v-if="this.continousIntervalFlags[this.WIDTH]" :min="this.widthMin" :max="this.widthMax" :interval="this.widthIncrement" v-model="width" @callback="this.updateDimensions"></vue-slider>
    <input class="slider" v-if="this.discreteValueFlags[this.WIDTH]" type="text" :readonly="true" v-model="this.width">
  
    <!-- DEPTH: -->
    <div class="text-entry">Depth:</div>
    <vue-slider class="slider" v-if="this.discreteIntervalFlags[this.DEPTH]" :interval="this.depthIncrement" :data="this.discreteIntervalDepth" v-model="depth" @callback="this.updateDimensions"></vue-slider>
    <vue-slider class="slider" v-if="this.continousIntervalFlags[this.DEPTH]" :min="this.depthMin" :max="this.depthMax" :interval="this.depthIncrement" v-model="depth" @callback="updateDimensions"></vue-slider>
    <input class="slider" v-if="this.discreteValueFlags[this.DEPTH]" type="text" :readonly="true" v-model="depth">
  
    <div class="text-entry">Choose the available unit:</div>
    <select class="dropdown" v-model="unit" @change="this.updateDimensions">
                              <option
                                v-for="optionUnit in availableOptionsUnits"
                                :key="optionUnit.id"
                                :value="optionUnit.unit"
                              >{{optionUnit.unit}}</option>
                            </select>
    <div class="center-controls">
      <i class="btn btn-primary material-icons" @click="previousPanel()">arrow_back</i>
      <i class="btn btn-primary material-icons" @click="nextPanel()">arrow_forward</i>
    </div>
  </div>
</template>


<script>
  import Vue from "vue";
  import Toasted from "vue-toasted";
  Vue.use(Toasted);
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
    SET_CUSTOMIZED_PRODUCT_UNIT,
    SET_CUSTOMIZED_PRODUCT_DIMENSIONS,
    SET_SLOT_DIMENSIONS,
    ACTIVATE_CAN_MOVE_CLOSET,
    DEACTIVATE_CAN_MOVE_SLOTS,
    SET_ID_CUSTOMIZED_PRODUCT,
    SET_RESIZE_FACTOR_DIMENSIONS
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
  
  const WIDTH = 0;
  const HEIGHT = 1;
  const DEPTH = 2;
  
  
  
  
  const DEFAULT_UNIT = "mm";
  export default {
    name: "CustomizerSideBarDimensionsPanel",
    data() {
      return {
        storeDispatchVec: [],
        dimensionVec: [],
        controlIndex: 0,
        i: 0,
  
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
  
        unit: "mm",
  
        availableOptionsDimensions: [],
        availableOptionsUnits: [],
        storeDimensions: [],
        dimensionOp: 0,
  
        heightType: NO_OPTION, //NNo type of dimension until it's choosen an option
        widthType: NO_OPTION, ///No type of dimension until it's choosen an option
        depthType: NO_OPTION, //No type of dimension until it's choosen an option
  
        /*Flags: */
        discreteValueFlags: [false, false, false],
        discreteIntervalFlags: [false, false, false],
        continousIntervalFlags: [false, false, false],
  
        HEIGHT: 0,
        WIDTH: 1,
        DEPTH: 2,
  
        /*If exists discrete interval, there's  a vector associated to it*/
        discreteIntervalHeight: [],
        discreteIntervalWidth: [],
        discreteIntervalDepth: [],
  
        //Convert the recommended width to a unit of the closet
        valueConvertedRecommededSlotsWidth: 0,
        idCustomizedProduct: 0
      };
    },
    components: {
      vueSlider
    },
    created() {
      if (this.dimensionOp == undefined) {
        this.undoDimensionConversion();
        //Transform 
        store.dispatch(SET_CUSTOMIZED_PRODUCT_DIMENSIONS, {
          width: this.storeDispatchVec[WIDTH],
          height: this.storeDispatchVec[HEIGHT],
          depth: this.storeDispatchVec[DEPTH],
          unit: DEFAULT_UNIT
        });
      } else {
        store.dispatch(SET_CUSTOMIZED_PRODUCT_DIMENSIONS, {
          width: this.width,
          height: this.height,
          depth: this.depth,
          unit: DEFAULT_UNIT
        });
      }
  
  
      store.dispatch(ACTIVATE_CAN_MOVE_CLOSET);
      store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS);
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
      this.initialPopulate();
  
  
    },
  
    methods: {
      resetFlags: function() {
        for (var i = 0; i < N_DIMENSIONS - 1; i++) {
          this.discreteValueFlags[i] = false;
          this.discreteIntervalFlags[i] = false;
          this.continousIntervalFlags[i] = false;
        }
      },
  
      updateDimensions() {
        this.undoDimensionConversion();
        //Transform 
        store.dispatch(SET_CUSTOMIZED_PRODUCT_DIMENSIONS, {
          width: this.storeDispatchVec[WIDTH],
          height: this.storeDispatchVec[HEIGHT],
          depth: this.storeDispatchVec[DEPTH],
          unit: DEFAULT_UNIT
        });
  
  
      },
      /*   convertDimensions(){
          Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/dimensions?unit=${this.unit}`)
          .then(response => this.storeDispatchVec.push(...response.data))
          .catch(error => {
            this.$toast.open(error.response.status + "An error occurred");
          });
        }, */
      undoDimensionConversion() {
        Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/dimensions?unit=${DEFAULT_UNIT}`)
          .then(response => this.storeDimensions.push(...response.data))
          .catch(error => {
            this.$toast.open(error.response.status + "An error occurred");
          });
  
  
        this.storeDispatchVec[WIDTH] = this.storeDimensions[this.dimensionOp].width;
        this.storeDispatchVec[HEIGHT] = this.storeDimensions[this.dimensionOp].height;
        this.storeDispatchVec[DEPTH] = this.storeDimensions[this.dimensionOp].depth;
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
        } else {
          //Not yet implemented dimension
          return ERROR_DIMENSION_TYPE;
        }
      },
      initialPopulate() {
        this.dimensionOp = this.availableOptionsDimensions[0];
        this.populateDimensions();
        this.createFirstReason();
  
      },
      //sends to product renderer the resize factor
      createFirstReason: function() {
        store.dispatch(SET_RESIZE_FACTOR_DIMENSIONS, {
          width: this.width,
          height: this.height,
          depth: this.depth,
        });
      },
      //Populate
      populateDimensions: function() {
  
  
        this.resetFlags();
        //Get information of the chosed option
        var op = this.dimensionOp;
        //Populate Height:
        this.heightType = this.identifyTypeDimensions(op.height);
        if (this.heightType == DISCRETE_INTERVAL) {
          this.discreteIntervalHeight = op.height.values;
  
          this.discreteIntervalFlags[this.HEIGHT] = true;
          this.continousIntervalFlags[this.HEIGHT] = false;
          this.discreteValueFlags[this.HEIGHT] = false;
  
          this.heightIncrement = 1;
        } else if (this.heightType == DISCRETE_VALUE) {
          this.height = this.determineMinOfInterval(this.heightType, op.height);
  
          this.discreteValueFlags[this.HEIGHT] = true;
          this.continousIntervalFlags[this.HEIGHT] = false;
          this.discreteIntervalFlags[this.HEIGHT] = false;
        } else {
          this.heightMin = this.determineMinOfInterval(
            this.heightType,
            op.height
          );
          this.heightMax = this.determineMaxOfInterval(
            this.heightType,
            op.height
          );
          this.heightIncrement = this.determineIncrementOfInterval(op.height);
  
          this.continousIntervalFlags[this.HEIGHT] = true;
          this.discreteIntervalFlags[this.HEIGHT] = false;
          this.discreteValueFlags[this.HEIGHT] = false;
        }
  
  
        //Populate Width
        this.widthType = this.identifyTypeDimensions(op.width);
        if (this.widthType == DISCRETE_INTERVAL) {
          this.discreteIntervalWidth = op.width.values;
  
          this.discreteIntervalFlags[this.WIDTH] = true;
          this.continousIntervalFlags[this.WIDTH] = false;
          this.discreteValueFlags[this.WIDTH] = false;
  
          this.widthIncrement = 1;
        } else if (this.widthType == DISCRETE_VALUE) {
          this.width = this.determineMinOfInterval(this.widthType, op.width);
  
          this.discreteValueFlags[this.WIDTH] = true;
          this.continousIntervalFlags[this.WIDTH] = false;
          this.discreteIntervalFlags[this.WIDTH] = false;
        } else {
          this.widthMin = this.determineMinOfInterval(this.widthType, op.width);
          this.widthMax = this.determineMaxOfInterval(this.widthType, op.width);
          this.widthIncrement = this.determineIncrementOfInterval(op.width);
  
          this.continousIntervalFlags[this.WIDTH] = true;
          this.discreteValueFlags[this.WIDTH] = false;
          this.discreteIntervalFlags[this.WIDTH] = false;
        }
        //Populate Depth:
        this.depthType = this.identifyTypeDimensions(op.depth);
        if (this.depthType == DISCRETE_INTERVAL) {
          this.discreteIntervalDepth = op.depth.values;
  
          this.discreteIntervalFlags[this.DEPTH] = true;
          this.continousIntervalFlags[this.DEPTH] = false;
          this.discreteValueFlags[this.DEPTH] = false;
  
          this.depthIncrement = 1;
        } else if (this.depthType == DISCRETE_VALUE) {
          this.depth = this.determineMinOfInterval(this.depthType, op.depth);
  
          this.discreteValueFlags[this.DEPTH] = true;
          this.continousIntervalFlags[this.DEPTH] = false;
          this.discreteIntervalFlags[this.DEPTH] = false;
        } else {
          this.depthMax = this.determineMaxOfInterval(this.depthType, op.depth);
          this.depthMin = this.determineMinOfInterval(this.depthType, op.depth);
          this.depthIncrement = this.determineIncrementOfInterval(op.depth);
  
          this.continousIntervalFlags[this.DEPTH] = true;
          this.discreteValueFlags[this.DEPTH] = false;
          this.discreteIntervalFlags[this.DEPTH] = false;
        }
        if (this.controlIndex == 0) { //First dimension
          this.createFirstReason;
          this.controlIndex++;
        }
  
      },
  
      //The following methods determine the min,max and increment to populate the height,width and depth slider
      determineMinOfInterval: function(typeOfInterval, dimensionJson) {
        if (typeOfInterval == CONTINUOUS_INTERVAL) {
          return dimensionJson.minValue;
        } else {
          //DISCRETE VALUE
          return dimensionJson.value;
        }
      },
      determineMaxOfInterval: function(typeOfInterval, dimensionJson) {
        var max = -1;
        if (typeOfInterval == CONTINUOUS_INTERVAL) {
          return dimensionJson.maxValue;
        } else {
          return dimensionJson.value;
        }
      },
      determineIncrementOfInterval: function(dimensionJson) {
        return dimensionJson.increment;
      },
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
      nextPanel() {
        //!TODO POST product
        //Post of product
  
        if (this.height != null && this.width != null && this.depth != null && this.dimensionOp != null) {
          Axios.post(MYCM_API_URL + '/customizedproducts', {
              productId: store.state.product.id,
              customizedDimensions: {
                height: this.height,
                width: this.width,
                depth: this.depth,
                unit: this.unit
              }
            })
            .then(response => {
              this.idCustomizedProduct = response.data.id;
              store.dispatch(SET_ID_CUSTOMIZED_PRODUCT, this.idCustomizedProduct);
              alert(store.state.customizedProduct.id);
              this.drawRecommendedSlots();
              this.$emit("advance");
            })
            .catch((error_message) => {
              this.$toast.open({
                message: error_message.response.data.message
              });
            });
        } else {
          this.$toast.open("Please select an option!");
        }
  
        /*         return new Promise((accept, reject) => {
                      if (this.height != null && this.width != null && this.depth != null && this.dimensionOp != null) {
                        Axios.post(MYCM_API_URL + '/customizedproducts', {
                          productId: store.state.product.id,
                          customizedDimensions: {
                            height: this.height,
                            width: this.width,
                            depth: this.depth,
                            unit: this.unit
                          }
                        })
                        .then(response => {
                          this.idCustomizedProduct=response.data.id
                          accept
                        })
                        .catch((error_message) => {
                          this.$toast.open({
                            message: error_message.response.data.message
                          });
                          flag = true;
                        });
                        if (!flag) {
                          alert(this.idCustomizedProduct);
      
                          store.dispatch(SET_ID_CUSTOMIZED_PRODUCT, this.idCustomizedProduct);
                          this.drawRecommendedSlots();
                          this.$emit("advance");
                        }else{
                          this.$toast.open("There was an error please try again!");
                        }
                      } else {
                        this.$toast.open("Please select an option!");
                      }
                    }); */
      },
      previousPanel() {
        //!TODO DELETE product
        this.$emit("back");
      },
      drawRecommendedSlots() {
        var widthCloset = /*store.state.customizedProduct.customizedDimensions.width;*/ 404.5;
        var depthCloset = /*store.state.customizedProduct.customizedDimensions.depth;*/ 100;
        var heightCloset = /*store.state.customizedProduct.customizedDimensions.height;*/ 300;
  
        var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
        var unitSlots = store.getters.productSlotWidths.unit;
  
        var recommendedSlotWidth = 100; //store.getters.recommendedSlotWidth;
        var minSlotWidth = store.getters.minSlotWidth;
  
        if (unitCloset != unitSlots) {
          this.convert(unitSlots, unitCloset, recommendedSlotWidth);
          recommendedSlotWidth = this.valueConvertedSlotsWidth;
          this.convert(unitSlots, unitCloset, minSlotWidth);
          minSlotWidth = this.valueConvertedSlotsWidth;
        }
  
  
        ///var reason = store.state.reason;
  
        var recommendedNumberSlots = parseInt(widthCloset / recommendedSlotWidth);
        var remainder = widthCloset % recommendedSlotWidth;
        var remainderWidth =
          widthCloset - recommendedNumberSlots * recommendedSlotWidth;
  
        for (let i = 0; i < recommendedNumberSlots; i++) {
  
          store.dispatch(SET_SLOT_DIMENSIONS, {
            idSlot: i,
            width: recommendedSlotWidth,
            height: heightCloset,
            depth: depthCloset,
            unit: unitCloset
          });
        }
        if (remainder > 0 && remainderWidth >= 150 /*store.getters.minSlotWidth*/ ) {
  
          store.dispatch(SET_SLOT_DIMENSIONS, {
            idSlot: recommendedNumberSlots,
            width: remainderWidth,
            height: heightCloset,
            depth: depthCloset,
            unit: unitCloset
          });
        }
      },
      convert(from, to, value) {
        Axios.get(
            `http://localhost:5000/mycm/api/units/convert/?from=${from}&to=${to}&value=${value}`
          )
          .then(response => (this.valueConvertedSlotsWidth = response.data))
          .catch(error => {});
      },
    }
  };
</script>

<style>
  .containerDimensions {
    margin: 3%;
    margin-left: 22.5%;
    margin-right: 22.5%;
    margin-bottom: 1%;
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
    width: 70%;
    margin-bottom: 3%;
    margin-right: 15%;
  }
  
  .slider {
    margin-left: 15%;
    margin-right: 15%;
  }
</style>


