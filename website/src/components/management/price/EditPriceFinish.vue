<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
          <p class="modal-card-title">Edit Price Finish</p>
      </header>
      <section class="modal-card-body">
        <b-field label="Designation">
          <b-input v-model="material.designation" disabled="true" type="String" icon="pound"></b-input>
        </b-field>
        <b-field label="Edit Price">
           <b-input :placeholder= material.value v-model="selectedValue" type="String" icon="pound"></b-input>
        </b-field>
        <b-field label="Currency"> 
          <b-select icon="coin" :placeholder= material.currency v-model="selectedCurrency">
            <option  v-for="currency in this.currencies" 
              :key="currency.currency" 
              :value="currency">
              {{currency.currency}}</option>
          </b-select>
        </b-field>
        <b-field label="Area"> 
          <b-select icon="move-resize-variant" :placeholder= material.area v-model="selectedArea">
            <option  v-for="area in this.areas" 
              :key="area.area" 
              :value="area">
              {{area.area}}</option>
          </b-select>
        </b-field>
        <div id="app" class="container">
          <b-field label="Select a date">
              <b-datepicker
                  :placeholder= material.startingDate
                  icon="calendar-today"
                  v-model="selectedInitialData">
              </b-datepicker>
          </b-field>
        </div>
      </section>
      <footer class="modal-card-foot">
        <button class="btn-primary" @click="updateBasicInformation()">Edit</button>
      </footer>
    </div>
  </div>
</template> 
<script>
import Axios from "axios";
import Config,{ MYCM_API_URL } from '../../../config.js';
import PriceTables from './../../../services/mycm_api/requests/pricetables.js';
import materials from '../../../services/mycm_api/requests/materials.js';
export default {
  name: "EditFinish",
  created(){
   
      Axios.get(MYCM_API_URL+`/currenciesperarea/currencies`)
      .then(response => {
        this.currencies = response.data
      })
       .catch((error)=>{
                //throw error?
            });
    Axios.get(MYCM_API_URL+`/currenciesperarea/areas`)
      .then(response => {
        this.areas = response.data
      })
      .catch((error)=>{
                //throw error?
            });  
    },
  data() {
    return {
      currencies:Array,
      areas:Array,
      selectedValue: null,
      selectedCurrency: null,
      selectedArea: null,
      selectedInitialData: null,
      selectedFinalData: null,
      iniData: null
    };
  },
  methods: {
    updateBasicInformation() {
      var updatedEntry = {
        tableEntry: {
          price: {
            value: this.selectedValue,
            currency: this.selectedCurrency.currency,
            area: this.selectedArea.area
          },
          //startingDate: this.selectedInitialData,
          //endingDate: this.selectedFinalData
          startingDate: "2019-01-04T18:12:00",
          endingDate: "2019-01-22T12:07:00"
        }
      }
      PriceTables.putMaterialFinishPriceTableEntry(this.material.id, this.material.tableEntryId, updatedEntry)
        .then(this.$toast.open("Update te price of the finish with success!"))
        .catch();
      }
  },
  props: {
    /**
     * Current Material details
     */
    material: {
      type: Object,
      required: true
    }
  }
};
</script>
