<template>
  <div>
    <customizer-progress-bar :stageIndex="currentStage"></customizer-progress-bar>
    <customizer-side-bar @changeStage="changeProgressBarStage"></customizer-side-bar>
    <canvas
      ref="threeCanvas"
      @drop="drop"
      @dragover="allowDrop"
      @mouseup="onMouseUp"
      @mousedown="onMouseDown"
      @mousemove="onMouseMove"
      @keydown="onKeyDown"
      :width="initialWidth"
      :height="initialHeight"
    ></canvas>
  </div>
</template>

<script>
import Vue from "vue";
import Store from "./../store/index.js";
import CustomizerSideBar from "./CustomizerSideBar";
import ProductRenderer from "./../3d/ProductRendererTemp.js";
import { SET_DOORS_FLAG } from "./../store/mutation-types.js";
import CustomizerProgressBar from "./CustomizerProgressBar.vue";
import Toasted from "vue-toasted";

Vue.use(Toasted);

export default {
  name: "Customizer",
  data() {
    return {
      productRenderer: {},
      currentStage: 0
    };
  },
  computed: {
    initialWidth() {
      return document.documentElement.clientWidth;
    },
    initialHeight() {
      return document.documentElement.clientHeight * 0.7;
    },
    slots() {
      var array = [];
      for (let i = 0; i < Store.state.customizedProduct.slots.length - 1; i++) {
        array.push(Store.getters.customizedProductSlotWidth(i));
      }
      return array;
    },
    loadProduct() {
      return Store.getters.productId;
    },
    updateDimensions() {
      return Store.getters.customizedProductDimensions;
    },
    addComponent() {
      return Store.getters.customizedProductComponents;
    },
    removeComponent(){
      return Store.getters.componentToRemove;
    },
    applyMaterial(){
      return Store.getters.customizedMaterial;
    },
    applyColor(){
      return Store.getters.customizedMaterialColor;
    },
    applyFinish(){
      return Store.getters.customizedMaterialFinish;
    },
    canMoveCloset(){
      return Store.getters.canMoveCloset;
    },
    canMoveSlots(){
      return Store.getters.canMoveSlots;
    },
    canMoveComponents(){
      return Store.getters.canMoveComponents;
    },
    controlDoorsFlag(){
      return Store.getters.doorsFlag;
    },
    populateWebsiteDimensions(){
      return Store.getters.resizeFactorDimensions;
    }
  },
  components: {
    CustomizerSideBar,
    CustomizerProgressBar
  },
  watch: {
    populateWebsiteDimensions : function(newValue){
      this.productRenderer.populateWebsiteDimensions(
        newValue
      );

    },
    slots: function(newValue, oldValue) {
      if(newValue.length > 0){
        this.productRenderer.removeAllSlots();
        this.productRenderer.addSlotNumbered(newValue);
      } else {
        this.productRenderer.removeAllSlots();
      }
    },
    loadProduct: function() {
      this.productRenderer.showCloset();
    },
    updateDimensions: function() {
      this.productRenderer.changeClosetDimensions(
        Store.getters.customizedProductDimensions.width,
        Store.getters.customizedProductDimensions.height,
        Store.getters.customizedProductDimensions.depth
      );
    },
    addComponent: function(newValue, oldValue) {
      if(oldValue.length <= newValue.length){
        this.productRenderer.addComponent(newValue[newValue.length - 1]);
      } else if(newValue.length == 0) {
        this.productRenderer.removeAllComponents();
      }
    },
    removeComponent: function(newValue){
      console.log(newValue)
      if(newValue){
        if(confirm('Are you sure you want to remove the selected component?')) this.productRenderer.removeComponent(newValue);
      }
    },
    applyMaterial: function(newValue) {
      this.productRenderer.applyTexture("./src/assets/materials/" + newValue);
    },
    applyColor: function(newValue){ 
      this.productRenderer.applyColor(newValue);
    },
    applyFinish: function(newValue){
      this.productRenderer.applyFinish(newValue);
    },
    canMoveCloset(newValue){
      this.productRenderer.canMoveCloset = newValue;
    },
    canMoveSlots(newValue){
      this.productRenderer.canMoveSlots = newValue;
    },
    canMoveComponents(newValue){
      this.productRenderer.canMoveComponents = newValue;
    },
    controlDoorsFlag(newValue){
      if(newValue == "CLOSET_HAS_HINGED_DOORS") this.$toast.open("There are closet slots that have hinged doors!");
      if(newValue == "CLOSET_HAS_SLIDING_DOORS") this.$toast.open("The closet already has sliding doors!");
      if(newValue == "SLOT_HAS_DOOR") this.$toast.open("This slot already has a door!");
      Store.dispatch(SET_DOORS_FLAG, {flag : "NONE"});
    }
  },
  methods: {
    /**
     * Mouse move event handler propagated to the instance of ProductRenderer.
     */
    onMouseMove: function(event) {
      this.productRenderer.onMouseMove(event);
    },
    /**
     * Mouse click release event handler propagated to the instance of ProductRenderer.
     */
    onMouseUp: function(event) {
      this.productRenderer.onMouseUp(event);
    },
    /**
     * Mouse click press/hold event handler propagated to the instance of ProductRenderer.
     */
    onMouseDown: function(event) {
      this.productRenderer.onMouseDown(event);
    },
    /**
     * Keyboard click event handler propagated to the instance of ProductRenderer.
     */
    onKeyDown: function(event) {
      alert("keydown");
      this.productRenderer.onKeyDown(event);
      event.preventDefault();
    },
    drop: function(event){
      event.preventDefault();
      this.productRenderer.renderDroppedComponent(event, this.$refs.threeCanvas);
    },
    allowDrop: function(event){
      event.preventDefault();
    },
    changeProgressBarStage: function(currentPanelIndex){
      this.currentStage = currentPanelIndex;
    }
  },
  mounted() {
    var canvas = this.$refs.threeCanvas;
    this.productRenderer = new ProductRenderer(canvas);
  }
};
</script>
