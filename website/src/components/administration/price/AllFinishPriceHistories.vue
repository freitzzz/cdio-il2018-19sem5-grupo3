<template>
  <div class="modal-card" style="width:auto">
    <header class="modal-card-head">
      <p class="modal-card-title">Comparison between all Material Finish's Price Histories</p>
    </header>
    <section class="modal-card-body">
      <b-field grouped>
        <b-field>
          <b-field>
            <b-select
              icon="coin"
              placeholder="Currency"
              v-model="selectedCurrency"
              @input="showPlotTimeSeriesChart"
            >
              <option
                v-for="currency in this.currencies"
                :key="currency.currency"
                :value="currency"
              >{{currency.currency}}</option>
            </b-select>
          </b-field>
          <b-field>
            <b-select
              icon="move-resize-variant"
              placeholder="Area"
              v-model="selectedArea"
              @input="showPlotTimeSeriesChart"
            >
              <option v-for="area in this.areas" :key="area.area" :value="area">{{area.area}}</option>
            </b-select>
          </b-field>
        </b-field>
      </b-field>
      <div style="width:100%" ref="timeSeriesChart"></div>
    </section>
  </div>
</template>

<script>
import Plotly from "plotly.js-finance-dist";
import PriceTableRequests from "./../../../services/mycm_api/requests/pricetables.js";
import MaterialRequests from "./../../../services/mycm_api/requests/materials.js";
import CurrenciesPerAreaRequests from "./../../../services/mycm_api/requests/currenciesperarea.js";

export default {
  name: "AllFinishPriceHistories",

  async created() {
    await CurrenciesPerAreaRequests.getCurrencies()
      .then(response => {
        this.currencies = response.data;
      })
      .catch(error => {
        //throw error?
      });
    await CurrenciesPerAreaRequests.getAreas()
      .then(response => {
        this.areas = response.data;
      })
      .catch(error => {
        //throw error?
      });
    this.selectedCurrency = { ...this.currencies[0] };
    this.selectedArea = { ...this.areas[0] };
    this.showPlotTimeSeriesChart();
  },

  data() {
    return {
      currencies: Array,
      areas: Array,
      selectedCurrency: null,
      selectedArea: null
    };
  },

  props: {
    materialFinishes: {
      type: Object,
      required: true
    }
  },

  methods: {
    async showPlotTimeSeriesChart() {
        alert(this.selectedArea.area);
      let plotData = [];
      for (let i = 0; i < this.materialFinishes.finishes.length; i++) {
        try {
            alert(this.selectedCurrency.currency);
          let { data } = await PriceTableRequests.getMaterialFinishPriceHistory(
            this.materialFinishes.material.id,
            this.materialFinishes.finishes[i].id,
            this.selectedCurrency.currency,
            this.selectedArea.area
          );
          let xAxisArray = [];
          let yAxisArray = [];
          for (let j = 0; j < data.length; j++) {
            xAxisArray.push(data[j].startingDate);
            xAxisArray.push(data[j].endingDate);
            yAxisArray.push(data[j].value);
            yAxisArray.push(data[j].value);
          }

          let trace = {
            type: "scatter",
            mode: "lines",
            name: this.materialFinishes.material.designation + " " + this.materialFinishes.finishes[i].description,
            x: xAxisArray,
            y: yAxisArray
          };

          plotData.push(trace);
        } catch (error) {
          //Throw error?
        }
      }
      let layout = {
        title: "Price Evolution Time Series",
        width: 800,
        height: 500
      };
      Plotly.newPlot(this.$refs.timeSeriesChart, plotData, layout);
    }
  }
};
</script>