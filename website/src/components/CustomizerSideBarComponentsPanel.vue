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
            <li>
              <div class="image-btn" @click="createDivElements(component)">
                <img :src="findComponentImage(component.model)" width="100%">
                <p>{{component.designation}}</p>
              </div>
            </li>
          </ul>
        </div>
        <div class="scrollable-div" style="height: 100px; width: 100%;">
          <div class="small-padding-div border" v-for="(divElement, index) in div_elements" :key="index">
            <div v-if="hasSlots()">
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
            </div>
          </div>
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

//TODO! CHANGE Toast
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
    createDivElements(component) {
      this.div_elements.push(component);
    },
    canAddComponentToSlot(model){
      return model.split(".")[0] != "sliding-door";
    },
    addDivElement(component, index) {
      //If the product has slots and the chosen component can be added to a slot, checks if the 
      if (this.hasSlots() && this.canAddComponentToSlot(component.model)){
        if(this.div_inputs[index] == undefined) {
          this.$toasted.show("You must choose a slot to apply the component!", {
            position: "top-center",
            duration: 2000
          });
      } else if(this.div_inputs[index] < 1 || this.div_inputs[index] > store.state.customizedProduct.slots.length + 1){
          this.$toasted.show("You must choose a valid slot to apply the component!", {
            position: "top-center",
            duration: 2000
          });
        } else {
          component.slot = this.div_inputs[index];
          store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, { component: component });
          //TODO! DISABLE apply button
        }
      } else if(!this.hasSlots() || !this.canAddComponentToSlot(component.model)){
        component.slot = 0;
        store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, { component: component });
          //TODO! DISABLE apply button
      }
    },
    removeDivElement(component, index) {
      store.dispatch(REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, { component: component });

      var aux;
      for (let i = index + 1; i < this.div_inputs.length; i++) {
        aux = this.div_inputs[i];
        this.div_inputs[i - 1] = aux;
      }

      this.div_elements.splice(component, 1);
      this.div_inputs.splice(this.div_inputs.length);

      this.$toasted.show("The component was sucessfully removed!", {
        position: "top-center",
        duration: 2000
      });
      //TODO! communicate with Three.js & Remove from store
    },
    nextPanel(){
      this.$emit("advance");
    },
    previousPanel(){
      //TODO! DELETE components
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

