<template>
    <div>
      <i ref="menuButton" @click="openNav()" class="material-icons md-36 btn" style="visibility:hidden">menu</i>
      <div ref="customizerSidenav" class="sidenav">
        <i class="closebtn material-icons md-24" @click="closeNav()">close</i>
        <h3>{{panels[currentPanelIndex].title}}</h3>
        <!--The child component changes dinamically depending on tthe currently selected component.-->
        <!--The Sidebar component listens out for any event that the child may trigger-->
        <customizer-side-bar-products-panel v-if="currentPanelIndex == 0" @advance="selectProduct"></customizer-side-bar-products-panel>
        <customizer-side-bar-dimensions-panel v-if="currentPanelIndex == 1"></customizer-side-bar-dimensions-panel>
        <customizer-side-bar-slots-panel v-if="currentPanelIndex == 2"></customizer-side-bar-slots-panel>
        <customizer-side-bar-materials-panel v-if="currentPanelIndex == 3"></customizer-side-bar-materials-panel>
        <div class="sidenav-controls">
          <i class="btn btn-primary material-icons" @click="previousPanel()" v-if="canDisplayPreviousButton">arrow_back</i>
          <i class="btn btn-primary material-icons" @click="nextPanel()" v-if="canDisplayNextButton">arrow_forward</i>
        </div>
      </div>
    </div>
</template>

<script>
import CustomizerSideBarProductsPanel from "./CustomizerSideBarProductsPanel";
import CustomizerSideBarDimensionsPanel from "./CustomizerSideBarDimensionsPanel";
import CustomizerSideBarMaterialsPanel from "./CustomizerSideBarMaterialsPanel";
import CustomizerSideBarSlotsPanel from "./CustomizerSideBarSlotsPanel";
import Store from "./../store/index.js";

export default {
  name: "CustomizerSideBar",
  data() {
    return {
      currentPanelIndex: 0,
      panels: [
        {
          title: "Structures",
          enabled: true
        },
        {
          title: "Dimensions",
          enabled: true
        },
        {
          title: "Divisions",
          enabled: false
        },
        {
          title: "Materials",
          enabled: true
        },
        {
          title: "Components",
          enabled: false
        },
        {
          title: "Payment",
          enabled: true
        }
      ]
    };
  },
  components: {
    CustomizerSideBarProductsPanel,
    CustomizerSideBarDimensionsPanel,
    CustomizerSideBarMaterialsPanel,
    CustomizerSideBarSlotsPanel
  },
  computed: {
    /**
     * Checks if the previous button can be displayed.
     */
    canDisplayPreviousButton() {
      return this.currentPanelIndex > 0;
    },
    /**
     * Checks if the next button can be displayed.
     */
    canDisplayNextButton() {
      //Do not display next button for the Products tab nor for the last tab
      return (
        this.currentPanelIndex > 0 &&
        this.currentPanelIndex < this.panels.length - 1
      );
    }
  },
  methods: {
    /**
     * Fades out the menu button and opens the sidenav
     */
    openNav() {
      this.$refs.menuButton.style.visibility = "hidden";
      this.$refs.customizerSidenav.style.width = "250px";
    },
    /**
     * Closes the sidenav and fades in the menu button
     */
    closeNav() {
      this.$refs.customizerSidenav.style.width = "0";
      this.$refs.menuButton.style.visibility = "visible";
    },
    /**
     * Go to the previous panel.
     */
    previousPanel() {
      if (this.currentPanelIndex > 0) {
        while(this.currentPanelIndex > 0  && !this.panels[--this.currentPanelIndex].enabled);
      }
    },
    /**
     * Go to the next panel.
     */
    nextPanel() {
      if (this.currentPanelIndex < this.panels.length - 1) {
        while(this.currentPanelIndex < this.panels.length && !this.panels[++this.currentPanelIndex].enabled);
      }
    },
    /**
     * Checks the product's slots and components properties and sets the flags accordingly and advances to the next stage.
     */
    selectProduct(){
      //enable the slots panel if the product has slots
      if(Store.getters.productSlotSizes != undefined){
        this.panels[2].enabled = true;
      }else{
        this.panels[2].enabled = false;
      }
      //enable the components panel if the product has components
      if(Store.getters.productComponents != undefined){
        this.panels[4].enabled = true;
      }else{
        this.panels[4].enabled = false;
      }
      this.nextPanel();
    }
  }
};
</script>

<style scoped>
.sidenav {
  height: 100%; /* Full height */
  width: 250px; /*full width on initial load, changed with Vue*/
  position: fixed; /*stay in place*/
  z-index: 1; /*stay on top*/
  top: 15%; /*Display from top left corner*/
  left: 0;
  overflow-x: hidden; /*Disable horizontal scroll*/
  padding-top: 60px;
  transition: 0.3s;
  background-color: #dddddda0;
}

.sidenav h3 {
  font-size: 24px;
  cursor: default;
  position: absolute;
  top: 15px;
  left: 15px;
  margin-right: 50px;
  cursor: pointer;
}

/*Center primary buttons on the side bar*/
.sidenav .sidenav-controls {
  text-align: center;
  margin: auto;
}

/* Position and style the close button (top right corner) */
.sidenav .closebtn {
  position: absolute;
  top: 15px;
  right: 15px;
  margin-left: 50px;
  cursor: pointer;
}
</style>

