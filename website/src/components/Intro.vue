<template>
  <div class="container" >
    <!-- Brand and toggle get grouped for better mobile display -->
    <div class="navbar-header">
     <!--  <button
        type="button"
        class="navbar-toggle collapsed"
        data-toggle="collapse"
        data-target="#bs-example-navbar-collapse-1">
        <span class="sr-only">Toggle navigation</span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
        <span class="icon-bar"></span>
      </button> -->
     <!--  <a class="navbar-brand page-scroll" href="#page-top">
        <i class="account"></i> Make your closet
      </a> -->
    </div>
<!--

    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
      <ul class="nav navbar-nav navbar-right">
        <li>
          <a class="page-scroll">Create a closet</a>
        </li>
        <li>
          <a class="page-scroll">Login</a>
        </li>
      </ul>
    </div> -->
    <!-- /.navbar-collapse -->
    <!-- Header -->
    <header id="header">
      <div class="intro">
        <div class="overlay">
          <div class="container">
            <div class="row">
              <div class="intro-text">
                <h1>Make your closet</h1>
                <p>Start now and make the closet of your dreams.</p>
                <a class="btn btn-custom btn-lg page-scroll" @click="enableCustomizer()">Get Started</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  </div>
</template>
<script>

export default {
  name: "intro",
  data() {
    return {
        
    };
  },
  methods: {
      enableCustomizer(){
 
          this.$emit('switch-to-customizer');
      }
  },
  components: {

  }
};
</script>
