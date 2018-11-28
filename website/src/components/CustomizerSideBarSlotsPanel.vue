<template>
  <div >
    <label class="slotsSelections">
    <input  type="radio" id ="recommendedSlots" value="recommendedSlots" v-model="picked"> Recommended Number Slots
    </label>
    <label class="slotsSelections">
    <input  type="radio" id ="minSlots" value="minSlots" v-model="picked"> Min Number Slots
    </label>
    <label class="slotsSelections">
    <input  type="radio" id ="maxSlots" value="maxSlots" v-model="picked"> Max Number Slots
    </label>
    <label class="slotsSelections">
    <input  type="radio" id ="customizedSlots" value="customizedSlots" v-model="picked"> Customized Number Slots
    </label>
    <div v-if="displaySliders">
        <!--<i class="btn btn-primary material-icons"  >+</i>
        <i class="btn btn-primary material-icons"  >-</i>
        Espaço livre caralho
        <input  type="text" id ="freeSpace" value="freeSpace" v-model="freeSpace">-->
        <vue-slider ref="slider" v-model="value"></vue-slider>
    
    </div>
  </div>
</template>

<script>
import store from "./../store";
export default {
  name: "CustomizerSideBarSlotsPanel",
  data() {
    return {
      picked: ""
    };
  },
  computed: {
    recommendedNumberSlots() {
      return store.getters.width / store.getters.recommendedSlotSize.width;
    },
    minNumberSlots() {
      return store.getters.width / store.getters.maxSlotSize.width;
    },
    maxNumberSlots() {
      return store.getters.width / store.getters.minSlotSize.width;
    },
    displaySliders(){
      return this.picked === "customizedSlots";
    }
  },
  methods: {
    createSliders() {
      alert("vou criar sliders mas nao sei muito bem como ok?");
    }
  }
};
</script>
<style>
.slotsSelections {
  float: left;
  padding: 7px 20px
}
</style>

