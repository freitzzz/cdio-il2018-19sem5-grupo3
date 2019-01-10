<template>
  <div>
    <div v-if="showSidePanel" ref="componentsSideCustomizer" class="components-side-customizer">
      <i class="closebtn material-icons md-18 md-grey" @click="closeNav()">close</i>
      <div class="text-title">
        <p>{{this.componentData.designation}}</p>
      </div>
      <div v-if="this.componentData.materials.length > 0">
        <div class="text-entry">Choose a material to add:</div>
        <div class="padding-div">
          <div class="scrollable-div" style="height: 300px; width: 100%;">
            <a v-if="this.selectedMaterial">
                <a v-if="hasFinishes()" class="sidepanel-entry" @click="changeShowFinishes">
                  <p><i class="material-icons md-12 md-blue">brush</i> <b>Finishes <i class="fa fa-caret-down"></i></b></p>
                </a>
                <div class="dropdown-container" v-if="showFinishes">
                  <a class="sidepanel-subentry" @click="removeFinish()">
                    <swatches value="" :trigger-style="{ width: '10px', height: '10px', position:'absolute', left:'-16px', top:'6px'}"/>
                    None
                  </a>   
                  <div v-for="finish in this.selectedMaterial.finishes" :key="finish.description">
                    <a class="sidepanel-subentry" @click="applyFinish(finish)">{{finish.description}}</a>   
                  </div>           
                </div>
                <a v-if="hasColors()" class="sidepanel-entry" @click="changeShowColors">
                  <p><i class="material-icons md-12 md-blue">color_lens</i> <b>Colors <i class="fa fa-caret-down"></i></b></p>
                </a>
                <div class="dropdown-container" v-if="showColors">
                  <a class="sidepanel-subentry" @click="removeColor()">
                    <swatches value="" :trigger-style="{ width: '10px', height: '10px', position:'absolute', left:'-16px', top:'6px'}"/>
                    None
                  </a>
                  <div v-for="color in this.selectedMaterial.colors" :key="color.name">
                    <a class="sidepanel-subentry" @click="applyColor(color)">
                      <swatches
                      :value="rgbToHex(color)"
                      :trigger-style="{ width: '10px',
                                        height: '10px',
                                        position: 'absolute',
                                        left: '-16px',
                                        top: '6px',
                                        borderRadius: '3px',
                                        cursor: 'pointer',
                                        border: 'thin solid black' }"
                      :disabled="true"/>
                      {{color.name}}
                    </a>   
                  </div>                       
                </div>
            </a>
            <ul class="image-list" v-for="material in this.componentData.materials" :key="material.id">
              <li class="image-btn" @click="applyMaterial(material); getMaterialInformation(material.id)">
                <img :src="findMaterialImage(material.image)" width="100%">
                <p>{{material.designation}}</p>
              </li>
            </ul>
          </div>
        </div>
            <div class="center-controls">
              <i class="btn btn-primary material-icons" @click="previousStep()" >arrow_back</i>
            </div>
      </div>
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
import store from "./../store";
import ProductRequests from "./../services/mycm_api/requests/products.js";
import MaterialRequests from "./../services/mycm_api/requests/materials.js";
import CustomizedProductRequests from "./../services/mycm_api/requests/customizedproducts.js";
import { REMOVE_CUSTOMIZED_PRODUCT_COMPONENT,
        ADD_CUSTOMIZED_PRODUCT_COMPONENT,
        SET_COMPONENT_TO_EDIT_MATERIAL,
        ACTIVATE_CAN_MOVE_COMPONENTS,
        SET_COMPONENT_TO_EDIT,
        SET_COMPONENT_TO_ADD } from "./../store/mutation-types.js";
import "vue-swatches/dist/vue-swatches.min.css";
import Swatches from "vue-swatches";

export default {
  name: "CustomizerSideBarComponentsPanel",
  components: { Swatches }, 
  data() {
    return {
      /* Flags to help component customization */
      componentData: null,
      showSidePanel: false,
      showFinishes: false,
      showColors: false,
      selectedMaterial: null,
      /* Information regarding requests */
      addedComponents: [],
      components: [],
      httpCode: null
    };
  },
  computed: {
    getComponentsOk() {
      return this.httpCode === 200;
    },
    addComponent() {
      return store.getters.componentToAdd;
    },
    editComponent(){
      return store.getters.componentToEdit;
    },
    removeComponent(){
      return store.getters.componentToRemove;
    }
  },
  watch: {
    addComponent: function(newValue) {
      if(!newValue) return;
      for(let i = 0; i < this.components.length; i++){
        if(this.components[i].model.split(".")[0] + ".png" == newValue.model){
          if(newValue.model.split(".")[0] != "hinged-door" && newValue.model.split(".")[0] != "sliding-door"){
            this.getComponentData(this.components[i].id);
            this.showFinishes = false;
            this.showColors = false;
          } else {
            this.closeNav();
          }

          var component = this.components[i];
          component.slot = newValue.slot;
          this.addedComponents.push(component);

          store.dispatch(SET_COMPONENT_TO_ADD);
          store.dispatch(ADD_CUSTOMIZED_PRODUCT_COMPONENT, {
            component: component
          });
        }
      }
    },
    editComponent: function(newValue){
      if(!newValue) return;
      for(let i = 0; i < this.components.length; i++){
        if(this.components[i].model == newValue.model){
          if(newValue.model.split(".")[0] != "hinged-door" && newValue.model.split(".")[0] != "sliding-door"){
            this.getComponentData(this.components[i].id);
            this.showFinishes = false;
            this.showColors = false;
          } else {
            this.closeNav();
          }
        }
      }
    },
    removeComponent: function(newValue){
      if(!newValue) return;
      var context = this;
      this.$dialog.confirm({
        title: 'Remove Component',
        hasIcon: true,
        type: 'is-info',
        icon: 'fas fa-exclamation-circle size:5px',
        iconPack: 'fa',
        message: 'Do you want to remove the selected product from the closet?',
        onConfirm: () => {
          context.showSidePanel = false;
          for(let i = 0; i < context.addedComponents.length; i++){
            var componentToRemove = context.addedComponents[i]; 
            if(componentToRemove.model == newValue.model){
              context.addedComponents.splice(i, 1);
              store.dispatch(REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, {
                index: i,
                component: componentToRemove
              })
              break;
            }
          }
        }
      })
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
    getComponentData(id){
      ProductRequests.getProductById(id)
      .then(response => {
        this.componentData = response.data;
        this.httpCode = response.status;
        this.showSidePanel = true;
      })
      .catch(error => {
        this.httpCode = error.response.status
      });
    },
    async getMaterialInformation(materialId){
     await MaterialRequests.getMaterial(materialId, {pricedFinishesOnly: true})
        .then(response => {
          this.selectedMaterial.colors = [];
          this.selectedMaterial.colors.push(...response.data.colors);

          this.selectedMaterial.finishes = [];
          this.selectedMaterial.finishes.push(...response.data.finishes);

          this.httpCode = response.status;
        })
        .catch(error => {
          if (error.response === undefined) {
            this.httpCode = 500;
            this.$toast.open("There was an error while fetching the selected material's data. Try reloading the page.");
          } else if(error.response.status == 404){
            this.finishes = [];
          }
        });
    },
    hasSlots(){
     return store.state.customizedProduct.slots.length > 0;
    },
    async hasFinishes(){
      return await this.selectedMaterial.finishes && this.selectedMaterial.finishes.length > 0;
    },
    async hasColors(){
      return await this.selectedMaterial.colors && this.selectedMaterial.colors.length > 0;
    },
    findComponentImage(filename) {
      return "./src/assets/products/" + filename.split(".")[0] + ".png";
    },
    findMaterialImage(filename) {
      return "./src/assets/materials/" + filename;
    },
    applyMaterial(material) {
      this.selectedMaterial = material;
      let imageFilePath = this.findMaterialImage(material.image);
      store.dispatch(SET_COMPONENT_TO_EDIT_MATERIAL, {
          material: imageFilePath
      });      
    },
    isComponentMandatory(componentId){
      for(let i = 0; i < this.components.length; i++){
        if(this.components[i].id == componentId) return this.components[i].mandatory == true;
      }
    },
    applyFinish(finish){
      store.dispatch(SET_COMPONENT_TO_EDIT_MATERIAL, {
          material: store.getters.componentToEditMaterial.material,
          finish: finish.shininess,
          red: store.getters.componentToEditMaterial.red,
          green: store.getters.componentToEditMaterial.green,
          blue: store.getters.componentToEditMaterial.blue,
          alpha: store.getters.componentToEditMaterial.alpha
      }); 
    },
    removeFinish(){
      store.dispatch(SET_COMPONENT_TO_EDIT_MATERIAL, {
          material: store.getters.componentToEditMaterial.material,
          finish: 20,
          red: store.getters.componentToEditMaterial.red,
          green: store.getters.componentToEditMaterial.green,
          blue: store.getters.componentToEditMaterial.blue,
          alpha: store.getters.componentToEditMaterial.alpha
      }); 
    },     
    applyColor(color){
      store.dispatch(SET_COMPONENT_TO_EDIT_MATERIAL, {
        material: store.getters.componentToEditMaterial.material,
        finish: store.getters.componentToEditMaterial.finish,
        red: color.red,
        green: color.green,
        blue: color.blue,
        alpha: color.alpha
      })
    },
    removeColor(){
      store.dispatch(SET_COMPONENT_TO_EDIT_MATERIAL, {
        material: store.getters.componentToEditMaterial.material,
        finish: store.getters.componentToEditMaterial.finish,
        red: 0,
        green: 0,
        blue: 0,
        alpha: "None"
      })
    },
    changeShowColors(){
      if(this.showColors == true) this.showColors = false;
      else this.showColors = true;
    },
    changeShowFinishes(){
      if(this.showFinishes == true) this.showFinishes = false;
      else this.showFinishes = true;
    },
    rgbToHex(color){
      var red = this.convertValue(color.red);
      var green = this.convertValue(color.green);
      var blue = this.convertValue(color.blue);
      return "#" + red + green + blue;
    },
    convertValue(value){
      var hex = Number(value).toString(16);
      if (hex.length < 2) hex = "0" + hex;
      return hex;
    },
    closeNav() {
      this.$refs.componentsSideCustomizer.style.width = "0";
      store.dispatch(SET_COMPONENT_TO_EDIT);
      this.showSidePanel = false;
      this.showFinishes = false;
      this.showColors = false;
    },
    nextPanel(){
      this.$dialog.confirm({
        title: 'Proceed to Checkout',
        cancelText:'Payment',
        confirmText:'Save',
        hasIcon: true,
        type: 'is-info',
        icon: 'fas fa-exclamation-circle size:5px',
        iconPack: 'fa',
        message: 'Do you wish to proceed to checkout or do you want to save the closet?',
        onConfirm: () => {
          //Save closet on profile
        },
        onCancel: () => {
          //Proceed to payment
          this.$emit("advance");
        }
      })
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
          store.dispatch(ADD_CUSTOMIZED_PRODUCT_COMPONENT);
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
  margin-left: 130px !important;
  position: absolute;
}

/* Title text (top of the sidebar nav for component customization) */
.text-title{
    padding-left: 1%;
    padding-top: 1%;
    position: fixed;
    z-index: 1;
    margin: 2%;
    left: 78%;
    top: 20%;
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