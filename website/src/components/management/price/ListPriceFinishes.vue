<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">List Finishes</p>
      </header>
        <section class="modal-card-body">
          <small-padding-div>
            <button class="btn-primary">
              <b-icon icon="plus"/>
            </button>
          </small-padding-div>
         <!--  <div v-if="createMaterialModal">
            <b-modal :active.sync="createMaterialModal" has-modal-card scroll="keep">
              <create-price-material 
                :active="createMaterialModal" 
                @emitMaterial="postMaterial"
              />
            </b-modal>
          </div>  -->
          <simple-table
            :columns="simpleTablesColumns.components"
            :allowActions="true"
            :data="this.material.finishes[0].id"
          />
        </section>
       <!--  <template slot="edit" slot-scope="props">
          <div class="custom-actions">
            <button
              class="btn-primary"
              @click="openListPriceFinishes(props.rowData.id)"
            >
            <b-icon icon="pencil"/>
            </button>
          </div>
          <div v-if="showListFinishes">
            <b-modal :active.sync="showListFinishes" has-modal-card scroll="keep">
              <list-price-finishes
                :material="currentSelectedMaterial"
              />
            </b-modal>
          </div>                    
        </template> -->
    </div>
  </div>
</template> 
<script>
import Axios from "axios";
import SimpleTable from './../../UIComponents/SimpleTable';
import Config,{ MYCM_API_URL } from '../../../config.js';
export default {
  name: "List Finishes",
   created(){
    },
  data() {
    return {
      tou:null,
      activeFlag: true,
      currencies:Array,
      areas:Array,
      simpleTablesColumns:{
                components:[
                    {
                        name: "id",
                        title: "ID"
                    },
                     {
                        name: "description",
                        title: "Description"
                    },
                     {
                        name: "shininess",
                        title: "Shininess"
                    },
                     {
                    name: "__slot:edit",
                    title: "Edit",
                    titleClass: "center aligned",
                    dataClass: "center aligned"
                    },
                  ]
      },
    }
  },
  components:{
        SimpleTable
  },
  methods: {
    updateBasicInformation() {
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
