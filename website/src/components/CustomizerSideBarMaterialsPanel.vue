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
           <a v-if="getMaterialInformationOK" class="sidepanel">
              <a v-if="hasFinishes" class="sidepanel-entry" @click="changeShowFinishes">
                <p><i class="material-icons md-12 md-blue">brush</i> <b>Finishes <i class="fa fa-caret-down"></i></b></p>
              </a>
              <div class="dropdown-container" v-if="showFinishes">
                <a class="sidepanel-subentry" @click="removeFinish()"><i class="material-icons md-12">not_interested</i> None</a>   
                <div v-for="finish in finishes" :key="finish.description">
                  <a class="sidepanel-subentry" @click="applyFinish(finish)">{{finish.description}}</a>   
                </div>           
              </div>
              <a v-if="hasColors" class="sidepanel-entry" @click="changeShowColors">
                <p><i class="material-icons md-12 md-blue">color_lens</i> <b>Colors <i class="fa fa-caret-down"></i></b></p>
              </a>
              <div class="dropdown-container" v-if="showColors">
                <a class="sidepanel-subentry" @click="removeColor()"><i class="material-icons md-12">not_interested</i> None</a>
                <div v-for="color in colors" :key="color.name">
                  <a class="sidepanel-subentry" @click="applyColor(color)">{{color.name}}</a>   
                </div>                       
              </div>
          </a>
          <ul class="image-list" v-for="material in materials" :key="material.id">
            <li class="image-btn" @click="applyMaterial(material), removeFinish(), removeColor(), getMaterialInformation(material.id)">
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
      <i class="btn btn-primary material-icons" @click="previousPanel()">arrow_back</i>
      <i class="btn btn-primary material-icons" @click="nextPanel()">arrow_forward</i>
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
import { SET_CUSTOMIZED_PRODUCT_MATERIAL, SET_CUSTOMIZED_PRODUCT_FINISH,
         SET_CUSTOMIZED_PRODUCT_COLOR, DEACTIVATE_CAN_MOVE_CLOSET,
         DEACTIVATE_CAN_MOVE_SLOTS
        } from "./../store/mutation-types.js";

Vue.use(Toasted);

export default {
  name: "CustomizerSideBarMaterialsPanel",
  data() {
    return {
      materials: [],
      finishes: [],
      colors: [],
      showColors: false,
      showFinishes: false,
      httpCode: null
    };
  },
  computed: {
    getMaterialsOk() {
      return this.httpCode === 200;
    },
    getMaterialInformationOK() {
      return this.httpCode === 200;
    },
    hasFinishes(){
      return this.finishes.length > 0;
    },
    hasColors(){
      return this.colors.length > 0;
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
    getMaterialInformation(materialId) {
      Axios.get(`${MYCM_API_URL}/materials/${materialId}`)
        .then(response => {
          this.finishes = [];
          this.finishes.push(...response.data.finishes);

          this.colors = [];
          this.colors.push(...response.data.colors);

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
      });
    },
    applyFinish(finish){
      store.dispatch(SET_CUSTOMIZED_PRODUCT_FINISH, {
        description: finish.description,
        shininess: finish.shininess
      })
    },
    removeFinish(){
      store.dispatch(SET_CUSTOMIZED_PRODUCT_FINISH, {
        description: "None",
        shininess: 20
      })
    },     
    applyColor(color){
      store.dispatch(SET_CUSTOMIZED_PRODUCT_COLOR, {
        name: color.name,
        red: color.red,
        green: color.green,
        blue: color.blue,
        alpha: color.alpha
      })
    },
    removeColor(){
      store.dispatch(SET_CUSTOMIZED_PRODUCT_COLOR, {
        name: "None",
        red: 0,
        green: 0,
        blue: 0,
        alpha: 0
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
    nextPanel() {
      if(store.getters.customizedMaterialColorName == "None" &&
      store.getters.customizedMaterialFinishDescription == "None"){
        this.$toast.open("You must choose at least one finish or color!");
      } else {
         Axios.put(MYCM_API_URL + `/customizedproducts/${store.state.customizedProduct.id}`,
            {
	            customizedMaterial: {
		            materialId: store.state.customizedProduct.customizedMaterial.id,
		            color: {
			            name: store.state.customizedProduct.customizedMaterial.color.name,
                  red: store.state.customizedProduct.customizedMaterial.color.red,
                  green: store.state.customizedProduct.customizedMaterial.color.green,
                  blue: store.state.customizedProduct.customizedMaterial.color.blue,
                  alpha: store.state.customizedProduct.customizedMaterial.color.alpha
		            },
		            finish: {
		          	  description: store.state.customizedProduct.customizedMaterial.finish.description,
                  shininess: store.state.customizedProduct.customizedMaterial.finish.shininess,
		            }
	            },
            })
            .then(response => {this.$emit("advance")})
            .catch((error_message) => {
                    this.$toast.open({
                        message: error_message.response.data.message
                    });
                  });
      } 
    },
    previousPanel(){
      this.deleteSlots().then(() => {
        this.$emit("back");
      }).catch((error_message)=>{
           this.$toast.open({
              message: error_message
          }); 
      });
    },
    deleteSlots(){
      let slotsToDelete1 = [];
      var size = store.state.customizedProduct.slots.length;
      for(let i = 0; i< size-1; i++){
        slotsToDelete1.unshift(store.state.customizedProduct.slots[i].idSlot);
      }
      return new Promise((accept,reject)=>{
        this.deleteSlot(slotsToDelete1)
        .then(() => {
          accept()})
        .catch((error_message) => { 
                reject(error_message)
      });
      })
    },
    deleteSlot(slotsToDelete1){
      return new Promise((accept, reject) => {
        let slotToDelete = slotsToDelete1.pop();
        Axios.delete(MYCM_API_URL + 
        `/customizedproducts/${store.state.customizedProduct.id}/slots/${slotToDelete}`)
        .then(() => {
                if(slotsToDelete1.length > 0 ){
                  return this.deleteSlot(slotsToDelete1)
                  .then(()=>{
                    accept()})
                  .catch((error_message) => { reject(error_message)});
                }else{
                  accept();
                }
              })
              .catch((error_message) => {
                  reject(error_message.response.data.message);
              });
      })
    },
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
