<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">Edit Material Finish Price Table Entry</p>
      </header>
      <section class="modal-card-body">
        <b-field label="Description">
          <b-input
            v-model="materialFinishPrice.description"
            disabled="true"
            type="String"
            icon="pound"
          ></b-input>
        </b-field>
        <b-field label="Shininess">
          <b-input
            v-model="materialFinishPrice.shininess"
            disabled="true"
            type="String"
            icon="pound"
          ></b-input>
        </b-field>
        <div>
          <b-field>
            <b-field label="Value">
              <b-input
                type="number"
                min="0"
                icon="cash-multiple"
                placeholder="Insert value here"
                v-model="selectedValue"
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
                    placeholder="Click to choose time"
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
                  <b-timepicker
                    icon="clock"
                    placeholder="Click to choose time"
                    v-model="endingTime"
                  >
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
        <button class="btn-primary" @click="updateMaterialFinishPriceTableEntry()">Edit</button>
      </footer>
    </div>
  </div>
</template> 

<script>
import Axios from "axios";
import Config, { MYCM_API_URL } from "../../../config.js";
import PriceTables from "./../../../services/mycm_api/requests/pricetables.js";
import materials from "../../../services/mycm_api/requests/materials.js";

export default {
  name: "EditPriceFinish",

  async created() {
    await Axios.get(MYCM_API_URL + `/currenciesperarea/currencies`)
      .then(response => {
        this.currencies = response.data;
      })
      .catch(error => {
        //throw error?
      });
    await Axios.get(MYCM_API_URL + `/currenciesperarea/areas`)
      .then(response => {
        this.areas = response.data;
      })
      .catch(error => {
        //throw error?
      });

    this.selectedValue = this.materialFinishPrice.value;
    for (let i = 0; i < this.currencies.length; i++) {
      if (this.materialFinishPrice.currency === this.currencies[i].currency) {
        this.selectedCurrency = { ...this.currencies[i] };
        break;
      }
    }
    for (let i = 0; i < this.areas.length; i++) {
      if (this.materialFinishPrice.area === this.areas[i].area) {
        this.selectedArea = { ...this.areas[i] };
        break;
      }
    }
    this.startingDate = new Date(this.materialFinishPrice.startingDate);
    this.endingDate = new Date(this.materialFinishPrice.endingDate);
    this.startingTime = new Date(
      this.materialFinishPrice.startingDate +
        "T" +
        this.materialFinishPrice.startingTime
    );
    this.endingTime = new Date(
      this.materialFinishPrice.endingDate +
        "T" +
        this.materialFinishPrice.endingTime
    );
  },

  data() {
    return {
      currencies: Array,
      areas: Array,
      selectedValue: null,
      selectedCurrency: null,
      selectedArea: null,
      startingDate: null,
      endingDate: null,
      startingTime: null,
      endingTime: null
    };
  },

  methods: {
    updateMaterialFinishPriceTableEntry() {
      if (this.selectedCurrency == null || this.selectedArea == null) {
        this.$toast.open({
          message: "Choose a currency and an area before you save your update!"
        });
        return;
      }
      if (this.startingDate == null || this.endingDate == null) {
        this.$toast.open({
          message:
            "Make sure you choose a starting date and an ending date before you save your update!"
        });
        return;
      }
      if (this.startingTime == null || this.endingTime == null) {
        this.$toast.open({
          message:
            "Make sure you choose a starting time and an ending time before you save your update!"
        });
        return;
      }
      var updatedEntry = {
        tableEntry: {
          price: {
            value: this.selectedValue,
            currency: this.selectedCurrency.currency,
            area: this.selectedArea.area
          },
          startingDate: this.parseDateTimeToGeneralIsoFormatString(
            this.startingDate,
            this.startingTime
          ),
          endingDate: this.parseDateTimeToGeneralIsoFormatString(
            this.endingDate,
            this.endingTime
          )
        }
      };
      this.$emit(
        "updateMaterialFinishPriceTableEntry",
        this.materialFinishPrice.materialId,
        this.materialFinishPrice.finishId,
        this.materialFinishPrice.tableEntryId,
        updatedEntry
      );
    },

    parseDateTimeToGeneralIsoFormatString(date, time) {
      let dateToIso = date == null ? null : date.toISOString();
      let timeToIso = time == null ? null : time.toISOString();
      return dateToIso == null || timeToIso == null
        ? ""
        : dateToIso.split("T")[0] + "T" + timeToIso.split("T")[1].split(".")[0];
    }
  },

  props: {
    /**
     * Current Material details
     */
    materialFinishPrice: {
      type: Object,
      required: true
    }
  }
};
</script>
