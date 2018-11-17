<template>
    <div>
        <a class="product-entry" v-for="(product, index) in products" :key="index">{{product.designation}}</a>
    </div>
</template>

<script>
import Axios from "axios";

export default {
  name: "CustomizerSideBarProductsPanel",
  data() {
    return {
      products: []
    };
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/products")
      .then(response => {
        this.products = response.data;
      })
      .catch(error => {});
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

