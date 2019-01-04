<template>
  <div>
    <div class="modal-card" style="width: auto">
      <header class="modal-card-head">
        <p class="modal-card-title">List Finishes</p>
      </header>
        <section class="modal-card-body">
                <button class="btn-primary" @click="createMaterial()">
                <b-icon icon="plus"/>
                </button>
                <div v-if="createMaterialModal">
                <b-modal :active.sync="createMaterialModal" has-modal-card scroll="keep">
                    <create-price-finish 
                        :active="createMaterialModal"
                        :material="this.material"
                        @createMaterialFinishPriceTableEntry="createMaterialFinishPriceTableEntry"
                    />
                </b-modal>
            </div>
           
          <b-field>
            <b-field label="Currency"> 
              <b-select icon="coin" placeholder="Currency" v-model="selectedCurrency" @input="convertValuesToCurrency">
                <option v-for="currency in this.currencies" 
                :key="currency.currency" 
                :value="currency">
                {{currency.currency}}</option>
              </b-select>
            </b-field>
            <b-field label="Area"> 
              <b-select icon="move-resize-variant" placeholder="Area" v-model="selectedArea" @input="convertValuesToArea">
                <option  v-for="area in this.areas" 
                :key="area.area" 
                :value="area">
                {{area.area}}</option>
              </b-select>
            </b-field>
          </b-field>
          <custom-simple-table
            :columns="simpleTablesColumns.components"
            :actionsButtons="this.buttons"
            :data="this.material.finishes"
            :allowActions="true"
            @emitButtonClick="showEditFinish"
          />
          <div v-if="editFinishModal">
                <b-modal :active.sync="editFinishModal" has-modal-card scroll="keep">
                    <edit-price-finish 
                        :active="editFinishModal"
                        :material="this.material.finishes"
                    />
                </b-modal>
                </div>
        </section>
    </div>
  </div>
</template> 
<script>
import Axios from "axios";
import CustomSimpleTable from './../../UIComponents/CustomSimpleTable';
import Config,{ MYCM_API_URL } from '../../../config.js';
import CurrenciesPerAreaRequests from './../../../services/mycm_api/requests/currenciesperarea.js';
import CreatePriceFinish from './CreatePriceFinish.vue';
import EditPriceFinish from './EditPriceFinish.vue';
import PriceTableRequests from './../../../services/mycm_api/requests/pricetables.js';
export default {
  name: "ListFinishes",
   created(){
       
       this.buttons.push(  
            {
                class:"btn-primary",
                icon:"pencil",
                id:1
            }

       );
        CurrenciesPerAreaRequests.getCurrencies()
            .then((response)=>{
                this.currencies = response.data;
            })
            .catch((error)=>{
                //throw error?
            });
        CurrenciesPerAreaRequests.getAreas()
            .then((response)=>{
                this.areas = response.data;
            })
            .catch((error)=>{
                //throw error?
            });
    },
  data() {
    return {
      buttons: [],
        createMaterialModal:false,
        editFinishModal: false,
      activeFlag: true,
      currencies:Array,
      areas:Array,
      selectedCurrency: null,
      selectedArea: null,
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
                      name: "price",
                      title: "Current Price"
                    },
                  ]
      },
    }
  },
  components:{
        CustomSimpleTable,
        CreatePriceFinish,
        EditPriceFinish
  },
  methods: {
      
      /**
         * Triggers the creation of a new material
         */
        createMaterial(){
            this.createMaterialModal=true;
        },
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
    fetchRequests(){
            this.refreshTable();
        },
        /**
         * Fetches all available materials
         */
        refreshTable(){
            this.data = [];
            MaterialRequests.getMaterials()
                .then((response)=>{
                    this.generateMaterialsTableData(response.data);
                    this.columns=this.generateMaterialsTableColumns();
                    this.total=this.data.length;
                })
                .catch((error_message)=>{
                    //Throw error?
                });
        },   
        /**
         * Shows the details of a material
         */
        async showEditFinish(materialId){
            this.editFinishModal=true;
        },
         /**
         * Posts a new material price table entry
         */
        async createMaterialFinishPriceTableEntry(entries){
            
            let errorOccurred = false;
            for(let i=0; i < entries.length; i++){
                try{
                    await PriceTableRequests.postMaterialFinishPriceTableEntry(entries[i].materialId,entries[i].finishId, entries[i].tableEntry);
                   
                }catch(error){
                    errorOccurred = true;
                    this.$toast.open(error.response.data.message);
                    break;
                }
            }
            if(!errorOccurred){
                this.$toast.open({
                message: "Prices created succesfully!"
                });
                this.createMaterialModal=false;
                this.refreshTable();
            }
        },
        
     async convertValuesToCurrency(){
            for(let i=0; i<this.data.length; i++){
                try{
                    let auxArray = this.data[i].price.split(' ');
                    let value = auxArray[0];
                    auxArray = auxArray[1].split('/');
                    let fromCurrency = auxArray[0];
                    let fromArea = auxArray[1];
                    let toCurrency = this.selectedCurrency.currency;
                    const {data: convertedPrice} = await CurrenciesPerAreaRequests.convertValue(fromCurrency,toCurrency,fromArea,fromArea,value)
                    this.data[i].price = convertedPrice.value + " " + convertedPrice.currency + "/" + convertedPrice.area;
                }catch(error){
                    //Throw error?
                }
            }
        },
        async convertValuesToArea(){
            for(let i=0; i<this.data.length; i++){
                try{
                    let auxArray = this.data[i].price.split(' ');
                    let value = auxArray[0];
                    auxArray = auxArray[1].split('/');
                    let fromCurrency = auxArray[0];
                    let fromArea = auxArray[1];
                    let toArea = this.selectedArea.area;
                    const {data: convertedPrice} = await CurrenciesPerAreaRequests.convertValue(fromCurrency,fromCurrency,fromArea,toArea,value);
                    this.data[i].price = convertedPrice.value + " " + convertedPrice.currency + "/" + convertedPrice.area;
                }catch(error){
                    //Throw error?
                }
            }
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
