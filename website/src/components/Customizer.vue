<template>
  <div>
    <customizer-progress-bar></customizer-progress-bar>
    <customizer-side-bar></customizer-side-bar>
      <canvas ref="threeCanvas" @mouseup="onMouseUp" @mousedown="onMouseDown" @mousemove="onMouseMove" @keydown="onKeyDown" :width="initialWidth" :height="initialHeight">
      </canvas>
      
  </div>
</template>

<script>
import ProductRenderer from "./../3d/ProductRenderer.js";
import CustomizerSideBar from "./CustomizerSideBar";
import CustomizerProgressBar from "./CustomizerProgressBar.vue";
import Store from "./../store/index.js";

export default {
  name: "Customizer",
  data() {
    return {
      productRenderer: {}
    };
  },
  computed: {
    initialWidth() {
      return document.documentElement.clientWidth;
    },
    initialHeight() {
      return document.documentElement.clientHeight * 0.70;
    },
    slots(){
      var array= [];
      for(let i = 0; i < Store.state.customizedProduct.slots.length; i++){
        array.push(Store.getters.customizedProductSlotWidth(i));
      }
      return array;
      //return Store.getters.customizedProductSlotWidth;
    },
    loadProduct(){
      return Store.getters.productId;
    },

    updateDimensions(){
      return Store.getters.customizedProductDimensions;
    }

    /* addComponent(){
      var array= [];
      for(let i = 0; i < Store.state.customizedProduct.slots.length; i++){
        array.push(Store.getters.customizedProductComponents(i));
      }
      return array;
    } */
  },
  components: {
    CustomizerSideBar,
    CustomizerProgressBar
  },
  watch: {
    slots: function(newValue, oldValue){
      this.productRenderer.addSlotNumbered(newValue);
    },
    loadProduct: function(){
      this.productRenderer.showCloset();
    },
    updateDimensions: function(){
      alert("ohhh :(");
      this.productRenderer.changeClosetDimensions(Store.getters.customizedProductDimensions.width, Store.getters.customizedProductDimensions.height ,Store.getters.customizedProductDimensions.depth);
    },
    addComponent: function(component){
        this.productRenderer.addComponent(component);
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
    onKeyDown: function(event){
      alert("keydown");
      this.productRenderer.onKeyDown(event);
              event.preventDefault()

    }
    /* addRecommendedSlots: function(){
      this.productRenderer.addSlot();
    } */

/* const unwatch = this.$store.watch(() => this.$store.getters.authenticatedAndOnline, res => {
  console.log(unwatch) // => undefined
  unwatch()
}, {
  immediate: true
}) */
     /*reloadCube() {
      var event = new CustomEvent("changeDimensions", {
        detail: {
          height: height,
          width: width,
          depth: depth
        }
      });
      document.dispatchEvent(event);
    },*/
 /* reloadClosetSlots() {
      var slotDiv = document.getElementById("slotDiv");
      var length = slotDiv.childNodes.length - 1;
      var list = [];
      while (length >= 1) {
        list.push(slotDiv.childNodes[length].childNodes[1].nodeValue);
        length--;
      }
      var eventX = new CustomEvent("changeSlots", {
        detail: {
          slots: document.getElementById("slotsInput").value,
          slotWidths: list
        }
      });
      document.dispatchEvent(eventX);
      manageSlotSliders();
    },
    reloadMaterial(elementId) {
      var event = new CustomEvent("changeMaterial", {
        detail: {
          //this should the be image's link
          material: document.getElementById(elementId).getAttribute("src")
        }
      });
      document.dispatchEvent(event);
    },
    changeShininess(shininess) {
      var event = new CustomEvent("changeShininess", {
        detail: {
          shininess: shininess
        }
      });
      document.dispatchEvent(event);
    },
    changeColor(color) {
      var event = new CustomEvent("changeColor", {
        detail: {
          color: color
        }
      });
    } */
  },
  mounted() {
    var canvas = this.$refs.threeCanvas;
    this.productRenderer = new ProductRenderer(canvas);
  },
};
</script>
