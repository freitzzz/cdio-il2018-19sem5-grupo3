<template>
  <div>
    <div ref="componentsSideCustomizer" class="components-side-customizer">
      <i class="closebtn material-icons md-18 md-grey" @click="closeNav()">close</i>
    </div>
    <div v-if="getComponentsOk">
      <div class="icon-div-top">
        <i class="material-icons md-12 md-blue btn">help</i>
        <span class="tooltiptext">In this step, you can drag components to the closet's structure.</span>
      </div>
      <div class="text-entry">Choose components to add:</div>
      <div class="padding-div">
        <div class="scrollable-div" style="height: 300px; width: 100%;">
          <ul class="image-list" v-for="component in components" :key="component.id">
            <li class="image-icon-div">
              <div class="image-btn">
                <img :src="findComponentImage(component.model)" width="100%">
                <span v-if="isComponentMandatory(component.id)">
                <i class="image-icon material-icons md-12 md-red btn">warning</i>
                <span class="tooltiptext">This component is mandatory!</span>
                </span>
                <p>{{component.designation}}</p>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div v-else>
      <div class="text-entry">
        <b>Error: {{httpCode}}</b>
      </div>
      <div class="text-entry">Yikes! Looks like we ran into a problem here...</div>
      <div class="icon-div-center">
        <i class="material-icons md-36 md-blue btn" @click="getProductComponents">refresh</i>
      </div>
    </div>
    <div class="center-controls">
      <i class="btn btn-primary material-icons" @click="previousPanel()" >arrow_back</i>
      <i class="btn btn-primary material-icons" @click="nextPanel()" >arrow_forward</i>
    </div>
  </div>
</template>

<script>
import ProductRequests from "./../services/mycm_api/requests/products.js";
import store from "./../store";
import { SET_CUSTOMIZED_PRODUCT_COMPONENTS,
        REMOVE_CUSTOMIZED_PRODUCT_COMPONENT,
        ACTIVATE_CAN_MOVE_COMPONENTS }
        from "./../store/mutation-types.js";

export default {
  name: "CustomizerSideBarComponentsPanel",
  data() {
    return {
      components: [],
      addedComponents: [],
      httpCode: null
    };
  },
  computed: {
    getComponentsOk() {
      return this.httpCode === 200;
    }
  },
  methods: {
    getProductComponents() {
      ProductRequests.getProductComponents(store.state.product.id)
        .then(response => {
          this.components = [];
          this.components.push(...response.data);
          this.httpCode = response.status;
        })
        .catch(error => {
          if (error.response === undefined) {
            this.httpCode = 500;
          } else {
            this.httpCode = error.response.status;
          }
        });
    },
    hasSlots(){
     return store.state.customizedProduct.slots.length > 0;
    },
    findComponentImage(filename) {
      return "./src/assets/products/" + filename.split(".")[0] + ".png";
    },
    isComponentMandatory(componentId){
      for(let i = 0; i < this.components.length; i++){
        if(this.components[i].id == componentId) return this.components[i].mandatory == true;
      }
    },
    closeNav() {
      this.$refs.componentsSideCustomizer.style.width = "0";
    },
    nextPanel(){
      //TODO! POST components
      this.$emit("advance");
    },
    previousPanel(){
      //TODO! DELETE ALL components
       this.$dialog.confirm({
        title: 'Return',
        hasIcon: true,
        type: 'is-info',
        icon: 'fas fa-exclamation-circle size:5px',
        iconPack: 'fa',
        message: 'Are you sure you want to return? All progress made in this step will be lost.',
        onConfirm: () => {
          store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS);
          this.$emit("back");
        }
      })
    }
  },
  created() {
    this.getProductComponents();
    store.dispatch(ACTIVATE_CAN_MOVE_COMPONENTS);
  }
};
</script>

<style>
/* Required component icon */
.image-icon-div {
  position: relative;
  overflow-x: hidden;
}

.image-icon-div .image-icon {
  position: absolute;
  top: 10%;
  left: 90%;
  transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  padding: 12px 24px;
  border: none;
  cursor: pointer;
  border-radius: 5px;
  text-align: center;
}

.image-icon-div .tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 10px;
  padding: 10%;
  position: absolute;
  top: 10px;
  left: 0px;
  right: 0px;
}

.image-icon-div:hover .tooltiptext  {
  opacity: .8 !important;
  visibility: visible;
}

/* Tool tip text icon */
.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
  top: 25px;
  left: 0px;
  right: 0px;
}

.icon-div-top:hover .tooltiptext {
  visibility: visible;
}

.icon-div-top {
  top: 15px;
  left: 15px;
  margin-left: 130px;
  position: absolute;
}

/* Sidenav for component customization */
.components-side-customizer {
    height: 100%;
    /* Full height */
    width: 300px;
    /*Full width on initial load, changed with Vue*/
    position: fixed;
    /*Stay in place*/
    z-index: 1;
    /*Stay on top*/
    top: 20%;
    /*Display from top left corner*/
    left: 78%;
    overflow-x: hidden;
    /*Disable horizontal scroll*/
    padding-top: 60px;
    margin: 2%;
    transition: 0.3s;
    background-color: #e9e9e9a0;
}
  
.components-side-customizer h3 {
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
.components-side-customizer .components-side-customizer-controls {
    text-align: center;
    margin: auto;
}
    
/* Position and style the close button (top right corner) */
.components-side-customizer .closebtn {
    position: absolute;
    top: 15px;
    right: 15px;
    margin-left: 50px;
    cursor: pointer;
}
</style>