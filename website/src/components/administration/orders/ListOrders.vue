<template>
  <div>
    <div>
      <button class="btn-primary" @click="getOrders()">
        <b-icon icon="refresh"/>
      </button>
    </div>
    <orders-table :data="data"/>
  </div>
</template>


<script>
import OrdersTable from "./OrdersTable.vue";
import OrderRequests from "./../../../services/myco_api/requests/orders.js";

export default {
  name: "ListOrders",
  data() {
    return {
      modalEnabled: false,
      data: []
    };
  },
  components: {
    OrdersTable
  },
  methods: {
    /**
     * Retrieves all of the available Orders.
     */
    getOrders() {
      OrderRequests.getOrders()
        .then(response => {
          this.data = this.generateOrdersTableData(response.data);
        })
        .catch(error => {
          this.$toast.open(error.response.data.message);
        });
    },
    /**
     * Generates the data of a orders table by a given list of orders
     */
    generateOrdersTableData(orders) {
      let ordersTableData = [];
      orders.forEach(order => {
        ordersTableData.push({
          id: order._id,
          cityToDeliverName: order.cityToDeliver.name,
          status: order.status
        });
      });
      return ordersTableData;
    }
  },
  created() {
    this.getOrders();
  }
};
</script>