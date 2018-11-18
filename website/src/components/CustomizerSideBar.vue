<template>
    <div>
      <i ref="menuButton" @click="openNav()" class="material-icons md-36 btn ">menu</i>
      <div ref="customizerSidenav" class="sidenav">
        <i class="closebtn material-icons md-36" @click="closeNav()">close</i>
        <h3>{{panels[currentPanelIndex].title}}</h3>
        <!--The child component changes dinamically depending on tthe currently selected component.-->
        <!--The Sidebar component listens out for any event that the child may trigger-->
        <customizer-side-bar-products-panel v-if="currentPanelIndex == 0" @select-product-identifier="storeProductIdentifier"></customizer-side-bar-products-panel>
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

export default {
  name: "CustomizerSideBar",
  data() {
    return {
      //chave: fase atual de configuração
      //valor: painel a ser rendered
      currentPanelIndex: 0,
      selectedProductId: 0,
      panels: [
        {
          title: "Products",
        },
        {
          title: "Dimensions",
        },
        {
          title: "Slots",
        },
        {
          title: "Materials",
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
    canDisplayPreviousButton() {
      return this.currentPanelIndex > 0;
    },
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
    previousPanel() {
      if (this.currentPanelIndex > 0) {
        this.currentPanelIndex--;
      }
    },
    nextPanel() {
      if (this.currentPanelIndex < this.panels.length - 1) {
        this.currentPanelIndex++;
      }
    },
    storeProductIdentifier(id) {
      //When a product is selected, proceed to the next panel
      this.selectedProductId = id;
      this.nextPanel();
    }
  }
};
</script>

<style scoped>
.sidenav {
  height: 100%; /* Full height */
  width: 0; /*0 width, changed with Vue*/
  position: fixed; /*stay in place*/
  z-index: 1; /*stay on top*/
  top: 0; /*Display from top left corner*/
  left: 0;
  overflow-x: hidden; /*Disable horizontal scroll*/
  padding-top: 60px;
  transition: 0.3s;
  background-color: rgba(54, 131, 175, 0.5);
}

.sidenav h3 {
  cursor: default;
}

/*Center primary buttons on the side bar*/
.sidenav .sidenav-controls {
  text-align: center;
  margin: auto;
}

/* Position and style the close button (top right corner) */
.sidenav .closebtn {
  position: absolute;
  top: 25px;
  right: 25px;
  margin-left: 50px;
  cursor: pointer;
}
</style>

