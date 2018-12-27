<template>
  <div>
    <div v-if="getComponentsOk">
      <div class="icon-div-top">
        <i class="material-icons md-12 md-blue btn">help</i>
        <span class="tooltiptext">In this step, you can add components to the structure.</span>
      </div>
      <div class="text-entry">Choose components to add:</div>
      <div class="padding-div">
        <div class="scrollable-div" style="height: 200px; width: 100%;">
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
        <!-- <div class="scrollable-div" style="height: 100px; width: 100%;">
          <div class="small-padding-div border" v-for="(divElement, index) in div_elements" :key="index"> -->
            <!-- <div v-if="hasSlots()">
              <div v-if="canAddComponentToSlot(divElement.model)">
                <div class="small-padding-div" align="center">
                  <b>{{divElement.designation}}</b>
                </div>
                <div class="small-padding-div" align="center">
                  Slot:
                  <input type="number" value="1" min="1" style="width:50px" v-model="div_inputs[index]">
                  <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="addDivElement(divElement, index)">check_circle_outline</i>
                  <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="removeDivElement(divElement, index)">highlight_off</i>
                </div>
              </div>
              <div v-else class="small-padding-div" align="center">
                  <b>{{divElement.designation}}</b>
                  <div class="small-padding-div" align="center">
                    <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="addDivElement(divElement)">check_circle_outline</i>
                    <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="removeDivElement(divElement)">highlight_off</i>
                  </div>
              </div>
            </div>
            <div v-else class="small-padding-div" align="center">
                <b>{{divElement.designation}}</b>
                <div class="small-padding-div" align="center">
                  <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="addDivElement(divElement)">check_circle_outline</i>
                  <i class="material-icons md-24 md-blue btn" style="padding:0px" @click="removeDivElement(divElement)">highlight_off</i>
                </div>
            </div> -->
          <!-- </div>
        </div> -->
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
import Vue from "vue";
import Axios from "axios";
import { error } from "three";
import store from "./../store";
import Toasted from "vue-toasted";
import { MYCM_API_URL } from "./../config.js";
import { SET_CUSTOMIZED_PRODUCT_COMPONENTS,
        REMOVE_CUSTOMIZED_PRODUCT_COMPONENT,
        ACTIVATE_CAN_MOVE_COMPONENTS }
        from "./../store/mutation-types.js";

Vue.use(Toasted);

export default {
  name: "CustomizerSideBarComponentsPanel",
  data() {
    return {
      components: [],
      div_elements: [],
      div_inputs: [],
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
      Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/components`)
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
    // createDivElements(component) {
    //   this.div_elements.push(component);
    // },
    // canAddComponentToSlot(model){
    //   return model.split(".")[0] != "sliding-door";
    // },
    isComponentMandatory(componentId){
      for(let i = 0; i < this.components.length; i++){
        if(this.components[i].id == componentId) return this.components[i].mandatory == true;
      }
    },
    // addDivElement(component, index) {
    //   //If the product has slots and the chosen component can be added to a slot, checks if the 
    //   if (this.hasSlots() && this.canAddComponentToSlot(component.model)){
    //     if(this.div_inputs[index] == undefined) {
    //       this.$toast.open("You must choose a slot to apply the component!");
    //     } else if(this.div_inputs[index] < 1 || this.div_inputs[index] > store.state.customizedProduct.slots.length + 1){
    //         this.$toast.open("You must choose a valid slot to apply the component!");
    //     } else {
    //         component.slot = this.div_inputs[index];
    //         store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, { component: component });
    //         //TODO! DISABLE apply button
    //     }
    //   } else if(!this.hasSlots() || !this.canAddComponentToSlot(component.model)){
    //     component.slot = 0;
    //     store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, { component: component });
    //       //TODO! DISABLE apply button
    //   }
    // },
    // removeDivElement(component, index) {
    //   component.slot = this.div_inputs[index];

    //   //TODO! only remove from graphical representation and store if DELETE request returns 204
    //   store.dispatch(REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, { component: component });
    //   this.div_inputs.splice(index, 1);
    //   this.div_elements.splice(index, 1);
    //   this.$toast.open("The component was sucessfully removed!");
    // },
    nextPanel(){
      //TODO! POST components
      this.$emit("advance");
    },
    previousPanel(){
      //TODO! DELETE ALL components
      store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS);
      this.$emit("back");
    }
  },
  created() {
    this.getProductComponents();
    store.dispatch(ACTIVATE_CAN_MOVE_COMPONENTS);
  }
};
</script>

<style>
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
</style>

