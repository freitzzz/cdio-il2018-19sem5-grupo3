<template>
  <div class="component">
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">In this step, you can add divisions to the closet's structure and customize their sizes.</span>
    </div>
    <div>
      <label class="slotsSelections">
        <input type="radio" id="recommendedSlots" value="recommendedSlots" v-model="picked"  @change="deactivateCanvasControls()"> Recommended Number Slots
      </label>
      <label class="slotsSelections">
        <input type="radio" id="customizedSlots" value="customizedSlots" v-model="picked"  @change="activateCanvasControls()" > Customized Number Slots
      </label>
    </div>
    <div v-if="displaySliders" class="slidersSection">
      <input type="text" :placeholder="freeSpaceValue" id="freeSpace" v-model="freeSpace" disabled>
      <i class="btn btn-primary material-icons" @click="removeLine(index)">-</i>
      <i class="btn btn-primary material-icons" @click="addLine">+</i>
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
    },
    nextPanel(){
      //TODO! POST slots
      this.$emit("advance");
    },
    previousPanel(){
      //TODO! DELETE slots

 store.dispatch(SET_SLOT_DIMENSIONS);
      this.$emit("back");
    },
    activateCanvasControls(){store.dispatch(ACTIVATE_CAN_MOVE_SLOTS);},
  deactivateCanvasControls(){
    //TODO! Desenhar slots recommendados quando checado o recommended 
    store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS)}
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
</style>