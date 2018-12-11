<template>
  <component :is="currentComp" @switch-to-customizer="switchPage()"></component>
</template>
<script>
import Customizer from "./Customizer.vue";
import Intro from "./Intro.vue";
import ManagementTopBar from "./ManagementTopBar";
import ListProductsVue from './management/product/ListProducts.vue';
import ProductsTableVue from './management/product/ProductsTable.vue';
import ListCustomizedProductCollections from './management/customizedproductcollections/ListCustomizedProductCollections';

export default {
  name: "home",
  data() {
    return {

      currentComp:Intro
    };
  },
  methods: {
    switchPage() {
  
      this.currentComp = ManagementTopBar;
    }
  },
  components: {
    Customizer,
    Intro,
    ManagementTopBar
  }
};
</script>

