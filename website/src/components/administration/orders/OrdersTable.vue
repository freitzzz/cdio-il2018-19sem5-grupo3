<template>
  <div>
    <vuetable :api-mode="false" :data="data" :fields="columns">
      <template slot="actions" slot-scope="props">
        <div class="custom-actions">
          <button class="btn-primary" @click="enableOrderDetails(props.rowData.id, false)">
            <b-icon icon="magnify"/>
          </button>
          <button class="btn-primary" v-if="displayEditButton(props.rowData)" @click="enableOrderDetails(props.rowData.id, true)">
            <b-icon icon="pencil"/>
          </button>
        </div>
      </template>
    </vuetable>
    <b-modal :active.sync="displayOrderDetails" has-modal-card scroll="keep">
      <order-details :orderId="selectedOrderId" :editable="editable" @updateTableEntry="updateTableEntry"/>
    </b-modal>
  </div>
</template>

<script>
import OrderRequests from "./../../../services/myco_api/requests/orders.js";
import OrderDetails from "./OrderDetails.vue";
export default {
  name: "OrdersTable",
  components: { OrderDetails },
  data() {
    return {
      columns: [
        {
          name: "id",
          title: "Order ID"
        },
        {
          name: "cityToDeliverName",
          title: "Delivery City"
        },
        {
          name: "status",
          title: "Status"
        },
        {
          name: "__slot:actions",
          title: "Actions"
        }
      ],
      selectedOrderId: 0,
      displayOrderDetails: false,
      editable: false
    };
  },
  props: {
    data: Array
  },
  watch: {
    /**
     * Watches for changes in the data prop and updates the table.
     */
    data(newVal) {
      this.data = newVal;
    }
  },
  methods: {
    /**
     * Updates an entry in the orders table with a matching identifier and new data.
     * @param {number} orderId
     * @param {string} newStatus
     */
    updateTableEntry(orderId, newStatus) {
       for (let i = 0; i < this.data.length; i++) {
         if (this.data[i].id == orderId) {
           this.data[i].status = newStatus;
         }
       }
       this.displayOrderDetails = false;
    },

    /**
     * Retrieves a order and toggles the flags that enable the details modal.
     * @param {number} orderId
     * @param {boolean} editable
     */
    enableOrderDetails(orderId, editable) {
       this.selectedOrderId = orderId;
       this.editable = editable;
       this.displayOrderDetails = true;
    },
    displayEditButton(order){
      return order.status === 'Producted'
    }
  }
};
</script>
