<template>
  <div class="modal-card" style="width: auto">
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
        <a @click="activateModalPrivacy">I have read the <u>Privacy Policy</u></a>
      </div>
    <div v-if="activateModal">  
      <b-modal :active.sync="activateModal" has-modal-card scroll="keep">
        <privacy-policy-modal></privacy-policy-modal>
      </b-modal>
    
    </div>
      <!-- Create check box + form  -->
    </section>
    <footer class="modal-card-foot">
      <div class="has-text-centered">
        <button class="btn-primary" @click="emitSignup()">Sign Up</button>
      </div>
    </footer>
 
  
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
        activateModal: false
  
      };
    },
    /**
     * Component methods
     */
    methods: {
      activateModalPrivacy() {
        if (this.activateModal) {
          this.activateModal = false;
        }else{
          this.activateModal = true;
        }
      },
  
      iHaveReadThePolicy: function() {
        if (this.privacyCheckBox) {
          this.privacyCheckBox = false;
        } else {
          this.privacyCheckBox = true;
        }
      },
  
      /**
       * Emits the signup action
       */
      emitSignup() {
        if (this.privacyCheckBox) {
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
    },
    components: {
      PrivacyPolicyModal
    }
  
  };
</script>

<style>
  u {
    text-decoration: underline;
    color: #0ba2db;
  }
</style>
