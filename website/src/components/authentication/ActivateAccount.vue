<template>
  <div class="modal-card" style="width:auto">
    <header class="modal-card-head">
      <p class="modal-card-title">Account Activation</p>
    </header>
    <section class="modal-card-body">
      <b-input
        type="String"
        v-model="activationCode"
        placeholder="Insert Activation Code here"
        icon="account-key"
        required
      ></b-input>
    </section>
    <footer class="modal-card-foot">
      <button class="btn-primary" @click="activateAccount">Activate Account</button>
    </footer>
  </div>
</template>

<script>

import UserRequests from "./../../services/myca_api/requests/users.js";

export default {

  name: "ActivateAccount",

  data() {
    return {
      activationCode: null
    };
  },

  methods:{

      activateAccount(){

          let activateAccountBody = {...this.userInfo};

          activateAccountBody.activationCode = this.activationCode;

          UserRequests.activateAccount(activateAccountBody)
            .then(response =>{
                this.$toast.open({
                    message: "Account activated with success!"
                });
                this.$emit("closeActivationModal");
            })
            .catch(error =>{
                let message = error.response.data.message;

                this.$toast.open({
                    message: message
                });
            });

      }

  },

  props: {
    userInfo: {
      type: Object,
      required: true
    }
  }

};
</script>
