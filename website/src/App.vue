<template>
    <div id="app">
      <customizer v-if="customizerEnabled"></customizer>
      <button v-else @click="enableCustomizer">Get Started</button>
    </div>
</template>

<script>
import Customizer from "./components/Customizer.vue";
export default {
  name: "app",
  data() {
    return {
      customizerEnabled: false
    };
  },
  methods: {
    enableCustomizer(){
      this.customizerEnabled = true;
    }
  },
  components: {
    Customizer
  }
};
</script>