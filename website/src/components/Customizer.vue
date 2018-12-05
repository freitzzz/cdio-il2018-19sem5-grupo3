<template>
  <div>
    <customizer-progress-bar></customizer-progress-bar>
    <customizer-side-bar></customizer-side-bar>
      <canvas ref="threeCanvas" @mouseup="onMouseUp" @mousedown="onMouseDown" @mousemove="onMouseMove" :width="initialWidth" :height="initialHeight">
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
      return Store.getters.customizedProductSlotWidth;
    },
    loadProduct(){
      return Store.getters.productId;
    }
  },
  components: {
    CustomizerSideBar,
    CustomizerProgressBar
  },
  watch: {
    slots: function(newValue, oldValue){
      this.productRenderer.addSlot(newValue);
    },
    loadProduct: function(){
      this.productRenderer.showCloset();
    }
  },
  //*Change the functions so that they don't access the DOM
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
