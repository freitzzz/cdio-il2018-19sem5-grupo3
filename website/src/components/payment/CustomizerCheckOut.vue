<template>
  <main class="main-content">
    <div class="invoice-app">
      <div class="header">
        <div>
          <p>Date: <input readonly v-model="currentDate"></p>
        </div>
      </div>
      <div></div>
      <table class="responsive-table"></table>
      <!-- button add new item (not necessary) -->
      <table></table>
    </div>
    <div class="center-controls">
      <i class="btn btn-primary material-icons" @click="previousPanel()">arrow_back</i>
      <i class="btn btn-primary" @click="payment()">Payment</i>
    </div>
  </main>
</template>

<script>
  import Vue from 'vue';
  /* import { library } from '@fortawesome/fontawesome-svg-core';
  import { faCoffee } from '@fortawesome/free-solid-svg-icons';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'; */
  import store from "./../../store";
  import {
    DEACTIVATE_CAN_MOVE_COMPONENTS
  } from "./../../store/mutation-types.js";
  
  /* Constants: */
  
  const TAX_RATE_PORTUGAL = 23;
  
  
  export default {
    name: "CustomizerCheckOut",
    data() {
      return {
        currentDate: this.getCurrentDate(),
        taxRate: TAX_RATE_PORTUGAL, //TODO:
        clientData: "",
        /* Client data is: name, address and phone number */
        currency: "",
        currencies: [],
        items: [],
        item: []
      }
    },
    created() {
      store.dispatch(DEACTIVATE_CAN_MOVE_COMPONENTS);
  
    },
    methods: {
      getCurrentDate() {
        var today = new Date();
  
        var dd, mm, yyyy;
        dd = today.getDate();
        mm = today.getMonth() + 1;
        yyyy = today.getFullYear();
  
        dd < 10 ? dd = '0' + dd : dd;
        mm < 10 ? mm = '0' + mm : mm;
  
        return dd + '/' + mm + '/' + yyyy;
      },
      previousPanel() {
        this.$emit("back");
      },
      payment() {
        this.$toast.open('Payment Successful');
      }
    }
  }
</script>

<style>
  
</style>
