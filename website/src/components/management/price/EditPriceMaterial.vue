<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">Edit Price Material</p>
      </header>
      <section class="modal-card-body">
        <b-field label="Reference">
          <b-input v-model="material.reference" disabled="true" type="String" icon="pound"></b-input>
        </b-field>
        <b-field label="Designation">
          <b-input v-model="material.designation" disabled="true" type="String" icon="pound"></b-input>
        </b-field>
        <b-field label="Edit Price">
           <b-input v-model="material.value" type="String" icon="pound"></b-input>
        </b-field>
        <b-field label="Currency"> 
          <b-select icon="tag" v-model="selectedCurrencie">
            <option  v-for="currency in this.currencies" 
              :key="currency.currency" 
              :value="currency">
              {{currency.currency}}</option>
          </b-select>
        </b-field>
        <b-field label="Area"> 
          <b-select icon="tag" v-model="selectedArea">
            <option  v-for="area in this.areas" 
              :key="area.area" 
              :value="area">
              {{area.area}}</option>
          </b-select>
        </b-field>
      </section>
      <footer class="modal-card-foot">
        <button class="btn-primary" @click="updateBasicInformation()">Edit</button>
      </footer>
    </div>
  </div>
</template> 
<script>
import Axios from "axios";
import Config,{ MYCM_API_URL } from '../../../config.js';
export default {
  name: "EditMaterial",
  created(){
        Axios.get(MYCM_API_URL+`/currenciesperarea/currencies`)
      .then(response => {
        this.currencies = response.data
      })
      .catch((error_message)=>{
        this.$toast.open({message:error_message.response.data.message});
    }); 
    Axios.get(MYCM_API_URL+`/currenciesperarea/areas`)
      .then(response => {
        this.areas = response.data
      })
      .catch((error_message)=>{
        this.$toast.open({message:error_message.response.data.message});
      });  
    },
  data() {
    return {
      activeFlag: true,
      currencies:Array,
      areas:Array
    };
  },
  methods: {
    updateBasicInformation() {
      alert(this.currencies.legth);
     /*  Axios.put(
        `http://localhost:5000/mycm/api/materials/${this.material.id}`,
        {
          reference: this.material.reference,
          designation: this.material.designation,
          image: this.material.image
        }
      )
        .then(this.$toast.open("Update te basic information with success!"))
        .catch(function(error) {});*/
    }, 
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
