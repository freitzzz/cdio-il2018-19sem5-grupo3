<template>
<body>
  <header>
    <div class="intro">
      <div class="topnav">
        <div class="title-container">
          <span class="small-text">
            <i class="material-icons md-12 md-blue btn no-hover">build</i>
            Make your closet
          </span>
        </div>
        <div class="login-container">
          <button type="submit" class="btn btn-custom-2 page-scroll" @click="signIn()">
            <span>
              <i class="material-icons md-18 md-white btn no-hover">person_outline</i>
              Sign in
            </span>
          </button>
        </div>
      </div>

      <div class="intro-text">
        <h1>Make your closet</h1>
        <p>Start now and make the closet of your dreams.</p>
        <a class="btn btn-custom btn-lg" @click="enableCustomizer()">Get Started</a>
      </div>
    </div>
  </header>
</body>
</template>

<script>
export default {
  name: "intro",
  data() {
    return {};
  },
  methods: {
    enableCustomizer() {
      this.$emit("switch-to-customizer");
    },
    signIn(){
      this.$emit("switch-to-sign-in-form");
    }
  },
  components: {}
};
</script>
<style>
</style>

