<template>
  <div>
    <div v-if="getMaterialsOk">
      <div class="icon-div-top">
        <i class="material-icons md-12 md-blue btn">help</i>
        <span class="tooltiptext">In this step, you can add a material to the closet.</span>
      </div>
      <div class="text-entry">Choose material to add:</div>
      <div class="padding-div">
        <div class="scrollable-div" style="height: 400px; width: 100%;">
          <ul class="image-list" v-for="material in materials" :key="material.id">
            <li class="image-btn" @click="applyMaterial(material)">
                <img :src="findMaterialImage(material.image)" width="100%">
                <p>{{material.designation}}</p>
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
        <i class="material-icons md-36 md-blue btn" @click="getProductMaterials">refresh</i>
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
import { SET_CUSTOMIZED_PRODUCT_MATERIAL, DEACTIVATE_CAN_MOVE_CLOSET, DEACTIVATE_CAN_MOVE_SLOTS } from "./../store/mutation-types.js";
Vue.use(Toasted);
export default {
  name: "CustomizerSideBarMaterialsPanel",
  data() {
    return {
      materials: [],
      httpCode: null
    };
  },
  computed: {
    getMaterialsOk() {
      return this.httpCode === 200;
    }
  },
   methods: {
    getProductMaterials() {
      Axios.get(`${MYCM_API_URL}/products/${store.state.product.id}/materials`)
        .then(response => {
          this.materials = [];
          this.materials.push(...response.data);
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
    findMaterialImage(filename) {
      return "./src/assets/materials/" + filename;
    },
    applyMaterial(material) {
      store.dispatch(SET_CUSTOMIZED_PRODUCT_MATERIAL, {
        id: material.id,
        reference: material.reference,
        designation: material.designation,
        image: material.image
      })
    },
    nextPanel(){
      this.$emit("advance");
    },
    previousPanel(){
      this.$emit("back");
    }
  },
  created() {
    this.getProductMaterials();
    store.dispatch(DEACTIVATE_CAN_MOVE_CLOSET);
    store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS);
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
  margin-left: 88px;
  position: absolute;
}
</style>
