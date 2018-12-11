<template>
  <div>
    <div v-if="showLogInModal">
      <b-modal :active.sync="showLogInModal">
      <Login @closeLogin="closeLogin()" @signUp="signUp()"/>
      </b-modal>
    </div>
    <div v-if="showSignUpModal">
      <b-modal :active.sync="showSignUpModal">
      <Signup @closeSignUp="closeSignUp()"/>
      </b-modal>
    </div>
    <component :is="currentComp" @switch-to-customizer="switchPage()" @switch-to-sign-in-form="logIn()"
    @switch-to-sign-up-form="signUp()"></component>
  </div>
</template>

<script>
import Customizer from "./Customizer.vue";
import Intro from "./Intro.vue";
import Login from "./authentication/Login.vue";
import Signup from "./authentication/Signup.vue";

export default {
  name: "home",
  data() {
    return {
      currentComp: Intro,
      showLogInModal: false,
      showSignUpModal: false
    };
  },
  methods: {
    switchPage() {
      this.currentComp = Customizer;
    },
    logIn() {
      this.showLogInModal = true;
    },
    closeLogIn(){
      this.showLogInModal = false;
    },
    signUp(){
      this.showLogInModal = false;
      this.showSignUpModal = true;
    },
    closeSignUp(){
      this.showSignUpModal = false;
    }
  },
  components: {
    Customizer,
    Intro,
    Login,
    Signup
  }
};
</script>

