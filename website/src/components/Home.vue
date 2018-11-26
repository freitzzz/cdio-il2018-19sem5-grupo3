<template>
  <component :is="currentComp" @switch-to-customizer="switchPage()"></component>
</template>
<script>
import Customizer from "./Customizer.vue";
import Intro from "./Intro.vue";
export default {
  name: "home",
  data() {
    return {

      currentComp:Intro
    };
  },
  methods: {
    switchPage() {
  
      this.currentComp = Customizer;
    }
  },
  components: {
    Customizer,
    Intro
  }
};
</script>

