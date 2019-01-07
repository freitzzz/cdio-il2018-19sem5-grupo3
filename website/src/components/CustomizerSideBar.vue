<template>
  <div>
    <i ref="menuButton" @click="openNav()" class="material-icons md-36 md-grey btn" style="visibility:hidden">menu</i>
    <div ref="customizerSidenav" :class="[(currentPanelIndex != 5) ? 'sidenav' : 'sidenavSpecial']">
      <i class="closebtn material-icons md-18 md-grey" @click="closeNav()">close</i>
  
      <transition name="fade">
        <div v-if="canDisplayEditButton">
          <div>
            <i class="editbtn material-icons md-18 md-grey" @click="togglePopup">edit</i>
            <span v-if="!hasPressedEditButton" class="notification-dot"></span>
          </div>
          <transition name="fade">
            <div class="editpopup" v-show="popupEnabled">
              <input type="text" placeholder="Reference" v-model="customizedProductReference">
              <input type="text" placeholder="Designation" v-model="customizedProductDesignation">
              <button class="btn-primary" @click="togglePopup">OK</button>
            </div>
          </transition>
        </div>
      </transition>
  
      <h3>{{panels[currentPanelIndex].title}}</h3>
      <!--The child component changes dinamically depending on tthe currently selected component.-->
      <!--The Sidebar component listens out for any event that the child may trigger-->
      <customizer-side-bar-products-panel v-if="currentPanelIndex == 0" @advance="selectProduct" />
      <customizer-side-bar-dimensions-panel v-if="currentPanelIndex == 1" @advance="nextPanel" @back="previousPanel" />
      <customizer-side-bar-slots-panel v-if="currentPanelIndex == 2" @advance="nextPanel" @back="previousPanel" />
      <customizer-side-bar-materials-panel v-if="currentPanelIndex == 3" @advance="nextPanel" @back="previousPanel" />
      <customizer-side-bar-components-panel v-if="currentPanelIndex == 4" @advance="nextPanel" @back="previousPanel" />
      <customizer-check-out v-if="currentPanelIndex == 5" />
    </div>
  </div>
</template>

<script>
  import CustomizerSideBarProductsPanel from "./CustomizerSideBarProductsPanel";
  import CustomizerSideBarDimensionsPanel from "./CustomizerSideBarDimensionsPanel";
  import CustomizerSideBarMaterialsPanel from "./CustomizerSideBarMaterialsPanel";
  import CustomizerSideBarSlotsPanel from "./CustomizerSideBarSlotsPanel";
  import CustomizerSideBarComponentsPanel from "./CustomizerSideBarComponentsPanel";
  import CustomizerCheckOut from "./payment/CustomizerCheckOut.vue";
  import {
    SET_CUSTOMIZED_PRODUCT_REFERENCE,
    SET_CUSTOMIZED_PRODUCT_DESIGNATION
  } from "./../store/mutation-types.js";
  import Store from "./../store/index.js";
  import CustomizedProductRequests from "./../services/mycm_api/requests/customizedproducts.js";
  
  export default {
    name: "CustomizerSideBar",
    data() {
      return {
        popupEnabled: false,
        hasPressedEditButton: false,
        currentPanelIndex: 0,
        panels: [{
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
      CustomizerSideBarSlotsPanel,
      CustomizerSideBarComponentsPanel,
      CustomizerCheckOut,
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
      },
      /**
       * Checks if the edit button can be displayed.
       * The edit button should only be displayed in the stages after the product selection and before the payment.
       */
      canDisplayEditButton() {
        return this.currentPanelIndex > 0 && this.currentPanelIndex < 5;
      },
  
      /**
       * Computed property used for binding the CustomizedProduct's reference directly to the store.
       */
      customizedProductReference: {
        get() {
          return Store.getters.customizedProductReference;
        },
        set(value) {
          return Store.dispatch(SET_CUSTOMIZED_PRODUCT_REFERENCE, value);
        }
      },
  
      /**
       * Computed property used for binding the CustomizedProduct's designation directly to the store.
       */
      customizedProductDesignation: {
        get() {
          return Store.getters.customizedProductDesignation;
        },
        set(value) {
          return Store.dispatch(SET_CUSTOMIZED_PRODUCT_DESIGNATION, value);
        }
      }
    },
    methods: {
      /**
       * Toggles the edit popup.
       */
      togglePopup() {
        this.hasPressedEditButton = true;
  
        //only perform a request if the popup was previously enabled
        if (this.currentPanelIndex > 1 && this.currentPanelIndex < 5 && this.popupEnabled) {
  
          var putBody = {
            reference: this.customizedProductReference
          };
  
          if (this.customizedProductDesignation.length > 0) {
            putBody.designation = this.customizedProductDesignation;
          }
  
          CustomizedProductRequests.putCustomizedProduct(Store.getters.customizedProductId, putBody)
            .then(() => {
              //only disable pop-up if promise resolved successfully
              this.popupEnabled = !this.popupEnabled;
            }).catch(error => {
              this.$toast.open(error.response.data);
            });
  
  
        } else {
          this.popupEnabled = !this.popupEnabled;
        }
      },
  
      /**
       * Fades out the menu button and opens the sidenav
       */
      openNav() {
        this.$refs.menuButton.style.visibility = "hidden";
  
        if (this.currentPanelIndex == 5) {
          this.$refs.customizerSidenav.style.width = "1000px";
        } else {
          this.$refs.customizerSidenav.style.width = "300px";
        }
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
          while (this.currentPanelIndex > 0 && !this.panels[--this.currentPanelIndex].enabled);
          this.$emit('changeStage', this.currentPanelIndex);
        }
      },
      /**
       * Go to the next panel.
       */
      nextPanel() {
        if (this.currentPanelIndex < this.panels.length - 1) {
          while (this.currentPanelIndex < this.panels.length && !this.panels[++this.currentPanelIndex].enabled);
          this.$emit('changeStage', this.currentPanelIndex)
        }
      },
      /**
       * Checks the product's slots and components properties and sets the flags accordingly and advances to the next stage.
       */
      selectProduct() {
        //enable the slots panel if the product has slots
        if (Store.getters.productSlotWidths != undefined) {
          this.panels[2].enabled = true;
        } else {
          this.panels[2].enabled = false;
        }
        //enable the components panel if the product has components
        if (Store.getters.productComponents != undefined) {
          this.panels[4].enabled = true;
        } else {
          this.panels[4].enabled = false;
        }
        this.nextPanel();
      }
  
    }
  };
</script>

<style scoped>
  .sidenav {
    height: 100%;
    /* Full height */
    width: 300px;
    /*full width on initial load, changed with Vue*/
    position: fixed;
    /*stay in place*/
    z-index: 1;
    /*stay on top*/
    top: 20%;
    /*Display from top left corner*/
    left: 0;
    overflow-x: hidden;
    /*Disable horizontal scroll*/
    padding-top: 60px;
    margin: 2%;
    transition: 0.3s;
    background-color: #d3f0ffa0;
  }
  
  .sidenav h3 {
    font-size: 24px;
    color: #797979 !important;
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
  
  .sidenav .editbtn {
    position: absolute;
    top: 15px;
    right: 50px;
    margin-left: 50px;
    margin-bottom: 15px;
    cursor: pointer;
  }
  
  .notification-dot {
    border-radius: 5px;
    background-color: #0ba4db9b;
    height: 25px;
    width: 25px;
    border-radius: 50%;
    position: absolute;
    top: 12px;
    right: 47px;
    /*Position the notification dot behind the edit button*/
    z-index: -1;
  }
  
  .sidenavSpecial .closebtn {
    position: absolute;
    top: 15px;
    right: 15px;
    margin-left: 20px;
    cursor: pointer;
  }
  
  .sidenavSpecial {
    height: 100%;
    /* Full height */
    width: 1300px;
    /*full width on initial load, changed with Vue*/
    position: fixed;
    /*stay in place*/
    z-index: 1;
    /*stay on top*/
    top: 15%;
    /*Display from top left corner*/
    left: 0;
    overflow-y: scroll;
    /*Disable horizontal scroll*/
    padding-top: 20px;
    margin: 2%;
    transition: 0.3s;
    background-color: rgb(240, 235, 235);
  }
  
  .sidenavSpecial h3 {
    font-size: 24px;
    color: #797979 !important;
    cursor: default;
    top: 15px;
    text-align: center;
    left: 15px;
    margin-right: 50px;
    cursor: pointer;
  }
  
  .editpopup {
    text-align: center;
    margin-left: 10%;
    margin-right: 10%;
    padding: 4%;
    background-color: #e9e9e9d2;
    position: absolute;
    z-index: 6;
    border-radius: 6px;
  }
  
  .editpopup::after {
    content: " ";
    position: absolute;
    bottom: 100%;
    left: 85%;
    margin-left: -5px;
    border-width: 5px;
    border-style: solid;
    border-color: transparent transparent #e9e9e9d2 transparent;
  }
  
  .editpopup input {
    padding: 3%;
    border-color: #e9e9e9d2;
    border-style: solid;
    border-radius: 6px;
    margin-bottom: 3%;
  }
  
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.3s;
  }
  
  .fade-enter,
  .fade-leave-to {
    opacity: 0;
  }
</style>

