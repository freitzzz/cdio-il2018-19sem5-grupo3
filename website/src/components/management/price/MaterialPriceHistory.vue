<template>
  <div class="modal-card" style="width: auto">
    <header class="modal-card-head">
      <p class="modal-card-label">Material Price History</p>
    </header>
    <section class="modal-card-body">
      <div>
        <b-field grouped>
          <b-field>
            <button class="btn-primary" @click="fetchRequests()">
              <b-icon icon="refresh"/>
            </button>
          </b-field>
          <b-field>
            <b-select
              icon="coin"
              placeholder="Currency"
              v-model="selectedCurrency"
              @input="convertValuesToCurrency"
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
              @input="convertValuesToArea"
            >
              <option v-for="area in this.areas" :key="area.area" :value="area">{{area.area}}</option>
            </b-select>
          </b-field>
        </b-field>
        <b-table :data="data" :paginated="true" :pagination-simple="true" per-page="10">
          <template slot-scope="props">
            <b-table-column field="id" label="ID">{{props.row.id}}</b-table-column>
            <b-table-column field="price" label="Price">{{props.row.value}}</b-table-column>
            <b-table-column
              field="startingDateTime"
              label="Starting Date & Time"
            >{{props.row.startingDateTime}}</b-table-column>
            <b-table-column
              field="endingDateTime"
              label="Ending Date & Time"
            >{{props.row.endingDateTime}}</b-table-column>
            <b-table-column field="actions" label="Actions">
              <div class="custom-actions">
                <button class="btn-primary" @click="editMaterialPriceTableEntry(props.row.id)">
                  <b-icon icon="pencil"></b-icon>
                </button>
              </div>
              <div v-if="showEditMaterialPriceTableEntryModal">
                <b-modal :active.sync="showEditMaterialPriceTableEntryModal">
                  <edit-price-material
                    :material="currentSelectedPriceTableEntry"
                    @updateMaterialPriceTableEntry="updateMaterialPriceTableEntry"
                  ></edit-price-material>
                </b-modal>
              </div>
            </b-table-column>
          </template>
        </b-table>
        <div style="width:100%" ref="timeSeriesChart"></div>
      </div>
    </section>
  </div>
</template>

<script>
import Plotly from "plotly.js-finance-dist";
import EditPriceMaterial from "./EditPriceMaterial";
import PriceTablesRequests from "./../../../services/mycm_api/requests/pricetables.js";
import MaterialRequests from "./../../../services/mycm_api/requests/materials.js";
import CurrenciesPerAreaRequests from "./../../../services/mycm_api/requests/currenciesperarea.js";

export default {
  name: "MaterialPriceHistory",

  components: {
    EditPriceMaterial
  },

  /**
   * Received properties from father component
   */
  props: {
    materialId: {
      type: Number,
      required: true
    }
  },

  /**
   * Component Created State call
   */
  created() {
    this.fetchRequests();
    CurrenciesPerAreaRequests.getCurrencies()
      .then(response => {
        this.currencies = response.data;
      })
      .catch(error => {
        this.$toast.open(error.response.data);
      });
    CurrenciesPerAreaRequests.getAreas()
      .then(response => {
        this.areas = response.data;
      })
      .catch(error => {
        this.$toast.open(error.response.data);
      });
  },
  /**
   * Component data
   */
  data() {
    return {
      selectedCurrency: null,
      selectedArea: null,
      currencies: Array,
      areas: Array,
      showEditMaterialPriceTableEntryModal: false,
      currentSelectedPriceTableEntry: null,
      data: []
    };
  },

  methods: {
    async convertValuesToCurrency() {
      for (let i = 0; i < this.data.length; i++) {
        try {
          let auxArray = this.data[i].value.split(" ");
          let value = auxArray[0];
          auxArray = auxArray[1].split("/");
          let fromCurrency = auxArray[0];
          let fromArea = auxArray[1];
          let toCurrency = this.selectedCurrency.currency;
          const {
            data: convertedPrice
          } = await CurrenciesPerAreaRequests.convertValue(
            fromCurrency,
            toCurrency,
            fromArea,
            fromArea,
            value
          );
          this.data[i].value =
            convertedPrice.value +
            " " +
            convertedPrice.currency +
            "/" +
            convertedPrice.area;
        } catch (error) {
          this.$toast.open(error.response.data);
        }
      }
      this.plotTimeSeriesChart();
    },

    async convertValuesToArea() {
      for (let i = 0; i < this.data.length; i++) {
        try {
          let auxArray = this.data[i].value.split(" ");
          let value = auxArray[0];
          auxArray = auxArray[1].split("/");
          let fromCurrency = auxArray[0];
          let fromArea = auxArray[1];
          let toArea = this.selectedArea.area;
          const {
            data: convertedPrice
          } = await CurrenciesPerAreaRequests.convertValue(
            fromCurrency,
            fromCurrency,
            fromArea,
            toArea,
            value
          );
          this.data[i].value =
            convertedPrice.value +
            " " +
            convertedPrice.currency +
            "/" +
            convertedPrice.area;
        } catch (error) {
          this.$toast.open(error.response.data);
        }
      }
      this.plotTimeSeriesChart();
    },

    fetchRequests() {
      this.refreshTable();
    },

    async refreshTable() {
      this.data = [];

      try {
        const { data } = await PriceTablesRequests.getMaterialPriceHistory(
          this.materialId,
          "",
          ""
        );
        let sortEntriesByStartingDateTime = [];
        for (let i = 0; i < data.length; i++) {
          sortEntriesByStartingDateTime.push({
            id: data[i].id,
            value: data[i].value + " " + data[i].currency + "/" + data[i].area,
            startingDateTime: data[i].startingDate,
            endingDateTime: data[i].endingDate
          });
        }
        sortEntriesByStartingDateTime.sort(function(a, b) {
          return new Date(a.startingDateTime) - new Date(b.startingDateTime);
        });
        for (let i = 0; i < sortEntriesByStartingDateTime.length; i++) {
          let tempStartingDateTimeAsStringArray = sortEntriesByStartingDateTime[
            i
          ].startingDateTime.split("T");
          let tempEndingDateTimeAsStringArray = sortEntriesByStartingDateTime[
            i
          ].endingDateTime.split("T");
          sortEntriesByStartingDateTime[i].startingDateTime =
            tempStartingDateTimeAsStringArray[0] +
            " " +
            tempStartingDateTimeAsStringArray[1];
          sortEntriesByStartingDateTime[i].endingDateTime =
            tempEndingDateTimeAsStringArray[0] +
            " " +
            tempEndingDateTimeAsStringArray[1];
        }
        this.data = sortEntriesByStartingDateTime;
        this.plotTimeSeriesChart();
      } catch (error) {
        this.$toast.open(error.response.data);
      }
    },

    async editMaterialPriceTableEntry(tableEntryId) {
      for (let i = 0; i < this.data.length; i++) {
        if (this.data[i].id == tableEntryId) {
          try {
            const { data } = await MaterialRequests.getMaterial(
              this.materialId
            );
            this.currentSelectedPriceTableEntry = {
              id: data.id,
              tableEntryId: tableEntryId,
              reference: data.reference,
              designation: data.designation,
              value: this.data[i].value.split(" ")[0],
              currency: this.data[i].value.split(" ")[1].split("/")[0],
              area: this.data[i].value.split(" ")[1].split("/")[1],
              startingDate: this.data[i].startingDateTime.split(" ")[0],
              endingDate: this.data[i].endingDateTime.split(" ")[0],
              startingTime: this.data[i].startingDateTime.split(" ")[1],
              endingTime: this.data[i].endingDateTime.split(" ")[1]
            };
          } catch (error) {
            //Throw error?
          }
          break;
        }
      }
      this.showEditMaterialPriceTableEntryModal = true;
    },

    updateMaterialPriceTableEntry(materialId, tableEntryId, updatedEntry) {
      PriceTablesRequests.putMaterialPriceTableEntry(
        materialId,
        tableEntryId,
        updatedEntry
      )
        .then(response => {
          this.$toast.open({
            message: "Update was successful!"
          });
          this.refreshTable();
          this.showEditMaterialPriceTableEntryModal = false;
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    },

    plotTimeSeriesChart() {
      let xAxisArray = [];
      let yAxisArray = [];

      for (let i = 0; i < this.data.length; i++) {
        xAxisArray.push(this.data[i].startingDateTime);
        xAxisArray.push(this.data[i].endingDateTime);
        yAxisArray.push(this.data[i].value.split(" ")[0]);
        yAxisArray.push(this.data[i].value.split(" ")[0]);
      }

      let trace = {
        type: "scatter",
        mode: "lines",
        name: "Material " + this.materialId,
        x: xAxisArray,
        y: yAxisArray,
        line: { color: "#17BECF" }
      };

      let data = [trace];

      let minValue = 0;
      let maxValue = Math.max(yAxisArray);

      let layout = {
        title: "Price Evolution Time Series",
        width: 750,
        height: 500,
        xaxis: {range: [minValue, maxValue]},
        yaxis: {range: [minValue, maxValue]},
      };

      Plotly.newPlot(this.$refs.timeSeriesChart, data, layout);
    }
  }
};
</script>
