<template>
  <div>
  
    <header class="modal-card-head">
      <p class="modal-card-title">Signup</p>
    </header>
    <section class="model-card-body">
      <div class="padding-div">
        <b-field label="Name">
          <b-input type="String" v-model="name" :placeholder="placeholder.name" icon="account" required></b-input>
        </b-field>
        <b-field label="E-mail">
          <b-input type="String" v-model="email" :placeholder="placeholder.email" icon="email" required></b-input>
        </b-field>
        <b-field label="Password">
          <b-input type="password" v-model="password" :placeholder="placeholder.password" icon="key" password-reveal required></b-input>
        </b-field>
  
        <b-checkbox type="is-info" @input="iHaveReadThePolicy"> </b-checkbox>
        <a @click="emitPrivacy()">I have read the <u>Privacy Policy</u></a>
      </div>
      <!-- Create check box + form  -->
    </section>
    <footer class="modal-card-foot">
      <div class="has-text-centered">
        <button class="btn-primary" @click="emitSignup()">Sign Up</button>
      </div>
    </footer>
    <div v-if="activateModalForm">
      <b-modal :active.sync="activateModalForm" class="modal-card" style="width:100% overflow-y: auto  overflow-x: hidden">
        <privacy-policy-modal ></privacy-policy-modal>
      </b-modal>
    </div>
  </div>
</template>

<script>
  import PrivacyPolicyModal from './PrivacyPolicyModal.vue';
  export default {
    /**
     * Component Data
     */
    data() {
      return {
        placeholder: {
          email: "superemail@email.com",
          password: "superpassword",
          name: "supername"
        },
        email: "",
        password: "",
        name: "",
        privacyCheckBox: false,
        checkBox: "",
        activateModalForm: false,
  
  
      };
    },
    components: {
      PrivacyPolicyModal
    },
    /**
     * Component methods
     */
    methods: {
      emitPrivacy: function() {
        this.activateModalForm ? this.activateModalForm = false : this.activateModalForm = true;
      },
      iHaveReadThePolicy: function() {
       this.privacyCheckBox ?  this.privacyCheckBox = false :  this.privacyCheckBox = true;
      },
  
      /**
       * Emits the signup action
       */
      emitSignup() {
        if (this.privacyCheckBox == true) {
          var invalidEmail = !this.email || this.email.trim() == "";
          var invalidName = !this.name || this.name.trim() == "";
          var invalidPassword = !this.password || this.password.trim() == "";
  
          if (invalidEmail && invalidPassword && invalidName) {
            this.$toasted.show(
              "Please, insert the required information to log in.", {
                position: "top-center",
                duration: 2000
              }
            );
          } else if (invalidName) {
            this.$toasted.show("Please, insert a valid name.", {
              position: "top-center",
              duration: 2000
            });
          } else if (invalidEmail) {
            this.$toasted.show("Please, insert a valid e-mail.", {
              position: "top-center",
              duration: 2000
            });
          } else if (invalidPassword) {
            this.$toasted.show("Please, insert a valid password.", {
              position: "top-center",
              duration: 2000
            });
          } else {
            let signupDetails = {
              email: this.email,
              password: this.password,
              name: this.name
            };
            this.$emit("emitSignup", signupDetails);
          }
        } else {
          this.$toast.open('Please confirm that you have read our privacy policy');
        }
      }
    }
  
  };
</script>

<style>
  u {
    text-decoration: underline;
    color: #0ba2db;
  }
</style>
