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
      :width="canvasWidth"
      :height="canvasHeight"
    ></canvas>
  </div>
</template>

<script>
import Store from "./../store/index.js";
import CustomizerSideBar from "./CustomizerSideBar";
import ProductRenderer from "./../3d/ProductRendererTemp.js";
import { SET_DOORS_FLAG, SET_COMPONENT_TO_REMOVE } from "./../store/mutation-types.js";
import CustomizerProgressBar from "./CustomizerProgressBar.vue";

export default {
  name: "Customizer",
  data() {
    return {
      productRenderer: ProductRenderer.prototype,
      currentStage: 0,
      canvasWidth: document.documentElement.clientWidth,
      canvasHeight: document.documentElement.clientHeight * 0.7
    };
  },
  computed: {
    slots() {
      var array = [];
      for (let i = 0; i < Store.state.customizedProduct.slots.length - 1; i++) {
        //!WARN do NOT remove this line
        Store.state.customizedProduct.slots[i].width;
        array.push(Store.getters.customizedProductSlot(i));
      }
      return array;
    },
    loadProduct() {
      return Store.getters.productId;
    },
    updateDimensions() {
      return Store.getters.customizedProductDimensions;
    },
    manageComponents() {
      return Store.getters.customizedProductComponents;
    },
    applyMaterial() {
      return Store.getters.customizedMaterial;
    },
    applyColor() {
      return Store.getters.customizedMaterialColor;
    },
    applyFinish() {
      return Store.getters.customizedMaterialFinish;
    },
    canMoveCloset() {
      return Store.getters.canMoveCloset;
    },
    canMoveSlots() {
      return Store.getters.canMoveSlots;
    },
    canMoveComponents() {
      return Store.getters.canMoveComponents;
    },
    controlDoorsFlag() {
      return Store.getters.doorsFlag;
    },
    componentToEditMaterial(){
      return Store.getters.componentToEditMaterial;
    },
    populateWebsiteDimensions() {
      return Store.getters.resizeFactorDimensions;
    }
  },
  components: {
    CustomizerSideBar,
    CustomizerProgressBar
  },
  watch: {
    populateWebsiteDimensions: function(newValue) {
      this.productRenderer.populateWebsiteDimensions(newValue);
    },
    slots: function(newValue, oldValue) {
      if(newValue.length > 0){
        let index = 0;
         for(let i = 0; i< newValue.length; i++){
          if(newValue[i].height == 0){
            index = i;
          }
        }
        let size = newValue.length - 1
        if(newValue[index].height == 0){
          this.productRenderer.moveSlotSlider(index, newValue[index].width)
        }
        if(newValue[size].height > 0){ 
          this.productRenderer.removeAllSlots();
          this.productRenderer.addSlotNumbered(newValue);
         } 
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
    manageComponents: function(newValue, oldValue) {
      if (newValue.length == 0) {
        this.productRenderer.removeAllComponents();
      } else if(newValue.length > oldValue.length){ //Adds a component
        this.productRenderer.generateComponent(newValue[newValue.length - 1].component);
      } else if(newValue.length < oldValue.length){ //Removes a component
        this.productRenderer.removeComponent(Store.getters.componentToRemove);
      } 
    },
    applyMaterial: function(newValue) {
      this.productRenderer.applyTexture("./src/assets/materials/" + newValue);
    },
    applyColor: function(newValue) {
      this.productRenderer.applyColor(newValue);
    },
    applyFinish: function(newValue) {
      this.productRenderer.applyFinish(newValue);
    },
    canMoveCloset(newValue) {
      this.productRenderer.canMoveCloset = newValue;
    },
    canMoveSlots(newValue) {
      this.productRenderer.canMoveSlots = newValue;
    },
    canMoveComponents(newValue) {
      this.productRenderer.canMoveComponents = newValue;
    },
    controlDoorsFlag(newValue) {
      if (newValue == "CLOSET_HAS_HINGED_DOORS")
        this.$toast.open("There are closet slots that have hinged doors!");
      if (newValue == "CLOSET_HAS_SLIDING_DOORS")
        this.$toast.open("The closet already has sliding doors!");
      if (newValue == "SLOT_HAS_DOOR")
        this.$toast.open("This slot already has a door!");
      Store.dispatch(SET_DOORS_FLAG, { flag: "NONE" });
    },
    componentToEditMaterial(newValue){
      if(!newValue) return;
      this.productRenderer.applyComponentMaterial(newValue, Store.getters.componentToEdit);
    }
  },
  methods: {
    /**
     * Mouse move event handler propagated to the instance of ProductRenderer.
     */
    onMouseMove: function(event) {
      this.productRenderer.onMouseMove(event, this.$refs.threeCanvas);
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
      //*This doesn't seem to work since the canvas is currently not focusable, 
      //*so the keydown is detected by the window instead
      alert("keydown");
      this.productRenderer.onKeyDown(event);
      event.preventDefault();
    },
    /**
     * Window resize event handler, which updates the canvas's size when the window size changes.
     */
    onWindowResize(){
      this.canvasWidth = document.documentElement.clientWidth;
      this.canvasHeight = document.documentElement.clientHeight * 0.7;

      this.productRenderer.resizeRenderer(this.canvasWidth, this.canvasHeight);
    },
    drop: function(event){
      event.preventDefault();
      this.productRenderer.renderDroppedComponent(
        event,
        this.$refs.threeCanvas
      );
    },
    allowDrop: function(event) {
      event.preventDefault();
    },
    changeProgressBarStage: function(currentPanelIndex) {
      this.currentStage = currentPanelIndex;
    }
  },
  mounted() {
    var canvas = this.$refs.threeCanvas;
    this.productRenderer = new ProductRenderer(canvas);

    this.$nextTick(function(){
      window.addEventListener("resize", this.onWindowResize);
    });
  },

  beforeDestroy(){
    window.removeEventListener("resize", this.onWindowResize);
  }
};
</script>
