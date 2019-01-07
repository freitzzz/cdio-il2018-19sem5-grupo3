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
          this.data = response.data;
        })
        .catch(error => {
          error.response.data.message;
        });
  //     this.data =
  //   [
  //     {
  //         "status": "In Validation",
  //         "_id": "5bd2310ccdf5656525be34aa",
  //         "cityToDeliver":"Porto",
  //         "orderContents": [
  //             {
  //                 "customizedproduct": 1,
  //                 "quantity": 1
  //             },
  //             {
  //                 "customizedproduct": 2,
  //                 "quantity": 1
  //             },
  //             {
  //                 "customizedproduct": 3,
  //                 "quantity": 1
  //             }
  //         ],
  //         "__v": 0
  //     },
  //     {
  //         "status": "In Validation",
  //         "cityToDeliver":"Braga",
  //         "_id": "5bd23123cdf5656525be34ab",
  //         "orderContents": [
  //             {
  //                 "customizedproduct": 1,
  //                 "quantity": 10
  //             },
  //             {
  //                 "customizedproduct": 2,
  //                 "quantity": 10
  //             },
  //             {
  //                 "customizedproduct": 3,
  //                 "quantity": 10
  //             }
  //         ],
  //         "__v": 0
  //     },
  //     {
  //         "status": "In Validation",
  //         "cityToDeliver":"Lisboa",
  //         "_id": "5bd23129cdf5656525be34ac",
  //         "orderContents": [
  //             {
  //                 "customizedproduct": 1,
  //                 "quantity": 100
  //             },
  //             {
  //                 "customizedproduct": 2,
  //                 "quantity": 100
  //             },
  //             {
  //                 "customizedproduct": 3,
  //                 "quantity": 100
  //             }
  //         ],
  //         "__v": 0
  //     }
  //  ];
    },
  },
  created() {
    this.getOrders();
  }
};
</script>
