<template>
    <div>
      <!--Only render products if the API call was successful-->
      <div v-if="getProductsOk">
        <a class="product-entry" v-for="product in products" :key="product.id" @click="selectProduct(product)">{{product.designation}}</a>
      </div>
      <div v-else>Error: {{httpCode}}<br>Yikes! Looks like we ran into a problem here
        <i class="material-icons md-36 btn" @click="getProducts">refresh</i>
      </div>
    </div>
</template>

<script>
import Axios from "axios";

export default {
  name: "CustomizerSideBarProductsPanel",
  data() {
    return {
      products: [],
      httpCode: 200
    };
  },
  computed: {
    getProductsOk() {
      return this.httpCode == 200;
    }
  },
  methods: {
    /**
     * Propagate an event to a parent component.
     */
    selectProduct(product) {
      this.$emit("select-product-identifier", product.id);
    },
    /**
     * Fetches products from the MYCM API.
     */
    getProducts() {
      Axios.get("http://localhost:5000/mycm/api/products")
        .then(response => {
          this.products = response.data;
          this.httpCode = response.status;
        })
        .catch(error => {
          
          this.httpCode = error.response.status;
        });
    }
  },
  created() {
    this.getProducts();
  }
};
</script>

<style scoped>
/* The navigation menu links */
.product-entry {
  /*padding: 8px 8px 8px 32px;*/
  text-decoration: none;
  font-size: 25px;
  color: #818181;
  display: block;
  transition: 0.3s;
}

/**Change color when hovering over an entry */
.product-entry:hover {
  color: blue;
  cursor: pointer;
}
</style>

