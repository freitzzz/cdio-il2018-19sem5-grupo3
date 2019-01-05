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
        <b-table
          :data="data"
          :columns="columns"
          :paginated="true"
          :pagination-simple="true"
          per-page="10"
        >
          <template slot="actions" slot-scope="props">
            <div class="custom-actions">
              <button
                class="btn-primary"
                @click="editMaterialPriceTableEntryDetails(props.rowData)"
              >
                <b-icon icon="pencil"></b-icon>
              </button>
            </div>
            <div v-if="showEditMaterialPriceTableEntryModal">
              <b-modal :active.sync="showEditMaterialPriceTableEntryModal">
                <edit-price-material
                  :material="currentSelectedPriceTableEntry"
                  @emitMaterial="updateMaterial"
                ></edit-price-material>
              </b-modal>
            </div>
          </template>
        </b-table>
      </div>
    </section>
  </div>
</template>

<script>
import EditPriceMaterial from "./EditPriceMaterial";
import PriceTableRequests from "./../../../services/mycm_api/requests/pricetables.js";
import MaterialRequests from "./../../../services/mycm_api/requests/materials.js";
import CurrenciesPerAreaRequests from "./../../../services/mycm_api/requests/currenciesperarea.js";

export default {
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
      defaultSortDirection: "asc",
      /**
       * Materials table columns
       */
      columns: [
        {
          field: "id",
          label: "ID"
        },
        {
          field: "price",
          label: "Price"
        },
        {
          field: "startingDateTime",
          label: "Starting Date & Time",
          labelClass: "center aligned",
          dataClass: "center aligned"
        },
        {
          field: "endingDateTime",
          label: "Ending Date & Time",
          labelClass: "center aligned",
          dataClass: "center aligned"
        },
        {
          field: "__slot:actions",
          label: "Actions",
          labelClass: "center aligned",
          dataClass: "center aligned"
        }
      ],
      data: []
    };
  },

  methods: {
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
          this.$toast.open(error.response.data.message);
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
          this.$toast.open(error.response.data.message);
        }
      }
    },

    fetchRequests() {
      this.refreshTable();
    },

    async refreshTable() {
      this.data = [];

      try {
        const { data } = await PriceTableRequests.getMaterialPriceHistory(
          this.materialId,
          "",
          ""
        );
        let sortEntriesByStartingDateTime = [];
        for (let i = 0; i < data.length; i++) {
          sortEntriesByStartingDateTime.push({
            id: data[i].id,
            price: data[i].value + " " + data[i].currency + "/" + data[i].area,
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
      } catch (error) {
        this.$toast.open(error.response.data.message);
      }
    }
  }
};
</script>
