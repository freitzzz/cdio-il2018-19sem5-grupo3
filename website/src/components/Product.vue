<template>
    <!--This will display the contents of the product property in a div-->
    <div>{{product}}</div>
</template>

<script>
import Axios from "axios";
export default {
  props: ["id"],
  data() {
    return {
      product: {},
      errors: []
    };
  },
  methods: {
    getBaseProducts(id) {
      //TODO: replace this with our own api
      Axios.get(`https://swapi.co/api/people/${id}`)
        .then(response => {
          this.product = response.data;
        })
        .catch(error => this.errors.push(error));
    }
  },
  created() {
    this.getBaseProducts(this.id);
  }
};
</script>
