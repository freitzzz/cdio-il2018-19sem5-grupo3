<template>
  <div>
    <div v-if="showSignUpModal">
      <Signup></Signup>
    </div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">Login</p>
      </header>
      <section class="model-card-body">
        <div class="padding-div">
          <b-field label="E-mail">
            <b-input
              type="String"
              v-model="email"
              :placeholder="placeholder.email"
              icon="email"
              required
            ></b-input>
          </b-field>
          <b-field label="Password">
            <b-input
              type="password"
              v-model="password"
              :placeholder="placeholder.password"
              icon="key"
              password-reveal
              required
            ></b-input>
          </b-field>
          <b-field>
            <a class="underlined-button" @click="signUp()">
              <u>Haven't registered yet? Click here to register.</u>
            </a>
          </b-field>
        </div>
      </section>
      <footer class="modal-card-foot">
        <div class="has-text-centered">
          <button class="btn-primary" @click="emitLogin()">Login</button>
        </div>
      </footer>
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import Toasted from "vue-toasted";
import Signup from "./../authentication/Signup.vue";

Vue.use(Toasted);

export default {
  /**
   * Component Data
   */
  data() {
    return {
      showSignUpModal: false,
      placeholder: {
        email: "superemail",
        password: "superpassword"
      },
      email: "",
      password: ""
    };
  },
  components: {
    Signup
  },
  /**
   * Component methods
   */
  methods: {
    /**
     * Emits the login action
     */
    emitLogin() {
      var checkEmail = !this.email || this.email.trim() == "";
      var checkPassword = !this.password || this.password.trim() == "";

      if (checkEmail && checkPassword) {
        this.$toasted.show(
          "Please, insert the required information to log in.",
          {
            position: "top-center",
            duration: 2000
          }
        );
      } else if (checkEmail) {
        this.$toasted.show("Please, insert a valid e-mail.", {
          position: "top-center",
          duration: 2000
        });
      } else if (checkPassword) {
        this.$toasted.show("Please, insert a valid password.", {
          position: "top-center",
          duration: 2000
        });
      } else {
        let loginDetails = {
          email: this.email,
          password: this.password
        };
        this.$emit("emitLogin", loginDetails);
      }
    },
    signUp() {
      this.$emit("signUp");
    }
  }
};
</script>

<style>
/* Underlined button (register button) */
.underlined-button {
  color: rgb(158, 158, 158) !important;
  border: 3px;
  margin: 5px;
  font-size: 13px;
  margin-top: 4px;
  transition: all 0.3s;
}

.underlined-button:hover {
  color: rgb(231, 231, 231) !important;
  cursor: pointer;
  transition: all 0.3s;
}

/* Custom text box (email, password input fields) */
.b-input {
  border-radius: 6px;
  outline: none;
  width: 100%;
  padding: 3px 0px 3px 3px;
  margin: 5px 1px 3px 0px;
  border: 1px solid #f0f0f0;
}

.b-input:focus {
  box-shadow: 0 0 5px #87d5f1;
  border: 1px solid #87d5f1;
  transition: all 0.3s;
}

.b-input:hover {
  box-shadow: 0 0 5px #e6e6e6;
  transition: all 0.3s;
}
</style>