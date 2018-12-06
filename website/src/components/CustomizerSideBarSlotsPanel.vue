<template>
  <div class="component">
    <label class="slotsSelections">
      <input type="radio" id="recommendedSlots" value="recommendedSlots" v-model="picked"> Recommended Number Slots
    </label>
    <label class="slotsSelections">
      <input type="radio" id="customizedSlots" value="customizedSlots" v-model="picked"> Customized Number Slots
    </label>
    <div v-if="displaySliders" class="slidersSection">
      <input type="text" :placeholder="freeSpaceValue" id="freeSpace" v-model="freeSpace" disabled>
      <i class="btn btn-primary material-icons" @click="removeLine(index)">-</i>
      <i class="btn btn-primary material-icons"  @click="addLine">+</i>
      <div class="slidersSection">
        <span v-for="n in minNumberSlots" :key="n">
          <vue-slider class="slidersSection"
            :min="minSizeSlot"
            :max="maxSizeSlot"
            :value="recommendedSizeSlot"
            v-model="sliderValue[n-1]"
          ></vue-slider>
        </span>
      <div v-for="(line, index) in lines.slice(0,maxNumberSlots)" v-bind:key="index">
        <vue-slider class="slidersSection"
          :min="minSizeSlot"
          :max="maxSizeSlot"
          :value="recommendedSizeSlot"
          v-model="sliderValues[index]"
        ></vue-slider>
      </div>
      </div>
    </div>
  </div>
</template>
<script>
import vueSlider from "vue-slider-component";
import store from "./../store";
import Axios from "axios";
import { SET_SLOT_DIMENSIONS } from "./../store/mutation-types.js";

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
      blockRemoval: true
    };
  },
  components: {
    vueSlider
  },
  computed: {
    freeSpaceValue() {
      ///for (n in this.sliderValue) {
      /// return parseInt(store.getters.width - this.sliderValue[n]);
      ///}
      return 200;
    },
    /*  recommendedNumberSlots() {
      ///if (store.getters.recommendedSlotSize.unit == store.getters.unit) {
      ///  return ( parseInt( store.getters.width / store.getters.recommendedSlotSize.width ) + 3);
      ///} else {
      ///convert(store.getters.unit,store.getters.recommendedSlotSize.unit,store.getters.recommendedSlotSize.width );
      /// return parseInt(store.getters.width / this.valueConverted) + 3;
      return 3;
    }, */
    minNumberSlots() {
      ///return parseInt(store.getters.width / store.getters.maxSlotSize);
      return 1;
    },
    maxNumberSlots() {
      ///return store.getters.width / minSizeSlot;
      return 7;
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
    },
    removeLine(lineId) {
      if (!this.blockRemoval) this.lines.splice(lineId, 1);
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
    /* var widthCloset = store.getters.width;
    var depthCloset = store.getters.depth;
    var heightCloset = store.getters.height;
    var unitCloset = store.getters.unit; */

    var widthCloset = 404.5;
    var depthCloset = 100;
    var heightCloset = 300;
    var unitCloset = "cm";

    var recommendedSlotWidth = store.getters.recommendedSlotWidth;

    var recommendedNumberSlots = parseInt(widthCloset / recommendedSlotWidth);
    var remainder = widthCloset % recommendedSlotWidth;

    var remainderWidth = widthCloset - (recommendedNumberSlots * recommendedSlotWidth);

    if (remainder>0 && remainderWidth<=store.getters.minSlotWidth){
      /* var addToMin = store.getters.minSlotWidth - remainder;
      recommendedNumberSlots--;
      var slotAn = re - addToMin
      if(slotAn>=store.getters.minSlotWidth){
        store.dispatch(SET_SLOT_DIMENSIONS, { 
        width: slotAn,
        height: heightCloset,
        depth: depthCloset,
        unit: unitCloset }); 

        store.dispatch(SET_SLOT_DIMENSIONS, { 
        width: store.getters.minSlotWidth,
        height: heightCloset,
        depth: depthCloset,
        unit: unitCloset }); 
      } */
    }
    for(let i=0; i<recommendedNumberSlots; i++){
      store.dispatch(SET_SLOT_DIMENSIONS, { 
        idSlot: i,
        width: recommendedSlotWidth,
        height: heightCloset,
        depth: depthCloset,
        unit: unitCloset }); 
    }
  } 
};
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
</style>