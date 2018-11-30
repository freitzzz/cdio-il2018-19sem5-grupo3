<template>
  <div>
    <label class="slotsSelections">
      <input type="radio" id="recommendedSlots" value="recommendedSlots" v-model="picked"> Recommended Number Slots
    </label>
    <label class="slotsSelections">
      <input type="radio" id="customizedSlots" value="customizedSlots" v-model="picked"> Customized Number Slots
    </label>
    <div v-if="displaySliders">
      <!--<input type="text" id="freeSpace" value="freeSpace" v-model="freeSpace">
      <div>
        <i class="btn btn-primary material-icons" @click="createMoreSliders()" >+</i>
        <i class="btn btn-primary material-icons">-</i>
        <span v-for="n in recommendedNumberSlots" :key="n">
          <vue-slider :min="minSizeSlot" :max="maxSizeSlot" v-model="sliderValue[n-1]"></vue-slider>
        </span>
        <span v-if="createNewSlider">
          <vue-slider :min="minSizeSlot" :max="maxSizeSlot" v-model="slideV"></vue-slider>
        </span>

        <div v-for="(line, index) in lines" v-bind:key="index">
      </div>-->
      <div v-for="(line, index) in lines" v-bind:key="index">
        <input v-model="line.countryCode">
        <i class="btn btn-primary material-icons" @click="removeLine(index)"></i>
        <i v-if="index + 1 === lines.length" @click="addLine" ></i>
      </div>
    </div>
  </div>
</template>
<script>
import vueSlider from "vue-slider-component";
import store from "./../store";
import Axios from "axios";
export default {
  name: "CustomizerSideBarSlotsPanel",
  data() {
    return {
      picked: "recommendedSlots",
      numberSlots: 0,
      sliderValue: [],
      freeSpace: "",
      createNewSlider: false,
      valueConverted: 4,
      lines: [],
      blockRemoval: true,
      phoneUsageTypes: [
        {
          label: "Home",
          value: "home"
        },
        {
          label: "Work",
          value: "work"
        },
        {
          label: "Mobile",
          value: "mobile"
        },
        {
          label: "Fax",
          value: "fax"
        }
      ],
      countryPhoneCodes: [
        {
          label: "+90",
          value: "+90"
        },
        {
          label: "+1",
          value: "+1"
        }
      ]
    };
  },
  components: {
    vueSlider
  },
  computed: {
    recommendedNumberSlots() {
      ///if (store.getters.recommendedSlotSize.unit == store.getters.unit) {
      ///  return ( parseInt( store.getters.width / store.getters.recommendedSlotSize.width ) + 3);
      ///} else {
      ///convert(store.getters.unit,store.getters.recommendedSlotSize.unit,store.getters.recommendedSlotSize.width );
      /// return parseInt(store.getters.width / this.valueConverted) + 3;
      return 3;
    },
    minNumberSlots() {
      return parseInt(store.getters.width / store.getters.maxSlotSize.width);
    },
    maxNumberSlots() {
      return store.getters.width / store.getters.minSlotSize.width;
    },
    minSizeSlot() {
      return store.getters.minSlotSize.width;
    },
    maxSizeSlot() {
      return store.getters.maxSlotSize.width;
    },
    recommendedSizeSlot() {
      return store.getters.recommendedSlotSize.width;
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
    removeSliders() {},
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
        countryCode: null,
        number: null,
        phoneUsageType: null
      });
    },
    addLine() {
      let checkEmptyLines = this.lines.filter(line => line.number === null);
      if (checkEmptyLines.length >= 1 && this.lines.length > 0) return;
      this.lines.push({
        countryCode: null,
        number: null,
        phoneUsageType: null
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
  }
};
</script>
<style>
.slotsSelections {
  float: left;
  padding: 7px 20px;
}
</style>

