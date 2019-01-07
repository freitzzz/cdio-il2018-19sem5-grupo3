<template>
  <div>
  
    <header class="modal-card-head">
      <p class="modal-card-title">Signup</p>
    </header>
    <section class="model-card-body">
      <div class="padding-div">
        <b-field label="Name">
          <b-input type="String" v-model="name" :placeholder="placeholder.name" icon="account"/>
        </b-field>
        <b-field label="E-mail">
          <b-input type="String" v-model="email" :placeholder="placeholder.email" icon="email" required></b-input>
        </b-field>
        <b-field label="Password">
          <b-input type="password" v-model="password" :placeholder="placeholder.password" icon="key" password-reveal required></b-input>
        </b-field>
        <b-field label="Phone Number">
          <telephone-input
            :enabledFlags="true"
            v-model="phoneNumber.value"
            @onInput="onPhoneNumberInput"
          />
        </b-field>


        <b-checkbox 
          v-model="openedPrivacyPolicy.value"
          type="is-info" 
          @input="confirmOpenedPrivacyPolicy"
          />
        <a @click="openPrivacyPolicy()">I have read the <u>Privacy Policy</u></a>
        <b-modal
          :active.sync="openedPrivacyPolicy.opened"
        >
          <privacy-policy/>
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

  /**
   * Requires PrivacyPolicy component
   */
  import PrivacyPolicy from './PrivacyPolicy.vue';
  
  /**
   * Requires vue-tel-input for the phone number input
   */
  import TelephoneInput from 'vue-tel-input';
  
  export default {
    /**
     * Component Data
     */
    data() {
      return {
        placeholder: {
          email: "sarahbonito@kkb.com",
          name: "Sarah Bonito",
          password: "*******",
          
        },
        email: "",
        password: "",
        phoneNumber:{
          value:"",
          valid:Boolean
        },
        name: "",
        openedPrivacyPolicy:{
          opened:false,
          value:false,
          times:0
        },
        checkBox: "",
      };
    },
    components: {
      PrivacyPolicy,
      TelephoneInput
    },
    /**
     * Component methods
     */
    methods: {

      /**
       * Confirms that the user has opened the signup form
       */
      confirmOpenedPrivacyPolicy(){
        if(this.openedPrivacyPolicy.times==0){
          this.openPrivacyPolicy();
        }
      },

      /**
       * Opens the privacy policy
       */
      openPrivacyPolicy(){
        this.openedPrivacyPolicy.opened=true;
        this.openedPrivacyPolicy.value=true;
        this.openedPrivacyPolicy.times++;
      },
  
      /**
       * Emits the signup action
       */
      emitSignup() {
        this
          .grantSignupDetailsAreValid()
          .then(()=>{
            let signupDetails={
              name:this.name,
              email:this.email,
              password:this.password,
              phoneNumber:this.phoneNumber.value
            };

            if(signupDetails.name.trim().length==0)
              delete signupDetails.name;
            
            if(signupDetails.phoneNumber.trim().length==0)
              delete signupDetails.phoneNumber;

            this.$emit('emitSignup',signupDetails);
            })
          .catch((message)=>{
            this.$toast.open({message:message});
          });
      },
      
      /**
       * Callback function that is called when phone number input is triggered
       */
      onPhoneNumberInput(phoneNumberInput){
        this.phoneNumber.valid=phoneNumberInput.isValid;
      },

      /**
       * Grants that the signup details are valid
       */
      grantSignupDetailsAreValid(){

        return new Promise((accept,reject)=>{

          if(this.email.trim().length==0){
            reject("Please provide an email!");
          }
          
          if(this.password.trim().length==0){
            reject("Please provide a password!");
          }

          if(this.phoneNumber.value.trim().length!=0&&!this.phoneNumber.valid){
            reject("Please provide a valid phone number!");
          }

          if(!this.openedPrivacyPolicy.value){
            reject("You must confirm that you've read the privacy policy!");
          }

          accept();
        });
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
