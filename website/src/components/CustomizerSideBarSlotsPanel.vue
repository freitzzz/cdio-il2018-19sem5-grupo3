<template>
  <div>
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">In this step, you can add divisions to the closet's structure and customize their sizes.</span>
    </div>
    <div class="slotsSelections">
      <label >
        <input type="radio" id="recommendedSlots" value="recommendedSlots" v-model="picked"  @change="deactivateCanvasControls()"> Recommended Number Slots
      </label>
      <label >
        <input type="radio" id="customizedSlots" value="customizedSlots" v-model="picked"  @change="activateCanvasControls()" > Customized Number Slots
      </label>
    </div>
    <div v-if="displaySliders" class="slidersSection">
      <input  class="slidersSection" size = 13  type="text" :placeholder="freeSpaceValue" id="freeSpace" v-model="freeSpace" disabled>
      <i class="btn btn-primary material-icons" @click="removeLine(index)">remove</i>
      <i class="btn btn-primary material-icons" @click="addLine">add</i>
      <div class="slidersSection">
        <span v-for="n in recommendedNumberSlots" :key="n">
          <vue-slider
            class="slidersSection"
            :min="minSizeSlot"
            :max="maxSizeSlot"
            :value="recommendedSizeSlot"
            v-model="sliderValue[n-1]"
          ></vue-slider>
        </span>
      </div>
      <div v-for="(line, index) in lines.slice(0,maxNumberSlots)" v-bind:key="index">
          <vue-slider
            class="slidersSection"
            :min="minSizeSlot"
            :max="maxSizeSlot"
            :value="recommendedSizeSlot"
            v-model="sliderValues[index]"
          ></vue-slider>
      </div>
    </div>
    <div class="center-controls">
      <i class="btn btn-primary material-icons" @click="previousPanel()" >arrow_back</i>
      <i class="btn btn-primary material-icons" @click="nextPanel()" >arrow_forward</i>
    </div>
  </div>
</template>
<script>
import vueSlider from "vue-slider-component";
import store from "./../store";
import Axios from "axios";
 import {MYCM_API_URL} from "./../config.js";
import { SET_SLOT_DIMENSIONS, DEACTIVATE_CAN_MOVE_CLOSET, ACTIVATE_CAN_MOVE_SLOTS, DEACTIVATE_CAN_MOVE_SLOTS } from "./../store/mutation-types.js";

export default {
  name: "CustomizerSideBarSlotsPanel",
  data() {
    return {
      picked: "recommendedSlots",
      numberSlots: 0,
      sliderValue: [],
      sliderValues: [],
      lines: [],
      freeSpace: "",
      createNewSlider: false,
      valueConverted: "",
      blockRemoval: true,
      slotsToPost:[],
      slotsToAdd: [],
      listRecommendedSlots: []
    };
  },
  components: {
    vueSlider
  },
  computed: {
    freeSpaceValue() {
      return 200;
    },
    minNumberSlots() {
      ///return parseInt(store.getters.width / store.getters.maxSlotSize);
      return 1;
    },
    maxNumberSlots() {
      ///return store.getters.width / minSizeSlot;
      return 3;
    },
    displaySliders() {
      return this.picked === "customizedSlots";
    }
  },
  methods: {
    createMoreSliders() {
      this.ceateNewSlider = true;
      this.recommendedNumberSlots++;
    },
    deactivateSliderCreation() {
      this.createNewSlider = false;
    },
    convert(from, to, value) {
      Axios.get(
        `http://localhost:5000/mycm/api/units/convert/?from=${from}&to=${to}&value=${value}`
      )
        .then(response => (this.valueConverted = response.data))
        .catch(error => {});
    },
    addLine() {
      let checkEmptyLines = this.lines.filter(line => line.number === null);
      if (checkEmptyLines.length >= 1 && this.lines.length > 0) return;
      this.lines.push({
        slider: null
      });
      this.drawOneSlot();
    },
    removeLine(lineId) {
      if (!this.blockRemoval) this.lines.splice(lineId, 1);
    },
    nextPanel(){
      this.postSlots().then(() => {
        this.$emit("advance")
      }).catch((error_message)=>{
           this.$toast.open({
              message: error_message
          }); 
      });
    },
    postSlots(){
      let postedSlots=0;
      let slotPosts = new Promise((accept, reject) => {
          for(let i=0; i<this.slotsToPost.length; i++){
          Axios.post(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}/slots`,
            {
              height: this.slotsToPost[i].height,
              depth: this.slotsToPost[i].depth,
              width: this.slotsToPost[i].width,
              unit: this.slotsToPost[i].unit
            }).then(() => {
               postedSlots++;
              if(postedSlots == this.slotsToPost.length)  accept();
            })
            .catch((error_message) => {
                reject(error_message.response.data.message);
            });
        }
      })
      return slotPosts;
    },
    previousPanel(){
      //TODO! DELETE slots
      store.dispatch(SET_SLOT_DIMENSIONS);
      this.$emit("back");
    },
    activateCanvasControls(){

      store.dispatch(SET_SLOT_DIMENSIONS);
      this.drawMinSlots()
      store.dispatch(ACTIVATE_CAN_MOVE_SLOTS);
    },
    deactivateCanvasControls(){
      store.dispatch(SET_SLOT_DIMENSIONS);
      this.getRecommendedSlots()
      store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS)
      },
      getRecommendedSlots(){
        Axios.get(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}/recommendedSlots`)
            .then(response => {
              
              this.listRecommendedSlots = response.data;
              this.drawRecommendedSlots();
            })
            .catch((error_message) => {
            
              this.$toast.open({
                message: error_message.response.data.message
              });
            });
      },
      drawRecommendedSlots(){
              var widthCloset = 6000;/*store.state.customizedProduct.customizedDimensions.width;*/ ///404.5;
              var depthCloset = 2500;/*store.state.customizedProduct.customizedDimensions.depth;*/ ///100;
              var heightCloset = 5000; /*store.state.customizedProduct.customizedDimensions.height;*/ ///300;
              
              var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
              var unitSlots = store.getters.productSlotWidths.unit;

             /*  if(unitCloset != unitSlots){
                this.convert(unitSlots,unitCloset,recommendedSlotWidth);
                recommendedSlotWidth = this.valueConvertedSlotsWidth;
                this.convert(unitSlots,unitCloset,minSlotWidth);
                minSlotWidth = this.valueConvertedSlotsWidth;
              }  */

              var reasonW = 404.5 / widthCloset;
              var reasonD = 100 / depthCloset;
              var reasonH = 300 / heightCloset;

              for (let i = 0; i < this.listRecommendedSlots.length; i++) {
                store.dispatch(SET_SLOT_DIMENSIONS, {
                  idSlot: i,
                  width: this.listRecommendedSlots[i].width * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
                });

                this.slotsToPost.push({
                    height: heightCloset,
                    depth: depthCloset,
                    width: this.listRecommendedSlots[i].width,
                    unit: unitCloset});
              } 
      },
      // drawMinSlots(){
       
      //         var widthCloset = 6000;/*store.state.customizedProduct.customizedDimensions.width;*/ ///404.5;
      //         var depthCloset = 2500;/*store.state.customizedProduct.customizedDimensions.depth;*/ ///100;
      //         var heightCloset = 5000; /*store.state.customizedProduct.customizedDimensions.height;*/ ///300;
              

      //         var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
      //         var unitSlots = store.getters.productSlotWidths.unit;

      //         var recommendedSlotWidth = store.getters.recommendedSlotWidth;
      //         var maxSlotWidth = 3000;//store.getters.maxSlotWidth;
      //         var minSlotWidth = store.getters.minSlotWidth;

      //         if(unitCloset != unitSlots){
      //           this.convert(unitSlots,unitCloset,recommendedSlotWidth);
      //           recommendedSlotWidth = this.valueConvertedSlotsWidth;
      //           this.convert(unitSlots,unitCloset,minSlotWidth);
      //           minSlotWidth = this.valueConvertedSlotsWidth;
      //         } 
      //         var reasonW = 404.5 / widthCloset;
      //         var reasonD = 100 / depthCloset;
      //         var reasonH = 300 / heightCloset;

      //         var minNumberSlots = parseInt(widthCloset / maxSlotWidth);
      //          var remainder = widthCloset % maxSlotWidth;
      //         var remainderWidth =
      //           widthCloset - minNumberSlots * maxSlotWidth;
                
      //         for (let i = 0; i < minNumberSlots; i++) {
      //           alert()
      //           store.dispatch(SET_SLOT_DIMENSIONS, {
      //             idSlot: i,
      //             width: maxSlotWidth * reasonW,
      //             height: heightCloset,
      //             depth: depthCloset,
      //             unit: unitCloset
      //           });


      //            this.slotsToPost.push({
      //             height: heightCloset,
      //             depth: depthCloset,
      //             width: maxSlotWidth,
      //             unit: unitCloset});
      //         } 
      //     if(remainder>0){
      //         store.dispatch(SET_SLOT_DIMENSIONS, {
      //             idSlot: minNumberSlots,
      //             width: remainderWidth * reasonW,
      //             height: heightCloset,
      //             depth: depthCloset,
      //             unit: unitCloset
      //           });
      //           this.slotsToPost.push({
      //             height: heightCloset,
      //             depth: depthCloset,
      //             width: remainderWidth,
      //             unit: unitCloset});
      //     }
      // },
      //  drawOneSlot(){
       
      //         var widthCloset = 6000;/*store.state.customizedProduct.customizedDimensions.width;*/ ///404.5;
      //         var depthCloset = 2500;/*store.state.customizedProduct.customizedDimensions.depth;*/ ///100;
      //         var heightCloset = 5000; /*store.state.customizedProduct.customizedDimensions.height;*/ ///300;
              

      //         var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
      //         var minSlotWidth = store.getters.minSlotWidth;
             
      //         var reasonW = 404.5 / widthCloset;
      //         var size = this.slotsToPost.length;

      //         for(let i = 0; i < size; i++){
                
      //           store.dispatch(SET_SLOT_DIMENSIONS, {
      //             idSlot: i,
      //             width: this.slotsToPost[i].width * reasonW,
      //             height: heightCloset,
      //             depth: depthCloset,
      //             unit: unitCloset
      //           });
      //            this.slotsToPost.push({
      //             height: heightCloset,
      //             depth: depthCloset,
      //             width: minSlotWidth,
      //             unit: unitCloset});
      //         }
      //       store.dispatch(SET_SLOT_DIMENSIONS, {
      //             idSlot: i,
      //             width: minSlotWidth * reasonW,
      //             height: heightCloset,
      //             depth: depthCloset,
      //             unit: unitCloset
      //           });
      //            this.slotsToPost.push({
      //             height: heightCloset,
      //             depth: depthCloset,
      //             width: minSlotWidth,
      //             unit: unitCloset});
      // },
  },
  watch: {
    lines() {
      this.blockRemoval = this.lines.length <= 1;
    }
  },
  mounted() {
    this.addLine();
  },
  created() {
    store.dispatch(DEACTIVATE_CAN_MOVE_CLOSET);
  },
}
</script>
<style>
.slotsSelections {
  float: left;
  padding: 7px 20px;
}
.slidersSection {
  margin-bottom: 13%;
  width: 7px 30px;
  margin-left: 5%;
  margin-right: 5%;
}
.component {
  margin-bottom: 31%;
}

.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
  top: 25px;
  left: 0px;
  right: 0px;
}

.icon-div-top:hover .tooltiptext {
  visibility: visible;
}

.icon-div-top {
  top: 15px;
  left: 15px;
  margin-left: 90px;
  position: absolute;
}
.allignButtons {
  margin-left: 150px ;
}
</style>