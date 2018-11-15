<template>
    <div id="app">
    </div>
</template>

<script>
import Product from "./components/Product.vue";
import Customizer from "./components/Customizer.vue";
export default {
  name: "app",
  data() {
    return {
      title: "Make Your Closet"
    };
  },
  methods: {
    loadCustomizer(){
    }
  },
  components: {
    Customizer
  }
};
</script>