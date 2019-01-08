<template>
  <main class="main-content">
    <div class="invoice-app">
      <div class="header">
        <div>
          <!-- h1 with the name of the company ??? -->
          <p>Date: <input readonly v-model="currentDate"></p>
        </div>
        <div class="section-spacer">
          <p><strong>Bill to:</strong></p>
          <!-- client data it will not be a text area -->
          <input v-model="clientData" readonly>
        </div>
      </div>
      <div>
        <label for="currency-picker">Currency:</label>
        <select v-model="currency">
          <option v-for="currencyValue in currencies" :key="currencyValue.currency" :value="currencyValue">{{currencyValue.currency}}</option>
        </select>
      </div>
      <table class="responsive-table"></table>
      <!-- button add new item (not necessary) -->
      <table></table>
    </div>
    <div class="btn-section">
      <i class="btn btn-primary" @click="previousPanel()">Go back</i>
      <i class="btn btn-primary" @click="payment()">Payment</i>
    </div>
  </main>
</template>

<script>
  import CurrencyPerArea from './../../services/mycm_api/requests/currenciesperarea.js'
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
        clientData: "Fernando Mendes",
        /* Client data is: name, address and phone number */
        currency: "",
        currencies: [],
        items: [],
        item: []
      }
    },
    created() {
      store.dispatch(DEACTIVATE_CAN_MOVE_COMPONENTS);
      
      /**Get list of all the currencies */
      CurrencyPerArea.getCurrencies()
      .then(response =>this.currencies.push(...response.data))
      .catch(error =>{
        this.$toast.open("An error occured trying to fetch the currencies.");
      });
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
  .btn-section {
    width:300px;

  }
  .center-controls {
    text-align: center;
    margin: auto;
  }
  
  .main-content {
    min-height: 100vh;
    padding: 3%;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .invoice-app {
    background-color: white;
    padding: 2%;
    border-radius: 0.5rem;
    width: 350px;
  }
  
  
  /* 
  .header {
  
  
       div {
          &:nth-child(-n+1){
              @media (min-width: 761px) {
                  order: 1;
                  flex: 1;
                  text-align: right;
                  padding-left: 1rem;
              }
          }
      }
  } */
  
  .section-spacer {
    margin: 1rem 0;
  }
  
  input,
  select,
  textarea {
    background-color: transparentize(color=white, amount=0.7);
    border: none;
    display: inline-block;
    transition: background-color 0.3s ease-in-out;
    width: 100%;
  }
  
  textarea {
    width: 100%;
    min-height: 80px;
  }
  
  
  /* select {
      only screen and (max-width: 760px) {
          width: 100%;
      }
  
      @media print {
          appearance: none;
      }
  } */
  
  table {
    width: auto;
    border-collapse: collapse;
    margin: 2rem 0;
  }
  
  
  /* 
  .responsive-table {
      width: 100%;
      @media 
      only screen and (max-width: 760px) {
  
          table, thead, tbody, th, td, tr { 
              display: block; 
          }
  
          thead tr { 
              position: absolute;
              top: -9999px;
              left: -9999px;
          }
  
          tr {
              padding: 2rem 0;
          }
  
          
          td[data-label] {
              position: relative;
              padding-left: 40%; 
              display: flex;
              align-items: center;
  
              &:before { 
                  content: attr(data-label);
                  position: absolute;
                  top: 0.5rem;
                  left: 0;
                  width: 35%; 
                  padding-right: 10px; 
                  white-space: nowrap;
                  font-weight: bold;
              }
          }
      }
  } */
  
  button {
    background-color: green;
    border: none;
    border-radius: 100px;
    padding: 0.5rem 1rem;
    cursor: pointer;
    transition: background-color 0.3s ease-in-out;
  }
  
  .text-right {
    text-align: right;
  }
  
  .text-bold {
    font-weight: bold;
  }
</style>
