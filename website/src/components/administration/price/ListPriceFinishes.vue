<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">List Finishes</p>
      </header>
      <section class="modal-card-body">
        <b-field grouped>
          <b-field>
            <button class="btn-primary" @click="createMaterial()">
              <b-icon icon="plus"/>
            </button>
            <div v-if="createMaterialFinishPriceTableEntryModal">
              <b-modal
                :active="createMaterialFinishPriceTableEntryModal"
                has-modal-card
                scroll="keep"
                :onCancel="confirmClose"
              >
                <create-price-finish
                  :material="this.material"
                  @createMaterialFinishPriceTableEntry="createMaterialFinishPriceTableEntry"
                />
              </b-modal>
            </div>
          </b-field>
          <b-field>
            <button class="btn-primary" @click="fetchRequests()">
              <b-icon icon="refresh"></b-icon>
            </button>
          </b-field>
          <b-field>
            <button class="btn-primary" @click="showPlotTimeSeriesChart">
              <b-icon icon="chart-line-stacked"></b-icon>
            </button>
          </b-field>
          <div v-if="showPlotTimeSeriesChartModal">
            <b-modal :active.sync="showPlotTimeSeriesChartModal" has-modal-card scroll="keep">
              <all-finish-price-histories
                :active="showPlotTimeSeriesChartModal"
                :materialFinishes="materialFinishes"
              ></all-finish-price-histories>
            </b-modal>
          </div>
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
        <b-table :data="data" :paginated="true" :pagination-simple="true" per-page="5">
          <template slot-scope="props">
            <b-table-column field="id" label="ID">{{props.row.id}}</b-table-column>
            <b-table-column field="description" label="Description">{{props.row.description}}</b-table-column>
            <b-table-column field="shininess" label="Shininess">{{props.row.shininess}}</b-table-column>
            <b-table-column field="price" label="Current Price">{{props.row.price}}</b-table-column>
            <b-table-column field="actions" label="Actions">
              <div class="custom-actions">
                <button class="btn-primary" @click="showMaterialFinishPriceHistory(props.row.id)">
                  <b-icon icon="chart-line"></b-icon>
                </button>
                <button
                  class="btn-primary"
                  @click="editMaterialFinishPriceTableEntry(props.row.id)"
                >
                  <b-icon icon="pencil"></b-icon>
                </button>
              </div>
              <div v-if="showEditMaterialFinishPriceTableEntryModal">
                <b-modal :active.sync="showEditMaterialFinishPriceTableEntryModal">
                  <edit-price-finish
                    :materialFinishPrice="currentSelectedFinish"
                    @updateMaterialFinishPriceTableEntry="updateMaterialFinishPriceTableEntry"
                  ></edit-price-finish>
                </b-modal>
              </div>
              <div v-if="showMaterialFinishPriceHistoryModal">
                <b-modal
                  :active.sync="showMaterialFinishPriceHistoryModal"
                  has-modal-card
                  scroll="keep"
                >
                  <finish-price-history :materialFinish="currentSelectedFinish"></finish-price-history>
                </b-modal>
              </div>
            </b-table-column>
          </template>
        </b-table>
      </section>
    </div>
  </div>
</template> 

<script>
import Axios from "axios";
import Config, { MYCM_API_URL } from "../../../config.js";
import CurrenciesPerAreaRequests from "./../../../services/mycm_api/requests/currenciesperarea.js";
import CreatePriceFinish from "./CreatePriceFinish.vue";
import EditPriceFinish from "./EditPriceFinish.vue";
import FinishPriceHistory from "./FinishPriceHistory";
import PriceTableRequests from "./../../../services/mycm_api/requests/pricetables.js";
import AllFinishPriceHistories from "./AllFinishPriceHistories";

export default {
  name: "ListFinishes",

  created() {
    this.fetchRequests();
    CurrenciesPerAreaRequests.getCurrencies()
      .then(response => {
        this.currencies = response.data;
      })
      .catch(error => {
        //throw error?
      });
    CurrenciesPerAreaRequests.getAreas()
      .then(response => {
        this.areas = response.data;
      })
      .catch(error => {
        //throw error?
      });
  },

  data() {
    return {
      createMaterialFinishPriceTableEntryModal: false,
      showEditMaterialFinishPriceTableEntryModal: false,
      showMaterialFinishPriceHistoryModal: false,
      showPlotTimeSeriesChartModal: false,
      currencies: Array,
      areas: Array,
      selectedCurrency: null,
      selectedArea: null,
      currentSelectedFinish: null,
      materialFinishes: null,
      data: []
    };
  },

  components: {
    CreatePriceFinish,
    EditPriceFinish,
    FinishPriceHistory,
    AllFinishPriceHistories
  },

  methods: {
    createMaterial() {
      this.createMaterialFinishPriceTableEntryModal = true;
    },

    fetchRequests() {
      this.refreshTable();
    },

    /**
     * Refreshes Table with finish price information
     */
    async refreshTable() {
      this.data = [];
      let finishes = this.material.finishes;

      for (let i = 0; i < finishes.length; i++) {
        try {
          const {
            data
          } = await PriceTableRequests.getCurrentMaterialFinishPrice(
            this.material.id,
            finishes[i].id,
            "",
            ""
          );
          this.data.push({
            id: data.finish.id,
            description: data.finish.description,
            shininess: data.finish.shininess,
            price:
              data.currentPrice.value +
              " " +
              data.currentPrice.currency +
              "/" +
              data.currentPrice.area
          });
        } catch (error) {
          //Throw error
        }
      }
    },

    /**
     * Shows the details of a material finish price table entry
     */
    async editMaterialFinishPriceTableEntry(finishId) {
      try {
        let { data } = await PriceTableRequests.getCurrentMaterialFinishPrice(
          this.material.id,
          finishId,
          "",
          ""
        );
        for (let i = 0; i < this.data.length; i++) {
          if (this.data[i].id == finishId) {
            this.currentSelectedFinish = {
              materialId: this.material.id,
              tableEntryId: data.tableEntryId,
              finishId: finishId,
              description: this.data[i].description,
              shininess: this.data[i].shininess,
              value: this.data[i].price.split(" ")[0],
              currency: this.data[i].price.split(" ")[1].split("/")[0],
              area: this.data[i].price.split(" ")[1].split("/")[1],
              startingDate: data.timePeriod.startingDate.split("T")[0],
              endingDate: data.timePeriod.endingDate.split("T")[0],
              startingTime: data.timePeriod.startingDate.split("T")[1],
              endingTime: data.timePeriod.endingDate.split("T")[1]
            };
            break;
          }
        }
        this.showEditMaterialFinishPriceTableEntryModal = true;
      } catch (error) {
        this.$toast.open({
          message:
            "An error happened while loading the finish's information, please try again!"
        });
      }
    },

    updateMaterialFinishPriceTableEntry(
      materialId,
      finishId,
      entryId,
      updatedEntry
    ) {
      PriceTableRequests.putMaterialFinishPriceTableEntry(
        materialId,
        finishId,
        entryId,
        updatedEntry
      )
        .then(response => {
          this.$toast.open({
            message: "Update was successful!"
          });
          this.refreshTable();
          this.showEditMaterialFinishPriceTableEntryModal = false;
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    },

    /**
     * Posts a new material finish price table entry
     */
    async createMaterialFinishPriceTableEntry(entries) {
      let errorOccurred = false;
      for (let i = 0; i < entries.length; i++) {
        try {
          await PriceTableRequests.postMaterialFinishPriceTableEntry(
            entries[i].materialId,
            entries[i].finishId,
            entries[i].tableEntry
          );
        } catch (error) {
          errorOccurred = true;
          this.$toast.open(error.response.data.message);
          break;
        }
      }
      if (!errorOccurred) {
        this.$toast.open({
          message: "Prices created succesfully!"
        });
        this.createMaterialFinishPriceTableEntryModal = false;
        this.refreshTable();
      }
    },

    async convertValuesToCurrency() {
      for (let i = 0; i < this.data.length; i++) {
        try {
          let auxArray = this.data[i].price.split(" ");
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
          this.data[i].price =
            convertedPrice.value +
            " " +
            convertedPrice.currency +
            "/" +
            convertedPrice.area;
        } catch (error) {
          //Throw error?
        }
      }
    },

    async convertValuesToArea() {
      for (let i = 0; i < this.data.length; i++) {
        try {
          let auxArray = this.data[i].price.split(" ");
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
          this.data[i].price =
            convertedPrice.value +
            " " +
            convertedPrice.currency +
            "/" +
            convertedPrice.area;
        } catch (error) {
          //Throw error?
        }
      }
    },

    showMaterialFinishPriceHistory(finishId) {
      this.currentSelectedFinish = {
        materialId: this.material.id,
        finishId: finishId
      };
      this.showMaterialFinishPriceHistoryModal = true;
    },

    showPlotTimeSeriesChart() {
      this.materialFinishes = null;
      this.materialFinishes = {
        material: {
          id: this.material.id,
          designation: this.material.designation
        },
        finishes: []
      };
      for (let i = 0; i < this.data.length; i++) {
        this.materialFinishes.finishes.push({
          id: this.data[i].id,
          description: this.data[i].description
        });
      }
      this.showPlotTimeSeriesChartModal = true;
    },

    confirmClose() {
      this.$dialog.confirm({
        title: "Confirm Close",
        message: `Are you sure you want exit?`,
        cancelText: "Cancel",
        confirmText: "OK",
        type: "is-info",
        onConfirm: () =>
          (this.createMaterialFinishPriceTableEntryModal = false),
        onCancel: () => (this.createMaterialFinishPriceTableEntryModal = true)
      });
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
