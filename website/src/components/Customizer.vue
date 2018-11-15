<template>
</template>

<script>
export default {
  data() {},
  //*Change the functions so that they don't access the DOM
  methods: {
    loadCustomizer() {
      alert("load the customizer canvas");
    },
    reloadCube() {
      var event = new CustomEvent("changeDimensions", {
        detail: {
          height: height,
          width: width,
          depth: depth
        }
      });
      document.dispatchEvent(event);
    },
    reloadClosetSlots() {
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
    }
  }
};
</script>
