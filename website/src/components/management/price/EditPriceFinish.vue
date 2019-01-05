<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
          <p class="modal-card-title">Edit Price Material</p>
      </header>
      <section class="modal-card-body">
        <b-field label="Description">
          <b-input v-model="material.description" disabled="true" type="String" icon="pound"></b-input>
        </b-field>
      <div>
        <b-field>
          <b-field label="Value">
            <b-input
              type="number"
              min="0"
              icon="cash-multiple"
              :placeholder= material.value
              v-model="selectedValue"
            ></b-input>
          </b-field>
           <b-field label="Currency">
            <b-select icon="coin" :placeholder= material.currency v-model="selectedCurrency">
              <option
                v-for="currency in this.currencies"
                :key="currency.currency"
                :value="currency"
              >{{currency.currency}}</option>
            </b-select>
          </b-field>
           <b-field label="Area">
            <b-select icon="move-resize-variant" :placeholder= material.area v-model="selectedArea">
              <option v-for="area in this.areas" :key="area.area" :value="area">{{area.area}}</option>
            </b-select>
          </b-field>
         </b-field>
        <b-field>
          <b-field label="Starting Date & Time">
            <b-field>
              <b-field>
                <b-datepicker
                  icon="calendar"
                  :placeholder= material.startingDate
                  v-model="startingDate"
                >
                  <button class="btn-primary" @click="startingDate= new Date()">
                    <b-icon icon="calendar-today"></b-icon>
                    <span>Today</span>
                  </button>
                  <button class="btn-primary" @click="startingDate = null">
                    <b-icon icon="close"></b-icon>
                    <span>Clear</span>
                  </button>
                </b-datepicker>
              </b-field>
              <b-field>
                <b-timepicker
                  icon="clock"
                  :placeholder= material.startingTime
                  v-model="startingTime"
                >
                  <button class="btn-primary" @click="startingTime = new Date()">
                    <b-icon icon="clock"></b-icon>
                    <span>Now</span>
                  </button>
                  <button class="btn-primary" @click="startingTime = null">
                    <b-icon icon="close"></b-icon>
                    <span>Clear</span>
                  </button>
                </b-timepicker>
              </b-field>
            </b-field>
          </b-field>
        </b-field>
        <b-field>
          <b-field label="Ending Date & Time">
            <b-field>
              <b-field>
                <b-datepicker
                  icon="calendar"
                  placeholder= material.endingDate
                  v-model="endingDate"
                >
                  <button class="btn-primary" @click="endingDate= new Date()">
                    <b-icon icon="calendar-today"></b-icon>
                    <span>Today</span>
                  </button>
                  <button class="btn-primary" @click="endingDate = null">
                    <b-icon icon="close"></b-icon>
                    <span>Clear</span>
                  </button>
                </b-datepicker>
              </b-field>
              <b-field>
                <b-timepicker icon="clock" placeholder= material.endingTme v-model="endingTime">
                  <button class="btn-primary" @click="endingTime = new Date()">
                    <b-icon icon="clock"></b-icon>
                    <span>Now</span>
                  </button>
                  <button class="btn-primary" @click="endingTime = null">
                    <b-icon icon="close"></b-icon>
                    <span>Clear</span>
                  </button>
                </b-timepicker>
              </b-field>
            </b-field>
          </b-field>
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
  name: "EditMaterial",
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
      startingDate: new Date(),
      endingDate: new Date(),
      startingTime: new Date(),
      endingTime: new Date()
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
      PriceTables.putMaterialPriceTableEntry(this.material.id, this.material.tableEntryId, updatedEntry)
        .then(this.$toast.open("Update te price of the material with success!"))
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
