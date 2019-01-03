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
        <router-link class="btn btn-custom btn-lg" tag="a" to="/customization">Get Started</router-link>
      </div>
    </div>
  </header>
  <router-view />
</body>
</template>

<script>
export default {
  name: "intro",
  data() {
    return {};
  },
  methods: {
    signIn(){
      this.$emit("switch-to-sign-in-form");
    }
  }
};
</script>
<style>
</style>

