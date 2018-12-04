<template>
  <div>
    <div v-if="getComponentsOk">
      <div class="icon-div-top">
        <i class="material-icons md-12 md-blue btn">help</i>
        <span class="tooltiptext">In this step, you can add components to the structure.</span>
      </div>
      <div class="text-entry">Choose components to add:</div>
      <div class="padding-div">
        <div class="scrollable-div" style="height: 300px; width: 100%;">
          <ul class="image-list" v-for="component in components" :key="component.id">
            <li>
              <div class="image-btn" @click="createComponent(component)">
                <img :src="findComponentImage(component.model)" width="100%">
                <p>{{component.designation}}</p>
              </div>
            </li>
          </ul>
        </div>
        <div class="scrollable-div" style="height: 100px; width: 100%;">
        <div v-for="span in spans" :key="span.id"></div>
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
  </div>
</template>

<script>
import Axios from "axios";
import { error } from "three";
import store from "./../store";
import { MYCM_API_URL } from "./../config.js";
import { SET_CUSTOMIZED_PRODUCT_COMPONENTS } from "./../store/mutation-types.js";

export default {
  name: "CustomizerSideBarComponentsPanel",
  created() {
    // store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, {
    //   components: this.components
    // });
  },
  data() {
    return {
      components: [],
      spans: [],
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
    findComponentImage(filename) {
      return "./src/assets/products/" + filename.split(".")[0] + ".png";
    },
    createComponent(component) {
      this.spans.push({value:''});
    }
  },
  created() {
    this.getProductComponents();
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

