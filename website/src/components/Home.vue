<template>
  <component :is="currentComp" @switch-to-customizer="switchPage()"></component>
</template>
<script>
import Customizer from "./Customizer.vue";
import Intro from "./Intro.vue";
import SideBar from "./SideBar";
import ListProductsVue from './management/product/ListProducts.vue';
import ProductsTableVue from './management/product/ProductsTable.vue';
export default {
  name: "home",
  data() {
    return {

      currentComp:Intro
    };
  },
  methods: {
    switchPage() {
  
      this.currentComp = SideBar;
    }
  },
  components: {
    Customizer,
    Intro,
    SideBar
  }
};
</script>

