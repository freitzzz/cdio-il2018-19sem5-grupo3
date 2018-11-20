<template>
    <div id="app">
<!--       <customizer v-if="customizerEnabled"></customizer>
      <button v-else @click="enableCustomizer">Get Started</button> -->
      <create-new-category :active=true></create-new-category>
    </div>
</template>

<script>
import Customizer from "./components/Customizer.vue";
import CreateNewCategory from "./components/management/category/CreateNewCategory.vue";
export default {
  name: "app",
  data() {
    return {
      customizerEnabled: false
    };
  },
  methods: {
    enableCustomizer() {
      this.customizerEnabled = true;
    }
  },
  components: {
    Customizer,
    CreateNewCategory
  }
};
</script> 