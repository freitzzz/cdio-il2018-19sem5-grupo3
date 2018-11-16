<template>
    <div>
      <i ref="menuButton" @click="openNav()" class="material-icons md-36 btn ">menu</i>
      <div ref="customizerSidenav" class="sidenav">
        <i class="closebtn material-icons md-36" @click="closeNav()">close</i>
        <h3>{{panels[currentPanelIndex].title}}</h3>
        <component :is="panels[currentPanelIndex].component"></component>
          <div class="sidenav-controls">
            <button class="btn btn-primary" @click="previousPanel()" v-if="hasPrevious">Back</button>
            <button class="btn btn-primary" @click="nextPanel()" v-if="hasNext">Next</button>
          </div>
      </div>
    </div>
</template>

<script>
import CustomizerSideBarProductsPanel from "./CustomizerSideBarProductsPanel";
import CustomizerSideBarDimensionsPanel from "./CustomizerSideBarDimensionsPanel";
import CustomizerSideBarMaterialsPanel from "./CustomizerSideBarMaterialsPanel";

export default {
  name: "CustomizerSideBar",
  data() {
    return {
      //chave: fase atual de configuração
      //valor: painel a ser rendered
      currentPanelIndex: 0,
      panels: [
        {
          title: "Products",
          component: CustomizerSideBarProductsPanel
        },
        {
          title: "Dimensions",
          component: CustomizerSideBarDimensionsPanel
        },
        {
          title: "Materials",
          component: CustomizerSideBarMaterialsPanel
        }
      ]
    };
  },
  components: {
    CustomizerSideBarProductsPanel,
    CustomizerSideBarDimensionsPanel,
    CustomizerSideBarMaterialsPanel
  },
  computed: {
    hasPrevious() {
      return this.currentPanelIndex > 0;
    },
    hasNext() {
      return this.currentPanelIndex < this.panels.length - 1;
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

