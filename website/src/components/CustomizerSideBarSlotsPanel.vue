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
      <i class="btn btn-primary material-icons" @click="removeLine(index)">remove</i>
      <i class="btn btn-primary material-icons" @click="addLine">add</i>
      <div class="slidersSection">
        <span v-for="n in (minNumberSlots - 1 )" :key="n">
          <vue-slider
            class="slidersSection" 
            :min="minSizeSlot"
            :max="maxSizeSlot"
            :value="slotWidthChange"
            v-model="sliderValue"
            @callback="updateWidthSlot"
          ></vue-slider>
        </span>
      </div>
      <div v-for="(line, index) in lines.slice(0, maxNumberSlots)" v-bind:key="index">
          <vue-slider
            class="slidersSection"
            :min="minSizeSlot"
            :max="maxSizeSlot"
            :value="slotWidthChange"
             v-model="sliderValue"
            @callback="updateWidthSlot"
          ></vue-slider>

          <!--v-model="sliderValues[index]"-->
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
import { ADD_SLOT_DIMENSIONS,SET_ID_SLOT, DEACTIVATE_CAN_MOVE_CLOSET, ACTIVATE_CAN_MOVE_SLOTS, DEACTIVATE_CAN_MOVE_SLOTS } from "./../store/mutation-types.js";

export default {
  name: "CustomizerSideBarSlotsPanel",
  data() {
    return {
      picked: "recommendedSlots",
      numberSlots: 0,
      sliderValue: this.minSizeSlot,
      sliderValues: [],
      lines: [],
      freeSpace: "",
      createNewSlider: false,
      valueConverted: "",
      blockRemoval: true,
      slotsToPost: [],
      slotsToAdd: [],
      listRecommendedSlots: [],
      listMinSlots: [],
      drawCustomizedSlots: [],
      slotsID: [],
      slotWidthChange: 0
    };
  },
  components: {
    vueSlider
  },
  computed: {
    freeSpaceValue() {
      return 200;
    },
    maxSizeSlot(){
      return store.getters.maxSlotWidth
    },
    minSizeSlot(){
       return store.getters.minSlotWidth
    },
    minNumberSlots(){
      var number = parseInt(/*store.state.customizedProduct.customizedDimensions.width*/6000 / store.getters.maxSlotWidth)
      return number;
    },
    maxNumberSlots(){
      var number = parseInt(/*store.state.customizedProduct.customizedDimensions.width*/6000 / store.getters.minSlotWidth) -1;
      return number;
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
       if (checkEmptyLines.length >= 1 && this.lines.length > 0){
         return;
       } 
      this.lines.push({
        slider: null
      });
      this.addSlot(this.lines.length);
    },
    addSlot(index){
      if(index <= this.maxNumberSlots){
        this.drawOneSlot();
      }else{
         this.$toast.open("Already reached the maximum number of slots"); 
      }
    },
    removeLine(lineId) {
      if (!this.blockRemoval) this.lines.splice(lineId, 1); this.removeSlot(this.lines.length);
    },
    removeSlot(index){
      if(index >= this.minNumberSlots)
      this.removeOneSlot();
    },
    nextPanel(){
      this.postSlots().then(() => {
        this.$emit("advance");
      }).catch((error_message)=>{
           this.$toast.open("There was an error adding slots, please try again"); 
      });
    },
    postSlots(){
      var widthCloset = 6000; //store.state.customizedProduct.customizedDimensions.width;
      var reasonW = store.state.resizeVectorGlobal.width;
      if(this.slotsToPost.length==0){
        for(let a = 0 ; a<store.state.customizedProduct.slots.length; a++){
          this.slotsToPost.push({
            height: store.state.customizedProduct.slots[a].height,
                    depth: store.state.customizedProduct.slots[a].depth,
                    width: store.state.customizedProduct.slots[a].width / reasonW,
                    unit: "mm" });// store.state.customizedProduct.customizedDimensions.unit
        }
      }
      let slotsToPost1 = [];
      for(let i = 0; i< this.slotsToPost.length - 1; i++){
        slotsToPost1.unshift(this.slotsToPost[i]);
      }
      return new Promise((accept,reject)=>{
        this.postSlot(slotsToPost1.slice())
        .then((customizedProduct) => {
          for(let i = 0; i< this.slotsToPost.length; i++){
              store.dispatch(SET_ID_SLOT, {
                idSlot: customizedProduct.slots[i].id,
                position: i});
          }
          accept(customizedProduct)
        })
        .catch((error_message) => { 
                reject(error_message)
        });
      })
    },
    postSlot(slotsToPost1){
      return new Promise((accept, reject) => {
        let slotToPost = slotsToPost1.pop();
        Axios.post(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}/slots`,
              {
                height: slotToPost.height,
                depth: slotToPost.depth,
                width: slotToPost.width,
                unit: slotToPost.unit
              }).then((customizedProduct) => {
                if(slotsToPost1.length > 0 ){
                  return this.postSlot(slotsToPost1)
                  .then((customizedProduct)=>{
                    accept(customizedProduct)})
                  .catch((error_message) => { reject(error_message)});
                }else{
                  accept(customizedProduct.data);
                }
              })
              .catch((error_message) => {
                  reject(error_message.response.data.message);
              });
      })
    },
    previousPanel(){
      store.dispatch(ADD_SLOT_DIMENSIONS); 
      this.$emit("back");
    },
    activateCanvasControls(){
      store.dispatch(ADD_SLOT_DIMENSIONS);
      this.getMinSlots();
      store.dispatch(ACTIVATE_CAN_MOVE_SLOTS);
    },
    deactivateCanvasControls(){
      store.dispatch(ADD_SLOT_DIMENSIONS);
      this.getRecommendedSlots()
      store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS)
    },
    getRecommendedSlots(){
      this.listRecommendedSlots = []
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
       this.slotsToPost = [];
              var widthCloset = 6000; //store.state.customizedProduct.customizedDimensions.width;
              var depthCloset = 2500; //store.state.customizedProduct.customizedDimensions.depth;
              var heightCloset = 5000; //store.state.customizedProduct.customizedDimensions.height;
              
              var unitCloset = "mm"//store.state.customizedProduct.customizedDimensions.unit;
              var unitSlots = store.getters.productSlotWidths.unit;

             /*  if(unitCloset != unitSlots){
                this.convert(unitSlots,unitCloset,recommendedSlotWidth);
                recommendedSlotWidth = this.valueConvertedSlotsWidth;
                this.convert(unitSlots,unitCloset,minSlotWidth);
                minSlotWidth = this.valueConvertedSlotsWidth;
              }  */
              var reasonW = store.state.resizeVectorGlobal.width;
              var reasonD = 100 / depthCloset;
              var reasonH = 300 / heightCloset;

              for (let i = 0; i < this.listRecommendedSlots.length; i++) {
                store.dispatch(ADD_SLOT_DIMENSIONS, {
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
     getMinSlots(){
        Axios.get(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}/minSlots`)
            .then(response => {
              
              this.listMinSlots = response.data;
              this.drawMinSlots();
            })
            .catch((error_message) => {
            
              this.$toast.open({
                message: error_message.response.data.message
              });
            });
    },
    drawMinSlots(){

      this.slotsToPost = [];
              var widthCloset = 6000; //store.state.customizedProduct.customizedDimensions.width;
              var depthCloset = 2500; //store.state.customizedProduct.customizedDimensions.depth;
              var heightCloset = 5000; //store.state.customizedProduct.customizedDimensions.height;
              
              var unitCloset = "mm"//store.state.customizedProduct.customizedDimensions.unit;
              var unitSlots = store.getters.productSlotWidths.unit;

             /*  if(unitCloset != unitSlots){
                this.convert(unitSlots,unitCloset,recommendedSlotWidth);
                recommendedSlotWidth = this.valueConvertedSlotsWidth;
                this.convert(unitSlots,unitCloset,minSlotWidth);
                minSlotWidth = this.valueConvertedSlotsWidth;
              } */
             var reasonW = store.state.resizeVectorGlobal.width;
              var reasonD = 100 / depthCloset;
              var reasonH = 300 / heightCloset;

              for (let i = 0; i < this.listMinSlots.length; i++) {
                store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: i,
                  width: this.listMinSlots[i].width * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
                });
              this.slotsToPost.push({
                    height: heightCloset,
                    depth: depthCloset,
                    width: this.listMinSlots[i].width,
                    unit: unitCloset});
              }
            
    },
    drawOneSlot(){
       Axios.post(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}/slots`,
              {
                height: this.slotsToPost[0].height,
                depth: this.slotsToPost[0].depth,
                width: store.getters.minSlotWidth,
                unit: this.slotsToPost[0].unit,
              }).then(() => {
                    this.drawCustomizedSlots = response.data
                })
              .catch((error_message) => {
                  error_message.response.data.message
              });
              var widthCloset = 6000; //store.state.customizedProduct.customizedDimensions.width;
              var depthCloset = 2500; //store.state.customizedProduct.customizedDimensions.depth;
              var heightCloset = 5000; //store.state.customizedProduct.customizedDimensions.height;
              
              var unitCloset = "mm"//store.state.customizedProduct.customizedDimensions.unit;
              var unitSlots = store.getters.productSlotWidths.unit;

              var min = store.getters.minSlotWidth;

             /*  if(unitCloset != unitSlots){
                this.convert(unitSlots,unitCloset,recommendedSlotWidth);
                recommendedSlotWidth = this.valueConvertedSlotsWidth;
                this.convert(unitSlots,unitCloset,minSlotWidth);
                minSlotWidth = this.valueConvertedSlotsWidth;
              }  */
              var reasonW = store.state.resizeVectorGlobal.width;
              var reasonD = 100 / depthCloset;
              var reasonH = 300 / heightCloset;

              
                store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: this.slotsToPost.length + 1,
                  width: min * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
                });
              this.slotsToPost.push({
                    height: heightCloset,
                    depth: depthCloset,
                    width: min,
                    unit: unitCloset});
              
  },
    removeOneSlot(){
      //store.dispatch(ADD_SLOT_DIMENSIONS, {removeSlot: 1});
      this.slotsToPost.pop();
    },
    updateWidthSlot(){
       var widthCloset = 6000; //store.state.customizedProduct.customizedDimensions.width;
              var depthCloset = 0; //store.state.customizedProduct.customizedDimensions.depth;
              var heightCloset = 0; //store.state.customizedProduct.customizedDimensions.height;

              var id = 0 ;
              
              var unitCloset = "mm"//store.state.customizedProduct.customizedDimensions.unit;
              
              var reasonW = store.state.resizeVectorGlobal.width;
      store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: id,
                  width: this.sliderValue * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
                });
                ///alert(store.state.customizedProduct.slots[0].height);
              /* this.slotsToPost.push({
                    height: heightCloset,
                    depth: depthCloset,
                    width: this.sliderValue * reasonW,
                    unit: unitCloset}); */
              
    }
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
    this.slotsToPost=[];
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