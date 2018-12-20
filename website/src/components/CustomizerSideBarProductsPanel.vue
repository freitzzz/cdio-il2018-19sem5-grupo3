<template>
  <div>
    <!--Only render products if the API call was successful-->
    <div v-if="getProductsOk">
        <div class="icon-div-top"><i class="material-icons md-12 md-blue btn">help</i>
          <span class="tooltiptext">In this step, you must choose one of our base products in order to start customizing it.</span>
        </div>
        <div class="text-entry">Select a structure:</div>
       <div class="padding-div">
        <div class="scrollable-div" style="height: 400px; width: 100%;">
            <ul class="image-list" v-for="product in products" :key="product.id">
            <li>
              <div class="image-btn" @click="selectProduct(product.id)">
                <img :src="findProductImage(product.model)" width="100%">
                <p>{{product.designation}}</p>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div v-else-if="getProductsNotFound">
      <div class="text-entry"><b>No base products</b></div>
      <div class="text-entry">There are no available base products at the moment. Try again later.</div>
      <div class="icon-div-center">
        <i class="material-icons md-36 md-blue btn" @click="getBaseProducts">refresh</i>
      </div>
    </div>
    <div v-else>
      <div class="text-entry"><b>Error: {{httpCode}}</b></div>
      <div class="text-entry">Yikes! Looks like we ran into a problem here...</div>
      <div class="icon-div-center">
        <i class="material-icons md-36 md-blue btn" @click="getBaseProducts">refresh</i>
      </div>
    </div>
  </div>
</template>

<script>
import Axios from "axios";
import store from "./../store";
import { INIT_PRODUCT, SET_CUSTOMIZED_PRODUCT_MATERIAL, SET_CUSTOMIZED_PRODUCT_COLOR, SET_CUSTOMIZED_PRODUCT_FINISH } from "./../store/mutation-types.js";
import { MYCM_API_URL } from "./../config.js";

export default {
  name: "CustomizerSideBarProductsPanel",
  data() {
    return {
      products: [],
      httpCode: null
    };
  },
  computed: {
    getProductsOk() {
      return this.httpCode === 200;
    },
    getProductsNotFound(){
      return this.httpCode === 404;
    }
  },
  methods: {
    /**
     * Propagate an event to a parent component.
     */
    selectProduct(productId) {
      Axios.get(`${MYCM_API_URL}/products/${productId}`)
        .then(response => {
          store.dispatch(INIT_PRODUCT, { product: response.data }); //Dispatches the action INIT_PRODUCT

          //Applies initial material by dispatching the actions SET_CUSTOMIZED_PRODUCT_MATERIAL, SET_CUSTOMIZED_PRODUCT_COLOR and SET_CUSTOMIZED_PRODUCT_FINISH
          store.dispatch(SET_CUSTOMIZED_PRODUCT_MATERIAL, {
            id : response.data.materials[0].id, 
            reference : response.data.materials[0].reference,
            designation : response.data.materials[0].designation,
            image : response.data.materials[0].image
          });

          store.dispatch(SET_CUSTOMIZED_PRODUCT_COLOR, {
            name: "None",
            red: 0,
            green: 0,
            blue: 0,
            alpha: 0
          });

          store.dispatch(SET_CUSTOMIZED_PRODUCT_FINISH, {
            name: "None",
            shininess: 20
          });

          this.httpCode = response.status;
          this.$emit("advance"); //Progresses to the next step (change product dimensions)
        })
        .catch(error => {
          this.httpCode = error.response.status;
        });
    },
    /**
     * Fetches products from the MYCM API.
     */
    getBaseProducts() {
      Axios.get(`${MYCM_API_URL}/products/base`)
        .then(response => {
          this.products = response.data;
          this.httpCode = response.status;
        })
        .catch(error => {
          if(error.response === undefined){
            this.httpCode = 500;
          }else{
            this.httpCode = error.response.status;
          }
        });
    },
    findProductImage(filename) {
      return "./src/assets/products/" + filename.split(".")[0] + ".png";
    }
  },  
  created() {
    this.getBaseProducts();
  }
};
</script>

<style scoped>
/* The navigation menu links */
.product-entry {
  margin: 6%;
  text-decoration: none;
  font-size: 18px;
  color: #797979 !important;
  display: block;
  transition: 0.3s;
}

/**Change color when hovering over an entry */
.product-entry:hover {
  color: #adadad !important;
  cursor: pointer;
}

.icon-div-center {
  text-align: center;
}

.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 120px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
}

.icon-div-top:hover .tooltiptext {
  visibility: visible;
}

.icon-div-top {
  top: 15px;
  left: 15px;
  margin-left: 100px;
  position: absolute;
}
</style>

