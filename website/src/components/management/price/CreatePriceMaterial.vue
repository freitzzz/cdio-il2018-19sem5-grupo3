<template>
  <div class="modal-card" style="width: auto">
    <header class="modal-card-head">
      <p class="modal-card-title">Schedule Material Price</p>
    </header>
    <section class="modal-card-body">
      <div v-if="materials.length > 0">
        <b-field label="Select the Materials">
          <b-autocomplete
            rounded
            v-model="searchedMaterial"
            :keep-first="true"
            :data="suggestedMaterials"
            field="designation"
            :clean-on-select="true"
            @select="option => selectMaterial(option)"
            placeholder="Search by Material designation"
            icon="magnify"
          >
            <template slot="empty">No materials found</template>
          </b-autocomplete>
        </b-field>

        <!--Prevents the auto complete prompt from overlapping the checkboxes-->
        <div v-if="isInputtingData" class="expandable-div"></div>

        <b-table
          :data="materials"
          :checked-rows.sync="selectedMaterials"
          :paginated="true"
          :pagination-simple="true"
          checkable
          per-page="5"
        >
          <template slot-scope="props">
            <b-table-column label="Designation">{{props.row.designation}}</b-table-column>
          </template>
        </b-table>
      </div>
      <div>
        <b-field>
          <b-field label="Value">
            <b-input
              type="number"
              min="0"
              icon="cash-multiple"
              placeholder="Insert value here"
              v-model="priceValue"
            ></b-input>
          </b-field>
          <b-field label="Currency">
            <b-select icon="coin" placeholder="Currency" v-model="selectedCurrency">
              <option
                v-for="currency in this.currencies"
                :key="currency.currency"
                :value="currency"
              >{{currency.currency}}</option>
            </b-select>
          </b-field>
          <b-field label="Area">
            <b-select icon="move-resize-variant" placeholder="Area" v-model="selectedArea">
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
                  placeholder="Click to choose date"
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
                  placeholder="Click to choose time"
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
                  placeholder="Click to choose date"
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
                <b-timepicker icon="clock" placeholder="Click to choose time" v-model="endingTime">
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
      <button class="btn-primary" @click="createPriceTableEntries">Schedule Price</button>
    </footer>
  </div>
</template>

<script>
import Axios from "axios";
import MaterialRequests from "./../../../services/mycm_api/requests/materials.js";
import PriceTableRequests from "./../../../services/mycm_api/requests/pricetables.js";
import CurrenciesPerAreaRequests from "./../../../services/mycm_api/requests/currenciesperarea.js";

export default {
  name: "CreatePriceMaterial",

  data() {
    return {
      searchedMaterial: "",
      selectedMaterials: [],
      materials: [],
      priceValue: "",
      currencies: Array,
      areas: Array,
      selectedCurrency: null,
      selectedArea: null,
      startingDate: new Date(),
      endingDate: new Date(),
      startingTime: new Date(),
      endingTime: new Date()
    };
  },

  created() {
    this.getAvailableMaterials();
    CurrenciesPerAreaRequests.getCurrencies()
      .then(response => {
        this.currencies = response.data;
      })
      .catch(error => {
        this.$toast.open(error.response.data.message);
      });
    CurrenciesPerAreaRequests.getAreas()
      .then(response => {
        this.areas = response.data;
      })
      .catch(error => {
        this.$toast.open(error.response.data.message);
      });
  },

  methods: {
    getAvailableMaterials() {
      MaterialRequests.getMaterials()
        .then(response => {
          this.materials.push(...response.data);
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    },

    selectMaterial(material) {
      var alreadyAdded = false;

      //check if the material was already added
      for (let i = 0; i < this.selectedMaterials.length; i++) {
        if (this.selectedMaterials[i].id == material.id) {
          alreadyAdded = true;
          break;
        }
      }

      if (!alreadyAdded) {
        this.selectedMaterials.push(material);
      }
    },

    createPriceTableEntries() {
      let entries = [];
      if(this.selectedCurrency == null || this.selectedArea == null){
        this.$toast.open({
          message : "Choose a currency and an area before you schedule the price!"
        });
        return;
      }
      for (let i = 0; i < this.selectedMaterials.length; i++) {
        entries.push({
          materialId: this.selectedMaterials[i].id,
          tableEntry: {
            tableEntry: {
              price: {
                value: this.priceValue,
                currency: this.selectedCurrency.currency,
                area: this.selectedArea.area
              },
              startingDate: this.parseDateTimeToGeneralIsoFormatString(this.startingDate, this.startingTime),
              endingDate: this.parseDateTimeToGeneralIsoFormatString(this.endingDate, this.endingTime)
            }
          }
        });
      }

      this.$emit("createMaterialPriceTableEntry", entries);
    },

    parseDateTimeToGeneralIsoFormatString(date, time){
      let dateToIso = date == null ? null : date.toISOString();
      let timeToIso = time == null ? null : time.toISOString();
      return dateToIso == null && timeToIso == null ? null : dateToIso.split("T")[0] + "T" + timeToIso.split("T")[1].split(".")[0];
    }
  },

  computed: {
    suggestedMaterials() {
      var suggestedMaterials = [];
      //the "i" flag makes the pattern case insensitive
      var patt = new RegExp(`^.*(${this.searchedMaterial}).*$`, "i");

      for (let i = 0; i < this.materials.length; i++) {
        var match = patt.test(this.materials[i].designation);
        if (match) {
          suggestedMaterials.push(this.materials[i]);
        }
      }

      return suggestedMaterials;
    },

    isInputtingData() {
      return this.searchedMaterial.length > 0;
    }
  }
};
</script>

<style>
.expandable-div {
  padding-bottom: 25%;
}
</style>